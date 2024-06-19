using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZYZ_CSharp_Extension.Core;

namespace ZYZ_CSharp_Extension.Encrypt.EncryptImpl
{
    public class DESEncrypt : IEncrypt
    {
        private const int BLOCK_SIZE = 8;
        private readonly IEncryptConfig iEncryptConfig;

        public DESEncrypt(IEncryptConfig encryptConfig)
        {
            iEncryptConfig = encryptConfig;
            ValidateConfig();
        }

        private void ValidateConfig()
        {
            if (iEncryptConfig.Key().Length != 8)
            {
                throw new EncryptException("Key size error");
            }
            if (iEncryptConfig.Mode() != EncryptMode.ECB && iEncryptConfig.Iv == null)
            {
                throw new EncryptException("No IV provided for non-ECB mode");
            }
        }

        public string EncryptToHex(byte[] dataBytes)
        {
            using (var inputStream = new MemoryStream(dataBytes))
            using (var memoryStream = new MemoryStream())
            {
                Encrypt(inputStream, memoryStream);
                byte[] encryptedData = memoryStream.ToArray();
                return BitConverter.ToString(encryptedData).Replace("-", "");
            }
        }

        public string EncryptToBase64(byte[] dataBytes)
        {
            using (var inputStream = new MemoryStream(dataBytes))
            using (var memoryStream = new MemoryStream())
            {
                Encrypt(inputStream, memoryStream);
                byte[] encryptedData = memoryStream.ToArray();
                return Convert.ToBase64String(encryptedData);
            }
        }

        public byte[] EncryptToByteArray(byte[] dataBytes)
        {
            using (var inputStream = new MemoryStream(dataBytes))
            using (var memoryStream = new MemoryStream())
            {
                Encrypt(inputStream, memoryStream);
                return memoryStream.ToArray();
            }
        }

        public void EncryptFile(string srcFilePath, string destFilePath)
        {
            using (var inputStream = File.OpenRead(srcFilePath))
            using (var outputStream = File.OpenWrite(destFilePath))
            {
                Encrypt(inputStream, outputStream);
            }
        }

        public string DecryptFromHex(string dataStr)
        {
            byte[] encryptedData = ConvertUtils.HexStringToByteArray(dataStr);
            using (var inputStream = new MemoryStream(encryptedData))
            using (var memoryStream = new MemoryStream())
            {
                Decrypt(inputStream, memoryStream);
                byte[] decryptedData = memoryStream.ToArray();
                return Encoding.UTF8.GetString(decryptedData);
            }
        }

        public string DecryptFromBase64(string dataStr)
        {
            byte[] encryptedData = Convert.FromBase64String(dataStr);
            using (var inputStream = new MemoryStream(encryptedData))
            using (var memoryStream = new MemoryStream())
            {
                Decrypt(inputStream, memoryStream);
                byte[] decryptedData = memoryStream.ToArray();
                return Encoding.UTF8.GetString(decryptedData);
            }
        }

        public byte[] DecryptFromByteArray(byte[] dataBytes)
        {
            using (var inputStream = new MemoryStream(dataBytes))
            using (var memoryStream = new MemoryStream())
            {
                Decrypt(inputStream, memoryStream);
                return memoryStream.ToArray();
            }
        }

        public void DecryptFromFile(string srcFilePath, string destFilePath)
        {
            using (var inputStream = File.OpenRead(srcFilePath))
            using (var outputStream = File.OpenWrite(destFilePath))
            {
                Decrypt(inputStream, outputStream);
            }
        }

        private void Encrypt(Stream inputStream, Stream outputStream)
        {
            var cipher = CipherUtilities.GetCipher($"{iEncryptConfig.Algorithm}/{iEncryptConfig.Mode}/{iEncryptConfig.Padding}");
            var keyParam = ParameterUtilities.CreateKeyParameter(iEncryptConfig.Algorithm().GetAlgorithm(), iEncryptConfig.Key());
            if (iEncryptConfig.Mode() != EncryptMode.ECB)
            {
                var ivParam = new ParametersWithIV(keyParam, iEncryptConfig.Iv());
                cipher.Init(true, ivParam);
            }
            else
            {
                cipher.Init(true, keyParam);
            }

            if (iEncryptConfig.Iv() != null && iEncryptConfig.ComposeIV && iEncryptConfig.Mode() != EncryptMode.ECB)
            {
                outputStream.Write(iEncryptConfig.Iv(), 0, iEncryptConfig.Iv().Length);
            }

            byte[] buffer = new byte[4096]; // 4KB buffer
            int bytesRead;
            while ((bytesRead = inputStream.Read(buffer, 0, buffer.Length)) > 0)
            {
                byte[] encryptedBytes = cipher.ProcessBytes(buffer, 0, bytesRead);
                outputStream.Write(encryptedBytes, 0, encryptedBytes.Length);
            }
            byte[] finalBytes = cipher.DoFinal();
            if (finalBytes != null && finalBytes.Length > 0)
            {
                outputStream.Write(finalBytes, 0, finalBytes.Length);
            }
        }

        private void Decrypt(Stream inputStream, Stream outputStream)
        {
            var cipher = CipherUtilities.GetCipher($"{iEncryptConfig.Algorithm}/{iEncryptConfig.Mode}/{iEncryptConfig.Padding}");
            var keyParam = ParameterUtilities.CreateKeyParameter(iEncryptConfig.Algorithm().GetAlgorithm(), iEncryptConfig.Key());

            if (iEncryptConfig.Iv() != null && iEncryptConfig.ComposeIV && iEncryptConfig.Mode() != EncryptMode.ECB)
            {
                byte[] actualIv = new byte[iEncryptConfig.Iv().Length];
                inputStream.Read(actualIv, 0, actualIv.Length);
                var ivParam = new ParametersWithIV(keyParam, actualIv);
                cipher.Init(false, ivParam);
            }
            else if (iEncryptConfig.Iv() != null && iEncryptConfig.Mode() != EncryptMode.ECB)
            {
                var ivParam = new ParametersWithIV(keyParam, iEncryptConfig.Iv());
                cipher.Init(false, ivParam);
            }
            else
            {
                cipher.Init(false, keyParam);
            }

            byte[] buffer = new byte[4096]; // 4KB buffer
            int bytesRead;
            while ((bytesRead = inputStream.Read(buffer, 0, buffer.Length)) > 0)
            {
                byte[] decryptedBytes = cipher.ProcessBytes(buffer, 0, bytesRead);
                outputStream.Write(decryptedBytes, 0, decryptedBytes.Length);
            }
            byte[] finalBytes = cipher.DoFinal();
            if (finalBytes != null && finalBytes.Length > 0)
            {
                outputStream.Write(finalBytes, 0, finalBytes.Length);
            }
        }
    }
}
