using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MenuName
{
    Loading,
    TitleMenu,
    Room,
    FindRoom,
    ErrorMenu,
}


public class Menu : MonoBehaviour
{

    public MenuName menuName;
    public bool open;

    public void Open()
    {
        gameObject.SetActive(true);
        open = true;
    }

    public void Close()
    {
        gameObject.SetActive(false);
        open = false;
    }
}
 