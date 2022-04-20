using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlantSpots : MonoBehaviour
{
    public bool IsUsed;
    public GameObject PlantButton;

    //Information from garden manager
    private GameObject gardenmanager;
    private GameObject plant;
    private GameObject watercan;
    private GameObject cam;

    private Vector3 plantOffset = new Vector3(0.1f, 0.2f, 0);
    private Vector3 watercanOffset = new Vector3(0.65f, 2.2f, 0);
    private Vector3 camOffset = new Vector3(0, 2f, -35);

    [HideInInspector] public GameObject ActivePlant;
    [HideInInspector] public GameObject ActiveSpot;


    //VoiceMinigameFunctionality
    [SerializeField] private GameObject voiceMinigameCanvasObject; //Refrence to the voice minigame so that all plants know what it is.

    private void Start()
    {
        gardenmanager = GameObject.Find("GardenManager");
    }

    private void Update()
    {
        if (IsUsed)
        {
            PlantButton.SetActive(true);
        }
        else
            PlantButton.SetActive(false);
    }

    public void PlacePlant()
    {
        plant = Instantiate(ActivePlant, transform.position, transform.rotation);
        plant.transform.position += plantOffset;

        plant.GetComponent<PlantCore>().VoiceMinigameObject = voiceMinigameCanvasObject; //Tells the plant what the voice minigame object is.
        IsUsed = true;
    }

    public void PlaceSanctuaryPlant()
    {
        plant = Instantiate(ActivePlant, transform.position, transform.rotation);
        plant.transform.position += plantOffset;

        IsUsed = true;
    }

    public void InspectPlant()//Switches to the plant management menu
    {
        gardenmanager.GetComponent<GardenManager>().MainPlant = plant;

        gardenmanager.GetComponent<GardenManager>().Spot = this.gameObject;

        watercan = gardenmanager.GetComponent<GardenManager>().WaterCan;
        cam = gardenmanager.GetComponent<GardenManager>().PlantCam.gameObject;

        watercan.transform.position = transform.position + watercanOffset;
        cam.transform.position = transform.position + camOffset;

        gardenmanager.GetComponent<GardenManager>().ChangeUI();
    }

    public void PlantSold()
    {
        if(plant.GetComponent<PlantCore>().FullyGrown)
        {
            CurrencySystem.AffectionTokens++;
        }

        Destroy(plant);
        IsUsed = false;
    }
}
