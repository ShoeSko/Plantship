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

    private Vector3 plantOffset = new Vector3(0, 1.5f, 0);
    private Vector3 watercanOffset = new Vector3(0.65f, 3.61f, 0);
    private Vector3 camOffset = new Vector3(0, 2.39f, -35);

    [HideInInspector] public GameObject ActivePlant;

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
        IsUsed = true;
    }

    public void InspectPlant()//Switches to the plant management menu
    {
        gardenmanager.GetComponent<GardenManager>().MainPlant = plant;

        watercan = gardenmanager.GetComponent<GardenManager>().WaterCan;
        cam = gardenmanager.GetComponent<GardenManager>().PlantCam.gameObject;

        watercan.transform.position = transform.position + watercanOffset;
        cam.transform.position = transform.position + camOffset;

        gardenmanager.GetComponent<GardenManager>().ChangeUI();
    }
}
