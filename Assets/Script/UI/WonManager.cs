using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using TMPro;

public class WonManager : MonoBehaviour
{
    public TMP_Text coolDownMenu;
    private float coolDownTo;

    // Start is called before the first frame update
    void Start()
    {
        coolDownTo = 10;
    }

    // Update is called once per frame
    void Update()
    {
        coolDownMenu.SetText(coolDownTo.ToString("0"));

        if (coolDownTo > 0)
        {
            coolDownTo -= Time.deltaTime;
        }
        else
        {
            RoomManager.instance.DisconnectPlayer();
        }
    }

    public void ToMenu()
    {
        RoomManager.instance.DisconnectPlayer();
    }
}