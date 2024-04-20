using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using System.IO;
using JetBrains.Annotations;
using System;

public class SaveHandler : MonoBehaviour
{

    string path;
    // Start is called before the first frame update
    void Start()
    {
        path = Application.persistentDataPath + "/save.json";
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnSave()
    {
        if (FindObjectOfType<PlayerHUD>().isAlive)
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
        else
            Debug.Log("You tried saving on the game over, but you realized there's literally no point of doing that.");
    }

    public void OnLoad()
    {
        if (FindObjectOfType<PlayerHUD>().isAlive)
        {
            if(FindObjectOfType<FPSController>().equippedGuns.Count == 4)
            {
                if (File.Exists(path))
                {
                    string saveText = File.ReadAllText(path);
                    SaveData myData = JsonUtility.FromJson<SaveData>(saveText);
                    FindObjectOfType<CharacterController>().enabled = false;

                    FindObjectOfType<FPSController>().transform.position = myData.playerPosition;
                    FindObjectOfType<PlayerHUD>().health = myData.playerHealth;
                    FindObjectOfType<PlayerHUD>().healthBar.fillAmount = myData.healthBarFill;
                    FindObjectOfType<Gun>().ammo = myData.weaponAmmo;
                    FindObjectOfType<Gun>().updateAmmoHUD?.Invoke(myData.weaponAmmo);
                    FindObjectOfType<FPSController>().EquipGun(FindObjectOfType<FPSController>().equippedGuns[myData.weaponIndex]);

                    FindObjectOfType<CharacterController>().enabled = true;
                    Debug.Log("Sucessfully loaded!");
                }
                else
                    Debug.Log("Couldn't find a save file... Save using [Z] or [D-pad Down] on a controller.");
            }
            else
                Debug.Log("You can only save/load the game after collecting all the weapons.");
        }
        else
            Debug.Log("You can't load on the game over :(");
    }

    public void OnDelete()
    {
        if (FindObjectOfType<PlayerHUD>().isAlive)
        {
            if (File.Exists(path))
            {
                File.Delete(path);
                Debug.Log("Save file deleted! Be careful now!!");
            }
            else
                Debug.Log("No save file to delete.");
        }
        else
            Debug.Log("Not a good idea to delete your save right now.");
    }
}
public class SaveData
{
    public Vector3 playerPosition;
    public float playerHealth;
    public float healthBarFill;
    public int weaponIndex;
    public int weaponAmmo;
}

/*if (FindObjectOfType<FPSController>().gunIndex == -1 && myData.weaponIndex > -1)
{
    Debug.Log("Hey, before loading this save, you didn't have any guns. So by default, yours guns won't load here UNLESS you pick them up and THEN save.");
    FindObjectOfType<CharacterController>().enabled = true;
    Debug.Log("On the other hand.. your health and position loaded successfully!");
}
else if (FindObjectOfType<FPSController>().gunIndex > -1 && myData.weaponIndex == -1)
{
    Debug.Log("Hey, you saved with no guns earlier. It's okay though, you get to keep the guns for this one.");
    FindObjectOfType<CharacterController>().enabled = true;
    Debug.Log("And your health and position loaded successfully!");
}
else if (FindObjectOfType<FPSController>().currentGun == null && myData.weaponIndex == -1)
{
    FindObjectOfType<CharacterController>().enabled = true;
    Debug.Log("Sucessfully loaded!");
}*/
//else if((FindObjectOfType<FPSController>().gun)
