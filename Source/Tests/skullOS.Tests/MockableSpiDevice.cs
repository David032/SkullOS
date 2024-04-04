using System.Device.Spi;

namespace skullOS.Tests
{
    //Taken from: https://github.com/dotnet/iot/blob/main/src/devices/Board/tests/SpiDummyDevice.cs
    public class MockableSpiDevice : SpiDevice
    {
        private bool _disposed;
        public MockableSpiDevice(SpiConnectionSettings connectionSettings, int[] pins)
        {
            ConnectionSettings = connectionSettings;
            Pins = pins;
            _disposed = false;
        }

        public override SpiConnectionSettings ConnectionSettings { get; }
        public int[] Pins { get; }

        public override byte ReadByte()
        {
            if (_disposed)
            {
                throw new ObjectDisposedException(nameof(MockableSpiDevice));
            }

            return 0xF8;
        }

        public override void Read(Span<byte> buffer)
        {
            throw new NotImplementedException();
        }

        public override void WriteByte(byte value)
        {
            throw new NotImplementedException();
        }

        public override void Write(ReadOnlySpan<byte> buffer)
        {
            throw new NotImplementedException();
        }

        public override void TransferFullDuplex(ReadOnlySpan<byte> writeBuffer, Span<byte> readBuffer)
        {
            throw new NotImplementedException();
        }

        protected override void Dispose(bool disposing)
        {
            _disposed = true;
            base.Dispose(disposing);
        }
    }
}
