using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Data;
using System.Globalization;

namespace WikiDigger
{
    static public class OtherTasks
    {

        public static Dictionary<string,bool> InintializeMarker()
        {
            Dictionary<string, bool> marker = new Dictionary<string, bool>();
            marker.Add("followers_count", true);
            marker.Add("location", true);
            marker.Add("statuses_count",true);
            marker.Add("friends_count", true);
            marker.Add("favourites_count",true);
            marker.Add("bot", true);
            marker.Add("bot_timezone", true);
            marker.Add("lang", true);
            marker.Add("time_zone", true);
            marker.Add("description", true);
            marker.Add("first_tracked",false);
            marker.Add("profile_image_url",true);
            marker.Add("total_reports", false);
            marker.Add("was_suspended_from_the start", false);
            marker.Add("total_reports_unsuspended", false);
            marker.Add("total_days_unsuspended", false);
            marker.Add("first_day_suspended", false);
            marker.Add("changed_state", false);
            marker.Add("last_tracked", false);
            marker.Add("been_reported_as_suspended", false);

            return marker;
        }
        public static void AggregateTweetsUpdated(String file1, String file2)
        {
            Dictionary<String, Dictionary<String, Object>> entities = new Dictionary<string, Dictionary<String, Object>>();
            StreamReader sr = new StreamReader(file1, Encoding.UTF8);
            CultureInfo ci = new CultureInfo("ru"); 
            String[] heads = sr.ReadLine().Split('^');
            String str = "";
            Dictionary<string, bool> marker = InintializeMarker();

            while ((str=sr.ReadLine())!=null)
            {
                Dictionary<String, String> aux = new Dictionary<string, string>();
                String[] items = str.Split('^');
                if (items.Length<heads.Length)
                {
                    str += "^ ";
                    items = str.Split('^');
                }
                for (int i = 0; i < heads.Length; i++)
                    aux.Add(heads[i], items[i]);

                if (!entities.ContainsKey(aux["source_id"]))
                {
                    Dictionary<String, Object> help = new Dictionary<string, Object>();
                    help.Add("followers_count", aux["followers_count"]);
                    help.Add("location", aux["location"]);
                    help.Add("statuses_count", aux["statuses_count"]);
                    help.Add("friends_count", aux["friends_count"]);
                    help.Add("favourites_count", aux["friends_count"]);
                    help.Add("bot", aux["bot"]);
                    help.Add("bot_timezone", aux["bot_timezone"]);
                    help.Add("lang", aux["lang"]);
                    help.Add("time_zone", aux["time_zone"]);
                    help.Add("description", aux["description"]);
                    help.Add("first_tracked",  DateTime.Parse(aux["report_created_short"],ci));
                    help.Add("profile_image_url", aux["profile_image_url"]);
                    help.Add("total_reports", 1);
                    help.Add("last_tracked", DateTime.Parse(aux["report_created_short"], ci));
                    

                    if (aux["exists"]=="False")
                    {
                        help.Add("was_suspended_from_the start", "1");
                        help.Add("changed_state", -1);
                        help.Add("total_reports_unsuspended", 0);
                        help.Add("total_days_unsuspended", 0);
                        help.Add("first_day_suspended", DateTime.Parse(aux["report_created_short"], ci));
                        help.Add("been_reported_as_suspended", true);
                    }

                    else
                    {
                        help.Add("was_suspended_from_the start", "0");
                        help.Add("changed_state", 0);
                        help.Add("total_reports_unsuspended", 1);
                        help.Add("total_days_unsuspended", null);
                        help.Add("first_day_suspended", null);
                        help.Add("been_reported_as_suspended", false);

                    }

                    entities.Add(aux["source_id"], help);
                    continue;
                }
                Dictionary<String, Object> profile = new Dictionary<string, object>(entities[aux["source_id"]]);
                
                if ((String)profile["was_suspended_from_the start"] == "0" && aux["exists"] == "False" && !(Boolean)profile["been_reported_as_suspended"])
                {
                    profile["first_day_suspended"] = DateTime.Parse(aux["report_created_short"], ci);
                    profile["changed_state"] = 1;
                    profile["total_reports_unsuspended"] = profile["total_reports"];
                    profile["total_days_unsuspended"] =(int) ((DateTime.Parse(aux["report_created_short"], ci).Ticks - ((DateTime)profile["first_tracked"]).Ticks)/TimeSpan.TicksPerDay);
                    profile["been_reported_as_suspended"] = true;
                }

                foreach (String key in entities[aux["source_id"]].Keys)
                {
                    if (key == "location" && (string)entities[aux["source_id"]][key] == "")
                        profile[key] = "NA";
                    if (key == "total_reports")
                    {
                        profile[key] = (int)profile[key] + 1;
                        continue;
                    }
                    if (key=="last_tracked")
                    {
                        profile["last_tracked"] = DateTime.Parse(aux["report_created_short"], ci);
                        continue;
                    }

                    if ((String)profile["was_suspended_from_the start"]=="0"&&key== "total_reports_unsuspended" && aux["exists"]=="True")
                    {
                        profile[key] = (int)profile[key] + 1;
                         
                    }

                    if (!marker[key]) continue;
                    profile[key] = UniteIfDiffirent((String)profile[key], aux[key]);

                }

                entities[aux["source_id"]] = new Dictionary<string, object>(profile);




            }

            sr.Close();

            foreach (String key in entities.Keys)
            {
                if (entities[key]["total_days_unsuspended"]==null)
                {
                    entities[key]["total_days_unsuspended"] = (int)((DateTime.Now.Ticks - ((DateTime)entities[key]["first_tracked"]).Ticks) / TimeSpan.TicksPerDay);
                    entities[key]["total_reports_unsuspended"] = entities[key]["total_reports"];
                    entities[key]["first_day_suspended"] = null;
                }
            }

            PrintTweetsSmart(entities, file2);
        }

        public static String UniteIfDiffirent(String rec, String inn)
        {
            String[] items = rec.Split(';');
            if (items[items.Length - 1] == inn) return rec;
            return rec + ";" + inn;

        } 
       
        public static void PrintTweetsSmart(Dictionary<String, Dictionary<String, Object>> d, String outFile)
        {
            StreamWriter sw = new StreamWriter(outFile, false, Encoding.UTF8);
            String str = "";
            Boolean first = true;

            foreach (String account in d.Keys)
            {
                if (first)
                {
                    str = "account_id^";
                    foreach (String property in d[account].Keys)
                        str += property + "^";
                    sw.WriteLine(str.Substring(0, str.Length - 1));
                    first = false;
                }
                str = account+"^";
                foreach (String property in d[account].Keys)
                {
                    String outline="";
                    /*if (property == "first_day_suspended" || property == "first_day_suspended"||property == "last_tracked")
                    {
                        if (d[account][property] == null)
                        {
                            outline = "NA";
                        }
                        else
                        {
                            outFile = ((DateTime)d[account][property]).ToLongDateString();
                        }
                        
                    }
                    else
                    {
                        outline = d[account][property].ToString();
                    }*/
                    try
                    {
                        outline = d[account][property].ToString();
                    }
                    catch(Exception ex)
                    {
                        if (d[account][property] == null)
                            outFile = "NA";
                        else
                        {
                            outFile = ((DateTime)d[account][property]).ToString();
                        }
                    }
                    str += outline + "^";
                }
                sw.WriteLine(str.Substring(0, str.Length - 1));
            }

            sw.Close();


        }

    }

    



}
