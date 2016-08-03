using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.IO;

namespace WikiDigger
{
    //class to parse XML-files


    class ParseXML
    {
        static public void ParseXMLwiki(String file)
        {
            Dictionary<String, Int64> counterCat = new Dictionary<String, Int64>();
            XmlReader XMLreader = XmlReader.Create(new StreamReader(Common.wikixmlFile, Encoding.UTF8));

            Console.OutputEncoding = Encoding.Unicode;

            StreamWriter swCat = new StreamWriter(Common.outputPagesBasedonCategoryFile, false, Encoding.Unicode);

            Boolean flagReadTitle = false;
            Boolean flagReadBody = false;
            Boolean flagReadPage = false;
            String pageName = "";
            String oldPage = "";
            Boolean pageFlag = false;


            try
            {

                while (XMLreader.Read())
                {

                    if (XMLreader.Name == "") continue;
                    if (XMLreader.Name == "revision")
                    {

                        continue;
                    }
                    if (XMLreader.Name == "title" && (XMLreader.NodeType == XmlNodeType.Element))
                    {
                        flagReadTitle = true;
                        continue;
                    }

                    if (XMLreader.Name == "title" && (XMLreader.NodeType == XmlNodeType.EndElement))
                    {
                        flagReadTitle = false;
                        continue;
                    }

                    if (XMLreader.Name == "page" && (XMLreader.NodeType == XmlNodeType.Element))
                    {
                        flagReadPage = true;
                        continue;
                    }

                    if (XMLreader.Name == "page" && (XMLreader.NodeType == XmlNodeType.EndElement))
                    {
                        flagReadPage = false;
                        continue;
                    }


                    if (XMLreader.Name == "text" && (XMLreader.NodeType == XmlNodeType.Element))
                    {
                        flagReadBody = true;
                        continue;
                    }


                    if (XMLreader.Name == "text" && (XMLreader.NodeType == XmlNodeType.EndElement))
                    {
                        flagReadBody = false;
                        continue;
                    }

                    if (XMLreader.NodeType == XmlNodeType.Text && flagReadTitle)
                    {
                        pageName = XMLreader.Value;

                    }
                    goto eend;
                    if (XMLreader.NodeType == XmlNodeType.Text && flagReadBody && flagReadPage)
                    {
                        String[] allText = XMLreader.Value.Split('\n');
                        int pos = -1;
                        String category = "";

                        for (int i = 0; i < allText.Length; i++)
                        {
                            //if ((pos = allText[i].IndexOf("[Категория:")) > 0 && pos < 10)
                            if ((pos = allText[i].IndexOf("[Категория:")) > 0)
                            {
                                /* int start;
                                 if (allText[i].IndexOf('|') > 0)
                                 {
                                     start = allText[i].IndexOf('|');
                                 }
                                 else start = allText[i].IndexOf(']');
                                 try { category = allText[i].Substring(pos + 11, start - pos - 11); }
                                 catch (Exception ex)
                                 {
                                     continue;
                                 }

                                 /*foreach (String s in Common.keys.Keys)
                                 {
                                     foreach (String ss in Common.keys[s])
                                     {
                                         if (InTitle(category, ss))
                                         {
                                             swCat.WriteLine(currentTitle + "\t" + ss + "\t" + s);
                                             counterCat[s]++;
                                             goto ou;
                                             //Console.WriteLine(s + " " + counter);
                                         }
                                     }
                                 }*/
                                if (oldPage != pageName)
                                {
                                    pageFlag = true;
                                    oldPage = pageName;
                                }
                                else
                                {
                                    pageFlag = false;
                                }
                                if (pageName == "Категория:Болгария")
                                {
                                    int iii = 0;
                                }
                                PrintCategogy(swCat, pageName, allText[i], pageFlag);
                                goto eend;
                            }


                        }


                    }

                }
            }

            catch (XmlException ex)
            {
                swCat.WriteLine("Ended abnormally..." + ex);
            }
        eend:
            swCat.Close();

        }

        public static void PrintCategogy(StreamWriter sw, String page, String source, Boolean pageFlag)
        {
            Boolean first = pageFlag;
            foreach (String key in Common.keys.Keys)
            {
                if (source.Contains(":" + key) && first)
                {
                    sw.WriteLine(page + "\t" + QuotedPrintable.EncodeQuotedPrintable(page) + "\t" + key);
                    first = false;
                    Common.keysPower[key] = 1;
                    continue;
                }

                if (source.Contains(":" + key)) Common.keysPower[key]++;


            }
        }
    }
}
