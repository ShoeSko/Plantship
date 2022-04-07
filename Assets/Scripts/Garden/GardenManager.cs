using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GardenManager : MonoBehaviour
{
    //cameras
    public Camera GardenCam;
    public Camera PlantCam;
    public Camera SacntuaryCam;

    //core elements
    public GameObject TestPlant;
    public GameObject SanctuaryTestPlant;
    public GameObject WaterCan;

    //UI scenes
    public List<GameObject> GardenUI = new List<GameObject>();
    public List<GameObject> PlantCareUI = new List<GameObject>();
    public List<GameObject> PlantCareInspectionUI = new List<GameObject>();//Just some elements from PlantCareUI
    public List<GameObject> SanctuaryUI = new List<GameObject>();
    public List<GameObject> PlantAffectionUI = new List<GameObject>();

    //selling
    public List<GameObject> SaveOrSellUI = new List<GameObject>();
    public GameObject SellConfirmation;
    public TextMeshProUGUI SellDescription;

    //special text
    public GameObject UINoSpace;
    public GameObject UINoMoney;
    public GameObject MenuInteraction;
    private Vector3 dropOffset = new Vector3(0, -303, 0);
    private bool MenuInteractionDown;

    //buttons
    public GameObject PlantAction;
    public GameObject SellButton;
    public GameObject dropdownButton;

    //sliders
    public Slider EXPSlider;
    public Slider PlantWaterSlider;
    public Slider AffectionSlider;

    //Audio
    public AudioSource SFXplayer;
    public AudioClip SlideDown;
    public AudioClip SlideUp;
    public AudioClip ClickOff;
    public AudioClip ClickOn;

    //PlantStuff
    [HideInInspector] public GameObject MainPlant;
    [HideInInspector] public GameObject Spot;
    public List<GameObject> ActivePlantSpots = new List<GameObject>();//Place in garden (can be increased)
    public List<GameObject> ActiveSanctuarySpots = new List<GameObject>();//Place in sanctuary (can be increased)
    private bool plantactionActive;

    //Affection
    private bool isInteracting;
    public GameObject Heart;


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

            AffectionSlider.minValue = MainPlant.GetComponent<PlantCore>().PreviousAffectionMilestone;
            AffectionSlider.maxValue = MainPlant.GetComponent<PlantCore>().NextAffectionMilestone;
            AffectionSlider.value = MainPlant.GetComponent<PlantCore>().Affection;


            if (MainPlant.GetComponent<PlantCore>().Stage == 3)
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

                    SFXplayer.clip = ClickOn;
                    SFXplayer.Play();

                    i = ActivePlantSpots.Count;//stop the loop
                }
                else if (i + 1 == ActivePlantSpots.Count)//if no spot is available
                {
                    SFXplayer.clip = ClickOff;
                    SFXplayer.Play();
                    StartCoroutine(NoSpaceDisplay());
                }
            }
        }
        else
        {
            SFXplayer.clip = ClickOff;
            SFXplayer.Play();
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

                SFXplayer.clip = ClickOn;
                SFXplayer.Play();

                i = ActiveSanctuarySpots.Count;//stop the loop
            }
            else if (i + 1 == ActiveSanctuarySpots.Count)//if no spot is available
            {
                SFXplayer.clip = ClickOff;
                SFXplayer.Play();
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
        if (!isInteracting)
        {
            for (int i = 0; i < GardenUI.Count; i++)
            {
                GardenUI[i].SetActive(!GardenUI[i].activeSelf);
            }

            for (int i = 0; i < PlantCareUI.Count; i++)
            {
                PlantCareUI[i].SetActive(!PlantCareUI[i].activeSelf);
            }

            SFXplayer.clip = ClickOn;
            SFXplayer.Play();

            GardenCam.enabled = !GardenCam.enabled;
            PlantCam.enabled = !PlantCam.enabled;
        }
        else
        {
            for (int i = 0; i < PlantCareInspectionUI.Count; i++)
            {
                PlantCareInspectionUI[i].SetActive(!PlantCareInspectionUI[i].activeSelf);
            }

            for (int i = 0; i < PlantAffectionUI.Count; i++)
            {
                PlantAffectionUI[i].SetActive(!PlantAffectionUI[i].activeSelf);
            }

            SFXplayer.clip = ClickOff;
            SFXplayer.Play();

            isInteracting = false;
        }
    }

    public void ChangeUIPlantInteraction()
    {
        for (int i = 0; i < PlantCareInspectionUI.Count; i++)
        {
            PlantCareInspectionUI[i].SetActive(!PlantCareInspectionUI[i].activeSelf);
        }

        for (int i = 0; i < PlantAffectionUI.Count; i++)
        {
            PlantAffectionUI[i].SetActive(!PlantAffectionUI[i].activeSelf);
        }

        SFXplayer.clip = ClickOff;
        SFXplayer.Play();

        isInteracting = true;
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

        SFXplayer.clip = ClickOn;
        SFXplayer.Play();

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

        SFXplayer.clip = ClickOn;
        SFXplayer.Play();
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

        SFXplayer.clip = ClickOn;
        SFXplayer.Play();

        ChangeUI();
    }

    public void DropDownMenu()
    {
        if (!MenuInteractionDown)
        {
            MenuInteraction.transform.position += dropOffset;
            dropdownButton.transform.eulerAngles = new Vector3(0, 0, 180);
            SFXplayer.clip = SlideDown;
            SFXplayer.Play();
            MenuInteractionDown = true;
        }
        else
        {
            MenuInteraction.transform.position -= dropOffset;
            dropdownButton.transform.eulerAngles = new Vector3(0, 0, 0);
            SFXplayer.clip = SlideUp;
            SFXplayer.Play();
            MenuInteractionDown = false;
        }
    }

    public void LovePlant()//prototype script, will likely be deleted or vastly changed
    {
        if(MainPlant.GetComponent<PlantCore>().AffectionLevel < MainPlant.GetComponent<PlantCore>().relationshipMilestones.Count)
        {
            int randomHearts = Random.Range(1, 4);
            for (int i = 0; i <= randomHearts; i++)
            {
                GameObject heart = Instantiate(Heart, MainPlant.transform.position, MainPlant.transform.rotation);
                Vector2 heartPos = new Vector2(heart.transform.position.x, heart.transform.position.y);
                Vector2 positionOffset = new Vector2(Random.Range(-0.5f, 0.6f), Random.Range(0f, 2f));
                heart.transform.position = heartPos + positionOffset;
                Vector2 velocity = new Vector2(Random.Range(-70f, 80f), Random.Range(50f, 90f));
                heart.GetComponent<Rigidbody2D>().AddForce(velocity);
            }

            MainPlant.GetComponent<PlantCore>().Affection += 5;
        }
    }
}