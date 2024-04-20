using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    string path;
    CaptionsHandler subtitles;
    SaveHandler save;
    PauseHandler pause;
    // Start is called before the first frame update
    void Start()
    {
        path = Application.persistentDataPath + "/save.json";
        subtitles = FindObjectOfType<CaptionsHandler>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnEnable()
    {
        Time.timeScale = 0;
        Cursor.lockState = CursorLockMode.None;
        save = FindObjectOfType<SaveHandler>();
        pause = FindObjectOfType<PauseHandler>();
    }

    void OnDisable()
    {
        Time.timeScale = 1;
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void OnSaveOnPause()
    {
        save.OnSave();
        pause.OnPause();
        
    }

    public void OnLoadOnPause()
    {
        save.OnLoad();
        pause.OnPause();
        
    }

    public void OnDeleteOnPause()
    {
        save.OnDelete();
        pause.OnPause();
        
    }
}
