using System;
using System.Text;
using HtmlAgilityPack;
using System.IO;
using System.Net;
using System.Collections.Generic;
using System.Text.RegularExpressions;


namespace WikiDigger
{
    static public class HTMLDigger
    {
        static public String ruCategoriesWikibase = "https://ru.wikipedia.org/wiki/%D0%A1%D0%BB%D1%83%D0%B6%D0%B5%D0%B1%D0%BD%D0%B0%D1%8F:%D0%92%D1%81%D0%B5_%D1%81%D1%82%D1%80%D0%B0%D0%BD%D0%B8%D1%86%D1%8B/Category:";

        //namespace 14 - category
        //0 - general

        static public List<List<String>> ReturnWikiPagesForCategories(SortedDictionary<String, String> keys)
        {
            List<List<String>> buf = new List<List<String>>();
            var document = new HtmlDocument();
            var client = new WebClient();
            bool over = true;
            String baseString = "https://ru.wikipedia.org/wiki/Категория:";
            String tail = "";

            foreach (String category in keys.Keys)
            {
                var stream = client.OpenRead(baseString + category.Trim().Replace(' ','_'));

                var reader = new StreamReader(stream, Encoding.GetEncoding("UTF-8"));
                var html = reader.ReadToEnd();
                document.LoadHtml(html);

                String tags = "//div[@id='category-empty']";
                if (document.DocumentNode.SelectNodes(tags)!=null) continue;

                tags = "//div[@class='mw-content-ltr']";
                HtmlNodeCollection nodes = document.DocumentNode.SelectNodes(tags);
                HtmlNodeCollection nodes2 = null; 
               
                foreach (HtmlNode node in nodes)
                {
                    if (node.FirstChild.Name=="h3" || node.FirstChild.Name == "div")
                    {
                        tags = "//li";
                        document.LoadHtml(node.InnerHtml);
                        nodes2 = document.DocumentNode.SelectNodes(tags);
                        goto leave;
                    }

                   
                }
                continue;  
            leave:;

                foreach (HtmlNode node in nodes2)
                {
                    if (node.InnerText.Contains("Обсуждение")) continue;
                    if (node.InnerText.Contains("Шаблон")) continue;
                    if (node.FirstChild.Name == "#text") continue;
                    String val = RemoveCommentPart(node.InnerText);
                    List<String> pair = new List<string>();
                    pair.Add(keys[category]);
                    pair.Add(val.Replace("\n","").Replace("\t","").Replace("►","").Replace("[×]","").Trim());
                    pair.Add("0");
                    buf.Add(pair);
                }
            }

            return buf;
        }

        static String RemoveCommentPart(String str)
        {
            if (str.Contains("(") && str.Contains(")") && str.Contains(".") && str.Contains(":"))
            {
                Int32 pos = str.LastIndexOf("(");
                return str.Substring(0, pos - 1);
            }
            return str;
        }

        static public List<List<String>> ReturnWikiCategory(SortedDictionary<String, Int64> keys)
        {
            List<List<String>> buf = new List<List<String>>();
            var document = new HtmlDocument();
            var client = new WebClient();
            bool over = true;
            String baseString = "https://ru.wikipedia.org";
            //foreach (Char c in Common.alpha)
            String tail = "/w/index.php?title=Служебная:Все_страницы&from=%28hed%29+P.E.&namespace=14";
            do
            {

                var stream = client.OpenRead(baseString + tail);

                var reader = new StreamReader(stream, Encoding.GetEncoding("UTF-8"));
                var html = reader.ReadToEnd();
                document.LoadHtml(html);


                String tags = "//li";
                HtmlNodeCollection nodes = document.DocumentNode.SelectNodes(tags);


                foreach (HtmlNode node in nodes)
                {
                    if (!node.InnerText.Contains("Категория:")) break;
                    String value = node.InnerText.Substring("Категория:".Length).ToLower();


                    foreach (String key in keys.Keys)
                    {
                        if (value.Contains(key.ToLower()))
                        {
                            List<String> pair = new List<string>();
                            pair.Add(key);
                            pair.Add(node.InnerText.Substring("Категория:".Length));
                            pair.Add("14");
                            buf.Add(pair);
                        }
                    }

                }

                tags = "//div[@class='mw-allpages-nav']";
                nodes = document.DocumentNode.SelectNodes(tags);


                foreach (HtmlNode node in nodes)
                {
                    String val = node.InnerHtml;
                    if (node.InnerText.Contains("|"))
                    {
                        val = val.Substring(val.IndexOf("|"));
                    }
                    if (node.InnerText.Contains("Следующая страница"))
                    {
                        int firstpos = val.IndexOf("\"");
                        int lastpos = val.Substring(firstpos + 1).IndexOf("\"");
                        tail = val.Substring(firstpos + 1, lastpos);
                        tail = QuotedPrintable.DecodeQuotedPrintable(tail, "Привет");
                        tail = tail.Replace("amp;", "");
                        goto jump;
                    }


                }
                over = false;
            jump:;
            } while (over);

            return buf;


        }

        static public List<List<String>> ReturnWikiPages(SortedDictionary<String, Int64> keys)
        {
            List<List<String>> buf = new List<List<String>>();
            var document = new HtmlDocument();
            var client = new WebClient();
            bool over = true;
            String baseString = "https://ru.wikipedia.org";
            //foreach (Char c in Common.alpha)
            String tail = "/wiki/Служебная:Все_страницы?from=&to=&namespace=0&hideredirects=1";
            do
            {

                var stream = client.OpenRead(baseString + tail);

                var reader = new StreamReader(stream, Encoding.GetEncoding("UTF-8"));
                var html = reader.ReadToEnd();
                document.LoadHtml(html);


                String tags = "//li";
                HtmlNodeCollection nodes = document.DocumentNode.SelectNodes(tags);


                foreach (HtmlNode node in nodes)
                {
                   
                    String value = node.InnerText.ToLower();


                    foreach (String key in keys.Keys)
                    {
                        if (value.Contains(key.ToLower()))
                        {
                            List<String> pair = new List<string>();
                            pair.Add(key);
                            pair.Add(node.InnerText);
                            pair.Add("14");
                            buf.Add(pair);
                        }
                    }

                }

                tags = "//div[@class='mw-allpages-nav']";
                nodes = document.DocumentNode.SelectNodes(tags);


                foreach (HtmlNode node in nodes)
                {
                    String val = node.InnerHtml;
                    if (node.InnerText.Contains("|"))
                    {
                        val = val.Substring(val.IndexOf("|"));
                    }
                    if (node.InnerText.Contains("Следующая страница"))
                    {
                        int firstpos = val.IndexOf("\"");
                        int lastpos = val.Substring(firstpos + 1).IndexOf("\"");
                        tail = val.Substring(firstpos + 1, lastpos);
                        tail = QuotedPrintable.DecodeQuotedPrintable(tail, "Привет");
                        tail = tail.Replace("amp;", "");
                        goto jump;
                    }


                }
                over = false;
            jump:;
            } while (over);

            return buf;


        }


        static public List<List<String>> ReturnWikiPages(SortedDictionary<String, String> keys)
        {
            List<List<String>> buf = new List<List<String>>();
            var document = new HtmlDocument();
            var client = new WebClient();
            bool over = true;
            String baseString = "https://ru.wikipedia.org";
            //foreach (Char c in Common.alpha)
            String tail = "/wiki/Служебная:Все_страницы?from=&to=&namespace=0&hideredirects=1";
            do
            {

                var stream = client.OpenRead(baseString + tail);

                var reader = new StreamReader(stream, Encoding.GetEncoding("UTF-8"));
                var html = reader.ReadToEnd();
                document.LoadHtml(html);


                String tags = "//li";
                HtmlNodeCollection nodes = document.DocumentNode.SelectNodes(tags);


                foreach (HtmlNode node in nodes)
                {

                    String value = node.InnerText.ToLower();


                    foreach (String key in keys.Keys)
                    {
                        if (value.Contains(key.ToLower()))
                        {
                            List<String> pair = new List<string>();
                            pair.Add(key);
                            pair.Add(node.InnerText);
                            pair.Add("14");
                            buf.Add(pair);
                        }
                    }

                }

                tags = "//div[@class='mw-allpages-nav']";
                nodes = document.DocumentNode.SelectNodes(tags);


                foreach (HtmlNode node in nodes)
                {
                    String val = node.InnerHtml;
                    if (node.InnerText.Contains("|"))
                    {
                        val = val.Substring(val.IndexOf("|"));
                    }
                    if (node.InnerText.Contains("Следующая страница"))
                    {
                        int firstpos = val.IndexOf("\"");
                        int lastpos = val.Substring(firstpos + 1).IndexOf("\"");
                        tail = val.Substring(firstpos + 1, lastpos);
                        tail = QuotedPrintable.DecodeQuotedPrintable(tail, "Привет");
                        tail = tail.Replace("amp;", "");
                        goto jump;
                    }


                }
                over = false;
            jump:;
            } while (over);

            return buf;


        }

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
