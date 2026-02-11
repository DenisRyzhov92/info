# Space Farm Idle Clicker - Дизайн игры

## Концепция

**Space Farm** — idle clicker игра в сеттинге космофермы. Игрок управляет фермой в космосе, собирает BioGel и развивает производство от мини-теплицы до терраформинга.

## Основной ресурс

- **BioGel** — основной ресурс игры, используется для всех покупок и апгрейдов.
- **Lifetime BioGel** — общее количество BioGel, когда-либо собранное игроком (для unlock-условий)

## Механики добычи

### Тап (Tap)
- Ручной сбор BioGel в куполе
- Каждый тап даёт `bioGelPerTap` единиц BioGel
- Можно улучшать через апгрейд "Manual Harvest Protocol"

### Idle (Пассивный доход)
- Автоматическая добыча дронами
- Даёт `bioGelPerSecond` единиц BioGel каждую секунду
- Можно улучшать через апгрейды дронов

## Цепочка апгрейдов

Каждый апгрейд имеет:
- **ID** — уникальный идентификатор
- **Название** — отображаемое имя
- **Описание** — текст для UI
- **Базовая стоимость** — стоимость первого уровня
- **Множитель стоимости** — во сколько раз растёт цена за уровень
- **Тип бонуса** — что улучшает (tap, idle, multiplier)
- **Значение бонуса** — на сколько увеличивает
- **Unlock условие** — минимальный lifetime BioGel для разблокировки

### Список апгрейдов:

1. **Manual Harvest Protocol** 
   - Тип: Tap Boost
   - Эффект: +1 BioGel за тап за уровень
   - Unlock: 0 (доступен с начала)
   - Базовая стоимость: 10 BioGel

2. **Micro Drone Swarm**
   - Тип: Idle Income
   - Эффект: +0.5 BioGel/s за уровень
   - Unlock: 100 lifetime BioGel
   - Базовая стоимость: 50 BioGel

3. **Hydroponic Racks**
   - Тип: Idle Income
   - Эффект: +2 BioGel/s за уровень
   - Unlock: 1K lifetime BioGel
   - Базовая стоимость: 200 BioGel

4. **Orbital Greenhouse**
   - Тип: Idle Income
   - Эффект: +10 BioGel/s за уровень
   - Unlock: 10K lifetime BioGel
   - Базовая стоимость: 1K BioGel

5. **Solar Mirror Array**
   - Тип: Income Multiplier
   - Эффект: x1.5 ко всему доходу за уровень
   - Unlock: 100K lifetime BioGel
   - Базовая стоимость: 10K BioGel

6. **Terraforming AI**
   - Тип: Idle Income
   - Эффект: +100 BioGel/s за уровень
   - Unlock: 1M lifetime BioGel
   - Базовая стоимость: 100K BioGel

## Магазин бустов за BioGel (Progress Boost Shop)

Временные ускорители прогресса. Каждый буст имеет:
- **ID** — уникальный идентификатор
- **Название** — отображаемое имя
- **Описание** — текст для UI
- **Стоимость** — цена в BioGel
- **Длительность** — сколько секунд действует
- **Эффект** — какой бонус даёт (x2 доход, x3 тап и т.д.)
- **Unlock условие** — минимальный lifetime BioGel

### Early Game (soft-launch)

- **Ion Pulse**
  - Эффект: x2 пассивный доход
  - Длительность: 60 секунд
  - Стоимость: 100 BioGel
  - Unlock: 500 lifetime BioGel

- **Solar Focus**
  - Эффект: x3 тап
  - Длительность: 30 секунд
  - Стоимость: 50 BioGel
  - Unlock: 200 lifetime BioGel

- **Drone Overclock**
  - Эффект: x2 пассивный доход
  - Длительность: 120 секунд
  - Стоимость: 200 BioGel
  - Unlock: 1K lifetime BioGel

### Mid Game

- **Orbital Sync**
  - Эффект: x1.5 ко всему доходу
  - Длительность: 300 секунд
  - Стоимость: 500 BioGel
  - Unlock: 10K lifetime BioGel

- **Bioreactor Surge**
  - Эффект: x3 пассивный доход
  - Длительность: 180 секунд
  - Стоимость: 1K BioGel
  - Unlock: 50K lifetime BioGel

### Late Game

- **Plasma Wave**
  - Эффект: x5 ко всему доходу
  - Длительность: 60 секунд
  - Стоимость: 5K BioGel
  - Unlock: 500K lifetime BioGel

- **Terraform Rush**
  - Эффект: x10 ко всему доходу
  - Длительность: 120 секунд
  - Стоимость: 20K BioGel
  - Unlock: 2M lifetime BioGel

## Real Money Products (IAP)

Полезные наборы за реальные деньги. Каждый продукт имеет:
- **Product ID** — идентификатор для магазина (Google Play / App Store)
- **Название** — отображаемое имя
- **Описание** — текст для UI
- **Цена** — реальная цена (устанавливается в магазине)
- **Награды** — что получает игрок

### Продукты:

- **Starter Supply Drop**
  - Product ID: `starter_supply_drop`
  - Награды: 1K BioGel + 1x Ion Pulse + 1x Solar Focus
  - Описание: "Начальный набор для быстрого старта"

- **Terraform Booster**
  - Product ID: `terraform_booster`
  - Награды: 50K BioGel + 2x Orbital Sync + 1x Bioreactor Surge
  - Описание: "Большой набор ресурсов для ускорения прогресса"

- **Colony Expansion Bundle**
  - Product ID: `colony_expansion_bundle`
  - Награды: 500K BioGel + 3x Plasma Wave + 1x Terraform Rush
  - Описание: "Максимальный набор для быстрого развития"

## Retention механики

- **Ежедневный бонус** — за вход в игру каждый день
- **Достижения** — 3-5 базовых достижений (первый тап, первый апгрейд, первый миллион и т.д.)
- **Миссии на сессию** — ежедневные задания (собери X BioGel, купи Y апгрейдов)

## Монетизация

### Rewarded Ads (только по запросу)
- **Политика**: Автоматической рекламы нет
- **Interstitial/App Open**: Отключены
- **Rewarded only**: Показывается только после нажатия кнопки "Watch ad"
- **Награды**: 
  - x2 добыча на 30 минут (временный буст)
  - Мгновенная награда BioGel (зависит от текущего дохода)

### IAP (In-App Purchases)
- Покупки выполняются только по нажатию игрока на кнопку продукта
- Поддержка Google Play Billing и Apple App Store
- Mock провайдер для локального тестирования

## Сохранения

### Формат сохранения (JSON)
Файл: `idle_clicker_save.json`
Путь: `Application.persistentDataPath`

### Сохраняемые данные:
- **BioGel** — текущее количество BioGel
- **Lifetime BioGel** — общее количество когда-либо собранного BioGel
- **Уровни апгрейдов** — словарь (upgradeId -> level)
- **Активные бусты** — список активных временных бустов с временем окончания
- **Время последнего сохранения** — Unix timestamp для расчёта оффлайн-дохода

### Оффлайн-доход
- Начисляется при возвращении в игру
- Рассчитывается на основе времени отсутствия (максимум 24 часа)
- Формула: `offlineSeconds * bioGelPerSecond * 0.5` (50% от онлайн-дохода)
