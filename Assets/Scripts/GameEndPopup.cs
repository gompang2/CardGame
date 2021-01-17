using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameEndPopup : MonoBehaviour
{
    // Input Data
    public Text scoreText;
    public Text bestText;

    public void Init(int score)
    {
        int best = PlayerPrefs.GetInt("Best", 0);

        scoreText.text = score.ToString();

        if(best < score)
        {
            best = score;
            PlayerPrefs.SetInt("Best", score);
        }

        bestText.text = best.ToString();
    }

    public void OnPressedRestart()
    {
        GameManager.Instance.RestartGame();
    }

    public void OnPressedHome()
    {
        GameManager.Instance.Release();
        Main.Instance.ChangeGameScene(false);
    }
}
