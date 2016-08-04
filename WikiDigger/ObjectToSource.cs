using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using System.IO;

namespace WikiDigger
{
    static public class ObjectToSource
    {
        public static void ComplexListToSource<T>(String to, List<List<T>> d)
        {
            StreamWriter sw = new StreamWriter(to);

            foreach (List<T> l in d)
            {
                String str = "";
                foreach (T t in l)
                {
                    str += t.ToString().Trim()+"\t";
                }
                sw.WriteLine(str.Substring(0,str.Length-1));
            }

            sw.Close();
        }

    }
}
