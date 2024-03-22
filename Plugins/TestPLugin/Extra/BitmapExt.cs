using System.Drawing;
using System.Drawing.Imaging;
using Avalonia;
using Avalonia.Platform;
using Bitmap = Avalonia.Media.Imaging.Bitmap;
using PixelFormat = System.Drawing.Imaging.PixelFormat;
using S_Bitmap = System.Drawing.Bitmap;

public static class ImageExtensions
{
    public static Bitmap ConvertToAvaloniaBitmap(this S_Bitmap bitmapTmp)
    {
        var bitmapdata = bitmapTmp.LockBits(new Rectangle(0, 0, bitmapTmp.Width, bitmapTmp.Height),
            ImageLockMode.ReadWrite, PixelFormat.Format32bppArgb);
        var bitmap1 = new Bitmap(Avalonia.Platform.PixelFormat.Bgra8888, AlphaFormat.Premul,
            bitmapdata.Scan0,
            new PixelSize(bitmapdata.Width, bitmapdata.Height),
            new Vector(96, 96),
            bitmapdata.Stride);
        bitmapTmp.UnlockBits(bitmapdata);
        bitmapTmp.Dispose();
        return bitmap1;
    }

    public static Bitmap ConvertToAvaloniaBitmap(this Icon ico)
    {
        var bmp = S_Bitmap.FromHicon(ico.Handle);
        var bitmapTmp = ico.ToBitmap();
        var bitmapdata = bitmapTmp.LockBits(new Rectangle(0, 0, bitmapTmp.Width, bitmapTmp.Height),
            ImageLockMode.ReadWrite, PixelFormat.Format32bppArgb);
        var bitmap1 = new Bitmap(Avalonia.Platform.PixelFormat.Bgra8888, AlphaFormat.Premul,
            bitmapdata.Scan0,
            new PixelSize(bitmapdata.Width, bitmapdata.Height),
            new Vector(96, 96),
            bitmapdata.Stride);
        bitmapTmp.UnlockBits(bitmapdata);
        bitmapTmp.Dispose();
        return bitmap1;
    }
}