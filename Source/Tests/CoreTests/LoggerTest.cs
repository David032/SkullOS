using skullOS.Core;
using skullOS.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;

namespace CoreTests
{
    public class LoggerTest
    {
        ISkullLogger SkullLogger;

        public LoggerTest()
        {
            FileManager.CreateSkullDirectory(isTest: true);
            SkullLogger = new SkullLogger();
        }

        [Fact]
        public void CanCreateLogger()
        {
            Assert.NotNull(SkullLogger);
        }

        [Fact]
        public void LogIsLocatable()
        {
            Assert.Equal(It.IsAny<string>(), SkullLogger.GetLogLocation());
        }

        [Fact]
        public void CanAddToLog() 
        {
            var logContentsBefore = File.ReadAllText(SkullLogger.GetLogLocation());
            SkullLogger.LogMessage("FooBar!");
            var logContentsAfter = File.ReadAllText(SkullLogger.GetLogLocation());

            Assert.NotEqual(logContentsBefore, logContentsAfter);
        }
    }
}
