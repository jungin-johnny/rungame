using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map : MonoBehaviour
{
    public GameObject cubePrefab;

    public GameObject playerPrefab;
    public GameObject enemyPrefab;

    public GameObject[] testPrefab;

    private CubeAgent playerAgent;
    private Transform playerTransform = null;

    public GameObject[] mapPrefab;
    private List<GameObject> mapList = new List<GameObject>();

    private int mapLength;
    private int mapRand;

    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if(playerTransform != null && GameManager.instance.currentState == EGameState.Playing &&
            playerTransform.position.z > (mapLength - 1) * 46.0f + 18.0f)
        {
            MakeMap();
        }
    }
    public void ChangeState(EGameState gameState)
    {
        switch (gameState)
        {
            case EGameState.Testing:
                {
                    //MakePlayer(true, new Vector3(0.0f, 1.0f, 0.0f));
                    //
                        MakePlayer(false, new Vector3(0.0f, 1.0f, 0.0f));
                }
                break;

            case EGameState.Learning:
                {
                    for (int i = 0; i < 20; ++i)
                        MakePlayer(false, new Vector3(0.0f, 1.0f, 0.0f));
                }
                break;

            case EGameState.Playing:
                {
                    MakeMap();
                    MakePlayer(true, new Vector3(0.0f, 1.0f, 0.0f));
                }
                break;

            default:
                Debug.Assert(false, "unknown game state");
                break;
        }
    }

    private void MakePlayer(bool isPlayer, Vector3 position)
    {
        if (isPlayer == true)
        {
            GameObject obj = Instantiate(playerPrefab, transform);//, position, Quaternion.identity
            playerAgent = obj.GetComponent<CubeAgent>();
            playerAgent.InitializeAgent(isPlayer, this);

            playerTransform = obj.transform;
        }
        else
        {
            if (GameManager.instance.currentState == EGameState.Testing)
            {
                for (int i = 0; i < 30; ++i)
                {
                    GameObject obj = Instantiate(testPrefab[i/10], transform);//, position, Quaternion.identity
                    obj.transform.position = position;
                    playerAgent = obj.GetComponent<CubeAgent>();
                    playerAgent.InitializeAgent(isPlayer, this);
                }
            }
            else
            {
                GameObject obj = Instantiate(enemyPrefab, transform);//, position, Quaternion.identity
                obj.transform.position = position;
                playerAgent = obj.GetComponent<CubeAgent>();
                playerAgent.InitializeAgent(isPlayer, this);
            }
        }
    }


    private void MakeMap()
    {
        int ranmap = Random.Range(0, mapPrefab.Length);
        //int ranmap = mapRand % mapPrefab.Length;

        for (int i = 0; i < 10; ++i)
        {
            if(ranmap == 4)
                MakePlayer(false, new Vector3(Random.Range(-5.0f, 5.0f), 20.0f, mapLength * 46.0f + Random.Range(0.0f, 46.0f)));
            else
                MakePlayer(false, new Vector3(Random.Range(-5.0f, 5.0f), 3.0f, mapLength * 46.0f + Random.Range(0.0f, 46.0f)));
        }

        for (int i = 0; i < 4; ++i)
            MakePlayer(false, new Vector3(Random.Range(-3.0f, 3.0f), 2.0f, mapLength * 46.0f + Random.Range(-1.0f, 1.0f)));

        GameObject obj = Instantiate(mapPrefab[ranmap], transform);
        obj.transform.localPosition = new Vector3(0.0f, 0.0f, mapLength * 46.0f);
        mapList.Add(obj);

        mapLength++;
    }

    private void MakeTile(float x, float y, float z)
    {
    }

    public void MakeLine(float zPos, bool makeEnemy)
    {
    }

    public void ResetMap()
    {
        mapRand++;

        foreach (GameObject obj in mapList)
        {
            Destroy(obj);
        }

        mapList.Clear();

        mapLength = 0;
        MakeMap();
        /*
        GameManager.instance.score = 0;
        backBlock.localPosition = new Vector3(0.0f, 0.0f, 2.2f * -10);
        foreach (Transform child in transform)
        {
            if (GameManager.instance.isLearning == true)
            {
                if (child.CompareTag("Cube") == false)
                    Destroy(child.gameObject);
            }
            else
            {
                GameObject.Destroy(child.gameObject);
            }
        }

        mapfront = -7.0f;

        for (int i = -9; i < 15; ++i)
        {
            MakeLine(2.2f * i, false);
        }*/
    }

    public void StartGame()
    {
        /*
        if (GameManager.instance.isLearning == true)
        {
            if(playerTransform == null)
            {
                GameObject playerObj = Instantiate(PlayerPrefabs, transform);
                playerObj.transform.position = transform.position;
                playerTransform = playerObj.transform;
                PlayerMovement player_move = playerObj.GetComponent<PlayerMovement>();
                player_move.isPlayer = true;
                player_move.joystick = joystick;
                player_move.myMap = this;
            }

            playerTransform.position = transform.position;
            playerTransform.localEulerAngles = Vector3.zero;
        }
        else
        {
            GameObject playerObj = Instantiate(PlayerPrefabs, transform);
            playerTransform = playerObj.transform;
            PlayerMovement player_move = playerObj.GetComponent<PlayerMovement>();
            
            player_move.isPlayer = true;
            player_move.joystick = joystick;
            player_move.myMap = this;

            playerTransform.position = transform.position;
            playerTransform.localEulerAngles = Vector3.zero;
        }*/
    }
}
