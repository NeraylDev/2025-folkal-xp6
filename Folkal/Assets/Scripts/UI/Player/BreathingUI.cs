using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class BreathingUI : MonoBehaviour
{
    [SerializeField] private PlayerEvents _playerEvents;
    [Space]
    [SerializeField] private RectTransform _breathingBorderTransform;
    [SerializeField] private RectTransform _breathingCircleTransform;
    [SerializeField] private Image[] _breathingUIImages;
    private List<Tween> _activeTweeners = new List<Tween>();

    private void Awake()
    {
        _breathingBorderTransform.localScale = Vector2.zero;
        _breathingCircleTransform.localScale = Vector2.zero;
        SetAlpha(0, 0);

        _playerEvents.onBreathingStart += (PlayerManager playerManager)
            => ShowUI(playerManager);
        _playerEvents.onBreathingStop += (PlayerManager playerManager)
            => HideUI(playerManager);
        _playerEvents.onBreathingCanceled += (PlayerManager playerManager)
            => ForceHideUI();
    }

    private void ShowUI(PlayerManager playerManager)
    {
        ResetTweenersList();

        _breathingBorderTransform.localScale = Vector2.zero;
        _breathingCircleTransform.localScale = Vector2.zero;
        SetAlpha(0, 0);

        float breathingInDuration = playerManager.GetPlayerBreathing.GetBreathingInDuration;
        SetScale(_breathingBorderTransform, 1, breathingInDuration / 5);
        SetScale(_breathingCircleTransform, 1, breathingInDuration);
        SetAlpha(1, breathingInDuration);
    }

    private void HideUI(PlayerManager playerManager)
    {
        ResetTweenersList();

        float breathingOutDuration = playerManager.GetPlayerBreathing.GetBreathingOutDuration;
        SetScale(_breathingBorderTransform, 0.2f, breathingOutDuration);
        SetScale(_breathingCircleTransform, 0.2f, breathingOutDuration);
        SetAlpha(0, breathingOutDuration);
    }

    private void ForceHideUI()
    {
        ResetTweenersList();

        SetScale(_breathingBorderTransform, 0.2f, 0.5f);
        SetScale(_breathingCircleTransform, 0.2f, 0.25f);
        SetAlpha(0, 0.25f);
    }

    private void ResetTweenersList()
    {
        _activeTweeners.ForEach((x) => x.Kill());
        _activeTweeners.Clear();
    }

    private void SetScale(RectTransform rectTransform, float size, float duration)
    {
        _activeTweeners.Add(rectTransform.DOScale(Vector2.one * size, duration));
    }

    private void SetAlpha(float value, float duration)
    {
        if (_breathingUIImages.Length <= 0)
            return;

        foreach (Image image in _breathingUIImages)
        {
            _activeTweeners.Add(image.DOFade(value, duration));
        }
    }

}
