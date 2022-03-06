using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuScript : MonoBehaviour
{
    public GameObject[] listOfMenues;
    public bool[] listOfUIBools;

    public void MenuToggle(int currentMenuItemID)
    {
        for (int i = 0; i < listOfMenues.Length; i++)
        {
            if (i == currentMenuItemID)
            {
                if (listOfUIBools[i])
                {
                    listOfMenues[i].SetActive(false);
                    listOfUIBools[i] = false;
                }
                else if (!listOfUIBools[i])
                {
                    listOfMenues[i].SetActive(true);
                    listOfUIBools[i] = true;
                }
            }
            else
            {
                if (listOfUIBools[i])
                {
                    listOfMenues[i].SetActive(false);
                    listOfUIBools[i] = false;
                }
            }
        }
    }
}
