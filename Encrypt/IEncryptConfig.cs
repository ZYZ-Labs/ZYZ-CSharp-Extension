using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace ZYZ_CSharp_Extension.Encrypt
{
    internal interface IEncryptConfig
    {
        /// <summary>
        /// 密钥
        /// </summary>
        /// <returns></returns>
        Byte[] Key();
        /// <summary>
        /// 偏移
        /// </summary>
        /// <returns></returns>
        Byte[] Iv()
        {
            byte[] iv = new byte[16]; // 128-bit IV
            RandomNumberGenerator.Fill(iv);
            return iv;
        }
    }
}
