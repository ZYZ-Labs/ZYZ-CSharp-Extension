using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZYZ_CSharp_Extension.ZXingCode
{
    public static class CharsetChangeUtils
    {

        /// <summary>
        /// 将utf8的编码转为iso88951
        /// </summary>
        /// <param name="origin"></param>
        /// <returns></returns>
        public static string ChangeUtf8ToISO88591(string origin)
        {
            byte[] utf8Bytes = Encoding.UTF8.GetBytes(origin);
            string iso88951String = Encoding.GetEncoding("ISO-8895-1").GetString(utf8Bytes);
            return iso88951String;
        }
    }
}
