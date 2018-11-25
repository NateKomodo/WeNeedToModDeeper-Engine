using System.Reflection;
using System.Collections.Generic;
using System.IO;
using System;
using System.Collections;
using UnityEngine;

namespace WeNeedToModDeeperEngine //NOTE the types below will be added to the dll at runtime by a spereate tool.
{
    public interface IPlugin //Define Interface for any plugins, including details methods and a run method
    {
        string GetPluginName();
        string GetPluginVersion();
        string GetAuthor();
        void run();
    }

    public class ModEngine
    {
        public ModEngine()
        {
            new ModEngineLoader();
            new ModEngineEventHandler();
        }
    }

    public class ModEngineLoader
    {
        public static void Main(string[] arguments) //Main method for testing
        {
            new ModEngineLoader();
        }
        public ModEngineLoader() //Load plugins
        {
            string folder = "plugins";
            new ModEngineEventHandler(); //Init events
            string path = Path.Combine(System.AppContext.BaseDirectory, folder);
            if (!Directory.Exists(path)) //Check if directory exists
            {
                Directory.CreateDirectory(path); //Create directory
                System.Diagnostics.Debug.WriteLine("Folder does not exist, creating");
            }
            System.Diagnostics.Debug.WriteLine("Loading plugins...");
            List<IPlugin> pluginList = GetPlugins<IPlugin>(path); //Create a list of plugins
            foreach (IPlugin plugin in pluginList) //Loop through plugins and get details, then run the plugin
            {
                System.Diagnostics.Debug.WriteLine("Loading plugin: " + plugin.GetPluginName() + " by " + plugin.GetAuthor() + ". Version " + plugin.GetPluginVersion());
                try
                {
                    plugin.run(); //Run the plugin
                }
                catch (Exception ex) //If something breaks debug message with an error
                {
                    System.Diagnostics.Debug.WriteLine("Error loading plugin: " + plugin.GetPluginName() + " by " + plugin.GetAuthor() + ". Version " + plugin.GetPluginVersion() + ". Exception was: " + ex.Message);
                }
            }
        }

        public List<T> GetPlugins<T>(string folder) //Loads plugins into a list
        {
            string[] files = Directory.GetFiles(folder, "*.dll"); //Get all DLLs in the plugin directory
            List<T> tList = new List<T>(); //Create a list for return and holding plugins
            System.Diagnostics.Debug.Assert(typeof(T).IsInterface); //Check that T is an interface for debug
            foreach (string file in files) //Loop through the DLL files in the folder
            {
                try
                {
                    Assembly assembly = Assembly.LoadFile(file); //Load the assembly
                    foreach (Type type in assembly.GetTypes()) //Loop through types in the assembly
                    {
                        if (!type.IsClass || type.IsNotPublic) continue; //We only want classes that are public
                        Type[] interfaces = type.GetInterfaces(); //Get implemented interfaces
                        if (((IList)interfaces).Contains(typeof(T))) //If the class implements the IPlugin interface
                        {
                            object obj = Activator.CreateInstance(type); //instanciate the class
                            T t = (T)obj; //Cast it to the type
                            tList.Add(t); //Add it to the list
                        }
                    }
                }
                catch (Exception ex) //If something fails
                {
                    System.Diagnostics.Debug.Fail(ex.Message); //Print error message over debug
                }
            }
            return tList; //Return the list
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
            get { return GameObject.FindGameObjectWithTag("Player").GetComponent<HealthController>().playerHealth;  }
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

    //TODO: Commands?
}
