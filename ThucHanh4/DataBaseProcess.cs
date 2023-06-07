using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThucHanh4
{
    public class DataBaseProcess
    {
        SqlConnection sqlconn = new SqlConnection();

        void OpenConnect()
        {
            sqlconn.ConnectionString = "server=DungNguyen" + ";database=ThucHanh4" + ";integrated security=true";
            if(sqlconn.State != System.Data.ConnectionState.Open)
                sqlconn.Open();
        }

        void CloseConnect()
        {
            if(sqlconn.State != System.Data.ConnectionState.Closed)
            {
                sqlconn.Close();
                sqlconn.Dispose();
            }    
        }    

        // Select
        public DataTable DataReader (string sqlSelect)
        {
            DataTable dt = new DataTable();
            OpenConnect();
            SqlDataAdapter sqlData = new SqlDataAdapter(sqlSelect,sqlconn);
            sqlData.Fill(dt);
            CloseConnect();
            return dt;
        }

        // Insert Update Delete
        public void Datachange(string sql)
        {
            OpenConnect();
            SqlCommand sqlcmd = new SqlCommand();
            sqlcmd.Connection = sqlconn;
            sqlcmd.CommandText = sql;
            sqlcmd.ExecuteNonQuery();
            CloseConnect();
        }
    }
}
