using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenuView : MonoBehaviour
{
    public GameObject pauseMenuUI;
    
    public void UpdateUI(bool isPaused)
    {
        pauseMenuUI.SetActive(isPaused);
    }
}
