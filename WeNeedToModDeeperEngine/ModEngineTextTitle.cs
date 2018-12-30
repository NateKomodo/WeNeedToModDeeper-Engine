using UnityEngine;
using UnityEngine.UI;

namespace WeNeedToModDeeperEngine //NOTE the types below are a framework that mod makers can choose to include
{
    public class ModEngineTextTitle
    {
        public ModEngineTextTitle(string message)
        {
            if (!ModEngine.HasChecked) ModEngine.CheckForUpdates();
            GameObject gameObject = GameObject.Find("BoostText");
            gameObject.GetComponent<Animator>().SetTrigger("Enable");
            gameObject.GetComponent<Text>().text = message;
        }
    }
}
