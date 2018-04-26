namespace PppLearning.Resilience.TestConsoles
{
    using Polly;
    using PppLearning.Framework.Consoles;
    using PppLearning.Resilience.TestConsoles.Model;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public class WaitAndRetryPolicyCommand : ConsoleCommand
    {
        public override string DisplayText => "Execute Retry Policy (Wait and Retry)";

        public override int MenuOrder => 1;

        /// <summary>
        /// The execute.
        /// </summary>
        public override void Execute()
        {
            this.WriteLineInColor("Enter school id", ConsoleColor.Yellow);
            var schoolId = Convert.ToInt32(Console.ReadLine());

            var classrooms = Policy
                            .HandleResult<IEnumerable<Classroom>>(classes => !classes.Any())
                            .WaitAndRetryAsync(
                                new[]
                                {
                                        TimeSpan.FromSeconds(1),
                                        TimeSpan.FromSeconds(2),
                                        TimeSpan.FromSeconds(4)
                                },
                                onRetryAsync: (result, span) => Task.Run(() => this.WriteLineInColor($"Retrying after - {span}", ConsoleColor.Green)))
                            .ExecuteAsync(() => this.GetClassroomsAsync(schoolId)).Result;


            this.WriteLineInColor($"Classrooms: {string.Join(", ", classrooms.Select(cls => cls.Name))}", ConsoleColor.Green);
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
            this.WriteLineInColor("Executing GetClassroomsAsync", ConsoleColor.Green);

            if (schoolId % 5 == 0)
            {
                return Enumerable.Empty<Classroom>();
            }

            return await Task.Run(() => new List<Classroom>() { new Classroom() { Id = 1, Name = "1A" }, new Classroom() { Id = 2, Name = "1B" } });
        }

    }
}
