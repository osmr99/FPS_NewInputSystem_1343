using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseHandler : MonoBehaviour
{
    bool paused = false;

    public void OnPause()
    {
        paused = !paused;
        if (paused)
            SceneManager.LoadScene("PauseMenu", LoadSceneMode.Additive);
        else if (!paused)
            SceneManager.UnloadSceneAsync("PauseMenu");
    }
}
