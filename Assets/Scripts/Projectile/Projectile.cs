using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public class Projectile : MonoBehaviour {
    
    // Start is called before the first frame update
    
    private Transform _target;
    public float speed;
    public event Action<int> OnHit = delegate { };
     
    
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (_target == null)
        {
            Destroy(gameObject);
            return;
        }

        Vector3 direction = _target.position - transform.position;
        direction.y = 0;
        float travelDinstanceInThisFrame = speed * Time.deltaTime;

        if (direction.magnitude <= travelDinstanceInThisFrame)
        {
            Destroy(gameObject);
            return;
        }
        
        transform.Translate(direction.normalized * travelDinstanceInThisFrame, Space.World);

    }

    public void Seek(Transform target)
    {
        _target = target;
    }
    
    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Ground"))
        {
            return;
        }
        StartCoroutine(TestCoroutine());
        OnHit(1);

    }
    
    IEnumerator TestCoroutine()
    {
        yield return new WaitForSeconds(3);
        Destroy(gameObject);
    }
    
}
