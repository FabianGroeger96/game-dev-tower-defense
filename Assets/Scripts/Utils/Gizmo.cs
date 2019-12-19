using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gizmo : MonoBehaviour
{

    public float gizomoSize = .75f;
    public Color gizmoColor = Color.yellow;
    // Start is called before the first frame update
    void OnDrawGizmos()
    {
        Gizmos.color = gizmoColor;
        Gizmos.DrawWireSphere(transform.position, gizomoSize);
    }
}
