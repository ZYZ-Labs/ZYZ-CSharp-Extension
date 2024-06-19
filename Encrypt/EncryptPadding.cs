using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZYZ_CSharp_Extension.Encrypt
{
    public enum EncryptPadding
    {
        NoPadding,
        PKCS5Padding
    }

    public static class EncyptPaddingExtension
    {
        public static string GetPadding(this EncryptPadding encryptPadding)
        {
            return encryptPadding switch
            {
                EncryptPadding.NoPadding => "NoPadding",
                EncryptPadding.PKCS5Padding => "PKCS5Padding",
                _ => throw new ArgumentOutOfRangeException(nameof(encryptPadding), encryptPadding, null),
            };
        }
    }
}
