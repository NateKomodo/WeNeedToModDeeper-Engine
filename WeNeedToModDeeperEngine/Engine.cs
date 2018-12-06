using System;
using UnityEngine;

namespace WeNeedToModDeeperEngine //NOTE the types below will be added to the dll at runtime by a spereate tool.
{
    public class ModEngine
    {
        public static void Main()
        {
            //Just so visual studio doesnt kill me
        }
    }

    public class ModEngineEventHandler
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

        public ModEngineEventHandler() //CTOR for unifying events, bridges game events with this class for ease of use
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
    }

    public class ModEngineComponents //Class for loading components via unity
    {
        public static Component GetComponent(string gameObject, String classname) //Get a specified class from a game object
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

        public static GameObject GetObject(string gameObject) //Get a game object from a game object name
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
        public static Component[] GetAllComponents(string gameObject) //Get all components from a game object
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
        public static bool AddComponent(string gameObject, Type component)
        {
            try
            {
                GameObject.FindGameObjectWithTag(gameObject).AddComponent(component);
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
                return (GameObject[])GameObject.FindObjectsOfType(typeof(GameObject));
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message); //If theres an error (Such as if no class object is found) print the error and return false
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

    public class ModEngineChat
    {
        public delegate void ChatDelegate(string text);
        public static event ChatDelegate ChatEvent;

        public static void MessageSent(string text)
        {
            ChatEvent.Invoke(text);
        }
    }

    //TODO: Commands?
}
