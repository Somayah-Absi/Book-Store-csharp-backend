using System.Security.Cryptography;

public class KeyGenerator
{
  public static string GenerateRandomKey(int length)
    {
        byte[] randomBytes = new byte[length];
        using (var rng = RandomNumberGenerator.Create())
        {
            rng.GetBytes(randomBytes);
        }
        return Convert.ToBase64String(randomBytes);
    }
}