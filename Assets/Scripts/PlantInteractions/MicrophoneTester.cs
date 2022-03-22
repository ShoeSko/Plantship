using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MicrophoneTester : MonoBehaviour
{
    private List<string> listOfDevices = new List<string>();
    [SerializeField] private TextMeshProUGUI[] textDisplay;
    void Start()
    {
        for (int Mic = 0; Mic < Microphone.devices.Length; Mic++)
        {
            listOfDevices.Add(Microphone.devices[Mic]);
            //Debug.Log("Microphone option " + Mic + " is called " + listOfDevices[Mic]);
            textDisplay[Mic].text = "Mic #" + (Mic+1) + " is called " + listOfDevices[Mic];
        }


        Microphone.Start(null, false, 5, 44100);


        //Debug.Log("Mic 1 is " + Microphone.devices[0] + " and the amount of Mics is " + Microphone.devices.Length);
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(Microphone.GetPosition(null));
        Debug.Log(Microphone.IsRecording(null));
    }
}
