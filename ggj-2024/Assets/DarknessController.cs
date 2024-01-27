using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DarknessController : MonoBehaviour
{
    [SerializeField]
    private SpriteRenderer spriteRenderer;

    [SerializeField]
    private float transitionTime = 2f;

    [SerializeField]
    private float distMin = 256f;

    [SerializeField]
    private float distMax = 512f;

    [SerializeField]
    private float transitionDirection = -1f;

    [SerializeField]
    private AnimationCurve distCurve = AnimationCurve.Linear(0, 0, 1, 1);

    [SerializeField]
    private AnimationCurve noiseCurve = AnimationCurve.Linear(0, 0, 1, 1);

    [SerializeField]
    private AnimationCurve overallCurve = AnimationCurve.Linear(0, 0, 1, 1);

    private float transition = 0f;

    private int _FadeNoise = 0;
    private int _Dist = 0;
    private int _PixelScaling = 0;
    private int _OverallFade = 0;

    private void Start()
    {
        _FadeNoise = Shader.PropertyToID("_FadeNoise");
        _Dist = Shader.PropertyToID("_Dist");
        _PixelScaling = Shader.PropertyToID("_PixelScaling");
        _OverallFade = Shader.PropertyToID("_OverallFade");
    }

    public void TransitionToDarkness()
    {
        transitionDirection = 1.0f;
    }

    public void TransitionToLight()
    {
        transitionDirection = -1.0f;
    }

    private void Update()
    {
        transition += transitionDirection * Time.deltaTime / transitionTime;
        transition = Mathf.Clamp01(transition);

        var material = spriteRenderer.material;
        material.SetFloat(_OverallFade, overallCurve.Evaluate(transition));
        material.SetFloat(_FadeNoise, noiseCurve.Evaluate(transition));
        material.SetFloat(_Dist, distCurve.Evaluate(transition) * (distMax - distMax) + distMin);
    }
}
