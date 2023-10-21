namespace skullOS.Tests
{
    public class ModulesTests
    {

        [Fact]
        public void TestModuleConstructor()
        {
            string testDataLocation = @"Data\Modules.txt";
            Modules modules = new Modules(testDataLocation);
            Assert.Single(modules.Get());
            Assert.Equal("Camera", modules.Get()[0].ModuleName);
        }
    }
}
