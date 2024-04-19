using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements;

public enum MovingObjectStates
{
    wander,
    pursue,
    attack,
    recovery,
    recovering
}

public class Enemy : MonoBehaviour
{
    [SerializeField] public MovingObjectStates currentState;
    Vector3 startingLocation;
    [SerializeField] GameObject targetLocation;
    [SerializeField] float wanderRange;
    float wanderInterval;
    float recoveryTime = 3.5f;
    float attackTime = 1;
    float hurtTime = 0.5f; // Whenever the enemy gets damaged, they enter a stunt state for 
                          //  this amount of seconds.
    NavMeshAgent agent;

    public Rigidbody Rigidbody { get; private set; }
    Vector3 origin;
    float elapsed_one = 0;
    public float elapsed_two = 0;
    public float elapsed_three = 0;
    float elapsed_four = 1;
    public bool inRange = false;

    // Start is called before the first frame update
    void Start()
    {
        Rigidbody = GetComponent<Rigidbody>();
        agent = GetComponent<NavMeshAgent>();
        origin = transform.position;
        startingLocation = origin;
        agent.SetDestination(startingLocation);
        currentState = MovingObjectStates.wander;
        //agent.SetDestination(new Vector3(0, 0, 0));
    }

    // Update is called once per frame
    void Update()
    {
        elapsed_one += Time.deltaTime;
        elapsed_two += Time.deltaTime;
        elapsed_three += Time.deltaTime;
        elapsed_four += Time.deltaTime;

        if (elapsed_one > hurtTime && Rigidbody.isKinematic == false)
        {
            Rigidbody.isKinematic = true;
            agent.enabled = true;
        }


        switch (currentState)
        {
            case MovingObjectStates.wander:
                startWander();
                break;

            case MovingObjectStates.pursue:
                startPursue();
                break;

            case MovingObjectStates.attack:
                startAttack();
                break;

            case MovingObjectStates.recovery:
                startRecovery();
                break;
            case MovingObjectStates.recovering:
                startRecovering();
                break;

        }

        if (elapsed_three > attackTime && currentState == MovingObjectStates.attack)
        {
            currentState = MovingObjectStates.recovery;
        }

        if (elapsed_four > recoveryTime && currentState == MovingObjectStates.recovering)
        {
            agent.speed = 3;
            agent.acceleration = 8;
            currentState = MovingObjectStates.wander;
        }
            
    }

    public void ApplyKnockback(Vector3 knockback)
    {
        agent.enabled = true;
        Rigidbody.isKinematic = false;
        Rigidbody.useGravity = false;
        elapsed_one = 0;
        GetComponent<Rigidbody>().AddForce(knockback, ForceMode.Impulse);
    }

    public void Respawn()
    {
        transform.position = origin;
        Rigidbody.AddForce(Vector3.zero, ForceMode.Impulse);
        Rigidbody.velocity = Vector3.zero;

    }

    Vector3 Wander()
    {
        Vector3 offset = new Vector3(Random.Range(-wanderRange, wanderRange), 0, Random.Range(-wanderRange, wanderRange));

        NavMeshHit hit;

        bool gotPoint = NavMesh.SamplePosition(offset, out hit, 1, NavMesh.AllAreas);

        if (gotPoint)
        {
            return hit.position;
        }
        else
        {
            return startingLocation;
        }
    }

    void randomWanderRate()
    {
        wanderInterval = Random.Range(2, 3);
    }

    void startWander()
    {
        if (elapsed_two > wanderInterval)
        {
            agent.speed = 3;
            agent.acceleration = 8;
            agent.SetDestination(Wander());
            randomWanderRate();
            elapsed_two = 0;
        }
    }

    void startPursue()
    {
        agent.speed = 3;
        agent.acceleration = 8;
        agent.SetDestination(targetLocation.transform.position);
    }

    void startAttack()
    {
        agent.speed = 100;
        agent.acceleration = 100;
        Vector3 attackLocation = targetLocation.transform.position + Vector3.forward;
        //agent.SetDestination(attackLocation);
        agent.SetDestination(targetLocation.transform.position);
    }

    void startRecovery()
    {
        inRange = false;
        elapsed_four = 0;
        agent.speed = 0;
        currentState = MovingObjectStates.recovering;
    }

    void startRecovering()
    {
        // This is just an empty placeholder.
    }
}
