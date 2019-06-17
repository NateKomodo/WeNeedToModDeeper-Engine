using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace WeNeedToModDeeperEngine
{
    public class ModEngineCustomSubmarine
    {
        public Dictionary<string, GameObject> prefabs = new Dictionary<string, GameObject>(); //GO name => gameobject

        public Dictionary<string, string> alias = new Dictionary<string, string>(); //alias => GO name

        public List<GameObject> toDestroy = new List<GameObject>();

        public BuildManifest myManifest;

        public bool corperative;

        public ModEngineCustomSubmarine(BuildManifest buildManifest, bool corperativeMode)
        {
            if (buildManifest == null) myManifest = new BuildManifest(); else myManifest = buildManifest;
            corperative = corperativeMode;
            if (myManifest.wasBuiltCoOp) corperative = true;
            AddAlias();
            PopulatePrefabs();
            DestroyOld();
            LoadFromManifest();
        }

        public GameObject CreateComponent(string name, Vector2 pos, string tag = null, int xscale = 0)
        {
            Debug.Log($"[CSUB] Creating new obj of type {name} at {pos.x}, {pos.y}");
            var newgo = GameObject.Instantiate(prefabs[name], pos, Quaternion.identity);
            newgo.transform.SetParent(GameObject.Find("SubmarineInteriorSmall").transform);
            newgo.transform.position = pos;
            newgo.name = name;
            if (tag != null) newgo.tag = tag;
            newgo.SetActive(true);
            if (xscale != 0) newgo.transform.localScale = new Vector2(xscale, newgo.transform.localScale.y);
            myManifest.manifest.Add(new BuildItem() { tag = tag, itemName = name, mode = CreationMode.CREATE, pos = new Position() { x = pos.x, y = pos.y}, xscale = xscale, refObj = newgo });
            return newgo;
        }

        public GameObject CreateCustomComponent(Sprite sprite, Vector2 pos, int xscale = 0)
        {
            Debug.Log($"[CSUB] Creating custom object at {pos.x}, {pos.y}");
            var cgo = GameObject.Instantiate(prefabs["SubmarineInteriorChunk1_03"], pos, Quaternion.identity);
            cgo.transform.SetParent(GameObject.Find("SubmarineInteriorSmall").transform);
            cgo.transform.position = pos;
            cgo.name = "customBGitem";
            cgo.SetActive(true);
            if (xscale != 0) cgo.transform.localScale = new Vector2(xscale, cgo.transform.localScale.y);
            cgo.GetComponentInChildren<SpriteRenderer>().sprite = sprite;
            myManifest.manifest.Add(new BuildItem() { itemName = "customBGitem", mode = CreationMode.CREATE, pos = new Position() { x = pos.x, y = pos.y}, xscale = xscale, refObj = cgo });
            return cgo;
        }

        public void Save(string path)
        {
            var altList = myManifest.manifest;
            foreach (var i in altList)
            {
                var go = i.refObj;
                if (go == null) continue;
                myManifest.manifest.Find(j => j.refObj == i.refObj).pos = new Position() { x = go.transform.position.x, y = go.transform.position.y };
            }
            myManifest.manifest.RemoveAll(i =>
            {
                if (i.refObj == null)
                {
                    Debug.Log($"RefObj for {i.itemName} was null, removing");
                    return true;
                }
                return false;
            });
            foreach (var val in myManifest.manifest.ToArray())
            {
                if (val.refObj != null) myManifest.manifest.Find(i => i.refObj = val.refObj).refObj = null;
            }
            FileStream stream = File.Create(path);
            var formatter = new BinaryFormatter();
            formatter.Serialize(stream, myManifest);
            stream.Close();
        }

        public void LoadFromManifest()
        {
            if (myManifest == null) return;

            //Load meta
            ModEngineVariables.Substats.boostJuice = myManifest.fuel;
            ModEngineVariables.Substats.NetworkmaxBoostJuice = myManifest.fuel;
            ModEngineVariables.Substats.boostJuiceDrainRate = myManifest.fuelDrainRate;
            if (myManifest.hull > 10)
            {
                ModEngineVariables.Substats.NetworkmaxSubHealth = myManifest.hull;
                ModEngineVariables.Substats.NetworksubHealth = myManifest.hull;
            }
            Debug.Log("[CSUB] finished meta");
            if (myManifest.subSprite != null)
            {
                foreach (var render in GameObject.Find("Submarine").GetComponentsInChildren<SpriteRenderer>()) render.sprite = LoadSpriteFromFile(myManifest.subSprite, Vector2.zero);
                GameObject.Find("InteractableSub").GetComponentInChildren<SpriteRenderer>().sprite = LoadSpriteFromFile(myManifest.subSprite, Vector2.zero);
            }
            Debug.Log("[CSUB] finished sprite");
            //Build sub
            foreach (var item in myManifest.manifest.ToArray())
            {
                try
                {
                    switch (item.mode)
                    {
                        case CreationMode.CREATE:
                            Debug.Log($"[CSUB] Creating new obj of type {item.itemName} at {item.pos.x}, {item.pos.y}");
                            var newgo = GameObject.Instantiate(prefabs[item.itemName], new Vector2(item.pos.x, item.pos.y), Quaternion.identity);
                            newgo.transform.SetParent(GameObject.Find("SubmarineInteriorSmall").transform);
                            newgo.transform.position = new Vector2(item.pos.x, item.pos.y);
                            newgo.name = item.itemName;
                            if (item.tag != null && item.tag != "") newgo.tag = item.tag;
                            newgo.SetActive(true);
                            if (item.xscale != 0) newgo.transform.localScale = new Vector2(item.xscale, newgo.transform.localScale.y);
                            myManifest.manifest.Find(i => i == item).refObj = newgo;
                            if (item.itemName == "CarpetEdgeTile") newgo.AddComponent<BoxCollider2D>();
                            break;
                        case CreationMode.MOVE:
                            Debug.Log($"[CSUB] Moving obj of type {item.itemName} to {item.pos.x}, {item.pos.y}");
                            var go = GameObject.Find(item.itemName);
                            if (go == null) { Debug.Log("GO null, abort"); break; }
                            if (!item.fromPopulate) go.transform.position = new Vector2(item.pos.x, item.pos.y);
                            if (item.xscale != 0) go.transform.localScale = new Vector2(item.xscale, go.transform.localScale.y);
                            myManifest.manifest.Find(i => i == item).refObj = go;
                            if (item.fromPopulate) myManifest.manifest.Find(i => i == item).fromPopulate = false;
                            break;
                        case CreationMode.CUSTOM:
                            Debug.Log($"[CSUB] Creating custom object at {item.pos.x}, {item.pos.y}");
                            var cgo = GameObject.Instantiate(prefabs["SubmarineInteriorChunk1_03"], new Vector2(item.pos.x, item.pos.y), Quaternion.identity);
                            cgo.transform.SetParent(GameObject.Find("SubmarineInteriorSmall").transform);
                            cgo.transform.position = new Vector2(item.pos.x, item.pos.y);
                            cgo.name = "customBGitem";
                            cgo.SetActive(true);
                            if (item.xscale != 0) cgo.transform.localScale = new Vector2(item.xscale, cgo.transform.localScale.y);
                            cgo.GetComponentInChildren<SpriteRenderer>().sprite = LoadSpriteFromFile(item.customSprite, Vector2.zero);
                            myManifest.manifest.Find(i => i == item).refObj = cgo;
                            break;
                    }
                }
                catch (Exception ex)
                {
                    Debug.Log($"[CSUB] Failed to proccess gameobject {item.itemName} at {item.pos.x}, {item.pos.y}: Error: {ex}");
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
                Debug.Log($"[Tree] {child.gameObject.name}");
                var go = child.gameObject;
                List<string> vals = new List<string>();
                foreach (var alias in alias.Values) vals.Add(alias);
                if (vals.Contains(go.name))
                {
                    var newGo = GameObject.Instantiate(go);
                    newGo.SetActive(false);
                    if (!prefabs.ContainsKey(go.name)) prefabs.Add(go.name, newGo);
                }
                if (indent == 1 && !go.name.Contains("Consoles") && ShouldDelete(go))
                {
                    toDestroy.Add(child.gameObject);
                    Debug.Log($"[CSUB] Flagged {go.name} for destruction");
                }
                if (indent >= 2 && ShouldDelete(go))
                {
                    Debug.Log($"[CSUB] Flagged {go.name} for destruction");
                    toDestroy.Add(child.gameObject);
                }
                if (!ShouldDelete(go))
                {
                    Debug.Log($"[CSUB] Adding {go.name} to manifest");
                    if (myManifest.manifest.Find(i => i.itemName == child.gameObject.name) == null)
                    {
                        myManifest.manifest.Add(new BuildItem() { fromPopulate = true, itemName = go.name, mode = CreationMode.MOVE, pos = new Position() { x = go.transform.position.x, y = go.transform.position.y }, xscale = go.transform.localScale.x });
                    }
                    if (go == null) Debug.Log($"Suddenly, {go.name} was null");
                }
                else if (ShouldContinue(go))
                {
                    Debug.Log($"Continue to recurse {go.name}");
                    RecursivePopulate(go, indent + 1);
                }
                else
                {
                    Debug.Log($"[CSUB] Flagged {go.name} for destruction");
                    toDestroy.Add(child.gameObject);
                }
            }
        }

        public bool ShouldContinue(GameObject go)
        {
            return go.name.Contains("Decoration") || go.name.Contains("DepthMeterCanvas") || go.name.Contains("Consoles");
        }

        public bool ShouldDelete(GameObject go)
        {
            if (corperative) return false;
            try
            {
                bool flag = false;
                if (go.transform.parent != null) flag = go.transform.parent.gameObject.name.Contains("DepthMeterCanvas");
                if ((go.name.Contains("Console") && go.name.Contains("Rigger")) || go.name.Contains("Torpedo") || go.name.Contains("DepthMeterCanvas") || go.name.Contains("Bed") || flag) return false;
            }
            catch { }
            return true;
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
            alias.Add("bg", "SubmarineInteriorChunk1_03");
        }

        public GameObject[] GetGameObjects(Vector2 centre, float radius)
        {
            List<GameObject> ret = new List<GameObject>();
            foreach (var go in myManifest.manifest)
            {
                try
                {
                    if (Vector2.Distance(centre, go.refObj.transform.position) < radius)
                    {
                        ret.Add(go.refObj);
                    }
                }
                catch { }
            }
            return ret.ToArray();
        }

        public GameObject[] GetGameObjects(string name)
        {
            List<GameObject> list = new List<GameObject>();
            foreach (var go in myManifest.manifest)
            {
                if (go.refObj.name == name) list.Add(go.refObj);
            }
            return list.ToArray();
        }

        public static Sprite LoadSpriteFromFile(string spriteImageFilePath, Vector2 pivot, int units = 32, SpriteMeshType spriteType = SpriteMeshType.Tight)
        {
            try
            {
                Texture2D texture = LoadTexture(spriteImageFilePath);
                int width = texture.width;
                int height = texture.height;
                return Sprite.Create(texture, new Rect(0, 0, width, height), pivot, units, 0, spriteType);
            }
            catch (Exception ex)
            {
                Debug.LogError("ModEngine Error while loading sprite: " + ex.Message);
                return null;
            }
        }

        private static Texture2D LoadTexture(string FilePath)
        {
            Texture2D Tex2D;
            byte[] FileData;
            if (File.Exists(FilePath))
            {
                FileData = File.ReadAllBytes(FilePath);
                Tex2D = new Texture2D(2, 2);
                if (Tex2D.LoadImage(FileData))
                    return Tex2D;
            }
            return null;
        }
    }

    [Serializable]
    public class BuildManifest
    {
        public bool wasBuiltCoOp;

        public string subSprite;

        public int hull;

        public float fuel;

        public float fuelDrainRate;

        public List<BuildItem> manifest = new List<BuildItem>();
    }

    [Serializable]
    public class BuildItem
    {
        public CreationMode mode;

        public string itemName;

        public Position pos;

        public float xscale;

        public string customSprite;

        public string tag;

        public bool fromPopulate;

        [NonSerialized]
        public GameObject refObj; //RUNTIME ONLY
    }

    [Serializable]
    public enum CreationMode
    {
        CREATE,
        MOVE,
        CUSTOM
    }

    [Serializable]
    public class Position
    {
        public float x;

        public float y;
    }
}
