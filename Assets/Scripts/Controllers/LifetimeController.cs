using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LifetimeController : MonoBehaviour
{
    public float timeToLive = 3f;
    private void Start()
    {
        Destroy(gameObject, timeToLive);
    }
}
