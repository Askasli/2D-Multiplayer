using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DashCoolDown : MonoBehaviour
{
    public static DashCoolDown instance;
    private TMP_Text coolDown;
    public TMP_Text dashText;
    public Image circleIcon;
    public Image galImage;
    public float timerToZero;
    private float backGroundAlpha;
    //private TMPro coolDown;


    // Start is called before the first frame update
    void Awake()
    {
        instance = this;
        coolDown = GetComponent<TMP_Text>();
    }

    // Update is called once per frame
    void Update()
    {
        
        coolDown.color = new Color(coolDown.color.r, coolDown.color.g, coolDown.color.b, backGroundAlpha);
        dashText.color = new Color(dashText.color.r, dashText.color.g, dashText.color.b, backGroundAlpha);
        circleIcon.color = new Color(circleIcon.color.r, circleIcon.color.g, circleIcon.color.b, backGroundAlpha);

        coolDown.SetText(timerToZero.ToString("0"));

        if (timerToZero <= 1)
        {
            if (backGroundAlpha > 0)
            {
                backGroundAlpha -= Time.deltaTime * 2f;
            }
        }
        else
        {
            if (backGroundAlpha < 1)
                backGroundAlpha += Time.deltaTime * 5f;
        }

       // coolDown.text = timerToZero.ToString("0");
    }
}
