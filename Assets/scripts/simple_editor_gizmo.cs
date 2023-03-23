using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class simple_editor_gizmo : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnDrawGizmos(){
        Gizmos.color = Color.white;
        Gizmos.DrawWireSphere(transform.position, 1f);
    }
}
