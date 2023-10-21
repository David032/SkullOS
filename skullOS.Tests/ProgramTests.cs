using skullOS.Core.Interfaces;

namespace skullOS.Tests
{
    //Main & Run are not tested
    public class ProgramTests
    {
        static List<ISubSystem> CreateTestModules()
        {
            string testDataLocation = @"Data\Modules.txt";
            Modules modules = new(testDataLocation);
            return Program.LoadModules(modules);
        }

        [Fact]
        public void TestLoadModules()
        {
            var modulesLoaded = CreateTestModules();
            Assert.NotNull(modulesLoaded);
        }

        [Fact]
        public void TestSetupModules()
        {
            var modulesLoaded = CreateTestModules();
            Assert.True(Program.SetupModules(modulesLoaded));
        }

        [Fact]
        public void TestRunModules()
        {
            var modulesLoaded = CreateTestModules();
            Assert.True(Program.RunModules(modulesLoaded));
        }
    }
}
