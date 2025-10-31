using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UIElements;

public class MentalDimensionPresenceHandler : MonoBehaviour
{
    [SerializeField] private PlayerEvents _playerEvents;
    [Space]
    [SerializeField] private GameObject _particles;
    [SerializeField] private MDPresenceMaterial[] _presenceMaterialList;

    private List<Tweener> _activeTweeners = new List<Tweener>();
    private float _defaultFogDensity;
    private bool _isActive;

    private MaterialPropertyBlock _propertyBlock;

    private void Awake()
    {
        _propertyBlock = new MaterialPropertyBlock();

        _playerEvents.onEnterMentalDimension += (PlayerManager playerManager)
            => SetState(true, playerManager);
        _playerEvents.onExitMentalDimension += (PlayerManager playerManager)
            => SetState(false, playerManager);
    }

    private void Start()
    {
        _presenceMaterialList = FindObjectsByType<MDPresenceMaterial>(FindObjectsSortMode.None);
        _defaultFogDensity = RenderSettings.fogDensity;
    }

    private void SetState(bool active, PlayerManager playerManager)
    {
        StopAllCoroutines();
        _activeTweeners.ForEach((x) => x.Kill());
        _activeTweeners.Clear();

        if (playerManager == null)
            return;

        if (active)
            Activate(playerManager);
        else
            Deactivate(playerManager);
    }

    private void Activate(PlayerManager playerManager)
    {
        if (_isActive)
            return;

        float transitionDuration = playerManager.GetPlayerBreathing.GetBreathingOutDuration;

        _particles.SetActive(true);
        _particles.transform.position = playerManager.transform.position;
        StartCoroutine(SetMaterialPresence(0, 1, transitionDuration));
        SetFogDensity(0, transitionDuration);
        playerManager.GetPlayerCamera.SetBackgroundColor(new Color32(28, 28, 41, 255), transitionDuration);

        _isActive = true;
    }

    private void Deactivate(PlayerManager playerManager)
    {
        if (!_isActive)
            return;

        float transitionDuration = 1f;

        _particles.SetActive(false);
        StartCoroutine(SetMaterialPresence(1, 0, transitionDuration));
        SetFogDensity(_defaultFogDensity, transitionDuration);
        playerManager.GetPlayerCamera.ResetBackgroundColor(transitionDuration);

        _isActive = false;
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
 