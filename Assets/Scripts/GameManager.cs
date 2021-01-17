using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : Singleton<GameManager>
{
    // Iput Data
    public float maxPlayTime;

    public Text playTimeText;

    public GameEndPopup gameEndPopup;

    // Local Data
    private bool isGame; // ���� ���� �������ΰ�?

    private float _playTime;  // ������ ������ �� �ð�
    private float playTime // _playTime�� GetSet ����
    {
        get => _playTime;
        set
        {
            _playTime = value;
            playTimeText.text = Math.Ceiling(_playTime).ToString();
        }
    }

    public void PreInit()
    {
        CardManager.Instance.PreInit();
    }

    public void Init()
    {
        isGame = true;
        playTime = maxPlayTime;
        gameEndPopup.gameObject.SetActive(false);

        CardManager.Instance.Init();
    }

    public void Release()
    {
        CardManager.Instance.Release();
    }

    public void OnGameEnd()
    {
        isGame = false;

        gameEndPopup.gameObject.SetActive(true);
        gameEndPopup.Init((int) Math.Ceiling(playTime));
    }

    void Update()
    {
        if (!isGame) return;

        playTime -= Time.deltaTime;

        if (playTime > 0) return;

        playTime = 0;
        OnGameEnd();
    }

    public void RestartGame()
    {
        Release();
        Init();
    }
}
