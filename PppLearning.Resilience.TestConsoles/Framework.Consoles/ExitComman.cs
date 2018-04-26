using System;

namespace PppLearning.Framework.Consoles
{
    /// <summary>
    /// Represents a command that exits the application when executed.
    /// </summary>
    public class ExitComman : ConsoleCommand
    {
        public override string DisplayText => "Exit";

        public override int MenuOrder => int.MaxValue;

        public override void Execute()
        {
            Environment.Exit(0);
        }


    }
}
