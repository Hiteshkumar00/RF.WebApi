using System;

namespace RF.WebApi.Api.Infrastructure.Data.Tables
{
    public class Product
    {
        public int? Id { get; set; }
        public int? AccountId { get; set; }
        public string? ProductName { get; set; }
        public string? ImageLink { get; set; }
        public int? WarrantyYear { get; set; }
        public int? WarrantyMonth { get; set; }
        public int? WarrantyDay { get; set; }
    }
}
