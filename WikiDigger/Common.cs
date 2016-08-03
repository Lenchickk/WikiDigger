using System;
using System.Collections.Generic;


namespace WikiDigger
{
    // Class for storing references and general procedures

    public static class Common
    {
        public static SortedDictionary<string, Int64> keys;
        public static SortedDictionary<String, Int64> keysPower;
        // public static String wikixmlFile = @"D:\wp\tube\wikixml\ruwiki-20160720-stub-meta-history.xml\ruwiki-20160720-stub-meta-history.xml";
        public static String wikixmlFile = @"D:\wp\pipe\store\ruwiki-20160501-stub-meta-history.xml\ruwiki-20160501-stub-meta-history.xml";
        public static String outputPagesBasedonCategoryFile = @"D:\wp\tube\pages\pagesbasedCategoryUk.csv";
        static String keyTablePostGre = "keys";
        static String pageTablePostGre = "pages";

    }
}
