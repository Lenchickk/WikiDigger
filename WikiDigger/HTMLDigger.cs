using System;
using System.Text;
using HtmlAgilityPack;
using System.IO;
using System.Net;

namespace WikiDigger
{
    static public class HTMLDigger
    {
        static public void BoldRipper()
        {
            //address from there to rip
            //this is an example of the ripping condition 
            DateTime now = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
            DateTime now2 = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
            ///

            String base_address = @"http://www.vesti.ru/news/index/date/";

            String current_address;// = base_address + Common.fullTime(now);//26.10.2015_09:23";

            var document = new HtmlDocument();
            var client = new WebClient();

            while (now.Year > 2005)
            {
                current_address = base_address;// +Common.fullTimeNT(now);

                var stream = client.OpenRead(current_address);

                var reader = new StreamReader(stream, Encoding.GetEncoding("UTF-8"));
                var html = reader.ReadToEnd();
                document.LoadHtml(html);

                String tags = "//div[@class='b-item_list item']";
                HtmlNodeCollection nodes = document.DocumentNode.SelectNodes(tags);

                foreach (HtmlNode n in nodes)
                {
                    // do something
                }

                //update _now_
            }


        }
    }
}
