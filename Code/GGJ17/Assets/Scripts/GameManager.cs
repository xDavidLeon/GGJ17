using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoSingleton<GameManager> {
    [Header("WIN/LOSE States")]
    public bool gameWin = false;
    public bool gameLose = false;

    [Header("UI")]
    public CanvasGroup canvasWin;
    public Text txtScoreWin;
    public Text textCargoCount;

    [Header("Gameplay")]
    public Transform SpawnPoint;
    public Player[] players;
    public GameObject aerialCamera;

    public enum CAMERA_MODE
    {
        FPS = 0,
        AERIAL = 1
    };
    public CAMERA_MODE camMode = CAMERA_MODE.FPS;

    protected override void Init()
    {
        base.Init();
        RestartGame();
    }

	void Update () {
        if (Input.GetKeyDown(KeyCode.R)) ResetPlayer();
        if (Input.GetKeyDown(KeyCode.C)) SetCameraMode(CAMERA_MODE.AERIAL);
        if (Input.GetKeyDown(KeyCode.V)) SetCameraMode(CAMERA_MODE.FPS);

        foreach (Player player in players)
            if (player.currentShip != null)
                textCargoCount.text = player.currentShip.CountCargo().ToString();

        if ((gameWin || gameLose) && Input.GetKeyDown(KeyCode.Space)) RestartGame();
    }

    public void ResetPlayer()
    {
        foreach (Player player in players)
            player.transform.position = SpawnPoint.position;
    }

    public void SetCameraMode(CAMERA_MODE c)
    {
        camMode = c;
        if (camMode == CAMERA_MODE.AERIAL) aerialCamera.SetActive(true);
        else aerialCamera.SetActive(false);
    }

    public void GameWin()
    {
        gameWin = true;
        canvasWin.alpha = 1.0f;
        txtScoreWin.text = players[0].currentShip.CountCargo().ToString();
    }

    public void RestartGame()
    {
        gameWin = false;
        gameLose = false;
        canvasWin.alpha = 0.0f;

        ResetPlayer();
        SetCameraMode(CAMERA_MODE.FPS);
    }
}
