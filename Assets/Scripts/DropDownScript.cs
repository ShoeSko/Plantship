using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropDownScript : MonoBehaviour
{
    public GameObject dropdownButton;
    public GameObject topMenu;
    private Vector3 offset = new Vector3(0, -303, 0);
    private bool MenuInteractionDown;

    
    public void DropDownMenu()
    {
         // offset = vector 2
      
        if (!MenuInteractionDown)
        {
            topMenu.transform.position += offset;
            dropdownButton.transform.eulerAngles = new Vector3(0, 0, 180);
            MenuInteractionDown = true;
        }
        else
        {
            topMenu.transform.position -= offset;
            dropdownButton.transform.eulerAngles = new Vector3(0, 0, 0);
            MenuInteractionDown = false;
        }
    }

}
