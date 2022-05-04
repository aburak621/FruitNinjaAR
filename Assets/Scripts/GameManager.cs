using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private GameObject Spawner;
    private Spawner spawnerScript;
    private int score = 0;
    private GameObject scoreUI;
    private TextMeshProUGUI scoreText;
    private GameObject audioManagerObject;
    private AudioManager audioManager;
    private GameObject canvas;
    private GameObject startText;
    private UILogic uiLogic;

    public bool isPaused = false;
    public bool started = false;
    private float timer = 0f;
    private bool isYooPlayed = false;
    private float endPauseTimer = 0f;
    private bool tickEndPauseTimer = false;

    // Start is called before the first frame update
    void Start()
    {
        // Cache variables
        audioManagerObject = GameObject.Find("AudioManager");
        audioManager = audioManagerObject.GetComponent<AudioManager>();
        Spawner = GameObject.Find("GameWorld").transform.Find("Spawner").gameObject;
        spawnerScript = Spawner.GetComponent<Spawner>();
        scoreUI = GameObject.Find("Canvas").transform.Find("ScoreText").gameObject;
        scoreText = scoreUI.GetComponent<TextMeshProUGUI>();
        scoreText.text = "0";
        startText = GameObject.Find("StartText");
        uiLogic = GameObject.Find("Canvas").GetComponent<UILogic>();
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= 0.5f && !isYooPlayed)
        {
            audioManager.PlayYooSound();
            isYooPlayed = true;
        }

        if (tickEndPauseTimer)
        {
            endPauseTimer += Time.deltaTime;
            if (endPauseTimer >= 1f)
            {
                tickEndPauseTimer = false;
                uiLogic.PauseButtonPressed();
            }
        }
    }

    public void SetupNewGame()
    {
        GameObject[] allFruits = GameObject.FindGameObjectsWithTag("Fruit");

        for (int i = 0; i < allFruits.Length; i++)
        {
            Destroy(allFruits[i]);
        }
        
        audioManager.PlayGameStartSound();
        // Start spawner
        spawnerScript.StartSpawning();
        // Reset score
        score = 0;
        scoreText.text = score.ToString();
        startText.SetActive(false);
        started = true;
        endPauseTimer = 0f;
        tickEndPauseTimer = false;
    }

    public void GameOver()
    {
        // Stop game
        Debug.Log("GAME OVER!");
        spawnerScript.StopSpawning();
        // Bring up menu
        // Play game over sound
        audioManager.PlayGameOverSound();
        endPauseTimer = 0f;
        tickEndPauseTimer = true;
    }

    public void IncreaseScore()
    {
        score++;
        scoreText.text = score.ToString();
        if (score % 50 == 0)
        {
            audioManager.PlayComboSound();
        }
    }

    public void Pause()
    {
        if (!isPaused)
        {
            isPaused = true;
            Time.timeScale = 0;
        }
        else
        {
            isPaused = false;
            Time.timeScale = 1;
        }
    }
}