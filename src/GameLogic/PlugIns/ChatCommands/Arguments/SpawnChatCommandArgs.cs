// <copyright file="SpawnChatCommandArgs.cs" company="MUnique">
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
// </copyright>

namespace MUnique.OpenMU.GameLogic.PlugIns.ChatCommands.Arguments
{
    /// <summary>
    /// Arguments used by TraceChatCommandPlugIn.
    /// </summary>
    public class SpawnChatCommandArgs : ArgumentsBase
    {
        /// <summary>
        /// Gets or sets the quantity.
        /// </summary>
        [Argument("quantity")]
        public short? Quantity { get; set; }

        /// <summary>
        /// Gets or sets the monster name.
        /// </summary>
        [Argument("monster")]
        public string? MonsterName { get; set; }
    }
}