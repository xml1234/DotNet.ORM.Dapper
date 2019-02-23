using System;
using System.Collections.Generic;
using System.Linq;


namespace ORM.Dapper
{
    class Program
    {
        static void Main(string[] args)
        {
            //查询
            //List<ColumnCat> AllColumnCat = SelectColumnCats().ToList<ColumnCat>();
            //foreach (ColumnCat cat in AllColumnCat.Where(c => c.Parentid == 0))
            //{
            //    Console.WriteLine("Name==>" + cat.Name + "\t");
            //    Console.WriteLine("时间==>" + cat.ModifiedOn + "\t");

            //    foreach (ColumnCat c in AllColumnCat
            //        .Where<ColumnCat>(subColumnCat => subColumnCat.Parentid == cat.Id))
            //    {
            //        Console.WriteLine();
            //        Console.WriteLine("Name==>" + c.Name + "\t");
            //        Console.WriteLine("时间==>" + c.ModifiedOn + "\t");
            //    }
            //}

            ////添加
            //ColumnCat cc = new ColumnCat();
            //cc.Name = "测试2";
            //cc.ModifiedOn=DateTime.Now;
            //cc.Parentid = 1;
            //InsertColumnCat(cc);

            //更新
            //ColumnCat cc = new ColumnCat();
            //cc.Id = 1;
            //cc.Name = "张三";
            //cc.ModifiedOn = DateTime.Now;
            //cc.Parentid = 1;
            //UpdateColumnCat(cc);

            ////删除
            //ColumnCat cc = new ColumnCat();
            //cc.Id = 2;
            //DeleteColumnCat(cc);


            List<ColumnCat> AllColumnCat = DBDapper.SelectColumnCats().ToList<ColumnCat>();
            foreach (ColumnCat cat in AllColumnCat)
            {
                Console.WriteLine();
                Console.WriteLine("Name==>" + cat.Name + "\t");
                Console.WriteLine("时间==>" + cat.ModifiedOn + "\t");
            }
            Console.ReadKey();
        }


    }


    public class NewId
    {
        public int Id { get; set; }
    }
}
