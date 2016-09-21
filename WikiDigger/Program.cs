using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WikiDigger
{
    class Program
    {
        static void Main(string[] args)
        {
            //Tasks.PutToFileAllCategoriesForKeyWordsRU();
            //Tasks.PutToFileAllPagesForKeyWordsRU();
            //Tasks.PutToFileAllPagesForCategoriesRU();
            //Tasks.AddTranslationToAllRecordsTweetsAnWar();
            //Tasks.CleanUSSRIP();
            //Tasks.CreateEditsAndEditorsTables();
            //Tasks.RemoveComments(Common.editsFile, Common.editsFileNoComments);

            //Tasks.Comments(@"C:\work\bigdata\edits.txt", @"C:\work\bigdata\edits_no_comments.txt");
            //Tasks.MergePageViewTables(Common.PageViewsStub + "pageViews1.txt", Common.mergedPageViews);
            //Tasks.MergePageViewTables(Common.PageViewsStub + "pageViews2.txt", Common.mergedPageViews);
            //Tasks.MergePageViewTables(Common.PageViewsStub + "pageViews3.txt", Common.mergedPageViews);
            //Tasks.CleanEditsExactTimeMerge(Common.editsRawStub + "edits.txt", Common.editsMergedFixedTime);
            //Tasks.DecodePageNames(@"C:\Google Drive\PAPERS\WIKI\wikidata\detailedpageViews.txt", @"C:\Google Drive\PAPERS\WIKI\wikidata\detailedpageViewsDecoded.txt");
            //QuotedPrintable.DecodeQuotedPrintable("Второй Всеукраинский съезд депутатов всех уровней в %D0%A1%D0%B5в%D0%B5%D1%80%D0%BE%D0%B4%D0%BE%D0%BD%D0%B5%D1%86%D0%BA%D0%B5", "UTF-8");
            //Tasks.BringRowsTogetherAggregatedComment(@"C:\Google Drive\PAPERS\WIKI\wikidata\aggregatedCommentFull.txt", Common.aggregatedChangesSQLFull);
            OtherTasks.AggregateTweetsUpdated(Common.ISISallreports, Common.ISISuniquesmartAggregated);
        }
    }
}
