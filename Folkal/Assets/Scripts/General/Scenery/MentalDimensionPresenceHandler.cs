using UnityEngine;

public class MentalDimensionPresenceHandler : MonoBehaviour
{
    [SerializeField] private PlayerEvents _playerEvents;
    [Space]
    [SerializeField] private Renderer _renderer;
    [SerializeField] private string _materialPresenceID;

    private MaterialPropertyBlock _propertyBlock;

    private void Awake()
    {
        _propertyBlock = new MaterialPropertyBlock();

        _playerEvents.onEnterMentalDimension += (PlayerManager playerManager)
            => SetMaterialPresence(1, 0);
    }

    private void SetMaterialPresence(float value, float duration)
    {
        _propertyBlock.Clear();
        _propertyBlock.SetFloat(_materialPresenceID, value);
        _renderer.SetPropertyBlock(_propertyBlock);
    }

}
