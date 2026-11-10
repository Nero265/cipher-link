namespace CipherLink.Protocol
{
    public static class FrameFormat
    {
        public static ReadOnlySpan<byte> Magic => "CLNK"u8;
        public const byte Version = 1;
        public const int PrefixSize = 9;
        public const int MaxHeaderSize = 64 * 1024;
    }
}
