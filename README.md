# AI_2D_hollow_knight

> 空洞騎士風格的 2D 銀河城（Metroidvania）遊戲 DEMO — Unity C# 基礎框架

---

## 專案簡介

本專案是一個以 **空洞騎士（Hollow Knight）** 為靈感的 2D 銀河城遊戲 Demo，使用 **Unity 引擎（URP 2D）** 搭建。  
框架採用多種軟體設計模式，確保程式碼的**可讀性**、**延展性**和**可維護性**，方便未來持續擴充技能、敵人、Boss 和關卡。

---

## 技術架構

### 設計模式

| 模式 | 用途 | 相關檔案 |
|------|------|----------|
| **階層式狀態機 (Hierarchical FSM)** | 玩家與敵人的行為控制 | `StateMachine.cs`, `SubStateMachine.cs`, `Player/States/`, `Enemies/States/` |
| **泛型單例 (Generic Singleton)** | 全域管理器（GameManager、AudioManager） | `Singleton.cs`, `GameManager.cs`, `AudioManager.cs` |
| **觀察者模式 (ScriptableObject Event Channel)** | 解耦模組間的事件溝通 | `VoidEventChannel.cs`, `IntEventChannel.cs`, `VoidEventListener.cs` |
| **策略模式** | Boss 攻擊招式的切換 | `MiniBoss/States/` |
| **數據驅動設計** | 用 ScriptableObject 存放數值，Inspector 直接調參 | `PlayerData.cs`, `EnemyData.cs`, `BossData.cs` |

---

## 資料夾結構

```
Assets/
└── _Project/
    ├── Scenes/             # 遊戲場景
    ├── Scripts/
    │   ├── Core/           # 核心框架（狀態機、單例、事件系統）
    │   │   ├── StateMachine/
    │   │   │   ├── IState.cs
    │   │   │   ├── StateMachine.cs
    │   │   │   └── SubStateMachine.cs
    │   │   ├── EventChannel/
    │   │   │   ├── VoidEventChannel.cs
    │   │   │   ├── IntEventChannel.cs
    │   │   │   └── VoidEventListener.cs
    │   │   ├── Singleton.cs
    │   │   └── GameManager.cs
    │   ├── Player/         # 玩家系統
    │   │   ├── PlayerController.cs   # 主控制器（輸入、物理偵測）
    │   │   ├── PlayerCombat.cs       # 戰鬥系統（四方向攻擊、Pogo）
    │   │   ├── PlayerHealth.cs       # 血量系統（無敵幀、擊退）
    │   │   ├── PlayerAnimator.cs     # 動畫橋接器
    │   │   ├── PlayerData.cs         # ScriptableObject 數值配置
    │   │   └── States/               # 玩家狀態（共 10 個）
    │   ├── Enemies/        # 敵人系統
    │   │   ├── EnemyBase.cs
    │   │   ├── EnemyData.cs
    │   │   └── States/               # 敵人狀態（共 4 個）
    │   ├── Bosses/         # Boss 系統
    │   │   ├── BossBase.cs           # Boss 基底（階段切換）
    │   │   ├── BossData.cs           # ScriptableObject Boss 數值
    │   │   ├── BossArenaManager.cs   # 競技場管理（觸發 Boss 戰）
    │   │   └── MiniBoss/             # 迷你 Boss + 6 個狀態
    │   ├── Systems/        # 遊戲子系統
    │   │   ├── HealthSystem.cs       # IDamageable 介面定義
    │   │   ├── CameraSystem.cs       # 相機跟隨 + 螢幕震動
    │   │   ├── HitStop.cs            # 畫面凍結（打擊感）
    │   │   ├── ParallaxBackground.cs # 視差背景
    │   │   └── AudioManager.cs       # 音效管理
    │   └── UI/             # 使用者介面
    │       ├── HealthBarUI.cs        # 玩家血量顯示
    │       └── BossHealthBarUI.cs    # Boss 血量顯示
    ├── Art/                # 美術資源（Sprites, Tilesets, Animations）
    ├── Audio/              # 音效資源（SFX, Music）
    ├── Prefabs/            # Prefab（Player, Enemies, Bosses, Effects）
    └── Resources/          # 運行時載入資源（Data, EventChannels）
```

---

## 如何開啟專案

1. 安裝 **Unity Hub** 與 **Unity 6** 或更新版本
2. 在 Unity Hub 中點選 **「Open」**，選擇本專案資料夾
3. 確認 **Universal Render Pipeline (URP 2D)** 套件已安裝（Package Manager）
4. 在 `Assets/_Project/Scenes/` 中開啟主場景
5. 按下 **Play** 即可執行

---

## 操作說明

| 按鍵 | 動作 |
|------|------|
| `A / D` 或 `← / →` | 左右移動 |
| `W / S` 或 `↑ / ↓` | 選擇攻擊方向（上/下） |
| `Space` | 跳躍 / 二段跳 |
| `Space`（貼牆時） | 牆跳 |
| `J` | 攻擊（四方向） |
| `K` | 衝刺 |
| `Esc` | 暫停 |

> **Pogo 技巧**：在空中向下攻擊（S + J）命中敵人時，玩家會被彈起。

---

## 延展指南

### 新增一個玩家技能（狀態）

1. 在 `Assets/_Project/Scripts/Player/States/` 建立新的 `PlayerXxxState.cs`，繼承 `PlayerBaseState`
2. 實作 `Enter()`、`LogicUpdate()`、`PhysicsUpdate()`、`Exit()` 方法
3. 在 `PlayerController.Awake()` 中建立此狀態的實例
4. 在相關狀態的 `CheckTransitions()` 中加入觸發此技能的條件

```csharp
// 範例：新增空中衝刺狀態
public class PlayerAirDashState : PlayerBaseState
{
    public PlayerAirDashState(PlayerController c, StateMachine sm, PlayerData d) : base(c, sm, d) {}
    public override void Enter() { /* 設定速度、動畫 */ }
    protected override void CheckTransitions() { /* 結束條件 */ }
}
```

### 新增一種敵人

1. 建立新 `EnemyData` ScriptableObject（右鍵 > Create > Data > Enemy Data）
2. 在 `Assets/_Project/Scripts/Enemies/` 建立新類別繼承 `EnemyBase`
3. 在 `Awake()` 中覆寫狀態機初始化，加入特有狀態
4. 設定 Prefab 並拖入場景

### 新增一個 Boss

1. 建立新 `BossData` ScriptableObject
2. 建立新類別繼承 `BossBase`，在 `Awake()` 中初始化專屬狀態機
3. 在 `Assets/_Project/Scripts/Bosses/` 資料夾下建立 Boss 資料夾與狀態
4. 使用 `BossArenaManager` 設定競技場觸發器

---

## 授權

本框架為學習與展示用途，歡迎自由使用與修改。
