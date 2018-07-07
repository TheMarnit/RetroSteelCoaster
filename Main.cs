using System.Collections.Generic;
using System.Linq;
using TrackedRiderUtility;
using UnityEngine;

namespace RetroSteelCoaster
{
    public class Main : IMod
    {
        private TrackRiderBinder binder;

        GameObject _go;

        public void onEnabled()
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
            retroSteelCoaster.description = "A classic steel rollercoaster.";
            retroSteelCoasterTrackGenerator.crossBeamGO = null;

            retroSteelCoaster.price = 1500;
            retroSteelCoaster.meshGenerator.customColors = new[]
            {
                new Color (0f / 255f, 0f / 255f, 0f / 255f, 1), new Color (80f / 255f, 115f / 255f, 150f / 255f, 1),
                new Color (110f / 255f, 170f / 255f, 255f / 255f, 1), new Color (95f / 255f, 140f / 255f, 200f / 255f, 1)
            };
            retroSteelCoaster.dropsImportanceExcitement = 0.665f;
            retroSteelCoaster.inversionsImportanceExcitement = 0.673f;
            retroSteelCoaster.averageLatGImportanceExcitement = 0.121f;
            retroSteelCoaster.accelerationImportanceExcitement = 0.525f;
            binder.Apply();
        }

        public void onDisabled()
        {
            binder.Unload();
        }

        public string Name => "Retro Steel Coaster";

        public string Description => "Adds a retro steel coaster.";

        string IMod.Identifier => "Marnit@ParkitectRetroSteelCoaster";


        public string Path
        {
            get
            {
                return ModManager.Instance.getModEntries().First(x => x.mod == this).path;
            }
        }
    }
}
