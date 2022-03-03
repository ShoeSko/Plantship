using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LiveTime : MonoBehaviour
{
    private static bool BootOnce;
    //Clock
    public int hour;
    public int minute;
    public int second;

    //date
    public int day;
    public int year;

    //Other
    private int SecondUpdater;

    //DateSaving
    [HideInInspector] public int yearSave;
    [HideInInspector] public int daySave;
    [HideInInspector] public int hourSave;
    [HideInInspector] public int minuteSave;
    [HideInInspector] public int secondSave;

    //Calculations
    [HideInInspector] private int yearPassed;
    [HideInInspector] private int dayPassed;
    [HideInInspector] private int hourPassed;
    [HideInInspector] private int minutePassed;
    [HideInInspector] private int secondPassed;

    private void Start()
    {
        UpdateClock();

        if (!BootOnce)
            BootUpdate();

        SecondUpdater = second;
    }

    private void Update()
    {
        UpdateClock();

        if(SecondUpdater != second)//Tick only once every second
        {
            SecondUpdater = second;
        }
    }

    private void UpdateClock()
    {
        year = System.DateTime.Now.Year;
        day = System.DateTime.Now.DayOfYear;

        hour = System.DateTime.Now.Hour;
        minute = System.DateTime.Now.Minute;
        second = System.DateTime.Now.Second;
    }

    private void BootUpdate()
    {
        yearPassed = year - yearSave;
        dayPassed = day + yearPassed * 365 - daySave;//How many days passed since game was turned off (doesnt update in-game)
        hourPassed = hour + dayPassed * 24 - hourSave;//How many hours passed since game was turned off(doesnt update in-game)
        minutePassed = minute + hourPassed * 60 - minuteSave;
        secondPassed = second + minutePassed * 60 - secondSave;

        //refill water counter based on time passed
        //grow plants based on time passed

        BootOnce = true;
    }

    void UnusedReferences()
    {
        //Debug.Log("It's now " + System.DateTime.Today);
        //string WhatTime = System.DateTime.Now.ToString("HH:MM:ss");
    }
}