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

    private void Update()
    {
        softcurrencyTEXT.text = "Soft Currency: " + SoftCurrency;
        hardcurrencyTEXT.text = "Hard Currency: " + HardCurrency;
    }

    public void AddSoftCurrency(int AddValue)
    {
        //test
        SoftCurrency++;
    }

    public void AddHardCurrency(int AddValue)
    {
        //test
        HardCurrency++;
    }
}
