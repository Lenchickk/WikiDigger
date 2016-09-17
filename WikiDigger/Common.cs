using System;
using System.Collections.Generic;


namespace WikiDigger
{
    // Class for storing references and general procedures

    public static class Common
    {
        public static String baseAddress = @"D:\wp\";
        public static SortedDictionary<string, Int64> keys;
        public static SortedDictionary<String, Int64> keysPower;
        public static String aggregatedChangesSQLGroup = "select * from aggregatedcomment order by page, timemark,troll, country;";
        public static String aggregatedChangesSQLFull = "select * from aggregatedcommentfull order by page, timemark,troll, country;";
        // public static String wikixmlFile = @"D:\wp\tube\wikixml\ruwiki-20160720-stub-meta-history.xml\ruwiki-20160720-stub-meta-history.xml";
        public static String wikixmlFile = @"C:\work\bigdata\ruwiki-20160901-stub-meta-history.xml\ruwiki-20160901-stub-meta-history.xml";
        public static String wikixmlFileOld = baseAddress + @"pipe\store\ruwiki-20160501-stub-meta-history.xml\ruwiki-20160501-stub-meta-history.xml";
        public static String outputPagesBasedonCategoryFile = @"D:\wp\tube\pages\pagesbasedCategoryUk.csv";
        static String keyTablePostGre = "keys";
        static String pageTablePostGre = "pages";
        public static String categoryFile = baseAddress + @"tube\categories\selectedcat.txt";
        public static String pagesFile = baseAddress + @"tube\categories\selectedpages.txt";
        public static String pagesFilonCategory = baseAddress + @"tube\categories\selectedpagesForCategory.txt";
        static public char[] alpha = "!0123456789АБВГДЕЁЖЗИЙКЛМНОПРСТУФХЦЧШЩЭЮЯABCDEFGHIJKLMNOPQRSTUVWXYZ".ToCharArray();
        static public String AnWarAllAcIn = baseAddress + @"tube\other\anwar\allTweetsAc.csv";
        static public String AnWarAllAcOut = baseAddress + @"tube\other\anwar\allTweetsAcTranslation1.csv";
        static public String ipRangeRawUSSR = baseAddress + @"tube\ips\post_soviet_range_www.countryipblocks.net.txt";
        static public String ipRangeReadyToTableUSSR = baseAddress + @"tube\ips\post_soviet_rangeip_clean.txt";
        static public String editsFile = @"C:\work\bigdata\edits1.txt";
        static public String editsFileNoComments = @"C:\work\bigdata\edits_no_comments.txt";
        static public String editorsFile = @"C:\work\bigdata\editors1.txt";
        public static volatile HashSet<String> interestPages = new HashSet<string>();
        public static String getPagesSQL = @"select distinct title from pages where _key<>'Славянск' and _key<>'Минск' and _key<>'Белорусси' and _key<>'Аваков' and _key<>'Аксенов' and nc=14;";
        public static String mergedPageViews = @"C:\Google Drive\PAPERS\WIKI\wikidata\pageViewMerged.txt";
        public static String PageViewsStub = @"C:\Google Drive\PAPERS\WIKI\wikidata\";
        public static String getCountriesDB = "select * from countries;";
        public static String editsMergedFixedTime = @"C:\Google Drive\PAPERS\WIKI\wikidata\editsMerged.txt";
        public static String editsRawStub = @"C:\Google Drive\PAPERS\WIKI\wikidata\";


    }
}
