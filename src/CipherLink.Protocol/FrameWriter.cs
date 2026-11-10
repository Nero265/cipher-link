using System.Buffers.Binary;
using System.Text.Json;

namespace CipherLink.Protocol
{
    public static class FrameWriter
    {
        public static async Task WriteHeaderAsync(Stream stream, MessageHeader header, CancellationToken ct = default)
        {
            var json = JsonSerializer.SerializeToUtf8Bytes(header, HeaderSerializer.Options);
            if (json.Length > FrameFormat.MaxHeaderSize)
                throw new InvalidDataException("Header to large");

            var prefix = new byte[FrameFormat.PrefixSize];
            FrameFormat.Magic.CopyTo(prefix);
            prefix[4] = FrameFormat.Version;
            BinaryPrimitives.WriteUInt32BigEndian(prefix.AsSpan(5), (uint)json.Length);

            await stream.WriteAsync(prefix, ct);
            await stream.WriteAsync(json, ct);
        }
    }
}
