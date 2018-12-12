using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameController : MonoBehaviour {

    public GameObject[] numbers;
    public GameObject oneUp;

    private static int deathState;
    private static float restartTimer;
    private static PlayerController playerController;
    private static Vector3 playerPosition = new Vector3(-8.521f, 0.3f, -5f);
    private static AudioSource[] audioClips;
    private static GameObject score;
    private static GameObject coin;
    private static GameObject time;
    private static int scoreNum = 0;
    private static int coinNum = 0;
    private static int timeNum;
    private static float interval;
    private static float invincibleTimer;
    private static bool isInvincible;
    private static bool gameOngoing;
    private static bool startCounting;
    private static float countingDuration;

    public void Awake()
    {
        LevelSetUp();
    }

    
	
	// Update is called once per frame
	void Update () {
        if (!startCounting)
        {
            if (deathState == 1)
            {
                restartTimer -= Time.deltaTime;
                if (restartTimer <= 0)
                {
                    SceneManager.LoadScene(SceneManager.GetActiveScene().name);
                }
            }
            if (gameOngoing)
            {
                interval -= Time.deltaTime;
                if (interval <= 0)
                {
                    timeNum--;
                    interval = 0.5f;
                    if (timeNum == 99)
                    {
                        audioClips[0].Stop();
                        audioClips[4].Stop();
                        audioClips[6].Play();
                    }
                    else if (timeNum == 0)
                    {
                        playerController.KillPlayer();
                    }
                }
            }

            if (isInvincible)
            {
                invincibleTimer -= Time.deltaTime;
                if (invincibleTimer <= 0)
                {
                    audioClips[4].Stop();
                    audioClips[0].Play();
                    isInvincible = false;
                }
            }

            score.GetComponent<Text>().text = "SCORE\n" + scoreNum.ToString("000000");
            coin.GetComponent<Text>().text = "x " + coinNum.ToString("00");
            time.GetComponent<Text>().text = "TIME\n" + timeNum.ToString("000");
        }
        else
        {
            countingDuration -= Time.deltaTime;
            if (countingDuration <= 0)
            {
                interval -= Time.deltaTime;
                if (interval <= 0 && timeNum > 0)
                {
                    timeNum--;
                    scoreNum += 50;
                    interval = 0.02f;
                    audioClips[7].Play();
                    score.GetComponent<Text>().text = "SCORE\n" + scoreNum.ToString("000000");
                    time.GetComponent<Text>().text = "TIME\n" + timeNum.ToString("000");
                }
            }
        }
        
    }

    public void Win()
    {
        audioClips[0].Stop();
        audioClips[4].Stop();
        audioClips[6].Stop();
        audioClips[5].Play();
        isInvincible = false;
        gameOngoing = false;
    }
    public void Clear()
    {
        audioClips[1].Play();
        startCounting = true;
    }

    public void Die()
    {
        deathState = 1;
        audioClips[0].Stop();
        audioClips[4].Stop();
        audioClips[6].Stop();
        audioClips[3].Play();
    }

    public void Checkpoint(Vector3 position)
    {
        playerPosition = new Vector3 (position.x, position.y, position.z);
    }

    private void LevelSetUp()
    {
        timeNum = 300;
        interval = 0.5f;
        playerController = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
        score = GameObject.FindWithTag("ScoreTex");
        coin = GameObject.FindWithTag("ScoreCoin");
        time = GameObject.FindWithTag("ScoreTime");
        deathState = 0;
        restartTimer = 3.0f;
        audioClips = GetComponents<AudioSource>();
        GameObject.FindWithTag("Player").transform.position = playerPosition;
        invincibleTimer = 13f;
        isInvincible = false;
        gameOngoing = true;
        startCounting = false;
        countingDuration = 1f;
        audioClips[1].Stop();
        audioClips[0].Play();
    }

    public void Score(string s, Vector3 position)
    {
        if (s == "1up")
        {
            Instantiate(oneUp, position, oneUp.transform.rotation);
            audioClips[2].Play();
        }
        else
        {
            for (var i = 0; i < s.Length; i++)
            {
                Vector3 newPosition = new Vector3(position.x + i * numbers[(int)char.GetNumericValue(s[i])].GetComponent<Renderer>().bounds.size.x, position.y, -4);
                Instantiate(numbers[(int)char.GetNumericValue(s[i])], newPosition, numbers[(int)char.GetNumericValue(s[i])].transform.rotation);
            }
            scoreNum += int.Parse(s);
        }      
        
    }

    public void Coin()
    {
        coinNum++;
        if (coinNum == 100) { 
            coinNum = 0;
            audioClips[2].Play();
        }   
    }

    public void Invincible()
    {

        audioClips[0].Stop();
        audioClips[4].Play();
        isInvincible = true;
    }
}
