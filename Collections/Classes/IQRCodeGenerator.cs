using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Collection.Classes
{
    public interface IQRCodeGenerator
    {
        Response<QrCodeResponseData> GenerateQRCode(string rawString);
    }
}
