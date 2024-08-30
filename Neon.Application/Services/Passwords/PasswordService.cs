using System.Security.Cryptography;
using System.Text;
using Konscious.Security.Cryptography;

namespace Neon.Application.Services.Passwords;

internal class PasswordService : IPasswordService, IDisposable
{
    private const int SALT_LENGTH = 32;

    private readonly RandomNumberGenerator _randomNumberGenerator = RandomNumberGenerator.Create();

    public async Task<string> HashAsync(string password)
    {
        var salt = new byte[SALT_LENGTH];

        _randomNumberGenerator.GetBytes(salt);

        var hashBytes = await HashAsync(password, salt);

        return Convert.ToBase64String(hashBytes);
    }

    public async Task<bool> CompareAsync(string hash, string password)
    {
        var hashBytes = Convert.FromBase64String(hash);
        var salt = new byte[SALT_LENGTH];

        Buffer.BlockCopy(hashBytes, 0, salt, 0, salt.Length);

        var passwordHashBytes = await HashAsync(password, salt);

        return Compare(hash, Convert.ToBase64String(passwordHashBytes));
    }

    public void Dispose() => _randomNumberGenerator.Dispose();

    private static async Task<byte[]> HashAsync(string password, byte[] salt)
    {
        var argon2 = new Argon2id(Encoding.Default.GetBytes(password))
        {
            Salt = salt,
            Iterations = 3,
            MemorySize = 65536,
            DegreeOfParallelism = Environment.ProcessorCount
        };

        var hashedBytes = await argon2.GetBytesAsync(64);
        var resultBytes = new byte[salt.Length + hashedBytes.Length];

        Buffer.BlockCopy(salt, 0, resultBytes, 0, salt.Length);
        Buffer.BlockCopy(hashedBytes, 0, resultBytes, salt.Length, hashedBytes.Length);

        return resultBytes;
    }

    private static bool Compare(string hash1, string hash2)
    {
        Span<byte> hash1Bytes = stackalloc byte[Encoding.Default.GetByteCount(hash1)];
        Span<byte> hash2Bytes = stackalloc byte[Encoding.Default.GetByteCount(hash2)];

        Encoding.Default.GetBytes(hash1, hash1Bytes);
        Encoding.Default.GetBytes(hash2, hash2Bytes);

        return CryptographicOperations.FixedTimeEquals(hash1Bytes, hash2Bytes);
    }
}