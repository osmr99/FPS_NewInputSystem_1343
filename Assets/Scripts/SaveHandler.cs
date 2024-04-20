using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using System.IO;
using JetBrains.Annotations;

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
        if (Keyboard.current.zKey.wasPressedThisFrame)
        {
            SaveData sd = new SaveData();

            sd.playerPosition = FindObjectOfType<FPSController>().transform.position;
            sd.playerHealth = FindObjectOfType<PlayerHUD>().health;
            sd.healthBarFill = FindObjectOfType<PlayerHUD>().healthBar.fillAmount;

            string jsonText = JsonUtility.ToJson(sd);
            File.WriteAllText(path, jsonText);
            Debug.Log("Sucessfully saved!");
        }

        if (Keyboard.current.xKey.wasPressedThisFrame)
        {
            string saveText = File.ReadAllText(path);
            SaveData myData = JsonUtility.FromJson<SaveData>(saveText);
            FindObjectOfType<CharacterController>().enabled = false;

            FindObjectOfType<FPSController>().transform.position = myData.playerPosition;
            FindObjectOfType<PlayerHUD>().health = myData.playerHealth;
            FindObjectOfType<PlayerHUD>().healthBar.fillAmount = myData.healthBarFill;

            FindObjectOfType<CharacterController>().enabled = true;
            Debug.Log("Sucessfully loaded!");
        }
    }
}
public class SaveData
{
    public Vector3 playerPosition;
    public float playerHealth;
    public float healthBarFill;
}
