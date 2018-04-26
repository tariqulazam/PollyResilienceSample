namespace PppLearning.Resilience.TestConsoles
{
    using Polly;
    using Polly.Timeout;
    using PppLearning.Framework.Consoles;
    using PppLearning.Resilience.TestConsoles.Model;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public class TimeoutPolicyCommand : ConsoleCommand
    {
        public override string DisplayText => "Execute Timeout Policy";

        public override int MenuOrder => 2;

        public override void Execute()
        {
            this.WriteLineInColor("Enter school id", ConsoleColor.Yellow);
            var schoolId = Convert.ToInt32(Console.ReadLine());

            try
            {
                var classrooms = Policy
                                .TimeoutAsync<IEnumerable<Classroom>>(3, TimeoutStrategy.Pessimistic)
                                .ExecuteAsync(() => this.GetClassroomsAsync(schoolId))
                                .Result;

                this.WriteLineInColor($"Classrooms: {string.Join(", ", classrooms.Select(cls => cls.Name))}", ConsoleColor.Green);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Execution timed out");
            }
        }

        public async Task<IEnumerable<Classroom>> GetClassroomsAsync(int schoolId)
        {
            this.WriteLineInColor("Executing GetClassroomsAsync", ConsoleColor.Green);

            if (schoolId % 2 == 0)
            {
                await Task.Delay(10000);
            }

            return await Task.Run(() => new List<Classroom>() { new Classroom() { Id = 1, Name = "1A" }, new Classroom() { Id = 2, Name = "1B" } });
        }
    }
}
