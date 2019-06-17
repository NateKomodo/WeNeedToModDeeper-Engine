using UnityEngine.Networking;

namespace WeNeedToModDeeperEngine //NOTE the types below are a framework that mod makers can choose to include
{
    public class ModEngineChatMessage
    {
        public ModEngineChatMessage(string message, PlayerNetworking.ChatMessageType type)
        {
            if (!ModEngine.HasChecked) ModEngine.CheckForUpdates();
            if (NetworkServer.active)
            {
                NetworkManagerBehavior.myPlayerNetworking.CallRpcSetMessageParameters(message, (int)type, ModEngineComponents.GetInstanceID("Player")); //Send a chat message in any of the fonts
            }
            else
            {
                NetworkManagerBehavior.myPlayerNetworking.CallCmdCreateMessage(message, (int)type, ModEngineComponents.GetInstanceID("Player")); //Send a chat message in any of the fonts
            }
        }
    }
}
