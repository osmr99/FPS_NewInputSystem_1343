using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;

public class PlayerHUD : MonoBehaviour
{
    FPSController player;
    [SerializeField] public Image healthBar;
    [SerializeField] Image heart;
    [SerializeField] TMP_Text currentAmmoText;
    [SerializeField] TMP_Text maxAmmoText;
    [SerializeField] public float health = 100;
    float maxHealth = 100;

    int ammo = 10;
    int maxAmmo = 10;
    bool isAlive = true;

    public UnityEvent gameOver;
    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<FPSController>();
        currentAmmoText.text = ammo.ToString();
        maxAmmoText.text = maxAmmo.ToString();
    }

    public void updateAmmo(int bullets)
    {
        if (isAlive)
            currentAmmoText.text = bullets.ToString();
    }

    public void updateHealth(int value)
    {
        health -= value;
        healthBar.fillAmount = (float)health / maxHealth;
        if (health <= 0)
        {
            isAlive = false;
            gameOver.Invoke();
        }
        else if(health > maxHealth)
        {
            health = maxHealth;
        }
    }

    public void OnFullHeal()
    {
        updateHealth((int)-maxHealth);
    }
}
