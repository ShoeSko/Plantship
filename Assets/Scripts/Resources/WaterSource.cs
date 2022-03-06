using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WaterSource : MonoBehaviour
{
    #region Variables
    [HideInInspector] public static float currentWaterStored; //The current amount of water stored
    [SerializeField] private int waterStorageLimit; //Upper limit of water storage (To be increased with the correct upgrades)


    [SerializeField] private Image waterLevelImage; //Image to represent the water level.

    [SerializeField] private float waterRegenerationRate = 5; //The rate water regenerates (Public/static later to be adjusted?)
    [SerializeField] private float regenRateSpeed = 5; //How long(in seconds) between each tic for water regeneration.
    #endregion

    private void Start()
    {
        DontDestroyOnLoad(this.gameObject);
        InvokeRepeating("WaterRegeneration", 0f, regenRateSpeed); //Repeats the water regeneration (Not made to function with idle yet) Repeats according to the repeat speed.
    }
    #region Update
    private void Update()
    {
        if(waterLevelImage != null)
            WaterLevelRepresentation();
    }
    #endregion


    private void WaterRegeneration()
    {
        if(currentWaterStored < waterStorageLimit)
        {
            currentWaterStored += waterRegenerationRate;
        }
    }

    private void WaterLevelRepresentation()
    {
        float waterFillLevel = currentWaterStored / waterStorageLimit; //Gives a value of how much water is stored compared to the max limit.

        waterLevelImage.fillAmount = waterFillLevel; //Changes the water image indicator to portray a level similair to the amount currently stored.
    }


    public void wateringPlant() //Used like this if we want multiple amounts one can water.
    {
        currentWaterStored -= 1; //Decrease water by 1.
        //Debug.Log("Noo my water decreased to " + currentWaterStored);
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
