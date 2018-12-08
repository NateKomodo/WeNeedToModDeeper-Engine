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

    public class ModEngineLegacyEventHandler
    {
        //Delegate void for event system
        public delegate void EventHandler();

        //Events to listen to for event system
        public static event EventHandler OnBossKilled;
        public static event EventHandler OnCaveLeave;
        public static event EventHandler OnGameStart;
        public static event EventHandler OnGameEnd;
        public static event EventHandler OnCaveEnter;
        public static event EventHandler OnBossStart;
        public static event EventHandler OnSteal;
        public static event EventHandler OnPlayerJoinedMidgame;
        public static event EventHandler OnPlayerLeftMidgame;
        public static event EventHandler OnSubmarineDamageCrash;
        public static event EventHandler OnSubmarineDamageDynamite;
        public static event EventHandler OnSubmarineDestroyed;
        public static event EventHandler OnLairGaurdianKilled;
        public static event EventHandler OnTacticalViewEnabled;
        public static event EventHandler OnTacticalViewDisabled;
        public static event EventHandler OnCivEnter;

        public ModEngineLegacyEventHandler() //CTOR for unifying events, bridges game events with this class for ease of use
        {
            ExteriorEnemyHealth.OnBossKilled += delegate ()
            {
                OnBossKilled.Invoke();
            };
            NetworkManagerBehavior.LeaveCave += delegate ()
            {
                OnCaveLeave.Invoke();
            };
            NetworkManagerBehavior.OnStartGame += delegate ()
            {
                OnGameStart.Invoke();
            };
            NetworkManagerBehavior.OnGameEnd += delegate ()
            {
                OnGameEnd.Invoke();
            };
            NetworkManagerBehavior.OnGameOver += delegate ()
            {
                OnGameEnd.Invoke();
            };
            NetworkManagerBehavior.OnGameEndEarly += delegate ()
            {
                OnGameEnd.Invoke();
            };
            NetworkManagerBehavior.OnGameEndLate += delegate ()
            {
                OnGameEnd.Invoke();
            };
            InteriorCameraBehavior.OnMyPlayerInCave += delegate ()
            {
                OnCaveEnter.Invoke();
            };
            InteriorCameraBehavior.OnMyPlayerOutOfCave += delegate ()
            {
                OnCaveLeave.Invoke();
            };
            ExteriorLevelGeneratorBehavior.OnStartedBossFight += delegate ()
            {
                OnBossStart.Invoke();
            };
            ShopCaseBehavior.OnShopBroken += delegate ()
            {
                OnSteal.Invoke();
            };
            PlayerNetworking.PlayerJoinedMidgame += delegate (UnityEngine.GameObject obj)
            {
                OnPlayerJoinedMidgame.Invoke();
            };
            PlayerNetworking.PlayerLeftMidGame += delegate (UnityEngine.GameObject obj)
            {
                OnPlayerLeftMidgame.Invoke();
            };
            PlayerNetworking.SubmarineDamageCrash += delegate ()
            {
                OnSubmarineDamageCrash.Invoke();
            };
            PlayerNetworking.SubmarineDamageDynamite += delegate ()
            {
                OnSubmarineDamageDynamite.Invoke();
            };
            MoveSubmarine.OnSubmarineDestroyed += delegate ()
            {
                OnSubmarineDestroyed.Invoke();
            };
            LairGuardianBossBehavior.OnLairGuardianKilled += delegate ()
            {
                OnLairGaurdianKilled.Invoke();
            };
            TacticalViewBehavior.OnEnabledTacticalView += delegate ()
            {
                OnTacticalViewEnabled.Invoke();
            };
            TacticalViewBehavior.OnDisabledTacticalView += delegate ()
            {
                OnTacticalViewDisabled.Invoke();
            };
            MusicManagerBehavior.OnCivMusic += delegate ()
            {
                OnCivEnter.Invoke();
            };
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
            get { return ModEngineComponents.GetGameObjectFromComponent<HandleWeapons>().GetComponent<HandleWeapons>(); }
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
        public static GameObject[] GetAllGameObjects()
        {
            try
            {
                return (GameObject[])GameObject.FindObjectsOfType(typeof(MonoBehaviour));
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message); //If theres an error (Such as if no class object is found) print the error and return false
                return null;
            }
        }
        public static GameObject GetGameObjectFromComponent<type>()
        {
            try
            {
                var obj = (GameObject)UnityEngine.Object.FindObjectOfType(typeof(type));
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

    public class ModEngineChatListener
    {
        public delegate void MessageEventHandler(string message);
        public static event MessageEventHandler OnChatMessage;

        public ModEngineChatListener()
        {
            var obj = (GameObject)ModEngineComponents.GetGameObjectFromComponent<InputField>();
            var comp = obj.GetComponent<InputField>();
            string prevText = "";
            while (true)
            {
                var text = comp.text;
                if (!(text == prevText))
                {
                    if ((text == "" || text == null) && !(prevText == "" || prevText == null))
                    {
                        OnChatMessage.Invoke(prevText);
                    }
                    prevText = text;
                }
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
    public class ModEngineEvents
    {
        public delegate void EventHandler();
        public delegate void IntEventHandler(int oldValue, int newValue);
        public delegate void BoolEventHandler(bool oldValue, bool newValue);
        public delegate void SubstatsEventHandler(SubStats oldValue, SubStats newValue);
        public delegate void AIDMEventHandler(AIDMBehavior oldValue, AIDMBehavior newValue);

        public event IntEventHandler OnGoldChange;
        public event IntEventHandler OnPlayerHealthChange;
        public event IntEventHandler OnPlayerMaxHealthChange;
        public event IntEventHandler OnBiomeChange;
        public event IntEventHandler OnSubHealthChange;
        public event IntEventHandler OnSubMaxHealthChange;
        public event BoolEventHandler OnDeathStatusChange;
        public event BoolEventHandler OnCaveStatusChange;
        public event BoolEventHandler OnCivStatusChanged;
        public event SubstatsEventHandler OnSubstatsChange;
        public event AIDMEventHandler OnAIDMChange;

        public ModEngineEvents()
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
            while (true)
            {
                if (!(ModEngineVariables.Gold == prevGold))
                {
                    OnGoldChange.Invoke(prevGold, ModEngineVariables.Gold);
                }
                prevGold = ModEngineVariables.Gold;

                if (!(ModEngineVariables.Playerhealth == prevHealth))
                {
                    OnPlayerHealthChange.Invoke(prevHealth, ModEngineVariables.Playerhealth);
                }
                prevHealth = ModEngineVariables.Playerhealth;

                if (!(ModEngineVariables.PlayerMaxHealth == prevMaxHealth))
                {
                    OnPlayerHealthChange.Invoke(prevMaxHealth, ModEngineVariables.PlayerMaxHealth);
                }
                prevMaxHealth = ModEngineVariables.PlayerMaxHealth;

                if (!(ModEngineVariables.WaterType == prevBiome))
                {
                    OnBiomeChange.Invoke(prevBiome, ModEngineVariables.WaterType);
                }
                prevBiome = ModEngineVariables.WaterType;

                if (!(ModEngineVariables.IsDead == prevDead))
                {
                    OnDeathStatusChange.Invoke(prevDead, ModEngineVariables.IsDead);
                }
                prevDead = ModEngineVariables.IsDead;

                if (!(ModEngineVariables.Substats.subHealth == prevSubHealth))
                {
                    OnSubHealthChange.Invoke(prevSubHealth, ModEngineVariables.Substats.subHealth);
                }
                prevSubHealth = ModEngineVariables.Substats.subHealth;

                if (!(ModEngineVariables.Substats.NetworkmaxSubHealth == prevSubMaxHealth))
                {
                    OnSubMaxHealthChange.Invoke(prevSubMaxHealth, ModEngineVariables.Substats.NetworkmaxSubHealth);
                }
                prevSubMaxHealth = ModEngineVariables.Substats.NetworkmaxSubHealth;

                if (!(ModEngineVariables.Substats == prevSubStats))
                {
                    OnSubstatsChange.Invoke(prevSubStats, ModEngineVariables.Substats);
                }
                prevSubStats = ModEngineVariables.Substats;

                if (!(AIDMBehavior.inCave == prevCave))
                {
                    OnCaveStatusChange.Invoke(prevCave, AIDMBehavior.inCave);
                }
                prevCave = AIDMBehavior.inCave;

                if (!(AIDMBehavior.inCiv == prevCiv))
                {
                    OnCivStatusChanged.Invoke(prevCiv, AIDMBehavior.inCiv);
                }
                prevCiv = AIDMBehavior.inCiv;

                if (!(ModEngineVariables.AIDM == prevAIDM))
                {
                    OnAIDMChange.Invoke(prevAIDM, ModEngineVariables.AIDM);
                }
                prevAIDM = ModEngineVariables.AIDM;
            }
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
            HandleWeapons handleWeps = ModEngineComponents.GetGameObjectFromComponent<HandleWeapons>().GetComponent<HandleWeapons>();
            handleWeps.SetItem(type, slot);
        }
        public static void SwitchItems()
        {
            HandleWeapons handleWeps = ModEngineComponents.GetGameObjectFromComponent<HandleWeapons>().GetComponent<HandleWeapons>();
            handleWeps.SwitchItems();
        }
        public static void ForceFire()
        {
            HandleWeapons handleWeps = ModEngineComponents.GetGameObjectFromComponent<HandleWeapons>().GetComponent<HandleWeapons>();
            handleWeps.DoItemAction();
        }
    }
}
