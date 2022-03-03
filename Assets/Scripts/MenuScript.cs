using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuScript : MonoBehaviour
{
    public bool shopMenuActive;
    public bool fertMenuActive;
    public bool collectMenuActive;

    public GameObject shopMenu;
    public GameObject fertMenu;
    public GameObject collectMenu;
    public GameObject achievMenu;
    public GameObject settingsMenu;

    public void shopMenuToggle()
    {
        if (shopMenuActive)
        {
            shopMenu.SetActive(false);
            shopMenuActive = false;
        }
        else
        {
            shopMenu.SetActive(true);
            shopMenuActive = true;
        }
    }

    public void fertMenuToggle()
    {
        if (fertMenuActive)
        {
            fertMenu.SetActive(false);
            fertMenuActive = false;
        }
        else
        {
            fertMenu.SetActive(true);
            fertMenuActive = true;
        }

    }  public void collectMenuToggle()
    {
        if (collectMenuActive)
        {
            collectMenu.SetActive(false);
            collectMenuActive = false;
        }
        else
        {
            collectMenu.SetActive(true);
            collectMenuActive = true;
        }
    }

}
