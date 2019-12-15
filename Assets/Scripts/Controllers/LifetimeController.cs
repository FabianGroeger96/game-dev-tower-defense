using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LifetimeController : MonoBehaviour
{
    public float TimeToLive = 3f;
    private void Start()
    {
        Destroy(gameObject, TimeToLive);
    }
}
