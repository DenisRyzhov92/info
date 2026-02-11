using UnityEngine;
using UnityEditor;
using System.IO;

/// <summary>
/// Editor меню для создания дефолтного конфига Space Farm Idle Clicker.
/// </summary>
public class CreateDefaultConfig : EditorWindow
{
    [MenuItem("Space Farm/Create Default Config")]
    public static void CreateConfig()
    {
        // Создаём папку Resources если её нет
        string resourcesPath = "Assets/Resources";
        if (!AssetDatabase.IsValidFolder(resourcesPath))
        {
            AssetDatabase.CreateFolder("Assets", "Resources");
        }

        // Создаём конфиг
        IdleClickerConfig config = ScriptableObject.CreateInstance<IdleClickerConfig>();

        // Стартовые параметры
        config.startingBioGel = 0;
        config.startingBioGelPerTap = 1;
        config.startingBioGelPerSecond = 0f;

        // Апгрейды
        config.upgrades.Clear();

        // 1. Manual Harvest Protocol
        var upgrade1 = new UpgradeDefinition
        {
            id = "manual_harvest",
            displayName = "Manual Harvest Protocol",
            description = "Увеличивает количество BioGel за тап",
            baseCost = 10,
            costMultiplier = 1.7f,
            bonusType = UpgradeBonusType.TapBoost,
            bonusValue = 1f,
            unlockLifetimeBioGel = 0
        };
        config.upgrades.Add(upgrade1);

        // 2. Micro Drone Swarm
        var upgrade2 = new UpgradeDefinition
        {
            id = "drone_swarm",
            displayName = "Micro Drone Swarm",
            description = "Дроны начинают автоматически собирать BioGel",
            baseCost = 50,
            costMultiplier = 1.8f,
            bonusType = UpgradeBonusType.IdleIncome,
            bonusValue = 0.5f,
            unlockLifetimeBioGel = 100
        };
        config.upgrades.Add(upgrade2);

        // 3. Hydroponic Racks
        var upgrade3 = new UpgradeDefinition
        {
            id = "hydroponic_racks",
            displayName = "Hydroponic Racks",
            description = "Увеличивает пассивный доход",
            baseCost = 200,
            costMultiplier = 1.9f,
            bonusType = UpgradeBonusType.IdleIncome,
            bonusValue = 2f,
            unlockLifetimeBioGel = 1000
        };
        config.upgrades.Add(upgrade3);

        // 4. Orbital Greenhouse
        var upgrade4 = new UpgradeDefinition
        {
            id = "orbital_greenhouse",
            displayName = "Orbital Greenhouse",
            description = "Значительно увеличивает пассивный доход",
            baseCost = 1000,
            costMultiplier = 2f,
            bonusType = UpgradeBonusType.IdleIncome,
            bonusValue = 10f,
            unlockLifetimeBioGel = 10000
        };
        config.upgrades.Add(upgrade4);

        // 5. Solar Mirror Array
        var upgrade5 = new UpgradeDefinition
        {
            id = "solar_mirror",
            displayName = "Solar Mirror Array",
            description = "Множитель ко всему доходу",
            baseCost = 10000,
            costMultiplier = 2.2f,
            bonusType = UpgradeBonusType.IncomeMultiplier,
            bonusValue = 1.5f,
            unlockLifetimeBioGel = 100000
        };
        config.upgrades.Add(upgrade5);

        // 6. Terraforming AI
        var upgrade6 = new UpgradeDefinition
        {
            id = "terraform_ai",
            displayName = "Terraforming AI",
            description = "Финальный апгрейд, максимальный пассивный доход",
            baseCost = 100000,
            costMultiplier = 2.5f,
            bonusType = UpgradeBonusType.IdleIncome,
            bonusValue = 100f,
            unlockLifetimeBioGel = 1000000
        };
        config.upgrades.Add(upgrade6);

        // Бусты за BioGel
        config.boostOffers.Clear();

        // Early Game
        var boost1 = new ProgressBoostOfferDefinition
        {
            id = "ion_pulse",
            displayName = "Ion Pulse",
            description = "x2 пассивный доход на 60 секунд",
            cost = 100,
            effectType = BoostEffectType.IdleMultiplier,
            multiplier = 2f,
            durationSeconds = 60,
            unlockLifetimeBioGel = 500
        };
        config.boostOffers.Add(boost1);

        var boost2 = new ProgressBoostOfferDefinition
        {
            id = "solar_focus",
            displayName = "Solar Focus",
            description = "x3 тап на 30 секунд",
            cost = 50,
            effectType = BoostEffectType.TapMultiplier,
            multiplier = 3f,
            durationSeconds = 30,
            unlockLifetimeBioGel = 200
        };
        config.boostOffers.Add(boost2);

        var boost3 = new ProgressBoostOfferDefinition
        {
            id = "drone_overclock",
            displayName = "Drone Overclock",
            description = "x2 пассивный доход на 120 секунд",
            cost = 200,
            effectType = BoostEffectType.IdleMultiplier,
            multiplier = 2f,
            durationSeconds = 120,
            unlockLifetimeBioGel = 1000
        };
        config.boostOffers.Add(boost3);

        // Mid Game
        var boost4 = new ProgressBoostOfferDefinition
        {
            id = "orbital_sync",
            displayName = "Orbital Sync",
            description = "x1.5 ко всему доходу на 300 секунд",
            cost = 500,
            effectType = BoostEffectType.AllIncomeMultiplier,
            multiplier = 1.5f,
            durationSeconds = 300,
            unlockLifetimeBioGel = 10000
        };
        config.boostOffers.Add(boost4);

        var boost5 = new ProgressBoostOfferDefinition
        {
            id = "bioreactor_surge",
            displayName = "Bioreactor Surge",
            description = "x3 пассивный доход на 180 секунд",
            cost = 1000,
            effectType = BoostEffectType.IdleMultiplier,
            multiplier = 3f,
            durationSeconds = 180,
            unlockLifetimeBioGel = 50000
        };
        config.boostOffers.Add(boost5);

        // Late Game
        var boost6 = new ProgressBoostOfferDefinition
        {
            id = "plasma_wave",
            displayName = "Plasma Wave",
            description = "x5 ко всему доходу на 60 секунд",
            cost = 5000,
            effectType = BoostEffectType.AllIncomeMultiplier,
            multiplier = 5f,
            durationSeconds = 60,
            unlockLifetimeBioGel = 500000
        };
        config.boostOffers.Add(boost6);

        var boost7 = new ProgressBoostOfferDefinition
        {
            id = "terraform_rush",
            displayName = "Terraform Rush",
            description = "x10 ко всему доходу на 120 секунд",
            cost = 20000,
            effectType = BoostEffectType.AllIncomeMultiplier,
            multiplier = 10f,
            durationSeconds = 120,
            unlockLifetimeBioGel = 2000000
        };
        config.boostOffers.Add(boost7);

        // IAP продукты
        config.realMoneyProducts.Clear();

        var product1 = new RealMoneyProductDefinition
        {
            productId = "starter_supply_drop",
            displayName = "Starter Supply Drop",
            description = "Начальный набор для быстрого старта",
            bioGelReward = 1000,
            boostRewards = new string[] { "ion_pulse", "solar_focus" },
            boostRewardCounts = new int[] { 1, 1 }
        };
        config.realMoneyProducts.Add(product1);

        var product2 = new RealMoneyProductDefinition
        {
            productId = "terraform_booster",
            displayName = "Terraform Booster",
            description = "Большой набор ресурсов для ускорения прогресса",
            bioGelReward = 50000,
            boostRewards = new string[] { "orbital_sync", "bioreactor_surge" },
            boostRewardCounts = new int[] { 2, 1 }
        };
        config.realMoneyProducts.Add(product2);

        var product3 = new RealMoneyProductDefinition
        {
            productId = "colony_expansion_bundle",
            displayName = "Colony Expansion Bundle",
            description = "Максимальный набор для быстрого развития",
            bioGelReward = 500000,
            boostRewards = new string[] { "plasma_wave", "terraform_rush" },
            boostRewardCounts = new int[] { 3, 1 }
        };
        config.realMoneyProducts.Add(product3);

        // Сохраняем asset
        string assetPath = "Assets/Resources/IdleClickerConfig.asset";
        AssetDatabase.CreateAsset(config, assetPath);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();

        Debug.Log($"[CreateDefaultConfig] Дефолтный конфиг создан: {assetPath}");
        EditorUtility.DisplayDialog("Конфиг создан", 
            $"Дефолтный конфиг Space Farm Idle Clicker создан в:\n{assetPath}\n\nНе забудьте назначить его в IdleClickerManager!", 
            "OK");

        // Выделяем созданный asset
        Selection.activeObject = config;
        EditorGUIUtility.PingObject(config);
    }
}
