using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ninject;
using PppLearning.Framework.Configuration.Subscription;
using PppLearning.Framework.Consoles;


namespace PppLearning.Resilience.TestConsoles
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var kernel = new StandardKernel())
            {
                kernel.Load<CompositionRoot>();

                // Remove the following line if you don't want a menu in your app
                kernel.Get<IConsoleMenuPopulator>().Populate();
            }
        }
    }
}
