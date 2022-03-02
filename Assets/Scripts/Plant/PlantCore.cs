using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(SpriteRenderer))] //Makes sure the gameobject has the required components.
public class PlantCore : MonoBehaviour
{
    #region Preset Variables
    public PlantDesigner plantTemplate;

    [Header("Plant Looks")]
    [Tooltip("The name the plant will go by")] private string _nameOfPlant; //Simple now, could future work add list of potential names to choose from instead.

    //In the scenario plants had different amount of stages, this would be a list instead of 4 separate sprites.
    [Tooltip("The sprite that represent the seed form of the plant")] private Sprite _seed;
    [Tooltip("The sprite that represent the first growth stage of the plant")] private Sprite _growthStage1;
    [Tooltip("The sprite that represent the second growth stage of the plant")] private Sprite _growthStage2;
    [Tooltip("The sprite that represent the fully grown plant")] private Sprite _fullyGrown;

    [Header("Plant Stats")]
    [Tooltip("The 3 values the plant need to reach for i'ts progression")] private int[] _progressionCostOfPlant = new int[3]; //Gives a list of 3 ints, a range can be added(need to know the wanted values)
    [Tooltip("The tasks that the plant preffers, and will ask for")] private string[] _preferencesOfPlant; //Gives a list to implement the preffered tasks, need to know what form the tasks will arrive in before it can be easier to use.

    [Tooltip("The price of the plant when it can be aquired")] private int _buyingPriceOfPlant; //Simple int for the price of buyig the plant
    [Tooltip("The price of the plant when it is fully grown and is to be sold")] private int _sellingPriceOfPlant; //Simple int for the price of selling the plant
    #endregion
    #region Other Variables
    private SpriteRenderer _spriteOfPlant; //Refrence to the plants spriterenderer.

    private int _growthStageAmount = 3; //Sets a definit value on the amount of stages a plant transforms into. Seed is the base and turns into = (stage 1, stage 2 & Fully grown)

    [SerializeField] private int _currentPlantProgressionValue; //!!Serialized for Testing!! The value of the plants current progression. (can be used to determine growth stage) 
    private int _growthStage; //The value representation of the current stage (0 = seed, 3 = fully grown)
    #endregion

    #region Awake / Start
    private void Awake()
    {
        //First Priority
        PlantInfoFeed(); //Run first to update all of the plants stats to fit the given template.

        //Second Priority
        PlantSpriteChange(); //Run early to update the sprite of the plant to the current stage.  (Also to be run when a progression goal is reached/Milestone)
    }

    private void Start()
    {
        
    }
    #endregion
    #region Update
    private void Update()
    {

    }
    #endregion


    #region Sprite changing
    private void PlantSpriteChange()
    {
        _spriteOfPlant = GetComponent<SpriteRenderer>(); //Makes sure the spriterenderer being used is the gameobjects own.

        Sprite[] growthStages = new Sprite[] { _growthStage1, _growthStage2, _fullyGrown }; //Creates a list of all the growth stages. (Hardcoded for now)

        for (int stage = 0; stage < _growthStageAmount; stage++) //Loop to go through all the 3 stage increase values to check for growth.
        {
            if(_progressionCostOfPlant[stage] <= _currentPlantProgressionValue) //If the plant has reached this stage, do it.
            {
                _spriteOfPlant.sprite = growthStages[stage]; //Since the plant has reached the stage, then change the sprite to the appropriate stage. 


                _growthStage = stage + 1; //sets the growth stage of the plant to it's appropriate value (Later will be a saved value? Prevent unlimited milestones.) !!!Move location late?
            }
            else if(_spriteOfPlant.sprite == null) //If it has not been given a sprite, make it a seed. (Takes effect if it has not reached stage 1 one)
            {
                _spriteOfPlant.sprite = _seed; //Sprite is now a seed.
            }
        }

        Debug.Log("Current stage is = " + _growthStage + "and current sprite image is = " + _spriteOfPlant.sprite.name);
    }
    #endregion
    #region Setting up plant
    private void PlantInfoFeed() //To be run first. Updates the variables of the script to fit the given template
    {
        this._nameOfPlant = plantTemplate.nameOfPlant; //Update the name to be the one from the template.

        this._seed = plantTemplate.seed; //Update the seed sprite to be the one from the template.
        this._growthStage1 = plantTemplate.growthStage1; //Update the growth 1 sprite to be the one from the template.
        this._growthStage2 = plantTemplate.growthStage2; //Update the growth 2 sprite to be the one from the template.
        this._fullyGrown = plantTemplate.fullyGrown; //Update the fully grown sprite to be the one from the template.

        this._progressionCostOfPlant = plantTemplate.progressionCostOfPlant; //Update the progression costs to be the ones from the template.
        this._preferencesOfPlant = plantTemplate.preferencesOfPlant; //Update the preferences to be the ones from the template.

        this._buyingPriceOfPlant = plantTemplate.buyingPriceOfPlant; //Update the buy price to be the one from the template.
        this._sellingPriceOfPlant = plantTemplate.sellingPriceOfPlant; //Update the sell price to be the one from the template.
    }
    #endregion
}
