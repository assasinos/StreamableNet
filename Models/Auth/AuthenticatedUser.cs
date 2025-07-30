using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StreamableNet.Models.Auth
{
    public class AuthenticatedUser
    {
        public object ad_tags { get; set; }
        public bool allow_download { get; set; }
        public string allowed_domain { get; set; }
        public bool beta { get; set; }
        public string bio { get; set; }
        public string color { get; set; }
        public string country { get; set; }
        public object dark_mode { get; set; }
        public double date_added { get; set; }
        public object default_sub { get; set; }
        public bool disable_streamable { get; set; }
        public string email { get; set; }
        public bool email_verified { get; set; }
        public object embed_options { get; set; }
        public object embed_plays { get; set; }
        public bool hide_sharing { get; set; }
        public bool hosting_provider { get; set; }
        public int id { get; set; }
        public string isp { get; set; }
        public bool no_trial { get; set; }
        public object parent { get; set; }
        public bool password_set { get; set; }
        public object payment_processor { get; set; }
        public object photo_url { get; set; }
        public object plan { get; set; }
        public bool plan_annual { get; set; }
        public bool plan_hide_branding { get; set; }
        public int plan_max_length { get; set; }
        public double plan_max_size { get; set; }
        public string plan_name { get; set; }
        public PlanOptions plan_options { get; set; }
        public int plan_plays { get; set; }
        public string plan_price { get; set; }
        public int plan_requests { get; set; }
        public int plays_remaining { get; set; }
        public int privacy { get; set; }
        public PrivacySettings privacy_settings { get; set; }
        public object pro { get; set; }
        public object remove_branding { get; set; }
        public int requests_remaining { get; set; }
        public bool restricted { get; set; }
        public string socket { get; set; }
        public int stale { get; set; }
        public object stream_key { get; set; }
        public object subreddits { get; set; }
        public object subscription_status { get; set; }
        public int total_clips { get; set; }
        public int total_embeds { get; set; }
        public int total_plays { get; set; }
        public int total_uploads { get; set; }
        public int total_videos { get; set; }
        public string twitter { get; set; }
        public string user_name { get; set; }
        public object watermark_link { get; set; }
        public object watermark_url { get; set; }

        public class Annual
        {
            public string paypal_id { get; set; }
            public string paypal_id_notrial { get; set; }
            public string price { get; set; }
            public string stripe_id { get; set; }
        }

        public class Monthly
        {
            public string paypal_id { get; set; }
            public string paypal_id_notrial { get; set; }
            public string price { get; set; }
            public string stripe_id { get; set; }
        }

        public class PlanOptions
        {
            public Annual annual { get; set; }
            public Monthly monthly { get; set; }
        }

        public class PrivacySettings
        {
            public bool allow_download { get; set; }
            public bool allow_sharing { get; set; }
            public string allowed_domain { get; set; }
            public string domain_restrictions { get; set; }
            public bool hide_view_count { get; set; }
            public string visibility { get; set; }
        }
    }

}
