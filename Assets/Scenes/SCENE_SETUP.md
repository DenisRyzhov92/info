# Настройка сцен Space Farm

## Структура сцен

Игра состоит из двух основных сцен:

1. **FarmScene** — сцена космофермы (сбор BioGel)
2. **TerraformScene** — сцена терраформирования новой планеты-базы

## Сцена 1: FarmScene (Космоферма)

### Объекты в сцене:

1. **GameManager** (пустой объект)
   - Компонент: `IdleFarmManager`
   - Этот объект будет сохраняться между сценами (DontDestroyOnLoad)

2. **SceneManager** (пустой объект)
   - Компонент: `SpaceFarmSceneManager`
   - Поле `Farm Scene Name`: `FarmScene`
   - Поле `Terraform Scene Name`: `TerraformScene`

3. **Canvas** (UI)
   - **BioGelText** (TextMeshPro) — отображение количества BioGel
   - **PerSecondText** (TextMeshPro) — отображение пассивного дохода
   - **TapUpgradeCostText** (TextMeshPro) — цена апгрейда тапа
   - **ClickButton** (Button) — кнопка тапа для сбора BioGel
   - **UpgradeButton** (Button) — кнопка покупки апгрейда тапа
   - **TerraformButton** (Button) — кнопка перехода на сцену терраформирования

### Фон сцены (опционально):
- Можно добавить 3D модель космофермы или 2D спрайт фона
- Или оставить простой цветной фон

---

## Сцена 2: TerraformScene (Терраформирование)

### Объекты в сцене:

1. **GameManager** (пустой объект)
   - Компонент: `IdleFarmManager` (тот же, что и в FarmScene)
   - Если GameManager уже существует (DontDestroyOnLoad), он автоматически будет использован

2. **SceneManager** (пустой объект)
   - Компонент: `SpaceFarmSceneManager`
   - Поле `Farm Scene Name`: `FarmScene`
   - Поле `Terraform Scene Name`: `TerraformScene`

3. **Canvas** (UI)
   - **BioGelText** (TextMeshPro) — отображение количества BioGel (общее для всех сцен)
   - **PerSecondText** (TextMeshPro) — отображение пассивного дохода
   - **FarmButton** (Button) — кнопка возврата на ферму
   - Дополнительные UI элементы для терраформирования (будут добавлены позже)

### Фон сцены (опционально):
- Можно добавить 3D модель планеты или 2D спрайт фона терраформирования

---

## Как создать сцены:

### Создание FarmScene:

1. В Unity: `File → New Scene` → выбери `Basic (Built-in)` или `Empty`
2. Сохрани как `FarmScene` в папку `Assets/Scenes`
3. Создай объекты по списку выше
4. Настрой UI элементы с правильными именами

### Создание TerraformScene:

1. В Unity: `File → New Scene` → выбери `Basic (Built-in)` или `Empty`
2. Сохрани как `TerraformScene` в папку `Assets/Scenes`
3. Создай объекты по списку выше
4. Настрой UI элементы с правильными именами

### Важно:

- **GameManager с IdleFarmManager** должен быть создан только в одной сцене (например, в FarmScene)
- При переходе между сценами он сохранится благодаря `DontDestroyOnLoad`
- BioGel и прогресс сохраняются между сценами автоматически

---

## Переименование текущей сцены:

Если хочешь переименовать `CosmoFarmMain` в `FarmScene`:

1. В `Project` нажми правой кнопкой на `CosmoFarmMain.unity`
2. Выбери `Rename`
3. Переименуй в `FarmScene.unity`
4. Unity автоматически обновит все ссылки
