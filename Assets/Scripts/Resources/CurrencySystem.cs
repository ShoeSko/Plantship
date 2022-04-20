using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CurrencySystem : MonoBehaviour
{
    [HideInInspector] public static int SoftCurrency;
    [HideInInspector] public static int HardCurrency;
    [HideInInspector] public static int AffectionTokens;

    public TextMeshProUGUI softcurrencyTEXT;
    public TextMeshProUGUI hardcurrencyTEXT;
    public TextMeshProUGUI watercountTEXT;
    public TextMeshProUGUI AffectionTokensTEXT;


    private void Start()
    {
        //Test thingy for build
        SoftCurrency = 300;
        HardCurrency = 50;
        AffectionTokens = 1;
    }

    private void Update()
    {
        softcurrencyTEXT.text = "" + SoftCurrency;
        hardcurrencyTEXT.text = "" + HardCurrency;
        watercountTEXT.text = "" + WaterSource.currentWaterStored;
        AffectionTokensTEXT.text = "" + AffectionTokens;
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
