using UnityEngine;

public abstract class Fruit : Throwable
{
    protected FruitTree _tree;

    public FruitTree GetTree => _tree;

    protected override void Awake()
    {
        base.Awake();

        if (transform.parent != null && transform.parent.TryGetComponent(out FruitTree rootTree))
        {
            SetTree(rootTree);
        }
    }

    public void SetTree(FruitTree tree) => _tree = tree;

    public void RemoveFromTree()
    {
        if (_tree == null)
            return;

        _tree.RemoveFruit(this);
        _tree = null;
    }

    public override void OnCollide() { }

}
