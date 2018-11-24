using System.Reflection;
using System.Collections.Generic;
using System.IO;
using System.Diagnostics;
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

    public class ModEngineLoader
    {
        public static void Main(string[] arguments) //Main method for testing
        {
            new ModEngineLoader(@"plugins");
        }
        public ModEngineLoader(string folder) //CTOR takes in folder for loading mods from
        {
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
                plugin.run();
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
        public event EventHandler OnBossKilled;
        public event EventHandler OnCaveLeave;
        public event EventHandler OnGameStart;
        public event EventHandler OnGameEnd;
        public event EventHandler OnCaveEnter;
        public event EventHandler OnBossStart;
        public event EventHandler OnSteal;
        public event EventHandler OnPlayerJoinedMidgame;
        public event EventHandler OnPlayerLeftMidgame;
        public event EventHandler OnSubmarineDamageCrash;
        public event EventHandler OnSubmarineDamageDynamite;
        public event EventHandler OnSubmarineDestroyed;
        public event EventHandler OnLairGaurdianKilled;
        public event EventHandler OnTacticalViewEnabled;
        public event EventHandler OnTacticalViewDisabled;
        public event EventHandler OnCivEnter;

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

    public class ModEngineVariables
    {
        public int gold
        {
            get { return GoldControllerBehavior.gold; }
            set { GoldControllerBehavior.gold = value; }
        }
        public bool devmode
        {
            get { return SubStats.devMode; }
            set { SubStats.devMode = value; }
        }
        public int playerhealth
        {
            get { return GameObject.FindGameObjectWithTag("Player").GetComponent<HealthController>().playerHealth;  }
            set { GameObject.FindGameObjectWithTag("Player").GetComponent<HealthController>().playerHealth = value; }
        }
        public SubStats substats
        {
            get { return GameControllerBehavior.subStats; }
            set { GameControllerBehavior.subStats = value; }
        }
        public int playerMaxHealth
        {
            get { return GameObject.FindGameObjectWithTag("Player").GetComponent<HealthController>().maxHealth; }
            set { GameObject.FindGameObjectWithTag("Player").GetComponent<HealthController>().maxHealth = value; }
        }
    }
    public class ModEngineComponents
    {
        public HealthController playerHealthController
        {
            get { return GameObject.FindGameObjectWithTag("Player").GetComponent<HealthController>(); }
        }
        public SubStats substats
        {
            get { return GameControllerBehavior.subStats; }
            set { GameControllerBehavior.subStats = value; }
        }
    }
}
