using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ENVScroller : MonoBehaviour
{
    public Vector2 velocity;
    void Start()
    {
        
    }

    void Update()
    {
        transform.Translate(velocity * Time.deltaTime);
    }
}
