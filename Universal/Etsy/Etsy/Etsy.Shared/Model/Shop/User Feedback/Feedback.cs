using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace Etsy.Model.Shop.User_Feedback
{
    /// <summary>
    /// Container for feedback request. Results is a list of the actual feedback objects
    /// </summary>
    public class Feedback
    {
        public int count { get; set; }
        public ObservableCollection<TransactionFeedback> results { get; set; }  // list of actual feedback
        public Etsy.Model.Shop.User_Feedback.Params @params { get; set; }
        public string type { get; set; }
        public Etsy.Model.Shop.User_Feedback.Pagination pagination { get; set; }
    }

    public class TransactionFeedback
    {
        public Nullable<int> feedback_id { get; set; }
        public Nullable<int> creation_tsz { get; set; }
        public string message { get; set; }
        public Nullable<int> value { get; set; }
        public Nullable<int> seller_user_id { get; set; }
        public Nullable<int> transaction_id { get; set; }
        public Nullable<Int32> image_feedback_id { get; set; }
        public string image_url_25x25 { get; set; }
        public string image_url_155x125 { get; set; }
        public string image_url_fullxfull { get; set; }
        public Nullable<int> target_user_id { get; set; }

        // More info to be obtained after initial deserialization

        public string buyer_name { get; set; }
        public Listing Listing { get; set; }    // associated listing for this transaction
        public string item_photo_url { get; set; }  // image url obtained from the listing
        public string title { get; set; }           // the title for the product listing
    }

    /// <summary>
    /// Part of the feedback request's response
    /// </summary>
    public class Params
    {
        public string user_id { get; set; }
        public int limit { get; set; }
        public int offset { get; set; }
        public object page { get; set; }
    }

    /// <summary>
    /// Part of the feedback request's response
    /// </summary>
    public class Pagination
    {
        public int effective_limit { get; set; }
        public int effective_offset { get; set; }
        public object next_offset { get; set; }
        public int effective_page { get; set; }
        public object next_page { get; set; }
    }

}
