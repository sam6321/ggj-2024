using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public class Blocker { 
        public Object key;
        public bool isInteractionBlocker;
        public bool isMovementBlocker;
    }

    [SerializeField]
    private float sensitivity = 1f;

    [SerializeField]
    private Rigidbody2D rb;

    [SerializeField]
    private Transform facingDirectionMarker;

    private Vector2 facingDirection = new(0, 1);

    [SerializeField]
    private List<Blocker> blockers = new();

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

    public void RemoveInteractionBlocker(Object key)
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

    private bool IsInteractionBlocked()
    {
        return blockers.Any(b => b.isInteractionBlocker);
    }

    private bool IsMovementBlocked()
    {
        return blockers.Any(b => b.isMovementBlocker);
    }

    // Update is called once per frame
    void Update()
    {
        Move();
        Interact();
    }

    private void Move()
    {
        if(IsMovementBlocked())
        {
            return;
        }

        Vector3 movement = new(0f, 0f, 0f);

        movement.x -= (Input.GetKey(KeyCode.A) ? 1f : 0f);
        movement.x += (Input.GetKey(KeyCode.D) ? 1f : 0f);
        movement.y -= (Input.GetKey(KeyCode.S) ? 1f : 0f);
        movement.y += (Input.GetKey(KeyCode.W) ? 1f : 0f);

        movement.Normalize();

        rb.velocity = /*sensitivity * Time.deltaTime * */movement * sensitivity;

        // determine facing direction from last pressed key
        if (Input.GetKeyDown(KeyCode.A))
        {
            facingDirection = new Vector2(-1, 0);
        }
        else if (Input.GetKeyDown(KeyCode.D))
        {
            facingDirection = new Vector2(1, 0);
        }
        else if (Input.GetKeyDown(KeyCode.S))
        {
            facingDirection = new Vector2(0, -1);
        }
        else if (Input.GetKeyDown(KeyCode.W))
        {
            facingDirection = new Vector2(0, 1);
        }

        if (facingDirectionMarker)
        {
            facingDirectionMarker.position = transform.position + (Vector3)facingDirection;
        }
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
            if (Physics2D.BoxCast(transform.position, new Vector2(1f, 1f), 0f, facingDirection, new()
            {
                layerMask = LayerMask.GetMask("Interactable"),
                useTriggers = true,
                useLayerMask = true,
            }, hits, 1.5f) != 0)
            {
                Debug.Log("Hit something!");
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
