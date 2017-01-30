using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Petriss.Utilities
{
    public class CapachaUtility
    {
        public byte[] GetCaptchaImage(string checkCode)
        {

            Bitmap image = new Bitmap(Convert.ToInt32(Math.Ceiling((decimal)(checkCode.Length * 15))), 25);
            Graphics g = Graphics.FromImage(image);
            try
            {
                Random random = new Random();
                g.Clear(Color.AliceBlue);
                Font font = new Font("Comic Sans MS", 14, FontStyle.Bold);
                string str = "";
                System.Drawing.Drawing2D.LinearGradientBrush brush = new System.Drawing.Drawing2D.LinearGradientBrush(new Rectangle(0, 0, image.Width, image.Height), Color.Blue, Color.DarkRed, 1.2f, true);
                for (int i = 0; i < checkCode.Length; i++)
                {
                    str = str + checkCode.Substring(i, 1);
                }
                g.DrawString(str, font, new SolidBrush(Color.Blue), 0, 0);
                g.Flush();
                System.IO.MemoryStream ms = new System.IO.MemoryStream();
                image.Save(ms, System.Drawing.Imaging.ImageFormat.Gif);
                return ms.ToArray();
            }
            finally
            {
                g.Dispose();
                image.Dispose();
            }
        }

        public byte[] VerificationTextGenerator()
        {

            string allChar = "0,1,2,3,4,5,6,7,8,9";
            string[] allCharArray = allChar.Split(',');
            string randomCode = "";
            int temp = -1;
            Random rand = new Random();
            for (int i = 0; i < 4; i++)
            {
                if (temp != -1)
                {
                    rand = new Random(i * temp * ((int)DateTime.Now.Ticks));
                }
                int t = rand.Next(10);
                if (temp != -1 && temp == t)
                {
                    VerificationTextGenerator();
                }
                temp = t;
                randomCode += allCharArray[t];
            }
            GetCaptchaImage(randomCode);
            HttpContext.Current.Session["Captcha"] = randomCode;
            return GetCaptchaImage(randomCode);
        }

    }
}
