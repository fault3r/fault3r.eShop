
using Microsoft.AspNetCore.Http;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace fault3r_Common
{
    public class ImageResizer
    {     
        public static MemoryStream ResizeImage(IFormFile picture, int width, int height)
        {
            MemoryStream picStream = new MemoryStream();
            MemoryStream orgPicStream = new();
            picture.CopyTo(orgPicStream);
            Bitmap picBitmap = new Bitmap(Image.FromStream(orgPicStream), new Size(width, height));            
            picBitmap.Save(picStream, ImageFormat.Png);
            picBitmap.Dispose();
            orgPicStream.Close();
            picStream.Close();
            return picStream;
        }
    }
}
