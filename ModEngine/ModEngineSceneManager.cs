using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine.SceneManagement;

namespace WeNeedToModDeeperEngine
{
    public class ModEngineSceneManager
    {
        public ModEngineSceneManager()
        {
            if (!ModEngine.HasChecked) ModEngine.CheckForUpdates();
        }

        public string GetCurrentSceneName()
        {
            return SceneManager.GetActiveScene().name;
        }

        public int GetCurrentSceneIndex()
        {
            return SceneManager.GetActiveScene().buildIndex;
        }

        public Scene GetCurrentScene()
        {
            return SceneManager.GetActiveScene();
        }

        public Scene GetSceneAtIndex(int index)
        {
            return SceneManager.GetSceneByBuildIndex(index);
        }

        public Scene GetSceneByName(string name)
        {
            return SceneManager.GetSceneByName(name);
        }

        public void LoadScene(string name)
        {
            SceneManager.LoadScene(name);
        }

        public void UnloadScene(string name)
        {
            SceneManager.UnloadSceneAsync(name);
        }

        public Scene CreateScene(string name)
        {
            return SceneManager.CreateScene(name);
        }

        public Scene GetActiveSceneAtIndex(int index)
        {
            return SceneManager.GetSceneAt(index);
        }

        public void SetActive(Scene scene)
        {
            SceneManager.SetActiveScene(scene);
        }
    }
}
