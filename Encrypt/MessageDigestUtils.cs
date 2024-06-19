using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZYZ_CSharp_Extension.Encrypt.MessageDigestImpl;

namespace ZYZ_CSharp_Extension.Encrypt
{
    public static class MessageDigestUtils
    {
        public static IMessageDigest GetMode(MessageDigestTypes messageDigestTypes)
        {
            switch (messageDigestTypes)
            {
                case MessageDigestTypes.MD5:
                    return new MD5MessageDigest();
                case MessageDigestTypes.SHA256:
                    return new SHA256MessageDigest();
                case MessageDigestTypes.SM3:
                    return new SM3MessageDigest();
                default:
                    throw new ArgumentException("Unsupported messageDigest type.");
            }
        }
    }
}
