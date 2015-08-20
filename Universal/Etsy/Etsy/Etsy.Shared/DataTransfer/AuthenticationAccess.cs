using Etsy.Util;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Etsy.DataTransfer
{
    /// <summary>
    /// Authentication-related HTTP functionality
    /// </summary>
    public class AuthenticationAccess
    {
        /// <summary>
        /// Get the temporary OAuth token & secret necessary for the user logging in
        /// </summary>
        /// <returns></returns>
        public static async Task<string> getTemporaryCredentials()
        {
            string baseURL = App.baseURL;
            HttpClient client = new HttpClient();
            OAuthBaseUpdated oauth = new OAuthBaseUpdated();    // authentication helper
            Utility util = new Utility();                       // contains helper functions

            baseURL = string.Format("{0}/oauth/request_token?scope=listings_r transactions_r cart_rw feedback_r profile_r profile_w email_r address_r address_w favorites_rw", App.baseURL);    // base url with permissions

            string nonce = oauth.GenerateNonce();                                                           // nonce
            string timeStamp = oauth.GenerateTimeStamp();                                                   // time stamp

            string callback_url = "http://localhost";   //Uri.EscapeDataString("http://localhost");

            string parametersBase = "&oauth_consumer_key=" + App.key
                                    + "&oauth_nonce=" + nonce
                                    + "&oauth_timestamp=" + timeStamp
                                    + "&oauth_signature_method=HMAC-SHA1"
                                    + "&oauth_callback=" + callback_url // doesn't get used
                                    + "&oauth_version=1.0";

            string signature = oauth.GenerateSignature(                                                     // Signature
                                                        new Uri(baseURL),
                                                        callback_url,
                                                        App.key,
                                                        App.sharedSecret,
                                                        "",
                                                        "",
                                                        "GET",
                                                        timeStamp,
                                                        "",
                                                        nonce,
                                                        out baseURL,
                                                        out parametersBase);


            baseURL = string.Format("{0}?{1}&oauth_signature={2}", baseURL,
                                                                    parametersBase,
                                                                    oauth.UrlEncode(signature));            // final URL

            try
            {
                var ree = await client.GetStringAsync(new Uri(baseURL));
                string response = System.Net.WebUtility.UrlDecode(ree);                                     // Decode response from URL format to string

                // Make use of the response. Get token and token secret
                util.parseOAuthResponse(response,
                                        out App.login_url,
                                        out App.oauth_token,
                                        out App.oauth_token_secret,
                                        out App.oauth_callback_confirmed);
            }
            catch (Exception e)
            {
                string mes = "ERROR: " + e.Message;
                return mes;
            }


            return App.login_url;
        }

        /// <summary>
        /// Request permanent OAuth token credentials
        /// Sign the request with the OAuth token, OAuth token_secret, and OAuth verifier
        /// </summary>
        /// <param name="oauth_verifier"></param>
        /// <returns></returns>
        public static async Task getAccessToken(string oauth_verifier)
        {
            string baseURL = App.baseURL;
            HttpClient client = new HttpClient();
            OAuthBaseUpdated oauth_new = new OAuthBaseUpdated();    // authentication helper
            Utility util = new Utility();                       // contains helper functions

            baseURL = string.Format("{0}/oauth/access_token", App.baseURL);    // base url with permissions

            string nonce = oauth_new.GenerateNonce();                                                           // nonce
            string timeStamp = oauth_new.GenerateTimeStamp();                                                   // time stamp

            string parametersBase = "&oauth_consumer_key=" + App.key
                                    + "&oauth_token=" + App.oauth_token
                                    + "&oauth_nonce=" + nonce
                                    + "&oauth_timestamp=" + timeStamp
                                    + "&oauth_signature_method=HMAC-SHA1"
                                    + "&oauth_verifier=" + oauth_verifier
                                    + "&oauth_version=1.0";

            string signature = oauth_new.GenerateSignature(new Uri(baseURL), "https://localhost", App.key, App.sharedSecret, App.oauth_token, App.oauth_token_secret, "GET", timeStamp, oauth_verifier, nonce, out baseURL, out parametersBase);

            baseURL = string.Format("{0}?{1}&oauth_signature={2}", baseURL,
                                                                    parametersBase,
                                                                    oauth_new.UrlEncode(signature));            // final URL

            try
            {
                var ree = await client.GetStringAsync(new Uri(baseURL));
                string response = System.Net.WebUtility.UrlDecode(ree);                                     // Decode response from URL format to string

                // Make use of the response. Get permanent access token and access token secret
                util.parseAccessTokenResponse(response,
                                                out App.access_token,
                                                out App.access_token_secret);

                // Write the access token and access token secret to storage
                await FileIO.EncryptAndSave(App.access_token, "access_token");                              // encrypt & save to storage
                await FileIO.EncryptAndSave(App.access_token_secret, "access_token_secret");
            }
            catch (Exception e)
            {
                string mes = e.Message;
            }

            return;
        }

        /// <summary>
        /// Add OAuth 1.0 authentication to the GET request
        /// </summary>
        /// <param name="baseURL"></param>
        /// <returns></returns>
        public static string addAuthentication(string baseURL, List<Parameter> parameters)      // "..." is the value, which must be URL encoded
        {
            OAuthBaseUpdated oauth_new = new OAuthBaseUpdated();    // authentication helper

            string nonce = oauth_new.GenerateNonce();                                                           // nonce
            string timeStamp = oauth_new.GenerateTimeStamp();                                                   // time stamp

            string parametersBase = "&oauth_consumer_key=" + App.key
                                    + "&oauth_token=" + App.access_token
                                    + "&oauth_nonce=" + nonce
                                    + "&oauth_timestamp=" + timeStamp
                                    + "&oauth_signature_method=HMAC-SHA1"
                                    + "&oauth_version=1.0";

            string uriString = baseURL + "?";           // build uri that must be placed in signature
            for (int i = 0; i < parameters.Count; i++)
            {
                uriString += parameters[i].key + "=" + Uri.EscapeDataString(parameters[i].value);   // must URL encode parameterValue
                if (i < parameters.Count - 1)
                    uriString += "&";
            }
            string signature = oauth_new.GenerateSignature(new Uri(uriString),    // must URL encode parameterValue
                                                            "", App.key, App.sharedSecret, App.access_token, App.access_token_secret, "GET", timeStamp, "", nonce, out baseURL, out parametersBase);

            baseURL = string.Format("{0}?{1}&oauth_signature={2}", baseURL,
                                                                   parametersBase,
                                                                   oauth_new.UrlEncode(signature));            // final URL

            return baseURL;
        }

        /// <summary>
        /// Add OAuth 1.0 authentication to the specified request type
        /// </summary>
        /// <param name="baseURL"></param>
        /// <returns></returns>
        public static string addAuthentication(string baseURL, List<Parameter> parameters, string REST_method)      // "..." is the value, which must be URL encoded
        {
            if (REST_method != "GET" && REST_method != "POST" && REST_method != "DELETE" && REST_method != "PUT")
                REST_method = "GET";    // default to a GET request

            OAuthBaseUpdated oauth_new = new OAuthBaseUpdated();    // authentication helper

            string nonce = oauth_new.GenerateNonce();                                                           // nonce
            string timeStamp = oauth_new.GenerateTimeStamp();                                                   // time stamp

            string parametersBase =   "&oauth_consumer_key=" + App.key
                                    + "&oauth_token=" + App.access_token
                                    + "&oauth_nonce=" + nonce
                                    + "&oauth_timestamp=" + timeStamp
                                    + "&oauth_signature_method=HMAC-SHA1"
                                    + "&oauth_version=1.0";

            string uriString = baseURL + "?";           // build uri that must be placed in signature
            for (int i = 0; i < parameters.Count; i++)
            {
                uriString += parameters[i].key + "=" + Uri.EscapeDataString(parameters[i].value);   // must URL encode parameterValue
                if (i < parameters.Count - 1)
                    uriString += "&";
            }
            string signature = oauth_new.GenerateSignature(new Uri(uriString),    // must URL encode parameterValue
                                                            "", App.key, App.sharedSecret, App.access_token, App.access_token_secret, REST_method, timeStamp, "", nonce, out baseURL, out parametersBase);

            baseURL = string.Format("{0}?{1}&oauth_signature={2}", baseURL,
                                                                    parametersBase,
                                                                    oauth_new.UrlEncode(signature));            // final URL

            return baseURL;
        }


    }
}
