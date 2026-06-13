using Nautilus.Assets;
using Nautilus.Assets.Gadgets;
using Nautilus.Assets.PrefabTemplates;
using Nautilus.Crafting;
using Nautilus.Extensions;
using Nautilus.Handlers;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace MobileResourceScanner.Items.Equipment
{

    public static class ScannerPrefab
    {
        public static List<Ingredient> ingredientList = new List<Ingredient>();

        public static PrefabInfo Info { get; } = PrefabInfo
            .WithTechType(BepInExPlugin.idString, BepInExPlugin.config.NameString, BepInExPlugin.config.DescriptionString)
            .WithIcon(SpriteManager.Get(TechType.MapRoomHUDChip));

        public static void Register()
        {
            var customPrefab = new CustomPrefab(Info);

            var scannerObj = new CloneTemplate(Info, TechType.MapRoomHUDChip);
            customPrefab.SetGameObject(scannerObj);

            ingredientList.Clear();
            foreach (var str in BepInExPlugin.config.Ingredients.Split(','))
            {
                var split = str.Split(':');
                if (split.Length != 2)
                {
                    BepInExPlugin.Dbgl($"Invalid ingredient entry '{str}', expected TechType:Amount", BepInEx.Logging.LogLevel.Warning);
                    continue;
                }
                if (!int.TryParse(split[1], out var amount))
                {
                    BepInExPlugin.Dbgl($"Invalid ingredient amount '{split[1]}' in entry '{str}'", BepInEx.Logging.LogLevel.Warning);
                    continue;
                }
                if (!Enum.TryParse<TechType>(split[0], out var tech))
                {
                    BepInExPlugin.Dbgl($"Invalid ingredient TechType '{split[0]}' in entry '{str}'", BepInEx.Logging.LogLevel.Warning);
                    continue;
                }
                ingredientList.Add(new Ingredient(tech, amount));
            }

            customPrefab.SetRecipe(new RecipeData(ingredientList))
                .WithFabricatorType(BepInExPlugin.config.FabricatorType);
            customPrefab.SetEquipment(EquipmentType.Chip);
            customPrefab.Register();
            KnownTechHandler.UnlockOnStart(customPrefab.Info.TechType);
        }
    }

}
