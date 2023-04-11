using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* Attach this script to any object that should be visible in editor upon selection */
public class simple_editor_gizmo : MonoBehaviour
{
    public Color gizmoColor = Color.white;
    public float gizmoRadius = 1f;
    void OnDrawGizmos(){
        Gizmos.color = gizmoColor;
        Gizmos.DrawWireSphere(transform.position, gizmoRadius);
    }
}
