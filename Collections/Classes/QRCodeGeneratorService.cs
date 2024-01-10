using QRCoder;
using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Tripous.Forms;

namespace Collection.Classes
{
    public class QRCodeGeneratorService : IQRCodeGenerator
    {
        public Response<QrCodeResponseData> GenerateQRCode(string rawString)
        {
            Response<QrCodeResponseData> result = new Response<QrCodeResponseData>();

            try
            {
                // Create a QR code generator
                QRCodeGenerator qrGenerator = new QRCodeGenerator();
                QRCodeData qrCodeData = qrGenerator.CreateQrCode(rawString, QRCodeGenerator.ECCLevel.Q, forceUtf8: true, utf8BOM: true);

                // Create a QR code image from the data
                QRCode qrCode = new QRCode(qrCodeData);
                Bitmap qrCodeImage = qrCode.GetGraphic(20);

                // Add a text overlay to the QR code image
                using (var g = Graphics.FromImage(qrCodeImage))
                using (var font = new Font(FontFamily.GenericMonospace, 12))
                using (var brush = new SolidBrush(Color.Black))
                using (var format = new StringFormat() { Alignment = StringAlignment.Center })
                {
                    int margin = 3, textHeight = 50;
                    var rect = new RectangleF(margin, 50, qrCodeImage.Width - 2 * margin, textHeight);
                    g.DrawString("Click on the generated Image to open in the browser", font, brush, rect, format);
                }

                // Convert the QR code image to base64 (asynchronously)
                var qrBit = Convert.ToBase64String(BitmapToBytes(qrCodeImage));
                var response = new QrCodeResponseData { Base64QrImage = qrBit };
                result = new Response<QrCodeResponseData>
                {
                    Succeeded = true,
                    Data = response,
                    Message = "Qr code generated successfully",
                };
            }
            catch (Exception ex)
            {
                //_logger.LogError(ex.Message, ex);
                //throw new ApiException(ex.Message);
            }

            return result;
        }

        public string RenderQrCode(string rawString)
        {
            var base64QrCode = "";
            string level = "Q";
            QRCodeGenerator.ECCLevel eccLevel = (QRCodeGenerator.ECCLevel)(level == "L" ? 0 : level == "M" ? 1 : level == "Q" ? 2 : 3);
            using (QRCodeGenerator qrGenerator = new QRCodeGenerator())
            using (QRCodeData qrCodeData = qrGenerator.CreateQrCode(rawString, eccLevel))
            using (QRCode qrCode = new QRCode(qrCodeData))
            {
               base64QrCode = qrCode.ToString();
            }

            return base64QrCode;
        }

        //Helper method to convert a Bitmap to a byte array
        private byte[] BitmapToBytes(Bitmap bitmap)
        {
            using (var stream = new MemoryStream())
            {
                bitmap.Save(stream, ImageFormat.Png);
                return stream.ToArray();
            }
        }
    }
}
