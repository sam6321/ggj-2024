using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public static PlayerMovement Instance;

    public class Blocker {
        public Object key;
        public bool isInteractionBlocker;
        public bool isMovementBlocker;
    }

    public enum WalkSound
    {
        Grass,
        GrassWet,
        Wood
    }

    [SerializeField]
    private float sensitivity = 1f;

    [SerializeField]
    private float walkSoundDelay = 0.25f;

    [SerializeField]
    private Rigidbody2D rb;

    [SerializeField]
    private new Collider2D collider;

    [SerializeField]
    private Animator animator;

    [SerializeField]
    private AudioSource audioSource;

    [SerializeField]
    private AudioClip[] walkSoundMap = new AudioClip[0];

    [SerializeField]
    private WalkSound walkSound = WalkSound.Grass;

    private Vector2 facingDirection = new(0, 1);
    private int facingDirectionIndex = 0;
    private float lastWalkSoundTime = 0f;
    private Vector3 lastFramePos = new(0f, 0f, 0f);

    [SerializeField]
    private List<Blocker> blockers = new();

    private HashSet<Collider2D> walkTriggers = new();

    private readonly Vector2[] facingDirections = new Vector2[]
    {
        new Vector2(0, -1),
        new Vector2(0, 1),
        new Vector2(-1, 0),
        new Vector2(1, 0),
    };

    private readonly KeyCode[] facingKeys = new KeyCode[]
    {
        KeyCode.S,
        KeyCode.W,
        KeyCode.A,
        KeyCode.D,
    };

    public void AddBlocker(Blocker blocker)
    {
        var existing = blockers.Find(b => b.key == blocker.key);
        if(existing != null)
        {
            Debug.LogError("Blocker with key " + blocker.key.ToString() + " already exists!");
        }
        else
        {
            blockers.Add(blocker);
        }
    }

    public void RemoveBlocker(Object key)
    {
        var existing = blockers.FindIndex(b => b.key == key);
        if (existing < 0)
        {
            Debug.LogError("Blocker with key " + key.ToString() + " does not exist!");
        }
        else
        {
            blockers.RemoveAt(existing);
        }
    }

    public bool IsInteractionBlocked()
    {
        return blockers.Any(b => b.isInteractionBlocker);
    }

    public bool IsMovementBlocked()
    {
        return blockers.Any(b => b.isMovementBlocker);
    }

    void Start()
    {
        Instance = this;
    }

    void Update()
    {
        Move();
        Interact();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("WalkSoundTrigger"))
        {
            walkTriggers.Add(collision);
            UpdateWalkSound();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("WalkSoundTrigger"))
        {
            walkTriggers.Remove(collision);
            UpdateWalkSound();
        }
    }

    private void UpdateWalkSound()
    {
        // there should only really be one of these but oh well just pick  the last one
        walkSound = WalkSound.Grass;

        foreach (Collider2D collider in walkTriggers)
        {
            walkSound = collider.gameObject.GetComponent<WalkSoundTrigger>().Sound;
        }
    }

    private void Move()
    {
        if(IsMovementBlocked())
        {
            rb.velocity = Vector2.zero;
            return;
        }

        // Check if we're actually moving right now.
        // we might be stuck against something and so should not play movement
        // sounds
        if (Vector2.Distance(transform.position, lastFramePos) > 1f && 
            Time.time > lastWalkSoundTime + walkSoundDelay)
        {
            lastWalkSoundTime = Time.time;
            AudioClip clip = walkSoundMap[(int)walkSound];

            audioSource.pitch = Random.Range(0.95f, 1.05f);
            audioSource.PlayOneShot(clip);
        }

        lastFramePos = transform.position;

        Vector3 movement = new(0f, 0f, 0f);

        movement.x -= (Input.GetKey(KeyCode.A) ? 1f : 0f);
        movement.x += (Input.GetKey(KeyCode.D) ? 1f : 0f);
        movement.y -= (Input.GetKey(KeyCode.S) ? 1f : 0f);
        movement.y += (Input.GetKey(KeyCode.W) ? 1f : 0f);

        movement.Normalize();

        rb.velocity = /*sensitivity * Time.deltaTime * */movement * sensitivity;

        // determine facing direction from movement or last key press
        if (movement.sqrMagnitude > 0)
        {
            facingDirectionIndex = FacingDirecitonFromMovement(movement);
        }
        else
        {
            int maybeNewFacingDirection = FacingDirectionFromKey();
            if (maybeNewFacingDirection >= 0)
            {
                facingDirectionIndex = maybeNewFacingDirection;
            }
        }

        facingDirection = facingDirections[facingDirectionIndex];
        animator.SetInteger("direction", facingDirectionIndex);
    }

    private int FacingDirecitonFromMovement(Vector2 movement)
    {
        float bestDot = -1;
        int bestIndex = 0;
        for (int i = 0; i < facingDirections.Length; i++)
        {
            float dot = Vector2.Dot(movement, facingDirections[i]);
            if (dot > bestDot)
            {
                bestDot = dot;
                bestIndex = i;
            }
        }
        return bestIndex;
    }

    private int FacingDirectionFromKey()
    {
        for (int i = 0; i < facingKeys.Length; i++)
        {
            if (Input.GetKeyDown(facingKeys[i]))
            {
                return i;
            }
        }
        return -1; // leave unchanged
    }

    private void Interact()
    {
        if(IsInteractionBlocked())
        {
            return;
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            // Box sweep a unit in facing direction and collect everything we hit and then check for interaction receivers
            RaycastHit2D[] hits = new RaycastHit2D[1];
            if (Physics2D.BoxCast(transform.position, collider.bounds.extents, 0f, facingDirection, new()
            {
                layerMask = LayerMask.GetMask("Interactable"),
                useTriggers = true,
                useLayerMask = true,
            }, hits, collider.bounds.size.magnitude) != 0)
            {
                foreach (RaycastHit2D hit in hits)
                {
                    if (hit.collider)
                    {
                        Interactable interactable = hit.collider.GetComponent<Interactable>();
                        if (interactable)
                        {
                            interactable.OnInteract(gameObject);
                        }
                    }
                }
            }
        }
    }
}
