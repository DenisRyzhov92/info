using UnityEngine;

/// <summary>
/// Определение временного буста за BioGel (Progress Boost Shop).
/// Используется в ScriptableObject конфиге для настройки магазина бустов.
/// </summary>
[System.Serializable]
public class ProgressBoostOfferDefinition
{
    [Header("Основная информация")]
    [Tooltip("Уникальный ID буста (например, 'ion_pulse', 'solar_focus')")]
    public string id;

    [Tooltip("Название буста для отображения в UI")]
    public string displayName;

    [Tooltip("Описание буста для UI")]
    [TextArea(2, 4)]
    public string description;

    [Header("Экономика")]
    [Tooltip("Стоимость буста в BioGel")]
    public long cost;

    [Header("Эффект")]
    [Tooltip("Тип эффекта: TapMultiplier, IdleMultiplier, AllIncomeMultiplier")]
    public BoostEffectType effectType;

    [Tooltip("Множитель эффекта (например, 2.0 = x2 доход)")]
    [Range(1.1f, 10f)]
    public float multiplier = 2f;

    [Tooltip("Длительность буста в секундах")]
    public int durationSeconds = 60;

    [Header("Unlock")]
    [Tooltip("Минимальный lifetime BioGel для разблокировки этого буста")]
    public long unlockLifetimeBioGel = 0;
}

/// <summary>
/// Тип эффекта временного буста.
/// </summary>
public enum BoostEffectType
{
    TapMultiplier,          // Множитель только для тапа
    IdleMultiplier,         // Множитель только для пассивного дохода
    AllIncomeMultiplier     // Множитель ко всему доходу (tap + idle)
}
