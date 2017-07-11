using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace UnionManager.Controllers
{
    public class CaptchaController : Controller
    {
        public ActionResult GetCaptchaImage(string prefix, bool noisy = false)
        {
            var rand = new Random((int)DateTime.Now.Ticks);
            //generate new question
            int a = rand.Next(1, 9);
            int b = rand.Next(1, 9);
            var captcha = string.Format("{0} + {1} = ?", a, b);

            //store answer
            Session["CAPTCHA" + prefix] = a + b;

            //image stream
            FileContentResult img = null;

            using (var mem = new MemoryStream())
            using (var bmp = new Bitmap(130, 30))
            using (var gfx = Graphics.FromImage((Image)bmp))
            {
                gfx.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;
                gfx.SmoothingMode = SmoothingMode.AntiAlias;
                gfx.FillRectangle(Brushes.SeaShell, new Rectangle(0, 0, bmp.Width, bmp.Height));

                //add noise
                if (noisy)
                {
                    int i, r, x, y;
                    var pen = new Pen(Color.Yellow);
                    for (i = 1; i < 10; i++)
                    {
                        pen.Color = Color.FromArgb(
                        (rand.Next(0, 255)),
                        (rand.Next(0, 255)),
                        (rand.Next(0, 255)));

                        r = rand.Next(0, (130 / 3));
                        x = rand.Next(0, 130);
                        y = rand.Next(0, 30);

                        gfx.DrawEllipse(pen, x - r, y - r, r, r);
                    }
                }

                //add question
                gfx.DrawString(captcha, new Font("Tahoma", 15), Brushes.CadetBlue, 2, 3);

                //render as Jpeg
                bmp.Save(mem, System.Drawing.Imaging.ImageFormat.Png);
                img = this.File(mem.GetBuffer(), "image/Png");
            }

            return img;
        }








        //public FileResult GetCaptchaImage()
        //{
        //    Session["CAPTCHA"] = GetRandomText().ToLower();
        //    string text = Session["CAPTCHA"].ToString();

        //    //first, create a dummy bitmap just to get a graphics object
        //    Image img = new Bitmap(1, 1);
        //    Graphics drawing = Graphics.FromImage(img);

        //    Font font = new Font("Arial", 15);
        //    //measure the string to see how big the image needs to be
        //    SizeF textSize = drawing.MeasureString(text, font);

        //    //free up the dummy image and old graphics object
        //    img.Dispose();
        //    drawing.Dispose();

        //    //create a new image of the right size
        //    img = new Bitmap((int)textSize.Width + 40, (int)textSize.Height + 20);
        //    drawing = Graphics.FromImage(img);

        //    Color backColor = Color.SeaShell;
        //    Color textColor = Color.CadetBlue;
        //    //paint the background
        //    drawing.Clear(backColor);

        //    //create a brush for the text
        //    Brush textBrush = new SolidBrush(textColor);

        //    drawing.DrawString(text, font, textBrush, 20, 10);

        //    drawing.Save();

        //    font.Dispose();
        //    textBrush.Dispose();
        //    drawing.Dispose();

        //    MemoryStream ms = new MemoryStream();
        //    img.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
        //    img.Dispose();

        //    return File(ms.ToArray(), "image/png");
        //}

        //private string GetRandomText()
        //{
        //    StringBuilder randomText = new StringBuilder();
        //    string alphabets = "012345679ACEFGHKLMNPRSWXZabcdefghijkhlmnopqrstuvwxyz";
        //    Random r = new Random();
        //    for (int j = 0; j <= 5; j++)
        //    {
        //        randomText.Append(alphabets[r.Next(alphabets.Length)]);
        //    }
        //    return randomText.ToString();
        //}
    }
}