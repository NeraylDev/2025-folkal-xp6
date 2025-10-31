using UnityEngine;

public class MDPresenceMaterial : MonoBehaviour
{
    [SerializeField] private string _presenceID;
    private Renderer _renderer;
    
    public string GetPresenceID => _presenceID;


    private void Awake()
    {
        if (TryGetComponent(out Renderer renderer))
            _renderer = renderer;
    }

    public void SetPropertyBlock(MaterialPropertyBlock propertyBlock)
        => _renderer.SetPropertyBlock(propertyBlock);

}
