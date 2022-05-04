using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UILogic : MonoBehaviour
{
    [SerializeField] private Sprite[] muteImages = new Sprite[2];
    
    private GameManager gameManager;
    private GameObject pauseButton;
    private GameObject restartButton;
    private GameObject muteButton;
    private GameObject menu;
    private GameObject scoreText;
    private bool isMuted = false;

    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        scoreText = transform.Find("ScoreText").gameObject;
        menu = transform.Find("Menu").gameObject;
        pauseButton = transform.Find("PauseButton").gameObject;
        restartButton = menu.transform.Find("RestartButton").gameObject;
        muteButton = menu.transform.Find("MuteButton").gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        if (gameManager.started && !pauseButton.activeSelf)
        {
            pauseButton.SetActive(true);
            scoreText.SetActive(true);
        }
    }

    public void PauseButtonPressed()
    {
        gameManager.Pause();

        if (gameManager.isPaused)
        {
            menu.SetActive(true);
        }
        else
        {
            menu.SetActive(false);
        }
    }

    public void RestartButtonPressed()
    {
        gameManager.SetupNewGame();
        menu.SetActive(false);
        gameManager.Pause();
    }


    public void MuteButtonPressed()
    {
        isMuted = !isMuted;
        Image muteImage = muteButton.GetComponent<Image>();

        if (!isMuted)
        {
            AudioListener.volume = 1;
            muteImage.sprite = muteImages[0];
        }
        else
        {
            AudioListener.volume = 0;
            muteImage.sprite = muteImages[1];
        }
    }
}