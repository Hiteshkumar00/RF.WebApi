namespace RF.WebApi.Api.Infrastructure.Data.Tables
{
    public class Account
    {
        public int? Id { get; set; }
        public string? ProfileName { get; set; }
        public string? ProfileLogoLink { get; set; }
        public string? Title { get; set; }
        public string? Address { get; set; }
        public string? Phone { get; set; }
        public string? Email { get; set; }
        public string? GSTIN { get; set; }
        public string? CurrencyType { get; set; }
        public string? DateFormat { get; set; }
        public string? ShortDateFormat { get; set; }
        
        public bool EnableSuggestions { get; set; }
        public bool EnableVoiceTyping { get; set; }

        public string? WhatsAppNumber { get; set; }
        public bool EnableWhatsApp { get; set; }
        public bool EnableAdvancedWhatsApp { get; set; }
        public string? WhatsAppPhoneNumberId { get; set; }
        public string? WhatsAppBusinessId { get; set; }
        public string? WhatsAppAccessToken { get; set; }

        public bool EnableEmail { get; set; }
        public string? EmailSmtpHost { get; set; }
        public int? EmailSmtpPort { get; set; }
        public string? EmailSmtpUsername { get; set; }
        public string? EmailSmtpPassword { get; set; }
        public bool EmailSmtpEnableSsl { get; set; }
    }
}
