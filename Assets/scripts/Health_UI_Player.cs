using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* PRE-Requisites. Player must be instantiated in scene
TODOS: Make it so that player prefab comes with the health UI prefab attached to it */

public class Health_UI_Player : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public static void updateLife(Health health){
        Debug.Log("Working to update the lives UI");
    }
}
