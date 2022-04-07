using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MicWithPlant : MonoBehaviour
{
    //NEEDS FOR MINIGAME

    // Refrence to Mic system (Trigger it?) + Read volume input.
    #region Variables
    [Header("Mic Inputs")]
    [SerializeField] private MicInput theMicrophone;

    [Header("Sliders")]
    [SerializeField] private Slider volumeShower; //Slider that gives feedback to player on the volume level they input.
    [SerializeField] private Slider volumeMinigameRangeSlider; //Slider that gives feedback to player where they are to reach the volume.
    [SerializeField] private Slider volumeMinigameScoreSlider; //Slider that gives feedback to player how far in the minigame they have progressed.

    [Header("Range of slider value")]
    [SerializeField] private float scoreRangeDifferece; //A value that is the difference between 0-?. on the range slider compared to volume shower.
    private float currentLowRange; //The current low range of the RangeSlider.
    private float currentHighRange; //The current high range of the RangeSlider.

    [SerializeField] private float scoreRangeLow; // The lowest the range slider is allowed to go.
    [SerializeField] private float scoreRangeHigh; // The highest the range slider is allowed to go.


    [Header("Range Slider Move Variables")]
    private float newRangeLocation; //The Next location for the slider to reach.
    private float speedToReachLocation; //How long will each tic move the slider.
        [SerializeField] private float maxSpeed; //Fastest speed it can move in.
        [SerializeField] private float minSpeed; //Slowest speed it can move in.
    [SerializeField] private float rateToLocation; //How fast is a tic for the move.
    [SerializeField] private float waitTimeWhenImobile; //How long will it wait when impbile

    private bool hasNewLocation; //Does the slider have a new location to reach?

    [Header("Score value")]
    private float currentScore; //The current score of the run.
    [SerializeField] float scoreIncreaseValue;
    [SerializeField] float scoreGoal; //The current goal of the score.

    [Header("End condition")]
    [HideInInspector] public bool WonVoiceMinigame;
    [SerializeField] private GameObject winTxt;

    #endregion

    private void Start()
    {
        Setup();
    }

    private void Setup()
    {
        volumeMinigameScoreSlider.maxValue = scoreGoal; // Sets the goal value to the stored value.

        winTxt.gameObject.SetActive(false);
        hasNewLocation = false;
        currentHighRange = 999; //Just to make it impossible to gain points imidietly
        currentLowRange = 999; //Just to make it impossible to gain points imidietly

    }


    private void Update()
    {

        if (theMicrophone._isInitialized) //If the Mic is running
        {
            ScoringInVolumeMinigame(); //Runs to update the point slider. (Might need to change)
            ScoreRangeMover();
            //if (WonVoiceMinigame)
            //{
            //    WonVoiceMinigame = false;
            //    //winTxt.gameObject.SetActive(false);
            //}

        }
        else
        {
            ResetMinigame();
        }

    }




    /// <summary>
    /// Moves the RangeSlider between it's low/Max, & updates the rest of the script to know where it is.
    /// </summary>
    private void ScoreRangeMover()
    {
        if (hasNewLocation)
        {
            //Wait, that is all you can do. It is currently reaching a new location.
            currentHighRange = volumeMinigameRangeSlider.value + scoreRangeDifferece;
            currentLowRange = volumeMinigameRangeSlider.value - scoreRangeDifferece;
        }
        else if (!hasNewLocation) //Does not have a new location, find a new one!
        {
            // How to determine new location. #1. Find location, #2. Determine Speed, #3. Run movement Coroutine
            hasNewLocation = true; //It now has a new location.

            newRangeLocation = Random.Range(scoreRangeLow, scoreRangeHigh); //Gives a location within the border of the slider.
            speedToReachLocation = Random.Range(minSpeed, maxSpeed); //Gives a speed within the border given.
            
            //Debug.Log("The new location is " + newRangeLocation + " & the speed to reach it is " + speedToReachLocation + " with a tic rate of " + rateToLocation);

            StartCoroutine(MoveTheRangeSlider());
        }
    }

    IEnumerator MoveTheRangeSlider()
    {
        while (volumeMinigameRangeSlider.value != newRangeLocation && theMicrophone._isInitialized) //Loops until it has reached the goal
        {
            yield return new WaitForSeconds(rateToLocation); // Tic a bit before taking action rather than doing it within a nano second!

            if(volumeMinigameRangeSlider.value > newRangeLocation) //New location is a lower value than current
            {
                if((volumeMinigameRangeSlider.value - newRangeLocation) < speedToReachLocation) //Is the speed higher than the distance missing?
                {
                    volumeMinigameRangeSlider.value = newRangeLocation; // Set the slider to the location
                }
                else
                {
                    volumeMinigameRangeSlider.value -= speedToReachLocation; //Decrease the RangeSlider towards the location.
                }
            }
            else if(volumeMinigameRangeSlider.value < newRangeLocation) //New location is higher value than current
            {
                if((newRangeLocation - volumeMinigameRangeSlider.value) > newRangeLocation) //Is the speed higher than the distance missing?
                {
                    volumeMinigameRangeSlider.value = newRangeLocation; // Set the slider to the location.
                }
                else
                {
                    volumeMinigameRangeSlider.value += speedToReachLocation; //Increases the RangeSlider towards the location.
                }
            }
            else
            {
                //hasNewLocation = false; //In this case it was a false alarm and the float gave the exact same number back. (Magic!!!)
            }
        }

        if (Random.Range(1, 5) == 3)
        {
            yield return new WaitForSeconds(waitTimeWhenImobile);
            hasNewLocation = false;
        }
        else
        {
            hasNewLocation = false; //The location has been reached. Get a new one please.
        }

    }

    /// <summary>
    /// This function will register when the Volume shower value is within the minigameRangeSlider value box, and increase points to show in minigamePointSlider.
    /// </summary>
    private void ScoringInVolumeMinigame()
    {
        if(volumeShower.value <= currentHighRange && volumeShower.value >= currentLowRange)
        {
            if (volumeMinigameScoreSlider.value <= scoreGoal) //Prevents the slider from going higher than the given goal for score.
            {
                currentScore += scoreIncreaseValue; //Increase the score value. 
                volumeMinigameScoreSlider.value = currentScore; //Sets the score slider value = to the current score value.
            }
        }
        else
        {
            //Plant die. Oh Nyo!
        }
        if (volumeMinigameScoreSlider.value == scoreGoal)
        {
            theMicrophone.ActivateVolumeRecording();
            WonVoiceMinigame = true;
            //winTxt.gameObject.SetActive(true);
        }
    }
    /// <summary>
    /// Resets all variables.
    /// </summary>
    public void ResetMinigame()
    {
        //What to reset, score value. Range location.

        currentScore = 0;
        volumeMinigameScoreSlider.value = 0;
        volumeMinigameRangeSlider.value = scoreRangeHigh;
        hasNewLocation = false;
        StopCoroutine(MoveTheRangeSlider());

        GardenManager.minigameButtonSwitch = false; //Makes sure the button resets.
    }

    private void OnDisable()
    {
        ResetMinigame(); //Resets setting on disabled.

        if (theMicrophone._isInitialized)
        {
            theMicrophone.ActivateVolumeRecording(); //Stops the recording. 
        }
        
    }
}
