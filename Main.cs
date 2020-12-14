using System.Collections.Generic;
using System.Linq;
using TrackedRiderUtility;
using UnityEngine;

namespace RetroSteelCoaster
{
    public class Main : AbstractMod
    {
        private TrackRiderBinder binder;

        GameObject _go;

        public override void onEnabled()
        {
            var dsc = System.IO.Path.DirectorySeparatorChar;


            binder = new TrackRiderBinder("kvwQwhKWWG");
            TrackedRide retroSteelCoaster =
                binder.RegisterTrackedRide<TrackedRide>("Steel Coaster", "retroSteelCoaster", "Retro Steel Coaster");
            RetroSteelCoasterMeshGenerator retroSteelCoasterTrackGenerator =
                binder.RegisterMeshGenerator<RetroSteelCoasterMeshGenerator>(retroSteelCoaster);
            TrackRideHelper.PassMeshGeneratorProperties(TrackRideHelper.GetTrackedRide("Steel Coaster").meshGenerator,
                retroSteelCoaster.meshGenerator);

            retroSteelCoaster.canCurveLifts = true;
            retroSteelCoaster.canHaveLSM = false;
            retroSteelCoaster.description = "Similar to the modern steel rollercoaster, this coaster is capable of having a curved lifthill but not of being launched.";
            retroSteelCoasterTrackGenerator.crossBeamGO = null;

            retroSteelCoaster.price = 1500;
            retroSteelCoaster.trackPricePerUnit = 25;
            retroSteelCoaster.meshGenerator.customColors = new[]
            {
                new Color (0f / 255f, 0f / 255f, 0f / 255f, 1), new Color (80f / 255f, 115f / 255f, 150f / 255f, 1),
                new Color (110f / 255f, 170f / 255f, 255f / 255f, 1), new Color (95f / 255f, 140f / 255f, 200f / 255f, 1)
            };
            retroSteelCoaster.airtimeImportanceExcitement = TrackRideHelper.GetTrackedRide("Steel Coaster").airtimeImportanceExcitement + 0.2f;
            retroSteelCoaster.dropsImportanceExcitement = TrackRideHelper.GetTrackedRide("Steel Coaster").dropsImportanceExcitement - 0.4f;
            retroSteelCoaster.inversionsImportanceExcitement = TrackRideHelper.GetTrackedRide("Steel Coaster").inversionsImportanceExcitement - 0.1f;
            retroSteelCoaster.averageLatGImportanceExcitement = TrackRideHelper.GetTrackedRide("Steel Coaster").averageLatGImportanceExcitement - 0.3f;
            retroSteelCoaster.accelerationImportanceExcitement = TrackRideHelper.GetTrackedRide("Steel Coaster").accelerationImportanceExcitement + 0.1f;
            retroSteelCoaster.velocityImportanceExcitement = TrackRideHelper.GetTrackedRide("Steel Coaster").velocityImportanceExcitement + 0.2f;
            retroSteelCoaster.excitementImportanceRideLengthTime = TrackRideHelper.GetTrackedRide("Steel Coaster").excitementImportanceRideLengthTime + 0.15f;
            binder.Apply();
        }

        public override void onDisabled()
        {
            binder.Unload();
        }

        public override string getName()
        {
            return "Retro Steel Coaster";
        }

        public override string getDescription()
        {
            return "Adds a retro steel coaster that is capable of having a curved lifthill but not of being launched";
        }

        public override string getIdentifier()
        {
            return "Marnit@ParkitectRetroSteelCoaster";
        }

        public override string getVersionNumber()
        {
            return "1.2.3";
        }

        public override bool isMultiplayerModeCompatible()
        {
            return true;
        }

        public override int getOrderPriority()
        {
            return 99;
        }

        public string Path
        {
            get
            {
                return ModManager.Instance.getModEntries().First(x => x.mod == this).path;
            }
        }
    }
}
