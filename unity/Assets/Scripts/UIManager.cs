using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public Joystick joystick;

    public GameObject[] titleUI;
    public GameObject[] inGameUI;
    public GameObject[] gameOverUI;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void ClearUI()
    {
        int index;

        for(index = 0; index < inGameUI.Length; ++index)
        {
            inGameUI[index].SetActive(false);
        }

        for (index = 0; index < titleUI.Length; ++index)
        {
            titleUI[index].SetActive(false);
        }
        
        for (index = 0; index < gameOverUI.Length; ++index)
        {
            gameOverUI[index].SetActive(false);
        }
    }

    public void ChangeState(EGameState gameState)
    {
        ClearUI();

        switch (gameState)
        {
            case EGameState.Testing:
                {
                    foreach (GameObject obj in inGameUI)
                    {
                        obj.SetActive(true);
                    }
                }
                break;

            case EGameState.MainMenu:
                {
                    foreach (GameObject obj in titleUI)
                    {
                        obj.SetActive(true);
                    }
                }
                break;

            case EGameState.Playing:
                {
                    foreach (GameObject obj in inGameUI)
                    {
                        obj.SetActive(true);
                    }
                }
                break;

            default:
                break;
        }
    }
}
