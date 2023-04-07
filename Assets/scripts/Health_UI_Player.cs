using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health_UI_Player : MonoBehaviour
{
    [HideInInspector]
    public List<GameObject> lives;
    Transform[] livesTransforms;

    void Start()
    {
        getComponents();
        assignlivesGameObjects();
    }

    public void updateLife(Health health){
        for (int i = 0; i < lives.Count; i++) 
        {
            if(i < health.health) lives[i].SetActive(true);
            else lives[i].SetActive(false);
        }
    }

    void getComponents(){
        lives = new List<GameObject>();
        livesTransforms = GetComponentsInChildren<Transform>();
    }
    void assignlivesGameObjects(){
        int i = 0;
        foreach(Transform liveTransform in livesTransforms){
            liveTransform.gameObject.SetActive(true);

            // We require i be grater than 1 because the first two children of this object are the parents of the lives
            if (i > 1) lives.Add(liveTransform.gameObject);
            i++;
        }
    }
}
