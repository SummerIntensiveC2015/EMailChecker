using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Finisar.SQLite;
using System.Data;

namespace Email_Notifications
{
    class sqlClass
    {
        public sqlClass()
        {

        }
        public int k = 0;
        #region ExecuteNonQuery
        public int iExecuteNonQuery(string FileData, string sSql, int where)
        {
            int n = 0;
            //try
            //{
                using (SQLiteConnection con = new SQLiteConnection())
                {
                    if (where == 0)
                    {
                        con.ConnectionString = @"Data Source=" + FileData + "; Version=3; New=True;";
                    }
                    else
                    {
                        con.ConnectionString = @"Data Source=" + FileData + "; Version=3; New=False;";
                    }
                    con.Open();
                    using (SQLiteCommand sqlCommand = con.CreateCommand())
                    {
                        sqlCommand.CommandText = sSql;
                        n = sqlCommand.ExecuteNonQuery();
                    }
                    con.Close();
                }
            //}
            //catch
            //{
            //    n = 0;
            //}
            return n;

        }
        #endregion 
        #region Execute
        public DataRow[] drExecute(string FileData, string sSql)
        {
            DataRow[] datarows = null;
            SQLiteDataAdapter dataadapter = null;
            DataSet dataset = new DataSet();
            DataTable datatable = new DataTable();
            try
            {
                using (SQLiteConnection con = new SQLiteConnection())
                {
                    con.ConnectionString = @"Data Source=" + FileData + "; Version=3; New=False;";
                    con.Open();
                    using (SQLiteCommand sqlCommand = con.CreateCommand())
                    {
                        dataadapter = new SQLiteDataAdapter(sSql, con);
                        dataset.Reset();
                        dataadapter.Fill(dataset);
                        datatable = dataset.Tables[0];
                        datarows = datatable.Select();
                        k = datarows.Count();
                    }
                    con.Close();
                }
            }
            catch(Exception ex)
            {
            //    throw ex;
                datarows = null;
            }
            return datarows;

        }
        #endregion 
    }
}
