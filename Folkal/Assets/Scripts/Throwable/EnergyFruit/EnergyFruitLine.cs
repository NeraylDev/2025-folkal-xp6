using UnityEngine;

public class EnergyFruitLine : MonoBehaviour
{
    [Header("Behaviour Settings")]
    [SerializeField] private float _maxSiblingLineLength = 10f;
    [SerializeField] private float _maxTreeLineLength = 10f;

    [Header("Rendering Settings")]
    [SerializeField] private LineRenderer _siblingLine;
    [SerializeField] private LineRenderer _treeLine;
    [SerializeField] private Vector2 _treeLineWidthRange;

    private EnergyFruit _fruit;

    private PlayerHand _playerHand;
    private PlayerMovement _playerMovement;

    public LineRenderer GetTreeLine => _treeLine;
    public LineRenderer GetSiblingLine => _siblingLine;

    private void Awake()
    {
        _fruit = GetComponentInParent<EnergyFruit>();

        if (_treeLine == null || _siblingLine == null)
            Debug.LogWarning("As linhas não podem ser renderizadas." +
                "Os componentes LineRenderer não possuem referência");
    }

    private void Start()
    {
        _playerHand = PlayerHand.instance;
        _playerMovement = PlayerMovement.instance;
    }

    private void Update()
    {
        UpdateTreeLinePosition();
        UpdateSiblingLinePosition();

        CalculeLinesLength();
    }

    private void UpdateTreeLinePosition()
    {
        if (_fruit.GetTree == null)
            return;

        _treeLine.SetPosition(0, transform.position);
        _treeLine.SetPosition(1, (_fruit.GetTree as EnergyTree).GetTreetop.transform.position);
    }

    private void UpdateTreeLineRender(float lineLength)
    {
        float lerpT = Mathf.Clamp01(lineLength / _maxTreeLineLength);

        Color treeLineColor = Color.Lerp
        (
            new Color(1f, 1f, 1f, 1),
            new Color(1f, 1f, 1f, 1),
            lerpT
        );

        float treeLineWidth = Mathf.Lerp(_treeLineWidthRange.y, _treeLineWidthRange.x, lerpT);

        SetColors(_treeLine, treeLineColor, treeLineColor);
        SetWidth(_treeLine, treeLineWidth);
    }

    private void UpdateSiblingLinePosition()
    {
        if (_fruit.GetNextSibling == null)
            return;

        _siblingLine.SetPosition(0, transform.position);
        _siblingLine.SetPosition(1, _fruit.GetNextSibling.transform.position);
    }

    private void UpdateSiblingLineRender(float lineLength)
    {
        float lerpT = Mathf.Clamp01(lineLength / _maxSiblingLineLength);

        Color siblingLineColor = Color.Lerp
        (
            Color.cyan,
            Color.cyan,
            lerpT
        );

        SetColors(_siblingLine, siblingLineColor, siblingLineColor);
    }

    private void CalculeLinesLength()
    {
        // Verifica distância entre esta fruita e as frutas relacionadas
        float distanceToPreviousSibling = Vector3.Distance(transform.position, _fruit.GetPreviousSibling.transform.position);
        float distanceToNextSibling = Vector3.Distance(transform.position, _fruit.GetNextSibling.transform.position);
        float greaterDistanceToSibling = distanceToPreviousSibling > distanceToNextSibling ? distanceToPreviousSibling : distanceToNextSibling;

        // Verifica distância entre esta fruta e a árvore relacionada
        float distanceToTreetop = Vector3.Distance(transform.position, (_fruit.GetTree as EnergyTree).GetTreetop.transform.position);

        if (_playerHand.GetHeldThrowable == _fruit)
        {
            float distanceBasedSpeedModifier = Mathf.Lerp(1, 0.25f, greaterDistanceToSibling / _maxSiblingLineLength);

            Vector3 fruitToTreeDirection = _fruit.GetNextSibling.transform.position - transform.position;
            Vector3 playerMoveDirection = _playerMovement.GetMoveDirection;
            float directionModifier = Mathf.Clamp01(Vector3.Dot(fruitToTreeDirection, playerMoveDirection));

            _playerMovement.SetMoveSpeedModifier(Mathf.Lerp(distanceBasedSpeedModifier, 1, directionModifier));

            float distanceToTree = Vector3.Distance(transform.position, _fruit.GetTree.transform.position);
            VerifyLinesLength(distanceToTree, greaterDistanceToSibling);
        }

        UpdateSiblingLineRender(greaterDistanceToSibling);
        UpdateTreeLineRender(distanceToTreetop);
    }

    private void VerifyLinesLength(float distanceToTree, float distanceToSibling)
    {
        EnergyTree energyTree = _fruit.GetTree as EnergyTree;

        if (distanceToSibling > _maxSiblingLineLength)
        {
            _playerHand.RemoveHeldThrowable();
            _playerMovement.ResetMoveSpeedModifier();
        }
        else if (distanceToTree > _maxTreeLineLength && !energyTree.isAnyFruitActive())
        {
            energyTree.ResetFruitsPosition();
            _playerHand.RemoveHeldThrowable();
            _playerMovement.ResetMoveSpeedModifier();
        }
    }

    private void SetColors(LineRenderer line, Color start, Color end)
    {
        line.startColor = start;
        line.endColor = end;
    }

    private void SetWidth(LineRenderer line, float width)
        => line.widthMultiplier = width;

    private void OnDrawGizmos()
    {
        if (_fruit != null)
        {
            if (_fruit.GetTree != null)
            {
                Gizmos.DrawWireSphere(_fruit.GetTree.transform.position, _maxTreeLineLength);
            }
        }
    }

}
