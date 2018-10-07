using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class VictoryManager : MonoBehaviour {

    [HideInInspector]
    public static VictoryManager instance;

    [Header("Instance Connections")]
    public GameObject victoryMenu;
    public GameObject defeatMenu;
    public GameObject objectivesMenu;

    public enum victoryChaining { ANY, ALL };
    [Header("Victory Requirements")]
    public victoryChaining victoryType;
    public List<VictoryBase> victoryConditions;

    bool victory = false;
    bool defeat = false;

    // Use this for initialization
    void Start () {
        instance = this;
	}
	
    // Checks if the conditions to win are met and if so, triggers victory
    public void CheckVictory()
    {
        bool victory = true;

        if (victoryType == victoryChaining.ALL)
        {
            victory = true;
            foreach(VictoryBase victoryBase in victoryConditions)
            {
                if(!victoryBase.RequirementsMet())
                {
                    victory = false;
                    break;
                }
            }
        }
        else
        {
            victory = false;
            foreach (VictoryBase victoryBase in victoryConditions)
            {
                if (victoryBase.RequirementsMet())
                {
                    victory = true;
                    break;
                }
            }
        }

        if(victory)
        {
            TriggerVictory();
        }
    }

    // Makes the victory button appear
    public void TriggerVictory()
    {
        // Only let this trigger once, and only if we haven't lost first
        if (!victory && !defeat)
        {
            victory = true;
            objectivesMenu.SetActive(false);
            victoryMenu.SetActive(true);
        }
    }

    // Makes the defeat menu appear
    public void TriggerDefeat()
    {
        // Only let this trigger once, can override a victory
        if (!defeat)
        {
            objectivesMenu.SetActive(false);
            victoryMenu.SetActive(false);
            defeatMenu.SetActive(true);
        }
    }

    // Marks this level as complete, marks resources gained, returns you to level select
    public void VictoryAction()
    {
        // save data
        SceneManager.LoadSceneAsync("LevelSelect");
    }

    // Sends you back to the level select menu
    public void ReturnAction()
    {
        SceneManager.LoadSceneAsync("LevelSelect");
    }

    // Resets the current level
    public void ResetAction()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
