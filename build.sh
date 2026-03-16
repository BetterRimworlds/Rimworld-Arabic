#!/bin/bash
# ==== build.sh ====

set -e  # Stop on errors

MOD="Rimworld-Arabic"
SRC_DIR="./Source"
MOD_DIR="./${MOD}"
SOLUTION_PATH="${SRC_DIR}/${MOD}.sln"

RW16="/rimworld/1.6/Mods/${MOD}"
RW16_STEAM="/rimworld/1.6-steam/Mods/${MOD}"

CONFIG="Release v1.6"

# --- Dependency check ---
if ! command -v inotifywait >/dev/null 2>&1; then
    echo "❌ inotifywait not installed."
    echo "Install with: sudo pacman -S inotify-tools  (or your distro equivalent)"
    exit 1
fi

# --- Restore once ---
dotnet restore "$SOLUTION_PATH"

# --- Sync mod to RimWorld folders ---
function sync_mod() {
    echo "🔄 Syncing mod files..."

    mkdir -p "$RW16"
    mkdir -p "$RW16_STEAM"

    # Copy full mod structure (About/, Languages/, Assemblies/, etc.)
    rsync -a --delete "${MOD_DIR}/" "${RW16}/"
    rsync -a --delete "${RW16}/" "${RW16_STEAM}/"

    echo "✅ Mod synced to RimWorld"
}

# --- Build project ---
function build() {
    echo "🔨 Building ${CONFIG}..."

    dotnet build "$SOLUTION_PATH" --configuration "$CONFIG" --no-restore

    echo "📦 Build complete."

    # Verify DLL exists
    if ! find "${MOD_DIR}/Assemblies" -name "*.dll" | grep -q .; then
        echo "❌ ERROR: No DLL found in ${MOD_DIR}/Assemblies"
        echo "Check your .csproj OutputPath."
        exit 1
    fi

    sync_mod
}

# --- Initial build ---
build

# --- Exit if single-run mode ---
if [ "$1" == "1" ]; then
    echo "👋 Done (single-run mode)."
    exit 0
fi

# --- Watch for changes ---
echo "👀 Watching for changes..."

inotifywait --recursive --monitor \
  --format "%e %w%f" \
  --exclude '/\.idea($|/)' \
  --event modify,move,create,delete \
  "$SRC_DIR" "$MOD_DIR" |
while read event fullpath; do
    if [[ "$fullpath" == "$SRC_DIR"* && "$fullpath" == *.cs ]]; then
        echo "🧠 C# changed: $fullpath"
        build
    elif [[ "$fullpath" == "$MOD_DIR"* && "$fullpath" == *.xml ]]; then
        echo "🌍 XML changed: $fullpath"
        sync_mod
    fi
done

