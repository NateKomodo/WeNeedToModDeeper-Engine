using System;
using UnityEngine;
using UnityEngine.UI;

namespace WeNeedToModDeeperEngine //NOTE the types below are a framework that mod makers can choose to include
{
    public class ModEngineEvents
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
        float prevBoostJuice = 0f;
        string prevText = "";
        int prevBossHealth = 0;
        GameObject[] prevConnected = new GameObject[1];
        LobbyManagerBehavior prevLobby = null;
        int prevSceneBuildIndex = -1;

        public ModEngineEvents()
        {
            if (!ModEngine.HasChecked) ModEngine.CheckForUpdates();
        }

        public bool SceneChanged()
        {
            var scene = new ModEngineSceneManager();
            int current = scene.GetCurrentSceneIndex();
            if (prevSceneBuildIndex != current) return true;
            return false;
        }

        public bool LobbyInitialized()
        {
            LobbyManagerBehavior lobby = GameObject.FindObjectOfType<LobbyManagerBehavior>();
            if (lobby != null && prevLobby == null) return true;
            return false;
        }

        public bool GoldChange()
        {
            if (!(ModEngineVariables.Gold == prevGold))
            {
                prevGold = ModEngineVariables.Gold;
                return true;
            }
            return false;
        }
        public bool PlayerHealthChange()
        {
            if (!(ModEngineVariables.Playerhealth == prevHealth))
            {
                prevHealth = ModEngineVariables.Playerhealth;
                return true;
            }
            return false;
        }
        public bool PlayerMaxHealthChange()
        {
            if (!(ModEngineVariables.PlayerMaxHealth == prevMaxHealth))
            {
                prevMaxHealth = ModEngineVariables.PlayerMaxHealth;
                return true;
            }
            return false;
        }
        public bool BiomeChange()
        {
            if (!(ModEngineVariables.WaterType == prevBiome))
            {
                prevBiome = ModEngineVariables.WaterType;
                return true;
            }
            return false;
        }
        public bool DeathStatusChange()
        {
            if (!(ModEngineVariables.IsDead == prevDead))
            {
                prevDead = ModEngineVariables.IsDead;
                return true;
            }
            return false;
        }
        public bool SubHealthChange()
        {
            if (!(ModEngineVariables.Substats.NetworksubHealth == prevSubHealth))
            {
                prevSubHealth = ModEngineVariables.Substats.NetworksubHealth;
                return true;
            }
            return false;
        }
        public bool SubMaxHealthChange()
        {
            if (!(ModEngineVariables.Substats.NetworkmaxSubHealth == prevSubMaxHealth))
            {
                prevSubMaxHealth = ModEngineVariables.Substats.NetworkmaxSubHealth;
                return true;
            }
            return false;
        }
        public bool SubStatsChanged()
        {
            if (!(ModEngineVariables.Substats == prevSubStats))
            {
                prevSubStats = ModEngineVariables.Substats;
                return true;
            }
            return false;
        }
        public bool CaveStatusChange()
        {
            if (!(AIDMBehavior.inCave == prevCave))
            {
                prevCave = AIDMBehavior.inCave;
                return true;
            }
            return false;
        }
        public bool CivStatusChange()
        {
            if (!(AIDMBehavior.inCiv == prevCiv))
            {
                prevCiv = AIDMBehavior.inCiv;
                return true;
            }
            return false;
        }
        public bool AIDMChange()
        {
            if (!(ModEngineVariables.AIDM == prevAIDM))
            {
                prevAIDM = ModEngineVariables.AIDM;
                return true;
            }
            return false;
        }
        public bool FuelChange()
        {
            if (!(ModEngineVariables.Substats.boostJuice == prevBoostJuice))
            {
                prevBoostJuice = ModEngineVariables.Substats.boostJuice;
                return true;
            }
            return false;
        }
        public string MessageSent()
        {
            try
            {
                var input = GameObject.FindObjectOfType<ChatBoxBehavior>().gameObject.GetComponentInChildren<InputField>();
                if (input == null) return null;
                var text = input.text;
                if (text != prevText)
                {
                    if (text == null || text == "")
                    {
                        if (prevText != null && prevText != "")
                        {
                            string data = prevText;
                            prevText = text;
                            return data;
                        }
                    }
                    prevText = text;
                }
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
            return null;
        }
        public bool BossHealthChanged()
        {
            int health = ModEngineComponents.GetObjectFromTag("Boss").GetComponent<ExteriorEnemyHealth>().Networkhealth;
            if (!(health == prevBossHealth))
            {
                prevBossHealth = health;
                return true;
            }
            return false;
        }
        public bool ConnectedPlayersChanged()
        {
            var connected = NetworkManagerBehavior.allPlayersInGame;
            GameObject[] current = connected.ToArray();
            if (current != prevConnected)
            {
                prevConnected = current;
                return true;
            }
            return false;
        }
        public bool KeyPressed(KeyCode key)
        {
            if (Input.GetKeyDown(key)) return true;
            return false;
        }
        public bool KeyReleased(KeyCode key)
        {
            if (Input.GetKeyUp(key)) return true;
            return false;
        }
        public bool KeyHeld(KeyCode key)
        {
            if (Input.GetKey(key)) return true;
            return false;
        }
    }
}
