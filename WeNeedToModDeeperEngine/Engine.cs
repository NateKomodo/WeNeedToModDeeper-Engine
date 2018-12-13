using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

namespace WeNeedToModDeeperEngine //NOTE the types below are a framework that mod makers can choose to include
{
    public class ModEngine
    {
        static string version = "2.5";
        static string gameversion = GlobalStats.version;

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
        public static GameObject Submarine
        {
            get { return GameObject.FindGameObjectWithTag("Submarine"); }
        }
        public static List<GameObject> PlayersInGame
        {
            get { return NetworkManagerBehavior.allPlayersInGame; }
        }
        public static List<GameObject> ExteriorEnemies
        {
            get
            {
                List<GameObject> list = new List<GameObject>();
                foreach (var comp in UnityEngine.Object.FindObjectsOfType<ExteriorEnemyHealth>())
                {
                    list.Add(comp.gameObject);
                }
                return list;
            }
        }
        public static List<GameObject> InteriorEnemies
        {
            get
            {
                List<GameObject> list = new List<GameObject>();
                foreach (var comp in UnityEngine.Object.FindObjectsOfType<InteriorEnemyDamageController>())
                {
                    list.Add(comp.gameObject);
                }
                return list;
            }
        }
        public static List<GameObject> Breaks
        {
            get
            {
                List<GameObject> list = new List<GameObject>();
                foreach (var comp in UnityEngine.Object.FindObjectsOfType<BreakBehavior>())
                {
                    list.Add(comp.gameObject);
                }
                return list;
            }
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
            get { return GameObject.FindGameObjectWithTag("Player").GetComponent<HandleWeapons>(); }
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
            if (NetworkServer.active)
            {
                NetworkManagerBehavior.myPlayerNetworking.CallRpcSetMessageParameters(message, (int)type, ModEngineComponents.GetInstanceID("Player")); //Send a chat message in any of the fonts
            }
            else
            {
                NetworkManagerBehavior.myPlayerNetworking.CallCmdCreateMessage(message, (int)type, ModEngineComponents.GetInstanceID("Player")); //Send a chat message in any of the fonts
            }
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

    public class ModEngineCanvasOverlay
    {
        public ModEngineCanvasOverlay(string message, int framesToLast)
        {
            TechLogCanvasBehavior.SetDisplayString(message, framesToLast);
        }
    }
    public class ModEngineTextTitle
    {
        public ModEngineTextTitle(string message)
        {
            GameObject gameObject = GameObject.Find("BoostText");
            gameObject.GetComponent<Animator>().SetTrigger("Enable");
            gameObject.GetComponent<Text>().text = message;
        }
    }

    public class ModEngineEvents
    {
        int prevGold = 0;
        int prevHealth = 10;
        int prevMaxHealth = 10;
        int prevBiome = 0;
        bool prevDead = false;
        int prevSubHealth = 0;
        int prevSubMaxHealth = 0;
        SubStats prevSubStats = null;
        bool prevCave = false;
        bool prevCiv = false;
        AIDMBehavior prevAIDM = null;
        float prevBoostJuice = 0f;
        string prevText = "";
        int prevBossHealth = 0;
        List<GameObject> prevConnected;

        public bool GoldChange()
        {
            if (!(ModEngineVariables.Gold == prevGold))
            {
                prevGold = ModEngineVariables.Gold;
                return true;
            }
            return false;
        }
        public bool PlayerHealthChange()
        {
            if (!(ModEngineVariables.Playerhealth == prevHealth))
            {
                prevHealth = ModEngineVariables.Playerhealth;
                return true;
            }
            return false;
        }
        public bool PlayerMaxHealthChange()
        {
            if (!(ModEngineVariables.PlayerMaxHealth == prevMaxHealth))
            {
                prevMaxHealth = ModEngineVariables.PlayerMaxHealth;
                return true;
            }
            return false;
        }
        public bool BiomeChange()
        {
            if (!(ModEngineVariables.WaterType == prevBiome))
            {
                prevBiome = ModEngineVariables.WaterType;
                return true;
            }
            return false;
        }
        public bool DeathStatusChange()
        {
            if (!(ModEngineVariables.IsDead == prevDead))
            {
                prevDead = ModEngineVariables.IsDead;
                return true;
            }
            return false;
        }
        public bool SubHealthChange()
        {
            if (!(ModEngineVariables.Substats.NetworksubHealth == prevSubHealth))
            {
                prevSubHealth = ModEngineVariables.Substats.NetworksubHealth;
                return true;
            }
            return false;
        }
        public bool SubMaxHealthChange()
        {
            if (!(ModEngineVariables.Substats.NetworkmaxSubHealth == prevSubMaxHealth))
            {
                prevSubMaxHealth = ModEngineVariables.Substats.NetworkmaxSubHealth;
                return true;
            }
            return false;
        }
        public bool SubStatsChanged()
        {
            if (!(ModEngineVariables.Substats == prevSubStats))
            {
                prevSubStats = ModEngineVariables.Substats;
                return true;
            }
            return false;
        }
        public bool CaveStatusChange()
        {
            if (!(AIDMBehavior.inCave == prevCave))
            {
                prevCave = AIDMBehavior.inCave;
                return true;
            }
            return false;
        }
        public bool CivStatusChange()
        {
            if (!(AIDMBehavior.inCiv == prevCiv))
            {
                prevCiv = AIDMBehavior.inCiv;
                return true;
            }
            return false;
        }
        public bool AIDMChange()
        {
            if (!(ModEngineVariables.AIDM == prevAIDM))
            {
                prevAIDM = ModEngineVariables.AIDM;
                return true;
            }
            return false;
        }
        public bool FuelChange()
        {
            if (!(ModEngineVariables.Substats.boostJuice == prevBoostJuice))
            {
                prevBoostJuice = ModEngineVariables.Substats.boostJuice;
                return true;
            }
            return false;
        }
        public string MessageSent()
        {
            try
            {
                var input = GameObject.FindObjectOfType<ChatBoxBehavior>().gameObject.GetComponentInChildren<InputField>();
                var text = input.text;
                if (text != prevText)
                {
                    if (text == null || text == "")
                    {
                        if (prevText != null && prevText != "")
                        {
                            //input.text = "";
                            string data = prevText;
                            prevText = text;
                            return data;
                        }
                        //input.text = "";
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
        public bool BossHealthChanged()
        {
            int health = ModEngineComponents.GetObjectFromTag("Boss").GetComponent<ExteriorEnemyHealth>().Networkhealth;
            if (!(health == prevBossHealth))
            {
                prevBossHealth = health;
                return true;
            }
            return false;
        }
        public bool ConnectedPlayersChanged()
        {
            var connected = NetworkManagerBehavior.allPlayersInGame;
            if (!(connected == prevConnected))
            {
                prevConnected = connected;
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
        public static void SpawnTimeTravller(bool needsSaving)
        {
            TimeTravelerSpawnPoint[] zones = UnityEngine.Object.FindObjectsOfType<TimeTravelerSpawnPoint>();
            int randZone = UnityEngine.Random.Range(0, zones.Length);
            Vector3 spawnPos = zones[randZone].transform.position;
            GameObject tt = UnityEngine.Object.Instantiate<GameObject>(ModEngineVariables.AIDM.timeTravelerPrefab, spawnPos, Quaternion.identity);
            TimeTravelerAI_Module timeTravelerAI = tt.GetComponent<TimeTravelerAI_Module>();
            if (timeTravelerAI != null && needsSaving)
            {
                timeTravelerAI.forceGood = true;
            }
            NetworkServer.Spawn(tt);
        }
        public static void SetItem(ItemSlot slot, ItemType type)
        {
            HandleWeapons handleWeps = ModEngineVariables.WeaponsHandler;
            handleWeps.SetItem(type, slot);
        }
        public static void SwitchItems()
        {
            HandleWeapons handleWeps = ModEngineVariables.WeaponsHandler;
            handleWeps.SwitchItems();
        }
        public static void ForceFire()
        {
            HandleWeapons handleWeps = ModEngineVariables.WeaponsHandler;
            handleWeps.DoItemAction();
        }
    }
    public class ModEngineSpawns
    {
        public static GameObject GetSpawnable(ModEngineSpawnables item)
        {
            GameObject toReturn = null;
            PickupManagerBehavior pm = GameControllerBehavior.pickupManager;
            foreach (var obj in pm.allConsumableSupplies)
            {
                if (obj.name == item.ToString()) toReturn = obj;
            }
            foreach (var obj in pm.allExteriorWeaponSwaps)
            {
                if (obj.name == item.ToString()) toReturn = obj;
            }
            foreach (var obj in pm.allHandheldItemPickups)
            {
                if (obj.name == item.ToString()) toReturn = obj;
            }
            foreach (var obj in pm.allMercenaries)
            {
                if (obj.name == item.ToString()) toReturn = obj;
            }
            foreach (var obj in pm.allPotions)
            {
                if (obj.name == item.ToString()) toReturn = obj;
            }
            foreach (var obj in pm.allSubUpgrades)
            {
                if (obj.name == item.ToString()) toReturn = obj;
            }
            foreach (var obj in pm.allTorpUpgrades)
            {
                if (obj.name == item.ToString()) toReturn = obj;
            }
            foreach (var obj in pm.allTrash)
            {
                if (obj.name == item.ToString()) toReturn = obj;
            }
            if (item.ToString() == "GoldPileLarge")
            {
                GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(GameControllerBehavior.AIDM.timeTravelerPrefab, GameObject.FindGameObjectWithTag("Player").transform.position, Quaternion.identity);
                TimeTravelerAI_Module comp = gameObject.GetComponent<TimeTravelerAI_Module>();
                foreach (var spawn in comp.goodSpawns)
                {
                    if (spawn.name == "GoldPileLarge") { toReturn = spawn; }
                }
                GameObject.Destroy(gameObject);
            }
            //GameObject.Find("CavePopulator").GetComponent<CavePopulatorBehavior>().treasureChestPrefabs
            return toReturn;
        }
        public static void SpawnObject(GameObject prefab, Vector2 pos)
        {
            var go = UnityEngine.Object.Instantiate<GameObject>(prefab, pos, Quaternion.identity);
            var comp = go.GetComponent<ItemPickupBehavior>();
            if (comp != null) { comp.NetworkshowDeadBody = false; }
            BackpackPickupBehavior component2 = go.GetComponent<BackpackPickupBehavior>();
            if (component2 != null) { component2.NetworkshowDeadBody = false; }
            NetworkServer.Spawn(go);
        }
        public static void SpawnTimeTravller(bool needsSaving)
        {
            TimeTravelerSpawnPoint[] zones = UnityEngine.Object.FindObjectsOfType<TimeTravelerSpawnPoint>();
            int randZone = UnityEngine.Random.Range(0, zones.Length);
            Vector3 spawnPos = zones[randZone].transform.position;
            GameObject tt = UnityEngine.Object.Instantiate<GameObject>(ModEngineVariables.AIDM.timeTravelerPrefab, spawnPos, Quaternion.identity);
            TimeTravelerAI_Module timeTravelerAI = tt.GetComponent<TimeTravelerAI_Module>();
            if (timeTravelerAI != null && needsSaving)
            {
                timeTravelerAI.forceGood = true;
            }
            NetworkServer.Spawn(tt);
        }
        public static void SpawnCave(Vector3 spawnPos, float rotation, CaveBehavior.CaveType type)
        {
            ModEngineVariables.AIDM.SpawnCave(spawnPos, rotation, type);
        }
    }

    public enum ModEngineSpawnables
    {
        FuelBarrel,
        Barrel,
        BatteryPickup,
        ShotgunModPickup,
        RifleModPickup,
        obj_ButcherKnifePickup,
        obj_ChemistryKitPickup,
        obj_DynamitePickup,
        obj_eldritchStaffPickup,
        obj_FlintlockPickup,
        obj_HealthKitPickup,
        obj_MonkeyWrenchPickup,
        obj_PhonographPickup,
        obj_pickAxePickup,
        obj_PipeWrenchPickup,
        obj_PirateSwordPickup,
        obj_RevolverPickup,
        obj_RivetGunPickup,
        obj_RosePickup,
        obj_SyringePickup,
        obj_TeslaGunPickup,
        obj_TranslationBookPickup,
        obj_WaterPumpPickup,
        obj_WrenchPickup,
        BackpackUpgrade,
        obj_PliersPickup,
        obj_CivilizationNPC,
        DamageElixir,
        HealthElixer,
        RepairElixir,
        BulletGrease,
        CannonUpgrade,
        EMPDamageUpgrade,
        EngineUpgrade,
        HullUpgrade,
        MaxFuelUpgrade,
        ShieldUpgrade,
        AcidModPickup,
        RocketModPickup,
        Ln2ModPickup,
        HomingModPickup,
        LaserModPickup,
        HazardousFuelModPickup,
        DuplicatorModPickup,
        PoisonElixer,
        obj_DevSkeleton,
        obj_BananaPickup,
        obj_Book,
        obj_RottenApple,
        obj_DynamiteSpawner,
        obj_BrokenSwordPickup,
        obj_CrabArmPickup,
        obj_MopPickup,
        obj_RustyWrenchPickup,
        TrashAttireUnlock,
        TrashHatUnlock,
        GoldPileLarge
    }
}
