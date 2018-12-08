using System;
using UnityEngine;
using UnityEngine.UI;

namespace WeNeedToModDeeperEngine //NOTE the types below are a framework that mod makers can choose to include
{
    public class ModEngine
    {
        public static void Main()
        {
            //Just so visual studio doesnt kill me
        }
    }

    public class ModEngineVariables //Varaible bridge to game code
    {
        public static int Gold
        {
            get { return GoldControllerBehavior.gold; }
            set { GoldControllerBehavior.gold = value; }
        }
        public static bool Devmode
        {
            get { return SubStats.devMode; }
            set { SubStats.devMode = value; }
        }
        public static int Playerhealth
        {
            get { return GameObject.FindGameObjectWithTag("Player").GetComponent<HealthController>().playerHealth; }
            set { GameObject.FindGameObjectWithTag("Player").GetComponent<HealthController>().playerHealth = value; }
        }
        public static SubStats Substats
        {
            get { return GameControllerBehavior.subStats; }
            set { GameControllerBehavior.subStats = value; }
        }
        public static int PlayerMaxHealth
        {
            get { return GameObject.FindGameObjectWithTag("Player").GetComponent<HealthController>().maxHealth; }
            set { GameObject.FindGameObjectWithTag("Player").GetComponent<HealthController>().maxHealth = value; }
        }
        [Obsolete("GlobalStats may not function correctly, recomended you use GlobalStats.Variable instead")]
        public static GlobalStats PlayerStats
        {
            get { return GameObject.FindGameObjectWithTag("Player").GetComponent<GlobalStats>(); }
        }
        public static AIDMBehavior AIDM
        {
            get { return GameControllerBehavior.AIDM; }
            set { GameControllerBehavior.AIDM = value; }
        }
        public static int WaterType
        {
            get { return GameControllerBehavior.AIDM.NetworkcurrentWaterType; }
            set { GameControllerBehavior.AIDM.NetworkcurrentWaterType = value; }
        }
        public static HealthController GetPlayerHealthController
        {
            get { return ModEngineComponents.GetObjectFromTag("Player").GetComponent<HealthController>(); }
        }
        public static bool IsDead
        {
            get { return ModEngineComponents.GetObjectFromTag("Player").GetComponent<HealthController>().dead; }
            set { ModEngineComponents.GetObjectFromTag("Player").GetComponent<HealthController>().dead = value; }
        }
        public static HandleWeapons WeaponsHandler
        {
            get { return ModEngineComponents.GetGameObjectWithComponent<HandleWeapons>().GetComponent<HandleWeapons>(); }
        }
        public static Transform GetTransform(string obj)
        {
            return ModEngineComponents.GetObjectFromTag(obj).transform;
        }
    }

    public class ModEngineComponents //Class for loading components via unity
    {
        public static Component GetComponentFromObject(string gameObject, String classname) //Get a specified class from a game object
        {
            try
            {
                return GameObject.FindGameObjectWithTag(gameObject).GetComponent(Type.GetType(classname)); //Return it based off input
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message); //If theres an error (Such as if no class object is found) print the error and return null
                return null;
            }
        }
        public static Component GetComponentFromObject<type>(string gameObject) //Get a specified class from a game object
        {
            try
            {
                return GameObject.FindGameObjectWithTag(gameObject).GetComponent(typeof(type)); //Return it based off input
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message); //If theres an error (Such as if no class object is found) print the error and return null
                return null;
            }
        }
        public static int GetInstanceID(string gameObject) //Get a instance ID from a game object
        {
            try
            {
                return GameObject.FindGameObjectWithTag(gameObject).GetInstanceID(); //Return it based off input
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message); //If theres an error (Such as if no class object is found) print the error and return null
                return -1;
            }
        }

        public static GameObject GetObjectFromTag(string gameObject) //Get a game object from a game object name
        {
            try
            {
                return GameObject.FindGameObjectWithTag(gameObject); //Return it based off input
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message); //If theres an error (Such as if no class object is found) print the error and return null
                return null;
            }
        }
        public static Component[] GetAllComponentsFromGameObject(string gameObject) //Get all components from a game object
        {
            try
            {
                return GameObject.FindGameObjectWithTag(gameObject).GetComponents(typeof(Component)); //Return Components[]
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message); //If theres an error (Such as if no class object is found) print the error and return null
                return null;
            }
        }
        public static bool AddComponentToGameObject<type>(string gameObject)
        {
            try
            {
                GameObject.FindGameObjectWithTag(gameObject).AddComponent(typeof(type));
                return true;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message); //If theres an error (Such as if no class object is found) print the error and return false
                return false;
            }
        }
        public static UnityEngine.Object[] GetAllGameObjects()
        {
            try
            {
                return GameObject.FindObjectsOfType(typeof(MonoBehaviour));
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message); //If theres an error (Such as if no class object is found) print the error and return false
                return null;
            }
        }
        public static GameObject GetGameObjectWithComponent<comp>()
        {
            try
            {
                var obj = (GameObject)UnityEngine.Object.FindObjectOfType(typeof(comp));
                return obj.gameObject;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
                return null;
            }
        }
        public static Component GetComponent<type>()
        {
            try
            {
                var obj = (GameObject)UnityEngine.Object.FindObjectOfType(typeof(type));
                return obj.GetComponent(typeof(type));
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
                return null;
            }
        }
    }

    public class ModEngineChatMessage
    {
        public ModEngineChatMessage(string message, PlayerNetworking.ChatMessageType type)
        {
            NetworkManagerBehavior.myPlayerNetworking.CallRpcSetMessageParameters(message, (int)type, ModEngineComponents.GetInstanceID("Player")); //Send a chat message in any of the fonts
        }
    }

    public class ModEngineTextOverlay
    {
        public ModEngineTextOverlay(string message)
        {
            MouthBehavior componentInChildren2 = ModEngineComponents.GetObjectFromTag("Player").GetComponentInChildren<MouthBehavior>();
            componentInChildren2.afflictedUI.GetComponent<Text>().text = message;
            componentInChildren2.afflictedUI.GetComponent<Animator>().SetTrigger("Enable");
        }
    }
    public class ModEngineEvents
    {
        static int prevGold = 0;
        static int prevHealth = 10;
        static int prevMaxHealth = 10;
        static int prevBiome = 0;
        static bool prevDead = false;
        static int prevSubHealth = 0;
        static int prevSubMaxHealth = 0;
        static SubStats prevSubStats = null;
        static bool prevCave = false;
        static bool prevCiv = false;
        static AIDMBehavior prevAIDM = null;
        static float prevBoostJuice = 0f;
        static string prevText = "";
        static int prevBossHealth = 0;

        public static bool GoldChange()
        {
            if (!(ModEngineVariables.Gold == prevGold))
            {
                prevGold = ModEngineVariables.Gold;
                return true;
            }
            return false;
        }
        public static bool PlayerHealthChange()
        {
            if (!(ModEngineVariables.Playerhealth == prevHealth))
            {
                prevHealth = ModEngineVariables.Playerhealth;
                return true;
            }
            return false;
        }
        public static bool PlayerMaxHealthChange()
        {
            if (!(ModEngineVariables.PlayerMaxHealth == prevMaxHealth))
            {
                prevMaxHealth = ModEngineVariables.PlayerMaxHealth;
                return true;
            }
            return false;
        }
        public static bool BiomeChange()
        {
            if (!(ModEngineVariables.WaterType == prevBiome))
            {
                prevBiome = ModEngineVariables.WaterType;
                return true;
            }
            return false;
        }
        public static bool DeathStatusChange()
        {
            if (!(ModEngineVariables.IsDead == prevDead))
            {
                prevDead = ModEngineVariables.IsDead;
                return true;
            }
            return false;
        }
        public static bool SubHealthChange()
        {
            if (!(ModEngineVariables.Substats.NetworksubHealth == prevSubHealth))
            {
                prevSubHealth = ModEngineVariables.Substats.NetworksubHealth;
                return true;
            }
            return false;
        }
        public static bool SubMaxHealthChange()
        {
            if (!(ModEngineVariables.Substats.NetworkmaxSubHealth == prevSubMaxHealth))
            {
                prevSubMaxHealth = ModEngineVariables.Substats.NetworkmaxSubHealth;
                return true;
            }
            return false;
        }
        public static bool SubStatsChanged()
        {
            if (!(ModEngineVariables.Substats == prevSubStats))
            {
                prevSubStats = ModEngineVariables.Substats;
                return true;
            }
            return false;
        }
        public static bool CaveStatusChange()
        {
            if (!(AIDMBehavior.inCave == prevCave))
            {
                prevCave = AIDMBehavior.inCave;
                return true;
            }
            return false;
        }
        public static bool CivStatusChange()
        {
            if (!(AIDMBehavior.inCiv == prevCiv))
            {
                prevCiv = AIDMBehavior.inCiv;
                return true;
            }
            return false;
        }
        public static bool AIDMChange()
        {
            if (!(ModEngineVariables.AIDM == prevAIDM))
            {
                prevAIDM = ModEngineVariables.AIDM;
                return true;
            }
            return false;
        }
        public static bool FuelChange()
        {
            if (!(ModEngineVariables.Substats.boostJuice == prevBoostJuice))
            {
                prevBoostJuice = ModEngineVariables.Substats.boostJuice;
                return true;
            }
            return false;
        }
        public static string MessageSent()
        {
            try
            {
                var input = GameObject.Find("ChatBox").GetComponentInChildren<InputField>();
                var text = input.text;
                if (text != prevText)
                {
                    if (text == null || text == "")
                    {
                        if (prevText != null && prevText != "")
                        {
                            input.text = "";
                            string data = prevText;
                            prevText = text;
                            return data;
                        }
                        input.text = "";
                    }
                    prevText = text;
                }
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
            return null;
        }
        public static bool BossHealthChanged()
        {
            int health = ModEngineComponents.GetObjectFromTag("Boss").GetComponent<ExteriorEnemyHealth>().Networkhealth;
            if (!(health == prevBossHealth))
            {
                prevBossHealth = health;
                return true;
            }
            return false;
        }
    }

    public class ModEngineItem
    {
        public ModEngineItem()
        {
            throw new NotImplementedException();
        }
    }

    public class ModEngineSubmarine
    {
        public ModEngineSubmarine()
        {
            throw new NotImplementedException();
        }
    }

    public class ModEngineNetwork
    {
        public ModEngineNetwork()
        {
            throw new NotImplementedException();
        }
    }

    public class ModEngineCommands
    {
        public static void SpawnCave(Vector3 spawnPos, float rotation, CaveBehavior.CaveType type)
        {
            ModEngineVariables.AIDM.SpawnCave(spawnPos, rotation, type);
        }
        public static void SpawnCiv()
        {
            ModEngineVariables.AIDM.GenerateCivEntrance();
        }
        public static void SpawnTimeTravller(float minWait, float maxWait, bool needsSaving)
        {
            ModEngineVariables.AIDM.SpawnTimeTraveler(minWait, maxWait, needsSaving);
        }
        public static void SpawnBoss()
        {
            ModEngineVariables.AIDM.SpawnBoss();
        }
        public static void SetItem(ItemSlot slot, ItemType type)
        {
            HandleWeapons handleWeps = ModEngineComponents.GetGameObjectWithComponent<HandleWeapons>().GetComponent<HandleWeapons>();
            handleWeps.SetItem(type, slot);
        }
        public static void SwitchItems()
        {
            HandleWeapons handleWeps = ModEngineComponents.GetGameObjectWithComponent<HandleWeapons>().GetComponent<HandleWeapons>();
            handleWeps.SwitchItems();
        }
        public static void ForceFire()
        {
            HandleWeapons handleWeps = ModEngineComponents.GetGameObjectWithComponent<HandleWeapons>().GetComponent<HandleWeapons>();
            handleWeps.DoItemAction();
        }
    }
}
