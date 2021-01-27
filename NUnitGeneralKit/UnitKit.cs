using GeneralKit;
using MySql.Data.MySqlClient;
using Newtonsoft.Json;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Xml.Linq;

namespace NUnitGeneralKit
{
    public class UnitKit
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void VerifyUnit()
        {
            //TestModel testModel = new TestModel();
            //testModel.Id = 100;//不填通不过
            ////testModel.Name = "张";//通不过
            ////testModel.Name = "张三";//通过
            ////testModel.Name = "张as";//通不过
            ////testModel.Name = "张三三";//通过
            ////testModel.Name = "张三三三三";//通不过
            //testModel.Old = 1;//通过
            ////testModel.Old = 12;//通不过
            //testModel.Old1 = 10;//通过
            ////testModel.Old1 = 8;//通不过
            //testModel.Old2 = 8;//通过
            //testModel.Old2 = 7;//通不过

            //testModel.ModelValidation();
            Assert.IsTrue(true);
        }

        [Test]
        public void NullAssert()
        {
            //引用类型
            string test = null;
            //默认值和NULL都为true
            test.IsNull();
            test.NotNull();

            //值类型
            int test1 = 0;
            //默认值和NULL都为true
            test1.IsNull();
            test1.NotNull();

            Assert.IsTrue(true);
        }

        [Test]
        public void EnumRemark()
        {
            TestEnum test = TestEnum.None;
            TestEnum test1 = TestEnum.True;
            TestEnum test2 = TestEnum.False;

            //""
            test.Remark();
            //"正确"
            test1.Remark();
            //"错误"
            test2.Remark();

            Assert.IsTrue(true);
        }

        [Test]
        public void ExistCountAssert()
        {
            List<string> test = new List<string>();
            ICollection<string> test1 = new List<string>();
            IEnumerable<string> test2 = new List<string>();
            Dictionary<string, string> test3 = new Dictionary<string, string>();

            test.Exist();
            test.NotExist();
            test1.Exist();
            test1.NotExist();
            test2.Exist();
            test2.NotExist();
            test3.Exist();
            test3.NotExist();

            Assert.IsTrue(true);
        }

        [Test]
        public void DataRowSetValue()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add(new DataColumn("Filed1", typeof(string)));
            dt.Columns.Add(new DataColumn("Filed2", typeof(decimal)));
            dt.Columns.Add(new DataColumn("Filed3", typeof(decimal)));

            DataRow dr = dt.NewRow();
            dr["Filed1"] = null;
            //dr["Filed2"] = null;//值类型赋值null会报错
            //dr.CellSetValue("Filed2", null);//自动会解决问题

            dt.Rows.Add(dr);
        }

        [Test]
        public void XmlToEntity()
        {
            var ele = XElement.Parse($@"
<Test1 attr1=""attr1"" attr2=""attr2"">
	<Test2 attr3=""attr3"" attr4=""attr4"">
		<Field attr5=""attr5"" attr6=""attr6"">
		</Field>
		<Field attr5=""attr5"" attr6=""attr6"">
		</Field>
		<Field attr5=""attr5"" attr6=""attr6"">
		</Field>
	</Test2>
</Test1>");

            Test1 test1 = ele.XmlToEntity<Test1>();
        }

        [Test]
        public void MoneyToUpper()
        {
            decimal test = 8436.44868M;

            string temp = test.MoneyUpper();
        }

        [Test]
        public void TryParseUnit()
        {
            string test = "15358";
            if (test.TryParse<int>(out var temp))
            {
            }

            if (test.TryParse<int>())
            {
            }
        }

        [Test]
        public void NpoiKitUnit()
        {
            string path = @"C:\Users\zbb58\Desktop\test.xlsx";
            NpoiKit npoiKit = new NpoiKit(path);
            npoiKit.CreateConfig(config =>
            {
                config.ColumnNameRow = 1;
                config.StartRow = 2;
                config.EndRow = 13;
                config.EndColumn = "B";
                //config.EndRow = 23;
            });
            DataTable dt = npoiKit.ToDataTable(npoiKit.GetSheet("Sheet1"));
        }

        [Test]
        public void TypeAssert()
        {
            string classObj = "";
            bool test1 = classObj.IsClass();
            bool test2 = classObj.IsValue();
            bool test3 = classObj.IsStruct();
            bool test4 = classObj.IsEnum();

            int valueObj = 0;
            test1 = valueObj.IsClass();
            test2 = valueObj.IsValue();
            test3 = valueObj.IsStruct();
            test4 = valueObj.IsEnum();

            TestEnum enumObj = TestEnum.None;
            test1 = enumObj.IsClass();
            test2 = enumObj.IsValue();
            test3 = enumObj.IsStruct();
            test4 = enumObj.IsEnum();

            TestStruct structObj = new TestStruct();
            test1 = structObj.IsClass();
            test2 = structObj.IsValue();
            test3 = structObj.IsStruct();
            test4 = structObj.IsEnum();
        }

        [Test]
        public void ExcelKitUnit()
        {
            ReadExcelKit excelKit = new ReadExcelKit(@"C:\Users\zbb58\Desktop\Execl测试\测试Excel.xlsx");
            excelKit.CreateConfig(config =>
            {
                config.ColumnNameRow = 1;
                config.StartRow = 2;
            });
            DataTable dt = excelKit.ReadDataTable(1);
        }

        [Test]
        public void SequelizeUnit()
        {
            // 生成结构化Sql [2021-1-28 zhangbingbin]
            string sql = SequelizeStructured.SequelizeToSql(new
            {
                fields = new[] { "Id", "Name", "Height", "Weight", "Sex" },
                model = "user",
                where = new
                {
                    like = new { Name = DBNull.Value }
                },
                include = new[] {
                    new{
                        fields = new string[] { "UserId" },
                        model = "usermaping",
                        join = new{
                            user = "Id",
                            usermaping = "UserId"
                        },
                        include = new[] {
                            new{
                                fields = new[] { "Id","WorkType","WorkName","Salary" },
                                model = "work",
                                join = new{
                                    usermaping = "WorkId",
                                    work = "Id"
                                }
                            }
                        }
                    }
                }
            });

            using (MySqlConnection connection = new MySqlConnection("Server=localhost;Port=3306;Database=dbTest;Uid=root;Pwd=zhangbingbin5896;"))
            {
                // Table名设置成主表名 [2021-1-28 zhangbingbin]
                DataTable dt = new DataTable("user");
                MySqlDataAdapter adapter = new MySqlDataAdapter(sql, connection);
                adapter.Fill(dt);
                // 直接动态生成类型 [2021-1-28 zhangbingbin]
                var obj = dt.SequelizeDynamic();
            }
        }
    }

    public struct TestStruct
    {
        public string filed { get; set; }
    }

    public enum TestEnum
    {
        None,
        [Remark("正确")]
        True,
        [Remark("错误")]
        False
    }

    public class Test1
    {
        public string attr1 { get; set; }
        public string attr2 { get; set; }
        public List<Test2> Test2 { get; set; }
    }

    public class Test2
    {
        public string attr3 { get; set; }
        public string attr4 { get; set; }
        public List<Field> Field { get; set; }
    }

    public class Field
    {
        public string attr5 { get; set; }
        public string attr6 { get; set; }
    }
}