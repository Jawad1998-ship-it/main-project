namespace Donation_Website.Models
{
    public class CartItemViewModel
    {
        public int CartItemID { get; set; }   // match the code and DB query
        public string FundraiserTitle { get; set; }
        public decimal Amount { get; set; }
        public string Status { get; set; }
    }

}
