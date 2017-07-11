using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.ModelBinding;

public static class Utilities
{
    public enum ImageComperssion
    {
        Maximum = 50,
        Good = 60,
        Normal = 70,
        Fast = 80,
        Minimum = 90,
    }

    public static void ResizeImage(this Stream inputStream, int width, int height, string savePath, ImageComperssion ic = ImageComperssion.Normal)
    {
        System.Drawing.Image img = new Bitmap(inputStream);
        System.Drawing.Image result = new Bitmap(width, height, PixelFormat.Format24bppRgb);
        using (Graphics g = Graphics.FromImage(result))
        {
            g.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
            g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
            g.DrawImage(img, 0, 0, width, height);
        }
        result.CompressImage(savePath, ic);
    }

    public static void ResizeImage(this System.Drawing.Image img, int width, int height, string savePath, ImageComperssion ic = ImageComperssion.Normal)
    {
        System.Drawing.Image result = new Bitmap(width, height, PixelFormat.Format24bppRgb);
        using (Graphics g = Graphics.FromImage(result))
        {
            g.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
            g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
            g.DrawImage(img, 0, 0, width, height);
        }
        result.CompressImage(savePath, ic);
    }

    public static void ResizeImageByWidth(this Stream inputStream, int width, string savePath, ImageComperssion ic = ImageComperssion.Normal)
    {
        System.Drawing.Image img = new Bitmap(inputStream);
        int height = img.Height * width / img.Width;
        System.Drawing.Image result = new Bitmap(width, height, PixelFormat.Format24bppRgb);
        using (Graphics g = Graphics.FromImage(result))
        {
            g.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
            g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
            g.DrawImage(img, 0, 0, width, height);
        }
        result.CompressImage(savePath, ic);
    }

    public static void ResizeImageByWidth(this System.Drawing.Image img, int width, string savePath, ImageComperssion ic = ImageComperssion.Normal)
    {
        int height = img.Height * width / img.Width;
        System.Drawing.Image result = new Bitmap(width, height, PixelFormat.Format24bppRgb);
        using (Graphics g = Graphics.FromImage(result))
        {
            g.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
            g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
            g.DrawImage(img, 0, 0, width, height);
        }
        result.CompressImage(savePath, ic);
    }

    public static void ResizeImageByHeight(this Stream inputStream, int height, string savePath, ImageComperssion ic = ImageComperssion.Normal)
    {
        System.Drawing.Image img = new Bitmap(inputStream);
        int width = img.Width * height / img.Height;
        System.Drawing.Image result = new Bitmap(width, height, PixelFormat.Format24bppRgb);
        using (Graphics g = Graphics.FromImage(result))
        {
            g.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
            g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
            g.DrawImage(img, 0, 0, width, height);
        }
        result.CompressImage(savePath, ic);
    }

    public static void ResizeImageByHeight(this System.Drawing.Image img, int height, string savePath, ImageComperssion ic = ImageComperssion.Normal)
    {
        int width = img.Width * height / img.Height;
        System.Drawing.Image result = new Bitmap(width, height, PixelFormat.Format24bppRgb);
        using (Graphics g = Graphics.FromImage(result))
        {
            g.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
            g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
            g.DrawImage(img, 0, 0, width, height);
        }
        result.CompressImage(savePath, ic);
    }

    public static void CompressImage(this System.Drawing.Image img, string path, ImageComperssion ic)
    {
        System.Drawing.Imaging.EncoderParameter qualityParam = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, Convert.ToInt32(ic));
        ImageFormat format = img.RawFormat;
        ImageCodecInfo codec = ImageCodecInfo.GetImageDecoders().FirstOrDefault(c => c.FormatID == format.Guid);
        string mimeType = codec == null ? "image/jpeg" : codec.MimeType;
        ImageCodecInfo jpegCodec = null;
        ImageCodecInfo[] codecs = ImageCodecInfo.GetImageEncoders();
        for (int i = 0; i < codecs.Length; i++)
        {
            if (codecs[i].MimeType == mimeType)
            {
                jpegCodec = codecs[i];
                break;
            }
        }
        EncoderParameters encoderParams = new EncoderParameters(1);
        encoderParams.Param[0] = qualityParam;
        img.Save(path, jpegCodec, encoderParams);
    }

    public static string ToLowerFirst(this string str)
    {
        return str.Substring(0, 1).ToLower() + str.Substring(1);
    }

    public static string GetErrors(this ModelStateDictionary modelState)
    {
        return string.Join("<br />", (from item in modelState
                                      where item.Value.Errors.Any()
                                      select item.Value.Errors[0].ErrorMessage).ToList());
    }

    public static DateTime ToPersianDate(this DateTime dt)
    {
        PersianCalendar pc = new PersianCalendar();
        int year = pc.GetYear(dt);
        int month = pc.GetMonth(dt);
        int day = pc.GetDayOfMonth(dt);
        int hour = pc.GetHour(dt);
        int min = pc.GetMinute(dt);

        return new DateTime(year, month, day, hour, min, 0);
    }

    public static string ToPersianDateString(this DateTime dt)
    {
        return dt.ToPersianDate().ToString("yyyy/MM/dd");
    }

    public static DateTime ToMiladiDate(this DateTime dt)
    {
        PersianCalendar pc = new PersianCalendar();
        return pc.ToDateTime(dt.Year, dt.Month, dt.Day, dt.Hour, dt.Minute, 0, 0);
    }

    public static string Encrypt(this string str)
    {
        string EncrptKey = "2013;[pnuLIT)WebCodeExpert";
        byte[] byKey = { };
        byte[] IV = { 18, 52, 86, 120, 144, 171, 205, 239 };
        byKey = System.Text.Encoding.UTF8.GetBytes(EncrptKey.Substring(0, 8));
        DESCryptoServiceProvider des = new DESCryptoServiceProvider();
        byte[] inputByteArray = Encoding.UTF8.GetBytes(str);
        MemoryStream ms = new MemoryStream();
        CryptoStream cs = new CryptoStream(ms, des.CreateEncryptor(byKey, IV), CryptoStreamMode.Write);
        cs.Write(inputByteArray, 0, inputByteArray.Length);
        cs.FlushFinalBlock();
        return Convert.ToBase64String(ms.ToArray());
    }

    public static string Decrypt(this string str)
    {
        str = str.Replace(" ", "+");
        string DecryptKey = "2013;[pnuLIT)WebCodeExpert";
        byte[] byKey = { };
        byte[] IV = { 18, 52, 86, 120, 144, 171, 205, 239 };
        byte[] inputByteArray = new byte[str.Length];

        byKey = System.Text.Encoding.UTF8.GetBytes(DecryptKey.Substring(0, 8));
        DESCryptoServiceProvider des = new DESCryptoServiceProvider();
        inputByteArray = Convert.FromBase64String(str.Replace(" ", "+"));
        MemoryStream ms = new MemoryStream();
        CryptoStream cs = new CryptoStream(ms, des.CreateDecryptor(byKey, IV), CryptoStreamMode.Write);
        cs.Write(inputByteArray, 0, inputByteArray.Length);
        cs.FlushFinalBlock();
        System.Text.Encoding encoding = System.Text.Encoding.UTF8;
        return encoding.GetString(ms.ToArray());
    }

    public static bool IsImage(this string filename)
    {
        return (filename.EndsWith(".png") || filename.EndsWith(".jpg") || filename.EndsWith(".gif") || filename.EndsWith(".bmp"));
    }

    public static string ToFarsiString(this string str)
    {
        return str.Replace("ئ", "ی").Replace("ك", "ک");
    }

    public static bool IsRequiredStringInputsValid(params string[] inputs)
    {
        for (int i = 0; i < inputs.Length; i++)
        {
            if (string.IsNullOrEmpty(inputs[i]))
                return false;

            if (inputs[i].Trim() == string.Empty)
                return false;
        }
        return true;
    }

    public static bool IsNumeric(this string text)
    {
        try
        {
            int.Parse(text);
            return true;
        }
        catch
        {
            return false;
        }
    }
}
