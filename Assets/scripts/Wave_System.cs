using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wave_System : MonoBehaviour
{
    [HideInInspector]
    public List<GameObject> waves;
    /* [HideInInspector] */
    public int currentWave = -1;
    private GameObject envSpace;
    int numEnemies;
    int numWaves;
    bool initialized = false;
    void Awake()
    {
    }
    void Start()
    {
        if (!initialized) initialize();
    }

    void initialize()
    {
        if (initialized) return;
        currentWave = -1;
        Event_System.onDeath += onDeath;

        //Each wave is a child of the Wave_System object
        numWaves = transform.childCount;
        for (int i = 0; i < numWaves; i++)
        {
            waves.Add(transform.GetChild(i).gameObject);
            waves[i].SetActive(false);
        }

        envSpace = GameObject.Find("enviroment_space");
        Event_System.onSpawnNextWave += spawnNextWave;

        Event_System.waveEnds(currentWave);
        initialized = true;
    }


    public void spawnNextWave()
    {
        //This method gets called by dialog system at its start witch can be before the start of wave_system. For this reason, we need to check if this script has been initialized
        if (!initialized) initialize();

        ++currentWave;
        if (currentWave >= numWaves)
        {
            Debug.Log("Wave_System: lastWaveFinished " + "currentWave: " + currentWave + " numWaves: " + numWaves + " numEnemies: " + numEnemies);
            Event_System.lastWaveFinished();
            return;
        }

        spawnWave(currentWave);
    }
    void spawnWave(int waveToSpawn)
    {
        GameObject wave = waves[waveToSpawn];
        wave.SetActive(true);
        // The number of enemies to spawn is equal to the number of children in the current wave object
        numEnemies = wave.transform.childCount;
        Debug.Log("Round End stats ===============\n new currentWave: " + currentWave + " numWaves: " + numWaves + " numEnemies: " + numEnemies + "\n===============");
        for (int i = 0; i < numEnemies; i++) wave.transform.GetChild(0).SetParent(envSpace.transform, true);
    }
    void onDeath(string to)
    {
        // This function only applies to enemy deaths
        if (!to.Contains("enemy")) return;
        --numEnemies;

        if (numEnemies <= 0) Event_System.waveEnds(currentWave);
    }
    public int voidGetNumWaves()
    {
        return numWaves;
    }
}
