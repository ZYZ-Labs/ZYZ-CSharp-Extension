using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZYZ_CSharp_Extension.Encrypt
{
    internal interface IEncrypt
    {
        /// <summary>
        /// 加密使用十六进制输出
        /// </summary>
        /// <param name="dataBytes"></param>
        /// <returns></returns>
        string EncryptToHex(Byte[] dataBytes);
        /// <summary>
        /// 加密使用Base64编码输出
        /// </summary>
        /// <param name="dataBytes"></param>
        /// <returns></returns>
        string EncryptToBase64(Byte[] dataBytes);
        /// <summary>
        /// 加密
        /// </summary>
        /// <param name="dataBytes"></param>
        /// <returns></returns>
        Byte[] EncryptToByteArray(Byte[] dataBytes);
        /// <summary>
        /// 加密文件
        /// </summary>
        /// <param name="srcFilePath"></param>
        /// <param name="destFilePath"></param>
        void EncryptFile(string srcFilePath, string destFilePath);
        /// <summary>
        /// 从十六进制字符串解密
        /// </summary>
        /// <param name="dataStr"></param>
        /// <returns></returns>
        string DecryptFromHex(string dataStr);
        /// <summary>
        /// 从Base64字符串解密
        /// </summary>
        /// <param name="dataStr"></param>
        /// <returns></returns>
        string DecryptFromBase64(string dataStr);
        /// <summary>
        /// 解密
        /// </summary>
        /// <param name="dataBytes"></param>
        /// <returns></returns>
        Byte[] DecryptFromByteArray(Byte[] dataBytes);
        /// <summary>
        /// 文件解密
        /// </summary>
        /// <param name="srcFilePath"></param>
        /// <param name="destFilePath"></param>
        void DecryptFromFile(string srcFilePath, string destFilePath);
    }
}
