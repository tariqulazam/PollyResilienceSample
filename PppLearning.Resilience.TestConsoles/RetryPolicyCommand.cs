namespace PppLearning.Resilience.TestConsoles
{
    using Polly;
    using PppLearning.Framework.Consoles;
    using PppLearning.Resilience.TestConsoles.Model;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;

    public class RetryPolicyCommand : ConsoleCommand
    {
        public override string DisplayText => "Execute Retry Policy";

        public override int MenuOrder => 0;

        public override void Execute()
        {
            this.WriteLineInColor("Enter school id", ConsoleColor.Yellow);
            var schoolId = Convert.ToInt32(Console.ReadLine());

            try
            {
                var classrooms = Policy
                    .HandleResult<IEnumerable<Classroom>>(classes => !classes.Any())
                    .Or<InvalidDataException>()
                    .RetryAsync(
                        retryCount: 3,
                        onRetryAsync: (result, retryNo) => Task.Run(() => this.WriteLineInColor($"Retry no - {retryNo}", ConsoleColor.Green)))
                    .ExecuteAsync(() => this.GetClassroomsAsync(schoolId)).Result;

                this.WriteLineInColor($"Classrooms: {string.Join(", ", classrooms.Select(cls => cls.Name))}", ConsoleColor.Green);
            }
            catch (Exception ex)
            {
                this.WriteLineInColor("Unhandled error occurred", ConsoleColor.Red);
            }
        }

        public async Task<IEnumerable<Classroom>> GetClassroomsAsync(int schoolId)
        {
            this.WriteLineInColor("Executing GetClassroomsAsync", ConsoleColor.Green);

            if (schoolId % 3 == 0)
            {
                throw new InvalidDataException("Invalid School Id");
            }

            if (schoolId % 5 == 0)
            {
                return Enumerable.Empty<Classroom>();
            }

            return await Task.Run(() => new List<Classroom>() { new Classroom() { Id = 1, Name = "1A" }, new Classroom() { Id = 2, Name = "1B" } });
        }

    }
}
