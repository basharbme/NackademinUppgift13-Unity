﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class GameMenu : MonoBehaviour
{
    [Header("Game manager stuff")]
    public string localPlayerName;

    [Header("Settings")]
    public float fadeSpeed = 3;
    [Range(0, 1)]
    public float interactableThreshold = 0.9f;
    [Range(0, 1)]
    public float blockRaycastThreshold = 0.4f;
    [Range(0, 1)]
    public float fadingThreshold = 0.95f;

    [Header("CanvasGroup references")]
    public CanvasGroup groupBackground;

    public CanvasGroup groupPlayerNamePanel;
    public CanvasGroup groupPlayerNameLocal;
    public CanvasGroup groupPlayerNameRemote;
    public CanvasGroup groupPickName;
    public CanvasGroup groupPlaceYourShips;
    public CanvasGroup groupNewGame;
    public CanvasGroup groupHostGame;
    public CanvasGroup groupJoinGame;
    public CanvasGroup groupLoadingModalWithAbort;
    public CanvasGroup groupErrorModalWithClose;
    public CanvasGroup groupGameStarted;

    [Header("Input references")]
    public InputField fieldPlayerName;
    public InputField fieldHostPort;
    public InputField fieldJoinAddress;
    public InputField fieldJoinPort;

    [Header("Specific elements")]
    public Text textPlayerName;
    public BoardShipPlacer shipPlacer;
    public Button buttonFinishMovingShips;
    public Text textErrorMessage;

    private IEnumerator Start()
    {
        yield return new WaitForSeconds(0.2f);
        StartCoroutine(FadeInMenuCoroutine(groupBackground));
        yield return new WaitForSeconds(1);
        StartCoroutine(FadeInMenuCoroutine(groupPickName));
    }

    public void FadeInMenu(CanvasGroup canvasGroup)
    {
        StartCoroutine(FadeInMenuCoroutine(canvasGroup));
    }
    public void FadeOutMenu(CanvasGroup canvasGroup)
    {
        StartCoroutine(FadeOutMenuCoroutine(canvasGroup));
    }

    private IEnumerator FadeInMenuCoroutine(CanvasGroup canvasGroup)
    {
        canvasGroup.blocksRaycasts = true;

        while ((canvasGroup.alpha = Mathf.Lerp(canvasGroup.alpha, 1, Time.deltaTime * fadeSpeed)) < fadingThreshold)
        {
            canvasGroup.interactable = canvasGroup.alpha > interactableThreshold;
            yield return null;
        }

        canvasGroup.interactable = true;
        canvasGroup.alpha = 1;
    }

    private IEnumerator FadeOutMenuCoroutine(CanvasGroup canvasGroup)
    {
        canvasGroup.interactable = false;
        while ((canvasGroup.alpha = Mathf.Lerp(canvasGroup.alpha, 0, Time.deltaTime * fadeSpeed)) > 1 - fadingThreshold)
        {
            canvasGroup.blocksRaycasts = canvasGroup.alpha > blockRaycastThreshold;
            yield return null;
        }
        
        canvasGroup.blocksRaycasts = false;
        canvasGroup.alpha = 0;
    }
    
    public void Menu_PickName_SetPlayerName()
    {
        string playerName = fieldPlayerName.text.Trim();
        if (playerName.Length > 0)
        {
            textPlayerName.text = localPlayerName = playerName;
            FadeOutMenu(groupBackground);
            FadeOutMenu(groupPickName);
            FadeInMenu(groupPlaceYourShips);
            FadeInMenu(groupPlayerNamePanel);
            FadeInMenu(groupPlayerNameLocal);
            shipPlacer.enabled = true;
        }
        else
        {
            Menu_Error_Show("Please enter your name.");
        }
    }

    public void Menu_PlaceYourShips_AllShipsMoved()
    {
        buttonFinishMovingShips.interactable = true;
    }

    public void Menu_PlaceYourShips_FinishedButton()
    {
        shipPlacer.enabled = false;
        FadeOutMenu(groupPlaceYourShips);
        FadeInMenu(groupNewGame);
    }

    public void Menu_Error_Show(string error)
    {
        textErrorMessage.text = error;
        FadeInMenu(groupErrorModalWithClose);
    }
}
