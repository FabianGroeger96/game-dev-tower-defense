using UnityEngine;

/// <summary>
/// Represents the current enemies sight.
/// </summary>
public class CurrentEnemySight : MonoBehaviour
{
    private LineRenderer _line;
    private Transform _startPoint;
    private Transform _endPoint;
    private TargetFinder _targetFinder;
    
    // Start is called before the first frame update
    void Start()
    {
        _line = GetComponentInChildren<LineRenderer>();
        _targetFinder = GetComponentInChildren<TargetFinder>();
        _line.startWidth = 0.2f;
        _line.endWidth = 0.2f;
        _startPoint = GetComponentInChildren<ProjectileSpawner>().transform;
        
    }

    // Update is called once per frame
    void Update()
    {
        GameObject target = _targetFinder.target;
        if (target == null)
        {
            _endPoint = _startPoint;
        }
        else
        {
            _endPoint = target.transform;
        }
        _line.SetPosition(0, _startPoint.position);
        _line.SetPosition(1, _endPoint.position);
    }
}
