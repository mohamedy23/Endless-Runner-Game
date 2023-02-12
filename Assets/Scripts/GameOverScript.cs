using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class GameOverScript : MonoBehaviour
{
    // Start is called before the first frame update
    public TMP_Text score;
    public TMP_Text highestScoreText;
    private int highestScoreValue;
    public GameObject star;
    public GameObject NormalScore;
    private bool win;
     void Awake()
    {
        GameObject thePlayer = GameObject.Find("GameObject");
        if(thePlayer == null)
            score.text = 0.ToString("0");
        else
        {
            Score playerScript = thePlayer.GetComponent<Score>();
            int val = playerScript.score;
            if (val > PlayerPrefs.GetInt("Highest_Score"))
            {
                HighestScore(val);
                win = true;
            }
            else
            {
                NormalScoreFunc(val);
                win=false;
            }
                
        }
    }

    public void Restart()
    {
        Time.timeScale = 1f;
        GameObject audioManager = GameObject.Find("GameOverAudioManager");
        GameOverAudioManager audioScript = audioManager.GetComponent<GameOverAudioManager>();
        if(win)
            audioScript.Stop("Win");
        else
            audioScript.Stop("Lose");
        SceneManager.LoadScene("SampleScene");
    }
    public void goToMainMenu()
    {
        Time.timeScale = 0f;
        GameObject audioManager = GameObject.Find("GameOverAudioManager");
        GameOverAudioManager audioScript = audioManager.GetComponent<GameOverAudioManager>();
        if (win)
            audioScript.Stop("Win");
        else
            audioScript.Stop("Lose");
        SceneManager.LoadScene("Start");
    }

    private void HighestScore(int val)
    {
        PlayerPrefs.SetInt("Highest_Score", val);
        star.SetActive(true);
        NormalScore.SetActive(false);
        highestScoreText.text = val.ToString("0");
        GameObject audioManager = GameObject.Find("GameOverAudioManager");
        GameOverAudioManager audioScript = audioManager.GetComponent<GameOverAudioManager>();
        audioScript.play("Win");
    }

    private void NormalScoreFunc(int val)
    {
        score.text = val.ToString("0");
        star.SetActive(false);
        NormalScore.SetActive(true);
        GameObject audioManager = GameObject.Find("GameOverAudioManager");
        GameOverAudioManager audioScript = audioManager.GetComponent<GameOverAudioManager>();
        audioScript.play("Lose");
    }
}
