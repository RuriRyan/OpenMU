// <copyright file="SpawnCommand.cs" company="MUnique">
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
// </copyright>

namespace MUnique.OpenMU.GameLogic.PlugIns.ChatCommands
{
    using System;
    using System.Runtime.InteropServices;
    using MUnique.OpenMU.DataModel.Configuration;
    using MUnique.OpenMU.DataModel.Entities;
    using MUnique.OpenMU.GameLogic.NPC;
    using MUnique.OpenMU.GameLogic.PlugIns.ChatCommands.Arguments;
    using MUnique.OpenMU.PlugIns;

    /// <summary>
    /// The spawn command spawns stuff.
    /// </summary>
    [Guid("CE5C959B-EBFE-44B5-AE5D-00DDF6A90633")]
    [PlugIn("Spawn command", "SpawnStuff.")]
    [ChatCommandHelp(Command, typeof(SpawnChatCommandArgs), CharacterStatus.GameMaster)]
    public class SpawnCommand : ChatCommandPlugInBase<SpawnChatCommandArgs>
    {
        private const string Command = "/spawn";

        /// <inheritdoc />
        public override string Key => Command;

        /// <inheritdoc />
        public override CharacterStatus MinCharacterStatusRequirement => CharacterStatus.GameMaster;

        /// <inheritdoc />
        protected override void DoHandleCommand(Player player, SpawnChatCommandArgs arguments)
        {
            if (arguments.MonsterName is null)
            {
                player.ShowMessage("ERROR: No Monster specified.");
                return;
            }

            var map = player.CurrentMap;
            if (map is null)
            {
                player.ShowMessage("ERROR: You have to be on a map. Try teleporting.");
                return;
            }

            MonsterDefinition? monsterDefinition = null;
            var searchName = arguments.MonsterName.Replace('_', ' ');
            foreach (var monster in player.GameContext.Configuration.Monsters)
            {
                if (monster.Designation.Equals(searchName, StringComparison.CurrentCultureIgnoreCase))
                {
                    monsterDefinition = monster;
                    break;
                }
            }

            if (monsterDefinition is null)
            {
                player.ShowMessage("ERROR: Monster not found.");
                return;
            }

            var context = player.GameContext.PersistenceContextProvider.CreateNewContext();
            var area = context.CreateNew<MonsterSpawnArea>();

            area.GameMap = map.Definition;
            area.MonsterDefinition = monsterDefinition;
            area.Quantity = (short)(arguments.Quantity > 0 ? arguments.Quantity : 1);
            area.SpawnTrigger = SpawnTrigger.Automatic;
            area.X1 = player.Position.X;
            area.Y1 = player.Position.Y;
            area.X2 = player.Position.X;
            area.Y2 = player.Position.Y;

            var npc = new Monster(area, monsterDefinition, map, new DefaultDropGenerator(player.GameContext.Configuration, Rand.GetRandomizer()), new BasicMonsterIntelligence(map), player.GameContext.PlugInManager);
            npc.Initialize();
            map.Add(npc);
            player.ShowMessage("Spawn successfully created.");
        }
    }
}