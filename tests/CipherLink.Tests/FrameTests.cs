using CipherLink.Protocol;
using System;
using System.Collections.Generic;
using System.Text;

namespace CipherLink.Tests
{
    public class FrameTests
    {
        [Fact]
        public async Task Header_RoundTrip()
        {
            var header = new MessageHeader(MessageType.File, "Nenad", DateTimeOffset.UtcNow, 1234, "XTEA-OFB", "slika.png", "AAECAwQFBgc=", "BLAKE-256");
            var ms = new MemoryStream();

            await FrameWriter.WriteHeaderAsync(ms, header);
            ms.Position = 0;
            var read = await FrameReader.ReadHeaderAsync(ms);

            Assert.Equal(header, read);
        }

        [Fact]
        public async Task BadMagic_Throws()
        {
            var ms = new MemoryStream(new byte[] { 1, 2, 3, 4, 1, 0, 0, 0, 5 });
            await Assert.ThrowsAsync<InvalidDataException>(() => FrameReader.ReadHeaderAsync(ms));
        }

        [Fact]
        public async Task OversizedHeader_Throws()
        {
            var bytes = new byte[] { (byte)'C', (byte)'L', (byte)'N', (byte)'K', 1, 0xFF, 0xFF, 0xFF, 0xFF };
            await Assert.ThrowsAsync<InvalidDataException>(
                () => FrameReader.ReadHeaderAsync(new MemoryStream(bytes)));
        }
    }
}
