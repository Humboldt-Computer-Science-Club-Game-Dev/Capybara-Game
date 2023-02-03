using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* PRE-Requisites. Player must be instantiated in scene
TODOS: Make it so that player prefab comes with the health UI prefab attached to it */

public class Health_UI_Player : MonoBehaviour
{
    private GameObject livesContainer;
    public List<GameObject> lives;
    // Start is called before the first frame update
    void Start()
    {
        livesContainer = this.transform.GetChild(0).gameObject;
        Transform[] livesTransforms = GetComponentsInChildren<Transform>();
        foreach(Transform liveTransform in livesTransforms){
            lives.Add(liveTransform.gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void updateLife(Health health){
        for (int i = 2; i < lives.Count; i++) 
        {
            int realI = i - 2;
            if(realI < health.health){
                lives[i].SetActive(true);
            }
            else{
                lives[i].SetActive(false);
            }
        }
    }
}
