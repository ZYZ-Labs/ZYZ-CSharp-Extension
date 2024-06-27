using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZXing;
using ZXing.Common;
using ZXing.QrCode.Internal;
using ZYZ_CSharp_Extension.Core;

namespace ZYZ_CSharp_Extension.ZXingCode
{
    public static class BarCodeUtils
    {
        /// <summary>
        /// 推测的条形码类型
        /// </summary>
        public static List<BarcodeFormat> BarcodeFormatList = new List<BarcodeFormat>();
        /// <summary>
        /// 默认条形码格式
        /// </summary>
        public static BarcodeFormat DefaultBarcodeFormat = BarcodeFormat.CODE_128;

        public static readonly Color DEFAULT_COLOR = Color.Black;


        static BarCodeUtils()
        {
            BarcodeFormatList.Add(BarcodeFormat.CODE_39);
            BarcodeFormatList.Add(BarcodeFormat.CODE_93);
            BarcodeFormatList.Add(BarcodeFormat.CODE_128);
            BarcodeFormatList.Add(BarcodeFormat.EAN_8);
            BarcodeFormatList.Add(BarcodeFormat.EAN_13);
        }


        /// <summary>
        /// 设置默认条形码格式
        /// </summary>
        /// <param name="barcodeFormat"></param>
        public static void SetBarcodeFormat(BarcodeFormat barcodeFormat)
        {
            DefaultBarcodeFormat = barcodeFormat;
        }


        /// <summary>
        /// 根据bitmap读取条形码
        /// </summary>
        /// <param name="bitmap"></param>
        /// <returns></returns>
        public static string ReadBarCaode(Bitmap bitmap)
        {
            byte[] pixels = BitmapUtils.GetBitmapBytes(bitmap);
            RGBLuminanceSource luminanceSource = new RGBLuminanceSource(pixels, bitmap.Width, bitmap.Height);
            BinaryBitmap binaryBitmap = new BinaryBitmap(new HybridBinarizer(luminanceSource));
            MultiFormatReader multiFormatReader = new MultiFormatReader();
            Dictionary<DecodeHintType, object> hints = new Dictionary<DecodeHintType, object>();
            hints.Add(DecodeHintType.POSSIBLE_FORMATS, BarcodeFormatList);
            multiFormatReader.Hints = hints;
            Result decodeResult = multiFormatReader.decode(binaryBitmap);
            return decodeResult.Text;
        }


        /// <summary>
        /// 生成条形码
        /// </summary>
        /// <param name="info">条形码信息</param>
        /// <param name="width">条形码宽度</param>
        /// <param name="height">条形码高度</param>
        /// <param name="color">条形码颜色</param>
        /// <param name="withInfo">是否生成图像携带条形码信息</param>
        /// <returns></returns>
        public static Bitmap CreateBarcode(string info, int width, int height, Color color = default(Color), bool withInfo = false)
        {
            if (color == default(Color))
            {
                color = DEFAULT_COLOR;
            }

            Dictionary<EncodeHintType, object> hint = new Dictionary<EncodeHintType, object>();
            hint.Add(EncodeHintType.MARGIN, 0);
            hint.Add(EncodeHintType.ERROR_CORRECTION, ErrorCorrectionLevel.H);
            return BitMatrixToBitmap(new MultiFormatWriter()
                .encode(CharsetChangeUtils.ChangeUtf8ToISO88591(info),
                DefaultBarcodeFormat, width, height, hint),
                color, withInfo, info);
        }

        /// <summary>
        /// byte矩阵转图片
        /// </summary>
        /// <param name="bitMatrix"></param>
        /// <param name="color"></param>
        /// <param name="withInfo"></param>
        /// <param name="info"></param>
        /// <returns></returns>
        private static Bitmap BitMatrixToBitmap(BitMatrix bitMatrix, Color color, bool withInfo, string info)
        {
            int width = bitMatrix.Width;
            int height = bitMatrix.Height;
            int[] pixels = new int[width * height];
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    pixels[y * width + x] = bitMatrix[x, y] ? (color.ToArgb() == Color.White.ToArgb() ? Color.Black.ToArgb() : color.ToArgb()) : Color.White.ToArgb();
                }
            }

            Bitmap bitmap = new Bitmap(width, height, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    bitmap.SetPixel(x, y, Color.FromArgb(pixels[y * width + x]));
                }
            }

            return AddInfoToBarcode(bitmap, withInfo, info);
        }

        /// <summary>
        /// 添加信息文本到条形码图片上
        /// </summary>
        /// <param name="src"></param>
        /// <param name="withInfo"></param>
        /// <param name="info"></param>
        /// <returns></returns>
        private static Bitmap AddInfoToBarcode(Bitmap src, bool withInfo, string info)
        {
            if (!withInfo)
            {
                return src;
            }

            int srcWidth = src.Width;
            int srcHeight = src.Height;

            using (Graphics g = Graphics.FromImage(src))
            {
                Font font = new Font("Arial", srcHeight * 0.17f, FontStyle.Regular);
                SizeF textSize = g.MeasureString(info, font);

                int resultWidth = Math.Max(srcWidth, (int)textSize.Width);
                int resultHeight = srcHeight + (int)textSize.Height + 20;

                Bitmap bitmap = new Bitmap(resultWidth, resultHeight, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
                using (Graphics canvas = Graphics.FromImage(bitmap))
                {
                    canvas.Clear(Color.White);
                    canvas.DrawImage(src, (resultWidth - srcWidth) / 2f, 0);
                    using (Brush brush = new SolidBrush(Color.Black))
                    {
                        canvas.DrawString(info, font, brush, (resultWidth - textSize.Width) / 2f, srcHeight + 10);
                    }
                    canvas.Save();
                }

                return bitmap;
            }
        }
    }
}
