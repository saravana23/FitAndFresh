using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FitAndFresh.Utility
{
    public class StaticDetails
    {
        public const string FoodIcon = "food_icon";
        public const string ManagementAccount = "Manager";
        public const string KitchenAccount = "Kitchen";
        public const string ReceptionAccount = "Reception";
        public const string CustomerAccount = "Customer";

        public const string SubmittedStatus = "Order Submitted";
        public const string InProcessStatus = "In the kitchen";
        public const string ReadyStatus = "Ready for collection";
        public const string CompletedStatus = "Completed";
        public const string CancelledStatus = "Cancelled";

        public const string PaymentPendingStatus = "Pending";
        public const string PaymentAcceptedStatus = "Accepted";
        public const string PaymentDeclinedStatus = "Declined";
    }
}
