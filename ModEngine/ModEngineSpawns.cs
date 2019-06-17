using UnityEngine;
using UnityEngine.Networking;

namespace WeNeedToModDeeperEngine //NOTE the types below are a framework that mod makers can choose to include
{
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
}
