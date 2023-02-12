using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class player : MonoBehaviour
{
    // Start is called before the first frame update
    public Rigidbody rb;
    private bool inLane1 = false;
    private bool inLane2 = true;
    private bool inLane3 = false;
    private Vector3 horizontalOffset = new Vector3(6, 0, 0);
    private Vector3 verticalOffset = new Vector3(0, 2.5f, 0);
    private bool canJump;
    private bool FirstTrigger = true;
    public GameObject lane1;
    public GameObject lane2;
    public GameObject lane3;
    private int laneLength = 50;
    private int zSpawn = -25;
    private List<GameObject> lanes1 = new List<GameObject>();
    private List<GameObject> lanes2 = new List<GameObject>();
    private List<GameObject> lanes3 = new List<GameObject>();
    private List<GameObject> obstacles1 = new List<GameObject>();
    private int laneIndex = 0;
    private int spawn = 0;
    private int destroy = 25;
    public GameObject lane_1_Obstacle;
    public GameObject lane_2_Obstacle;
    public GameObject lane_3_Obstacle;
    private int temp = 30;
    private int laneStart = -25;
    private int laneEnd = 25;
    public GameObject healthOrbs;
    public GameObject abilityOrbs;
    private int noOfOldCollectables = 0;
    private int noOfCurrentCollectables = 0;
    private List<List<GameObject>> collectables = new List<List<GameObject>>();
    public TMP_Text scoreValue;
    public int score = 0;
    public TMP_Text abilityValue;
    private int ability = 10;
    public TMP_Text healthValue;
    private int health = 5;
    private bool GameIsPaused = false;
    public GameObject PauseMenu;
    public GameObject GameOverMenu;
    public GameObject WindowButtons;
    public TMP_Text FinalScore;
    private bool ShowWindowButtons = true;
    private int localHealth;
    public GameObject Star;

    //public GameObject gameObject;
    //public GameObject cube;
    //public Sound[] sounds;
    void Start()
    {
        rb = this.GetComponent<Rigidbody>();
        lanes1.Add(lane1);
        lanes2.Add(lane2);
        lanes3.Add(lane3);
        AudioManager();
        //foreach (Sound s in sounds)
        //{
        //    s.audioSource = gameObject.AddComponent<AudioSource>();
        //    // s.audioSource.loop = true;
        //    s.audioSource.clip = s.clip;
        //    s.audioSource.volume = s.volume;
        //    s.audioSource.pitch = s.pitch;
        //    Debug.Log(s.name);
        //    Debug.Log(s.volume);
        //}
    }

    // Update is called once per frame
    void Update()
    {
        //rb.AddForce(Vector3.forward * 8);
        this.transform.Translate(Vector3.forward * Time.deltaTime * 10);
        RotateCollectables();
        //abilityOrbs.transform.Rotate(Vector3.up * Time.deltaTime * 10);
        // this.transform.position += new Vector3(0,0,10);
        getLane();
        InputManager();
        SpawnManager();
        UIManager();
        //Score();
        //gameObject
        //Score playerScript = gameObject.GetComponent<Score>();
        //playerScript.Update();
        //healthOrbs.transform.Rotate(0f,20f * Time.deltaTime,0f);
        //DontDestroyOnLoad(this.gameObject);
    }

    public void moveRight()
    {
        if (!inLane3)
        {
            this.transform.position += horizontalOffset;
            Debug.Log("Right");
        }
    }
    public void moveLeft()
    {
        if (!inLane1)
        {
            this.transform.position -= horizontalOffset;
            Debug.Log("left");
        }
        
    }

    public void Jump()
    {
        if (this.transform.position.y == 0.6f && ability > 0)
        {
            this.transform.position += verticalOffset;
            SetAbility(-1);
        }
    }

    public void Ability()
    {
        if (ability >= 5)
        {
            SpecialAbility();
        }
    }

    void AudioManager()
    {
        GameObject audioManager = GameObject.Find("PlayAudioManager");
        PlayAudioManager audioScript = audioManager.GetComponent<PlayAudioManager>();
        audioScript.play("Chasing");
    }

    void RotateCollectables()
    {
        foreach (List<GameObject> collectableList in collectables)
        {
            foreach (GameObject collectable in collectableList)
            {
                if (collectable.IsDestroyed() == false)
                {
                    collectable.transform.Rotate(0f, 90.0f * Time.deltaTime, 0.0f, Space.Self);
                }
                
            }
            
        }
        
    }
    void GameIsOver()
    {
        Time.timeScale = 0f;
        GameObject audioManager = GameObject.Find("PlayAudioManager");
        PlayAudioManager audioScript = audioManager.GetComponent<PlayAudioManager>();
        // FindObjectOfType<PlayAudioManager>().play("Desert");
        audioScript.Stop("Chasing");
        //audioScript.play("Desert");
        SceneManager.LoadScene("GameOver");
    }
    void getLane()
    {
        if (transform.position.x >= -2.5 && transform.position.x <= 2.5)
        {
            inLane2 = true;
            inLane1 = false;
            inLane3 = false;
        }
        if (transform.position.x >= -8.5 && transform.position.x <= -3.5)
        {
            inLane1 = true;
            inLane2 = false;
            inLane3 = false;
        }
        if (transform.position.x >= 3.5 && transform.position.x <= 8.5)
        {
            inLane1 = false;
            inLane2 = false;
            inLane3 = true;
        }
    }
    void InputManager()
    {
        if ((Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A)) && inLane2 && !GameIsPaused)
        {
            this.transform.position -= horizontalOffset;
        }
        if ((Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A)) && inLane3 && !GameIsPaused)
        {
            this.transform.position -= horizontalOffset;
        }
        if ((Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D)) && inLane1 && !GameIsPaused)
        {
            this.transform.position += horizontalOffset;
        }
        if ((Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D) )&& inLane2 && !GameIsPaused)
        {
            this.transform.position += horizontalOffset;
        }
        if (Input.GetKeyDown(KeyCode.Space) && this.transform.position.y == 0.6f && ability > 0 && !GameIsPaused)
        {
            this.transform.position += verticalOffset;
            SetAbility(-1);
        }
        if (Input.GetKeyDown(KeyCode.Q) && ability >= 5 && !GameIsPaused)
        {
            SpecialAbility();
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            PauseManager();
        }
        
        if (Input.GetKeyDown(KeyCode.W) && !GameIsPaused)
        {
            WindowButtonManager();
        }
    }

    void WindowButtonManager()
    {
        if (ShowWindowButtons)
        {
            WindowButtons.SetActive(false);
            ShowWindowButtons = false;
        }
        else
        {
            WindowButtons.SetActive(true);
            ShowWindowButtons = true;
        }
    }

    public void PauseManager()
    {
        if (!GameIsPaused)
        {
            Pause();
        }
        else
        {
            Resume();
        }
    }
    void Pause()
    {
        PauseMenu.SetActive(true);
        WindowButtons.SetActive(false);
        Time.timeScale = 0f;
        GameIsPaused = true;
        GameObject audioManager = GameObject.Find("PlayAudioManager");
        PlayAudioManager audioScript = audioManager.GetComponent<PlayAudioManager>();
        audioScript.Pause("Chasing");
        audioScript.play("Pause");
    }
    public void Resume()
    {
        PauseMenu.SetActive(false);
        if(ShowWindowButtons)
            WindowButtons.SetActive(true);
        Time.timeScale = 1f;
        GameIsPaused = false;
        GameObject audioManager = GameObject.Find("PlayAudioManager");
        PlayAudioManager audioScript = audioManager.GetComponent<PlayAudioManager>();
        audioScript.Stop("Pause");
        audioScript.play("Chasing");
    }

    public void goToMainMenu()
    {
        Time.timeScale = 0f;
        GameObject audioManager = GameObject.Find("PlayAudioManager");
        PlayAudioManager audioScript = audioManager.GetComponent<PlayAudioManager>();
        audioScript.Stop("Pause");
        audioScript.Stop("Chasing");
        SceneManager.LoadScene("Start");
    }
    public void Restart()
    {
        Time.timeScale = 1f;
        GameObject audioManager = GameObject.Find("PlayAudioManager");
        PlayAudioManager audioScript = audioManager.GetComponent<PlayAudioManager>();
        audioScript.Stop("Pause");
        audioScript.Stop("Chasing");
        SceneManager.LoadScene("SampleScene");
    }
    //void printLane()
    //{
    //    if (inLane1)
    //        //Debug.Log("lane 1");
    //    if (inLane2)
    //        //Debug.Log("lane 2");
    //    if (inLane3)
    //        //Debug.Log("lane 3");
    //}

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "lane1ObstacleChild")
        {
            if(localHealth == health)
                SetScore(3);        
        }
        if (other.gameObject.tag == "lane2ObstacleChild")
        {
            if (localHealth == health) 
                SetScore(2);
        }
        if (other.gameObject.tag == "lane3ObstacleChild")
        {
            if (localHealth == health) 
                SetScore(1);
        }
        
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "lane1ObstacleChild")
        {
            localHealth = health;
        }
        if (other.gameObject.tag == "lane2ObstacleChild")
        {
            localHealth = health;
        }
        if (other.gameObject.tag == "lane3ObstacleChild")
        {
            localHealth = health;
        }

    }



    void SpawnLane()
    {
            GameObject l1 = Instantiate(lane1, new Vector3(-6f,0f,zSpawn + laneLength + laneLength/2), Quaternion.identity);
            GameObject l2 = Instantiate(lane2, new Vector3(0f, 0f, 
                                        zSpawn + laneLength + laneLength / 2), Quaternion.identity);
            GameObject l3 = Instantiate(lane3, new Vector3(6f, 0f, zSpawn + laneLength + laneLength / 2), Quaternion.identity);
            lane1 = l1;
            lane2 = l2; 
            lane3 = l3;
            //lanes.Add(g);
            lanes1.Add(l1);
            lanes2.Add(l2);
            lanes3.Add(l3);
        
            zSpawn += laneLength;
            laneStart = zSpawn;
            laneEnd = laneStart + laneLength;
    }
    void DestroyLane()
    {
        Destroy(lanes1[0]);
        lanes1.RemoveAt(0);

        Destroy(lanes2[0]);
        lanes2.RemoveAt(0);

        Destroy(lanes3[0]);
        lanes3.RemoveAt(0);
       
    }
    
    void SpawnObstacle()
    {
        //spwaing the first level

        GameObject type1 = chooseObstacleType();
        GameObject type2 = chooseObstacleType();
        GameObject type3 = chooseObstacleType();
        GameObject o1 = Instantiate(type1, ObstaclePosition(1,type1), Quaternion.identity);
        GameObject o2 = Instantiate(type2, ObstaclePosition(3,type2), Quaternion.identity);
        GameObject o3 = Instantiate(type3, ObstaclePosition(5,type3), Quaternion.identity);
        obstacles1.Add(o1);
        obstacles1.Add(o2);
        obstacles1.Add(o3);

    }
    void DestroyObstacle()
    {
        Destroy(obstacles1[0]);
        obstacles1.RemoveAt(0);

        Destroy(obstacles1[0]);
        obstacles1.RemoveAt(0);

        Destroy(obstacles1[0]);
        obstacles1.RemoveAt(0);
    }

    void SpawnCollectable()
    {
        int[] levels = {2, 4 };
        SpawnCollectableHelper(levels);
    }

    void SpawnCollectableHelper(int [] levels)
    {
        int l = 3;
        List<GameObject> currentCollectables = new List<GameObject>();
        foreach (int level in levels)
        {
            l = 3;
            int start = laneStart + 10 * (level - 1);
            int end = laneStart + 10 * (level);
            int noOfCollectables = Random.Range(1, 4);
            //noOfCurrentCollectables += noOfCollectables;
            List<int> xChoices = new List<int>();
            xChoices.Add(-6);
            xChoices.Add(0);
            xChoices.Add(6);
            for (int i = 0; i < noOfCollectables; i++)
            {
                //print(xChoices.Capacity);
                //print(xChoices.Count);
                GameObject collectable = chooseCollectable();
                int x = xChoices.ToArray()[Random.Range(0, l--)];
                //print(x);
                //if(xChoices.Count != 0)
                xChoices.Remove(x);
                int z = ChooseZ(start, end);
                GameObject c = Instantiate(collectable, new Vector3(x, 1, z), Quaternion.identity);
                currentCollectables.Add(c);
            }
        }
        collectables.Add(currentCollectables);
    }

    void DestroyCollectable()
    {
        foreach (GameObject collectable in collectables[0])
        {
            Destroy(collectable);
        }
        collectables.Remove(collectables[0]);
    }
    GameObject chooseCollectable()
    {
        GameObject[] collectables = {healthOrbs, abilityOrbs};
        int random = Random.Range(0,2);
        return collectables[random];
    }

    int ChooseX1()
    {
        int[] array = {-6,0,6};
        int laneIndex = Random.Range(0,3);
        return array[laneIndex];
    }
    int ChooseX2()
    {
        int[] array = { -3, 3 };
        int laneIndex = Random.Range(0, 2);
        return array[laneIndex];
    }
    int ChooseZ(int start, int end)
    {
        int laneIndex = (start + end)/2;
        return laneIndex;
    }

    void SpawnManager()
    {
        if (this.transform.position.z > spawn)
        {
            SpawnLane();
            SpawnObstacle();
            SpawnCollectable();
            //Debug.Log("add lane");
            spawn += laneLength;
        }
        if (this.transform.position.z > destroy)
        {
            DestroyLane();
            if(obstacles1.ToArray().Length == 6)
                DestroyObstacle();
            if(destroy >= 75)
                DestroyCollectable();
            //Debug.Log("add lane");
            destroy += laneLength;
        }

    }

    Vector3 ObstaclePosition(int level, GameObject type)
    {
        int start = laneStart + 10 * (level - 1);
        int end = laneStart + 10 * (level);
        int x = 0;
        if (type == lane_1_Obstacle)
        {
             x = ChooseX1();
        }
        if (type == lane_2_Obstacle)
        {
             x = ChooseX2();
        }
        
        int z = ChooseZ(start, end);
        return new Vector3(x,1,z);
    }

    GameObject chooseObstacleType()
    {
        GameObject[] types = {lane_1_Obstacle, lane_2_Obstacle, lane_3_Obstacle };
        int obstacleIndex = Random.Range(0, 3);
        return types[obstacleIndex];
    }
    
    void UIManager()
    {
        if (health != 0)
        {
            scoreValue.text = score.ToString("0");
            abilityValue.text = ability.ToString("0");
            healthValue.text = health.ToString("0");
        }
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        GameObject audioManager = GameObject.Find("PlayAudioManager");
        PlayAudioManager audioScript = audioManager.GetComponent<PlayAudioManager>();

        if (collision.gameObject.tag == "lane1Obstacle")
        {

            Destroy(collision.gameObject);
            if(audioScript != null)
                audioScript.play("Obstacle");
            SetHealth(-3);
        }
        if (collision.gameObject.tag == "lane2Obstacle")
        {
            Destroy(collision.gameObject);
            if (audioScript != null) 
                audioScript.play("Obstacle");
            SetHealth(-2);
        }
        if (collision.gameObject.tag == "lane3Obstacle")
        {
            Destroy(collision.gameObject);
            if (audioScript != null) 
                audioScript.play("Obstacle");
            SetHealth(-1);
        }
        if (collision.gameObject.tag == "healthOrbs")
        {
            Destroy(collision.gameObject);
            if (audioScript != null) 
                audioScript.play("Collectable");
            SetHealth(1);
        }
        if (collision.gameObject.tag == "abilityOrbs")
        {
            Destroy(collision.gameObject);
            if (audioScript != null) 
                audioScript.play("Collectable");
            SetAbility(1);
        }

    }

    void SetHealth(int x)
    {
        health += x;
        health = Mathf.Min(health, 5);
        health = Mathf.Max(health, 0);
        if (health == 0)
            GameIsOver();
    }
    void SetAbility(int x)
    {
        ability += x;
        ability = Mathf.Min(ability,10);
        ability = Mathf.Max(ability,0);
    }
    
    void SetScore(int value)
    {
        score += value;
        if (score > PlayerPrefs.GetInt("Highest_Score"))
        {
            Star.SetActive(true) ;
        }
    }

    void SpecialAbility()
    {
        foreach (GameObject obstacle in obstacles1)
        {
            Destroy(obstacle);
        }
        SetAbility(-5);
    }

    void Play()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
    public static int[] getUniqueRandomArray(int min, int max, int count)
    {
        int[] result = new int[count];
        List<int> numbersInOrder = new List<int>();
        for (var x = min; x < max; x++)
        {
            numbersInOrder.Add(x);
        }
        for (var x = 0; x < count; x++)
        {
            var randomIndex = UnityEngine.Random.Range(0, numbersInOrder.Count);
            result[x] = numbersInOrder[randomIndex];
            numbersInOrder.RemoveAt(randomIndex);
        }

        return result;
    }
}
