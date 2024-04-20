using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Threading;

public class Fader : MonoBehaviour
{
    [SerializeField] PlayerHUD hud;
    [SerializeField] Color targetColor;
    [SerializeField] Image wholeScreen;
    [SerializeField] float fadeSpeed;
    bool damage = false;
    bool spawnTransition = true;
    bool reset = false;
    bool gameOver = false;

    // Start is called before the first frame update
    void Start()
    {
        fadeSpeed = 0.9f;
        targetColor = Color.black;
    }

    // Update is called once per frame
    void Update()
    {
        var currentColor = wholeScreen.color;

        //Debug.Log(currentColor.a);
        if (spawnTransition && currentColor.a > -0.1)
        {
            currentColor.a -= fadeSpeed * Time.deltaTime;
            if (currentColor.a <= -0.1)
                spawnTransition = false;
        }
        if (damage && currentColor.a <= 0.5)
        {
            currentColor = Color.Lerp(currentColor, targetColor, fadeSpeed * Time.deltaTime);
        }
        if (damage && currentColor.a > 0.5)
        {
            damage = false;
            reset = true;
        }
        if (reset && currentColor.a > -0.1 && gameOver == false)
        {
            currentColor.a -= fadeSpeed * Time.deltaTime;
            if (currentColor.a <= -0.1)
                reset = false;
        }
        if (gameOver)
        {
            targetColor = Color.black;
            currentColor = Color.Lerp(currentColor, targetColor, fadeSpeed * Time.deltaTime);
            Cursor.lockState = CursorLockMode.None;
        }


        //currentColor.a = fadeSpeed * Time.deltaTime;

        wholeScreen.color = currentColor;

        //if (targetColor == Color.red && targetColor.a > 0.5f)
        //{
        //setToTransparent();
        //}
    }

    public void tookDamage()
    {
        targetColor = Color.red;
        fadeSpeed = 6f;
        damage = true;
    }

    public void receivedGameOver()
    {
        targetColor = Color.black;
        fadeSpeed = 10f;
        reset = false;
        gameOver = true;
    }
}
