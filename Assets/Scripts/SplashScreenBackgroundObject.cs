using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SplashScreenBackgroundObject : MonoBehaviour
{
    private SplashScreenController ssc;

    private void Start()
    {
        ssc = FindObjectOfType<SplashScreenController>();
    }

    private void Update()
    {
        transform.position+=new Vector3(-ssc.GetSpeed()*Time.deltaTime*4f,0,0);
    }
}
