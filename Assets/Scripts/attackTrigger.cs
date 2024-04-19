using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class attackTrigger : MonoBehaviour
{
    Enemy enemy;

    void Start()
    {
        enemy = GetComponentInParent<Enemy>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out FPSController FPSController) && enemy.currentState != MovingObjectStates.recovering)
        {
            enemy.inRange = true;
            enemy.currentState = MovingObjectStates.attack;
            enemy.elapsed_three = 0;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out FPSController FPSController) && enemy.currentState != MovingObjectStates.recovering)
        {
            enemy.inRange = false;
            enemy.elapsed_two = 0;
            enemy.currentState = MovingObjectStates.pursue;
        }
    }
}
