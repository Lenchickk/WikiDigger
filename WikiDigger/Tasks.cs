using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Data;

namespace WikiDigger
{
    static public class Tasks
    {
        //Create a table with post-soviet ranges to push to PostGreqsql later
       
        static public void DecodePageNames(String f1, String f2)
        {
            StreamReader sr = new StreamReader(f1, Encoding.UTF8);
            StreamWriter sw = new StreamWriter(f2,false);
            Dictionary<String, String> mapping = new Dictionary<string, string>();

            String str = "";

            while ((str=sr.ReadLine())!=null)
            {
                if (str == "") continue;
                String[] items = str.Split('\t');

                if (!mapping.ContainsKey(items[1]))
                {
                    String import = QuotedPrintable.DecodeQuotedPrintable(items[1], "UTF-8");
                    import = QuotedPrintable.DecodeQuotedPrintable(import, "UTF-8");
                    mapping.Add(items[1], QuotedPrintable.DecodeQuotedPrintable(import, "UTF-8"));
                }
                items[1] = mapping[items[1]];
                str = "";
                foreach (String s in items) str += s + "\t";
                str = str.Substring(0, str.Length - 1);
                sw.WriteLine(str);
            }

            sr.Close();
            sw.Close();
        }


        static public void BringRowsTogetherAggregatedComment(String fileTo, String sqlRequest)
        {
            StreamWriter sw = new StreamWriter(fileTo, false);

            DataTable dt = PostGrePlugIn.getTablePostGre(sqlRequest);
            List<String> countries = PostGrePlugIn.DataTableToList(PostGrePlugIn.getTablePostGre(Common.getCountriesDB));

            DateTime date=new DateTime();
            String pagename="";
            Dictionary<String, String> values=new Dictionary<string, String>();
            values.Add("troll", "0");
            values.Add("NS", "0");

            foreach (String c in countries) values.Add(c, "0");
            String outta = "";
            bool firstFlag = true;

            foreach (DataRow dr in dt.Rows)
            {
                if ((DateTime.Parse(dr[2].ToString())!=date || dr[0].ToString().Trim()!=pagename) && !firstFlag)
                {
                    outta = date.ToString("yyyy-MM-dd") + "\t" + pagename + "\t";
                    foreach (String s in values.Keys)
                        outta += values[s].ToString() + "\t";
                    outta = outta.Substring(0, outta.Length - 1);
                    Int32 totalnontroll = 0;
                    foreach (String key in values.Keys)
                    {
                        if (key == "troll") continue;
                        totalnontroll += Int32.Parse(values[key]);
                    }
                    sw.WriteLine(outta+"\t" + totalnontroll.ToString());
                    values = new Dictionary<string, string>();
                    
                    values.Add("troll", "0");
                    values.Add("NS", "0");
                    foreach (String c in countries) values.Add(c, "0");
                }
                firstFlag = false;
                date = DateTime.Parse(dr[2].ToString());
                pagename = dr[0].ToString().Trim();

                if (dr[3].ToString()=="1")
                {
                    values["troll"] = dr[5].ToString();
                    continue;
                }

                if (dr[4].ToString()=="")
                {
                    values["NS"] = dr[5].ToString();
                    continue;
                }

                values[dr[4].ToString().Trim()] = dr[5].ToString();

            }
            sw.Close();
        } 

        //extact all categories for a set of keywords from Russian Wikipedia
        static public void PutToFileAllCategoriesForKeyWordsRU()
        {
            SortedDictionary<String, Int64> keys = PostGrePlugIn.getTableKeysAsDictionary();
            List<List<String>> categories = HTMLDigger.ReturnWikiCategory(keys);
            ObjectToSource.ComplexListToSource<String>(Common.categoryFile, categories);

        }

        //extact all pages for a set of keywords from Russian Wikipedia
        static public void PutToFileAllPagesForKeyWordsRU()
        {
            SortedDictionary<String, Int64> keys = PostGrePlugIn.getTableKeysAsDictionary();
            List<List<String>> categories = HTMLDigger.ReturnWikiPages(keys);
            ObjectToSource.ComplexListToSource<String>(Common.pagesFile, categories);

        }

        //extact all pages for a set of categories from Russian Wikipedia
        //table - categories

        static public void PutToFileAllPagesForCategoriesRU()
        {
            SortedDictionary<String, String> categories = PostGrePlugIn.getTableCategoriesAsDictionary();
            List<List<String>> pages = HTMLDigger.ReturnWikiPagesForCategories(categories);
            ObjectToSource.ComplexListToSource<String>(Common.pagesFilonCategory, pages);

        }

        //add a string with the translation to the file on Аnоmymоus Wаr
        static public void AddTranslationToAllRecordsTweetsAnWar()
        {
            MicrosoftTranslatePlugIn.AddTranslationToAllRecordsTweetsAnWarWrap();
        }

        static public void CleanUSSRIP()
        {
            StreamReader sr = new StreamReader(Common.ipRangeRawUSSR);
            StreamWriter sw = new StreamWriter(Common.ipRangeReadyToTableUSSR);

            String str = "";
            String country = "";
            while ((str = sr.ReadLine()) != null)
            {
                if (str.Contains("#"))
                {
                    country = str.Replace("#", String.Empty);
                    continue;
                }
                String[] items = str.Split('-');
                sw.WriteLine(country + "\t" + items[0] + "\t" + items[1]);

            }

            sr.Close();
            sw.Close();

        }


        static public void CreateEditsAndEditorsTables()
        {
            Common.interestPages = PostGrePlugIn.DataTableToHashSet(PostGrePlugIn.getTablePostGre(Common.getPagesSQL));
            ParseXML.ParseXMLwikiForEdits(Common.wikixmlFile);

        } 

        static public void MergePageViewTables(String from, String to)
        {
            StreamReader sr = new StreamReader(from);
            StreamWriter sw = new StreamWriter(to, true);

            String str = "";
            String[] buf=null;

            while ((str=sr.ReadLine())!=null)
            {
                String[] items = str.Split('\t');
                if (items.Length < 6) continue;
                
                if (items[1]=="1")
                {
                    buf = items;
                    continue;   
                }

                sw.WriteLine(items[0] + "\t" + items[2] + "\t" + items[3] + "\t" + items[4] + "\t" + items[5] + "\t" + buf[2]+ "\t" + buf[3] + "\t" + buf[4] + "\t" + buf[5]);

            }

            sw.Close();
            sr.Close();

        }


        static public void MergePageViewTablesDeatiled(String from, String to)
        {
            StreamReader sr = new StreamReader(from);
            StreamWriter sw = new StreamWriter(to, true);

            String str = "";
            String[] buf = null;

            while ((str = sr.ReadLine()) != null)
            {
                String[] items = str.Split('\t');
                

            }

            sw.Close();
            sr.Close();

        }

        static public void RemoveComments(String from, String to)
        {
    
            StreamWriter sw = new StreamWriter(to,false,Encoding.UTF8);
            StreamReader sr = new StreamReader(from,true);

            String str = "";
            
            while ((str=sr.ReadLine())!=null)
            {
                String[] items = str.Split('\t');
                Int32 len = items.Length;
                String outstring = "";

                for (int i = 0; i < len - 1; i++) outstring += items[i] + "\t";

                outstring = outstring.Substring(0, outstring.Length - 1);
                sw.WriteLine(outstring);
                                            

            }

            sw.Close();
            sr.Close();
        }


        static public void CleanEditsExactTimeMerge(String from, String to)
        {
            StreamReader sr = new StreamReader(from,true);
            StreamWriter sw = new StreamWriter(to, false);

            String str = "";
       
            while ((str = sr.ReadLine()) != null)
            {
                String[] items = str.Split('\t');
                if (items.Length!=12) 
                {
                    continue;
                }
    
                items[3] = items[3].Replace("T", " ");
                items[3] = items[3].Replace("Z", String.Empty);
                if (items[3]=="2006-09-13 08:42:21")
                {
                    items[0] = items[0].Replace(@"\\", String.Empty);
                    continue;
                }
                str = "";
                for (int i = 0; i < items.Length; i++) str += items[i]+"\t";
                str = str = str.Substring(0,str.Length - 1);
                sw.WriteLine(str);
            }

            sw.Close();
            sr.Close();

        }

    }
}
