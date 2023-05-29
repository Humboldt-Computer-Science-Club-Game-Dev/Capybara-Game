using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Event_System : MonoBehaviour
{
    public delegate void damageTaken(int damage, string to);
    public static event damageTaken onDamageTaken;

    public delegate void death(string to);
    public static event death onDeath;

    public delegate void gameOverDel();
    public static event gameOverDel onGameOver;

    public delegate void WaveEnds(int waveIndex);
    public static event WaveEnds onWaveEnds;

    public delegate void SpawnNextWave();
    public static event SpawnNextWave onSpawnNextWave;

    // Sequence events don't currently serve a purpose but they are here for future use.
    // DELETE this comment if a use for sequence events is implemented.
    public delegate void SequenceEnds();
    public static event SequenceEnds onSequenceEnds;

    /* 
        I have no dam clue why but events like onDamageTaken can't be called 
        outside of this script file ¯\_(ツ)_/¯ 
    */
    public static void takeDamage(int damage, string to)
    {
        if (onDamageTaken != null) onDamageTaken(damage, to);
    }
    public static void die(string to)
    {
        if (onDeath != null) onDeath(to);
    }
    public static void gameOver()
    {
        if (onGameOver != null) onGameOver();

        cleanUpForNextScene();

        //TODO: Make the next scene load the same scene dynamically. (Not hard coded)
        SceneManager.LoadScene("AnthonysProtoype_test_02");
    }

    public static void waveEnds(int waveIndex)
    {
        if (onWaveEnds != null) onWaveEnds(waveIndex);
    }

    public static void spawnNextWave()
    {
        if (onSpawnNextWave != null) onSpawnNextWave();
    }

    public static void sequenceEnds()
    {
        if (onSequenceEnds != null) onSequenceEnds();
    }

    static void cleanUpForNextScene()
    {
        // Setting these event to null so that they don't call the next time the scene is loaded.
        onDamageTaken = null;
        onDeath = null;
        onGameOver = null;
        onWaveEnds = null;
        onSpawnNextWave = null;
        onSequenceEnds = null;
    }
}
