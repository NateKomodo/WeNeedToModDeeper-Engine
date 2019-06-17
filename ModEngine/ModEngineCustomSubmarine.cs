using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using WeNeedToModDeeperEngine;

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

        public void Save(string path)
        {
            File.WriteAllText(path, JsonConvert.SerializeObject(myManifest));
        }

        public void LoadFromManifest()
        {
            if (myManifest == null) return;

            //Load meta
            ModEngineVariables.Substats.boostJuice = myManifest.fuel;
            ModEngineVariables.Substats.NetworkmaxBoostJuice = myManifest.fuel;
            ModEngineVariables.Substats.boostJuiceDrainRate = myManifest.fuelDrainRate;
            ModEngineVariables.Substats.NetworkmaxSubHealth = myManifest.hull;
            ModEngineVariables.Substats.NetworksubHealth = myManifest.hull;

            foreach (var render in GameObject.Find("Submarine").GetComponentsInChildren<SpriteRenderer>()) render.sprite = myManifest.subSprite;
            GameObject.Find("InteractableSub").GetComponentInChildren<SpriteRenderer>().sprite = myManifest.subSprite;

            //Build sub
            foreach (var item in myManifest.manifest)
            {
                try
                {
                    switch (item.mode)
                    {
                        case CreationMode.CREATE:
                            Debug.Log($"[CSUB] Creating new obj of type {item.itemName} at {item.pos.x}, {item.pos.y}");
                            var newgo = GameObject.Instantiate(prefabs[item.itemName], item.pos, Quaternion.identity);
                            newgo.transform.SetParent(GameObject.Find("SubmarineInteriorSmall").transform);
                            newgo.transform.position = item.pos;
                            newgo.name = item.itemName;
                            newgo.SetActive(true);
                            newgo.transform.localScale = new Vector2(item.xscale, newgo.transform.localScale.y);
                            break;
                        case CreationMode.MOVE:
                            Debug.Log($"[CSUB] Moving obj of type {item.itemName} to {item.pos.x}, {item.pos.y}");
                            var go = GameObject.Find(item.itemName);
                            go.transform.position = item.pos;
                            go.transform.localScale = new Vector2(item.xscale, go.transform.localScale.y);
                            break;
                        case CreationMode.CUSTOM:
                            Debug.Log($"[CSUB] Creating custom object at {item.pos.x}, {item.pos.y}");
                            var cgo = GameObject.Instantiate(prefabs["SubmarineInteriorChunk1_03"], item.pos, Quaternion.identity);
                            cgo.transform.SetParent(GameObject.Find("SubmarineInteriorSmall").transform);
                            cgo.transform.position = item.pos;
                            cgo.SetActive(true);
                            cgo.name = "customBGitem";
                            cgo.transform.localScale = new Vector2(item.xscale, cgo.transform.localScale.y);
                            cgo.GetComponentInChildren<SpriteRenderer>().sprite = item.customSprite;
                            break;
                    }
                }
                catch
                {
                    Debug.Log($"[CSUB] Failed to proccess gameobject {item.itemName} at {item.pos.x}, {item.pos.y}");
                }
            }
        }

        public void DestroyOld()
        {
            foreach (var go in toDestroy)
            {
                try
                {
                    Debug.Log($"[CSUB] Destroying GO {go.name}");
                    GameObject.Destroy(go);
                }
                catch
                {
                    Debug.Log("[CSUB] Exception. Most likely already destroyed GO");
                }
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
            alias.Add("bg", "SubmarineInteriorChunk1_03");
        }
    }

    public class BuildManifest
    {
        public Sprite subSprite;

        public int hull;

        public float fuel;

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
