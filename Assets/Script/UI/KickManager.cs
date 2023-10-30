using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class KickManager : MonoBehaviour
{
    public TMP_Text coolDownToMenu;
    private float coolDownTo;

    // Start is called before the first frame update
    void Start()
    {
        coolDownTo = 10;
    }

    // Update is called once per frame
    void Update()
    {
        coolDownToMenu.SetText(coolDownTo.ToString("0"));

        if (coolDownTo > 0)
        {
            coolDownTo -= Time.deltaTime;
        }
        else
        {
            SceneManager.LoadScene(0);
        }
    }

    public void ToMenu()
    {
        SceneManager.LoadScene(0);
    }
}
