using PppLearning.Framework.Logging;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PppLearning.Framework.Consoles
{
    /// <summary>
    /// Outputs to the Console a menu populated with the provided commands.
    /// </summary>
    public class ConsoleMenuPopulator : IConsoleMenuPopulator
    {
        /// <summary>
        /// The log.
        /// </summary>
        private static ILogBase log;

        /// <summary>
        /// Initializes a new instance of the <see cref="ConsoleMenuPopulator"/> class.
        /// </summary>
        /// <param name="commands">
        /// The commands.
        /// </param>
        /// <param name="logFactory">
        /// The log factory.
        /// </param>
        public ConsoleMenuPopulator(ConsoleCommand[] commands, ILogFactory logFactory)
        {
            this.Commands = commands.OrderBy(c => c.MenuOrder).ToList();
            if (log == null)
            {
                log = logFactory.CreateLogger(GetType());
            }
        }

        /// <summary>
        /// The list of commnad objects to be populated in the menu
        /// </summary>
        public List<ConsoleCommand> Commands { get; }

        /// <summary>
        /// Populates the menu with the command objects
        /// </summary>
        public void Populate()
        {
            while (true)
            {
                try
                {
                    Console.WriteLine();

                    int index = 0;
                    Console.WriteLine(new string('-', 20));
                    foreach (ConsoleCommand command in this.Commands)
                    {
                        Console.WriteLine($"{++index}: {command.DisplayText}");
                    }
                    Console.WriteLine(new string('-', 20));
                    Console.Write("Choose an item by entering the index: ");
                    var menuKey = Console.ReadKey(false).KeyChar;
                    Console.WriteLine();

                    if (!char.IsDigit(menuKey))
                        continue;

                    var selectedCommand = this.Commands[(int)char.GetNumericValue(menuKey) - 1];
                    selectedCommand?.Execute();

                    Console.WriteLine();
                }
                catch (Exception exception)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine(exception);
                    Console.ResetColor();
                    log.Error(exception);
                }
            }
        }
    }
}

