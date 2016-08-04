﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WikiDigger
{
    static public class Tasks
    {
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


    }
}