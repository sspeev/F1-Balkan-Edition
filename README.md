# 🏎️ F1 Balkan Edition

[![Unity 6](https://img.shields.io/badge/Made%20with-Unity%206-black.svg?style=flat-square&logo=unity)](https://unity.com/)
[![Language C#](https://img.shields.io/badge/Language-C%23-blue.svg?style=flat-square&logo=c-sharp)](https://docs.microsoft.com/en-us/dotnet/csharp/)
[![Emotiv Insight 2](https://img.shields.io/badge/Headset-Emotiv%20Insight%202-red.svg?style=flat-square)](https://www.emotiv.com/insight/)
[![Riptide Networking](https://img.shields.io/badge/Network-Riptide%20Networking-orange.svg?style=flat-square)](https://github.com/RiptideNetworking/Riptide)

**F1 Balkan Edition** is a high-speed Formula 1 racing game built on **Unity 6** that bridges real-world neuroscience with virtual motorsport. By utilizing the **Emotiv Insight 2** EEG headset, players can control the steering and acceleration of their vehicle in real-time using mental commands and motion tracking, offering a truly hands-free driving experience.

---

## 🚀 Key Features

* **🧠 Neuro-Controls (EEG Integration):** Drive with your mind! Uses the Emotiv Unity SDK to read motion data (gyro/accel) and mental commands for steering, accelerating, and braking.
* **🏎️ Physics-Based Vehicle Mechanics:** Built with Unity's WheelCollider physics, including dynamic DRS (Drag Reduction System) speed boosts and realistic gravel drag deceleration.
* **🏁 Showroom & Car Selection:** Custom-decimated high-performance vehicle models (including Corvette, Porsche, Mustang, and F1 Audi) fully optimized for optimal frame rates.
* **📊 Global Leaderboard integration:** Automatically sends lap records to a local REST API and displays them dynamically in the menus.
* **🌐 Multiplayer Foundation:** Pre-configured with the Riptide UDP Networking Engine for high-speed client-server synchronization.

---

## 🛠️ Tech Stack & Dependencies

* **Engine:** Unity 6 (`6000.4.9f1`)
* **Input System:** Unity Input System (New) & Mobile Joystick Pack
* **Hardware Plugin:** Emotiv Unity SDK
* **Networking:** Riptide Networking DLL
* **Database & Serialization:** Newtonsoft.Json & Leguar TotalJSON

---

## 📦 Project Directory Structure

```text
├── Assets/
│   ├── Blender Models/      # Optimized FBX car meshes & prefabs
│   ├── Joystick Pack/       # On-screen UI controls for mobile platforms
│   ├── MultiplayerEngine/   # Network manager and Riptide networking scripts
│   ├── Scenes/              # GameWelcome, Menu, ShowRoom, GameScene, Tutorial
│   ├── Scripts/             # Core C# controllers (Physics, Audio, Input, UI)
│   └── unity-plugin/        # Emotiv SDK integration
├── Packages/                # Unity Package Manifests
├── ProjectSettings/         # Unity Project settings & configurations
├── SourceFiles/             # IGNORED raw artwork, .blend files, and .pdn paint files
└── README.md                # Project documentation
```

---

## 🚦 Getting Started

### Prerequisites
1. **Unity Hub** & **Unity 6 (6000.4.9f1 or newer)** installed.
2. (Optional) **Emotiv Cortex Service** installed on your machine and a configured **Emotiv Insight 2** headset.
3. (Optional) A local ASP.NET/Express database backend listening on ports `7008`/`5257` for the leaderboard.

### Installation
1. Clone the repository:
   ```bash
   git clone https://github.com/sspeev/F1-Balkan-Edition.git
   cd F1-Balkan-Edition
   ```
2. Open **Unity Hub**, click **Add**, and select the project root folder.
3. Unity will automatically resolve and download the package dependencies.

---

## 🎮 How to Play

### Control Modes
Toggle **Brain Controls** inside the Options menu:

#### ⌨️ Standard Keyboard Controls (Brain Controls OFF)
* **Accelerate:** `W` or Up Arrow
* **Steer:** `A` / `D` or Left / Right Arrows
* **Brake:** `S` or Down Arrow
* **Toggle Track Lights:** `L`
* **Change Sun Position (Day/Night):** `N` (Night) / `M` (Day)

#### 🧠 Brainwave Headset Controls (Brain Controls ON)
* Connect your Emotiv Insight 2 headset.
* The steering is linked directly to the headset's Y/Z motion channels (tilting your head left/right).

---

## ⚙️ Optimization & Git Guidelines

To prevent your GitHub repository from breaking or hitting file size limits:
1. **Never ignore `.meta` files:** Unity metadata must be pushed to GitHub to preserve scene references.
2. **FBX Conversion:** All raw `.blend` and `.pdn` files should remain in the root **`SourceFiles/`** folder (which is ignored by Git). Only stage and push the compressed `.fbx` and `.png`/`.jpg` textures inside the `Assets/` directory.
3. **Keep File Sizes Under 100MB:** Keep textures compressed (2K or 4K PNG) and decimate meshes in Blender to ensure no asset exceeds 100 MB.
