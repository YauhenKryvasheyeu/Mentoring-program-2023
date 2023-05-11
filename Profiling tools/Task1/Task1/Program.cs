using System.Diagnostics;
using System.Security.Cryptography;

internal class Program
{
    private static void Main(string[] args)
    {
        var hash = GeneratePasswordHashUsingSalt("password", new byte[16]);
        Console.ReadLine();
    }

    public static string GeneratePasswordHashUsingSalt(string passwordText, byte[] salt)
    {
        var iterate = 10000;
        var pbkdf2 = new Rfc2898DeriveBytes(passwordText, salt, iterate);
        byte[] hash = pbkdf2.GetBytes(20);
        byte[] hashBytes = new byte[36];

        int i = 0;
        for (; i < salt.Length; i++)
        {
            hashBytes[i] = salt[i];
        }

        for (int j = 0; j < hash.Length; i++, j++)
        {
            hashBytes[i] = hash[j];
        }

        var passwordHash = Convert.ToBase64String(hashBytes);
        return passwordHash;
    }
}