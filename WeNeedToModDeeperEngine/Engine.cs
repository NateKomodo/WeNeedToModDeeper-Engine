using System.Reflection;
using System.Collections.Generic;
using System.IO;
using System.Diagnostics;
using System;
using System.Collections;

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
            if (!Directory.Exists(folder)) //Check if directory exists
            {
                Directory.CreateDirectory(folder); //Create directory
                Debug.WriteLine("Folder does not exist, creating");
            }
            Debug.WriteLine("Loading plugins...");
            List<IPlugin> pluginList = GetPlugins<IPlugin>(folder); //Create a list of plugins
            foreach(IPlugin plugin in pluginList) //Loop through plugins and get details, then run the plugin
            {
                Debug.WriteLine("Loading plugin: " + plugin.GetPluginName() + " by " + plugin.GetAuthor() + ". Version " + plugin.GetPluginVersion());
                plugin.run();
            }
        }

        public List<T> GetPlugins<T>(string folder) //Loads plugins into a list
        {
            string[] files = Directory.GetFiles(folder, "*.dll"); //Get all DLLs in the plugin directory
            List<T> tList = new List<T>(); //Create a list for return and holding plugins
            Debug.Assert(typeof(T).IsInterface); //Check that T is an interface for debug
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
                    Debug.Fail(ex.Message); //Print error message over debug
                }
            }
            return tList; //Return the list
        }
    }
}
