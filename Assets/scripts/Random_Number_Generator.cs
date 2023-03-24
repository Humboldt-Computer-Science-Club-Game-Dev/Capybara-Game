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
    public static int random10DigitNumber(){
        return Random.Range(10, 100);
    }
    public static float randomPositiveFloatUnder(float max){
        return Random.Range(0, max);
    }
    
}
