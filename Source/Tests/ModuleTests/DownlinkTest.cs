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
    public class DownlinkTest
    {
        Downlink Downlink;

        public DownlinkTest()
        {
            Downlink = new Downlink();
        }

        [Fact]
        public void CreateThrowsException()
        {
            Assert.Throws<NotImplementedException>(() => Downlink.Create());
        }

        [Fact]
        public void OnActionThrowsException()
        {
            Assert.Throws<OnActionException>(() =>
                Downlink.OnAction(It.IsAny<object>(), It.IsAny<EventArgs>()));
        }

        [Fact]
        public void OnEnableThrowsException()
        {
            Assert.Throws<OnEnableException>(() => Downlink.OnEnable(It.IsAny<string[]>()));
        }

        [Fact]
        public void OnStringThrowsException()
        {
            Assert.Throws<NotImplementedException>(() => Downlink.ToString());
        }
    }
}
