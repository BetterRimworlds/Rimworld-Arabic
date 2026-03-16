# RimWorld Arabic (Arabic Translation Mod)
**by [Better Rimworlds](https://github.com/BetterRimworlds)** • powered by Autonomo AI

Bring a **full Arabic localization** to RimWorld — built to be **playable, UI-safe, and consistent** across the game’s terminology.

> ✅ Designed for real gameplay: stable placeholders, consistent RimWorld vernacular, and UI-friendly strings.

---

## What you get

- **Arabic translation** for RimWorld UI + game text
- **Consistent RimWorld terminology** (custom glossary / vernacular)
- **Placeholder-safe strings** (no broken `{0}`, `[PAWN_nameDef]`, etc.)
- **UI-safe length constraints** (labels/titles kept readable where possible)

---

## Translation Costs

```
================ ARABIC TRANSLATION ANALYSIS ================
Volume: 124,864 English words -> 139,813 Arabic words

--- LLM (ChatGPT 5.1 Equivalent) ---
Total API Calls           : 17,009
Total LLM Tokens In       : 4,896,299
Total LLM Tokens Out      : 336,601
LLM total cost            : $9.49
  ├─ Input cost           : $6.12
  └─ Output cost          : $3.37
Total runtime             : 8.67 hours

--- Human Translation Team (Dubai) ---
Project Lead Time         : 84.8 calendar days
Average Rate              : $35.00/hr

Role            | #  | Total Hrs  | Hrs/Person   | Cost
---------------------------------------------------------------------------
Translators     | 3  | 1,048.6    | 349.5        | 134,692.35 AED ($36,700.91)
Editors         | 1  | 258.7      | 258.7        | 33,224.11 AED ($9,052.89)
Proofreaders    | 1  | 90.9       | 90.9         | 11,673.34 AED ($3,180.75)
---------------------------------------------------------------------------
TOTAL BILLABLE HOURS: 1,398.1  | 179,589.80 AED ($48,934.55)

    [ VS SINGLE HUMAN ]
    Human Calendar Time   : 391.5 Days (279.6 work + 111.9 wknd)
    Autonomo Speedup      : 1083.2x FASTER

--- Human Translation Team (USA) ---
Project Lead Time         : 101.8 calendar days
Average Rate              : $75.00/hr

Role            | #  | Total Hrs  | Hrs/Person   | Cost
---------------------------------------------------------------------------
Translators     | 3  | 1,258.3    | 419.4        | $94,373.77
Editors         | 1  | 310.4      | 310.4        | $23,278.86
Proofreaders    | 1  | 109.1      | 109.1        | $8,179.06
---------------------------------------------------------------------------
TOTAL BILLABLE HOURS: 1,677.8  | $125,831.70

    [ VS SINGLE HUMAN ]
    Human Calendar Time   : 469.8 Days (335.6 work + 134.2 wknd)
    Autonomo Speedup      : 1299.9x FASTER
```


## Installation

### Option A: Steam Workshop (recommended)
1. Subscribe to the mod on Steam Workshop
2. Launch RimWorld
3. Go to **Mods** → enable **RimWorld Arabic**
4. Restart RimWorld when prompted

> If you don’t see it in your list, restart Steam and RimWorld.

*(Workshop link: add once published.)*

---

### Option B: Manual install (GitHub download)
1. Download this repository as a ZIP:
   - Click **Code** → **Download ZIP**
2. Extract it
3. Copy the folder into your RimWorld Mods directory:

**Windows**
```

C:\Program Files (x86)\Steam\steamapps\common\RimWorld\Mods\

```

**Linux**
```

~/.steam/steam/steamapps/common/RimWorld/Mods/

```

**macOS**
```

~/Library/Application Support/Steam/steamapps/common/RimWorld/RimWorldMac.app/Mods/

```

4. Make sure the folder structure looks like:
```

RimWorld/Mods/RimWorld-Arabic/
About/
Languages/
...

```

5. Launch RimWorld → **Mods** → enable **RimWorld Arabic** → restart.

---

## Enable Arabic in RimWorld

After the mod is enabled:

1. Go to **Options**
2. Find **Language**
3. Select **Arabic**
4. Restart RimWorld if asked

---

## Load order

Typically:
- **Core**
- DLCs (if any)
- Other mods
- **RimWorld Arabic**

If another mod includes its own translation files, it may override parts of the Arabic text depending on load order.

---

## Known behavior

- Some UI strings are deliberately kept short to avoid overflow.
- Some mod-added content may remain in English unless those mods ship Arabic translations or you add patches.
- If you use many mods, translation completeness depends on whether those mods provide keyed strings / translation keys.

---

## Troubleshooting

### “Arabic isn’t showing up in the language menu”
- Confirm the mod is **enabled**
- Confirm the folder path is correct:
  - `Mods/RimWorld-Arabic/Languages/Arabic/`
- Restart RimWorld after enabling the mod

### “Some text is still in English”
- That text likely comes from:
  - another mod (no Arabic translation available)
  - newly added RimWorld content that hasn’t been updated yet
- Please open an issue with:
  - a screenshot
  - the exact English text
  - your mod list + load order (if possible)

### “Text looks weird / missing characters”
- RimWorld font rendering is sensitive to:
  - font mods
  - UI scaling
- Try disabling font/UI mods to confirm compatibility.

---

## Bug reports & requests

Open a GitHub issue here:
- Include **screenshots**
- Include the **exact string** (English if possible)
- Include your **RimWorld version** and **mod list**

---

## Credits

Published by **[Better Rimworlds](https://github.com/BetterRimworlds)**
Built with the **Autonomo AI** localization pipeline (Automated QA Inspection & Copyediting).

---

## Disclaimer

RimWorld is the property of its respective owner(s).
This translation mod is an independent community project and is not affiliated with or endorsed by Ludeon Studios.
