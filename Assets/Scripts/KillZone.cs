using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillZone : MonoBehaviour
{
    [SerializeField] AudioSource sound;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        sound?.Play();
        var enemy = collision.gameObject.GetComponent<Enemy>();
        if (enemy != null)
            enemy.Respawn();
    }
}
