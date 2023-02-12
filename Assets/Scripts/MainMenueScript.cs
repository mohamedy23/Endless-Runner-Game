using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenueScript : MonoBehaviour
{
    // Start is called before the first frame update
    public Toggle sound;
     void Start()
    {
        GameObject audioManager = GameObject.Find("StartAudioManager");
        StartAudioManager audioScript = audioManager.GetComponent<StartAudioManager>();
        Debug.Log(sound.isOn);
        if (AudioListener.volume == 0)
            sound.isOn = false;
        else
            sound.isOn = true;
        if(sound.isOn)
            audioScript.play("Dahab");


    }


    public void Play()
    {
        Time.timeScale = 1f;
        GameObject audioManager = GameObject.Find("StartAudioManager");
        StartAudioManager audioScript = audioManager.GetComponent<StartAudioManager>();
        audioScript.Stop("Dahab");
        SceneManager.LoadScene("SampleScene");
    }

    public void Mute()
    {
        GameObject audioManager = GameObject.Find("StartAudioManager");
        StartAudioManager audioScript = audioManager.GetComponent<StartAudioManager>();
        if (sound.isOn == false)
        {
            AudioListener.volume = 0;
            Debug.Log("off");
        }
        else
        {
            AudioListener.volume = 1;
            audioScript.play("Dahab");
            Debug.Log("on");
        }
    }
    public void Quit()
    {
        Application.Quit();
    }
}
