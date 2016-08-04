using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;
using System.Runtime.Serialization.Json;
using System.Runtime.Serialization;
using System.Web;
using WikiDigger.Translator;
using System.ServiceModel.Channels;
using System.ServiceModel;
using System.Threading;
using System.Text.RegularExpressions;



namespace WikiDigger
{
    public static class MicrosoftTranslatePlugIn
    {


        public static String GetTokenWrapper()
        {
            AdmAccessToken admToken;

            AdmAuthentication admAuth = new AdmAuthentication("wikiSalience", "6lMqUKH1+7cQjncrYMIx00BD7HtYKi3KlQ+DxbUTuoM=");
            admToken = admAuth.GetAccessToken();
            return "Bearer " + admToken.access_token;


        }

        public static String Translate(HttpRequestMessageProperty httpRequestProperty,string authToken, string what, string to)
        {
            // Add TranslatorService as a service reference, Address:http://api.microsofttranslator.com/V2/Soap.svc
            LanguageServiceClient client = new LanguageServiceClient();
            //Set Authorization header before sending the request
            //HttpRequestMessageProperty httpRequestProperty = new HttpRequestMessageProperty();
            //httpRequestProperty.Method = "POST";
            //httpRequestProperty.Headers.Add("Authorization", authToken);
            // Creates a block within which an OperationContext object is in scope.
            // to = "en";

            using (OperationContextScope scope = new OperationContextScope(client.InnerChannel))
            {
                OperationContext.Current.OutgoingMessageProperties[HttpRequestMessageProperty.Name] = httpRequestProperty;
                //string sourceText = "<UL><LI>Use generic class names. <LI>Use pixels to express measurements for padding and margins. <LI>Use percentages to specify font size and line height. <LI>Use either percentages or pixels to specify table and container width.   <LI>When selecting font families, choose browser-independent alternatives.   </LI></UL>";
                string sourceText = what;

                //string sourceText = "A cat ate a dog";

                string translationResult;
                //Keep appId parameter blank as we are sending access token in authorization header.

                try { translationResult = client.Translate("", sourceText, "", to, "text/plain", "general", ""); }
                catch(Exception ex) { return ex.ToString(); }
                //Console.OutputEncoding = Encoding.UTF8;
                //Console.WriteLine("Translation for source {0} from {1} to {2} is", sourceText, "ru", "en");
                //Console.WriteLine(translationResult);
                //Console.WriteLine("Press any key to continue...");
                //Console.ReadKey(true);
                return translationResult;
            }

        }
        public static String Translate(string authToken, string what, string to)
        {
            // Add TranslatorService as a service reference, Address:http://api.microsofttranslator.com/V2/Soap.svc
            LanguageServiceClient client = new LanguageServiceClient();
            //Set Authorization header before sending the request
            HttpRequestMessageProperty httpRequestProperty = new HttpRequestMessageProperty();
            httpRequestProperty.Method = "POST";
            httpRequestProperty.Headers.Add("Authorization", authToken);
            // Creates a block within which an OperationContext object is in scope.
            // to = "en";

            using (OperationContextScope scope = new OperationContextScope(client.InnerChannel))
            {
                OperationContext.Current.OutgoingMessageProperties[HttpRequestMessageProperty.Name] = httpRequestProperty;
                //string sourceText = "<UL><LI>Use generic class names. <LI>Use pixels to express measurements for padding and margins. <LI>Use percentages to specify font size and line height. <LI>Use either percentages or pixels to specify table and container width.   <LI>When selecting font families, choose browser-independent alternatives.   </LI></UL>";
                string sourceText = what;

                //string sourceText = "A cat ate a dog";

                string translationResult;
                //Keep appId parameter blank as we are sending access token in authorization header.

                translationResult = client.Translate("", sourceText, "", to, "text/plain", "general", "");
                //Console.OutputEncoding = Encoding.UTF8;
                //Console.WriteLine("Translation for source {0} from {1} to {2} is", sourceText, "ru", "en");
                //Console.WriteLine(translationResult);
                //Console.WriteLine("Press any key to continue...");
                //Console.ReadKey(true);
                return translationResult;
            }

        }
        public static String TranslateToEnglish(String inn)
        {
            return Translate(GetTokenWrapper(), inn, "en");

        }

        public static String TranslateTo(String inn, String to)
        {
            return Translate(GetTokenWrapper(), inn, to);

        }

        public static void AddTranslationToAllRecordsTweetsAnWarWrap()
        {
            StreamReader sr = new StreamReader(Common.AnWarAllAcIn);
            StreamWriter sw = new StreamWriter(Common.AnWarAllAcOut);

            String str = sr.ReadLine();
            sw.WriteLine(str+"^translation_desc^destination_eng");

            String admToken = GetTokenWrapper();
            HttpRequestMessageProperty httpRequestProperty = new HttpRequestMessageProperty();
            httpRequestProperty.Method = "POST";
            httpRequestProperty.Headers.Add("Authorization", admToken);
            Int64 count = 0;

            while ((str=sr.ReadLine())!=null)
            {
                String[] items = str.Split('^');
                if (items.Length == 29)
                {
                    sw.WriteLine(str);
                    continue;
                }
                Int32 pos = items.Length - 1;
                String translation = items[pos];
                String destination = items[13];
                out1: ;
                if (translation!="" && !IsEnglish(translation)) translation=Translate(httpRequestProperty,admToken,translation,"en");
                if (translation.Contains("ServiceModel.ProtocolException")) goto out3;
                out2:;
                if (destination != "" && !IsEnglish(destination)) destination = Translate(httpRequestProperty, admToken, destination, "en");
                if (destination.Contains("ServiceModel.ProtocolException")) goto out2;
                sw.WriteLine(str + "^" + translation+"^"+destination);
                count++;
            }
            out3:;
            sr.Close();
            sw.Close();

        }

        static public bool IsEnglish(string inputstring)
        {
            Regex regex = new Regex(@"[A-Za-z0-9 .,-=+(){}\[\]\\]");
            MatchCollection matches = regex.Matches(inputstring);
            if (matches.Count.Equals(inputstring.Length))
                return true;
            else
                return false;
        }

    }








    [DataContract]
    public class AdmAccessToken
    {
        [DataMember]
        public string access_token { get; set; }
        [DataMember]
        public string token_type { get; set; }
        [DataMember]
        public string expires_in { get; set; }
        [DataMember]
        public string scope { get; set; }
    }

    public class AdmAuthentication
    {
        public static readonly string DatamarketAccessUri = "https://datamarket.accesscontrol.windows.net/v2/OAuth2-13";
        private string clientId;
        private string clientSecret;
        private string request;
        private AdmAccessToken token;
        private Timer accessTokenRenewer;
        //Access token expires every 10 minutes. Renew it every 9 minutes only.
        private const int RefreshTokenDuration = 9;
        public AdmAuthentication(string clientId, string clientSecret)
        {

            this.clientId = clientId;
            this.clientSecret = clientSecret;
            //If clientid or client secret has special characters, encode before sending request
            this.request = string.Format("grant_type=client_credentials&client_id={0}&client_secret={1}&scope=http://api.microsofttranslator.com", HttpUtility.UrlEncode(clientId), HttpUtility.UrlEncode(clientSecret));
            this.token = HttpPost(DatamarketAccessUri, this.request);
            //renew the token every specified minutes
            accessTokenRenewer = new Timer(new TimerCallback(OnTokenExpiredCallback), this, TimeSpan.FromMinutes(RefreshTokenDuration), TimeSpan.FromMilliseconds(-1));
        }
        public AdmAccessToken GetAccessToken()
        {
            return this.token;
        }
        private void RenewAccessToken()
        {
            AdmAccessToken newAccessToken = HttpPost(DatamarketAccessUri, this.request);
            //swap the new token with old one
            //Note: the swap is thread unsafe
            this.token = newAccessToken;
            Console.WriteLine(string.Format("Renewed token for user: {0} is: {1}", this.clientId, this.token.access_token));
        }
        private void OnTokenExpiredCallback(object stateInfo)
        {
            try
            {
                RenewAccessToken();
            }
            catch (Exception ex)
            {
                Console.WriteLine(string.Format("Failed renewing access token. Details: {0}", ex.Message));
            }
            finally
            {
                try
                {
                    accessTokenRenewer.Change(TimeSpan.FromMinutes(RefreshTokenDuration), TimeSpan.FromMilliseconds(-1));
                }
                catch (Exception ex)
                {
                    Console.WriteLine(string.Format("Failed to reschedule the timer to renew access token. Details: {0}", ex.Message));
                }
            }
        }
        private AdmAccessToken HttpPost(string DatamarketAccessUri, string requestDetails)
        {
            //Prepare OAuth request 
            WebRequest webRequest = WebRequest.Create(DatamarketAccessUri);
            webRequest.ContentType = "application/x-www-form-urlencoded";
            webRequest.Method = "POST";
            byte[] bytes = Encoding.ASCII.GetBytes(requestDetails);
            webRequest.ContentLength = bytes.Length;
            using (Stream outputStream = webRequest.GetRequestStream())
            {
                outputStream.Write(bytes, 0, bytes.Length);
            }
            using (WebResponse webResponse = webRequest.GetResponse())
            {
                DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(AdmAccessToken));
                //Get deserialized object from JSON stream
                AdmAccessToken token = (AdmAccessToken)serializer.ReadObject(webResponse.GetResponseStream());
                return token;
            }
        }
    }


}
