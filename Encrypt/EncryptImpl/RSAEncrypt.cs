using Org.BouncyCastle.Crypto.Digests;
using Org.BouncyCastle.Crypto.Encodings;
using Org.BouncyCastle.Crypto.Engines;
using Org.BouncyCastle.Crypto.Generators;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Pkcs;
using Org.BouncyCastle.Security;
using Org.BouncyCastle.X509;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZYZ_CSharp_Extension.Encrypt.EncryptImpl
{
    public class RSAEncrypt : IRSAEncrypt
    {
        private readonly IRSAEncryptConfig _config;
        private readonly IAsymmetricBlockCipher _cipher;
        private byte[] _publicKey;
        private byte[] _privateKey;

        public RSAEncrypt(IRSAEncryptConfig config)
        {
            _config = config;
            _cipher = new OaepEncoding(new RsaEngine(), new Sha256Digest(), new Sha256Digest(), null);
            _publicKey = new byte[0];
            _privateKey = new byte[0];
        }

        public IRSAEncrypt GenerateKeyPair()
        {
            var keyPairGenerator = new RsaKeyPairGenerator();
            keyPairGenerator.Init(new KeyGenerationParameters(new SecureRandom(), 2048));
            var keyPair = keyPairGenerator.GenerateKeyPair();
            _publicKey = SubjectPublicKeyInfoFactory.CreateSubjectPublicKeyInfo(keyPair.Public).GetEncoded();
            _privateKey = PrivateKeyInfoFactory.CreatePrivateKeyInfo(keyPair.Private).GetEncoded();
            return this;
        }

        private AsymmetricKeyParameter GetPublicKey()
        {
            if ((_config.PublicKey().Length == 0 && _config.PrivateKey().Length == 0) && _publicKey.Length == 0)
            {
                GenerateKeyPair();
            }
            else if (_config.PublicKey().Length == 0)
            {
                throw new EncryptException(EncryptExceptionEnums.PUBLIC_KEY_NOT_SET.GetMessage());
            }
            return PublicKeyFactory.CreateKey(_publicKey);
        }

        private AsymmetricKeyParameter GetPrivateKey()
        {
            if ((_config.PublicKey().Length == 0 && _config.PrivateKey().Length == 0) && _privateKey.Length == 0)
            {
                GenerateKeyPair();
            }
            else if (_config.PrivateKey().Length == 0)
            {
                throw new EncryptException(EncryptExceptionEnums.PRIVATE_KEY_NOT_SET.GetMessage());
            }
            return PrivateKeyFactory.CreateKey(_privateKey);
        }

        public byte[] GetPublicKeyBytes()
        {
            if (_config.PublicKey().Length == 0 && _publicKey.Length == 0)
            {
                GenerateKeyPair();
            }
            return _publicKey;
        }

        public byte[] GetPrivateKeyBytes()
        {
            if (_config.PrivateKey().Length == 0 && _privateKey.Length == 0)
            {
                GenerateKeyPair();
            }
            return _privateKey;
        }

        public string EncryptToHex(byte[] dataBytes)
        {
            return BitConverter.ToString(Encrypt(dataBytes)).Replace("-", "");
        }

        public string EncryptToBase64(byte[] dataBytes)
        {
            return Convert.ToBase64String(Encrypt(dataBytes));
        }

        public byte[] EncryptToByteArray(byte[] dataBytes)
        {
            return Encrypt(dataBytes);
        }

        public void EncryptFile(string srcFile, string destFile)
        {
            throw new EncryptException(EncryptExceptionEnums.FILE_ENCRYPT_NOT_SUPPORT.GetMessage());
        }

        public string DecryptFromHex(string dataStr)
        {
            byte[] bytes = HexStringToByteArray(dataStr);
            return Encoding.UTF8.GetString(Decrypt(bytes));
        }

        public string DecryptFromBase64(string dataStr)
        {
            byte[] bytes = Convert.FromBase64String(dataStr);
            return Encoding.UTF8.GetString(Decrypt(bytes));
        }

        public byte[] DecryptFromByteArray(byte[] dataBytes)
        {
            return Decrypt(dataBytes);
        }

        public void DecryptFromFile(string srcFile, string destFile)
        {
            throw new EncryptException(EncryptExceptionEnums.FILE_ENCRYPT_NOT_SUPPORT.GetMessage());
        }

        private byte[] Encrypt(byte[] dataBytes)
        {
            _cipher.Init(true, GetPublicKey());
            return _cipher.ProcessBlock(dataBytes, 0, dataBytes.Length);
        }

        private byte[] Decrypt(byte[] dataBytes)
        {
            _cipher.Init(false, GetPrivateKey());
            return _cipher.ProcessBlock(dataBytes, 0, dataBytes.Length);
        }

        private byte[] HexStringToByteArray(string hex)
        {
            int numberChars = hex.Length;
            byte[] bytes = new byte[numberChars / 2];
            for (int i = 0; i < numberChars; i += 2)
            {
                bytes[i / 2] = Convert.ToByte(hex.Substring(i, 2), 16);
            }
            return bytes;
        }
    }

}
