using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Markup;
using WPF_9.security.models;

namespace WPF_9.security.service
{
    public class Aes256 : IEncryptionService
    {
        private Aes256Model _model;

        public Aes256(Aes256Model model)
        {
            _model = model;
        }

        public Encoding Encoding => _model.Encoding;

        public byte[] Decrypt(byte[] cipherText)
        {
            Aes aesAlg = Aes.Create();
            aesAlg.Key = _model.Key;
            aesAlg.IV = _model.IV;

            using (MemoryStream msDecr = new MemoryStream())
            {
                using (CryptoStream csDecr = new CryptoStream(msDecr, aesAlg.CreateDecryptor(), CryptoStreamMode.Write))
                {
                    csDecr.Write(cipherText, 0, cipherText.Length);
                }

                byte[] decryptedBytes = msDecr.ToArray();
                return decryptedBytes;
            }
        }

        public byte[] Encrypt(byte[] plainText)
        {
            Aes aesAlg = Aes.Create();

            aesAlg.Key = _model.Key;
            aesAlg.IV = _model.IV;

            using (MemoryStream msEncrypt = new MemoryStream())
            {
                using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, aesAlg.CreateEncryptor(), CryptoStreamMode.Write))
                {
                    csEncrypt.Write(plainText, 0, plainText.Length);
                }
                byte[] encrBytes = msEncrypt.ToArray();
                return encrBytes;
            }
        }

        public int GenHashCode(byte[] bytes)
        {
            int hash = 23;
            for (int i = 0; i < bytes.Length; i++)
            {
                hash = hash * 31 + bytes[i];
            }
            return hash;
        }

        public int GenHashCode(string str)
        {
            return GenHashCode(_model.Encoding.GetBytes(str));
        }
    }
}
