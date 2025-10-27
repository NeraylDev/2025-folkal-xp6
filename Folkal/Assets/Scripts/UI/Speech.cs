using System;
using System.Collections;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public abstract class Speech<Data> : MonoBehaviour
{
    [SerializeField] protected UIEvents _uiEvents;

    [Header("Speech Settings")]
    [SerializeField] protected GameObject _speechBox;
    [SerializeField] protected TMP_Text _speechText;
    [SerializeField] protected float _timePerLetter = 0.5f;
    protected int lineIndex;
    private bool _executingSpeech;

    private float _timeToAllowClick = 0.1f;
    private float _currentTimeToClick;
    private bool _typingText;

    [HideInInspector] public UnityEvent onStartSpeech;
    [HideInInspector] public UnityEvent onFinishSpeech;

    public bool IsExecutingSpeech => _executingSpeech;
    protected GameObject GetSpeechBox => _speechBox;
    protected TMP_Text GetSpeechText => _speechText;

    protected virtual void Awake()
    {
        onStartSpeech.AddListener(ShowSpeechBox);
        SetEvents();
    }

    private void Update()
    {
        if (_executingSpeech && _currentTimeToClick <= _timeToAllowClick)
            _currentTimeToClick += Time.deltaTime;
    }

    public void SetEvents()
    {
        InputActionAsset inputAsset = InputSystem.actions;
        if (inputAsset == null)
            return;

        inputAsset.FindAction("Interact").canceled += (InputAction.CallbackContext context)
            => TryUpdateSpeech();
    }

    public void TryUpdateSpeech()
    {
        if (!_executingSpeech || _currentTimeToClick < _timeToAllowClick)
            return;

        if (_typingText)
        {
            StopAllCoroutines();
            _typingText = false;

            UpdateText(true);
        }
        else
        {
            lineIndex++;
            if (IsSpeechFinished())
                HideSpeechBox();
            else
                UpdateText();
        }

        _currentTimeToClick = 0;
    }

    protected abstract bool IsSpeechFinished();
    public abstract void StartSpeech(Data data);
    protected abstract void UpdateText(bool instantaneously = false);

    protected virtual void ShowSpeechBox()
    {
        _speechBox.SetActive(true);
        UpdateText();

        _currentTimeToClick = 0;
        _executingSpeech = true;

        if (_uiEvents == null)
            return;

        _uiEvents.RaiseSpeechStart();
    }

    protected virtual void HideSpeechBox()
    {
        onFinishSpeech.Invoke();
        _executingSpeech = false;
        _speechBox.SetActive(false);

        if (_uiEvents == null)
            return;

        _uiEvents.RaiseSpeechEnd();
    }

    protected IEnumerator TypeText(string text)
    {
        _typingText = true;

        foreach (char letter in text)
        {
            _speechText.text += letter;
            yield return new WaitForSeconds(_timePerLetter);
        }

        _typingText = false;
    }
}
