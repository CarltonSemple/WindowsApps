using System;
using System.Collections.Generic;
using System.Text;
using Windows.Security.Cryptography;
using Windows.Security.Cryptography.Core;
using Windows.Storage.Streams;

namespace Etsy.Util
{
    /// <summary>
    /// Contains miscellaneous functions extracted to make other code look cleaner
    /// </summary>
    public class Utility
    {
        /// <summary>
        /// Parse the the OAuth token request response
        /// </summary>
        /// <param name="response"></param>
        /// <param name="login_url"></param>
        /// <param name="oauth_token"></param>
        /// <param name="oauth_token_secret"></param>
        /// <param name="oauth_callback_confirmed"></param>
        public void parseOAuthResponse( string response, 
                                        out string login_url,
                                        out string oauth_token,
                                        out string oauth_token_secret,
                                        out bool oauth_callback_confirmed)
        {
            string[] urlAndRestOfResponse = response.Split('?');
            // login url
            login_url = urlAndRestOfResponse[0];
                string[] temp = login_url.Split('=');
                login_url = temp[1];

            // the others
            oauth_token = "oauth_token="; oauth_token_secret = "oauth_token_secret"; oauth_callback_confirmed = false; // initial values

            string[] varis = urlAndRestOfResponse[1].Split('&');
            
            for(int i = 0; i < varis.Length; i++)   // oauth_token
                if(varis[i].Contains(oauth_token))
                {
                    string[] tem = varis[i].Split('=');
                    oauth_token = tem[1];
                    break;
                }

            for (int i = 0; i < varis.Length; i++)   // oauth_token_secret
                if (varis[i].Contains(oauth_token_secret))
                {
                    string[] tem = varis[i].Split('=');
                    oauth_token_secret = tem[1];
                }

            for (int i = 0; i < varis.Length; i++)   // oauth_callback_confirmed
            {
                if (varis[i].Contains("true"))
                    oauth_callback_confirmed = true;
                else if (varis[i].Contains("false"))
                    oauth_callback_confirmed = false;
                }
            }

        /// <summary>
        /// Do a simple parsing of the response, which comes in the simple form: oauth_token=xxx&oauth_token_secret=yyy
        /// </summary>
        /// <param name="response"></param>
        /// <param name="access_token"></param>
        /// <param name="access_token_secret"></param>
        public void parseAccessTokenResponse(   string response,
                                                out string access_token,
                                                out string access_token_secret)
        {
            access_token = "";  access_token_secret = "";

            string[] values = response.Split('&');
            
            foreach(string v in values)
            {
                if (v.Contains("oauth_token="))
                    access_token = v.Replace("oauth_token=", "");
                else if (v.Contains("oauth_token_secret="))
                    access_token_secret = v.Replace("oauth_token_secret=", "");
            }

            foreach (string v in values)
            {
                if (v.Contains("oauth_token="))
                    access_token = v.Replace("oauth_token=", "");
                else if (v.Contains("oauth_token_secret="))
                    access_token_secret = v.Replace("oauth_token_secret=", "");
            }
        }

    }
    
}
