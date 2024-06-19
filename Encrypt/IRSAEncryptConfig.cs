using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace ZYZ_CSharp_Extension.Encrypt
{
    public abstract class IRSAEncryptConfig : IEncryptConfig
    {
        /// <summary>
        /// RSA专用公钥
        /// </summary>
        /// <returns></returns>
        public abstract Byte[] PublicKey();
        /// <summary>
        /// RSA专用私钥
        /// </summary>
        /// <returns></returns>
        public abstract Byte[] PrivateKey();

        public byte[] Key()
        {
            return new byte[0];
        }

        public byte[] IV()
        {
            return new byte[0];
        }

        public EncryptAlgorithm Algorithm()
        {
            return EncryptAlgorithm.RSA;
        }

        public EncryptMode Mode()
        {
            return EncryptMode.ECB;
        }

        public EncryptPadding Padding()
        {
            return EncryptPadding.PKCS5Padding;
        }
        public bool ComposeIV => true;
    }
}
