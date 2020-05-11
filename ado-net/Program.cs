using System;
using Microsoft.Data.SqlClient;

namespace ado_net
{
    class Program
    {
        private const string connString = @"Server=localhost;Database=Blogging;uid=sa;pwd=123456";
        static void Main(string[] args)
        {
            //1. 创建连接
            // var conn = new SqlConnection();
            // //方式一：手写连接字符串
            // conn.ConnectionString = connString;
            using (var conn = new SqlConnection(connString))
            {
                
            
                //方式二：SqlConnectionStringBuilder进行数据库配置
                // var connBuilder = new SqlConnectionStringBuilder();
                // connBuilder.DataSource = "localhost";
                // connBuilder.InitialCatalog = "Blogging";
                // connBuilder.UserID = "sa";
                // connBuilder.Password = "123456";
                // var connString = connBuilder.ConnectionString;
                // conn.ConnectionString = connString;
                
                
                //方式三： 未成功，还有待研究
                // var connString = ConfigurationManager.AppSettings["ConnectionString"].ToString();
                // var appSettings = ConfigurationManager.AppSettings;  
                // conn.ConnectionString = "";

                //2. 打开连接
                conn.Open();
                //SqlConnection的几个重要属性
                //state:
                //    Closed 关闭
                //    Open 打开
                //    Connection 连接中
                //    Executing 正在执行命令
                //    Fetching  正在检索   
                //    Broken    连接中断
                var state = conn.State;
                var database = conn.Database;
                var dataSource = conn.DataSource;
                var timeout = conn.ConnectionTimeout;
                
                //3. 执行命令
                //方式一
                var command = conn.CreateCommand();
                //增，删，改操作
                const string sql = @"INSERT INTO Blogging.dbo.Blog (BlogId, Url, Rating) VALUES(1, 'www.google.com', 4)";
                // const string sql = @"UPDATE Blogging.dbo.Blog SET Rating = 5 WHERE BlogId = 5";
                // const string sql = @"DELETE Blogging.dbo.Blog WHERE BlogId = 5";
                command.CommandText = sql;
                var columns = command.ExecuteNonQuery();


                //方式二
                // var cmd = new SqlCommand(sql, conn);

                const string countSql = @"select count(1) from Blog";
                var countCmd = new SqlCommand(countSql, conn);
                var size = countCmd.ExecuteScalar();
                Console.WriteLine("总条目数：{0}",size);
                
                const string query = @"select * from Blogging.dbo.Blog where BlogId = @BlogId";
                var param = new SqlParameter("@BlogId",1);
                var queryCmd = new SqlCommand(query, conn);
                queryCmd.Parameters.Add(param);
                var queryValue = queryCmd.ExecuteScalar();
                var reader = queryCmd.ExecuteReader();
                Console.WriteLine("查询结果：{0}", reader);
                while (reader.Read())
                {
                    var blog = new Blog();
                    var id = int.Parse(reader["BlogId"].ToString());
                    var url = reader["Url"].ToString();
                    var rating = int.Parse(reader["Rating"].ToString());
                    blog.BlogId = id;
                    blog.Url = url;
                    blog.Rating = rating;
                    Console.WriteLine("读取博客：{0}-{1}",blog.BlogId, blog.Url);
                }
                
                //关闭reader
                reader.Close();
                
                //4. 关闭连接
                //close与dispose间的区别：
                //close后还可以open，
                //dispose后，connetionString被清空，不能再open
                conn.Close();
                conn.Dispose();
                
            }
        }
    }
}