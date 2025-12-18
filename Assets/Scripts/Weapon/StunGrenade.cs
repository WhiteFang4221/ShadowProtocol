using System;
using System.Collections;
using DG.Tweening;
using UnityEngine;

[RequireComponent(typeof(MeshRenderer))]
public class StunGrenade : MonoBehaviour
{
    private static readonly float BlinkDivider = 2f;
    [SerializeField] private StunArea _stunArea;
    
    [Header("Settings")]
    [SerializeField] private float _activationDelay = 3f;
    [SerializeField] private Color _warningColor = Color.red;
    [SerializeField] private float _initialBlinkDuration = 1f;
    [SerializeField] private float _fastBlinkDuration = 0.1f;
    [SerializeField] private float _fastBlinkStartTime = 2f;

    private Sequence _blinkTweener;
    private MeshRenderer _renderer;
    private Material _originalMaterial;
    private Color _originalColor;
    private float _currentBlinkDuration;
    private float _startTime;
    private bool _isFastBlinking = false;

    private void Awake()
    {
        _renderer = GetComponent<MeshRenderer>();
    }

    private void Start()
    {
        if (_renderer != null)
        {
            _originalMaterial = _renderer.material;
            _originalColor = _originalMaterial.color;
        }
        else
        {
            Debug.LogError("StunGrenade: MeshRenderer не найден!", this);
            enabled = false;
            return;
        }

        _startTime = Time.time;
        _currentBlinkDuration = _initialBlinkDuration;
        _isFastBlinking = false;

        StartBlinkAnimation();
    }

    private void Update()
    {
        float currentTime = Time.time;
        float elapsedTime = currentTime - _startTime;
        float timeLeft = _activationDelay - elapsedTime;

        if (elapsedTime >= _activationDelay)
        {
            Activate();
            return;
        }

        if (!_isFastBlinking && timeLeft <= _fastBlinkStartTime)
        {
            _isFastBlinking = true;
            _currentBlinkDuration = _fastBlinkDuration;
            RestartBlinkTween();
        }
    }

    private void StartBlinkAnimation()
    {
        RestartBlinkTween();
    }

    private void RestartBlinkTween()
    {
        if (_blinkTweener != null)
        {
            _blinkTweener.Kill();
        }

        _blinkTweener = DOTween.Sequence()
            .SetLoops(-1, LoopType.Restart)
            .Append(_originalMaterial.DOColor(_warningColor, _currentBlinkDuration / BlinkDivider))
            .Append(_originalMaterial.DOColor(_originalColor, _currentBlinkDuration / BlinkDivider));
    }

    private void Activate()
    {
        if (_blinkTweener != null)
        {
            _blinkTweener.Kill();
            _blinkTweener = null;
        }

        if (_originalMaterial != null)
        {
            _originalMaterial.color = _warningColor;
        }

        _stunArea.Explode();
        Destroy(gameObject);
    }

}