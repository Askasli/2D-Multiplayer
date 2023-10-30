using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class KickManager : MonoBehaviour
{
    public TMP_Text coolDownToMenu;
    private float coolDownTo;

    private void Start()
    {
        StartCoroutine(Countdown());
    }

    private IEnumerator Countdown()
    {
        while (coolDownTo > 0)
        {
            coolDownTo -= Time.deltaTime;
            coolDownToMenu.text = coolDownTo.ToString("0");
            yield return null;
        }

        SceneManager.LoadScene(0);
    }
}
