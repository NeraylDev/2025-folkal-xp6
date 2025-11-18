using UnityEngine;

public class BombTree : FruitTree
{
    [SerializeField] private GameObject _bombPrefab;
    [SerializeField] private Transform _bombSpawnpoint;

    private void Awake()
    {
        onRemoveFruit += RespawnBomb;
    }

    private void RespawnBomb(Fruit fruit)
    {
        GameObject bomb = Instantiate(_bombPrefab, transform);
        bomb.transform.position = _bombSpawnpoint.position;
    }

}
