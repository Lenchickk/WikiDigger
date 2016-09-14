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
            Tasks.CreateEditsAndEditorsTables();
            //Tasks.RemoveComments(Common.editsFile, Common.editsFileNoComments);

            //Tasks.Comments(@"C:\work\bigdata\edits.txt", @"C:\work\bigdata\edits_no_comments.txt");
            //Tasks.MergePageViewTables(Common.PageViewsStub + "pageViews1.txt", Common.mergedPageViews);
            //Tasks.MergePageViewTables(Common.PageViewsStub + "pageViews2.txt", Common.mergedPageViews);
            //Tasks.MergePageViewTables(Common.PageViewsStub + "pageViews3.txt", Common.mergedPageViews);
            //Tasks.CleanEditsExactTimeMerge(Common.editsRawStub + "edits.txt", Common.editsMergedFixedTime);
    
        }
    }
}
