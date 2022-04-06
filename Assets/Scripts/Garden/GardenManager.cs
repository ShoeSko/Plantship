using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GardenManager : MonoBehaviour
{
    public Camera GardenCam;
    public Camera PlantCam;
    public Camera SacntuaryCam;

    public GameObject TestPlant;
    public GameObject SanctuaryTestPlant;
    public GameObject WaterCan;

    public List<GameObject> ActivePlantSpots = new List<GameObject>();//Place in garden (can be increased)
    public List<GameObject> ActiveSanctuarySpots = new List<GameObject>();//Place in sanctuary (can be increased)

    public List<GameObject> GardenUI = new List<GameObject>();
    public List<GameObject> PlantCareUI = new List<GameObject>();
    public List<GameObject> SanctuaryUI = new List<GameObject>();

    public List<GameObject> SaveOrSellUI = new List<GameObject>();

    public GameObject SellConfirmation;
    public TextMeshProUGUI SellDescription;

    public GameObject UINoSpace;
    public GameObject UINoMoney;
    public GameObject PlantAction;
    public GameObject SellButton;

    public Slider EXPSlider;
    public Slider PlantWaterSlider;

    [HideInInspector] public GameObject MainPlant;
    [HideInInspector] public GameObject Spot;

    private bool plantactionActive;


    private void Update()
    {
        if (MainPlant != null)
        {
            Debug.Log(MainPlant.name);

            EXPSlider.maxValue = MainPlant.GetComponent<PlantCore>().NextMilestoneEXP;
            EXPSlider.value = MainPlant.GetComponent<PlantCore>().currentGrowthValue;
            EXPSlider.minValue = MainPlant.GetComponent<PlantCore>().PreviousMilestoneEXP;

            PlantWaterSlider.maxValue = MainPlant.GetComponent<PlantCore>().waterPlantCanStoreLimit;
            PlantWaterSlider.value = MainPlant.GetComponent<PlantCore>().waterStoredInPlant;

            if(MainPlant.GetComponent<PlantCore>().Stage == 3)
            {
                PlantAction.SetActive(true);
            }
            else
            {
                PlantAction.SetActive(false);
            }

            SellDescription.text = "Are you sure you want to sell " + MainPlant.GetComponent<PlantCore>().nameOfPlant + " for " + MainPlant.GetComponent<PlantCore>().CurrentSellValue + " coins?";
        }
    }

    public void BuyPlant()
    {
        if(CurrencySystem.SoftCurrency >= 100)
        {
            for (int i = 0; i < ActivePlantSpots.Count; i++)
            {
                if (ActivePlantSpots[i].GetComponent<PlantSpots>().IsUsed == false)//If the spot is empty
                {
                    ActivePlantSpots[i].GetComponent<PlantSpots>().ActivePlant = TestPlant;
                    ActivePlantSpots[i].GetComponent<PlantSpots>().PlacePlant();
                    CurrencySystem.SoftCurrency -= 100;

                    i = ActivePlantSpots.Count;//stop the loop
                }
                else if (i + 1 == ActivePlantSpots.Count)//if no spot is available
                {
                    StartCoroutine(NoSpaceDisplay());
                }
            }
        }
        else
        {
            StartCoroutine(NoMoneyDisplay()); 
        }
    }

    public void AddPlantToSanctuary()
    {
        for (int i = 0; i < ActiveSanctuarySpots.Count; i++)
        {
            if (ActiveSanctuarySpots[i].GetComponent<PlantSpots>().IsUsed == false)//If the spot is empty
            {
                ActiveSanctuarySpots[i].GetComponent<PlantSpots>().ActivePlant = SanctuaryTestPlant;
                ActiveSanctuarySpots[i].GetComponent<PlantSpots>().PlacePlant();

                if (plantactionActive)
                    PlantActionButton();

                Spot.GetComponent<PlantSpots>().PlantSold();
                ChangeUI();

                i = ActiveSanctuarySpots.Count;//stop the loop
            }
            else if (i + 1 == ActiveSanctuarySpots.Count)//if no spot is available
            {
                Debug.Log("There's no space in sanctuary");
            }
        }
    }

    IEnumerator NoSpaceDisplay()
    {
        UINoSpace.SetActive(true);

        yield return new WaitForSeconds(2);

        UINoSpace.SetActive(false);
    }

    IEnumerator NoMoneyDisplay()
    {
        UINoMoney.SetActive(true);

        yield return new WaitForSeconds(2);

        UINoMoney.SetActive(false);
    }

    public void ChangeUI()
    {
        for(int i = 0; i < GardenUI.Count; i++)
        {
            GardenUI[i].SetActive(!GardenUI[i].activeSelf);
        }

        for (int i = 0; i < PlantCareUI.Count; i++)
        {
            PlantCareUI[i].SetActive(!PlantCareUI[i].activeSelf);
        }

        GardenCam.enabled = !GardenCam.enabled;
        PlantCam.enabled = !PlantCam.enabled;
    }

    public void ChangeUISanctuary()
    {
        for (int i = 0; i < GardenUI.Count; i++)
        {
            GardenUI[i].SetActive(!GardenUI[i].activeSelf);
        }

        for (int i = 0; i < SanctuaryUI.Count; i++)
        {
            SanctuaryUI[i].SetActive(!SanctuaryUI[i].activeSelf);
        }

        GardenCam.enabled = !GardenCam.enabled;
        SacntuaryCam.enabled = !SacntuaryCam.enabled;
    }

    /// <summary>
    /// Activates watering of the plant as long as it is being held down.
    /// </summary>
    /// <param name="wateringCanObject"></param>
    public void wateringCanIsBehingHeld()
    {
        MainPlant.GetComponent<PlantCore>().WateringIsInProgress = true; //The watering is now in progress
    }

    /// <summary>
    /// Deactivates watering of the plant when it is released.
    /// </summary>
    /// <param name="wateringCanObject"></param>
    public void WateringCanIsNotBeingHeld()
    {
        GameObject.Find("Watercan").GetComponent<SpriteRenderer>().enabled = false; //Watering can object is turned off.
        MainPlant.GetComponent<PlantCore>().WateringIsInProgress = false; //The watering is no longer in progress
    }

    public void PlantActionButton()
    {
        plantactionActive = !plantactionActive;
        for (int i = 0; i < SaveOrSellUI.Count; i++)
        {
            SaveOrSellUI[i].SetActive(!SaveOrSellUI[i].activeSelf);
        }
    }

    public void AskSellConfirmation()
    {
        SellConfirmation.SetActive(!SellConfirmation.activeSelf);//Toggle active
    }

    public void SellPlant()
    {
        CurrencySystem.SoftCurrency += MainPlant.GetComponent<PlantCore>().CurrentSellValue;
        if(plantactionActive)
            PlantActionButton();
        SellConfirmation.SetActive(false);
        Spot.GetComponent<PlantSpots>().PlantSold();
        ChangeUI();
    }
}
