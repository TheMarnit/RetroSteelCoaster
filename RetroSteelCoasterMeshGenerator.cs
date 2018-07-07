// RetroSteelCoasterMeshGenerator
using UnityEngine;

public class RetroSteelCoasterMeshGenerator : MeshGenerator
{
    private TubeExtruder centerTubeExtruder;

    private BoxExtruder centerBoxExtruder;

    private TubeExtruder leftTubeExtruder;

    private TubeExtruder rightTubeExtruder;

    private ResizableTubeExtruder centerCrossTubeExtruder;

    private ResizableTubeExtruder sideCrossTubeExtruder;

    private BoxExtruder crossBoxExtruder;

    private BoxExtruder collisionMeshExtruder;

    private const float sideTubesRadius = 0.02665f;

    private const int sideTubesVertCount = 6;

    private const float centerTubeRadius = 0.08f;

    private const int centerTubeVertCount = 8;

    private const float buildVolumeHeight = 0.8f;

    protected override void Initialize()
    {
        base.Initialize();
        base.trackWidth = 0.45234f;
        base.chainLiftHeight = 0.1f;
    }

    public override void prepare(TrackSegment4 trackSegment, GameObject putMeshOnGO)
    {
        if (trackSegment.track.TrackedRide.everyUpIsLift)
        {
            Vector3 endpoint = trackSegment.getEndpoint();
            float y = endpoint.y;
            Vector3 startpoint = trackSegment.getStartpoint();
            if (y - startpoint.y > 0.1f)
            {
                trackSegment.isLifthill = true;
            }
        }
        liftExtruder = null;
        
        if (trackSegment.isLifthill && (trackSegment is ChangeHeight4 || trackSegment.getStartpoint().y != trackSegment.getEndpoint().y || (UnityEngine.Object)frictionWheelsGO == (UnityEngine.Object)null))
        {
            liftExtruder = instantiateLiftExtruder(trackSegment);
        }
        lsmExtruder = null;
        if (trackSegment.isLaunch)
        {
            lsmExtruder = instantiateLSMExtruder(trackSegment);
        }
        brakeExtruder = null;
        if (useBrakeGraphics(trackSegment) && (trackSegment.isBrake() || trackSegment is Station))
        {
            Vector3 normal = trackSegment.getNormal(0f);
            Vector3 trackPivot = getTrackPivot(trackSegment.getPoint(0f, 0), normal);
            Vector3 tangentPoint = trackSegment.getTangentPoint(0f);
            brakeExtruder = new BrakeExtruder(trackPivot, tangentPoint, normal);
        }
        if ((UnityEngine.Object)tunnelMeshGenerator != (UnityEngine.Object)null)
        {
            tunnelMeshGenerator.prepare(trackSegment);
        }

        putMeshOnGO.GetComponent<Renderer>().sharedMaterial = base.material;
        centerTubeExtruder = new TubeExtruder(centerTubeRadius, centerTubeVertCount);
        centerTubeExtruder.setUV(14, 15);
        centerTubeExtruder.closeEnds = true;
        centerBoxExtruder = new BoxExtruder(centerTubeRadius * 2f, centerTubeRadius * 2f);
        centerBoxExtruder.setUV(14, 15);
        centerBoxExtruder.closeEnds = true;
        leftTubeExtruder = new TubeExtruder(sideTubesRadius, sideTubesVertCount);
        leftTubeExtruder.setUV(15, 14);
        rightTubeExtruder = new TubeExtruder(sideTubesRadius, sideTubesVertCount);
        rightTubeExtruder.setUV(15, 14);
        centerCrossTubeExtruder = new ResizableTubeExtruder(1f, centerTubeVertCount);
        centerCrossTubeExtruder.setUV(14, 15);
        centerCrossTubeExtruder.closeEnds = true;
        sideCrossTubeExtruder = new ResizableTubeExtruder(1f, centerTubeVertCount);
        sideCrossTubeExtruder.setUV(15, 14);
        crossBoxExtruder = new BoxExtruder(sideTubesRadius * 2f, sideTubesRadius * 1.8f);
        crossBoxExtruder.setUV(14, 14);
        collisionMeshExtruder = new BoxExtruder(base.trackWidth, 0.02665f);
        base.buildVolumeMeshExtruder = new BoxExtruder(base.trackWidth, 0.8f);
        base.buildVolumeMeshExtruder.closeEnds = true;
        base.setModelExtruders(centerTubeExtruder, leftTubeExtruder, rightTubeExtruder);
    }

    public override void sampleAt(TrackSegment4 trackSegment, float t)
    {
        base.sampleAt(trackSegment, t);
        Vector3 normal = trackSegment.getNormal(t);
        Vector3 trackPivot = base.getTrackPivot(trackSegment.getPoint(t, 0), normal);
        Vector3 tangentPoint = trackSegment.getTangentPoint(t);
        Vector3 normalized = Vector3.Cross(normal, tangentPoint).normalized;
        Vector3 middlePoint = trackPivot + normalized * base.trackWidth / 2f;
        Vector3 middlePoint2 = trackPivot - normalized * base.trackWidth / 2f;
        Vector3 vector = trackPivot + normal * getCenterPointOffsetY();
        if (trackSegment is Station || trackSegment is Brake || (trackSegment.isLifthill && base.liftExtruder == null))
        {
            centerBoxExtruder.extrude(trackPivot + normal * base.trackWidth / 3f, tangentPoint, normal);
        }
        else { 
            centerTubeExtruder.extrude(trackPivot, tangentPoint, normal);
        }
        leftTubeExtruder.extrude(middlePoint, tangentPoint, normal);
        rightTubeExtruder.extrude(middlePoint2, tangentPoint, normal);
        collisionMeshExtruder.extrude(trackPivot, tangentPoint, normal);
        if (base.liftExtruder != null)
        {
            base.liftExtruder.setUV(14, 14);
            base.liftExtruder.extrude(vector - (normal * 0.23f), tangentPoint, normal);
        }
    }

    public override void afterExtrusion(TrackSegment4 trackSegment, GameObject putMeshOnGO)
    {
        base.afterExtrusion(trackSegment, putMeshOnGO);
        float tieInterval = trackSegment.getLength(0) / (float)Mathf.RoundToInt(trackSegment.getLength(0) / 0.3f);
        float pos = 0;
        
        while (pos <= trackSegment.getLength(0) + 0.1f)
        {
            float tForDistance = trackSegment.getTForDistance(pos, 0);
            pos += tieInterval;
            Vector3 normal = trackSegment.getNormal(tForDistance);
            Vector3 tangentPoint = trackSegment.getTangentPoint(tForDistance);
            Vector3 binormal = Vector3.Cross(normal, tangentPoint).normalized;
            Vector3 trackPivot = base.getTrackPivot(trackSegment.getPoint(tForDistance, 0), normal);

            if (trackSegment is Station || trackSegment is Brake || (trackSegment.isLifthill && base.liftExtruder == null))
            {
                crossBoxExtruder.setHeight(sideTubesRadius);
                crossBoxExtruder.extrude((trackPivot - binormal * base.trackWidth / 2f) + (normal * sideTubesRadius * 0.5f), binormal, normal);
                crossBoxExtruder.setHeight(sideTubesRadius * 3f);
                crossBoxExtruder.extrude((trackPivot - binormal * base.trackWidth / 4f) + (normal * sideTubesRadius * 1.5f), binormal, normal);
                crossBoxExtruder.extrude((trackPivot + binormal * base.trackWidth / 4f) + (normal * sideTubesRadius * 1.5f), binormal, normal);
                crossBoxExtruder.setHeight(sideTubesRadius);
                crossBoxExtruder.extrude((trackPivot + binormal * base.trackWidth / 2f) + (normal * sideTubesRadius * 0.5f), binormal, normal);
                crossBoxExtruder.end();
            }
            else
            {
                centerCrossTubeExtruder.setRadius(centerTubeRadius / 3f);
                centerCrossTubeExtruder.extrude(trackPivot - binormal * base.trackWidth / 3f, binormal, normal);
                centerCrossTubeExtruder.setRadius(centerTubeRadius / 2f);
                centerCrossTubeExtruder.extrude(trackPivot, binormal, normal);
                centerCrossTubeExtruder.setRadius(centerTubeRadius / 3f);
                centerCrossTubeExtruder.extrude(trackPivot + binormal * base.trackWidth / 3f, binormal, normal);
                centerCrossTubeExtruder.end();

                sideCrossTubeExtruder.setRadius(sideTubesRadius - 0.001f);
                sideCrossTubeExtruder.extrude(trackPivot - binormal * base.trackWidth / 2f, binormal, normal);
                sideCrossTubeExtruder.setRadius(sideTubesRadius - 0.01f);
                sideCrossTubeExtruder.extrude(trackPivot - binormal * base.trackWidth / 3f, binormal, normal);
                sideCrossTubeExtruder.extrude(trackPivot + binormal * base.trackWidth / 3f, binormal, normal);
                sideCrossTubeExtruder.setRadius(sideTubesRadius - 0.001f);
                sideCrossTubeExtruder.extrude(trackPivot + binormal * base.trackWidth / 2f, binormal, normal);
                sideCrossTubeExtruder.end();
            }
        }
    }

    public override Mesh getMesh(GameObject putMeshOnGO)
    {
        return MeshCombiner.start().add(centerTubeExtruder, centerBoxExtruder, leftTubeExtruder, rightTubeExtruder, centerCrossTubeExtruder, sideCrossTubeExtruder, crossBoxExtruder).end(putMeshOnGO.transform.worldToLocalMatrix);
    }

    public override Mesh getCollisionMesh(GameObject putMeshOnGO)
    {
        return collisionMeshExtruder.getMesh(putMeshOnGO.transform.worldToLocalMatrix);
    }

    public override Extruder getBuildVolumeMeshExtruder()
    {
        return base.buildVolumeMeshExtruder;
    }

    public override float getCenterPointOffsetY()
    {
        return 0.08027f;
    }

    public override float trackOffsetY()
    {
        return 0.25f + base.heartlineOffset;
    }

    public override float getTunnelOffsetY()
    {
        return 0.2f;
    }

    public override float getTunnelWidth()
    {
        return 0.7f;
    }

    public override float getTunnelHeight()
    {
        return 1f;
    }

    protected override float railHalfHeight()
    {
        return 0.02665f;
    }
}