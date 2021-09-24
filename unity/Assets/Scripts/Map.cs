using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map : MonoBehaviour
{
    public Joystick joystick;

    //public PlayerMovement 
    public PhysicMaterial physicsMaterial;

    public GameObject PlayerPrefabs;
    public GameObject[] tile;
    public GameObject[] lavaTile;
    public GameObject enemy;
    public GameObject hole;
    public GameObject skull;

    public Transform playerTransform;
    public Transform cameraTransform;
    public Transform backBlock;

    private float mapfront;

    // Start is called before the first frame update
    void Start()
    {
        float temp = Time.time * 100f;
        Random.InitState((int)temp);

        //ResetMap();
    }

    // Update is called once per frame
    void Update()
    {
        if(GameManager.instance.currentState == GameState.Playing)
        {
            backBlock.Translate(0.0f, 0.0f, Time.deltaTime * 2.2f / 0.8f);

            if (playerTransform.position.z > mapfront)
            {
                MakeLine(mapfront + 40.0f, true);
                mapfront += 2.2f;
            }

            if(playerTransform.position.z - backBlock.position.z > 20.0f)
            {
                backBlock.localPosition = new Vector3(0.0f, 0.0f, playerTransform.position.z - 20.0f);
            }
        }
    }

    private void MakeTile(float x, float y, float z)
    {
        GameObject obj = Instantiate(tile[Random.Range(0, tile.Length)], transform);
        obj.tag = "Tile";
        obj.transform.localPosition = new Vector3(x, y, z);
        obj.transform.localScale = new Vector3(2.0f, 2.0f, 2.0f);
        BoxCollider boxCollider = obj.AddComponent<BoxCollider>();
        Rigidbody rigidbody = obj.AddComponent<Rigidbody>();
        rigidbody.isKinematic = true;
    }

    public void MakeLine(float zPos, bool makeEnemy)
    {
        for (int j = -2; j < 3; ++j)
        {
            if (makeEnemy == true)
            {
                float randNum = Random.Range(0.0f, 1.0f);
                if (randNum < 0.05f)
                {
                    GameObject enemy_obj = Instantiate(enemy, transform);
                    enemy_obj.transform.localPosition = new Vector3(2.2f * j, -0.5f, zPos);
                    MakeTile(2.2f * j, -1.6f, zPos);
                }
                else if(randNum < 0.15f)
                {
                    GameObject enemy_obj = Instantiate(hole, transform);
                    enemy_obj.transform.localPosition = new Vector3(2.2f * j, -0.5f, zPos);
                }
                else
                {
                    MakeTile(2.2f * j, -1.6f, zPos);
                }
            }
            else
            {
                MakeTile(2.2f * j, -1.6f, zPos);
            }
        }

        if(makeEnemy == true && (Random.Range(0.0f, 1.0f) < 0.1f))
        {
            //float spd = Random.Range(1.0f, 3.0f);
            float spd = (Random.Range(0, 2)==1) ? -2.5f : 2.5f;

            for (int i = 0; i < 20; ++i)
            {
                GameObject skull_obj = Instantiate(skull, transform);
                skull_obj.transform.localPosition = new Vector3(-60.0f + 6f * i, 0.1f, zPos);
                skull_obj.GetComponent<Skull>().speed = 2.5f;
                Destroy(skull_obj, 20.0f);
            }
        }

        for (int j = -8; j < 8; ++j)
        {
            GameObject obj = Instantiate(lavaTile[Random.Range(0, lavaTile.Length)], transform);

            obj.tag = "Lava";
            obj.transform.localPosition = new Vector3(2.2f * j, -6f, zPos);
            obj.transform.localScale = new Vector3(2.05f, 2.05f, 2.05f);
            Rigidbody rigidbody = obj.AddComponent<Rigidbody>();
            rigidbody.isKinematic = true;
        }
    }

    public void ResetMap()
    {
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
        }
    }

    public void StartGame()
    {
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
        }
    }
}
