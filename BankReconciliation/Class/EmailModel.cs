namespace BankReconciliation.Class
{
    public class EmailModel
    {
        public EmailSendType EmailType { get; set; }
        public string AuthKeys { get; set; }
        public string UserID { get; set; }
        //public string Username { get; set; }
        public string Token { get; set; }
        public string EmailAddress { get; set; }
        public string StateCode { get; set; }
    }


    public enum EmailSendType
    {
        PasswordReset
    }

    public class ResponseMsgModel
    {
        public string UserID { get; set; }
        public bool ResponseStatus { get; set; }
        public string ResponseMsg { get; set; }
    }


}
