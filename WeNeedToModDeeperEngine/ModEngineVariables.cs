using System;
using System.Collections.Generic;
using UnityEngine;

namespace WeNeedToModDeeperEngine //NOTE the types below are a framework that mod makers can choose to include
{
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
        public static MoveCharacter MoveCharacter
        {
            get { return (MoveCharacter)ModEngineComponents.GetComponentFromObject<MoveCharacter>("Player"); }
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
            get { return NetworkManagerBehavior.myLocalPlayer.GetComponent<HealthController>().playerHealth; }
            set { NetworkManagerBehavior.myLocalPlayer.GetComponent<HealthController>().playerHealth = value; }
        }
        public static SubStats Substats
        {
            get { return GameControllerBehavior.subStats; }
            set { GameControllerBehavior.subStats = value; }
        }
        public static int PlayerMaxHealth
        {
            get { return NetworkManagerBehavior.myLocalPlayer.GetComponent<HealthController>().maxHealth; }
            set { NetworkManagerBehavior.myLocalPlayer.GetComponent<HealthController>().maxHealth = value; }
        }
        [Obsolete("GlobalStats may not function correctly, recomended you use GlobalStats.Variable instead")]
        public static GlobalStats PlayerStats
        {
            get { return NetworkManagerBehavior.myLocalPlayer.GetComponent<GlobalStats>(); }
        }
        public static AIDMBehavior AIDM
        {
            get { return GameControllerBehavior.AIDM; }
            set { GameControllerBehavior.AIDM = value; }
        }
        public static int WaterType
        {
            get
            {
                if (GameControllerBehavior.AIDM != null) { return GameControllerBehavior.AIDM.NetworkcurrentWaterType; } else { return 0; }
            }
            set { GameControllerBehavior.AIDM.NetworkcurrentWaterType = value; }
        }
        public static HealthController GetPlayerHealthController
        {
            get { return NetworkManagerBehavior.myLocalPlayer.GetComponent<HealthController>(); }
        }
        public static bool IsDead
        {
            get { return NetworkManagerBehavior.myLocalPlayer.GetComponent<HealthController>().dead; }
            set { NetworkManagerBehavior.myLocalPlayer.GetComponent<HealthController>().dead = value; }
        }
        public static HandleWeapons WeaponsHandler
        {
            get { return NetworkManagerBehavior.myLocalPlayer.GetComponent<HandleWeapons>(); }
        }
        public static Transform GetTransform(string obj)
        {
            return ModEngineComponents.GetObjectFromTag(obj).transform;
        }
        public static PlayerNetworking PlayerNetworking
        {
            get { return NetworkManagerBehavior.myPlayerNetworking; }
            set { NetworkManagerBehavior.myPlayerNetworking = value; }
        }
    }
}
