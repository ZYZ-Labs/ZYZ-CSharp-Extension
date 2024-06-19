using Org.BouncyCastle.Crypto.Digests;
using Org.BouncyCastle.Utilities.Encoders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZYZ_CSharp_Extension.Encrypt.MessageDigestImpl
{
    /// <summary>
    /// MD5信息摘要
    /// </summary>
    public class MD5MessageDigest : IMessageDigest
    {
        private byte[] Digest(Stream inputStream)
        {
            var md5Digest = new MD5Digest();
            byte[] buffer = new byte[1024];
            int bytesRead;
            while ((bytesRead = inputStream.Read(buffer, 0, buffer.Length)) != -1)
            {
                md5Digest.BlockUpdate(buffer, 0, bytesRead);
            }

            byte[] result = new byte[md5Digest.GetDigestSize()];
            md5Digest.DoFinal(result, 0);
            return result;
        }

        public string DigestToHex(byte[] dataBytes)
        {
            using (var inputStream = new MemoryStream(dataBytes))
            {
                byte[] digest = Digest(inputStream);
                return Hex.ToHexString(digest);
            }
        }

        public string DigestToBase64(byte[] dataBytes)
        {
            using (var inputStream = new MemoryStream(dataBytes))
            {
                byte[] digest = Digest(inputStream);
                return Convert.ToBase64String(digest);
            }
        }

        public string DigestFile(string srcFile)
        {
            if (!File.Exists(srcFile))
            {
                throw new EncryptException(EncryptExceptionEnums.FILE_NOT_FOUND.GetMessage());
            }

            using (var inputStream = File.OpenRead(srcFile))
            {
                byte[] digest = Digest(inputStream);
                return Hex.ToHexString(digest);
            }
        }
    }
}
