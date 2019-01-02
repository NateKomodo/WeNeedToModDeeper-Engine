using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace WeNeedToModDeeperEngine
{
    public class ModEngineLobbyInterface
    {
        LobbyManagerBehavior LobbyManagerBehavior;

        public Sprite[] SpritesForVessels
        {
            get { return LobbyManagerBehavior.spritesForVessels; }
            set { LobbyManagerBehavior.spritesForVessels = value; }
        }

        public String[] DescriptionsForVessels
        {
            get { return LobbyManagerBehavior.descriptionsForVessels; }
            set { LobbyManagerBehavior.descriptionsForVessels = value; }
        }

        public String[] ScencesForVessels
        {
            get { return LobbyManagerBehavior.scenesForVessels; }
            set { LobbyManagerBehavior.scenesForVessels = value; }
        }

        public ModEngineLobbyInterface(LobbyManagerBehavior managerBehavior)
        {
            LobbyManagerBehavior = managerBehavior;
        }

        public void AddSubmarine(string title, string description, Sprite image, string sceneName)
        {
            LobbyManagerBehavior.spritesForVessels = AddToArraySprite(LobbyManagerBehavior.spritesForVessels, image);
            LobbyManagerBehavior.descriptionsForVessels = AddToArrayString(LobbyManagerBehavior.descriptionsForVessels, description);
            LobbyManagerBehavior.scenesForVessels = AddToArrayString(LobbyManagerBehavior.scenesForVessels, title + "_" + sceneName);
        }

        private Sprite[] AddToArraySprite(Sprite[] array, Sprite image)
        {
            List<Sprite> list = new List<Sprite>();
            foreach (var entry in array) list.Add(entry);
            list.Add(image);
            return list.ToArray();
        }

        private string[] AddToArrayString(string[] array, string text)
        {
            List<string> list = new List<string>();
            foreach (var entry in array) list.Add(entry);
            list.Add(text);
            return list.ToArray();
        }

        public Sprite LoadSpriteFromFile(string spriteImageFilePath, Vector2 pivot, SpriteMeshType spriteType = SpriteMeshType.Tight)
        {
            try
            {
                Texture2D texture = LoadTexture(spriteImageFilePath);
                int width = texture.width;
                int height = texture.height;
                return Sprite.Create(texture, new Rect(0, 0, width, height), pivot, 32, 0, spriteType);
            }
            catch (Exception ex)
            {
                UnityEngine.Debug.LogError("ModEngine Error while loading sprite: " + ex.Message);
                return null;
            }
        }

        private Texture2D LoadTexture(string FilePath)
        {
            Texture2D Tex2D;
            byte[] FileData;
            if (File.Exists(FilePath))
            {
                FileData = File.ReadAllBytes(FilePath);
                Tex2D = new Texture2D(2, 2);
                if (Tex2D.LoadImage(FileData))
                    return Tex2D;
            }
            return null;
        }
    }

    public class ModEngineItemMenuInterface
    {
        public ModEngineItemMenuInterface()
        {
            throw new NotImplementedException();
        }
    }
}
