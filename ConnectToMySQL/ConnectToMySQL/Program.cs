using System;
using MySql.Data.MySqlClient;

namespace ConnectToMySQL
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            string connStr = "Database=testdata;datasource=127.0.0.1;port=3306;user=root;pwd=123456;";
            MySqlConnection conn = new MySqlConnection(connStr);

            conn.Open();

            #region 查询
            MySqlCommand command = new MySqlCommand("select * from testtable", conn);

            MySqlDataReader dataReader = command.ExecuteReader();

            while (dataReader.Read())
            {
                string username = dataReader.GetString("username");
                string password = dataReader.GetString("password");
                Console.WriteLine(username + ":" + password);
            }
            #endregion
            
            #region 插入
            //string username = "cwer";string password = "lcker';delete from user;";
            ////MySqlCommand cmd = new MySqlCommand("insert into user set username ='" + username + "'" + ",password='" + password + "'", conn);
            //MySqlCommand cmd = new MySqlCommand("insert into user set username=@un , password = @pwd", conn);

            //cmd.Parameters.AddWithValue("un", username);
            //cmd.Parameters.AddWithValue("pwd", password);

            //cmd.ExecuteNonQuery();
            #endregion

            #region 删除
            //MySqlCommand cmd = new MySqlCommand("delete from user where id = @id", conn);
            //cmd.Parameters.AddWithValue("id", 18);

            //cmd.ExecuteNonQuery();
            #endregion

            #region 更新
//            MySqlCommand cmd = new MySqlCommand("update user set password = @pwd where id = 14", conn);
//            cmd.Parameters.AddWithValue("pwd", "sikiedu.com");
//
//            cmd.ExecuteNonQuery();
            #endregion
            
            conn.Close();

            Console.ReadKey();
        }
    }
}