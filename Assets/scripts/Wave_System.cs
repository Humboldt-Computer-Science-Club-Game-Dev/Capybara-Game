using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wave_System : MonoBehaviour
{
    [HideInInspector]
    public List<GameObject> waves;
    private int currentWave = -1;
    private GameObject envSpace;
    int numEnemies;
    int numWaves;
    void Start()
    {
        Event_System.onDeath += onDeath;

        //Each wave is a child of the Wave_System object
        numWaves = transform.childCount;
        for(int i = 0; i < numWaves; i++){
            waves.Add(transform.GetChild(i).gameObject);
            waves[i].SetActive(false);
        }
        envSpace = GameObject.Find("enviroment_space");
        spawnNextWave();
    }

    void spawnNextWave(){
        ++currentWave;
        if(currentWave >= numWaves) {
            Debug.Log("No more waves to spawn");
            return;
        }

        spawnWave(currentWave);
    }
    void spawnWave(int waveToSpawn){
        GameObject wave = waves[waveToSpawn];
        wave.SetActive(true);
        // The number of enemies to spawn is equal to the number of children in the current wave object
        numEnemies = wave.transform.childCount;
        for(int i = 0; i < numEnemies; i++) wave.transform.GetChild(0).SetParent(envSpace.transform, true);
    }
    void onDeath(string to){
        // This function only applies to enemy deaths
        if(!to.Contains("enemy")) return;
        --numEnemies;
        if(numEnemies <= 0) spawnNextWave();
    }
}
