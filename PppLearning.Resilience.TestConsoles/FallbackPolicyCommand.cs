using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PppLearning.Resilience.TestConsoles
{
    using Polly;
    using PppLearning.Framework.Consoles;
    using PppLearning.Resilience.TestConsoles.Model;
    using System.IO;

    /// <summary>
    /// The retry policy command.
    /// </summary>
    public class FallbackPolicyCommand : ConsoleCommand
    {
        /// <summary>
        /// Gets the display text.
        /// </summary>
        public override string DisplayText => "Execute Fallback Policy";

        /// <summary>
        /// The menu order.
        /// </summary>
        public override int MenuOrder => 3;

        /// <summary>
        /// The execute.
        /// </summary>
        public override void Execute()
        {
            this.WriteLineInColor("Enter school id", ConsoleColor.Yellow);
            var schoolId = Convert.ToInt32(Console.ReadLine());

            try
            {
                var classrooms = Policy<IEnumerable<Classroom>>
                                .Handle<InvalidDataException>()
                                .FallbackAsync(Enumerable.Empty<Classroom>())
                                .ExecuteAsync(() => this.GetClassroomsAsync(schoolId))
                                .Result;

                this.WriteLineInColor($"Classrooms: {string.Join(", ", classrooms.Select(cls => cls.Name))}", ConsoleColor.Green);
            }
            catch (Exception ex)
            {
                this.WriteLineInColor("Unhandled error occurred", ConsoleColor.Red);
            }
        }

        /// <summary>
        /// The get classrooms async.
        /// </summary>
        /// <param name="schoolId">
        /// The school id.
        /// </param>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        public async Task<IEnumerable<Classroom>> GetClassroomsAsync(int schoolId)
        {
            Console.WriteLine("Executing GetClassroomsAsync");

            if (schoolId % 2 == 0)
            {
                throw new InvalidDataException("School does not have any licence.");
            }

            if (schoolId % 5 == 0)
            {
                throw new UnauthorizedAccessException("You are not authorize to access this.");
            }

            return await Task.Run(() => new List<Classroom>() { new Classroom() { Id = 1, Name = "1A" }, new Classroom() { Id = 2, Name = "1B" } });
        }
    }
}
