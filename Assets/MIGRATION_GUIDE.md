# Руководство по миграции на новую систему Space Farm Idle Clicker

## Что было сделано

✅ **Обновлён GameDesign.md** - полная документация всех ресурсов, апгрейдов, бустов и IAP

✅ **Создана система конфигов** - ScriptableObject для настройки всех элементов игры

✅ **Реализованы все 6 апгрейдов** из дизайна:
- Manual Harvest Protocol
- Micro Drone Swarm
- Hydroponic Racks
- Orbital Greenhouse
- Solar Mirror Array
- Terraforming AI

✅ **Создан магазин бустов** - 7 временных бустов за BioGel (Ion Pulse, Solar Focus и т.д.)

✅ **Улучшена система сохранений** - JSON вместо PlayerPrefs, поддержка оффлайн-дохода

✅ **Создана структура IAP** - базовая система для реальных покупок

✅ **Созданы UI компоненты** - готовые компоненты для списков апгрейдов, бустов и IAP

## Как начать использовать

### Шаг 1: Создайте конфиг

В Unity Editor:
1. Меню: **Space Farm -> Create Default Config**
2. Это создаст `Assets/Resources/IdleClickerConfig.asset`

### Шаг 2: Обновите сцену

**Вариант А: Использовать новый IdleClickerManager (рекомендуется)**

1. Найдите старый `IdleFarmManager` в сцене
2. Добавьте компонент `IdleClickerManager` на тот же объект (или создайте новый)
3. Назначьте созданный конфиг в поле `Config`
4. Назначьте UI элементы (BioGelText, PerSecondText и т.д.)
5. Старый `IdleFarmManager` можно удалить после проверки

**Вариант Б: Оставить старый IdleFarmManager**

Старый менеджер продолжит работать, но без новых функций (апгрейды, бусты, IAP).

### Шаг 3: Добавьте UI для апгрейдов (опционально)

1. Создайте ScrollView для списка апгрейдов
2. Создайте prefab кнопки апгрейда:
   - Добавьте компонент `UpgradeButtonView`
   - Настройте UI элементы (nameText, descriptionText, levelText, costText, buyButton)
3. Добавьте `UpgradeListView` на Content ScrollView
4. Назначьте prefab и Content Root

### Шаг 4: Добавьте магазин бустов (опционально)

Аналогично апгрейдам, используйте `ProgressBoostOfferListView` и `ProgressBoostOfferButtonView`.

### Шаг 5: Добавьте IAP магазин (опционально)

1. Создайте GameObject `IAP`
2. Добавьте `RealMoneyStoreController`
3. Добавьте `MockIapProvider` для тестирования
4. Создайте UI аналогично апгрейдам

## Структура файлов

```
Assets/
├── Scripts/
│   ├── Core/
│   │   ├── IdleClickerEngine.cs          # Ядро экономики
│   │   ├── IdleClickerManager.cs        # MonoBehaviour обёртка
│   │   ├── IdleSaveStorage.cs            # Сохранения JSON
│   │   └── NumberFormatter.cs            # Форматирование чисел
│   ├── Config/
│   │   ├── IdleClickerConfig.cs         # Главный конфиг
│   │   ├── UpgradeDefinition.cs         # Определение апгрейда
│   │   ├── ProgressBoostOfferDefinition.cs
│   │   └── RealMoneyProductDefinition.cs
│   ├── UI/
│   │   ├── UpgradeButtonView.cs
│   │   ├── UpgradeListView.cs
│   │   ├── ProgressBoostOfferButtonView.cs
│   │   ├── ProgressBoostOfferListView.cs
│   │   ├── RealMoneyProductButtonView.cs
│   │   └── RealMoneyProductListView.cs
│   ├── Monetization/
│   │   ├── IapProviderBase.cs
│   │   ├── MockIapProvider.cs
│   │   └── RealMoneyStoreController.cs
│   └── Editor/
│       └── CreateDefaultConfig.cs       # Меню создания конфига
├── Resources/
│   └── IdleClickerConfig.asset          # Создаётся через меню
└── GameDesign.md                        # Обновлённая документация
```

## Важные замечания

1. **Совместимость**: Старый `IdleFarmManager` использует PlayerPrefs, новый - JSON. Прогресс не переносится автоматически.

2. **Конфиг обязателен**: `IdleClickerManager` требует назначенный конфиг, иначе не будет работать.

3. **UI элементы**: Новый менеджер автоматически ищет UI элементы по имени (BioGelText, PerSecondText, ClickButton), но лучше назначать их вручную.

4. **Тестирование**: Используйте `MockIapProvider` для тестирования IAP без реальных покупок.

## Дальнейшее развитие

- Добавить Unity IAP провайдер для реальных покупок
- Добавить систему достижений
- Добавить ежедневные бонусы
- Добавить rewarded рекламу
- Улучшить визуализацию бустов (таймеры, эффекты)
