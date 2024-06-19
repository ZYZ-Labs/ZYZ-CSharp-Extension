using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZYZ_CSharp_Extension.Encrypt
{
    public enum EncryptExceptionEnums
    {
        INIT_KEY_FIRST,
        KEY_SIZE_ERROR,
        ENCRYPT_ERROR,
        DECRYPT_ERROR,
        NO_IV,
        MODE_NOT_SUPPORT,
        PADDING_NOT_SUPPORT_DATA_SIZE,
        CONFIG_ERROR,
        PUBLIC_KEY_NOT_SET,
        PRIVATE_KEY_NOT_SET,
        FILE_NOT_FOUND,
        FILE_ENCRYPT_NOT_SUPPORT
    }

    public static class EncryptExceptionEnumsExtensions
    {
        public static string GetMessage(this EncryptExceptionEnums exceptionEnum)
        {
            switch (exceptionEnum)
            {
                case EncryptExceptionEnums.INIT_KEY_FIRST:
                    return "请先初始化Key";
                case EncryptExceptionEnums.KEY_SIZE_ERROR:
                    return "密钥长度错误";
                case EncryptExceptionEnums.ENCRYPT_ERROR:
                    return "加密失败";
                case EncryptExceptionEnums.DECRYPT_ERROR:
                    return "解密失败";
                case EncryptExceptionEnums.NO_IV:
                    return "该模式需要传入IV";
                case EncryptExceptionEnums.MODE_NOT_SUPPORT:
                    return "当前模式尚未支持";
                case EncryptExceptionEnums.PADDING_NOT_SUPPORT_DATA_SIZE:
                    return "当前数据的长度不符合填充模式";
                case EncryptExceptionEnums.CONFIG_ERROR:
                    return "配置异常";
                case EncryptExceptionEnums.PUBLIC_KEY_NOT_SET:
                    return "公钥未设置";
                case EncryptExceptionEnums.PRIVATE_KEY_NOT_SET:
                    return "私钥未设置";
                case EncryptExceptionEnums.FILE_NOT_FOUND:
                    return "文件未找到";
                case EncryptExceptionEnums.FILE_ENCRYPT_NOT_SUPPORT:
                    return "不支持文件加解密";
                default:
                    throw new ArgumentOutOfRangeException(nameof(exceptionEnum), exceptionEnum, null);
            }
        }
    }

}
