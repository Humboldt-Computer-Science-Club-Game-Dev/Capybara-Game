using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* 
    This script is attached to the environment object's parent that need to scroll. 
    It is a simple script that moves the object in the direction of the scrollSpeed vector. 
*/
public class ENVScroller : MonoBehaviour
{
    public Vector2 scrollSpeed;
    void Update()
    {
        transform.Translate(scrollSpeed * Time.deltaTime);
    }
}
