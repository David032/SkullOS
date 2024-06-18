using Moq;
using skullOS.Modules;
using skullOS.Modules.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModuleTests
{
    public class QrCodeReaderTest
    {
        QrCodeReader codeReader;

        public QrCodeReaderTest()
        {
            codeReader = new QrCodeReader();
        }

        [Fact]
        public void CreateThrowsException()
        {
            Assert.Throws<NotImplementedException>(() => codeReader.Create());
        }

        [Fact]
        public void OnActionThrowsException()
        {
            Assert.Throws<OnActionException>(() =>
                codeReader.OnAction(It.IsAny<object>(), It.IsAny<EventArgs>()));
        }

        [Fact]
        public void OnEnableThrowsException()
        {
            Assert.Throws<OnEnableException>(() => codeReader.OnEnable(It.IsAny<string[]>()));
        }

        [Fact]
        public void OnStringThrowsException()
        {
            Assert.Throws<NotImplementedException>(() => codeReader.ToString());
        }
    }
}
