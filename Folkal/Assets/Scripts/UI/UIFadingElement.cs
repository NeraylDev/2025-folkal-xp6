using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class UIFadingElement : MonoBehaviour
{
    private UIEvents _uiEvents;

    private Image _blackImage;
    private Tweener _fadingTweener;

    private void Awake()
    {
        _blackImage = GetComponent<Image>();
    }

    private void Start()
    {
        _blackImage.color = new Color(_blackImage.color.r, _blackImage.color.g, _blackImage.color.b, 1);
    }

    public void Initialize(UIManager uiManager)
    {
        _uiEvents = uiManager.GetEvents;

        _uiEvents.onFadeInToBlack += FadeIn;
        _uiEvents.onFadeOutToBlack += FadeOut;
    }

    public void FadeIn(float duration = 1f)
    {
        if (_fadingTweener != null)
            _fadingTweener.Kill();

        _fadingTweener = _blackImage.DOFade(1, duration).OnComplete(_uiEvents.RaiseFinishFadeInToBlack);
    }

    public void FadeOut(float duration = 1f)
    {
        if (_fadingTweener != null)
            _fadingTweener.Kill();

        _fadingTweener = _blackImage.DOFade(0, duration).OnComplete(_uiEvents.RaiseFinishFadeOutToBlack);
    }
}
