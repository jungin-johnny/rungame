using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum GameState
{
    MainMenu,
    Playing,
    GameOver,
    Testing
}

public class GameManager : MonoBehaviour
{
    public GameState currentState = GameState.MainMenu;

    public GameObject[] inGameUI;
    public GameObject[] titleUI;
    public GameObject[] gameOverUI;
    public Text scoreText;

    public Map map;
    public Map[] learningMaps;

    public static GameManager instance = null;
    public Transform[] cubesTransform;

    private int backTouch = 0;
    public float score = 0;

    public bool isLearning;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(instance);
        }
        else
        {
            if(instance != this)
            {
                Destroy(this.gameObject);
            }
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        if (currentState == GameState.Testing)
        {
            foreach (GameObject obj in inGameUI)
                obj.SetActive(true);

            return;
        }

        if (isLearning == true)
        {
            SetIngame();
        }
        else
        {
            foreach (GameObject obj in titleUI)
                obj.SetActive(true);

            map.ResetMap();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (isLearning == false)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                backTouch++;
                if (IsInvoking("DoubleBackTouch") == false)
                {
                    Invoke("DoubleBackTouch", 1.0f);
                }
            }
            else if (backTouch == 2)
            {
                CancelInvoke("DoubleBackTouch");
                Application.Quit();
            }
        }

        switch(currentState)
        {
            case GameState.MainMenu:
                break;

            case GameState.Playing:
                score += (Time.deltaTime * 2.0f);
                scoreText.text = "SCORE : " + ((int)score).ToString();
                break;

            case GameState.GameOver:
                break;
        }
    }

    public void SetIngame()
    {
        score = 0;

        for (int i = 0; i < titleUI.Length; ++i)
            titleUI[i].SetActive(false);

        for (int i = 0; i < gameOverUI.Length; ++i)
            gameOverUI[i].SetActive(false);

        for (int i = 0; i < inGameUI.Length; ++i)
            inGameUI[i].SetActive(true);

        //if (currentState == GameState.GameOver)
        if (isLearning == true)
        {
            foreach (Map learnMap in learningMaps)
            {
                learnMap.ResetMap();
                learnMap.StartGame();
            }
        }
        else
        {
            map.ResetMap();
            map.StartGame();
        }
        currentState = GameState.Playing;
    }

    public void SetGameOver()
    {
        for (int i = 0; i < inGameUI.Length; ++i)
            inGameUI[i].SetActive(false);

        for (int i = 0; i < gameOverUI.Length; ++i)
            gameOverUI[i].SetActive(true);

        currentState = GameState.GameOver;
    }

    void DoubleBackTouch()
    {
        backTouch = 0;
    }
}
