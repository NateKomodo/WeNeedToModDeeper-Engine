using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;

namespace WeNeedToModDeeperEngine //NOTE the types below are a framework that mod makers can choose to include
{
    public class ModEngineCustomSubmarineLegacy //TODO ext sprite
    {
        public Dictionary<string, GameObject> prefabs = new Dictionary<string, GameObject>();

        public List<GameObject> oldSub = new List<GameObject>();

        public List<GameObject> currentSub = new List<GameObject>();

        public string spritePath;

        public ModEngineCustomSubmarineLegacy(string subPath)
        {
            GameObject.FindObjectOfType<FloodSystemControllerBehavior>().puddles.Clear();
            var subint = GameObject.Find("SubmarineInteriorSmall");
            Recursive(subint, 1);
            DestroyOld();
            if (subPath != null && subPath != "")
            {
                System.Timers.Timer runonce = new System.Timers.Timer(200);
                runonce.Elapsed += (s, e) => { Load(subPath); };
                runonce.AutoReset = false;
                runonce.Start();
            }
        }

        public GameObject AddComponent(string path, int units, Vector3 position)
        {
            try
            {
                var go = AddComponent("SubmarineInteriorChunk1_03", position);
                go.GetComponentInChildren<SpriteRenderer>().sprite = LoadSpriteFromFile(path, Vector2.zero, units);
                go.name = path + "&" + units;
                return go;
            }
            catch { }
            return null;
        }

        public GameObject AddComponent(string type, Vector3 position, bool createCollider = true)
        {
            try
            {
                GameObject go = GameObject.Instantiate<GameObject>(prefabs[type], position, Quaternion.identity);
                currentSub.Add(go);
                go.transform.SetParent(GameObject.Find("SubmarineInteriorSmall").transform);
                go.transform.position = position;
                go.SetActive(true);
                go.name = type;
                if (type == "CarpetEdgeTile" && createCollider)
                {
                    var col = AddComponent("InteriorGroundCollider", position, false);
                    Vector2 S = go.GetComponent<SpriteRenderer>().sprite.bounds.size;
                    col.GetComponent<BoxCollider2D>().size = S;
                }
                return go;
            }
            catch { }
            return null;
        }

        public GameObject AddComponent(string path, int units, Vector3 position, float xscale)
        {
            try
            {
                var go = AddComponent("SubmarineInteriorChunk1_03", position, xscale);
                go.GetComponentInChildren<SpriteRenderer>().sprite = LoadSpriteFromFile(path, Vector2.zero, units);
                go.name = path + "&" + units;
                return go;
            }
            catch { }
            return null;
        }

        public GameObject AddComponent(string type, Vector3 position, float xscale, bool createCollider = true)
        {
            try
            {
                GameObject go = GameObject.Instantiate<GameObject>(prefabs[type], position, Quaternion.identity);
                currentSub.Add(go);
                go.transform.SetParent(GameObject.Find("SubmarineInteriorSmall").transform);
                go.transform.position = position;
                go.SetActive(true);
                go.name = type;
                go.transform.localScale = new Vector2(xscale, go.transform.localScale.y);
                if (type == "CarpetEdgeTile" && createCollider)
                {
                    var col = AddComponent("InteriorGroundCollider", position, false);
                    Vector2 S = go.GetComponent<SpriteRenderer>().sprite.bounds.size;
                    col.GetComponent<BoxCollider2D>().size = S;
                }
                return go;
            }
            catch { }
            return null;
        }

        public void DestroyComponent(GameObject go)
        {
            if (go.name.Contains("Console") && go.name.Contains("Rigger")) return;
            currentSub.Remove(go);
            try
            {
                GameObject.Destroy(go);
            }
            catch { }
        }

        public GameObject[] GetGameObjects(Vector2 centre, float radius)
        {
            List<GameObject> ret = new List<GameObject>();
            foreach (var go in currentSub)
            {
                try
                {
                    if (Vector2.Distance(centre, go.transform.position) < radius)
                    {
                        ret.Add(go);
                    }
                }
                catch { }
            }
            return ret.ToArray();
        }

        public GameObject[] GetGameObjects(string name)
        {
            List<GameObject> list = new List<GameObject>();
            foreach (var go in currentSub)
            {
                if (go.name == name) list.Add(go);
            }
            return list.ToArray();
        }

        public void Load(string filePath)
        {
            string[] subData = File.ReadAllLines(filePath);
            foreach (var line in subData)
            {
                try
                {
                    string[] data = line.Split(' ');
                    if (data[0].StartsWith("+"))
                    {
                        string[] coords = data[1].Split(',');
                        Vector3 position = new Vector3(float.Parse(coords[0]), float.Parse(coords[1]), float.Parse(coords[2]));
                        float xscale = float.Parse(coords[3]);
                        if (data[0].Contains("&"))
                        {
                            string[] custom = data[0].Split('&');
                            AddComponent(custom[0].Substring(1), int.Parse(custom[1]), position, xscale);
                        }
                        else
                        {
                            AddComponent(data[0].Substring(1), position, xscale);
                        }
                    }
                    else if (data[0].StartsWith(">"))
                    {
                        string[] coords = data[1].Split(',');
                        Vector3 position = new Vector3(float.Parse(coords[0]), float.Parse(coords[1]), float.Parse(coords[2]));
                        float xscale = float.Parse(coords[3]);
                        foreach (var obj in currentSub)
                        {
                            if (obj.name == data[0].Substring(1))
                            {
                                obj.transform.position = position;
                                obj.transform.localScale = new Vector2(xscale, obj.transform.localScale.y);
                            }
                        }
                    }
                    else if (data[0].StartsWith("#"))
                    {
                        string param = data[0].Substring(1);
                        if (param == "fuel")
                        {
                            ModEngineVariables.Substats.boostJuice = float.Parse(data[1]);
                            ModEngineVariables.Substats.NetworkmaxBoostJuice = float.Parse(data[1]);
                        }
                        if (param == "fuelDrainRate")
                        {
                            ModEngineVariables.Substats.boostJuiceDrainRate = float.Parse(data[1]);
                        }
                        if (param == "hull")
                        {
                            ModEngineVariables.Substats.NetworkmaxSubHealth = int.Parse(data[1]);
                            ModEngineVariables.Substats.NetworksubHealth = int.Parse(data[1]);
                        }
                        if (param == "sprite")
                        {
                            foreach (var render in GameObject.Find("Submarine").GetComponentsInChildren<SpriteRenderer>()) render.sprite = LoadSpriteFromFile(data[1], Vector2.zero, 32);
                            GameObject.Find("InteractableSub").GetComponentInChildren<SpriteRenderer>().sprite = LoadSpriteFromFile(data[1], new Vector2(0, 0.5f), 32);
                        }
                    }
                }
                catch { }
            }
        }

        public void Save(string filePath)
        {
            string data = "";
            foreach (var obj in currentSub)
            {
                if (!obj.name.Contains("Rigger"))
                {
                    data += "+" + obj.name + " " + obj.transform.position.x + "," + obj.transform.position.y + "," + obj.transform.position.z + "," + obj.transform.localScale.x + Environment.NewLine;
                }
                else
                {
                    data += ">" + obj.name + " " + obj.transform.position.x + "," + obj.transform.position.y + "," + obj.transform.position.z + "," + obj.transform.localScale.x + Environment.NewLine;
                }
            }
            data += "#fuel " + ModEngineVariables.Substats.NetworkmaxBoostJuice + Environment.NewLine;
            data += "#fuelDrainRate " + ModEngineVariables.Substats.boostJuiceDrainRate + Environment.NewLine;
            data += "#hull " + ModEngineVariables.Substats.NetworkmaxSubHealth + Environment.NewLine;
            if (spritePath != "" && spritePath != null) data += "#sprite " + spritePath + Environment.NewLine;
            string[] final = data.Split(new string[] { Environment.NewLine }, StringSplitOptions.None);
            File.WriteAllLines(filePath, final);
        }

        private void DestroyOld()
        {
            foreach (var go in oldSub)
            {
                try
                {
                    GameObject.Destroy(go);
                }
                catch { }
            }
        }

        private void Recursive(GameObject go, int indent)
        {
            foreach (Transform child in go.transform)
            {
                if (indent > 3) continue;
                try
                {
                    GameObject newgo = GameObject.Instantiate<GameObject>(child.gameObject);
                    newgo.SetActive(false);
                    prefabs.Add(child.gameObject.name, newgo);
                    if (indent == 1 && !child.gameObject.name.Contains("Consoles")) oldSub.Add(child.gameObject);
                    if (indent == 2 && child.gameObject.name.Contains("Console") && child.gameObject.name.Contains("Rigger")) currentSub.Add(child.gameObject);
                    Recursive(child.gameObject, indent + 1);
                }
                catch { }
            }
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
                UnityEngine.Debug.LogError("ModEngine Error while loading sprite: " + ex.Message);
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
}
