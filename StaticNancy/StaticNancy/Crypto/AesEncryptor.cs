using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;

namespace StaticNancy.Crypto
{
    sealed class AesEncryptor : IDisposable
    {
        readonly Aes _aes;

        public AesEncryptor(int keyBits, int blockBits)
        {
            _aes = Aes.Create();

            _aes.KeySize = keyBits;
            _aes.BlockSize = blockBits;
            _aes.Mode = CipherMode.CBC;
            _aes.Padding = PaddingMode.PKCS7;
        }

        /// <summary>
        /// Returns IV + CIPHER.
        /// </summary>
        /// <param name="plainBytes"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public byte[] Encrypt(byte[] plainBytes, byte[] key)
        {
            _aes.GenerateIV();

            byte[] iv = _aes.IV;

            byte[] cipherBytes = GetCipherBytes(plainBytes, key, iv);

            return Combine(iv, cipherBytes);
        }

        byte[] GetCipherBytes(byte[] plainBytes, byte[] key, byte[] iv)
        {
            using (ICryptoTransform encryptor = _aes.CreateEncryptor(key, iv))
            {
                return TransformBytes(plainBytes, encryptor);
            }
        }

        public byte[] Decrypt(byte[] cipherBytes, byte[] key)
        {
            byte[] iv = GetIVFromCipher(cipherBytes);

            var payloadBytes = new byte[cipherBytes.Length - iv.Length];

            Buffer.BlockCopy(cipherBytes, iv.Length, payloadBytes, 0, payloadBytes.Length);

            return GetPlainBytes(payloadBytes, key, iv);
        }

        byte[] GetIVFromCipher(byte[] cipherBytes)
        {
            int ivLength = _aes.BlockSize / 8;

            var iv = new byte[ivLength];

            Buffer.BlockCopy(cipherBytes, 0, iv, 0, ivLength);

            return iv;
        }

        byte[] GetPlainBytes(byte[] cipherBytes, byte[] key, byte[] iv)
        {
            using (ICryptoTransform encryptor = _aes.CreateDecryptor(key, iv))
            {
                return TransformBytes(cipherBytes, encryptor);
            }
        }

        byte[] TransformBytes(byte[] inputBytes, ICryptoTransform transform)
        {
            using (var outputStream = new MemoryStream())
            {
                using (var cryptoStream = new CryptoStream(outputStream, transform, CryptoStreamMode.Write))
                {
                    using (var binary = new BinaryWriter(cryptoStream))
                    {
                        binary.Write(inputBytes);
                    }
                }

                return outputStream.ToArray();
            }
        }

        byte[] Combine(byte[] a, byte[] b)
        {
            var ret = new byte[a.Length + b.Length];
            Buffer.BlockCopy(a, 0, ret, 0, a.Length);
            Buffer.BlockCopy(b, 0, ret, a.Length, b.Length);
            return ret;
        }

        public void Dispose()
        {
            if (_aes != null)
            {
                _aes.Dispose();
            }
        }
    }
}
