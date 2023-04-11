using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/* 
    Helper class for generating random numbers. 
    This class is needed because random numbers 
    won't be different if generated 
    consecutively in the same frame unless 
    either the seed is changed or the time is 
    changed or they are generated in different 
    scripts. This class is used to generate 
    random numbers in different scripts.
 */
public class Random_Number_Generator : MonoBehaviour
{
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
    public static float fiftyFifty(){
        return Mathf.Round(Random.Range(0, 2));
    }
}
