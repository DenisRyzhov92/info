# Space Farm Idle Clicker - Система

Полная система idle clicker для Space Farm с поддержкой апгрейдов, бустов, IAP и сохранений.

## Структура

### Core (Ядро)
- **IdleClickerEngine** - ядро экономики игры (чистая логика, без MonoBehaviour)
- **IdleClickerManager** - MonoBehaviour-обёртка для интеграции с Unity
- **IdleSaveStorage** - система сохранения/загрузки в JSON формате
- **NumberFormatter** - утилита для форматирования чисел (12.5K, 3.1M)

### Config (Конфигурация)
- **IdleClickerConfig** - ScriptableObject с настройками всех апгрейдов, бустов и IAP
- **UpgradeDefinition** - определение апгрейда
- **ProgressBoostOfferDefinition** - определение временного буста за BioGel
- **RealMoneyProductDefinition** - определение IAP продукта

### UI (Пользовательский интерфейс)
- **UpgradeButtonView** - компонент кнопки апгрейда
- **UpgradeListView** - список всех апгрейдов
- **ProgressBoostOfferButtonView** - компонент кнопки буста
- **ProgressBoostOfferListView** - список всех бустов
- **RealMoneyProductButtonView** - компонент кнопки IAP продукта
- **RealMoneyProductListView** - список всех IAP продуктов

### Monetization (Монетизация)
- **IapProviderBase** - базовый интерфейс для IAP провайдеров
- **MockIapProvider** - mock провайдер для тестирования
- **RealMoneyStoreController** - контроллер магазина реальных покупок

### Editor
- **CreateDefaultConfig** - меню для создания дефолтного конфига

## Быстрый старт

### 1. Создание конфига

В Unity: **Space Farm -> Create Default Config**

Это создаст `Assets/Resources/IdleClickerConfig.asset` со всеми апгрейдами и бустами из дизайна.

### 2. Настройка сцены

1. Создайте пустой GameObject `IdleClickerManager`
2. Добавьте компонент `IdleClickerManager`
3. Назначьте созданный конфиг в поле `Config`

### 3. Настройка UI

#### Основной HUD:
- Создайте TextMeshPro текст `BioGelText` для отображения BioGel
- Создайте TextMeshPro текст `PerSecondText` для отображения пассивного дохода
- Создайте Button `ClickButton` для тапа

#### Список апгрейдов:
1. Создайте ScrollView для апгрейдов
2. Создайте prefab кнопки апгрейда с компонентом `UpgradeButtonView`
3. Добавьте `UpgradeListView` на Content ScrollView
4. Назначьте prefab кнопки и Content Root

#### Магазин бустов:
1. Создайте ScrollView для бустов
2. Создайте prefab кнопки буста с компонентом `ProgressBoostOfferButtonView`
3. Добавьте `ProgressBoostOfferListView` на Content ScrollView
4. Назначьте prefab кнопки и Content Root

#### IAP магазин:
1. Создайте GameObject `IAP` с компонентом `RealMoneyStoreController`
2. Добавьте `MockIapProvider` для тестирования (или `UnityIapProvider` для реальных покупок)
3. Создайте ScrollView для IAP продуктов
4. Создайте prefab кнопки продукта с компонентом `RealMoneyProductButtonView`
5. Добавьте `RealMoneyProductListView` на Content ScrollView

### 4. Миграция со старого IdleFarmManager

Старый `IdleFarmManager.cs` можно удалить или переименовать. Новый `IdleClickerManager` полностью заменяет его функциональность.

## API для использования из кода

```csharp
// Получить менеджер
IdleClickerManager manager = IdleClickerManager.Instance;

// Тап
manager.OnTapField();

// Покупка апгрейда
manager.BuyUpgrade("manual_harvest");

// Покупка буста
manager.BuyBoost("ion_pulse");

// Получить текущий BioGel
long bioGel = manager.GetBioGel();

// Получить уровень апгрейда
int level = manager.GetUpgradeLevel("manual_harvest");

// Проверить разблокировку
bool unlocked = manager.IsUpgradeUnlocked("drone_swarm");
```

## Сохранения

Сохранения автоматически выполняются:
- При изменении ресурсов
- При покупке апгрейдов/бустов
- При паузе/выходе из фокуса приложения

Файл сохранения: `Application.persistentDataPath/idle_clicker_save.json`

Оффлайн-доход рассчитывается автоматически при загрузке (максимум 24 часа, 50% от онлайн-дохода).

## Кастомизация

Все настройки игры находятся в `IdleClickerConfig`:
- Стартовые параметры
- Список апгрейдов (можно добавлять/редактировать в Inspector)
- Список бустов
- Список IAP продуктов

Для добавления нового апгрейда:
1. Откройте `IdleClickerConfig` в Inspector
2. Добавьте новый элемент в список `Upgrades`
3. Заполните все поля

Аналогично для бустов и IAP продуктов.
