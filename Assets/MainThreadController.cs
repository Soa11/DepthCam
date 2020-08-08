using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainThreadController : MonoBehaviour
{

    public static float average;

    public float seconds = 2f;

    float nextTriggerSecond = 0;

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Time.timeSinceLevelLoad > nextTriggerSecond)
        {
            nextTriggerSecond += seconds;


            Debug.Log("average: " + average);
        }
    }
}
