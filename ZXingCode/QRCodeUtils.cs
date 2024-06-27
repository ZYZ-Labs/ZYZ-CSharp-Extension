using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Text;
using ZXing;
using ZXing.Common;
using ZXing.QrCode;
using ZXing.QrCode.Internal;

namespace ZYZ_CSharp_Extension.ZXingCode
{
    public static class QRCodeUtils
    {
        /// <summary>
        /// 读取二维码
        /// </summary>
        /// <param name="bitmap"></param>
        /// <returns></returns>
        public static string ReadQRCode(Bitmap bitmap)
        {
            int width = bitmap.Width;
            int height = bitmap.Height;
            int[] pixels = new int[width * height];
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    pixels[y * width + x] = bitmap.GetPixel(x, y).ToArgb();
                }
            }
            var luminanceSource = new RGBLuminanceSource(pixels, width, height);
            var binaryBitmap = new BinaryBitmap(new HybridBinarizer(luminanceSource));
            var result = new QRCodeReader().decode(binaryBitmap);
            return result.Text;
        }

        /// <summary>
        /// 快捷创建二维码bitmap
        /// </summary>
        /// <param name="info">二维码信息</param>
        /// <param name="size">二维码大小</param>
        /// <param name="color">二维码颜色</param>
        /// <param name="logo">二维码的logo</param>
        /// <param name="withInfo">二维码是否携带信息</param>
        /// <returns></returns>
        public static Bitmap CreateQRCode(string info, int size, Color color = default(Color), Bitmap logo = null, bool withInfo = false)
        {
            if (color == default(Color))
            {
                color = Color.Black;
            }

            var hint = new Dictionary<EncodeHintType, object>
            {
                { EncodeHintType.MARGIN, 0 },
                { EncodeHintType.ERROR_CORRECTION, ErrorCorrectionLevel.H }
            };
            var bitMatrix = new QRCodeWriter().encode(
                Encoding.GetEncoding("ISO-8859-1").GetString(Encoding.UTF8.GetBytes(info)),
                BarcodeFormat.QR_CODE, size, size, hint);
            return BitMatrixToBitmap(bitMatrix, color, logo, withInfo, info);
        }

        /// <summary>
        /// bitmap添加logo和信息文本
        /// </summary>
        /// <param name="bitMatrix">二维码的bit矩阵数据</param>
        /// <param name="color">二维码的颜色</param>
        /// <param name="logo">二维码的logo</param>
        /// <param name="withInfo">二维码是否携带信息</param>
        /// <param name="info">携带的信息</param>
        /// <returns></returns>
        private static Bitmap BitMatrixToBitmap(BitMatrix bitMatrix, Color color, Bitmap logo, bool withInfo, string info)
        {
            int width = bitMatrix.Width;
            int height = bitMatrix.Height;
            Bitmap bitmap = new Bitmap(width, height, PixelFormat.Format32bppArgb);

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    bitmap.SetPixel(x, y, bitMatrix[x, y] ? (color.ToArgb() == Color.White.ToArgb() ? Color.Black : color) : Color.White);
                }
            }

            return AddLogoAndInfoToQRCode(bitmap, logo, withInfo, info);
        }

        /// <summary>
        /// 添加logo和信息文本到二维码图片上
        /// </summary>
        /// <param name="src">原始的bitmap</param>
        /// <param name="logo">二维码的log</param>
        /// <param name="withInfo">二维码是否携带信息</param>
        /// <param name="info">携带的信息</param>
        /// <returns></returns>
        private static Bitmap AddLogoAndInfoToQRCode(Bitmap src, Bitmap logo, bool withInfo, string info)
        {
            int srcWidth = src.Width;
            int srcHeight = src.Height;
            int logoWidth = logo?.Width ?? 0;
            int logoHeight = logo?.Height ?? 0;

            int resultWidth;
            int resultHeight;
            Bitmap bitmap;

            if (withInfo)
            {
                using (Graphics g = Graphics.FromImage(src))
                {
                    Font font = new Font("Arial", srcHeight * 0.17f, FontStyle.Regular);
                    SizeF textSize = g.MeasureString(info, font);

                    resultWidth = Math.Max(srcWidth, (int)textSize.Width);
                    resultHeight = srcHeight + (int)textSize.Height + 20;

                    bitmap = new Bitmap(resultWidth, resultHeight, PixelFormat.Format32bppArgb);
                    using (Graphics canvas = Graphics.FromImage(bitmap))
                    {
                        canvas.Clear(Color.White);
                        canvas.DrawImage(src, new Rectangle((resultWidth - srcWidth) / 2, 0, srcWidth, srcHeight));
                        using (Brush brush = new SolidBrush(Color.Black))
                        {
                            canvas.DrawString(info, font, brush, (resultWidth - textSize.Width) / 2f, srcHeight + 10);
                        }
                        canvas.Save();
                    }
                }
            }
            else
            {
                resultWidth = srcWidth;
                resultHeight = srcHeight;
                bitmap = new Bitmap(resultWidth, resultHeight, PixelFormat.Format32bppArgb);
                using (Graphics canvas = Graphics.FromImage(bitmap))
                {
                    canvas.DrawImage(src, new Rectangle(0, 0, srcWidth, srcHeight));
                }
            }

            if (logo != null)
            {
                try
                {
                    using (Graphics canvas = Graphics.FromImage(bitmap))
                    {
                        float scale = Math.Min((float)srcWidth / 5f / logoWidth, (float)srcHeight / 5f / logoHeight);
                        int scaledWidth = (int)(logoWidth * scale);
                        int scaledHeight = (int)(logoHeight * scale);

                        using (Bitmap scaledLogo = new Bitmap(logo, new Size(scaledWidth, scaledHeight)))
                        {
                            int logoLeft = (srcWidth - scaledWidth) / 2;
                            int logoTop = (srcHeight - scaledHeight) / 2;
                            canvas.DrawImage(scaledLogo, new Rectangle(logoLeft, logoTop, scaledWidth, scaledHeight));
                        }
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    return src;
                }
            }

            return bitmap;
        }

    }
}
