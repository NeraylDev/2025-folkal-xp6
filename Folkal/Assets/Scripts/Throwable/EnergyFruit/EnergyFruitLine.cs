using UnityEngine;

public class EnergyFruitLine : MonoBehaviour
{
    [Header("Behaviour Settings")]
    [SerializeField] private float _maxRootLineLength = 10f;
    [SerializeField] private float _maxSiblingLineLength = 10f;

    [Header("Rendering Settings")]
    [SerializeField] private LineRenderer _rootLine;
    [SerializeField] private LineRenderer _siblingLine;

    private EnergyFruit _fruit;
    private PlayerHand _playerHand;

    public LineRenderer GetRootLine => _rootLine;
    public LineRenderer GetSiblingLine => _siblingLine;

    private void Awake()
    {
        _fruit = GetComponentInParent<EnergyFruit>();

        if (_rootLine == null || _siblingLine == null)
            Debug.LogWarning("As linhas não podem ser renderizadas." +
                "Os componentes LineRenderer não possuem referência");
    }

    private void Start()
    {
        _playerHand = PlayerHand.instance;
    }

    private void Update()
    {
        UpdateRootLine();
        UpdateSiblingLine();

        VerifyLinesLength();
    }


    private void UpdateRootLine()
    {
        if (_fruit.GetRoot == null)
            return;

        _rootLine.SetPosition(0, transform.position);
        _rootLine.SetPosition(1, _fruit.GetRoot.transform.position);
    }

    private void UpdateSiblingLine()
    {
        if (_fruit.GetNextSibling == null)
            return;

        _siblingLine.SetPosition(0, transform.position);
        _siblingLine.SetPosition(1, _fruit.GetNextSibling.transform.position);
    }

    private void VerifyLinesLength()
    {
        float distanceToPreviousSibling = Vector3.Distance(transform.position, _fruit.GetPreviousSibling.transform.position);
        float distanceToNextSibling = Vector3.Distance(transform.position, _fruit.GetNextSibling.transform.position);
        float greaterDistanceToSibling = 0;

        greaterDistanceToSibling = distanceToPreviousSibling > distanceToNextSibling ? distanceToPreviousSibling : distanceToNextSibling;
        Color siblingLineColor = Color.Lerp(Color.black, Color.cyan, distanceToNextSibling / _maxSiblingLineLength);
        SetColors(_siblingLine, siblingLineColor, siblingLineColor);


        float distanceToRoot = Vector3.Distance(transform.position, _fruit.GetRoot.transform.position);
        Color rootLineColor = Color.Lerp(new Color(0, 0.5f, 0.25f, .1f), new Color(0, 0.1f, 1f, 1), distanceToRoot / _maxRootLineLength);
        SetColors(_rootLine, rootLineColor, rootLineColor);


        if (_playerHand.GetHeldThrowable == _fruit)
        {
            EnergyTree energyTree = _fruit.GetRoot as EnergyTree;

            if (greaterDistanceToSibling > _maxSiblingLineLength)
            {
                _playerHand.RemoveHeldThrowable();
            }
            else if (distanceToRoot > _maxRootLineLength && !energyTree.isAnyFruitActive())
            {
                energyTree.ResetFruitsPosition();
                _playerHand.RemoveHeldThrowable();
            }
        }
    }

    private void SetColors(LineRenderer line, Color start, Color end)
    {
        line.startColor = start;
        line.endColor = end;
    }

    private void OnDrawGizmos()
    {
        
        if (_fruit != null)
        {
            if (_fruit.GetRoot != null)
            {
                Gizmos.DrawWireSphere(_fruit.GetRoot.transform.position, _maxRootLineLength);
            }
        }

    }

}
