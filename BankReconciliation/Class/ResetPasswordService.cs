using System.Collections.Generic;
using System.Net.Http;

namespace BankReconciliation.Class
{
    public class ResetPasswordService
    {
        private HttpServiceManager _httpService;
        public ResetPasswordService(string baseUrl)
        {
            _httpService = new HttpServiceManager(baseUrl);
        }

        public List<ResponseMsgModel> ResetPassword(List<EmailModel> emailModels)
        {
            var serviceUrl = string.Format("Email/sendMultiple");
            var response = _httpService.PostToRemoteService(serviceUrl, emailModels);
            return response.Content.ReadAsAsync<List<ResponseMsgModel>>().Result;
        }

        public ResponseMsgModel ResetPassword(EmailModel emailModels)
        {
            var serviceUrl = string.Format("Email/send");
            var response = _httpService.PostToRemoteService(serviceUrl, emailModels);
            return response.Content.ReadAsAsync<ResponseMsgModel>().Result;
        }
    }
}
