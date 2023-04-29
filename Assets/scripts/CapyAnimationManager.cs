using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CapyAnimationManager : MonoBehaviour
{
    GameObject parent;
    Vector3 PrevPos; 
    Vector3 NewPos; 
    Vector3 ObjVelocity;
    Animator animator;
    string currentAnimation = "idle";
    string[] directions = { "upright", "downright", "upleft", "downleft", "right", "left", "up", "down", "idle" };
    void Start()
    {
        parent = transform.parent.gameObject;
        PrevPos = parent.transform.position;
        NewPos = parent.transform.position;
        animator = GetComponent<Animator>();
    }

    void FixedUpdate()
    {
        NewPos = parent.transform.position;
        ObjVelocity = (NewPos - PrevPos) / Time.fixedDeltaTime;
        PrevPos = NewPos;
        handleAnimation();
    }

    int  getDirectionIndex(){
        // We return the index from deriving the direction from the velocity instead of returning a string because 
        // it reduces the amount of typing you have to do when you want to add a new direction
        if(ObjVelocity.x > 0.1f && ObjVelocity.y > 0.1f){
            return 0;
        }
        else if(ObjVelocity.x > 0.1f && ObjVelocity.y < -0.1f){
            return 1;
        }
        else if(ObjVelocity.x < -0.1f && ObjVelocity.y > 0.1f){
            return 2;
        }
        else if(ObjVelocity.x < -0.1f && ObjVelocity.y < -0.1f){
            return 3;
        }
        else if (ObjVelocity.x > 0.1f)
        {
            return 4;
        }
        else if (ObjVelocity.x < -0.1f)
        {
            return 5;
        }
        else if (ObjVelocity.y > 0.1f)
        {
            return 6;
        }
        else if (ObjVelocity.y < -0.1f)
        {
            return 7;
        }
        else
        {
            return 8;
        }
    }

    string getDirection(){
        return directions[getDirectionIndex()];
    }

    void handleAnimation(){
        //TODO: Have isShooting be derived from the player or gun script
        string direction = getDirection();
        bool isShooting = false;

        if(isShooting) setAnimation("shoot" + direction);
        else setAnimation(direction);
    }

    void setAnimation(string direction){
        if(currentAnimation == direction) return;
        animator.SetTrigger(direction);
        currentAnimation = direction;
    }
}
