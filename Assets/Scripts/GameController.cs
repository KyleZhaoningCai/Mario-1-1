using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameController : MonoBehaviour {

    private static GameObject item;
    private static GameObject hearts1;
    private static GameObject hearts2;
    private static GameObject hearts3;
    private static GameObject hearts4;
    private static GameObject hearts5;
    private static GameObject[] hearts;
    private static Vector3 itemScale;
    private static Vector3 hideScale;
    private static Vector3 displayScale;
    private static int deathState;
    private static float restartTimer;
    private static PlayerController playerController;
    private static int playerLives = 5;
    private static GameObject infoText;
    private static bool awaitingF3 = false;
    private static Vector3 playerPosition = new Vector3(-7.59f, 1.56f, -2f);
    private static AudioSource[] audioClips;

    public void Awake()
    {
        LevelSetUp();
    }

    
	
	// Update is called once per frame
	void Update () {  
        if (awaitingF3)
        {
            if (Input.GetKey(KeyCode.F3))
            {
                playerLives = 5;
                awaitingF3 = false;
                playerPosition = new Vector3(-7.59f, 1.56f, -2f);
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            }
        }
        else
        {
            if (deathState == 1)
            {
                restartTimer -= Time.deltaTime;
                if (restartTimer <= 0)
                {
                    SceneManager.LoadScene(SceneManager.GetActiveScene().name);
                }
            }
        }        
	}

    // Use this for initialization
    void Start()
    {
    }

    public void GainLoseKey(bool hasKey)
    {
        if (hasKey)
        {
            item.transform.localScale = displayScale;
        }
        else
        {
            item.transform.localScale = hideScale;
        }
    }

    public void Win()
    {
        infoText.GetComponent<Text>().text = "Congratulation! You've successfully found the gold!\nPress 'F3' to play again";
        awaitingF3 = true;
        audioClips[0].Stop();
        audioClips[2].Play();
    }

    public void Die()
    {
        playerLives--;
        hearts[playerLives].transform.localScale = hideScale;
        if (playerLives > 0)
        {
            deathState = 1;
        }
        else
        {
            infoText.GetComponent<Text>().text = "Game Over!\nPress 'F3' to play again";
            awaitingF3 = true;
            audioClips[0].Stop();
            audioClips[1].Play();
        }
    }

    public void Checkpoint(Vector3 position)
    {
        playerPosition = new Vector3 (position.x + 4, position.y, position.z);
    }

    private void LevelSetUp()
    {
        playerController = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
        hearts1 = GameObject.FindWithTag("Heart");
        hearts2 = GameObject.FindWithTag("Heart2");
        hearts3 = GameObject.FindWithTag("Heart3");
        hearts4 = GameObject.FindWithTag("Heart4");
        hearts5 = GameObject.FindWithTag("Heart5");
        infoText = GameObject.FindWithTag("InfoText");
        hearts = new GameObject[] { hearts1, hearts2, hearts3, hearts4, hearts5 };
        item = GameObject.FindWithTag("Item");
        itemScale = item.transform.localScale;
        hideScale = new Vector3(0, 0, 0);
        displayScale = new Vector3(1, 1, 1);
        deathState = 0;
        restartTimer = 3.0f;
        audioClips = GetComponents<AudioSource>();
        GameObject.FindWithTag("Player").transform.position = playerPosition;
        for (int i = 0; i < playerLives; i++)
        {
            hearts[i].transform.localScale = displayScale;
        }
        audioClips[1].Stop();
        audioClips[2].Stop();
        audioClips[0].Play();

    }
}
