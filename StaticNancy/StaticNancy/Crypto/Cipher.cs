using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace StaticNancy.Crypto
{
    public sealed class Cipher
    {
        const int NEW_SALT_BYTES = 24;
        const int NEW_ITERATIONS = 10000;
        const int KEY_BITS = 256;
        const int BLOCK_BITS = 128;

        /// <summary>
        /// Encrypt with AES then authenticate using HMAC both with keys from PBKDF2.
        /// </summary>
        /// <param name="plainText"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public string EncryptUsingPassword(string plainText, string password)
        {
            byte[] plainBytes = Encoding.UTF8.GetBytes(plainText);
            byte[] cipherBytes = EncryptBytesUsingPassword(plainBytes, password);
            return Convert.ToBase64String(cipherBytes);
        }

        public string DecryptUsingPassword(string cipherText, string password)
        {
            byte[] cipherBytes = Convert.FromBase64String(cipherText);
            byte[] plainBytes = DecryptBytesUsingPassword(cipherBytes, password);
            return Encoding.UTF8.GetString(plainBytes);
        }

        /// <summary>
        /// Encrypt with AES then authenticate using HMAC both with keys from PBKDF2.
        /// </summary>
        /// <param name="plainBytes"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        byte[] EncryptBytesUsingPassword(byte[] plainBytes, string password)
        {
            byte[] cipherSalt = MakeRandomSalt(NEW_SALT_BYTES);
            byte[] cipherKey = MakeKey(password, cipherSalt);

            byte[] authSalt = MakeRandomSalt(NEW_SALT_BYTES);
            byte[] authKey = MakeKey(password, authSalt);

            byte[] cipherBytes = EncryptBytes(plainBytes, cipherKey);

            byte[] noMacBytes = Combine(authSalt, cipherSalt, cipherBytes);

            byte[] hmac = MakeHMAC(noMacBytes, authKey);

            // AUTH_SALT + CIPHER_SALT + IV + CIPHER_TEXT + HMAC

            byte[] final = Combine(noMacBytes, hmac);

            return final;
        }

        byte[] DecryptBytesUsingPassword(byte[] cipherBytes, string password)
        {
            var cipherSalt = new byte[NEW_SALT_BYTES];
            var authSalt = new byte[NEW_SALT_BYTES];

            Buffer.BlockCopy(cipherBytes, 0, authSalt, 0, NEW_SALT_BYTES);
            Buffer.BlockCopy(cipherBytes, NEW_SALT_BYTES, cipherSalt, 0, NEW_SALT_BYTES);

            byte[] cipherKey = MakeKey(password, cipherSalt);
            byte[] authKey = MakeKey(password, authSalt);

            int hmacByteLength;
            if (!AuthenticateHMAC(cipherBytes, authKey, out hmacByteLength))
                throw new CryptographicException("HMAC authentication failed");

            var messageBytes = new byte[cipherBytes.Length - (NEW_SALT_BYTES * 2) - hmacByteLength];
            Buffer.BlockCopy(cipherBytes, NEW_SALT_BYTES * 2, messageBytes, 0, messageBytes.Length);

            return DecryptBytes(messageBytes, cipherKey);
        }

        bool AuthenticateHMAC(byte[] cipherBytes, byte[] authKey, out int hmacByteLength)
        {
            using (var hmac = new HMACSHA256(authKey))
            {
                hmacByteLength = hmac.HashSize / 8;
                var givenHmac = new byte[hmacByteLength];
                Buffer.BlockCopy(cipherBytes, cipherBytes.Length - givenHmac.Length, givenHmac, 0, givenHmac.Length);

                byte[] computedHmac = hmac.ComputeHash(cipherBytes, 0, cipherBytes.Length - givenHmac.Length);

                return TimeSafeByteEquals(givenHmac, computedHmac);
            }
        }

        byte[] MakeHMAC(byte[] message, byte[] authKey)
        {
            using (var hmac = new HMACSHA256(authKey))
            {
                return hmac.ComputeHash(message);
            }
        }

        byte[] EncryptBytes(byte[] plainBytes, byte[] cipherKey)
        {
            using (var aes = new AesEncryptor(KEY_BITS, BLOCK_BITS))
            {
                return aes.Encrypt(plainBytes, cipherKey);
            }
        }

        byte[] DecryptBytes(byte[] cipherBytes, byte[] cipherKey)
        {
            using (var aes = new AesEncryptor(KEY_BITS, BLOCK_BITS))
            {
                return aes.Decrypt(cipherBytes, cipherKey);
            }
        }

        byte[] MakeRandomSalt(int saltBytes)
        {
            using (var random = RandomNumberGenerator.Create())
            {
                var salt = new byte[saltBytes];
                random.GetBytes(salt);
                return salt;
            }
        }

        byte[] MakeKey(string password, byte[] salt)
        {
            using (var pbkdf = new Rfc2898DeriveBytes(password, salt, NEW_ITERATIONS))
            {
                return pbkdf.GetBytes(KEY_BITS / 8);
            }
        }

        byte[] Combine(byte[] a, byte[] b, byte[] c)
        {
            var concat = new byte[a.Length + b.Length + c.Length];
            Buffer.BlockCopy(a, 0, concat, 0, a.Length);
            Buffer.BlockCopy(b, 0, concat, a.Length, b.Length);
            Buffer.BlockCopy(c, 0, concat, a.Length + b.Length, c.Length);
            return concat;
        }

        byte[] Combine(byte[] a, byte[] b)
        {
            var ret = new byte[a.Length + b.Length];
            Buffer.BlockCopy(a, 0, ret, 0, a.Length);
            Buffer.BlockCopy(b, 0, ret, a.Length, b.Length);
            return ret;
        }

        bool TimeSafeByteEquals(byte[] a, byte[] b)
        {
            uint diff = (uint)a.Length ^ (uint)b.Length;
            for (int i = 0; i < a.Length && i < b.Length; i++)
                diff |= (uint)(a[i] ^ b[i]);
            return diff == 0;
        }
    }
}
