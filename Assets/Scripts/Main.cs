using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Main : Singleton<Main>
{
    // Input Data
    public GameObject title;
    public GameObject inGame;

    void Start()
    {
        GameManager.Instance.PreInit();

        ChangeGameScene(false);
    }

    public void ChangeGameScene(bool isInGame)
    {
        title.SetActive(!isInGame);
        inGame.SetActive(isInGame);
    }

    public void OnPressedStartButton()
    {
        ChangeGameScene(true);

        GameManager.Instance.Init();
    }
}
