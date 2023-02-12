using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.SceneManagement;

public class Score : MonoBehaviour
{
    //public GameObject player;
    public int score;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    public void Update()
    {
        //score = player.
        //score = player.;
        DontDestroyOnLoad(this.gameObject);
        GameObject thePlayer = GameObject.Find("player");
        
        if (SceneManager.GetActiveScene().name != "GameOver" && SceneManager.GetActiveScene().name != "Start")
        {
            player playerScript = thePlayer.GetComponent<player>();
            score = playerScript.score;
           
           // Debug.Log(score);
        }
        

    }

}
