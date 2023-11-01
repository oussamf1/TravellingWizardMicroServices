using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System.Security.Cryptography;

namespace UserOperationsMicroService.Utils
{
    public class PasswordComputation
    {
        public string ComputePasswordHash(string password)
        {
            byte[] salt = GetSalt();
            string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
            password: password!,
            salt: salt,
            prf: KeyDerivationPrf.HMACSHA256,
            iterationCount: 100000,
            numBytesRequested: 256 / 8));

            return hashed;

        }
        public bool VerifyPassword(string password, string storedHashedPassword)
        {
            byte[] storedHashBytes = Convert.FromBase64String(storedHashedPassword);

            byte[] enteredPasswordHash = KeyDerivation.Pbkdf2(
               password: password,
               salt: GetSalt(), // Retrieve the same salt used for hashing
               prf: KeyDerivationPrf.HMACSHA256,
               iterationCount: 100000,
               numBytesRequested: 256 / 8);

            return ByteArraysEqual(storedHashBytes, enteredPasswordHash);
        }
        private bool ByteArraysEqual(byte[] a, byte[] b)
        {
            if (a.Length != b.Length)
                return false;

            for (int i = 0; i < a.Length; i++)
            {
                if (a[i] != b[i])
                    return false;
            }

            return true;
        }
        private byte[] GetSalt()
        {
            byte[] salt = new byte[]
            {
               0x01, 0x23, 0x45, 0x67, 0x89, 0xAB, 0xCD, 0xEF,
               0xFE, 0xDC, 0xBA, 0x98, 0x76, 0x54, 0x32, 0x10
            };
            return salt;

        }

    }
}
