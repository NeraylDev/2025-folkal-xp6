using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public abstract class Speech<Data> : MonoBehaviour
{
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
    }

    private void Update()
    {
        if (_executingSpeech && _currentTimeToClick <= _timeToAllowClick)
            _currentTimeToClick += Time.deltaTime;
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
            if (IsDialogueFinished())
                HideSpeechBox();
            else
                UpdateText();
        }

        _currentTimeToClick = 0;
    }

    protected abstract bool IsDialogueFinished();
    public abstract void StartSpeech(Data data);
    protected abstract void UpdateText(bool instantaneously = false);

    protected virtual void ShowSpeechBox()
    {
        _speechBox.SetActive(true);
        UpdateText();

        _currentTimeToClick = 0;
        _executingSpeech = true;
    }

    protected virtual void HideSpeechBox()
    {
        _speechBox.SetActive(false);
        _executingSpeech = false;
        onFinishSpeech.Invoke();
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
