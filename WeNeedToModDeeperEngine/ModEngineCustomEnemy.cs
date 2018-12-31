using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;

namespace WeNeedToModDeeperEngine //NOTE the types below are a framework that mod makers can choose to include
{
    public class ModEngineCustomEnemy
    {
        public GameObject gameObject;

        public GameObject[] InjectionPrefabs
        {
            get
            {
                ExteriorEnemyGrabInjectBehavior inj = gameObject.GetComponentInChildren<ExteriorEnemyGrabInjectBehavior>();
                if (inj != null)
                {
                    return inj.injectionPrefabs;
                }
                else
                {
                    return null;
                }
            }
            set
            {
                ExteriorEnemyGrabInjectBehavior inj = gameObject.GetComponentInChildren<ExteriorEnemyGrabInjectBehavior>();
                if (inj != null)
                {
                    inj.injectionPrefabs = value;
                }
            }
        }

        public int Health
        {
            get
            {
                if (type == EnemyType.EXTERIOR) return gameObject.GetComponent<ExteriorEnemyHealth>().Networkhealth;
                if (type == EnemyType.INTERIOR) return gameObject.GetComponent<InteriorEnemyDamageController>().Networkhealth;
                return -1;
            }
            set
            {
                if (type == EnemyType.EXTERIOR) gameObject.GetComponent<ExteriorEnemyHealth>().Networkhealth = value;
                if (type == EnemyType.INTERIOR) gameObject.GetComponent<InteriorEnemyDamageController>().Networkhealth = value;
            }
        }

        public enum EnemyTemplates
        {
            DUOPUS,
            PIRATESHIP,
            SHARK,
            MINEHAZARD,
            ICEHAZARD,
            TURTLE,
            BUBBLE,
            ORCA,
            NARWHAL,
            PENGUIN,
            STARFISH,
            BARRACUDA,
            PENGUIN_INT,
            STARFISH_INT,
            HUMANOID_INT,
            MONOBODY
        }

        public enum EnemyType
        {
            INTERIOR,
            EXTERIOR
        }

        public Dictionary<string, Sprite> Sprites = new Dictionary<string, Sprite>();

        public Dictionary<string, Vector2> Offsets = new Dictionary<string, Vector2>();

        private int units;

        public readonly bool isMonobodyExt = false;

        public readonly bool isMonobodyInt = false;

        private EnemyType type;

        public ModEngineCustomEnemy(EnemyType entype, EnemyTemplates enemyTemplate, string name)
        {
            type = entype;
            try
            {
                if (type == EnemyType.EXTERIOR)
                {
                    units = 32;
                    switch (enemyTemplate)
                    {
                        case EnemyTemplates.BUBBLE:
                            gameObject = GetObjectFromArray(GameControllerBehavior.AIDM.atlanticHazards, "GiantBubble");
                            break;
                        case EnemyTemplates.DUOPUS:
                            gameObject = GetObjectFromArray(GameControllerBehavior.AIDM.atlanticEnemiesMedium, "obj_duopus");
                            break;
                        case EnemyTemplates.MINEHAZARD:
                            gameObject = GetObjectFromArray(GameControllerBehavior.AIDM.barnacleEnemiesEasy, "BarnacleMine");
                            break;
                        case EnemyTemplates.ICEHAZARD:
                            gameObject = GetObjectFromArray(GameControllerBehavior.AIDM.arcticEnemiesEasy, "IceCrystal");
                            break;
                        case EnemyTemplates.NARWHAL:
                            gameObject = GetObjectFromArray(GameControllerBehavior.AIDM.arcticEnemiesEasy, "obj_Narwhal");
                            break;
                        case EnemyTemplates.ORCA:
                            gameObject = GetObjectFromArray(GameControllerBehavior.AIDM.arcticEnemiesMedium, "obj_Orca");
                            break;
                        case EnemyTemplates.SHARK:
                            gameObject = GetObjectFromArray(GameControllerBehavior.AIDM.atlanticEnemiesEasy, "obj_shark");
                            break;
                        case EnemyTemplates.STARFISH:
                            gameObject = GetObjectFromArray(GameControllerBehavior.AIDM.atlanticEnemiesEasy, "StarfishExterior");
                            break;
                        case EnemyTemplates.TURTLE:
                            gameObject = GetObjectFromArray(GameControllerBehavior.AIDM.volcanicEnemiesHard, "VolcanicTortoise");
                            break;
                        case EnemyTemplates.BARRACUDA:
                            gameObject = GetObjectFromArray(GameControllerBehavior.AIDM.stormyEnemiesEasy, "obj_barracuda");
                            break;
                        case EnemyTemplates.PIRATESHIP:
                            gameObject = GetObjectFromArray(GameControllerBehavior.AIDM.stormyEnemiesMedium, "PirateShipEnemy");
                            break;
                        case EnemyTemplates.PENGUIN:
                            gameObject = GetObjectFromArray(GameControllerBehavior.AIDM.arcticEnemiesMedium, "PenguinExterior");
                            break;
                        case EnemyTemplates.MONOBODY:
                            NewMonobody(type);
                            isMonobodyExt = true;
                            break;
                        default:
                            gameObject = GetObjectFromArray(GameControllerBehavior.AIDM.atlanticEnemiesEasy, "obj_shark");
                            break;
                    }
                }
                if (type == EnemyType.INTERIOR)
                {
                    units = 200;
                    switch (enemyTemplate)
                    {
                        case EnemyTemplates.HUMANOID_INT:
                            gameObject = GetInjectObjectFromArray(GameControllerBehavior.AIDM.stormyEnemiesMedium, "PirateShipEnemy");
                            break;
                        case EnemyTemplates.PENGUIN_INT:
                            gameObject = GetInjectObjectFromArray(GameControllerBehavior.AIDM.arcticEnemiesMedium, "PenguinExterior");
                            break;
                        case EnemyTemplates.MONOBODY:
                            NewMonobody(type);
                            isMonobodyInt = true;
                            break;
                        case EnemyTemplates.STARFISH_INT:
                            gameObject = GetInjectObjectFromArray(GameControllerBehavior.AIDM.atlanticEnemiesEasy, "StarfishExterior");
                            break;
                    }
                }
                gameObject.name = name;
                if (isMonobodyExt || isMonobodyInt) return;
                PopulateSprites();
                PopulateOffsets();
            }
            catch (Exception ex)
            {
                UnityEngine.Debug.LogError("ModEngine Error while creating custom enemy (Is AIDM active?): " + ex.Message);
                gameObject = null;
                return;
            }
        }

        private void NewMonobody(EnemyType type)
        {
            if (type == EnemyType.EXTERIOR)
            {
                units = 32;
                gameObject = GetObjectFromArray(GameControllerBehavior.AIDM.atlanticEnemiesEasy, "obj_shark");
                PopulateSprites();
                Dictionary<string, Sprite> Sprites2 = new Dictionary<string, Sprite>();
                foreach (var entry in Sprites) Sprites2.Add(entry.Key, entry.Value);
                foreach (var entry in Sprites2)
                {
                    if (entry.Key != "SharkBody")
                    {
                        Sprites[entry.Key] = null;
                    }
                }
                UpdateSprites();
                foreach (var entry in Sprites2)
                {
                    if (entry.Key != "SharkBody")
                    {
                        Sprites.Remove(entry.Key);
                    }
                }
            }
            if (type == EnemyType.INTERIOR)
            {
                units = 200;
                gameObject = GetInjectObjectFromArray(GameControllerBehavior.AIDM.arcticEnemiesMedium, "PenguinExterior");
                PopulateSprites();
                Dictionary<string, Sprite> Sprites2 = new Dictionary<string, Sprite>();
                foreach (var entry in Sprites) Sprites2.Add(entry.Key, entry.Value);
                foreach (var entry in Sprites2)
                {
                    if (entry.Key != "PenguinInteriorBody")
                    {
                        Sprites[entry.Key] = null;
                    }
                }
                UpdateSprites();
                foreach (var entry in Sprites2)
                {
                    if (entry.Key != "PenguinInteriorBody")
                    {
                        Sprites.Remove(entry.Key);
                    }
                }
            }
        }

        private void PopulateSprites()
        {
            foreach (var render in gameObject.GetComponentsInChildren<SpriteRenderer>())
            {
                try
                {
                    Sprites.Add(render.gameObject.name, render.sprite);
                }
                catch
                {
                    Debug.LogError("Mod engine error: Sprite already in dictionary, continuing");
                }
            }
        }

        private void PopulateOffsets()
        {
            foreach (var render in gameObject.GetComponentsInChildren<SpriteRenderer>())
            {
                Offsets.Add(render.gameObject.name, render.material.mainTextureOffset);
            }
        }

        public void UpdateSprites()
        {
            foreach (var render in gameObject.GetComponentsInChildren<SpriteRenderer>())
            {
                var name = render.gameObject.name;
                foreach (var entry in Sprites)
                {
                    if (entry.Key == name)
                    {
                        render.sprite = entry.Value;
                    }
                }
            }
        }

        public void UpdateOffsets()
        {
            foreach (var render in gameObject.GetComponentsInChildren<SpriteRenderer>())
            {
                var name = render.gameObject.name;
                foreach (var entry in Offsets)
                {
                    if (entry.Key == name)
                    {
                        render.material.mainTextureOffset = entry.Value;
                    }
                }
            }
        }

        private GameObject GetInjectObjectFromArray(GameObject[] array, string name)
        {
            foreach (var go in array)
            {
                if (go.name == name) return GameObject.Instantiate<GameObject>(go.GetComponentInChildren<ExteriorEnemyGrabInjectBehavior>().injectionPrefabs[0]);
            }
            UnityEngine.Debug.LogError("Did not find requested item " + name);
            return null;
        }

        private GameObject GetObjectFromArray(GameObject[] array, string name)
        {
            foreach (var go in array)
            {
                if (go.name == name) return GameObject.Instantiate<GameObject>(go);
            }
            UnityEngine.Debug.LogError("Did not find requested item " + name);
            return null;
        }

        public GameObject InstanciateGameObject(Vector2 position)
        {
            UnityEngine.Debug.Log("Instanciating custom game object: " + gameObject.name + " at: " + position.x + " " + position.y);
            GameObject go = GameObject.Instantiate<GameObject>(gameObject, position, Quaternion.identity);
            NetworkServer.Spawn(go);
            return go;
        }

        public Sprite LoadSpriteFromFile(string spriteImageFilePath, Vector2 pivot, Sprite spritePivotToCopy = null, SpriteMeshType spriteType = SpriteMeshType.Tight)
        {
            if (spritePivotToCopy != null) pivot = spritePivotToCopy.pivot;
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

        private Texture2D LoadTexture(string FilePath)
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

        public void ChangeGameObject(GameObject go, EnemyType type)
        {
            gameObject = go;
            Sprites.Clear();
            PopulateSprites();
            if (type == EnemyType.EXTERIOR) { units = 32; } else { units = 200; }
        }

        public void AttachToParent(GameObject parent)
        {
            gameObject.transform.parent = parent.transform;
        }

        public void AttachChild(GameObject child)
        {
            child.transform.parent = gameObject.transform;
        }

        public GameObject[] GetAllChildren()
        {
            List<GameObject> children = new List<GameObject>();
            foreach (Transform child in gameObject.transform)
            {
                children.Add(child.gameObject);
            }
            return children.ToArray();
        }

        public GameObject GetParent()
        {
            return gameObject.transform.parent.gameObject;
        }

        public Component GetComponent<comp>()
        {
            return gameObject.GetComponent(typeof(comp));
        }

        public Component GetComponentInChildren<comp>()
        {
            return gameObject.GetComponentInChildren(typeof(comp));
        }

        public Component GetComponentInParent<comp>()
        {
            return gameObject.GetComponentInParent(typeof(comp));
        }

        public Component[] GetComponents<comp>()
        {
            return gameObject.GetComponents(typeof(comp));
        }

        public Component[] GetComponentsInChildren<comp>()
        {
            return gameObject.GetComponentsInChildren(typeof(comp));
        }

        public Component[] GetComponentsInParent<comp>()
        {
            return gameObject.GetComponentsInParent(typeof(comp));
        }

        public void AddComponent<comp>()
        {
            gameObject.AddComponent(typeof(comp));
        }
    }
}
