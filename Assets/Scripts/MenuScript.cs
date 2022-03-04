using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuScript : MonoBehaviour
{
    public GameObject[] listOfUINamesThingies;
    public bool[] listOfUIBools;

    public void MenuToggle(int currentMenuItemID)
    {
        for (int i = 0; i < listOfUINamesThingies.Length; i++)
        {
            if (i == currentMenuItemID)
            {
                if (listOfUIBools[i])
                {
                    listOfUINamesThingies[i].SetActive(false);
                    listOfUIBools[i] = false;
                }
                else if (!listOfUIBools[i])
                {
                    listOfUINamesThingies[i].SetActive(true);
                    listOfUIBools[i] = true;
                }
            }
            else
            {
                if (listOfUIBools[i])
                {
                    listOfUINamesThingies[i].SetActive(false);
                    listOfUIBools[i] = false;
                }
            }
        }
    }
}
