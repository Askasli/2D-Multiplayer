using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Pun;
using Photon.Realtime;



public class KillFeedManager : MonoBehaviourPunCallbacks
{

    public static KillFeedManager instance;
    public TMP_Text killFeedText;
    private float alphaAmount;
    public int enableAlphaKillFeed;

    private void Start()
    {
        instance = this;
    }

    private void Update()
    {
        killFeedText.color = new Color(killFeedText.color.r, killFeedText.color.g, killFeedText.color.b, alphaAmount);
        
        if(enableAlphaKillFeed == 1)
        {
            if (alphaAmount < 1)
                alphaAmount += Time.deltaTime * 5f;
        }
        else
        {
            if (alphaAmount > 0)
                alphaAmount -= Time.deltaTime * 5f;
            else
            {
                killFeedText.text = "";
            }
        }

    }
}