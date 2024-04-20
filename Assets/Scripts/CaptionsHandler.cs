using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Unity.VisualScripting;

public class CaptionsHandler : MonoBehaviour
{
    [SerializeField] TMP_Text captions;
    int lifetime = 0;
    int fading = 0;
    Color captionsColor = Color.white;

    // Start is called before the first frame update
    void Start()
    {
        captions.color = new Color(captionsColor.r, captionsColor.g, captionsColor.b, 0);
    }
    public void displayCaptions(string text, int solidDuration, int fadingDuration, Color color)
    {
        captions.text = text;
        captionsColor = color;
        captions.color = color;
        lifetime = solidDuration;
        fading = fadingDuration;
        captions.characterSpacing = 6;
    }

    private void FixedUpdate() // It updates 50 times per second.
    {
        if (lifetime > 0)
        {
            lifetime--;
            captions.characterSpacing -= 0.04f;
        }
        else if(lifetime == 0 && fading > 0)
        {
            captions.color = new Color(captionsColor.r,captionsColor.g, captionsColor.b, (float)fading / 100 * 0.99f);
            fading--;
            captions.characterSpacing -= 0.04f;
        }
    }

    public void OnPause()
    {
        captions.color = new Color(captionsColor.r, captionsColor.g, captionsColor.b, 0);
        if (FindObjectOfType<PauseHandler>().paused == true)
        {
            lifetime = 0;
            fading = 0; 
        }
    }
}
