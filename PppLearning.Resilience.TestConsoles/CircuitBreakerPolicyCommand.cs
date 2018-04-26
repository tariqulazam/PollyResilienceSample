namespace PppLearning.Resilience.TestConsoles
{
    using Polly;
    using Polly.CircuitBreaker;
    using PppLearning.Framework.Consoles;
    using PppLearning.Resilience.TestConsoles.Model;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;

    public class CircuitBreakerPolicyCommand : ConsoleCommand
    {
        private CircuitBreakerPolicy circuitBreaker;

        public override string DisplayText => "Execute CircuitBreaker Policy";

        public override int MenuOrder => 5;

        public CircuitBreakerPolicyCommand()
        {
            this.circuitBreaker = Policy.Handle<InvalidDataException>().CircuitBreakerAsync(
                exceptionsAllowedBeforeBreaking: 2,
                durationOfBreak: TimeSpan.FromSeconds(10),
                onBreak: (exception, timeSpan) => this.WriteLineInColor($"Circuit state on break - {this.circuitBreaker.CircuitState}", ConsoleColor.Cyan),
                onReset: () => this.WriteLineInColor($"Circuit state on reset - {this.circuitBreaker.CircuitState}", ConsoleColor.Cyan));
        }

        public override void Execute()
        {
            this.WriteLineInColor("Enter school id", ConsoleColor.Yellow);
            var schoolId = Convert.ToInt32(Console.ReadLine());

            try
            {
                this.WriteLineInColor($"Circuit state before operation- {this.circuitBreaker.CircuitState}", ConsoleColor.Cyan);
                var classrooms = this.circuitBreaker.ExecuteAsync(() => this.GetClassroomsAsync(schoolId)).Result;

                this.WriteLineInColor(
                    $"Classrooms: {string.Join(", ", classrooms.Select(cls => cls.Name))}",
                    ConsoleColor.Green);
            }
            catch (Exception ex)
            {
                this.WriteLineInColor("Unhandler exception encountered", ConsoleColor.Red);
            }
            finally
            {
                this.WriteLineInColor($"Circuit state after operation- {this.circuitBreaker.CircuitState}", ConsoleColor.Cyan);
            }
        }

        public async Task<IEnumerable<Classroom>> GetClassroomsAsync(int schoolId)
        {
            this.WriteLineInColor("Executing GetClassroomsAsync", ConsoleColor.Green);

            if (schoolId % 2 == 0)
            {
                throw new InvalidDataException("School does not exist.");
            }

            return await Task.Run(() => new List<Classroom>() { new Classroom() { Id = 1, Name = "1A" }, new Classroom() { Id = 2, Name = "1B" } });
        }
    }
}
