using System;
using System.IO;
using UnityEngine;

namespace WeNeedToModDeeperEngine //NOTE the types below are a framework that mod makers can choose to include
{
    public class ModEngineCustomItem
    {
        public enum ModItemType
        {
            MeleeRepair,
            MeleeCombat,
            RangedRepair,
            RangedCombat,
            MeleeHeal,
            MeleeMixed,
            RangedProjectile,
            CustomMelee,
            CustomRanged
        }

        public GameObject gameObject;

        public readonly ModItemType type;

        public readonly int repair;

        public readonly int damage;

        public readonly int heal;

        public readonly float healChance;

        public readonly float cureChance;

        public readonly float reload;

        public readonly Sprite sprite;

        public readonly Sprite projectile;

        public WeaponParent parent;

        public ModEngineCustomItem(Sprite itemSprite, ModItemType itemType, int repairValue, int combatValue, int healValue, float chanceToCure, float chanceToHeal, float reloadTime, WeaponParent customParent = null, Sprite projectileSprite = null)
        {
            type = itemType;
            repair = repairValue;
            damage = combatValue;
            heal = healValue;
            healChance = chanceToHeal;
            cureChance = chanceToCure;
            reload = reloadTime;
            sprite = itemSprite;
            projectile = projectileSprite;
            GameObject prefab;
            switch (itemType)
            {
                case ModItemType.MeleeRepair:
                    prefab = (GameObject)Resources.Load("obj_wrench");
                    gameObject = UnityEngine.Object.Instantiate<GameObject>(prefab);
                    parent = gameObject.GetComponent<WeaponParent>();
                    parent.myItemType = ItemType.wrench;
                    parent.gameObject.GetComponentInChildren<WrenchBehavior>().damage = repairValue;
                    parent.gameObject.GetComponentInChildren<WrenchBehavior>().combatDamage = combatValue;
                    parent.gameObject.GetComponentInChildren<WrenchBehavior>().reloadTime = reloadTime;
                    gameObject.SetActive(false);
                    break;
                case ModItemType.MeleeCombat:
                    prefab = (GameObject)Resources.Load("obj_pirateSword");
                    gameObject = UnityEngine.Object.Instantiate<GameObject>(prefab);
                    parent = gameObject.GetComponent<WeaponParent>();
                    parent.myItemType = ItemType.pirateSword;
                    parent.gameObject.GetComponentInChildren<PirateSwordBehavior>().damage = combatValue;
                    parent.gameObject.GetComponentInChildren<PirateSwordBehavior>().reloadTime = reloadTime;
                    gameObject.SetActive(false);
                    break;
                case ModItemType.MeleeHeal:
                    prefab = (GameObject)Resources.Load("obj_healthkit");
                    gameObject = UnityEngine.Object.Instantiate<GameObject>(prefab);
                    parent = gameObject.GetComponent<WeaponParent>();
                    parent.myItemType = ItemType.healthKit;
                    parent.gameObject.GetComponentInChildren<healthkitBehavior>().damage = combatValue;
                    parent.gameObject.GetComponentInChildren<healthkitBehavior>().healing = healValue;
                    parent.gameObject.GetComponentInChildren<healthkitBehavior>().chanceToCure = chanceToCure;
                    parent.gameObject.GetComponentInChildren<healthkitBehavior>().reloadTime = reloadTime;
                    gameObject.SetActive(false);
                    break;
                case ModItemType.MeleeMixed:
                    prefab = (GameObject)Resources.Load("obj_pliers");
                    gameObject = UnityEngine.Object.Instantiate<GameObject>(prefab);
                    parent = gameObject.GetComponent<WeaponParent>();
                    parent.myItemType = ItemType.pliers;
                    parent.gameObject.GetComponentInChildren<PliersBehavior>().damage = combatValue;
                    parent.gameObject.GetComponentInChildren<PliersBehavior>().healing = healValue;
                    parent.gameObject.GetComponentInChildren<PliersBehavior>().repair = repairValue;
                    parent.gameObject.GetComponentInChildren<PliersBehavior>().healChance = chanceToHeal;
                    parent.gameObject.GetComponentInChildren<PliersBehavior>().reloadTime = reloadTime;
                    gameObject.SetActive(false);
                    break;
                case ModItemType.RangedCombat:
                    prefab = (GameObject)Resources.Load("obj_flintlock");
                    gameObject = UnityEngine.Object.Instantiate<GameObject>(prefab);
                    parent = gameObject.GetComponent<WeaponParent>();
                    parent.myItemType = ItemType.flintlock;
                    parent.gameObject.GetComponentInChildren<FlintlockBehavior>().damage = combatValue;
                    parent.gameObject.GetComponentInChildren<FlintlockBehavior>().reloadTime = reloadTime;
                    gameObject.SetActive(false);
                    break;
                case ModItemType.RangedRepair:
                    prefab = (GameObject)Resources.Load("obj_rivetGun");
                    gameObject = UnityEngine.Object.Instantiate<GameObject>(prefab);
                    parent = gameObject.GetComponent<WeaponParent>();
                    parent.myItemType = ItemType.rivetGun;
                    parent.gameObject.GetComponentInChildren<RivetGunBehavior>().damage = combatValue;
                    parent.gameObject.GetComponentInChildren<RivetGunBehavior>().repairDamage = repairValue;
                    parent.gameObject.GetComponentInChildren<RivetGunBehavior>().reloadTime = reloadTime;
                    gameObject.SetActive(false);
                    break;
                case ModItemType.RangedProjectile:
                    prefab = (GameObject)Resources.Load("obj_eldritchStaff");
                    gameObject = UnityEngine.Object.Instantiate<GameObject>(prefab);
                    parent = gameObject.GetComponent<WeaponParent>();
                    parent.myItemType = ItemType.eldritchStaff;
                    parent.gameObject.GetComponentInChildren<eldritchStaffBehavior>().damage = combatValue;
                    parent.gameObject.GetComponentInChildren<eldritchStaffBehavior>().chanceToSpawnHorror = 0;
                    parent.gameObject.GetComponentInChildren<eldritchStaffBehavior>().reloadTime = reloadTime;
                    if (projectileSprite != null) parent.gameObject.GetComponentInChildren<eldritchStaffBehavior>().projectile.GetComponentInChildren<SpriteRenderer>().sprite = projectileSprite;
                    gameObject.SetActive(false);
                    break;
                case ModItemType.CustomMelee:
                    prefab = (GameObject)Resources.Load("obj_pliers");
                    gameObject = UnityEngine.Object.Instantiate<GameObject>(prefab);
                    GameObject.Destroy(gameObject.GetComponent<WeaponParent>());
                    if (customParent != null) gameObject.AddComponent(customParent.GetType());
                    parent = gameObject.GetComponent<WeaponParent>();
                    gameObject.SetActive(false);
                    break;
                case ModItemType.CustomRanged:
                    prefab = (GameObject)Resources.Load("obj_rivetGun");
                    gameObject = UnityEngine.Object.Instantiate<GameObject>(prefab);
                    GameObject.Destroy(gameObject.GetComponent<WeaponParent>());
                    if (customParent != null) gameObject.AddComponent(customParent.GetType());
                    parent = gameObject.GetComponent<WeaponParent>();
                    gameObject.SetActive(false);
                    break;
            }
            gameObject.GetComponentInChildren<SpriteRenderer>().sprite = itemSprite;
            gameObject.SetActive(true);
            ModEngineVariables.WeaponsHandler.item1 = parent;
            ModEngineVariables.WeaponsHandler.SwitchItems();
            ModEngineVariables.WeaponsHandler.SwitchItems();
            ModEngineVariables.WeaponsHandler.SendLoadoutInfo();
        }

        public void Unequip()
        {
            parent.Unequip();
        }

        public void Destroy()
        {
            Unequip();
            GameObject.Destroy(gameObject);
            
        }

        public void SceneContingencyCheck()
        {
            if (ModEngineVariables.WeaponsHandler.GetItem(ItemSlot.item1) == ItemType.none && ModEngineVariables.WeaponsHandler.item1 == null)
            {
                GameObject prefab;
                switch (type)
                {
                    case ModItemType.MeleeRepair:
                        prefab = (GameObject)Resources.Load("obj_wrench");
                        gameObject = UnityEngine.Object.Instantiate<GameObject>(prefab);
                        parent = gameObject.GetComponent<WeaponParent>();
                        parent.myItemType = ItemType.wrench;
                        parent.gameObject.GetComponentInChildren<WrenchBehavior>().damage = repair;
                        parent.gameObject.GetComponentInChildren<WrenchBehavior>().combatDamage = damage;
                        parent.gameObject.GetComponentInChildren<WrenchBehavior>().reloadTime = reload;
                        gameObject.SetActive(false);
                        break;
                    case ModItemType.MeleeCombat:
                        prefab = (GameObject)Resources.Load("obj_pirateSword");
                        gameObject = UnityEngine.Object.Instantiate<GameObject>(prefab);
                        parent = gameObject.GetComponent<WeaponParent>();
                        parent.myItemType = ItemType.pirateSword;
                        parent.gameObject.GetComponentInChildren<PirateSwordBehavior>().damage = damage;
                        parent.gameObject.GetComponentInChildren<PirateSwordBehavior>().reloadTime = reload;
                        gameObject.SetActive(false);
                        break;
                    case ModItemType.MeleeHeal:
                        prefab = (GameObject)Resources.Load("obj_healthkit");
                        gameObject = UnityEngine.Object.Instantiate<GameObject>(prefab);
                        parent = gameObject.GetComponent<WeaponParent>();
                        parent.myItemType = ItemType.healthKit;
                        parent.gameObject.GetComponentInChildren<healthkitBehavior>().damage = damage;
                        parent.gameObject.GetComponentInChildren<healthkitBehavior>().healing = heal;
                        parent.gameObject.GetComponentInChildren<healthkitBehavior>().chanceToCure = cureChance;
                        parent.gameObject.GetComponentInChildren<healthkitBehavior>().reloadTime = reload;
                        gameObject.SetActive(false);
                        break;
                    case ModItemType.MeleeMixed:
                        prefab = (GameObject)Resources.Load("obj_pliers");
                        gameObject = UnityEngine.Object.Instantiate<GameObject>(prefab);
                        parent = gameObject.GetComponent<WeaponParent>();
                        parent.myItemType = ItemType.pliers;
                        parent.gameObject.GetComponentInChildren<PliersBehavior>().damage = damage;
                        parent.gameObject.GetComponentInChildren<PliersBehavior>().healing = heal;
                        parent.gameObject.GetComponentInChildren<PliersBehavior>().repair = repair;
                        parent.gameObject.GetComponentInChildren<PliersBehavior>().healChance = healChance;
                        parent.gameObject.GetComponentInChildren<PliersBehavior>().reloadTime = reload;
                        gameObject.SetActive(false);
                        break;
                    case ModItemType.RangedCombat:
                        prefab = (GameObject)Resources.Load("obj_flintlock");
                        gameObject = UnityEngine.Object.Instantiate<GameObject>(prefab);
                        parent = gameObject.GetComponent<WeaponParent>();
                        parent.myItemType = ItemType.flintlock;
                        parent.gameObject.GetComponentInChildren<FlintlockBehavior>().damage = damage;
                        parent.gameObject.GetComponentInChildren<FlintlockBehavior>().reloadTime = damage;
                        gameObject.SetActive(false);
                        break;
                    case ModItemType.RangedRepair:
                        prefab = (GameObject)Resources.Load("obj_rivetGun");
                        gameObject = UnityEngine.Object.Instantiate<GameObject>(prefab);
                        parent = gameObject.GetComponent<WeaponParent>();
                        parent.myItemType = ItemType.rivetGun;
                        parent.gameObject.GetComponentInChildren<RivetGunBehavior>().damage = damage;
                        parent.gameObject.GetComponentInChildren<RivetGunBehavior>().repairDamage = repair;
                        parent.gameObject.GetComponentInChildren<RivetGunBehavior>().reloadTime = reload;
                        gameObject.SetActive(false);
                        break;
                    case ModItemType.RangedProjectile:
                        prefab = (GameObject)Resources.Load("obj_eldritchStaff");
                        gameObject = UnityEngine.Object.Instantiate<GameObject>(prefab);
                        parent = gameObject.GetComponent<WeaponParent>();
                        parent.myItemType = ItemType.eldritchStaff;
                        parent.gameObject.GetComponentInChildren<eldritchStaffBehavior>().damage = damage;
                        parent.gameObject.GetComponentInChildren<eldritchStaffBehavior>().chanceToSpawnHorror = 0;
                        parent.gameObject.GetComponentInChildren<eldritchStaffBehavior>().reloadTime = reload;
                        if (projectile != null) parent.gameObject.GetComponentInChildren<eldritchStaffBehavior>().projectile.GetComponentInChildren<SpriteRenderer>().sprite = projectile;
                        gameObject.SetActive(false);
                        break;
                    case ModItemType.CustomMelee:
                        prefab = (GameObject)Resources.Load("obj_pliers");
                        gameObject = UnityEngine.Object.Instantiate<GameObject>(prefab);
                        GameObject.Destroy(gameObject.GetComponent<WeaponParent>());
                        if (parent != null) gameObject.AddComponent(parent.GetType());
                        parent = gameObject.GetComponent<WeaponParent>();
                        gameObject.SetActive(false);
                        break;
                    case ModItemType.CustomRanged:
                        prefab = (GameObject)Resources.Load("obj_rivetGun");
                        gameObject = UnityEngine.Object.Instantiate<GameObject>(prefab);
                        GameObject.Destroy(gameObject.GetComponent<WeaponParent>());
                        if (parent != null) gameObject.AddComponent(parent.GetType());
                        parent = gameObject.GetComponent<WeaponParent>();
                        gameObject.SetActive(false);
                        break;
                }
                gameObject.GetComponentInChildren<SpriteRenderer>().sprite = sprite;
                gameObject.SetActive(true);
                ModEngineVariables.WeaponsHandler.item1 = parent;
                ModEngineVariables.WeaponsHandler.SwitchItems();
                ModEngineVariables.WeaponsHandler.SwitchItems();
                ModEngineVariables.WeaponsHandler.SendLoadoutInfo();
            }
        }

        public static Sprite LoadSpriteFromFile(string spriteImageFilePath, Vector2 pivot, int units = 350, SpriteMeshType spriteType = SpriteMeshType.Tight)
        {
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

        private static Texture2D LoadTexture(string FilePath)
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
