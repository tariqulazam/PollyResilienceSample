namespace PppLearning.Framework.Consoles
{
    using System;

    /// <summary>
    /// When inherited represents a command object suitable for console applications.
    /// </summary>
    public abstract class ConsoleCommand
    {

        private static readonly object lockObject = new object();

        /// <summary>
        /// Gets or sets the position of this command to be displayed in menu
        /// </summary>
        public virtual int MenuOrder { get; set; }

        /// <summary>
        /// The text in a menu to represent this command.
        /// </summary>
        public abstract string DisplayText { get; }

        /// <summary>
        /// When implemented executes the command
        /// </summary>
        public abstract void Execute();

        public void WriteLineInColor(string msg, ConsoleColor color)
        {
            lock (lockObject)
            {
                Console.ForegroundColor = color;
                Console.WriteLine(msg);
                Console.ResetColor();
            }
        }
    }
}