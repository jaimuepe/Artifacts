using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PauseCanvas : MonoBehaviour
{

    public Button continueButton;

    public void ExitToDesktop()
    {
        Application.Quit();
    }

    public void SwitchState()
    {
        if (GameManager.instance.gamePaused)
        {
            gameObject.SetActive(false);
            EventSystem.current.SetSelectedGameObject(null);
        }
        else
        {
            gameObject.SetActive(true);
            EventSystem.current.SetSelectedGameObject(continueButton.gameObject);
        }
        GameManager.instance.SwitchPauseState();
    }
}
