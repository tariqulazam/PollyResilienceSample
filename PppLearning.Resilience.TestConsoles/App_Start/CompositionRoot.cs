using Ninject;
using Ninject.Modules;
using PppLearning.Framework.Composition;

namespace PppLearning.Resilience.TestConsoles
{
    public partial class CompositionRoot : NinjectModule
    {

        public override void Load()
        {
            Kernel.BindAllClasses();
            Kernel.BindFactories();
            Kernel.Load(Modules.Framework);
        }

    }
}