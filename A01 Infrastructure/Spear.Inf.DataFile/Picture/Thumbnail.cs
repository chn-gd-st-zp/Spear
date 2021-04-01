using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace Spear.Inf.Core.DataFile
{
    /// <summary>
    /// 图片处理类
    /// 1、生成缩略图片或按照比例改变图片的大小和画质
    /// 2、将生成的缩略图放到指定的目录下
    /// </summary>
    public class Thumbnail : IDisposable
    {
        public Image ImageSource
        {
            get { return _imageSource; }
        }
        private Image _imageSource;

        private int image_Width;
        private int image_Height;

        /// <summary>
        /// 类的构造函数
        /// </summary>
        /// <param name="filePath">图片文件的全路径名称</param>
        public Thumbnail(string filePath)
        {
            try
            {
                FileStream fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read);

                int byteLength = (int)fileStream.Length;
                byte[] fileBytes = new byte[byteLength];
                fileStream.Read(fileBytes, 0, byteLength);

                //文件流关閉,文件解除锁定
                fileStream.Close();

                _imageSource = Image.FromStream(new MemoryStream(fileBytes));
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// 释放资源
        /// </summary>
        public void Dispose()
        {
            _imageSource.Dispose();
        }

        /// <summary>
        /// 设置回调方法
        /// </summary>
        /// <returns></returns>
        public bool ThumbnailCallback()
        {
            return false;
        }

        /// <summary>
        /// 绘制缩略图
        /// </summary>
        /// <param name="width">缩略图的宽度</param>
        /// <param name="height">缩略图的高度</param>
        /// <returns>缩略图的Image对象</returns>
        public Image Draw(int width, int height)
        {
            try
            {
                image_Width = width;
                image_Height = height;

                return Draw2();
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// 绘制缩略图
        /// </summary>
        /// <param name="percent">缩略图的宽度百分比如：需要百分之80，就填0.8</param>  
        /// <returns>缩略图的Image对象</returns>
        public Image Draw(double percent)
        {
            try
            {
                image_Width = Convert.ToInt32(_imageSource.Width * percent);
                image_Height = Convert.ToInt32(_imageSource.Height * percent);

                return Draw2();
            }
            catch
            {
                throw;
            }
        }

        private Image Draw1()
        {
            Image.GetThumbnailImageAbort callb = new Image.GetThumbnailImageAbort(ThumbnailCallback);

            Image image_Target = _imageSource.GetThumbnailImage(image_Width, image_Height, callb, IntPtr.Zero);

            return image_Target;
        }

        private Image Draw2()
        {
            //用指定的大小和格式初始化Bitmap类的新实例
            Bitmap bitmap = new Bitmap(image_Width, image_Height, PixelFormat.Format32bppArgb);

            //从指定的Image对象创建新Graphics对象
            Graphics graphics = Graphics.FromImage(bitmap);

            //清除整个绘图面并以透明背景色填充
            graphics.Clear(Color.Transparent);

            //在指定位置并且按指定大小绘制原图片对象
            graphics.DrawImage(_imageSource, new Rectangle(0, 0, image_Width, image_Height));

            return bitmap;
        }
    }
}
