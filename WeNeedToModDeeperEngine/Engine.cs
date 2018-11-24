using System.Reflection;
using System.Collections.Generic;
using System.IO;
using System.Diagnostics;
using System;
using System.Collections;

namespace WeNeedToModDeeperEngine //NOTE the class below will be injected into the dll at runtime.
{
    public interface IPlugin
    {
        string GetPluginName();
        string GetPluginVersion();
        string GetAuthor();
        void run();
    }

    public class ModEngineLoader
    {
        public ModEngineLoader(string folder)
        {
            if (!Directory.Exists(folder))
            {
                Directory.CreateDirectory(folder);
            }
            List<IPlugin> pluginList = GetPlugins<IPlugin>(folder);
            foreach(IPlugin plugin in pluginList)
            {
                Debug.Print("Loading plugin: " + plugin.GetPluginName() + " by " + plugin.GetAuthor() + ". Version " + plugin.GetPluginVersion());
                plugin.run();
            }
        }

        public List<T> GetPlugins<T>(string folder)
        {
            string[] files = Directory.GetFiles(folder, "*.dll");
            List<T> tList = new List<T>();
            Debug.Assert(typeof(T).IsInterface);
            foreach (string file in files)
            {
                try
                {
                    Assembly assembly = Assembly.LoadFile(file);
                    foreach (Type type in assembly.GetTypes())
                    {
                        if (!type.IsClass || type.IsNotPublic) continue;
                        Type[] interfaces = type.GetInterfaces();
                        if (((IList)interfaces).Contains(typeof(T)))
                        {
                            object obj = Activator.CreateInstance(type);
                            T t = (T)obj;
                            tList.Add(t);
                        }
                    }
                }
                catch (Exception ex)
                {
                    Debug.Fail(ex.Message);
                }
            }
            return tList;
        }
    }
}
