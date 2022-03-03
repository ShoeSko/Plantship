using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WaterSource : MonoBehaviour
{
    #region Variables
    private float currentWaterStored; //The current amount of water stored
    [SerializeField] private int waterStorageLimit; //Upper limit of water storage (To be increased with the correct upgrades)


    [SerializeField] private Image waterLevelImage; //Image to represent the water level.

    [SerializeField] private float waterRegenerationRate; //The rate water regenerates (Public/static later to be adjusted?)
    [SerializeField] private float regenRateSpeed = 5; //How long(in seconds) between each tic for water regeneration.
    #endregion

    private void Start()
    {
        InvokeRepeating("WaterRegeneration", 0f, regenRateSpeed); //Repeats the water regeneration (Not made to function with idle yet) Repeats according to the repeat speed.
    }
    #region Update
    private void Update()
    {
        WaterLevelRepresentation();
    }
    #endregion


    private void WaterRegeneration()
    {
        if(currentWaterStored < waterStorageLimit)
        {
            currentWaterStored = currentWaterStored + waterRegenerationRate;
        }
    }

    private void WaterLevelRepresentation()
    {
        float waterFillLevel = currentWaterStored / waterStorageLimit; //Gives a value of how much water is stored compared to the max limit.

        waterLevelImage.fillAmount = waterFillLevel; //Changes the water image indicator to portray a level similair to the amount currently stored.
    }


    public void wateringPlant(int amountOfWatering) //Used like this if we want multiple amounts one can water.
    {
        if(amountOfWatering == 1)
        {

        }

        if(amountOfWatering == 2)
        {

        }

        if(amountOfWatering == 3)
        {

        }
    }



    public GameObject[] listOfUINamesThingies;
    public bool[] listOfUIBools;

    public void MenuToggle(int currentMenuItemID)
    {
        for (int i = 0; i < listOfUINamesThingies.Length; i++)
        {
            if(i == currentMenuItemID)
            {
                if (listOfUIBools[i])
                {
                    listOfUINamesThingies[i].SetActive(false);
                    listOfUIBools[i] = false;
                }
                else if (listOfUIBools[i])
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
