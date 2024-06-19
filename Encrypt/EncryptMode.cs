using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System;

namespace ZYZ_CSharp_Extension.Encrypt
{
    public enum EncryptMode
    {
        ECB,
        CBC,
        CTR,
        GCM
    }

    public static class EncryptModeExtensions
    {
        public static string GetMode(this EncryptMode encryptMode)
        {
            return encryptMode switch
            {
                EncryptMode.ECB => "ECB",
                EncryptMode.CBC => "CBC",
                EncryptMode.CTR => "CTR",
                EncryptMode.GCM => "GCM",
                _ => throw new ArgumentOutOfRangeException(nameof(encryptMode), encryptMode, null),
            };
        }
    }
}

