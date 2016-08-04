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
        public static String categoryFile = @"D:\wp\tube\categories\selectedcat.txt";
        public static String pagesFile = @"D:\wp\tube\categories\selectedpages.txt";
        public static String pagesFilonCategory = @"D:\wp\tube\categories\selectedpagesForCategory.txt";
        static public char[] alpha = "!0123456789АБВГДЕЁЖЗИЙКЛМНОПРСТУФХЦЧШЩЭЮЯABCDEFGHIJKLMNOPQRSTUVWXYZ".ToCharArray();
        static public String AnWarAllAcIn = @"D:\wp\tube\other\anwar\allTweetsAc.csv";
        static public String AnWarAllAcOut = @"D:\wp\tube\other\anwar\allTweetsAcTranslation1.csv";
    }
}
