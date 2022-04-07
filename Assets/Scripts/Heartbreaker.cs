using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Heartbreaker : MonoBehaviour
{
    private float timer;
    private void Update()
    {
        timer += Time.deltaTime;

        if(timer >= 10)
        {
            Destroy(gameObject);
        }
    }
}
