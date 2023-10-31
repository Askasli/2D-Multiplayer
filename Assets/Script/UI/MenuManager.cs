using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
    [SerializeField] Menu[] menus;
    public static MenuManager instance { get; set; }

    private void Awake()
    {
        instance = this;
    }

    public void OpenMenu(MenuName menuName)
    {
        for (int i = 0; i < menus.Length; i++)
        {
            if (menus[i].menuName == menuName)
            {
                menus[i].Open();
            }
            else if (menus[i].open)
            {
                CloseMenu(menus[i]);
            }
        }
    }

    public void OpenMenu(Menu m_menu)
    {
        for (int i = 0; i < menus.Length; i++)
        {
            if (menus[i].open)
            {
                CloseMenu(menus[i]);
            }
        }

        m_menu.Open();
    }

    public void CloseMenu(Menu m_menu)
    {
        m_menu.Close();
    }
    
}
