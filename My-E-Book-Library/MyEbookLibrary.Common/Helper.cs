using MyEbookLibrary.Common.DTO;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyEbookLibrary.Common
{
    public class Helper
    {
        public static string ToTitleCase(string value)
        {
            TextInfo textInfo = new CultureInfo("en-US", false).TextInfo;
            return textInfo.ToTitleCase(value);
        }

        public static List<T> SortAndPage<T>(List<T> list, int pageSize, int pageNumber, string sortColumnName, Enums.OrderByEnum sortDirection)
        {
            if (string.IsNullOrEmpty(sortColumnName))
            {
                throw new ArgumentNullException("Blank Sort Column Name");
            }
            
            var propertyInfo = typeof(T).GetProperty(sortColumnName);
            var records = sortDirection == Enums.OrderByEnum.Asc ?
                                list.OrderBy(e => propertyInfo?.GetValue(e, null)).Skip(pageSize * pageNumber).Take(pageSize).ToList()
                                :
                                list.OrderByDescending(e => propertyInfo?.GetValue(e, null)).Skip(pageSize * pageNumber).Take(pageSize).ToList();
            return records;
        }

        public static string ListToCommaSeparatedString<T>(IEnumerable<T> list)
        {
            return string.Join(", ", list.Select(s => s.ToString()));
        }

        public static byte[] ResizeImage(byte[] imageFile, Size size)
        {
            Image imgToResize = ByteArrayToImage(imageFile);
            int sourceWidth = imgToResize.Width;
            // Get the image current height
            int sourceHeight = imgToResize.Height;
            float nPercent = 0;
            float nPercentW = 0;
            float nPercentH = 0;
            // Calculate width and height with new desired size
            nPercentW = ((float)size.Width / (float)sourceWidth);
            nPercentH = ((float)size.Height / (float)sourceHeight);
            nPercent = Math.Min(nPercentW, nPercentH);
            // New Width and Height
            int destWidth = (int)(sourceWidth * nPercent);
            int destHeight = (int)(sourceHeight * nPercent);
            Bitmap b = new Bitmap(destWidth, destHeight);
            Graphics g = Graphics.FromImage(b);
            g.InterpolationMode = InterpolationMode.HighQualityBicubic;

            g.DrawImage(imgToResize, 0, 0, destWidth, destHeight);
            g.Dispose();
            return ImageToByteArray(b);
        }

        public static Image ByteArrayToImage(byte[] imageFile)
        {
            using (var ms = new MemoryStream(imageFile))
            {
                return Image.FromStream(ms);
            }
        }
        public static byte[] ImageToByteArray(Image image)
        {
            using (var ms = new MemoryStream())
            {
                image.Save(ms, ImageFormat.Jpeg);
                return ms.ToArray();
            }
        }

        public static bool IsWindowsOs
        {
            get
            {
                var Os = Environment.OSVersion;
                if (Os.Platform == PlatformID.Win32NT)
                {
                    return true;
                }
                return false;
            }
        }
    }
}
