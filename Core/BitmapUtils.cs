using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace ZYZ_CSharp_Extension.Core
{
    /// <summary>
    /// Bitmap工具类
    /// </summary>
    public static class BitmapUtils
    {
        /// <summary>
        /// 获取Bitmap的Byte数组
        /// </summary>
        /// <param name="bitmap"></param>
        /// <returns></returns>
        public static byte[] GetBitmapBytes(Bitmap bitmap)
        {
            // 确定位图的像素格式
            PixelFormat pixelFormat = bitmap.PixelFormat;

            // 锁定位图的像素数据
            BitmapData bitmapData = bitmap.LockBits(
                new Rectangle(0, 0, bitmap.Width, bitmap.Height),
                ImageLockMode.ReadOnly,
                pixelFormat
            );

            // 获取每个像素的字节数
            int bytesPerPixel = Image.GetPixelFormatSize(pixelFormat) / 8;

            // 计算字节数组的大小
            int byteCount = bitmapData.Stride * bitmap.Height;
            byte[] pixels = new byte[byteCount];

            // 复制位图的像素数据到字节数组中
            Marshal.Copy(bitmapData.Scan0, pixels, 0, byteCount);

            // 解锁位图的像素数据
            bitmap.UnlockBits(bitmapData);

            return pixels;
        }
    }
}
