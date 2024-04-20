using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseHandler : MonoBehaviour
{
    string path;
    public bool paused = false;
    void Start()
    {
        path = Application.persistentDataPath + "/save.json";
    }

    public void OnPause()
    {
        if(FindObjectOfType<PlayerHUD>().isAlive)
        {
            paused = !paused;
            if (paused)
                SceneManager.LoadScene("PauseMenu", LoadSceneMode.Additive);
            else if (!paused)
                SceneManager.UnloadSceneAsync("PauseMenu");
        }
    }

    public void OnSaveOnPause()
    {
        if (FindObjectOfType<FPSController>().equippedGuns.Count == 4)
        {
            SaveData sd = new SaveData();

            sd.playerPosition = FindObjectOfType<FPSController>().transform.position;
            sd.playerHealth = FindObjectOfType<PlayerHUD>().health;
            sd.healthBarFill = FindObjectOfType<PlayerHUD>().healthBar.fillAmount;
            sd.weaponAmmo = FindObjectOfType<Gun>().ammo;
            sd.weaponIndex = FindObjectOfType<FPSController>().gunIndex;

            string jsonText = JsonUtility.ToJson(sd);
            File.WriteAllText(path, jsonText);
            Debug.Log("Sucessfully saved!");
        }
        else
            Debug.Log("You can only save/load the game after collecting all the weapons.");
    }
}
