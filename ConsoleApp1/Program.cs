using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using SQLite;

namespace SqlLiteTest
{
    class Program
    {
        static void Main(string[] args)
        {
            var db = new SQLiteConnection(@"C:\temp\test.s3db");
            //db.DeleteAll<Panel>();
            db.CreateTable<Panel>();
            db.CreateTable<Window>();
            var table = db.Table<Panel>();
            var windows =db.Table<Window>();
            foreach (var panel in table)
            {
                var win = db.Find<Window>(panel.WindowId);
            }
            db.CreateTable<Panel>();
            db.CreateTable<Window>();
            //var p = db.Find<Panel>(i=>i.Mark == "test");
            //p.Length = 10000;
            //p.Mark = "test1";
            //db.Update(p);
            db.BeginTransaction();
            for (var i = 0; i < 200; i++)
            {
                var win = new Window
                {
                    Height = i * DateTime.Now.Millisecond,
                    Width = i * DateTime.Now.Millisecond,
                };
                var winId = db.Insert(win);
                db.Insert(new Panel
                {
                    Mark = $"Марка {i}",
                    Length = i * DateTime.Now.Millisecond,
                    WindowId = winId
                });
            }
            db.Commit();
        }
    }

    public class Panel
    {
        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }
        public string Mark { get; set; }
        public double Length { get; set; }
        [Indexed]
        public int WindowId { get; set; }

        public override string ToString()
        {
            return $"{ID} {Mark} {Length}";
        }
    }

    public class Window
    {
        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }
        public double Height { get; set; }
        public double Width { get; set; }
    }

    public class TestTable122
    {
        public string OBJ { get; set; }
    }
}
