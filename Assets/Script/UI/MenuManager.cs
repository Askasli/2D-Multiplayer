using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
    [SerializeField] Menu[] menu;
    public static MenuManager instance { get; set; }

    private void Awake()
    {
        instance = this;
    }

    public void OpenMenu(string menuName)
    {
        for (int i = 0; i < menu.Length; i++)
        {
            if(menu[i].MenuName == menuName)
            {
                menu[i].Open();
            }
            else if(menu[i].open)
            {
                CloseMenu(menu[i]);
            }
        }
    }    

    public void OpenMenu(Menu m_menu)
    {
        for (int i = 0; i < menu.Length; i++)
        {
            if(menu[i].open)
            {
                CloseMenu(menu[i]);
            }
        }

        m_menu.Open();
    }
    
    public void CloseMenu(Menu m_menu)
    {
        m_menu.Close();
    }
    
}
