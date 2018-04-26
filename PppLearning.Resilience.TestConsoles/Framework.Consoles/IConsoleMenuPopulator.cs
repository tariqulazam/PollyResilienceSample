using System.Collections.Generic;

namespace PppLearning.Framework.Consoles
{
    /// <summary>
    /// When implemented outputs to the Console a menu populated with the provided commands.
    /// </summary>
    public interface IConsoleMenuPopulator
    {
        /// <summary>
        /// Populates the menu with the command objects
        /// </summary>
        void Populate();

        /// <summary>
        /// The list of commnad objects to be populated in the menu
        /// </summary>
        List<ConsoleCommand> Commands { get; }
    }
}