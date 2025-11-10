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

    [Header("Visual")]
    [SerializeField] private Color _mentalDimensionFogColor;

    [Header("Dimention Elements")]
    [SerializeField] private MentalDimensionElement _refletionElements;
    [SerializeField] private MentalDimensionElement _corruptionElements;
    [Space]
    [SerializeField] private Material _terrainMaterial;
    [SerializeField] private List<MDPresenceMaterial> _presenceMaterialList = new List<MDPresenceMaterial>();

    private List<Tweener> _activeTweeners = new List<Tweener>();
    private Color _defaultFogColor;
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
        if (_terrainMaterial != null)
            _terrainMaterial.SetFloat("_NS_Presence", 0);

        InitializeMaterialList(FindObjectsByType<MDPresenceMaterial>(FindObjectsSortMode.None));
        _defaultFogColor = RenderSettings.fogColor;
        _defaultFogDensity = RenderSettings.fogDensity;
    }

    private void OnApplicationQuit()
    {
        if (_terrainMaterial != null)
            _terrainMaterial.SetFloat("_NS_Presence", 0);
    }

    private void InitializeMaterialList(MDPresenceMaterial[] materials)
    {
        if (materials == null)
            return;

        foreach (MDPresenceMaterial material in materials)
            _presenceMaterialList.Add(material);
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
        SetFogColor(_mentalDimensionFogColor, transitionDuration);
        SetFogDensity(0.075f, transitionDuration);
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
        SetFogColor(_defaultFogColor, transitionDuration);
        SetFogDensity(_defaultFogDensity, transitionDuration / 2);
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

    private void SetFogColor(Color color, float duration)
    {
        Tweener fogColorTween = DOTween.To
        (
            () => RenderSettings.fogColor,
            x => RenderSettings.fogColor = x,
            color,
            duration
        );

        _activeTweeners.Add(fogColorTween);
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
        if (_presenceMaterialList.Count <= 0)
            return;

        List<MDPresenceMaterial> materialsToRemove = new List<MDPresenceMaterial>();

        foreach (MDPresenceMaterial material in _presenceMaterialList)
        {
            if (material == null)
            {
                materialsToRemove.Add(material);
                continue;
            }

            _propertyBlock.Clear();
            _propertyBlock.SetFloat(material.GetPresenceID, value);
            material.SetPropertyBlock(_propertyBlock);
        }

        if (materialsToRemove.Count <= 0)
            return;

        foreach (MDPresenceMaterial material in materialsToRemove)
        {
            _presenceMaterialList.Remove(material);
        }
    }
    
    private IEnumerator SetMaterialPresence(float initialValue, float endValue, float duration)
    {
        float timer = 0;
        do
        {
            float presence = Mathf.Lerp(initialValue, endValue, timer / duration);

            SetMaterialPresence(presence);
            if (_terrainMaterial != null)
                _terrainMaterial.SetFloat("_NS_Presence", presence);

            timer += Time.fixedDeltaTime;
            yield return new WaitForFixedUpdate();
        }
        while (timer < duration);
    }

}
 