using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace WeNeedToModDeeperEngine
{
    public class ModEngineTextureChanger
    {
        public enum EnemyType
        {
            INTERIOR,
            EXTERIOR
        }

        public GameObject gameObject;

        public Dictionary<string, Sprite> Sprites = new Dictionary<string, Sprite>();

        private int units;

        public ModEngineTextureChanger(GameObject target, EnemyType type)
        {
            gameObject = target;
            PopulateSprites();
            if (type == EnemyType.EXTERIOR) units = 32;
            if (type == EnemyType.EXTERIOR) units = 200;
        }

        private void PopulateSprites()
        {
            foreach (var render in gameObject.GetComponentsInChildren<SpriteRenderer>())
            {
                try
                {
                    Sprites.Add(render.gameObject.name, render.sprite);
                }
                catch
                {
                    Debug.LogError("Mod engine error: Sprite already in dictionary, continuing");
                }
            }
        }

        public void UpdateSprites()
        {
            foreach (var render in gameObject.GetComponentsInChildren<SpriteRenderer>())
            {
                var name = render.gameObject.name;
                foreach (var entry in Sprites)
                {
                    if (entry.Key == name)
                    {
                        render.sprite = entry.Value;
                    }
                }
            }
        }
        public Sprite LoadSpriteFromFile(string spriteImageFilePath, Vector2 pivot, Sprite spritePivotToCopy = null, SpriteMeshType spriteType = SpriteMeshType.Tight)
        {
            if (spritePivotToCopy != null) pivot = spritePivotToCopy.pivot;
            try
            {
                Texture2D texture = LoadTexture(spriteImageFilePath);
                int width = texture.width;
                int height = texture.height;
                return Sprite.Create(texture, new Rect(0, 0, width, height), pivot, units, 0, spriteType);
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
}
