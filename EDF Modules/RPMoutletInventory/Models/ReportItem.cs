namespace RPMoutletInventory.Models
{
    public class ReportItem
    {
        public string ItemID { get; set; }
        public string Status { get; set; }
        public string ErrorCode { get; set; }
        public string ErrorMessage { get; set; }

        public override string ToString()
        {
            if (Status == "Success")
            {
                return string.Format("ItemID: {0}, Status: {1}", ItemID, Status);
            }
            return string.Format("ItemID: {0},\nStatus: {1},\nErrorCode: {2},\nErrorMessage: {3}", ItemID, Status, ErrorCode, ErrorMessage);
        }
    }
}
