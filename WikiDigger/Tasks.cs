using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace WikiDigger
{
    static public class Tasks
    {
        //Create a table with post-soviet ranges to push to PostGreqsql later
       
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


    }
}
