using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WaterSource : MonoBehaviour
{
    #region Variables
    [HideInInspector] public static float currentWaterStored; //The current amount of water stored
    [HideInInspector] public static float WaterCap = 800; //The current amount of water stored
    [SerializeField] private int waterStorageLimit; //Upper limit of water storage (To be increased with the correct upgrades)


    [SerializeField] private Image waterLevelImage; //Image to represent the water level.

    private float waterRegenerationRate; //The rate water regenerates (Public/static later to be adjusted?)
    private float regenRateSpeed = 10; //How long(in seconds) between each tic for water regeneration. (this should later be 10 minutes)

    public GameObject WaterObject;
    private Slider WaterSlider;
    #endregion

    private void Start()
    {
        DontDestroyOnLoad(this.gameObject);
        InvokeRepeating("WaterRegeneration", 0f, regenRateSpeed); //Repeats the water regeneration (Not made to function with idle yet) Repeats according to the repeat speed.

        WaterSlider = WaterObject.GetComponent<Slider>();
        WaterSlider.maxValue = WaterCap;
        WaterSlider.value = currentWaterStored;
    }
    #region Update
    private void Update()
    {
        waterRegenerationRate = WaterCap * 0.06f;//always regenerates 6% of water cap (putting this here makes sure it gets updated automatically)

        if(waterLevelImage != null)
            WaterLevelRepresentation();

        WaterSlider.value = currentWaterStored;
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
        currentWaterStored -= 1; //Decrease water in water can by 6 units.
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
