using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Npgsql;
using NpgsqlTypes;
using System.Data;

namespace WikiDigger
{
    static class PostGrePlugIn
    {
        static String connstring = String.Format("Server=localhost;Port=5432;User Id=postgres;Password=admin;Database=postgres;");
        static String getKeysQuery = "select * from public.keys";
        public static IDataReader reader;


        //Index and name reverted. A table with 2 rows into a dictionary

        static SortedDictionary<String, Int64> TableToDictionaryWithIndex(DataTable dt, string name, string indexName)
        {
            SortedDictionary<String, Int64> dict = new SortedDictionary<string, long>();

            foreach (DataRow dr in dt.Rows)
            {
                dict.Add(dr[name].ToString(), Int64.Parse(dr[indexName].ToString()));
            }

            return dict;
        }


        static SortedDictionary<String, List<String>> TableToDictionaryWithIndex(DataTable dt)
        {
            SortedDictionary<String, List<String>> dict = new SortedDictionary<string, List<String>>();

            foreach (DataRow dr in dt.Rows)
            {
                List<String> buf = new List<String>();
                for (int i = dr.ItemArray.Length - 1; i > 0; i--)
                    buf.Add(dr[i].ToString());

                dict.Add(dr[0].ToString(), buf);
            }

            return dict;
        }


        static public SortedDictionary<String, Int64> getTableKeysAsDictionary()
        {
            return TableToDictionaryWithIndex(getTableKeys(), "title", "kid");
        }

        static public DataTable getTableKeys()
        {
            return getTablePostGre(getKeysQuery);
        }

        static public DataTable getTablePostGre(String sqlQuery)
        {
            DataTable dt = new DataTable();
            DataSet ds = new DataSet();

            try
            {
                NpgsqlConnection conn = new NpgsqlConnection(connstring);
                conn.Open();

                NpgsqlDataAdapter da = new NpgsqlDataAdapter(sqlQuery, conn);
                ds.Reset();
                // filling DataSet with result from NpgsqlDataAdapter
                da.Fill(ds);
                // since it C# DataSet can handle multiple tables, we will select first
                dt = ds.Tables[0];
            }
            catch (Exception ex)
            {

            }

            return dt;
        }

        static public void RunSQLQuery(String sqlQuery)
        {
            try
            {
                // PostgeSQL-style connection string
                NpgsqlConnection conn = new NpgsqlConnection(connstring);
                conn.Open();
                // quite complex sql statement
                // Making connection with Npgsql provider
                NpgsqlCommand command = new NpgsqlCommand(sqlQuery, conn);

                reader = command.ExecuteReader();
                reader.Close();


                conn.Close();


            }

            catch (Exception msg)
            {
                // something went wrong, and you wanna know why
                Console.WriteLine("something is wrong!");
                //throw;
            }
        }
    }
}
