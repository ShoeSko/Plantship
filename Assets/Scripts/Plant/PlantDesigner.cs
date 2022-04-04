using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "New Plant", menuName = "Create Plant")]
public class PlantDesigner : ScriptableObject
{
    #region Variables
    [Header("Plant Looks")]
    [Tooltip("The name the plant will go by")] public string nameOfPlant; //Simple now, could future work add list of potential names to choose from instead.

    //In the scenario plants had different amount of stages, this would be a list instead of 4 separate sprites.
    [Tooltip("The sprite that represent the seed form of the plant")] public Sprite seed;
    [Tooltip("The sprite that represent the first growth stage of the plant")] public Sprite growthStage1;
    [Tooltip("The sprite that represent the second growth stage of the plant")] public Sprite growthStage2;
    [Tooltip("The sprite that represent the fully grown plant")] public Sprite fullyGrown;

    [Header("Plant Stats")]
    [Tooltip("The 3 values the plant need to reach for i'ts progression")] public int[] progressionCostOfPlant = new int[3]; //Gives a list of 3 ints, a range can be added(need to know the wanted values)
    [Tooltip("The tasks that the plant preffers, and will ask for")] public string[] preferencesOfPlant; //Gives a list to implement the preffered tasks, need to know what form the tasks will arrive in before it can be easier to use.
    [Tooltip("The tasks that the plant preffers, and will ask for")] public string[] preferencesOfPlantNoMic; //Gives a list to implement the preffered tasks when Mic is disabled, need to know what form the tasks will arrive in before it can be easier to use.
    
    [Tooltip("The tasks that the plant preffers, and will ask for")] public float PlantWaterConsumption; //Currently expected to be static, but allows for experimentation later

    [Tooltip("The price of the plant when it can be aquired")] public int buyingPriceOfPlant; //Simple int for the price of buyig the plant
    [Tooltip("The price of the plant when it is fully grown and is to be sold")] public List<int> sellingPriceOfPlant = new List<int>(); //Simple int for the price of selling the plant
    #endregion
}