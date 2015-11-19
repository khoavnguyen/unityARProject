﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using Vuforia;
using UnityEngine.Networking;

public class GameOverState : State
{
    GameObject explosion;
    Button PlayAgainButton;
    Button QuitButton;
    Text DisplayWinner;
    GameObject goBack;
    // Use this for initialization
    protected virtual void Awake()
    {
        explosion = GameObject.Find("explosion");
        explosion.SetActive(false);
        goBack = GameObject.Find("GO_Backdrop");
        if (goBack != null)
        {
            goBack.GetComponent<MeshRenderer>().enabled = false;
        }
        base.Awake();

        PlayAgainButton = GameObject.Find("PlayAgainButton").GetComponent<Button>();
        QuitButton = GameObject.Find("QuitButton").GetComponent<Button>();
        DisplayWinner = GameObject.Find("DisplayWinner").GetComponent<Text>();

        if (!PlayAgainButton)
            Debug.LogError("PlayAgainButton");
        if (!QuitButton)
            Debug.LogError("QuitButton");
    }

    public override void Initialize()
    {
        if (!gameManager)
            Debug.LogError("Cant find game manager");

        if (goBack != null)
        {
            goBack.GetComponent<MeshRenderer>().enabled = true;
        }
    }

    public override void ToMainMenu()
    {
        //Debug.Log("GameManager " + gameManager.name);
        //Debug.Log("MainMenuState " + gameManager.mainMenuState.name);
        NetworkManager.singleton.StopHost();
        gameManager.SetState(gameManager.mainMenuState);
        gameManager.ResetGame();

        if (goBack != null)
        {
            goBack.GetComponent<MeshRenderer>().enabled = false;
        }
    }

    public override void ToSharedModeMenu()
    {
        //Debug.Log("To shared Menu");
        gameManager.SetState(gameManager.sharedModeMenuState);

        if (goBack != null)
        {
            goBack.GetComponent<MeshRenderer>().enabled = false;
        }
    }

    public override void RunState()
    {
        displayWinner();
    }

    public void displayWinner()
    {
        ObjectTracker imgTracker = TrackerManager.Instance.GetTracker<ObjectTracker>();
        imgTracker.Stop();
        if (!player.isAllGlobalBombsDefused())
        {
            explosion.SetActive(true);

            if (player.isMultiplayer())
            {
                DisplayWinner.text = "Team 1 wins!";
            }
            else
            {
                DisplayWinner.text = player.getPlanterName() + " wins!";
            }
        }
        else
        {
            if (player.isMultiplayer())
            {
                DisplayWinner.text = "Team 2 wins!";
            }
            else
            {
                DisplayWinner.text = player.getDefuserName() + " wins!";
            }
        }
    }
}
