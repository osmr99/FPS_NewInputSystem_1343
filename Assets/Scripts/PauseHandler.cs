using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseHandler : MonoBehaviour
{
    public bool paused = false;
    void Start()
    {

    }

    public void OnPause()
    {
        if(FindObjectOfType<PlayerHUD>().isAlive)
        {
            paused = !paused;
            if (paused)
                SceneManager.LoadScene("PauseMenu", LoadSceneMode.Additive);
            else if (!paused)
            {
                SceneManager.UnloadSceneAsync("PauseMenu");
                FindObjectOfType<FPSController>().mouseLock = true;
            }    
        }
    }
}
