using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ResourceManager : MonoBehaviour
{
    //Resources
    public static int Water;
    public static int SoftCurrency;
    public static int HardCurrency;

    private void Start()
    {
        DontDestroyOnLoad(this.gameObject);
    }


    public void AddWater(int AddingWaterNumber)
    {
        Water += AddingWaterNumber;
    }

    public void AddSoftCurrency(int AddingSoftNumber)
    {
        SoftCurrency += AddingSoftNumber;
    }

    public void AddHardCurrency(int AddingHardNumber)
    {
        HardCurrency += AddingHardNumber;
    }
}