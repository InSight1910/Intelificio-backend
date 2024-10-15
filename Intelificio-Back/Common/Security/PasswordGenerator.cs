using System.Security.Cryptography;
using System.Text;

namespace Backend.Common.Security
{
    public static class PasswordGenerator 
    {
        public static string GenerateSecurePassword(int length = 8) 
        {
            const string validUperChars = "ABCDEFGHJKLMNOPQRSTUVWXYZ";
            const string validloweChars = "abcdefghijklmnopqrstuvwxyz";
            const string validSpecial = "!@#$%^&*?_~-";
            const string validarNumber = "0123456789";
            StringBuilder password = new StringBuilder();
            byte[] randomBytes = new byte[length];

        
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomBytes);
            }


            foreach (byte b in randomBytes)
            {
                password.Append(validUperChars[b % validUperChars.Length]);
                password.Append(validSpecial[b % validSpecial.Length]);
                password.Append(validarNumber[b % validarNumber.Length]);
                password.Append(validloweChars[b % validloweChars.Length]);
            }

            return password.ToString();
        }
    }
}