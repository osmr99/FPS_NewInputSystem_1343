using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ammo : MonoBehaviour
{
    [SerializeField] FPSController controller;
    public void OnReload()
    {
        controller.IncreaseAmmo(999);
    }
}
