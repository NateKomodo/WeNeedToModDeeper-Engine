using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace ModEngine
{
    public class ModEngineCustomSubmarine
    {
        public Dictionary<string, GameObject> prefabs = new Dictionary<string, GameObject>(); //GO name => gameobject

        public Dictionary<string, string> alias = new Dictionary<string, string>(); //alias => GO name

        public List<GameObject> toDestroy = new List<GameObject>();

        public BuildManifest myManifest;

        public ModEngineCustomSubmarine(BuildManifest buildManifest)
        {
            myManifest = buildManifest;
            AddAlias();
            PopulatePrefabs();
            DestroyOld();
            LoadFromManifest();
        }

        public void LoadFromManifest()
        {
            if (myManifest == null) return;

        }

        public void DestroyOld()
        {
            foreach (var go in toDestroy)
            {
                Debug.Log($"[CSUB] Destroying GO {go.name}");
                GameObject.Destroy(go);
            }
        }

        public void PopulatePrefabs()
        {
            var subint = GameObject.Find("SubmarineInteriorSmall");
            RecursivePopulate(subint, 1);
        }

        public void RecursivePopulate(GameObject parent, int indent)
        {
            if (indent > 3) return;
            foreach (Transform child in parent.transform)
            {
                var go = child.gameObject;
                if (alias.Values.Contains(go.name))
                {
                    var newGo = GameObject.Instantiate(go);
                    newGo.SetActive(false);
                    prefabs.Add(go.name, newGo);
                }
                if (indent == 1 && !child.gameObject.name.Contains("Consoles")) toDestroy.Add(child.gameObject);
                if (indent == 2 && !(child.gameObject.name.Contains("Console") && child.gameObject.name.Contains("Rigger"))) toDestroy.Add(child.gameObject);
                RecursivePopulate(go, indent + 1);
            }
        }

        public void AddAlias()
        {
            alias.Add("door", "obj_door");
            alias.Add("doorframe", "DoorFrame");
            alias.Add("hatch", "obj_SubmarineHatch");
            alias.Add("breakzone", "BreakZoneMetal");
            alias.Add("light", "CeilingLight2");
            alias.Add("floor", "CarpetEdgeTile");
            alias.Add("roof", "CarpetEdgeTile");
            alias.Add("drain", "FloorDrain");
            alias.Add("blackout", "Blackout");
            alias.Add("transition", "Transition");
            alias.Add("ttspawnpoint", "TimeTravelerSpawnPoint");
            alias.Add("ladder", "obj_ladder_piece");
            alias.Add("floorcollider", "InteriorGroundCollider");
            alias.Add("wallcollider", "InteriorWallCollider");
        }
    }

    public class BuildManifest
    {
        public Sprite subSprite;

        public int health;

        public int fuel;

        public float fuelDrainRate;

        public List<BuildItem> manifest;
    }

    public class BuildItem
    {
        public CreationMode mode;

        public string itemName;

        public Vector2 pos;

        public float xscale;

        public Sprite customSprite;
    }

    public enum CreationMode
    {
        CREATE,
        MOVE,
        CUSTOM
    }
}
