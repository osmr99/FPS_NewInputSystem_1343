using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pursueTrigger : MonoBehaviour
{
    Enemy enemy;

    void Start()
    {
        enemy = GetComponentInParent<Enemy>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out FPSController FPSController) && enemy.inRange == false && enemy.currentState != MovingObjectStates.recovering)
        {
            enemy.currentState = MovingObjectStates.pursue;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.TryGetComponent(out FPSController FPSController) && enemy.inRange == false && enemy.currentState != MovingObjectStates.recovering)
        {
            enemy.elapsed_two = 0;
            enemy.currentState = MovingObjectStates.pursue;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out FPSController FPSController) && enemy.inRange == false && enemy.currentState != MovingObjectStates.recovering)
        {
            enemy.elapsed_two = 0;
            enemy.currentState = MovingObjectStates.wander;
        }
    }
}
