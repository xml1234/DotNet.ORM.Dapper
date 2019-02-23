using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using Dapper;

namespace ORM.Dapper
{
    public class Connection
    {
        private readonly string sqlconnection =
            "Data Source=.;Initial Catalog=DB_Demo;User Id=sa;Password=123456;";

        public SqlConnection OpenConnection()
        {
            SqlConnection connection = new SqlConnection(sqlconnection);
            connection.Open();
            return connection;
        }


    }

    //先创建一个类,是数据库的ColumnCat表的模型。
    public class ColumnCat
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime ModifiedOn { get; set; }
        public int Parentid { get; set; }
    }

    public class DBDapper
    {


        /// <summary>
        /// 获取ColumnCat对象的集合。
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<ColumnCat> SelectColumnCats()
        {
            Connection connection = new Connection();
            using (IDbConnection conn = connection.OpenConnection())
            {
                const string query = "select * from ColumnCat order by id desc";
                return conn.Query<ColumnCat>(query, null);
            }

        }

        /// <summary>
        /// 接下来向数据库里添加一个类别
        /// </summary>
        /// <param name="cat"></param>
        /// <returns></returns>
        public static int InsertColumnCat(ColumnCat cat)
        {
            Connection connection = new Connection();
            using (IDbConnection conn = connection.OpenConnection())
            {
                const string query = "insert into ColumnCat([name],ModifiedOn,Parentid) values (@name, @ModifiedOn, @Parentid)";
                int row = conn.Execute(query, cat);
                //更新对象的Id为数据库里新增的Id,假如增加之后不需要获得新增的对象，
                //只需将对象添加到数据库里，可以将下面的一行注释掉。
                SetIdentity(conn, id => cat.Id = id, "id", "ColumnCat");
                return row;

            }
        }
        public static void SetIdentity(IDbConnection conn, Action<int> setId, string primarykey
            , string tableName)
        {
            if (string.IsNullOrEmpty(primarykey)) primarykey = "id";
            if (string.IsNullOrEmpty(tableName))
            {
                throw new ArgumentException("tableName参数不能为空，为查询的表名");
            }
            string query = string.Format("SELECT max({0}) as Id FROM {1}", primarykey
                , tableName);
            NewId identity = conn.Query<NewId>(query, null).Single<NewId>();
            setId(identity.Id);
        }

        /// <summary>
        /// 更新一个类别
        /// </summary>
        /// <param name="cat"></param>
        /// <returns></returns>
        public static int UpdateColumnCat(ColumnCat cat)
        {
            Connection connection = new Connection();
            using (IDbConnection conn = connection.OpenConnection())
            {
                const string query = "update ColumnCat set name=@Name, ModifiedOn = @ModifiedOn, Parentid = @Parentid where Id = @id";
                return conn.Execute(query, cat);
            }
        }

        //删除一个类别:
        public static int DeleteColumnCat(ColumnCat cat)
        {
            Connection connection = new Connection();
            using (IDbConnection conn = connection.OpenConnection())
            {
                const string query = "delete from ColumnCat where id=@id";
                return conn.Execute(query, cat);
            }
        }

        /// <summary>
        /// Dapper对事务处理的例子,如删除类别的同时删除类别下的所有新闻。或者删除产品的同时，删除产品图片表里关联的所有图片。
        /// </summary>
        /// <param name="cat"></param>
        /// <returns></returns>
        public int DeleteColumnCatAndColumn(ColumnCat cat)
        {
            Connection connection = new Connection();
            using (IDbConnection conn = connection.OpenConnection())
            {
                const string deleteColumn = "delete from [Column] where ColumnCatid=@catid";
                const string deleteColumnCat = "delete from ColumnCat where id=@Id";

                IDbTransaction transaction = conn.BeginTransaction();
                int row = conn.Execute(deleteColumn, new { catid = cat.Id }, transaction, null, null);
                row += conn.Execute(deleteColumnCat, new { id = cat.Id }, transaction, null, null);
                transaction.Commit();
                return row;
            }
        }


    }

}


