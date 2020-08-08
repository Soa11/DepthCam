using Intel.RealSense;
using UnityEngine;

[ProcessingBlockData(typeof(PrintNumbers))]
public class PrintNumbers : RsProcessingBlock
{
    public ushort Distance = 1000;

    ushort[] depthData;



    public float oldThreshold = 0;
    public float threshold = 0;
    bool isInZone = false;
    float timeInZone = 0;

    float averageValue;

    float nextTriggerSecond = 0;


    Vector2[] points = {
        new Vector2(0.25f, 0.25f),
        new Vector2(0.5f, 0.25f),
        new Vector2(0.75f, 0.25f),
        new Vector2(0.25f, 0.5f),
        new Vector2(0.5f, 0.5f),
        new Vector2(0.75f, 0.5f),
        new Vector2(0.25f, 0.75f),
        new Vector2(0.5f, 0.75f),
        new Vector2(0.75f, 0.75f)
    };


    void OnDisable()
    {
        depthData = null;
    }

    Frame ApplyFilter(DepthFrame depth, FrameSource frameSource)
    {
        using (var p = depth.Profile)
        {
            var count = depth.Width * depth.Height;
            if (depthData == null || depthData.Length != count)
                depthData = new ushort[count];

            depth.CopyTo(depthData);

            for (int i = 0; i < count; i++)
            {
                if (depthData[i] > Distance)
                    depthData[i] = 0;
            }

            var v = frameSource.AllocateVideoFrame<DepthFrame>(p, depth, depth.BitsPerPixel, depth.Width, depth.Height, depth.Stride, Extension.DepthFrame);
            v.CopyFrom(depthData);

            return v;
        }
    }

    public override Frame Process(Frame frame, FrameSource frameSource)
    {
        if (frame.IsComposite)
        {
            using (var fs = FrameSet.FromFrame(frame))
            using (var depthFrame = fs.DepthFrame)
            {
                float sum = 0;


                //Loop through all our listed points.
                for (int i = 0; i < points.Length; i++)
                {
                    Vector2 pointCoord = points[i];
                    Vector2Int point = new Vector2Int((int)(pointCoord.x * depthFrame.Width), (int)(pointCoord.y * depthFrame.Height));

                    float depthAtPoint = depthFrame.GetDistance(point.x, point.y);
                    sum += depthAtPoint;


                    if (depthAtPoint < threshold)
                    {
                        isInZone = true;
                        timeInZone += 0.01f; ;
                    }
                    else
                    {
                        isInZone = false;
                        timeInZone -= 0.01f;
                        if (timeInZone < 0)
                        {
                            timeInZone = 0;
                        }
                    }


                    //Debug.Log("point: " + i + " depth: " + depthAtPoint);

                }
                float average = sum /= points.Length;
                threshold = average;

                MainThreadController.average = average;



                var v = ApplyFilter(depthFrame, frameSource);
                return v;

            }
        }

        if (frame is DepthFrame)
            return ApplyFilter(frame as DepthFrame, frameSource);

        return frame;
    }
}
