using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Run_Pineapple_Anim : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Animator animator = GetComponent<Animator>();
        float randomStartPoint = Random.Range(0.0f, 1.0f);
        animator.Play("pineapple", -1, randomStartPoint);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
