using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* 
    This is a super simple script that is meant to just store an 
    integer that will get accessed by enemies as they position there 
    selfs on screen. Each enemy has a designated rest bound and will 
    continue to move left until they collide with an collider that 
    has this script and has the same rest bound as the enemies
    designated rest bound. 
*/
public class Enemy_Rest_Bound : MonoBehaviour
{
    public int restBound = 1;
}
