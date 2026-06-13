using Nautilus.Json;
using Nautilus.Options;
using Nautilus.Options.Attributes;
using UnityEngine;

namespace MobileResourceScanner
{
    [Menu("Mobile Resource Scanner")]
    [ConfigFile("MobileResourceScanner")]
    public class ModConfig : ConfigFile
    {
        [Toggle("Enabled", Tooltip = "Enable this mod.")]
        public bool ModEnabled = true;

        [Toggle("Debug logging", Tooltip = "Write debug messages to the BepInEx log.")]
        public bool IsDebug = false;

        [Toggle("Require scanned", Tooltip = "Only show resources that have been scanned with the hand-held scanner.")]
        public bool RequireScanned = false;

        [Toggle("Always show all tech types", Tooltip = "List every tech type, even those not normally available to scanner rooms.")]
        public bool AlwaysShowAllTechTypes = false;

        [Slider("Range", 10f, 1000f, DefaultValue = 500f, Format = "{0:F0} m", Step = 10f, Tooltip = "Scanner range around the player's current position.")]
        public float Range = 500f;

        [Slider("Scan interval", 1f, 60f, DefaultValue = 10f, Format = "{0:F0} s", Step = 1f, Tooltip = "Seconds between scanner refreshes.")]
        [OnChange(nameof(OnIntervalChanged))]
        public float Interval = 10f;

        [Choice("Equipment menu button", "Left", "Right", "Middle", Tooltip = "Mouse button used when clicking the equipped chip to open the resource menu.")]
        public int MenuButton = 1;

        [Keybind("Open resource menu", Tooltip = "Key used to open the resource menu.")]
        public KeyCode MenuHotkey = KeyCode.L;

        [Toggle("Require shift for hotkey", Tooltip = "Keep the original Shift + L behavior while still allowing the key itself to be rebound.")]
        public bool RequireShiftForHotkey = true;

        [Toggle("Reset selected resource", Tooltip = "Clear the currently selected resource.")]
        [OnChange(nameof(OnResetResourceChanged))]
        public bool ResetSelectedResource = false;

        public string CurrentResource = "None";
        public string Ingredients = "ComputerChip:1,Magnetite:1";

        [Choice("Fabricator type", Tooltip = "Where the chip recipe is added. Restart required after changing.")]
        public CraftTree.Type FabricatorType = CraftTree.Type.MapRoom;

        public string NameString = "Mobile Resource Scanner";
        public string DescriptionString = "Equip to enable mobile resource scanning";
        public string MenuHeader = "Select Resource";
        public string OpenMenuString = "Switch Resource ({0})";

        private void OnIntervalChanged(SliderChangedEventArgs e)
        {
            BepInExPlugin.intervalChanged = true;
        }

        private void OnResetResourceChanged(ToggleChangedEventArgs e)
        {
            if (!e.Value)
                return;

            CurrentResource = TechType.None.ToString();
            ResetSelectedResource = false;
            Save();
            BepInExPlugin.SetCurrentResource(TechType.None);
        }
    }
}
