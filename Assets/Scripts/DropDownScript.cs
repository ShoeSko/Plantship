using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropDownScript : MonoBehaviour
{
    public GameObject dropdownButton;
    public GameObject topMenu;
    private Vector3 offset = new Vector3(0, -303, 0);
    private bool MenuInteractionDown;

    public bool AlternativeOffset;

    public AudioSource SFXplayer;
    public AudioClip SlideDown;
    public AudioClip SlideUp;

    
    public void DropDownMenu()
    {
        if (AlternativeOffset)
            offset = new Vector3(0, -1, 0);

        if (!MenuInteractionDown)
        {
            topMenu.transform.position += offset;
            dropdownButton.transform.eulerAngles = new Vector3(0, 0, 180);
            SFXplayer.clip = SlideDown;
            SFXplayer.Play();
            MenuInteractionDown = true;
        }
        else
        {
            topMenu.transform.position -= offset;
            dropdownButton.transform.eulerAngles = new Vector3(0, 0, 0);
            SFXplayer.clip = SlideUp;
            SFXplayer.Play();
            MenuInteractionDown = false;
        }
    }

}
