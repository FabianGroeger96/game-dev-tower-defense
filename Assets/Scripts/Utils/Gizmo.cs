using UnityEngine;

/// <summary>
/// Represents the gizmo of an object.
/// </summary>
public class Gizmo : MonoBehaviour
{
    // defines the gizmo size
    public float gizomoSize = .75f;

    // defines the color of the gizmo
    public Color gizmoColor = Color.yellow;

    /// <summary>
    /// Draws the gizmo in the shape of a sphere.
    /// </summary>
    void OnDrawGizmos()
    {
        Gizmos.color = gizmoColor;
        Gizmos.DrawWireSphere(transform.position, gizomoSize);
    }
}