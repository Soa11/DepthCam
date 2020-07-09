using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Intel.RealSense;

public class testStream : MonoBehaviour
{
    Pipeline Pipeline = new Pipeline();
    FrameSet FrameSet;

    // Start is called before the first frame update
    void Start()
    {
        Pipeline.Start();
    }

    // Update is called once per frame
    void Update()
    {
        FrameSet = Pipeline.WaitForFrames();
        DepthFrame depthFrame = FrameSet.DepthFrame;
        int width = depthFrame.Width;
        int height = depthFrame.Height;
        float distancetoCentre = depthFrame.GetDistance(width / 2, height / 2);

        print("distancetoCentre: " + distancetoCentre);


    }
}
