using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlantSpots : MonoBehaviour
{
    public bool IsActive;
    public bool IsUsed;
    public GameObject PlantButton;

    private GameObject plant;

    private Vector3 offset = new Vector3(0, 1.5f, 0);

    [HideInInspector] public GameObject ActivePlant;

    private void Update()
    {
        if (IsActive && IsUsed)
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

    public void Buttest()
    {
        Debug.Log("Hey");
        SceneManager.LoadScene("UI Testing");
    }
}
