using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;


using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Actuators;


public class PlayerMovement : Agent
{
    public Vector3 Speed;
    public Joystick joystick;

    public bool isPlayer;
    public Ease testEase;

    private float resumeTime = 0.0f;
    public Rigidbody myRigid;

    private float isJump = 0.0f;

    public float TestRay = 0.0f;
    public string TestTag = "";

    public Map myMap;

    public Vector2 controlSignal = Vector2.zero;

    private float touchTime = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        //myRigid = GetComponentInChildren<Rigidbody>();
    }

    public override void OnEpisodeBegin()
    {
        if(GameManager.instance.isLearning == true){
            myMap.ResetMap();
            myMap.StartGame();
        }
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        sensor.AddObservation(this.transform.localPosition.x);
        sensor.AddObservation(isJump);

        for (int i = 0; i < 12; ++i){
            Vector3 newVec = Quaternion.Euler(0f, 0 + i * 30f, 0f) * new Vector3(0.0f, 0.0f, 1.0f);
            Debug.DrawRay(transform.position, newVec * 50.0f, Color.yellow);
            RaycastHit hit;

            if (Physics.Raycast(transform.position, newVec, out hit, 50.0f)){
                sensor.AddObservation(hit.distance);
                if (hit.transform.CompareTag("Enemy")){
                    sensor.AddObservation(1.0f);
                }
                else if(hit.transform.CompareTag("Hole")){
                    sensor.AddObservation(2.0f);
                }
                else if (hit.transform.CompareTag("BackBlock"))
                {
                    sensor.AddObservation(3.0f);
                }
                else
                {
                    sensor.AddObservation(0.0f);
                }
            }
            else
            {
                sensor.AddObservation(50.0f);
                sensor.AddObservation(0.0f);
            }
        }
    }

    public override void OnActionReceived(ActionBuffers actionBuffers)
    {
        if (GameManager.instance.currentState == GameState.Playing)
        {
            if (isPlayer == true)
            {
                controlSignal.x = actionBuffers.ContinuousActions[0];
                controlSignal.y = actionBuffers.ContinuousActions[1];

                int choose_action = actionBuffers.DiscreteActions[0];

                if (isJump <= 0.0f)
                {
                    transform.localEulerAngles = new Vector3(0.0f, transform.localEulerAngles.y, transform.localEulerAngles.z);

                    if (Mathf.Abs(controlSignal.x) + Mathf.Abs(controlSignal.y) > 0.1f)
                    {
                        transform.eulerAngles = new Vector3(0, Mathf.Atan2(controlSignal.x, controlSignal.y) * Mathf.Rad2Deg, 0);
                        transform.Translate(controlSignal.x * Speed.x * Time.deltaTime, 0.0f, controlSignal.y * Speed.z * Time.deltaTime, Space.World);
                    }

                    AddReward(1f * Time.deltaTime);

                    if (choose_action == 1)
                    {
                        Jump();
                    }
                }
            }
        }
    }

    public override void Heuristic(in ActionBuffers actionsOut)
    {
        if (isPlayer == true)
        {
            var discreteActionsOut = actionsOut.DiscreteActions;
            var continousActionsOut = actionsOut.ContinuousActions;
            continousActionsOut[0] = joystick.Horizontal;
            continousActionsOut[1] = joystick.Vertical;            
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Cube"))
        {
            //Vector3 col_velocity = collision.rigidbody.velocity;
            //myRigid.AddForce(col_velocity.x * 2.0f, 0.0f, col_velocity.z * 2.0f, ForceMode.Impulse);
            //myRigid.AddForce(collision.relativeVelocity * 1.1f, ForceMode.Impulse);
        }
        else if (collision.gameObject.CompareTag("Enemy"))
        {
            if(GameManager.instance.isLearning == true)
            {
                AddReward(-10.0f);
                EndEpisode();
            }
            else
            {
                myRigid.AddForce(new Vector3(0.0f, 30.0f, -30.0f), ForceMode.Impulse);
                myRigid.AddTorque(new Vector3(Random.Range(-250.0f, 250.0f),
                           Random.Range(-250.0f, 250.0f), Random.Range(-250.0f, 250.0f)));
                GameManager.instance.SetGameOver();
                isPlayer = false;
                Destroy(gameObject, 5.0f);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            if (GameManager.instance.isLearning == true)
            {
                AddReward(-10.0f);
                EndEpisode();
            }
            else
            {
                GameManager.instance.SetGameOver();
                isPlayer = false;
                Destroy(gameObject, 5.0f);
            }
        }
    }


    // Update is called once per frame
    void Update()
    {
        if (GameManager.instance.currentState == GameState.Playing ||
            GameManager.instance.currentState == GameState.Testing){
            if (isPlayer == true){

                touchTime += Time.deltaTime;

                if (joystick == null && GameManager.instance.isLearning == false){
                    joystick = GameManager.instance.map.joystick;
                    return;
                }

                if (Input.GetKeyDown(KeyCode.Space))
                {
                    Jump();
                    //discreteActionsOut[0] = 1;
                }

                if (Input.touchCount > 0)
                {
                    Touch touch = Input.GetTouch(0);

                    if (touch.phase == TouchPhase.Began)
                    {
                        touchTime = 0.0f;
                    }
                    else if (touch.phase == TouchPhase.Ended && touchTime < 0.3f)
                    {
                        Jump();// discreteActionsOut[0] = 1;
                    }
                }

                //if(discreteActionsOut[0] != 1)
                //    discreteActionsOut[0] = 0;

                GameManager.instance.map.cameraTransform.position = new Vector3(0.0f, 16.7f, -10.7f + transform.position.z);
            }

            if (isJump > 0.0f)
            {
                isJump -= Time.deltaTime;
                if (isJump <= 0.0f)
                {
                    isJump = 0.0f;
                }
            }
        }
    }

    private void Jump()
    {
        isJump = 2.5f;
        //myRigid.velocity = new Vector3(0.0f, 0.0f, myRigid.velocity.z);
        myRigid.AddTorque(transform.right * 45.0f);
        myRigid.AddForce(transform.forward * (8.0f - transform.position.y) + transform.up * (8.0f - transform.position.y), ForceMode.Impulse);
    }

    private void FixedUpdate()
    {
        //resumeTime -= Time.deltaTime;
        
        if (GameManager.instance.currentState == GameState.Playing)
        {
            
        }
    }
}
