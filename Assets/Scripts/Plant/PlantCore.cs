using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
[RequireComponent(typeof(SpriteRenderer))] //Makes sure the gameobject has the required components.
public class PlantCore : MonoBehaviour
{
    #region Preset Variables
    public PlantDesigner plantTemplate;

    [Header("Plant Looks")]
    [HideInInspector][Tooltip("The name the plant will go by")] public string nameOfPlant; //Simple now, could future work add list of potential names to choose from instead.

    //In the scenario plants had different amount of stages, this would be a list instead of 4 separate sprites.
    [Tooltip("The sprite that represent the seed form of the plant")] private Sprite _seed;
    [Tooltip("The sprite that represent the first growth stage of the plant")] private Sprite growthStage1;
    [Tooltip("The sprite that represent the second growth stage of the plant")] private Sprite growthStage2;
    [Tooltip("The sprite that represent the fully grown plant")] private Sprite fullyGrown;

    [Header("Plant Stats")]
    [HideInInspector] [Tooltip("The 3 values the plant need to reach for i'ts progression")] public int[] ListprogressionCostOfPlant = new int[3]; //Gives a list of 3 ints, a range can be added(need to know the wanted values)
    [Tooltip("The tasks that the plant preffers, and will ask for")] private string[] preferencesOfPlant; //Gives a list to implement the preffered tasks, need to know what form the tasks will arrive in before it can be easier to use.
    [Tooltip("The tasks that the plant preffers, and will ask for")] private string[] preferencesOfPlantNoMic; //Gives a list to implement the preffered tasks when Mic is disabled, need to know what form the tasks will arrive in before it can be easier to use.

    [Tooltip("The price of the plant when it can be aquired")] private int buyingPriceOfPlant; //Simple int for the price of buyig the plant
    [Tooltip("The price of the plant when it can be aquired")] private float plantWaterConsumptionRate; //Simple int for the price of buyig the plant
    [HideInInspector] [Tooltip("The price of the plant when it is fully grown and is to be sold")] private List<int> sellingPriceOfPlant; //Simple int for the price of selling the plant
    
    [HideInInspector] [Tooltip("Required affection to reach various milestones on the relationship meter")] public List<int> relationshipMilestones; //Simple int for the price of selling the plant
    [HideInInspector] [Tooltip("Required affection to reach various milestones on the relationship meter")] private List<float> relationshipCostModifier; //Simple int for the price of selling the plant

    private Sprite dryPot;
    private Sprite wetPot;
    #endregion
    #region Other Variables
    private SpriteRenderer spriteOfPlant; //Refrence to the plants spriterenderer.

    private int growthStageAmount = 3; //Sets a definit value on the amount of stages a plant transforms into. Seed is the base and turns into = (stage 1, stage 2 & Fully grown)

    [HideInInspector] public int currentGrowthValue; //!!Serialized for Testing!! The value of the plants current progression. (can be used to determine growth stage) 
    [SerializeField] private int growthRate = 1; //The amount it tic grows.
    [SerializeField] private float growthSpeed = 1f; //The speed it tic grows
    [HideInInspector] public int growthStage; //The value representation of the current stage (0 = seed, 3 = fully grown)
    [HideInInspector] public int Stage; //The value representation of the current stage (0 = seed, 3 = fully grown)
    private bool CheckGrowth = true;
    [HideInInspector] public bool FullyGrown;

    private GameObject plantPot;

    [HideInInspector] public float waterStoredInPlant; //The amount of water this plant has been given(Lowers over time)
    [HideInInspector] public float waterPlantCanStoreLimit = 200; //The upper amount of wate this plant can store. This should be changeable?
    [HideInInspector] public bool WateringIsInProgress; //Is the watering can being held?
    [HideInInspector] public bool ReadyToGrowUp; //A bool for when the plant is ready for milestone growth. (If true. stop growing + need minigame to grow).
    private MicWithPlant micScript; //Access to the mic script.
    public GameObject VoiceMinigameObject; //The main object of the voice minigame to make it run, when turned on.
    private float wateringTickSpeed = 0.1f; //How fast will the watering take place.
    private float wateringTimer; //Timer to limit water speed.

    [HideInInspector] public bool isActivePlant; //The plant itself need to know it is the current active plant.

    [HideInInspector] public int NextMilestoneEXP;
    [HideInInspector] public int PreviousMilestoneEXP;

    [HideInInspector] public int CurrentSellValue;

    [HideInInspector] public int AffectionLevel = 0;//Current level of this plant's affection
    [HideInInspector] public int Affection;//Relationship "experience points", determine the AffectionLevel
    [HideInInspector] public int NextAffectionMilestone;//Relationship "experience points", determine the AffectionLevel
    [HideInInspector] public int PreviousAffectionMilestone;//Relationship "experience points", determine the AffectionLevel


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
        plantPot = transform.Find("PlantPot").gameObject;

        //Voice minigame work
        //VoiceMinigameObject.SetActive(true);
        ////Debug.Log("Voice mini game object is called " + VoiceMinigameObject.name);
        //VoiceMinigameObject.SetActive(false);
        micScript = VoiceMinigameObject.GetComponent<MicWithPlant>(); //Gets a refrence to the MicWithPlant Script.
    }
    #endregion
    #region Update
    private void Update()
    {
        //Debug.Log("Growth Stage = " + Stage);
        CurrentSellValue =(int) (sellingPriceOfPlant[Stage] * relationshipCostModifier[AffectionLevel]);
        Debug.Log("My sell value is now: " + CurrentSellValue);

        AmIActivePlant(); //Check if this plant is the current active plant.

        if (WateringIsInProgress)
        {
            GainingWater();
            timerForWater();
        }
        else
        {
            wateringTimer = 0; //Resets timer.
            //Debug.Log("Button was released");
        }

        CheckForMilestone(); //Checks if milestones have been reached (Robust for Prototype)

        if (FullyGrown)
        {
            waterStoredInPlant = 0;           
        }
        else if (CheckGrowth)
        {
            GrowingPlant(); //Growing power of plant!
        }

        if(AffectionLevel < relationshipMilestones.Count)
            ReadRelationshipMeter();
        else
            PreviousAffectionMilestone = 0;
    }
    #endregion

    #region Watering
    /// <summary>
    /// 
    /// </summary>
    public void GainingWater() //Plant gaining water.  (Watering option in case we want multiple watering levels)
    {
        if (FindObjectOfType<WaterSource>()) //Safety net to prevent code from trying to use WaterSource without finding it.
        {
            WaterSource water = FindObjectOfType<WaterSource>(); //Gives refrence to the water source as Water.

            if(WaterSource.currentWaterStored > 0) //Is there water in the water source
            {
                if(waterStoredInPlant < waterPlantCanStoreLimit) //Is the timer ready to water
                {
                    if (wateringTimer >= wateringTickSpeed) //Checks if the plant is filled with water
                    {
                        water.wateringPlant();
                        waterStoredInPlant += 1; //Increase water by 1
                        wateringTimer = 0; //Resets timer.

                    //Debug.Log("Water is being stored at currently " + waterStoredInPlant);
                    }
                    GameObject.Find("Watercan").GetComponent<SpriteRenderer>().enabled = true; //Watering can object is turned off.
                }
                else
                {
                    GameObject.Find("Watercan").GetComponent<SpriteRenderer>().enabled = false; //Watering can object is turned off.
                }
            }
            else
            {
                Debug.Log("I am being held down but you ran out of water");
                GameObject.Find("Watercan").GetComponent<SpriteRenderer>().enabled = false; //Watering can object is turned off.
            }
        }

    }

    private void timerForWater() //Timer to limit the watering speed
    {
        if(wateringTimer <= wateringTickSpeed)
        {
            wateringTimer += Time.deltaTime;
            //Debug.Log("Timer just reset");
        }
    }

    /// <summary>
    /// Activates watering of the plant as long as it is being held down.
    /// </summary>
    /// <param name="wateringCanObject"></param>
    public void wateringCanIsBehingHeld()
    {
        WateringIsInProgress = true; //The watering is now in progress
    }
    
    /// <summary>
    /// Deactivates watering of the plant when it is released.
    /// </summary>
    /// <param name="wateringCanObject"></param>
    public void WateringCanIsNotBeingHeld()
    {
        GameObject.Find("Watercan").GetComponent<SpriteRenderer>().enabled = false; //Watering can object is turned off.
        WateringIsInProgress = false; //The watering is no longer in progress
    }

    #endregion
    #region Plant Growth
    private void GrowingPlant()
    {
        if(waterStoredInPlant > 0 && !ReadyToGrowUp) //Only takes effect if there is water!!! & that it is not ready to grow up.
        {
            plantPot.GetComponent<SpriteRenderer>().sprite = wetPot;
            StartCoroutine(PlantGowing());
            StartCoroutine(ConsumeWater());

            CheckGrowth = false;
        }
    }
    
    IEnumerator PlantGowing()
    {
        //Debug.Log("I did PlantGowing");
        yield return new WaitForSeconds(growthSpeed);
        if (waterStoredInPlant > 0 && !ReadyToGrowUp)
        {
            currentGrowthValue += growthRate; //Increases progression, add modifiers for growth rate later?
            StartCoroutine(PlantGowing()); //Reruns the program.
        }
        else
        {
            //Debug.Log(gameObject.name + " ran out of water ;(");
            plantPot.GetComponent<SpriteRenderer>().sprite = dryPot;
            CheckGrowth = true;
        }
    }

    IEnumerator ConsumeWater()
    {
        yield return new WaitForSeconds(1f);//This number should be 144 in the final product
        waterStoredInPlant -= plantWaterConsumptionRate;

        if(waterStoredInPlant > 0 && !ReadyToGrowUp)
        {
            StartCoroutine(ConsumeWater());
        }
    }

        #endregion
        #region Milestone Reach
        private void CheckForMilestone()
    {
        if(currentGrowthValue >= ListprogressionCostOfPlant[2]) //Checks if the progression has reached the laststage.
        {
            if(growthStage != 3) //Checks that it has not already changed stage
            {
                if (ReadyToGrowUp && micScript.WonVoiceMinigame)
                {
                    Stage = 3;
                    //Debug.Log("IM AT STAGE 3");
                    growthStage = 3; //Sets the growthStage to Full grown
                    PlantSpriteChange(); //Update sprite to the new plant form.
                    FullyGrown = true;


                    ReadyToGrowUp = false; //No longer ready to grow up.

                    micScript.WonVoiceMinigame = false; //Resets minigame.
                    VoiceMinigameObject.SetActive(false);
                }
                else if (!ReadyToGrowUp && !FullyGrown && isActivePlant) //If you are not ready to grow up, become it.
                {
                    StopCoroutine(ConsumeWater()); //Stops the water from being emptied.
                    StopCoroutine(PlantGowing()); //Stops the plant from growing without permission.
                    ReadyToGrowUp = true; //Makes sure script knows plant is ready to grow up.
                }
            }
        }
        else if(currentGrowthValue >= ListprogressionCostOfPlant[1]) //Check if the progression has reached the second stage.
        {
            if(growthStage != 2) //Checks if it has not already changed stage
            {
                if (ReadyToGrowUp && micScript.WonVoiceMinigame)
                {
                    Stage = 2;
                    //Debug.Log("IM AT STAGE 2");
                    growthStage = 2; //Sets the growthStage to stage 2
                    NextMilestoneEXP = ListprogressionCostOfPlant[2];
                    PreviousMilestoneEXP = ListprogressionCostOfPlant[1];
                    PlantSpriteChange(); //Update sprite to the new plant form. 

                    ReadyToGrowUp = false; //No longer ready to grow up.

                    micScript.WonVoiceMinigame = false; //Resets minigame.
                    VoiceMinigameObject.SetActive(false);
                }
                else if (!ReadyToGrowUp && NextMilestoneEXP != ListprogressionCostOfPlant[2] && isActivePlant) //If you are not ready to grow up, become it.
                {
                    StopCoroutine(ConsumeWater()); //Stops the water from being emptied.
                    StopCoroutine(PlantGowing()); //Stops the plant from growing without permission.
                    ReadyToGrowUp = true; //Makes sure script knows plant is ready to grow up.
                }
            }
        }
        else if(currentGrowthValue >= ListprogressionCostOfPlant[0]) //Check if the progression has reached the second stage.
        {
            if(growthStage != 1) //Checks if it has not already changed stage
            {
                if (ReadyToGrowUp && micScript.WonVoiceMinigame && isActivePlant) //Grows up if it is ready to grow up & you have won the voice minigame & is the active plant
                {
                    Stage = 1;
                    //Debug.Log("IM AT STAGE 1");
                    growthStage = 1; //Sets the growthStage to stage 1
                    NextMilestoneEXP = ListprogressionCostOfPlant[1];
                    PreviousMilestoneEXP = ListprogressionCostOfPlant[0];
                    PlantSpriteChange(); //Update sprite to the new plant form. 

                    ReadyToGrowUp = false; //No longer ready to grow up.

                    micScript.WonVoiceMinigame = false; //Resets minigame.
                    VoiceMinigameObject.SetActive(false);
                }
                else if (!ReadyToGrowUp && NextMilestoneEXP != ListprogressionCostOfPlant[1]) //If you are not ready to grow up, become it.
                {
                    StopCoroutine(ConsumeWater()); //Stops the water from being emptied.
                    StopCoroutine(PlantGowing()); //Stops the plant from growing without permission.
                    ReadyToGrowUp = true; //Makes sure script knows plant is ready to grow up.
                }
                //print("YES!  " + ReadyToGrowUp + micScript.WonVoiceMinigame + isActivePlant );
            }
        }
        else if(growthStage == 0)
        {
            Stage = 0;
            //Debug.Log("IM AT STAGE 0");
            NextMilestoneEXP = ListprogressionCostOfPlant[0];
        }
    }
    #endregion
    private void AmIActivePlant()
    {
        if (FindObjectOfType<GardenManager>().MainPlant == this.gameObject) //Am I the active plant?
        {
            isActivePlant = true; //I am now the active plant
            //print("I am Active plant!");
            if (!ReadyToGrowUp)
            {
                VoiceMinigameObject.SetActive(false); //If I am not to grow turn of game.
            }
        }
        else
        {
            isActivePlant = false; //I am no longer the active plant
        }
    }

    #region Sprite changing
    private void PlantSpriteChange()
    {
        spriteOfPlant = GetComponent<SpriteRenderer>(); //Makes sure the spriterenderer being used is the gameobjects own.

        Sprite[] growthStages = new Sprite[] { growthStage1, growthStage2, fullyGrown }; //Creates a list of all the growth stages. (Hardcoded for now)

        if(growthStage != 0)
        {
            spriteOfPlant.sprite = growthStages[--growthStage]; //Since the plant has reached the stage, then change the sprite to the appropriate stage. 
        }
        else
        {
            spriteOfPlant.sprite = _seed; //Sprite is now a seed.
        }

        //Debug.Log("Current stage is = " + growthStage + "and current sprite image is = " + spriteOfPlant.sprite.name); //What is the current growth and the image associated with it.
    }
    #endregion

    #region Affection
    
    private void ReadRelationshipMeter()
    {
        NextAffectionMilestone = relationshipMilestones[AffectionLevel];

        if (Affection >= NextAffectionMilestone)
        {
            PreviousAffectionMilestone = relationshipMilestones[AffectionLevel];
            AffectionLevel++;
        }
    }
    

    #endregion

    #region Setting up plant
    private void PlantInfoFeed() //To be run first. Updates the variables of the script to fit the given template
    {
        this.nameOfPlant = plantTemplate.nameOfPlant; //Update the name to be the one from the template.

        this._seed = plantTemplate.seed; //Update the seed sprite to be the one from the template.
        this.growthStage1 = plantTemplate.growthStage1; //Update the growth 1 sprite to be the one from the template.
        this.growthStage2 = plantTemplate.growthStage2; //Update the growth 2 sprite to be the one from the template.
        this.fullyGrown = plantTemplate.fullyGrown; //Update the fully grown sprite to be the one from the template.

        this.ListprogressionCostOfPlant = plantTemplate.progressionCostOfPlant; //Update the progression costs to be the ones from the template.
        this.preferencesOfPlant = plantTemplate.preferencesOfPlant; //Update the preferences to be the ones from the template.
        this.preferencesOfPlantNoMic = plantTemplate.preferencesOfPlantNoMic; //Updates the preference to be the ones from the template when the Mic is disabled.

        this.buyingPriceOfPlant = plantTemplate.buyingPriceOfPlant; //Update the buy price to be the one from the template.
        this.sellingPriceOfPlant = plantTemplate.sellingPriceOfPlant; //Update the sell price to be the one from the template.
        this.plantWaterConsumptionRate = plantTemplate.PlantWaterConsumption;

        this.relationshipMilestones = plantTemplate.RelationshipMilestones;
        this.relationshipCostModifier = plantTemplate.RelationshipPriceModifier;

        dryPot = plantTemplate.DryPot;
        wetPot = plantTemplate.WetPot;
    }
    #endregion
}