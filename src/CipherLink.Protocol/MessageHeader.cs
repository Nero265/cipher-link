namespace CipherLink.Protocol
{
    public sealed record MessageHeader(
        MessageType Type,
        string Sender,
        DateTimeOffset Timestamp,
        long EncryptedSize,
        string? Algorithm = null,
        string? FileName = null,
        string? Iv = null,
        string? HashAlgorithm = null);
}
