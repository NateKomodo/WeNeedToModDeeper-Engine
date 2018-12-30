using UnityEngine;
using UnityEngine.Networking;

namespace WeNeedToModDeeperEngine //NOTE the types below are a framework that mod makers can choose to include
{
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
}
