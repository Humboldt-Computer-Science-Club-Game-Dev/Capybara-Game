using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Random_Number_Generator : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public static int random1DigitNumber()
    {
        return Random.Range(1, 10);
    }
}