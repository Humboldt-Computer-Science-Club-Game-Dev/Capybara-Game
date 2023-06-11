using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pause_Menu : MonoBehaviour
{
    bool paused = false;
    GameObject outerContainer;
    // Start is called before the first frame update
    void Start()
    {
        foreach (Transform child in this.transform.parent.transform)
        {
            if (child.gameObject.name == "pause_menu_outer_container")
            {
                outerContainer = child.gameObject;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void runPauseMenu()
    {
        if (!paused)
        {
            Time.timeScale = 0;
            outerContainer.SetActive(true);
        }
        else
        {
            Time.timeScale = 1;
            outerContainer.SetActive(false);
            // Hide pause menu
        }
        paused = !paused;
    }

    public void levelSelect()
    {
        Time.timeScale = 1;
        paused = !paused;
        outerContainer.SetActive(false);
        Event_System.openLevelSelect();
    }

    public void restartLevel()
    {
        Time.timeScale = 1;
        paused = !paused;
        outerContainer.SetActive(false);
        Event_System.restartLevel();
    }

    public void mainMenu()
    {
        Time.timeScale = 1;
        paused = !paused;
        outerContainer.SetActive(false);
        Event_System.openMainMenu();
    }

    public void quitGame()
    {
        Event_System.quitGame();
    }
}
