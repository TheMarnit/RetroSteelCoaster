// TubeExtruder
using UnityEngine;

public class ResizableTubeExtruder : Extruder
{
    public bool faceSmoothing;

    public bool closeEnds;

    private float radius;

    private int circleVertCount;

    private bool ended = true;

    private Vector3 previousMiddlePoint;

    private int totalLength;

    public ResizableTubeExtruder(float radius, int circleVertCount)
    {
        this.radius = radius;
        this.circleVertCount = circleVertCount;
    }

    public override void extrude(Vector3 middlePoint, Vector3 tangent, Vector3 normal)
    {
        int num = Mathf.RoundToInt((middlePoint - previousMiddlePoint).magnitude * 3f);
        if (num < 1)
        {
            num = 1;
        }
        totalLength += num;
        int num2 = 1;
        if (!faceSmoothing)
        {
            num2 = 2;
        }
        for (int i = 0; i < circleVertCount; i++)
        {
            Vector3 vector = Quaternion.AngleAxis(360f - 360f / (float)circleVertCount * (float)i, tangent) * normal;
            Vector3 item = middlePoint + vector * radius;
            base.vertices.Add(item);
            base.uv.Add(base.uvCoord);
            base.normals.Add(vector);
        }
        if (ended && closeEnds)
        {
            for (int j = 0; j < circleVertCount - 1; j++)
            {
                base.indizes.Add(base.vertices.Count - circleVertCount);
                base.indizes.Add(base.vertices.Count - circleVertCount + j);
                base.indizes.Add(base.vertices.Count - circleVertCount + j + 1);
            }
        }
        if (!ended && base.vertices.Count > circleVertCount)
        {
            for (int k = 0; k < circleVertCount; k++)
            {
                int num3 = base.vertices.Count - 2 * circleVertCount;
                int num4 = base.vertices.Count - circleVertCount;
                base.indizes.Add(num3 + k);
                base.indizes.Add(num4 + k);
                base.indizes.Add(num3 + (k + 1) % circleVertCount);
                base.indizes.Add(num4 + k);
                base.indizes.Add(num4 + (k + 1) % circleVertCount);
                base.indizes.Add(num3 + (k + 1) % circleVertCount);
            }
        }
        previousMiddlePoint = middlePoint;
        ended = false;
    }

    public void setRadius(float radius)
    {
        this.radius = radius;
    }
    public override void end()
    {
        if (!ended)
        {
            if (closeEnds)
            {
                for (int i = 0; i < circleVertCount - 1; i++)
                {
                    base.indizes.Add(base.vertices.Count - circleVertCount + i + 1);
                    base.indizes.Add(base.vertices.Count - circleVertCount + i);
                    base.indizes.Add(base.vertices.Count - circleVertCount);
                }
            }
            ended = true;
        }
    }
}
