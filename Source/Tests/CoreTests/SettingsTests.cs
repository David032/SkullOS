using skullOS.Core;

namespace CoreTests
{
    public class SettingsTests
    {
        [Fact]
        public void TestSettingsLoader()
        {
            var result = SettingsLoader.LoadConfig(@"TestData/TestSettings.txt");
            Assert.NotNull(result);
            Assert.Single(result);
            result.TryGetValue("Worked", out string response);
            Assert.Equal("True", response);
        }
    }
}
