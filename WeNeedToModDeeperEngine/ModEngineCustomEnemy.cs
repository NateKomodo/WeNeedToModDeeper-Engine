using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace WeNeedToModDeeperEngine //NOTE the types below are a framework that mod makers can choose to include
{
    public class ModEngineCustomEnemy
    {
        public readonly GameObject gameObject;

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
            STARFISH,
            BARRACUDA,
            PENGUIN_INT,
            STARFISH_INT,
            HUMANOID_INT
        }

        public enum EnemyType
        {
            INTERIOR,
            EXTERIOR
        }

        public ModEngineCustomEnemy(EnemyType type, EnemyTemplates enemyTemplate, params string[] spritesPaths)
        {
            try
            {
            SpriteMeshType spriteType = SpriteMeshType.Tight;
            List<Sprite> sprites = new List<Sprite>();

            if (type == EnemyType.EXTERIOR)
            {
                foreach (string path in spritesPaths)
                {
                    Sprite sprite = LoadSpriteFromFile(path, 32, spriteType);
                    if (sprite != null) sprites.Add(sprite);
                }
            }

            if (type == EnemyType.INTERIOR)
            {
                foreach (string path in spritesPaths)
                {
                    Sprite sprite = LoadSpriteFromFile(path, 200, spriteType);
                    if (sprite != null) sprites.Add(sprite);
                }
            }

            if (sprites.Count == 0) return;

                if (type == EnemyType.EXTERIOR)
                {
                    int i = 0;
                    switch (enemyTemplate)
                    {
                        case EnemyTemplates.BUBBLE:
                            gameObject = GetObjectFromArray(GameControllerBehavior.AIDM.atlanticHazards, "GiantBubble");
                            gameObject.GetComponentInChildren<SpriteRenderer>().sprite = sprites[0];
                            gameObject.SetActive(false);
                            break;
                        case EnemyTemplates.DUOPUS:
                            gameObject = GetObjectFromArray(GameControllerBehavior.AIDM.atlanticEnemiesMedium, "obj_duopus");
                            foreach (var renderer in gameObject.GetComponentsInChildren<SpriteRenderer>())
                            {
                                renderer.sprite = sprites[i];
                                i++;
                            }
                            gameObject.SetActive(false);
                            break;
                        case EnemyTemplates.MINEHAZARD:
                            gameObject = GetObjectFromArray(GameControllerBehavior.AIDM.barnacleEnemiesEasy, "BarnacleMine");
                            gameObject.GetComponentInChildren<SpriteRenderer>().sprite = sprites[0];
                            gameObject.SetActive(false);
                            break;
                        case EnemyTemplates.ICEHAZARD:
                            gameObject = GetObjectFromArray(GameControllerBehavior.AIDM.arcticEnemiesEasy, "IceCrystal");
                            gameObject.GetComponentInChildren<SpriteRenderer>().sprite = sprites[0];
                            gameObject.SetActive(false);
                            break;
                        case EnemyTemplates.NARWHAL:
                            gameObject = GetObjectFromArray(GameControllerBehavior.AIDM.arcticEnemiesEasy, "obj_Narwhal");
                            foreach (var renderer in gameObject.GetComponentsInChildren<SpriteRenderer>())
                            {
                                renderer.sprite = sprites[i];
                                i++;
                            }
                            gameObject.SetActive(false);
                            break;
                        case EnemyTemplates.ORCA:
                            gameObject = GetObjectFromArray(GameControllerBehavior.AIDM.arcticEnemiesMedium, "obj_Orca");
                            foreach (var renderer in gameObject.GetComponentsInChildren<SpriteRenderer>())
                            {
                                renderer.sprite = sprites[i];
                                i++;
                            }
                            gameObject.SetActive(false);
                            break;
                        case EnemyTemplates.SHARK:
                            gameObject = GetObjectFromArray(GameControllerBehavior.AIDM.atlanticEnemiesEasy, "obj_shark");
                            foreach (var renderer in gameObject.GetComponentsInChildren<SpriteRenderer>())
                            {
                                renderer.sprite = sprites[i];
                                i++;
                            }
                            gameObject.SetActive(false);
                            break;
                        case EnemyTemplates.STARFISH:
                            gameObject = GetObjectFromArray(GameControllerBehavior.AIDM.atlanticEnemiesEasy, "StarfishExterior");
                            gameObject.GetComponentInChildren<SpriteRenderer>().sprite = sprites[0];
                            gameObject.SetActive(false);
                            break;
                        case EnemyTemplates.TURTLE:
                            gameObject = GetObjectFromArray(GameControllerBehavior.AIDM.volcanicEnemiesHard, "VolcanicTortoise");
                            foreach (var renderer in gameObject.GetComponentsInChildren<SpriteRenderer>())
                            {
                                renderer.sprite = sprites[i];
                                i++;
                            }
                            gameObject.SetActive(false);
                            break;
                        case EnemyTemplates.BARRACUDA:
                            gameObject = GetObjectFromArray(GameControllerBehavior.AIDM.stormyEnemiesEasy, "obj_barracuda");
                            foreach (var renderer in gameObject.GetComponentsInChildren<SpriteRenderer>())
                            {
                                renderer.sprite = sprites[i];
                                i++;
                            }
                            gameObject.SetActive(false);
                            break;
                        case EnemyTemplates.PIRATESHIP:
                            gameObject = GetObjectFromArray(GameControllerBehavior.AIDM.stormyEnemiesMedium, "PirateShipEnemy");
                            foreach (var renderer in gameObject.GetComponentsInChildren<SpriteRenderer>())
                            {
                                renderer.sprite = sprites[i];
                                i++;
                            }
                            gameObject.SetActive(false);
                            break;
                        default:
                            gameObject = GetObjectFromArray(GameControllerBehavior.AIDM.atlanticEnemiesEasy, "obj_shark");
                            break;
                    }
                }
                if (type == EnemyType.INTERIOR)
                {
                    int i = 0;
                    switch (enemyTemplate)
                    {
                        case EnemyTemplates.HUMANOID_INT:
                            gameObject = GetObjectFromArray(GameControllerBehavior.AIDM.stormyEnemiesMedium, "PirateShipEnemy").GetComponentInChildren<ExteriorEnemyGrabInjectBehavior>().injectionPrefabs[0];
                            foreach (var renderer in gameObject.GetComponentsInChildren<SpriteRenderer>())
                            {
                                renderer.sprite = sprites[i];
                                i++;
                            }
                            gameObject.SetActive(false);
                            break;
                        case EnemyTemplates.PENGUIN_INT:
                            gameObject = GetObjectFromArray(GameControllerBehavior.AIDM.arcticEnemiesMedium, "PenguinExterior").GetComponentInChildren<ExteriorEnemyGrabInjectBehavior>().injectionPrefabs[0];
                            foreach (var renderer in gameObject.GetComponentsInChildren<SpriteRenderer>())
                            {
                                renderer.sprite = sprites[i];
                                i++;
                            }
                            gameObject.SetActive(false);
                            break;
                        case EnemyTemplates.STARFISH_INT:
                            gameObject = GetObjectFromArray(GameControllerBehavior.AIDM.atlanticEnemiesEasy, "StarfishExterior").GetComponentInChildren<ExteriorEnemyGrabInjectBehavior>().injectionPrefabs[0];
                            gameObject.GetComponentInChildren<SpriteRenderer>().sprite = sprites[0];
                            gameObject.SetActive(false);
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                UnityEngine.Debug.LogError("ModEngine Error while creating custom enemy (Are sprite paths correct and are there the right amount for the template?): " + ex.Message);
                gameObject = null;
                return;
            }
        }

        private GameObject GetObjectFromArray(GameObject[] array, string name)
        {
            foreach (var go in array)
            {
                if (go.name == name) return go;
            }
            UnityEngine.Debug.LogError("Did not find requested item " + name);
            return null;
        }

        public GameObject InstanciateGameObject(Vector2 position)
        {
            return GameObject.Instantiate<GameObject>(gameObject, position, Quaternion.identity);
        }

        private Sprite LoadSpriteFromFile(string spriteImageFilePath, int unitsPerPixel, SpriteMeshType spriteType = SpriteMeshType.Tight)
        {
            try
            {
                Texture2D texture = LoadTexture(spriteImageFilePath);
                return Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0, 0), unitsPerPixel, 0, spriteType);
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
    }
}
