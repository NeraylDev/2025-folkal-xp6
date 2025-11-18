using UnityEngine;

public abstract class Fruit : Throwable
{
    protected FruitTree _tree;

    public FruitTree GetTree => _tree;

    protected override void Awake()
    {
        base.Awake();

        FruitTree tree = GetComponentInParent<FruitTree>();
        if (transform.parent != null)
        {
            SetTree(tree);
        }
    }

    public void SetTree(FruitTree tree)
    {
        if (tree == null)
            return;

        tree.AddFruit(this);
        _tree = tree;
    }

    public void RemoveFromTree()
    {
        if (_tree == null)
            return;

        _tree.RemoveFruit(this);
        _tree = null;
    }

    protected override void OnCollide() { }

}
