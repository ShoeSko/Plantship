using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CurrencySystem : MonoBehaviour
{
    [HideInInspector] public static int SoftCurrency;
    [HideInInspector] public static int HardCurrency;

    public TextMeshProUGUI softcurrencyTEXT;
    public TextMeshProUGUI hardcurrencyTEXT;
    public TextMeshProUGUI watercountTEXT;


    private void Start()
    {
        //Test thingy for build
        SoftCurrency = 200;
        HardCurrency = 50;
    }

    private void Update()
    {
        softcurrencyTEXT.text = "Soft Currency: " + SoftCurrency;
        hardcurrencyTEXT.text = "Hard Currency: " + HardCurrency;
        watercountTEXT.text = "Water: " + WaterSource.currentWaterStored;
    }

    public void AddSoftCurrency(int AddValue)
    {
        //test
        //SoftCurrency += AddValue;
        SoftCurrency ++;
    }

    public void AddHardCurrency(int AddValue)
    {
        //test
        //HardCurrency += AddValue;
        HardCurrency++;
    }
}
