using System.Security.Cryptography;
using System.Text;

namespace Backend.Common.Security
{
    public static class PasswordGenerator 
    {
        public static string GenerateSecurePassword(int length = 8) 
        {
            const string validChars = "ABCDEFGHJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789!@#$%^&*?_~-";
            StringBuilder password = new StringBuilder();
            byte[] randomBytes = new byte[length];

        
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomBytes);
            }


            foreach (byte b in randomBytes)
            {
                password.Append(validChars[b % validChars.Length]);
            }

            return password.ToString();
        }
    }
}