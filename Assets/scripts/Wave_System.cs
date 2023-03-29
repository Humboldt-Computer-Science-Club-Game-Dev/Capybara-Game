using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wave_System : MonoBehaviour
{
    [HideInInspector]
    public List<GameObject> waves;
    private int currentWave = -1;
    private GameObject envSpace;
    public int numEnemies;
    // Start is called before the first frame update
    
    void Start()
    {
        Event_System.onDeath += enemyDeath;
        int numWaves = transform.childCount;
        for(int i = 0; i < numWaves; i++){
            waves.Add(transform.GetChild(i).gameObject);
            waves[i].SetActive(false);
        }
        envSpace = GameObject.Find("enviroment_space");
        spawnNextWave();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void spawnNextWave(){
        //TODO: Check if there are any more waves to spawn
        Debug.Log("Current wave: " + currentWave);
        if((currentWave + 1) >= transform.childCount) {
            Debug.Log("No more waves to spawn");
            return;
        }
        //IF so, spawn the next wave
        ++currentWave;
        spawnWave(currentWave);
        //ELSE remove onDeath event 
        //then show win screen
    }
    void spawnWave(int waveToSpawn){
        waves[waveToSpawn].SetActive(true);
        numEnemies = waves[waveToSpawn].transform.childCount;
        for(int i = 0; i < numEnemies; i++){
            waves[waveToSpawn].transform.GetChild(0).SetParent(envSpace.transform, true);
        }
    }
    void enemyDeath(string to){
        if(!to.Contains("enemy")) return;
        --numEnemies;
        if(numEnemies <= 0) spawnNextWave();
    }
}
