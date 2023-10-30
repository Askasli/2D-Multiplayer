using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using TMPro;

public class WonManager : MonoBehaviour
{
    public TMP_Text coolDownMenu;
    private float coolDownTo = 10;

    private void Start()
    {
        StartCoroutine(Countdown());
    }

    private IEnumerator Countdown()
    {
        while (coolDownTo > 0)
        {
            coolDownTo -= Time.deltaTime;
            coolDownMenu.text = coolDownTo.ToString("0");
            yield return null;
        }

        RoomManager.instance.DisconnectPlayer();
    }

    public void ToMenu()
    {
        RoomManager.instance.DisconnectPlayer(); //delete
    }
}