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


    static public class ParseXML
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


        static public String CleanExactTime(String from)
        {
            String str = from;

            str = from.Replace("T", " ");
            str = str.Replace("Z", String.Empty);

            return str;
        }

        static public List<String> ReleaseFile(String file, List<String> list)
        {
            StreamWriter sw = new StreamWriter(file, true, Encoding.UTF8);
            foreach (String str in list) sw.WriteLine(str);
            sw.Close();
            return new List<string>();
        }

        static public Dictionary<string, List<Object>> ReleaseFile(String file, Dictionary<string, List<Object>> list)
        {
            StreamWriter sw = new StreamWriter(file, false, Encoding.UTF8);
            foreach (String str in list.Keys) sw.WriteLine(list[str][2].ToString()+"\t"+list[str][0].ToString()+"\t" + list[str][1].ToString());
            sw.Close();
            return list ;
        }
        static public void ParseXMLwikiForEdits(String file)
        {
            XmlReader XMLreader = XmlReader.Create(new StreamReader(Common.wikixmlFile, Encoding.UTF8));
            PostGrePlugIn.openConnection();
            Console.OutputEncoding = Encoding.Unicode;

            StreamWriter editsStream = new StreamWriter(Common.editsFile, false, Encoding.UTF8);
            StreamWriter editorsStream = new StreamWriter(Common.editorsFile, false, Encoding.UTF8);
            editsStream.Close();
            editorsStream.Close();

            List<String> editsStore = new List<string>();
            Dictionary<String, List<Object>> editorsStore = new Dictionary<string, List<Object>>();
            Int64 editsCounter = 0;
            Int64 editorsCounter = 0;

            Boolean flagReadTitle = false;
            Boolean flagReadBody = false;
            Boolean flagReadPage = false;
            String pageName = "";
            String oldPage = "";
            Boolean pageFlag = false;
            Boolean readRevisionMode = false;
            String editorID = "";
            String comment = "";
            String pageID = "";


            try
            {

                while (XMLreader.Read())
                {

                   
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
                        readRevisionMode = false;
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
                        if (Common.interestPages.Contains(QuotedPrintable.EncodeQuotedPrintable(pageName))) readRevisionMode = true;
                        while (XMLreader.Name != "id") XMLreader.Read();
                        XMLreader.Read();
                        pageID = XMLreader.Value;
                        flagReadTitle = false;
                    }
                    



                    if (XMLreader.Name == "revision" && XMLreader.NodeType == XmlNodeType.Element)
                    {
                        String timeString="";
                        DateTime day=new DateTime();
                        String ip = "NA";
                        String country = "NA";
                        Byte troll = 0;
                        Int32 typetroll = -1;
                        String name = "NA";
                        Int32 nameID = -1;
                        Double distance = -1;
                        Byte inGroup = 0;

                        if (readRevisionMode) inGroup = 1;

                        while (!(XMLreader.NodeType==XmlNodeType.EndElement && XMLreader.Name=="revision"))
                        {
                            XMLreader.Read();
                            if (XMLreader.NodeType==XmlNodeType.Element&&XMLreader.Name=="timestamp")
                            {
                                XMLreader.Read();
                                timeString = XMLreader.Value;
                                day = DateTime.Parse(timeString);
                                XMLreader.Read();
                            }

                            if (XMLreader.NodeType == XmlNodeType.Element && XMLreader.Name == "contributor")
                            {
                                while (XMLreader.Name != "ip" && XMLreader.Name != "username") XMLreader.Read(); 
                                  
                                if (XMLreader.Name=="ip")
                                {
                                    XMLreader.Read();
                                    ip=editorID = XMLreader.Value;
                                    country=PostGrePlugIn.DataTableToList(PostGrePlugIn.getTablePostGre(PostGrePlugIn.ReturnIpQuery("countryipranges", ip, "country")))[0];
                                    PostGrePlugIn.GetTrollResults(PostGrePlugIn.getTablePostGre(PostGrePlugIn.ReturnIpQuery("troll_bases", ip, " c1,c2,c3,c4, distance ")),out troll,out distance,out typetroll);

                                }
                                else
                                {
                                    XMLreader.Read();
                                    name = XMLreader.Value.Replace(@"\","_slash_");
                                   
                                    while (XMLreader.Name!="id") XMLreader.Read();
                                    XMLreader.Read();
                                                                         
                                    nameID = Int32.Parse(XMLreader.Value);
                                    editorID = nameID.ToString();

                                }

                                while (XMLreader.Name != "contributor") XMLreader.Read();
                                XMLreader.Read();
                                XMLreader.Read();
                                if (XMLreader.Name=="comment")
                                {
                                    XMLreader.Read();
                                    comment = XMLreader.Value.Replace('\n', ' ');
                                    comment = comment.Replace(@"\", " ");
                                    comment = comment.Replace('\t', ' ');
                                }
                                else 
                                {
                                    comment = "";
                                }

                                break;
                            }


                          




                        }




                        if (!editorsStore.ContainsKey(editorID))
                        {
                            List<object> list = new List<object>();
                            list.Add(1);
                            list.Add(1 * inGroup);
                            name = name.Replace(@"\", "_slash_");
                            list.Add(ip + "\t" + name + "\t" + nameID + "\t" + country + "\t" + troll.ToString() + "\t" + typetroll.ToString() + "\t" + distance.ToString());
                            editorsStore.Add(editorID, list);
                        }
                        else
                        {
                            editorsStore[editorID][0] = Int32.Parse(editorsStore[editorID][0].ToString()) + 1;
                            editorsStore[editorID][1] = Int32.Parse(editorsStore[editorID][0].ToString())*inGroup + 1;
                        }
                        if (distance>0)
                        {
                            ;
                        }
                        editsStore.Add(pageName + "\t" + pageID + "\t" + inGroup.ToString() + "\t"+ CleanExactTime(timeString) + "\t" + day.ToString("yyyy-MM-dd") + "\t" + ip + "\t" + name + "\t" + nameID + "\t" + country + "\t"   + troll.ToString() + "\t" + typetroll.ToString() + "\t" + distance.ToString() + "\t" + comment);
                        if (editsStore.Count > 10000) editorsStore = ReleaseFile(Common.editorsFile, editorsStore);
                        if (editsStore.Count > 10000) editsStore = ReleaseFile(Common.editsFile, editsStore);

                    }

                }
            }

            catch (XmlException ex)
            {
                Console.WriteLine("Ended abnormally..." + ex);
            }
        eend:;
            

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
