using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Collection.Classes
{
    public class BarCode
    {
        public async Task<string> GenerateConfirmDocBarcode(string urlpath)
        {
            var sources = string.Empty;

            const string serviceMethod = "/barcode/api/v1/QrCoder/encode-string";

            var BarcodeServiceUrl = System.Configuration.ConfigurationManager.AppSettings["BaseURL"];

            try
            {
                var url = $"{BarcodeServiceUrl}{serviceMethod}";

                if (!string.IsNullOrEmpty(urlpath))
                {
                    var barCodeRequest = new BarCodeRequest
                    {
                        rawString = "123659774"// urlpath
                    };
                    var _httpClient = new HttpClient();
                    _httpClient.DefaultRequestHeaders.Accept.Clear();
                    _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    _httpClient.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/x-www-form-urlencoded; charset=utf-8");
                    var contents = new StringContent(JsonConvert.SerializeObject(barCodeRequest), Encoding.UTF8, "application/json");
                    var response = await _httpClient.PostAsync(url, contents);
                    if (response.IsSuccessStatusCode)
                    {
                        string apiResponse = await response.Content.ReadAsStringAsync();
                        BarcodeResponse myDeserializedClass = JsonConvert.DeserializeObject<BarcodeResponse>(apiResponse);
                        //if(_appSettings.ServiceBaseUrl == "http://localhost/PayValueWebApiCore")
                        //{
                        //    sources = "https://liveservices.ogunstaterevenue.com/barcode/Upload/0e6f13e5-d.jpg";
                        //    return sources;
                        //}
                        sources = myDeserializedClass.data.qrImageUrl;
                        return sources;
                    }

                    return sources;
                }

            }
            catch (Exception)
            {

                throw;
            }

            return sources;

        }


    }


    public class BarCodeRequest
    {
        public string rawString { get; set; }
    }

    public class BarcodeResponseData
    {
        public string qrImageUrl { get; set; }
        public string base64QrImage { get; set; }
    }
    public class BarcodeResponse
    {
        public bool succeeded { get; set; }
        public string message { get; set; }
        public List<string> errors { get; set; }
        public BarcodeResponseData data { get; set; }
    }
}
