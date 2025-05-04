# Cross-Platform System Resource Monitor

This is a cross-platform console application built with **.NET 8.0** that monitors system resources (CPU, RAM, and Disk usage) in real-time and supports a plugin-based architecture for extensibility.

---

## 🚀 Features

- Monitors:
  - CPU usage (%)
  - RAM usage (used/total in MB)
  - Disk usage (used/total in MB)
- Console output of system metrics at configurable intervals
- **Plugin architecture** for sending metrics to:
  - A REST API endpoint
  - Local file logging (provided sample)
- Clean Architecture using Dependency Injection and interface segregation
- Cross-platform support: Windows, Linux, macOS (Windows fully implemented)

---

## 📦 Requirements

- [.NET 8.0 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/8.0)
- Visual Studio 2022 (17.8 or later) with .NET 8 support installed
- Git (optional, for cloning)

---

## 🛠️ How to Build and Run the Project

### 🖥️ Option 1: Using Visual Studio 2022

1. **Open the Solution:**
   - Open `RishiProject.sln` in Visual Studio 2022.

2. **Restore Dependencies:**
   - VS will automatically restore NuGet packages. If not, go to:
     - `Tools` → `NuGet Package Manager` → `Manage NuGet Packages for Solution` → Restore.

3. **Set Startup Project:**
   - Right-click on the `RishiProject` project → `Set as Startup Project`.

     ```

4. **Build & Run:**
   - Press `Ctrl+F5` or click the **Run** button to start without debugging.
   - The console will display real-time CPU, RAM, and disk usage.
   - Plugins will execute with each update.

---

### 🔧 Option 2: Using Command Line

1. **Navigate to Root Directory:**
   ```bash
   cd path/to/RishiProject

1. **Build the Project:**
   ```bash
   dotnet build

1. **Run the Application:**
   ```bash
   dotnet run --project RishiProject

