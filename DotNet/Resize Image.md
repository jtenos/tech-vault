```csharp
// This will resize image to be 1024 height or width (whichever is bigger). Explore other options to look for things like image quality.
/*******
using (var image = Image.FromFile(file.FullName))
{
    int newHeight, newWidth;
    if (image.Height > image.Width)
    {
        newHeight = 1024;
        newWidth = (int)(1024.0 * image.Width / image.Height);
    }
    else
    {
        newWidth = 1024;
        newHeight = (int)(1024.0 * image.Height / image.Width);
    }
    using (var newImage = new Bitmap(image, new Size(newWidth, newHeight)))
    {
        newImage.Save(Path.Combine(outputDir.FullName, file.Name), ImageFormat.Jpeg);
    }
}
*********/

// A more complete example:

using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 
using System.Drawing; 
using System.Drawing.Imaging; 
 
 
namespace ConsoleApplication8 
{ 
    /// <summary> 
    /// http://social.msdn.microsoft.com/Forums/en-US/csharpgeneral/thread/a43fe625-a5cd-4438-895f-3ddeec6eb866 
    /// </summary> 
    class Program 
    { 
        static void Main(string[] args) 
        { 
            var image = Image.FromFile(@"C:\Users\ENOSJ\Downloads\orig.jpg"); 
            foreach (int width in new[] { 100, 200, 300, 400 }) 
            { 
                //var newImage = (Image)image.Clone(); 
                var scale = (decimal)image.Width / image.Height; 
                int newWidth = width; 
                int newHeight = (int)(newWidth / scale); 
                //var thumbnail = newImage.GetThumbnailImage(newWidth, newHeight, () => true, IntPtr.Zero); 
                //newImage.PhysicalDimension.Height = newHeight; 
                var newImage = ResizeImage(image, newWidth, newHeight); 
                SaveJpeg(string.Format(@"C:\temp\thumbnail-{0}-25.jpg", width), newImage, 25); 
                SaveJpeg(string.Format(@"C:\temp\thumbnail-{0}-50.jpg", width), newImage, 50); 
                SaveJpeg(string.Format(@"C:\temp\thumbnail-{0}-75.jpg", width), newImage, 75); 
                SaveJpeg(string.Format(@"C:\temp\thumbnail-{0}-100.jpg", width), newImage, 100); 
                //newImage.Save(@"C:\temp\thumbnail.jpg", ImageFormat.Jpeg); 
                newImage.Save(string.Format(@"C:\temp\thumbnail-{0}.png", width), ImageFormat.Png); 
            } 
        } 
 
 
        public static System.Drawing.Bitmap ResizeImage(System.Drawing.Image image, int width, int height) 
        { 
            //a holder for the result  
            Bitmap result = new Bitmap(width, height); 
 
 
            //use a graphics object to draw the resized image into the bitmap  
            using (Graphics graphics = Graphics.FromImage(result)) 
            { 
                //set the resize quality modes to high quality  
                graphics.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality; 
                graphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic; 
                graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality; 
                //draw the image into the target bitmap  
                graphics.DrawImage(image, 0, 0, result.Width, result.Height); 
            } 
 
 
            //return the resulting bitmap  
            return result; 
        } 
 
 
        public static void SaveJpeg(string path, Image image, int quality) 
        { 
            //ensure the quality is within the correct range  
            if ((quality < 0) || (quality > 100)) 
            { 
                //create the error message  
                string error = string.Format("Jpeg image quality must be between 0 and 100, with 100 being the highest quality.  A value of {0} was specified.", quality); 
                //throw a helpful exception  
                throw new ArgumentOutOfRangeException(error); 
            } 
 
 
            //create an encoder parameter for the image quality  
            EncoderParameter qualityParam = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, quality); 
            //get the jpeg codec  
            ImageCodecInfo jpegCodec = GetEncoderInfo("image/jpeg"); 
 
 
            //create a collection of all parameters that we will pass to the encoder  
            EncoderParameters encoderParams = new EncoderParameters(1); 
            //set the quality parameter for the codec  
            encoderParams.Param[0] = qualityParam; 
            //save the image using the codec and the parameters  
            image.Save(path, jpegCodec, encoderParams); 
        } 
 
 
        public static ImageCodecInfo GetEncoderInfo(string mimeType) 
        { 
            //do a case insensitive search for the mime type  
            string lookupKey = mimeType.ToLower(); 
 
 
            //the codec to return, default to null  
            ImageCodecInfo foundCodec = null; 
 
 
            //if we have the encoder, get it to return  
            if (Encoders.ContainsKey(lookupKey)) 
            { 
                //pull the codec from the lookup  
                foundCodec = Encoders[lookupKey]; 
            } 
 
 
            return foundCodec; 
        } 
 
 
        /// <summary>  
        /// A quick lookup for getting image encoders  
        /// </summary>  
        private static Dictionary<string, ImageCodecInfo> encoders = null; 
 
 
        /// <summary>  
        /// A quick lookup for getting image encoders  
        /// </summary>  
        public static Dictionary<string, ImageCodecInfo> Encoders 
        { 
            //get accessor that creates the dictionary on demand  
            get 
            { 
                //if the quick lookup isn't initialised, initialise it  
                if (encoders == null) 
                { 
                    encoders = new Dictionary<string, ImageCodecInfo>(); 
                } 
 
 
                //if there are no codecs, try loading them  
                if (encoders.Count == 0) 
                { 
                    //get all the codecs  
                    foreach (ImageCodecInfo codec in ImageCodecInfo.GetImageEncoders()) 
                    { 
                        //add each codec to the quick lookup  
                        encoders.Add(codec.MimeType.ToLower(), codec); 
                    } 
                } 
 
 
                //return the lookup  
                return encoders; 
            } 
        }  
    } 
} 
```