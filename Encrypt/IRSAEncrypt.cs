using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZYZ_CSharp_Extension.Encrypt
{
    public interface IRSAEncrypt:IEncrypt
    {
        /// <summary>
        /// 生成公钥私钥对
        /// </summary>
        /// <returns></returns>
        IRSAEncrypt GenerateKeyPair();

        /// <summary>
        /// 获取公钥
        /// </summary>
        /// <returns></returns>
        Byte[] GetPublicKeyBytes();

        /// <summary>
        /// 获取私钥
        /// </summary>
        /// <returns></returns>
        Byte[] GetPrivateKeyBytes();
    }
}
