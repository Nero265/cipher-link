
using System.Buffers.Binary;
using System.Text.Json;

namespace CipherLink.Protocol
{
    public static class FrameReader
    {
        public static async Task<MessageHeader> ReadHeaderAsync(Stream stream, CancellationToken ct = default)
        {
            var prefix = new byte[FrameFormat.PrefixSize];
            await stream.ReadExactlyAsync(prefix, ct);

            if (!prefix.AsSpan(0, 4).SequenceEqual(FrameFormat.Magic))
                throw new InvalidDataException("Invalid magic");
            if (prefix[4] != FrameFormat.Version)
                throw new InvalidDataException($"Unsupported version {prefix[4]}");

            uint len = BinaryPrimitives.ReadUInt32BigEndian(prefix.AsSpan(5));
            if (len == 0 || len > FrameFormat.MaxHeaderSize)
                throw new InvalidDataException("Invalid header length");

            var json = new byte[len];
            await stream.ReadExactlyAsync(json, ct);

            return JsonSerializer.Deserialize<MessageHeader>(json, HeaderSerializer.Options)
                ?? throw new InvalidDataException("Empty header");
        }
    }
}
