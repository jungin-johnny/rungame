using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum EGameState
{
    MainMenu,
    Playing,
    GameOver,
    Testing,
    Learning
}
public class GameManager : MonoBehaviour
{
    public readonly int LEARNING_STAGE = 10;
    public float recentEscapeTime { get; private set; } = 0.0f;

    public static GameManager instance = null;

    public EGameState currentState;

    public CameraScript mainCam;

    public GameObject mapPrefab;
    public GameObject uiPrefab;

    public List<Map> mapList = new List<Map>();
    private UIManager uiManager = null;

    public Transform laserTrans;

    public int[] hittedNum = new int[3];
    public int[] SSNum = new int[3];
    public int[] PPNum = new int[3];

    public int[] trynum = new int[6];
    public int[] successnum = new int[6];

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(instance);
        }
        else
        {
            if (instance != this)
            {
                Destroy(this.gameObject);
            }
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        GameObject ui = Instantiate(uiPrefab);
        uiManager = ui.GetComponent<UIManager>();

        int numStage = 1;
        /*
        if (currentState == EGameState.Learning)
        {
            numStage = LEARNING_STAGE;
        }*/

        for (int i = 0; i < numStage; ++i)
        {
            GameObject mapObj = Instantiate(mapPrefab);
            mapList.Add(mapObj.GetComponent<Map>());
        }

        SetGameState(currentState);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (recentEscapeTime > 0.0f)
            {
                Application.Quit();
            }
            else
            {
                recentEscapeTime = 0.5f;
            }
        }

        if (recentEscapeTime > 0.0f)
        {
            recentEscapeTime -= Time.deltaTime;
        }
    }
    public void SetGameState(EGameState gameState)
    {
        uiManager.ChangeState(currentState);

        foreach (Map map in mapList)
        {
            map.ChangeState(currentState);
        }
    }

    public Joystick GetJoystick()
    {
        return uiManager.joystick;
    }
}
