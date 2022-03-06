using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlantSpots : MonoBehaviour
{
    public bool IsUsed;
    public GameObject PlantButton;

    private GameObject plant;

    private Vector3 offset = new Vector3(0, 1.5f, 0);

    [HideInInspector] public GameObject ActivePlant;

    private void Update()
    {
        if (IsUsed)
        {
            PlantButton.SetActive(true);
        }
    }

    public void PlacePlant()
    {
        plant = Instantiate(ActivePlant, transform.position, transform.rotation);
        plant.transform.position += offset;
        IsUsed = true;
    }

    public void Buttest()//Switches to the plant management menu
    {
        SceneManager.LoadScene("UI Testing");
    }
}
