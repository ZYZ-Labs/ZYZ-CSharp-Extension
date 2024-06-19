using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZYZ_CSharp_Extension.Encrypt.EncryptImpl;

namespace ZYZ_CSharp_Extension.Encrypt
{
    public static class EncryptUtils
    {
        static EncryptUtils()
        {

        }

        public static IEncrypt GetAlgorithm<T>(T iEncryptConfig) where T : IEncryptConfig
        {
            switch (iEncryptConfig.Algorithm())
            {
                case EncryptAlgorithm.AES:
                    return new AESEncrypt(iEncryptConfig);
                case EncryptAlgorithm.DES:
                    return new DESEncrypt(iEncryptConfig);
                case EncryptAlgorithm.DESede:
                    return new DESedeEncrypt(iEncryptConfig);
                case EncryptAlgorithm.SM4:
                    return new SM4Encrypt(iEncryptConfig);
                case EncryptAlgorithm.RSA:
                    if (iEncryptConfig is IRSAEncryptConfig rsaEncryptConfig)
                    {
                        return new RSAEncrypt(rsaEncryptConfig);
                    }
                    else
                    {
                        throw new ArgumentException("Invalid configuration for RSA encryption.");
                    }
                default:
                    throw new ArgumentException("Unsupported encryption algorithm.");
            }
        }
    }
}
