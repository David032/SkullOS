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
    public class UplinkTest
    {
        Uplink Uplink;

        public UplinkTest()
        {
            Uplink = new Uplink();
        }

        [Fact]
        public void CreateThrowsException()
        {
            Assert.Throws<NotImplementedException>(() => Uplink.Create());
        }

        [Fact]
        public void OnActionThrowsException()
        {
            Assert.Throws<OnActionException>(() => 
                Uplink.OnAction(It.IsAny<object>(), It.IsAny<EventArgs>()));
        }

        [Fact]
        public void OnEnableThrowsException()
        {
            Assert.Throws<OnEnableException>(() => Uplink.OnEnable(It.IsAny<string[]>()));
        }

        [Fact]
        public void OnStringThrowsException()
        {
            Assert.Throws<NotImplementedException>(() => Uplink.ToString());
        }
    }
}
