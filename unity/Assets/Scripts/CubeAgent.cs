using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Actuators;

using System.Linq;
public class CubeAgent : Agent
{
    public readonly float MINIMUM_JOYSTICK_RANGE = 0.15f;
    public readonly float ROTATE_CUBE_SPEED = 15.0f;

    public readonly float RESUME_IDLE_TIME = 0.1f;
    public readonly float RESUME_JUMP_TIME = 1.0f;

    public readonly float MAX_RAYCAST_LENGTH = 50.0f;

    public Vector3 speed;
    public bool isPlayer { get; set; }

    public int isGround = 0;
    private bool actionReady = false;
    private bool isJump = false;
    private Map map;

    private Rigidbody myRigidbody;
    private Joystick joystick;

    private Vector2 controlSignal;
    private float resumeActionTime;

    private bool isWait = false;

    public int ranMap;

    public int myNum;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {        
        if (Mathf.Abs(controlSignal.x) > MINIMUM_JOYSTICK_RANGE ||
            Mathf.Abs(controlSignal.y) > MINIMUM_JOYSTICK_RANGE)
        {
            transform.rotation =
                Quaternion.Lerp(
                    transform.rotation,
                    Quaternion.LookRotation(new Vector3(controlSignal.x, 0.0f, controlSignal.y)),
                    Time.deltaTime * ROTATE_CUBE_SPEED
                );
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            isJump = true;
        }

        switch (GameManager.instance.currentState)
        {
            case EGameState.Playing:
                {
                    if (actionReady == true)
                    {
                        RequestDecision();
                    }
                }
                break;

            case EGameState.Testing:
                {
                    if (actionReady == true)
                    {
                        RequestDecision();
                    }
                }
                break;

            case EGameState.Learning:
                {
                    if (actionReady == true)
                    {
                        RequestDecision();
                    }
                }
                break;

            default:
                // Not Request Decision
                break;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGround += 1;
        }
        else if(collision.gameObject.CompareTag("Cube"))
        {
            GameManager.instance.hittedNum[myNum]++;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGround -= 1;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Arrival") == true)
        {
            if (GameManager.instance.currentState == EGameState.Learning || GameManager.instance.currentState == EGameState.Testing)
            {
                GameManager.instance.successnum[ranMap] += 1;
                GameManager.instance.PPNum[myNum] += 1;
                SetReward(100.0f);
                EndEpisode();
                OnEpisodeBegin();
            }
            else if (GameManager.instance.currentState == EGameState.Playing && isPlayer == true)
            {
                SetReward(100.0f);
                EndEpisode();
                OnEpisodeBegin();
                GameManager.instance.laserTrans.position = new Vector3(0.0f, 0.0f, -10.0f);
                GameManager.instance.mapList[0].ResetMap();
            }
        }
        else if (other.CompareTag("Laser") == true || other.CompareTag("Out") == true)
        {
            if (GameManager.instance.currentState == EGameState.Learning || GameManager.instance.currentState == EGameState.Testing)
            {
                SetReward(-100.0f);
                EndEpisode();
                OnEpisodeBegin();
            }
            else if (GameManager.instance.currentState == EGameState.Playing)
            {
                if (isPlayer == false)
                {
                    Destroy(gameObject, 3.0f);
                    EndEpisode();
                }
                else
                {
                    EndEpisode();
                    OnEpisodeBegin();
                    GameManager.instance.laserTrans.position = new Vector3(0.0f, 0.0f, -10.0f);
                    GameManager.instance.mapList[0].ResetMap();
                }
            }
        }
    }

    private void FixedUpdate()
    {
        if (isGround > 0 && resumeActionTime > 0.0f && actionReady == false)
        {
            resumeActionTime -= Time.deltaTime;

            if (resumeActionTime <= 0.0f)
            {
                actionReady = true;
            }
        }

        if(transform.position.y < -20.0f)
        {
            SetReward(-100.0f);
            EndEpisode();
            OnEpisodeBegin();
        }
    }
    public void InitializeAgent(bool isPlayer, Map map)
    {
        this.isPlayer = isPlayer;
        this.map = map;

        if (isPlayer == true)
        {
            joystick = GameManager.instance.GetJoystick();
            GameManager.instance.mainCam.chaseTransform = transform;
        }

        gameObject.SetActive(true);

        myRigidbody = gameObject.GetComponent<Rigidbody>();
    }

    public override void OnEpisodeBegin()
    {
        if(myRigidbody == null)
        {
            myRigidbody = gameObject.GetComponent<Rigidbody>();
        }
        myRigidbody.velocity = Vector3.zero;
        myRigidbody.angularVelocity = Vector3.zero;

        if (GameManager.instance.currentState == EGameState.Learning || GameManager.instance.currentState == EGameState.Testing)
        {
            ranMap = Random.Range(0, 6);
            GameManager.instance.trynum[ranMap] += 1;
            GameManager.instance.SSNum[myNum] += 1;
            transform.position = new Vector3((100.0f * ranMap) + Random.Range(-3.0f, 3.0f), 2.0f, Random.Range(-1.0f, 1.0f));
        }

        if (GameManager.instance.currentState == EGameState.Playing && isPlayer == true)
        {
            transform.position = new Vector3(0.0f, 2.0f, 0.0f);
        }

        controlSignal = Vector2.zero;
        resumeActionTime = RESUME_IDLE_TIME;
        //isGround = 0;
        actionReady = false;
        isJump = false;
    }

    public override void CollectObservations(VectorSensor sensor)
    {   
        sensor.AddObservation(this.transform.localEulerAngles.y);
        
        for (int i = 0; i < 5; ++i)
        {
            Vector3 newVec = Quaternion.Euler(transform.localEulerAngles.x, -90.0f + 45.0f * i, 0.0f) * new Vector3(0.0f, 0.0f, 1.0f);
            Vector3 groundVec = Quaternion.Euler(transform.localEulerAngles.x + 25.0f, -90.0f + 45.0f * i, 0.0f) * new Vector3(0.0f, 0.0f, 1.0f);

            //Debug.DrawRay(transform.position + new Vector3(0.0f, 1.0f, 0.0f), groundVec * MAX_RAYCAST_LENGTH, Color.yellow, 1.0f, true);
            RaycastHit hit;

            if (Physics.Raycast(transform.position + new Vector3(0.0f, 1.0f, 0.0f), newVec, out hit, MAX_RAYCAST_LENGTH, 1 << LayerMask.NameToLayer("Obstacle")))
            {
                sensor.AddObservation(hit.distance);
            }
            else
            {
                sensor.AddObservation(MAX_RAYCAST_LENGTH);
            }

            if (Physics.Raycast(transform.position + new Vector3(0.0f, 1.0f, 0.0f), groundVec, out hit, MAX_RAYCAST_LENGTH, 1 << LayerMask.NameToLayer("Ground")))
            {
                sensor.AddObservation(hit.distance);
            }
            else
            {
                sensor.AddObservation(MAX_RAYCAST_LENGTH);
            }
            /*

            if (isPlayer == true)
            {
                Debug.DrawRay(transform.position + new Vector3(0.0f, 1.0f, 0.0f), groundVec * MAX_RAYCAST_LENGTH, Color.yellow, 1.0f, true);
                Debug.DrawRay(transform.position + new Vector3(0.0f, 1.0f, 0.0f), newVec * MAX_RAYCAST_LENGTH, Color.red, 1.0f, true);
            }*/
        }

        Collider[] col = Physics.OverlapSphere(transform.position, 10.0f, 1 << LayerMask.NameToLayer("Cube"));
        Collider[] sortArray = col.OrderBy(x => Vector3.Distance(new Vector3(this.transform.position.x, 0.0f, this.transform.position.z),
                                            new Vector3(x.transform.position.x, 0.0f, x.transform.position.z))).ToArray();

        for (int i = 1; i < 3; ++i)
        {
            if(i >= sortArray.Length)
            {
                sensor.AddObservation(20.0f);
                sensor.AddObservation(20.0f);
            }
            else
            {
                sensor.AddObservation(sortArray[i].transform.position.x - transform.position.x);
                sensor.AddObservation(sortArray[i].transform.position.z - transform.position.z);
            }
        }

        sensor.AddObservation(transform.position.z - GameManager.instance.laserTrans.position.z);
    }

    public override void OnActionReceived(ActionBuffers actionBuffers)
    {
        controlSignal.x = actionBuffers.ContinuousActions[0];
        controlSignal.y = actionBuffers.ContinuousActions[1];

        int choose_action = actionBuffers.DiscreteActions[0];

        if (choose_action == 0)
        {
            Vector3 forwardForce = Vector3.forward * speed.x * controlSignal.y;
            Vector3 rightForce = Vector3.right * speed.z * controlSignal.x;
            Vector3 upForce = Vector3.up * speed.y;

            myRigidbody.AddForce(forwardForce + rightForce + upForce, ForceMode.Impulse);
            
            resumeActionTime = RESUME_IDLE_TIME;
        }
        else
        {
            Vector3 force = new Vector3(
                (transform.localRotation * Vector3.forward).x * speed.x * 2.0f,
                speed.y * 1.5f,
                (transform.localRotation * Vector3.forward).z * speed.z * 2.0f
                );

            myRigidbody.AddForce(force, ForceMode.Impulse);
            resumeActionTime = RESUME_JUMP_TIME;
        }

        isJump = false;
        actionReady = false;
    }

    public override void Heuristic(in ActionBuffers actionsOut)
    {
        if (isPlayer == true)
        {
            var discreteActionsOut = actionsOut.DiscreteActions;
            discreteActionsOut[0] = (isJump == true) ? 1 : 0;

            var continousActionsOut = actionsOut.ContinuousActions;
            continousActionsOut[0] = joystick.Horizontal;
            continousActionsOut[1] = joystick.Vertical;
        }
    }


}
