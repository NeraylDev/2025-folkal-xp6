using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UIElements;

public class MentalDimensionPresenceHandler : MonoBehaviour
{
    public enum MentalDimensionMode
    {
        Refletion,
        Corruption
    }

    [SerializeField] private PlayerEvents _playerEvents;

    [Header("Dimention Elements")]
    [SerializeField] private MentalDimensionElement _refletionElements;
    [SerializeField] private MentalDimensionElement _corruptionElements;
    [Space]
    [SerializeField] private MDPresenceMaterial[] _presenceMaterialList;

    private List<Tweener> _activeTweeners = new List<Tweener>();
    private float _defaultFogDensity;

    private MentalDimensionMode _currentMode;
    private bool _isActive;

    private MaterialPropertyBlock _propertyBlock;
    

    private void Awake()
    {
        _propertyBlock = new MaterialPropertyBlock();

        _playerEvents.onEnterMentalDimension += (playerManager, mode, duration)
            => SetState(true, playerManager, mode, duration);
        _playerEvents.onExitMentalDimension += (playerManager, duration)
            => SetState(false, playerManager, duration);
    }

    private void Start()
    {
        _presenceMaterialList = FindObjectsByType<MDPresenceMaterial>(FindObjectsSortMode.None);
        _defaultFogDensity = RenderSettings.fogDensity;
    }

    private void SetState(bool active, PlayerManager playerManager, MentalDimensionMode mode, float transitionDuration)
    {
        StopAllCoroutines();
        _activeTweeners.ForEach((x) => x.Kill());
        _activeTweeners.Clear();

        if (playerManager == null)
            return;

        if (active)
            Activate(playerManager, mode, transitionDuration);
        else
            Deactivate(playerManager, transitionDuration);
    }

    private void SetState(bool active, PlayerManager playerManager, float transitionDuration)
    {
        SetState(active, playerManager, MentalDimensionMode.Refletion, transitionDuration);
    }

    private void Activate(PlayerManager playerManager, MentalDimensionMode mode, float transitionDuration)
    {
        if (_isActive)
            return;

        _currentMode = mode;

        if (_currentMode == MentalDimensionMode.Refletion)
            _refletionElements.Activate(playerManager.transform.position);
        else
            _corruptionElements.Activate(playerManager.transform.position);

        StartCoroutine(SetMaterialPresence(0, 1, transitionDuration));
        SetFogDensity(0, transitionDuration);
        playerManager.GetPlayerCamera.SetBackgroundColor(new Color32(28, 28, 41, 255), transitionDuration);

        _isActive = true;
    }

    private void Deactivate(PlayerManager playerManager, float transitionDuration)
    {
        if (!_isActive)
            return;

        if (_refletionElements.IsActive)
            _refletionElements.Deactivate();

        if (_corruptionElements.IsActive)
            _corruptionElements.Deactivate();

        StartCoroutine(SetMaterialPresence(1, 0, transitionDuration));
        SetFogDensity(_defaultFogDensity, transitionDuration);
        playerManager.GetPlayerCamera.ResetBackgroundColor(transitionDuration);

        _isActive = false;
    }

    public void ChangeMode(MentalDimensionMode mode)
    {
        if (mode == MentalDimensionMode.Refletion)
        {
            _refletionElements.Activate(_corruptionElements.transform.position);
            _corruptionElements.Deactivate();
        }
        else
        {
            _corruptionElements.Activate(_refletionElements.transform.position);
            _refletionElements.Deactivate();
        }

        _currentMode = mode;
    }

    private void SetFogDensity(float value, float duration)
    {
        Tweener fogDensityTween = DOTween.To
        (
            () => RenderSettings.fogDensity,
            x => RenderSettings.fogDensity = x,
            value,
            duration
        );

        _activeTweeners.Add(fogDensityTween);
    }

    private void SetMaterialPresence(float value)
    {
        if (_presenceMaterialList.Length <= 0)
            return;

        foreach (MDPresenceMaterial material in _presenceMaterialList)
        {
            _propertyBlock.Clear();
            _propertyBlock.SetFloat(material.GetPresenceID, value);
            material.SetPropertyBlock(_propertyBlock);
        }
    }
    
    private IEnumerator SetMaterialPresence(float initialValue, float endValue, float duration)
    {
        float timer = 0;
        do
        {
            SetMaterialPresence(Mathf.Lerp(initialValue, endValue, timer / duration));

            timer += Time.fixedDeltaTime;
            yield return new WaitForFixedUpdate();
        }
        while (timer < duration);
    }

}
 