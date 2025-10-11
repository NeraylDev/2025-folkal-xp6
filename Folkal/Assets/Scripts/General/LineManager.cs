using UnityEngine;

public abstract class LineManager : MonoBehaviour
{
    [Header("Line Manager Settings")]
    [SerializeField] protected LineRenderer _lineRenderer;
    [SerializeField] protected float _maxLineLength;

    protected virtual void UpdateLinePosition(Vector3 a, Vector3 b)
    {
        _lineRenderer.SetPosition(0, a);
        _lineRenderer.SetPosition(_lineRenderer.positionCount - 1, b);

        CalculateLineLength();
    }

    protected abstract void CalculateLineLength();
    protected abstract void OnReachMaxLength();

    protected void SetWidthMultiplier(float multiplier)
        => _lineRenderer.widthMultiplier = multiplier;

    protected void SetColor(Color color)
    {
        _lineRenderer.startColor = color;
        _lineRenderer.endColor = color;
    }

    protected void SetColorGradient(Color a, Color b)
    {
        _lineRenderer.startColor = a;
        _lineRenderer.endColor = b;
    }

}
