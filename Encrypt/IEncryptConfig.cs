using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace ZYZ_CSharp_Extension.Encrypt
{
    public interface IEncryptConfig
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
        /// <summary>
        /// 加密类型
        /// </summary>
        /// <returns></returns>
        EncryptAlgorithm Algorithm();
        /// <summary>
        /// 加密模式
        /// </summary>
        /// <returns></returns>
        EncryptMode Mode();
        /// <summary>
        /// 填充方式
        /// </summary>
        /// <returns></returns>
        EncryptPadding Padding();
        /// <summary>
        /// 头部是否组合IV
        /// </summary>
        Boolean ComposeIV { get; }
    }
}
