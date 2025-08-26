<h1 align="center">
  CS2 WarnSystem
</h1>

<p align="center">
<i>Loved the tool? Please consider <a href="https://paypal.com/paypalme/playpointsk">donating</a> 💸 to help it improve!</i>
</p>

<p align="center">
<a href="https://www.paypal.com/paypalme/playpointsk"><img src="https://img.shields.io/badge/support-PayPal-blue?logo=PayPal&style=flat-square&label=Donate"/>
</a>
</p>

---

## 📜 About the Plugin

A **Counter-Strike 2 plugin** for **CounterStrikeSharp** that allows admins to **warn players**.  
All warnings are stored in **MySQL** (via **Dapper + MySqlConnector**) and when a player reaches  
the configured threshold (default: **3 warns**), a configurable punishment command (kick/ban/gag/etc.)  
is automatically executed.

---

## 🔹 Features

- Persistent warning storage in **MySQL**
- **Two database tables:**
    - `warn_system_warns` – aggregated statistics (total/active warns, penalties, last warn)
    - `warn_system_logs` – detailed log of every warn (admin, target, timestamp)
- **Configurable punishment command** with placeholders (`{steamid64}`, `{userid}`, `{username}`, `{warns}`, `{minutes}`, `{totalpenalties}`)
- **Dynamic penalty scaling** (e.g., 1st = 60 min, 2nd = 120 min…) – can be toggled in config
- **Active warns reset option** after punishment
- **Admin-only warn menu** integrated with MenuManagerCS2
- Multi-language localization (default `sk.json`)
- Colored chat messages with placeholders
- Warn any player via a simple menu command
- Store **total warns, active warns, penalties, last warn date** in DB
- Automatic punishment once threshold is reached
- Configurable reset of active warns after punishment
- Localization support (easy to add more languages)
- Integration with **MenuManagerCS2**
- MySQL schema auto-created on first run

---

## 🛠 Installation

**Requirements**
- [CounterStrikeSharp](https://github.com/roflmuffin/CounterStrikeSharp)
- [MenuManagerCS2](https://github.com/NickFox007/MenuManagerCS2)
- MySQL database (with user & password)

**Steps**
1. Download the latest release and upload it to the addons folder.
2. Start or restart the server.

---

## ⚙️ Configuration

Config is generated on first run:

```json
{
  "Language": "en",
  "ChatPrefix": " {lightred}[WARN]",
  "WarnCommand": "warn",
  "AdminPermission": "@css/ban",
  "Database": {
    "Server": "127.0.0.1",
    "Port": 3306,
    "Database": "database_name",
    "User": "database_user",
    "Password": "database_password",
    "SslMode": "None"
  },
  "WarnThreshold": 3,
  "PenaltyScalingEnabled": true,
  "PenaltyBaseMinutes": 60,
  "PenaltyCommand": "css_gag #{steamid64} {minutes} 3X-WARN",
  "ResetActiveWarnsAfterPenalty": true
}
```

### 🔧 Options
- **Language** – language file used from `lang/` folder (default `en`)
- **ChatPrefix** – prefix with colors for chat messages
- **WarnCommand** – command to open the warn menu
- **AdminPermission** – required permission for using the command
- **Database** – connection settings for MySQL
- **WarnThreshold** – how many active warns trigger punishment
- **PenaltyScalingEnabled** – enable/disable dynamic scaling of penalty duration (true/false)
- **PenaltyBaseMinutes** – base time in minutes (e.g., 60 → first penalty 60 min, second penalty 120 min…)
- **PenaltyCommand** – command executed once threshold is reached  
  (placeholders: `{steamid64}`, `{userid}`, `{username}`, `{warns}`, `{minutes}`, `{totalpenalties}`)
- **ResetActiveWarnsAfterPenalty** – reset active warns after punishment (true/false)

---

## 🎨 Chat Colors

You can use the following color tags inside messages and prefixes:

- `{default}`
- `{white}`
- `{darkred}`
- `{green}`
- `{lightyellow}`
- `{lightblue}`
- `{olive}`
- `{lime}`
- `{red}`
- `{lightpurple}`
- `{purple}`
- `{grey}` or `{gray}`
- `{yellow}`
- `{gold}`
- `{silver}`
- `{blue}`
- `{darkblue}`
- `{bluegrey}`
- `{magenta}`
- `{lightred}`
- `{orange}`

---

## 📩 Contact
- **Discord:** `tichotm`
