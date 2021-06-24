// <copyright file="HelpCommand.cs" company="MUnique">
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
// </copyright>

namespace MUnique.OpenMU.GameLogic.PlugIns.ChatCommands
{
    using System;
    using System.Runtime.InteropServices;
    using MUnique.OpenMU.DataModel.Configuration;
    using MUnique.OpenMU.DataModel.Entities;
    using MUnique.OpenMU.PlugIns;

    /// <summary>
    /// The help command which shows the usage of a command.
    /// </summary>
    [Guid("CE5C959B-EBFE-44B5-AE5D-00DDF6A90633")]
    [PlugIn("Spawn command", "SpawnStuff.")]
    [ChatCommandHelp(Command, typeof(Arguments))]
    public class SpawnCommand : IChatCommandPlugIn
    {
        private const string Command = "/spawn";

        /// <inheritdoc />
        public string Key => Command;

        /// <inheritdoc />
        public CharacterStatus MinCharacterStatusRequirement => CharacterStatus.Normal;

        /// <inheritdoc />
        public void HandleCommand(Player player, string command)
        {
            var area = player.PersistenceContext.CreateNew<MonsterSpawnArea>();
            MonsterDefinition monsterDefinition = null;
            foreach (var monster in player.GameContext.Configuration.Monsters)
            {
                if (monster.Designation == "Spider")
                {
                    monsterDefinition = monster;
                    return;
                }
            }

            if (monsterDefinition is null)
            {
                return;
            }

            area.GameMap = player.CurrentMap?.Definition;
            area.MonsterDefinition = monsterDefinition;
            area.Quantity = 1;
            area.SpawnTrigger = SpawnTrigger.Automatic;
            area.X1 = player.Position.X;
            area.Y1 = player.Position.Y;
            area.X2 = player.Position.X;
            area.Y2 = player.Position.Y;

            player.CurrentMap?.Definition.MonsterSpawns.Add(area);
            player.ShowMessage("Spawn successfully created");
        }

        private class Arguments
        {
            public string? CommandName { get; set; }
        }
    }
}