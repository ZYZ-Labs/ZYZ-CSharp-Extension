using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZYZ_CSharp_Extension.Encrypt
{
    /// <summary>
    /// 加密方式
    /// </summary>
    public enum EncryptAlgorithm
    {
        AES,
        DES,
        DESede,
        RSA,
        SM4
    }
    public static class EncryptAlgorithmExtensions
    {
        public static string GetAlgorithm(this EncryptAlgorithm encryptAlgorithm)
        {
            return encryptAlgorithm switch
            {
                EncryptAlgorithm.AES => "AES",
                EncryptAlgorithm.DES => "DES",
                EncryptAlgorithm.DESede => "DESede",
                EncryptAlgorithm.RSA => "RSA",
                EncryptAlgorithm.SM4 => "SM4",
                _ => throw new ArgumentOutOfRangeException(nameof(encryptAlgorithm), encryptAlgorithm, null),
            };
        }
    }
}
