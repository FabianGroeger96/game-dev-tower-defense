using UnityEditor;
using UnityEngine;
using Quaternion = UnityEngine.Quaternion;
using Vector3 = UnityEngine.Vector3;


public class Tower : MonoBehaviour
{
    [SerializeField] 
    public int exp;
    private ProjectileSpawner _spawner;
    private TargetFinder _targetFinder;
    private Transform _rotation;
    private MeshRenderer _renderer;
    private bool _placed = false;
    // Start is called before the first frame update
    
    void Start()
    {
        _spawner = GetComponentInChildren<ProjectileSpawner>();
        _targetFinder = GetComponentInChildren<TargetFinder>();
        _rotation = GetComponentInChildren<Transform>();
        _renderer = GetComponentInChildren<MeshRenderer>();
        transform.gameObject.layer = LayerMask.NameToLayer("PlaceMode");
        foreach (Transform child in transform)
        {
            child.gameObject.layer = LayerMask.NameToLayer("PlaceMode");
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (_targetFinder.target != null)
        {
            RotateToTarget();
            if (Input.GetKeyDown(KeyCode.Space))
            {
                _spawner.launch(this, _targetFinder.target.transform, _rotation.rotation);
            }
        }
        

    }

    private void RotateToTarget()
    {
        Vector3 direction = _targetFinder.target.transform.position - transform.position;
        Quaternion lookRotation = Quaternion.LookRotation(direction);
        Vector3 rotation = Quaternion.Lerp(_rotation.rotation, lookRotation, Time.deltaTime * 10).eulerAngles;
        _rotation.rotation = Quaternion.Euler(0f, rotation.y, 0f);

    }

    public void HandleHit(int obj)
    {
        exp += obj;
    }

    public void ChangeMaterial(Material material)
    {
        _renderer.material = material;
    }

    public void placed()
    {
        transform.gameObject.layer = LayerMask.NameToLayer("Tower");
        foreach (Transform child in transform)
        {
            child.gameObject.layer = LayerMask.NameToLayer("Tower");
        }
        _placed = true;
    }

}
