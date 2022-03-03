using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlantSpots : MonoBehaviour
{
    public bool IsActive;
    public bool IsUsed;

    private Vector3 offset = new Vector3(0, 2.77f, 0);

    [HideInInspector] public GameObject ActivePlant;

    public void PlacePlant()
    {
        GameObject plant = Instantiate(ActivePlant, transform.position, transform.rotation);
        plant.transform.position += offset;
        IsUsed = true;
    }
}
