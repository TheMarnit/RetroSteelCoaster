﻿using UnityEngine;
using System;
using System.IO;

public class IboxCoasterMeshGenerator : MeshGenerator
{
    private const float buildVolumeHeight = 0.8f;

    private ShapeExtruder leftRailExtruder;

    private ShapeExtruder rightRailExtruder;

    private ShapeExtruder metalFrontCrossTieExtruder_1;

    private ShapeExtruder metalFrontCrossTieExtruder_2;

    private ShapeExtruder metalFrontCrossTieExtruder_3;

    private ShapeExtruder metalRearCrossTieExtruder_1;

    private ShapeExtruder metalRearCrossTieExtruder_2;

    private ShapeExtruder metalRearCrossTieExtruder_3;

    private ShapeExtruder metalIBeamExtruder_1;

    private ShapeExtruder metalIBeamExtruder_2;

    private ShapeExtruder metalIBeamExtruder_3;

    private BoxExtruder woodenVerticalSupportPostExtruder;

    private BoxExtruder collisionMeshExtruder;

    private StreamWriter streamWriter;

    private float errorMargin90deg = 0.001f;

    private float supportBeamExtension = 0.2f;

    private float beamWidth = 0.98f;

    private float invertHeadSpace = 1f;

    private float iBeamBankingSwitch = 30.0f;

    public string path;

    protected override void Initialize()
    {
        base.Initialize();
        trackWidth = 0.41f;
    }

    public override void prepare(TrackSegment4 trackSegment, GameObject putMeshOnGO)
    {
        base.prepare(trackSegment, putMeshOnGO);
        putMeshOnGO.GetComponent<Renderer>().sharedMaterial = material;
        leftRailExtruder = new ShapeExtruder();
        leftRailExtruder.setShape(new Vector3[14]
        {
            new Vector3(0.046103f, 0f, 0f),
            new Vector3(0.048103f, -0.002f, 0f),
            new Vector3(0.048103f, -0.026f, 0f),
            new Vector3(0.046103f, -0.028f, 0f),
            new Vector3(0.021286f, -0.028f, 0f),
            new Vector3(0.021286f, -0.1144f, 0f),
            new Vector3(0.048772f, -0.1144f, 0f),
            new Vector3(0.048772f, -0.119108f, 0f),
            new Vector3(-0.054515f, -0.119108f, 0f),
            new Vector3(-0.054515f, -0.1144f, 0f),
            new Vector3(-0.032771f, -0.1144f, 0f),
            new Vector3(-0.032771f, -0.0065f, 0f),
            new Vector3(-0.054515f, -0.0065f, 0f),
            new Vector3(-0.054515f, 0f, 0f)
        }, true);
        leftRailExtruder.setUV(15, 15);
        rightRailExtruder = new ShapeExtruder();
        rightRailExtruder.setShape(new Vector3[14]
        {
            new Vector3(0.054515f, 0f, 0f),
            new Vector3(0.054515f, -0.0065f, 0f),
            new Vector3(0.032771f, -0.0065f, 0f),
            new Vector3(0.032771f, -0.1144f, 0f),
            new Vector3(0.054515f, -0.1144f, 0f),
            new Vector3(0.054515f, -0.119108f, 0f),
            new Vector3(-0.048772f, -0.119108f, 0f),
            new Vector3(-0.048772f, -0.1144f, 0f),
            new Vector3(-0.021286f, -0.1144f, 0f),
            new Vector3(-0.021286f, -0.028f, 0f),
            new Vector3(-0.046103f, -0.028f, 0f),
            new Vector3(-0.048103f, -0.026f, 0f),
            new Vector3(-0.048103f, -0.002f, 0f),
            new Vector3(-0.046103f, 0f, 0f)
        }, true);
        rightRailExtruder.setUV(14, 15);
        metalFrontCrossTieExtruder_1 = new ShapeExtruder();
        metalFrontCrossTieExtruder_1.setShape(new Vector3[4]
        {
            new Vector3(0.022f, 0.04f, 0f),
            new Vector3(0.046f, 0.04f, 0f),
            new Vector3(0.046f, 0.035f, 0f),
            new Vector3(0.03f, 0.035f, 0f)
        }, true);
        metalFrontCrossTieExtruder_1.setUV(15, 14);
        metalFrontCrossTieExtruder_1.closeEnds = true;
        metalFrontCrossTieExtruder_2 = new ShapeExtruder();
        metalFrontCrossTieExtruder_2.setShape(new Vector3[4]
        {
            new Vector3(0.03f, 0.035f, 0f),
            new Vector3(0.03f, -0.035f, 0f),
            new Vector3(0.022f, -0.04f, 0f),
            new Vector3(0.022f, 0.04f, 0f)
        }, true);
        metalFrontCrossTieExtruder_2.setUV(15, 14);
        metalFrontCrossTieExtruder_2.closeEnds = true;
        metalFrontCrossTieExtruder_3 = new ShapeExtruder();
        metalFrontCrossTieExtruder_3.setShape(new Vector3[4]
        {
            new Vector3(0.03f, -0.035f, 0f),
            new Vector3(0.046f, -0.035f, 0f),
            new Vector3(0.046f, -0.04f, 0f),
            new Vector3(0.022f, -0.04f, 0f)
        }, true);
        metalFrontCrossTieExtruder_3.setUV(15, 14);
        metalFrontCrossTieExtruder_3.closeEnds = true;
        metalRearCrossTieExtruder_1 = new ShapeExtruder();
        metalRearCrossTieExtruder_1.setShape(new Vector3[4]
        {
            new Vector3(-0.046f, 0.04f, 0f),
            new Vector3(-0.022f, 0.04f, 0f),
            new Vector3(-0.03f, 0.035f, 0f),
            new Vector3(-0.046f, 0.035f, 0f)
        }, true);
        metalRearCrossTieExtruder_1.setUV(15, 14);
        metalRearCrossTieExtruder_1.closeEnds = true;
        metalRearCrossTieExtruder_2 = new ShapeExtruder();
        metalRearCrossTieExtruder_2.setShape(new Vector3[4]
        {
            new Vector3(-0.046f, -0.035f, 0f),
            new Vector3(-0.03f, -0.035f, 0f),
            new Vector3(-0.022f, -0.04f, 0f),
            new Vector3(-0.046f, -0.04f, 0f)
        }, true);
        metalRearCrossTieExtruder_2.setUV(15, 14);
        metalRearCrossTieExtruder_2.closeEnds = true;
        metalRearCrossTieExtruder_3 = new ShapeExtruder();
        metalRearCrossTieExtruder_3.setShape(new Vector3[4]
        {
            new Vector3(-0.022f, 0.04f, 0f),
            new Vector3(-0.022f, -0.04f, 0f),
            new Vector3(-0.03f, -0.035f, 0f),
            new Vector3(-0.03f, 0.035f, 0f)
        }, true);
        metalRearCrossTieExtruder_3.setUV(15, 14);
        metalRearCrossTieExtruder_3.closeEnds = true;
        metalIBeamExtruder_1 = new ShapeExtruder();
        metalIBeamExtruder_1.setShape(new Vector3[4]
        {
            new Vector3(-0.021f, 0.04f, 0f),
            new Vector3(0.021f, 0.04f, 0f),
            new Vector3(0.021f, 0.035f, 0f),
            new Vector3(-0.021f, 0.035f, 0f)
        }, true);
        metalIBeamExtruder_1.setUV(15, 14);
        metalIBeamExtruder_1.closeEnds = true;
        metalIBeamExtruder_2 = new ShapeExtruder();
        metalIBeamExtruder_2.setShape(new Vector3[4]
        {
            new Vector3(-0.005f, 0.037f, 0f),
            new Vector3(0.005f, 0.037f, 0f),
            new Vector3(0.005f, -0.037f, 0f),
            new Vector3(-0.005f, -0.037f, 0f)
        }, true);
        metalIBeamExtruder_2.setUV(15, 14);
        metalIBeamExtruder_2.closeEnds = true;
        metalIBeamExtruder_3 = new ShapeExtruder();
        metalIBeamExtruder_3.setShape(new Vector3[4]
        {
            new Vector3(-0.021f, -0.035f, 0f),
            new Vector3(0.021f, -0.035f, 0f),
            new Vector3(0.021f, -0.04f, 0f),
            new Vector3(-0.021f, -0.04f, 0f)
        }, true);
        metalIBeamExtruder_3.setUV(15, 14);
        metalIBeamExtruder_3.closeEnds = true;
        collisionMeshExtruder = new BoxExtruder(trackWidth, 0.02666f);
        buildVolumeMeshExtruder = new BoxExtruder(trackWidth, 0.8f);
        buildVolumeMeshExtruder.closeEnds = true;
        woodenVerticalSupportPostExtruder = new BoxExtruder(0.043f, 0.043f);
        woodenVerticalSupportPostExtruder.closeEnds = true;
        woodenVerticalSupportPostExtruder.setUV(14, 14);
        base.setModelExtruders(leftRailExtruder, rightRailExtruder, metalFrontCrossTieExtruder_1, metalFrontCrossTieExtruder_2, metalFrontCrossTieExtruder_3, metalRearCrossTieExtruder_1, metalRearCrossTieExtruder_2, metalRearCrossTieExtruder_3, metalIBeamExtruder_1, metalIBeamExtruder_2, metalIBeamExtruder_3, woodenVerticalSupportPostExtruder);
    }

    public override void sampleAt(TrackSegment4 trackSegment, float t)
    {
        base.sampleAt(trackSegment, t);
        Vector3 normal = trackSegment.getNormal(t);
        Vector3 trackPivot = getTrackPivot(trackSegment.getPoint(t), normal);
        Vector3 tangentPoint = trackSegment.getTangentPoint(t);
        Vector3 normalized = Vector3.Cross(normal, tangentPoint).normalized;
        Vector3 middlePoint = trackPivot + normalized * trackWidth / 2f;
        Vector3 middlePoint2 = trackPivot - normalized * trackWidth / 2f;
        Vector3 vector = trackPivot + normal * getCenterPointOffsetY();
        leftRailExtruder.extrude(middlePoint, tangentPoint, normal);
        rightRailExtruder.extrude(middlePoint2, tangentPoint, normal);
        collisionMeshExtruder.extrude(trackPivot, tangentPoint, normal);
        if (liftExtruder != null)
        {
            liftExtruder.extrude(vector - normal * (0.16f + chainLiftHeight / 2f), tangentPoint, normal);
        }
    }

    public override void afterExtrusion(TrackSegment4 trackSegment, GameObject putMeshOnGO)
    {
        base.afterExtrusion(trackSegment, putMeshOnGO);
        float sample = trackSegment.getLength(0) / (float)Mathf.RoundToInt(trackSegment.getLength(0) / this.crossBeamSpacing);
        float pos = 0.25f;
        int index = 0;
        while (pos < trackSegment.getLength(0))
        {
            float tForDistance = trackSegment.getTForDistance(pos, 0);

            index++;
            pos += sample;

            Vector3 normal = trackSegment.getNormal(tForDistance);
            Vector3 tangentPoint = trackSegment.getTangentPoint(tForDistance);
            Vector3 binormal = Vector3.Cross(normal, tangentPoint).normalized;
            Vector3 trackPivot = base.getTrackPivot(trackSegment.getPoint(tForDistance, 0), normal);
            
            bool trainFacingXPositive = tangentPoint.x > Mathf.Abs(tangentPoint.z);
            bool trainFacingXNegative = tangentPoint.x < -Mathf.Abs(tangentPoint.z);
            bool trainFacingZPositive = tangentPoint.z > Mathf.Abs(tangentPoint.x);
            bool trainFacingZNegative = tangentPoint.z < -Mathf.Abs(tangentPoint.x);

            float trackBanking = 0f;

            Vector3 bottomBeamDirection = new Vector3();

            if (trainFacingXPositive)
            {
                trackBanking = Mathf.Repeat(Mathf.Atan2(normal.z, -normal.y), Mathf.PI * 2.0f) * Mathf.Rad2Deg;
                if (trackBanking > 180)
                    trackBanking -= 360;
                bottomBeamDirection.z = Math.Abs(trackBanking) <= 90 ? -1.0f : 1.0f;
            }
            if (trainFacingXNegative)
            {
                trackBanking = Mathf.Repeat(Mathf.Atan2(-normal.z, -normal.y), Mathf.PI * 2.0f) * Mathf.Rad2Deg;
                if (trackBanking > 180)
                    trackBanking -= 360;
                bottomBeamDirection.z = Math.Abs(trackBanking) <= 90 ? 1.0f : -1.0f;
            }
            if (trainFacingZPositive)
            {
                trackBanking = Mathf.Repeat(Mathf.Atan2(normal.x, -normal.y), Mathf.PI * 2.0f) * Mathf.Rad2Deg;
                if (trackBanking > 180)
                    trackBanking -= 360;
                bottomBeamDirection.x = Math.Abs(trackBanking) > 90 ? -1.0f : 1.0f;
            }
            if (trainFacingZNegative)
            {
                trackBanking = Mathf.Repeat(Mathf.Atan2(-normal.x, -normal.y), Mathf.PI * 2.0f) * Mathf.Rad2Deg;
                if (trackBanking > 180)
                    trackBanking -= 360;
                bottomBeamDirection.x = Math.Abs(trackBanking) > 90 ? 1.0f : -1.0f;
            }

            //track beam
            Vector3 startPoint = trackPivot + normal * 0.159107f + binormal * (beamWidth / 2);
            Vector3 endPoint = trackPivot + normal * 0.159107f - binormal * (beamWidth / 2);
            
            bool equalHeight = Mathf.Abs(startPoint.y - endPoint.y) < 0.97f;

            if (Mathf.Abs(trackBanking) > iBeamBankingSwitch)
            {
                metalIBeamExtrude(startPoint, -1f * binormal, equalHeight ? Vector3.up : normal);
                metalIBeamExtrude(endPoint, -1f * binormal, equalHeight ? Vector3.up : normal);
                metalIBeamEnd();
            }
            else
            {
                float distance = (beamWidth) / Mathf.Sin((90-Mathf.Abs(trackBanking)) * Mathf.Deg2Rad) - (beamWidth/2);
                endPoint = trackPivot + normal * 0.159107f - binormal * distance;
                metalCrossTieExtrude(startPoint, -1f * binormal, equalHeight ? Vector3.up : normal);
                metalCrossTieExtrude(endPoint, -1f * binormal, equalHeight ? Vector3.up : normal);
                metalCrossTieEnd();
            }

            if (!(trackSegment is Station))
            {
                //Bottom beam calculation
                Vector3 bottomBeamPivot = new Vector3(trackPivot.x, Mathf.Min(startPoint.y, endPoint.y), trackPivot.z);

                Vector3 bottomBeamStart = new Vector3();
                Vector3 bottomBeamEnd = new Vector3();

                Vector3 bottomBeamBinormal = bottomBeamDirection.normalized;
                if (((trainFacingXNegative || trainFacingXPositive) ? -1.0f : 1.0) * ((Mathf.Abs(trackBanking) <= 90) ? -1.0f : 1.0f) * trackBanking < 0)
                {
                    bottomBeamStart.x = endPoint.x;
                    bottomBeamStart.z = endPoint.z;
                    bottomBeamStart.y = endPoint.y > startPoint.y ? startPoint.y : endPoint.y;

                    bottomBeamEnd = bottomBeamStart - bottomBeamDirection.normalized * beamWidth;
                }
                else
                {
                    bottomBeamEnd.x = startPoint.x;
                    bottomBeamEnd.z = startPoint.z;
                    bottomBeamEnd.y = endPoint.y > startPoint.y ? startPoint.y : endPoint.y;

                    bottomBeamStart = bottomBeamEnd + bottomBeamDirection.normalized * beamWidth;
                }

                if (Mathf.Abs(trackBanking) > 90)
                {
                    bottomBeamStart.y -= ((Mathf.Abs(trackBanking)/90)-1) * invertHeadSpace;
                    bottomBeamEnd.y -= ((Mathf.Abs(trackBanking) / 90) - 1) * invertHeadSpace;
                }

                //Bottom beam extruding
                if (Mathf.Abs(trackBanking) > iBeamBankingSwitch)
                {
                    metalCrossTieExtrude(bottomBeamEnd, -1f * bottomBeamBinormal, Vector3.up);
                    metalCrossTieExtrude(bottomBeamStart, -1f * bottomBeamBinormal, Vector3.up);
                    metalCrossTieEnd();
                }

                //Top beam extruding
                if (trackBanking > 90 || trackBanking < -90)
                {
                    metalCrossTieExtrude(new Vector3(bottomBeamEnd.x, Mathf.Max(startPoint.y, endPoint.y), bottomBeamEnd.z), -1f * bottomBeamBinormal, Vector3.up);
                    metalCrossTieExtrude(new Vector3(bottomBeamStart.x, Mathf.Max(startPoint.y, endPoint.y), bottomBeamStart.z), -1f * bottomBeamBinormal, Vector3.up);
                    metalCrossTieEnd();
                }
                LandPatch terrain = GameController.Instance.park.getTerrain(trackPivot);

                if (terrain != null)
                {
                    float lowest = terrain.getLowestHeight();

                    Vector3 projectedTangentDirection = tangentPoint;
                    projectedTangentDirection.y = 0;
                    projectedTangentDirection.Normalize();
                    Vector3 leftVerticalSupportPost = new Vector3(bottomBeamEnd.x, startPoint.y + supportBeamExtension, bottomBeamEnd.z);
                    Vector3 rightVerticalSupportPost = new Vector3(bottomBeamStart.x, endPoint.y + supportBeamExtension, bottomBeamStart.z);

                    if (normal.y > errorMargin90deg)
                    {
                        leftVerticalSupportPost.y = Mathf.Max(startPoint.y, endPoint.y) + supportBeamExtension;
                        rightVerticalSupportPost.y = Mathf.Max(startPoint.y, endPoint.y) + supportBeamExtension;
                    }

                    //left post
                    woodenVerticalSupportPostExtruder.extrude(leftVerticalSupportPost, new Vector3(0, -1, 0), projectedTangentDirection);
                    woodenVerticalSupportPostExtruder.extrude(new Vector3(leftVerticalSupportPost.x, lowest, leftVerticalSupportPost.z), new Vector3(0, -1, 0), projectedTangentDirection);
                    woodenVerticalSupportPostExtruder.end();

                    //right post
                    woodenVerticalSupportPostExtruder.extrude(rightVerticalSupportPost, new Vector3(0, -1, 0), projectedTangentDirection);
                    woodenVerticalSupportPostExtruder.extrude(new Vector3(rightVerticalSupportPost.x, lowest, rightVerticalSupportPost.z), new Vector3(0, -1, 0), projectedTangentDirection);
                    woodenVerticalSupportPostExtruder.end();
                }

            }
        }
    }

    public override Mesh getMesh(GameObject putMeshOnGO)
    {
        return MeshCombiner.start().add(leftRailExtruder, rightRailExtruder, metalFrontCrossTieExtruder_1, metalFrontCrossTieExtruder_2, metalFrontCrossTieExtruder_3, metalRearCrossTieExtruder_1, metalRearCrossTieExtruder_2, metalRearCrossTieExtruder_3, metalIBeamExtruder_1, metalIBeamExtruder_2, metalIBeamExtruder_3, woodenVerticalSupportPostExtruder).end(putMeshOnGO.transform.worldToLocalMatrix);
    }

    public override Mesh getCollisionMesh(GameObject putMeshOnGO)
    {
        return collisionMeshExtruder.getMesh(putMeshOnGO.transform.worldToLocalMatrix);
    }

    public override Extruder getBuildVolumeMeshExtruder()
    {
        return buildVolumeMeshExtruder;
    }

    public override float getCenterPointOffsetY()
    {
        return 0.27f;
    }

    public override float trackOffsetY()
    {
        return 0.24f;
    }

    public override float getTunnelOffsetY()
    {
        return 0.15f;
    }

    public override float getTunnelWidth()
    {
        return 0.8f;
    }

    public override float getTunnelHeight()
    {
        return 0.8f;
    }
    public override float getFrictionWheelOffsetY()
    {
        return 0.115f;
    }
    protected override float railHalfHeight()
    {
        return 0.02666f;
    }
    public void metalCrossTieExtrude(Vector3 middlePoint, Vector3 tangent, Vector3 normal)
    {
        metalFrontCrossTieExtruder_1.extrude(middlePoint, tangent, normal);
        metalFrontCrossTieExtruder_2.extrude(middlePoint, tangent, normal);
        metalFrontCrossTieExtruder_3.extrude(middlePoint, tangent, normal);
        metalRearCrossTieExtruder_1.extrude(middlePoint, tangent, normal);
        metalRearCrossTieExtruder_2.extrude(middlePoint, tangent, normal);
        metalRearCrossTieExtruder_3.extrude(middlePoint, tangent, normal);
    }
    public void metalIBeamExtrude(Vector3 middlePoint, Vector3 tangent, Vector3 normal)
    {
        metalIBeamExtruder_1.extrude(middlePoint, tangent, normal);
        metalIBeamExtruder_2.extrude(middlePoint, tangent, normal);
        metalIBeamExtruder_3.extrude(middlePoint, tangent, normal);
    }
    public void metalCrossTieEnd()
    {
        metalFrontCrossTieExtruder_1.end();
        metalFrontCrossTieExtruder_2.end();
        metalFrontCrossTieExtruder_3.end();
        metalRearCrossTieExtruder_1.end();
        metalRearCrossTieExtruder_2.end();
        metalRearCrossTieExtruder_3.end();
    }
    public void metalIBeamEnd()
    {
        metalIBeamExtruder_1.end();
        metalIBeamExtruder_2.end();
        metalIBeamExtruder_3.end();
    }
    public void WriteToFile(string text)
    {
        streamWriter = File.AppendText(path + @"/mod.log");
        streamWriter.WriteLine(DateTime.Now + ": " + text);
        streamWriter.Flush();
        streamWriter.Close();
    }
}
