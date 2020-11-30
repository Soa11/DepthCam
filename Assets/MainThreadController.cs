using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainThreadController : MonoBehaviour
{
    //This number is being changed by the camera processing blocks.
    public static float average;

    public float secondsBetweenReports = 2f;

    float minDistance = 100;

    float halfMin;



    float nextTriggerSecond = 0;


    float oldAverage;

    float timeInZone = 0;

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        //check floor
        if(Time.timeSinceLevelLoad >1 && Time.timeSinceLevelLoad < 5)
        {
            if(average < minDistance)
            {
                minDistance = average;
                halfMin = minDistance / 2;
                Debug.Log("MinDistance= " + minDistance);
            }
        }

        if (average < minDistance)
        {
            //average is greater than floor
            timeInZone += 0.01f;
            Debug.Log("in Zone");
        }
        else
        {
            timeInZone -= 0.01f;
            Debug.Log("counting donw");

            if (timeInZone < 0)
            {
                timeInZone = 0;
            }
        }
        if(average < halfMin)
        {
            Debug.Log("IN HALF DISTANCE");
        }


        if (Time.timeSinceLevelLoad > nextTriggerSecond)
        {

            nextTriggerSecond += secondsBetweenReports;


            //Debug.Log("average: " + average);
            oldAverage = average;
        }
    }
}
