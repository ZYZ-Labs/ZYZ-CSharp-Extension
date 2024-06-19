using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZYZ_CSharp_Extension.Encrypt
{
    public interface IMessageDigest
    {
        /// <summary>
        /// 获取信息摘要使用十六进制输出
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public string DigestToHex(byte[] data);

        /// <summary>
        /// 获取信息摘要使用Base64输出
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public string DigestToBase64(byte[] data);

        /// <summary>
        /// 获取文件的信息摘要，使用十六进制输出
        /// </summary>
        /// <param name="srcPath"></param>
        /// <returns></returns>
        public string DigestFile(string srcPath);
    }
}
