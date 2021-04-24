using BigCookieKit;
using BigCookieKit.Office;
using BigCookieKit.XML;
using BigCookieSequelize;
using MiniExcelLibs;
using MySql.Data.MySqlClient;
using Newtonsoft.Json;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading;
using System.Threading.Tasks.Dataflow;
using System.Xml.Linq;

namespace NUnitBigCookieKit
{
    public class UnitKit
    {
        [SetUp]
        public void Setup()
        {
        }

        /// <summary>
        /// 模型类校验单元测试
        /// </summary>
        [Test]
        public void VerifyUnit()
        {
            VerifyModel model = new VerifyModel();
            //model.StrField = "jskdlfjlsdjlflsdlfjlsdlkf";
            //model.IntField = 10;
            //model.DecimalField = 10.5264m;

            Assert.IsTrue(model.ModelValidation());
        }

        [Test]
        public void NullAssertUnit()
        {
            //引用类型
            string test = null;
            //默认值和NULL都为true
            test.IsNull();
            test.NotNull();

            //值类型
            int test1 = 0;
            //默认值和NULL都为true
            test1.IsNull(false);
            test1.NotNull(false);

            Assert.IsTrue(true);
        }

        [Test]
        public void EnumRemarkUnit()
        {
            TestEnum test = TestEnum.None;
            TestEnum test1 = TestEnum.True;
            TestEnum test2 = TestEnum.False;

            //""
            test.ToDisplay();
            //"正确"
            test1.ToDisplay();
            //"错误"
            test2.ToDisplay();

            Assert.IsTrue(true);
        }

        [Test]
        public void ExistCountAssertUnit()
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
        public void DataRowSetValueUnit()
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
        public void XmlToEntityUnit()
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
        public void MoneyToUpperUnit()
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
        public void TypeAssertUnit()
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
        public void SequelizeUnit()
        {
            using (MySqlConnection connection = new MySqlConnection("Server=localhost;Port=3306;Database=dbTest;Uid=root;Pwd=zhangbingbin5896;"))
            {
                var Obj = SequelizeStructured.SequelizeConfig(new
                {
                    fields = new[] { "UserId", "LogonName", "Password", "ExternalId", "CustomerId", "NickName", "FirstName", "LastName", "Phone", "Email", "Gender", "ActiveDate", "LastLogonTime", "InvitateCode", "ExpirationDate", "Source", "AccountType", "Status", "Disable", "CreateUserId", "CreateTime", "UpdateUserId", "UpdateTime", "Version" },
                    model = "cus_user",
                    where = new
                    {
                        eq = new { UserId = 135686 }
                    },
                    include = new[] {
                        new{
                            fields = new string[] { "UserFieldId","UserId","FieldId","DataValue","Disable","CreateTime","UpdateTime","CreateUserId","UpdateUserId","Version" },
                            model = "cus_userfield",
                            join = new{
                                cus_user = "UserId",
                                cus_userfield = "UserId"
                            }
                        }
                    }
                }, connection);
                string json = JsonConvert.SerializeObject(Obj);
            }
        }

        [Test]
        public void ZipUnit()
        {
            Kit.DirToFormZipPacket(@"C:\Users\zbb58\Desktop\test.zip", @"C:\Users\zbb58\Desktop\Program");
            //Kit.FileToFormZipPacket(@"C:\Users\zbb58\Desktop\test.zip", @"C:\Users\zbb58\Desktop\Excel测试文件\test.xlsx");
        }

        [Test]
        public void XmlReadUnit()
        {
            string path = @"C:\Users\zbb58\Desktop\sheet1.xml";
            XmlReadKit xmlReadKit = new XmlReadKit(path);
            //如果单应用于Xml结构 没有属性 设置不读属性 性能提升大
            xmlReadKit.IsReadAttributes = false;
            var packet = xmlReadKit.XmlRead("sheetData");
        }

        [Test]
        public void XmlReadSetUnit()
        {
            string path = @"C:\Users\zbb58\Desktop\Excel测试文件\test.xlsx";
            ReadExcelKit excelKit = new ReadExcelKit(path);
            excelKit.AddConfig(config =>
            {
                config.SheetIndex = 1;
                config.StartRow = 1;
                //config.EndRow = 100;
                //config.StartColumn = "B";
                //config.EndColumn = "B";
            });
            var rows = excelKit.XmlReaderSet();
        }

        [Test]
        public void XmlReadDictionaryUnit()
        {
            string path = @"C:\Users\zbb58\Desktop\Excel测试文件\test.xlsx";
            ReadExcelKit excelKit = new ReadExcelKit(path);
            excelKit.AddConfig(config =>
            {
                config.SheetIndex = 1;
                config.ColumnNameRow = 1;
                config.StartRow = 2;
                //config.EndRow = 100;
                //config.StartColumn = "B";
                //config.EndColumn = "B";
            });
            var dics = excelKit.XmlReaderDictionary();
        }

        [Test]
        public void XmlReadDataTableUnit()
        {
            string path = @"C:\Users\zbb58\Desktop\Excel测试文件\test.xlsx";
            ReadExcelKit excelKit = new ReadExcelKit(path);
            excelKit.AddConfig(config =>
            {
                config.SheetIndex = 1;
                //config.ColumnNameRow = 1;
                config.StartRow = 2;
                //config.EndRow = 100;
                config.ColumnSetting = new[] {
                    new ColumnConfig(){ ColumnName="Id", ColumnType=typeof(int), NormalType= ColumnNormal.Increment },
                    new ColumnConfig(){ ColumnName="UniqueNo", ColumnType=typeof(Guid), NormalType= ColumnNormal.Guid },
                    new ColumnConfig(){ ColumnName="CreateTime", ColumnType=typeof(DateTime), NormalType= ColumnNormal.NowDate },
                    new ColumnConfig(){ ColumnName="测试1", ColumnType=typeof(string), Column="A" },
                    new ColumnConfig(){ ColumnName="测试2", ColumnType=typeof(string), Column="B" },
                };
            });
            DataTable dt = excelKit.XmlReadDataTable();
        }

        [Test]
        public void ActorModelUnit()
        {
            var batch = new ActorModel<int>(100, index =>
            {
                foreach (var item in index)
                {
                    Console.WriteLine(item);
                }
                Thread.Sleep(500);
            });
            for (int i = 0; i < 400; i++)
            {
                batch.Post(i);
            }
            batch.Complete(true);
            Console.WriteLine("完成");
        }

        [Test]
        public void ActionBlockUnit()
        {
            ThreadPool.SetMinThreads(100, 100);
            var block = new ActionBlock<int>(index =>
            {
                Console.WriteLine(index);
                Thread.Sleep(500);
            }, new ExecutionDataflowBlockOptions()
            {
                MaxDegreeOfParallelism = 100,
            });
            for (int i = 0; i < 400; i++)
            {
                block.Post(i);
            }
            block.Complete();
            block.Completion.Wait();
            Console.WriteLine("完成");
        }

        [Test]
        public void FastIndexOfUnit()
        {
            int[] t1 = new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28, 29, 30, 31, 32, 33 };
            int[] t2 = new int[] { 8, 9, 10, 11 };

            var index = Kit.FastIndexOf(t1, t2);
        }

        [Test]
        public void MiniExcelUnit()
        {
            string path = @"C:\Users\zbb58\Desktop\Excel测试文件\test.xlsx";
            var rows = MiniExcel.Query(path);

            //自行转换成DataTable
            DataTable dt = new DataTable();
            foreach (var item in rows)
            {
                if (dt.Columns.Count == 0)
                {
                    foreach (var item1 in (IDictionary<string, object>)item)
                        dt.Columns.Add(item1.Key);
                }
                else
                {
                    DataRow ndr = dt.NewRow();
                    foreach (var item1 in (IDictionary<string, object>)item)
                    {
                        if (dt.Columns[item1.Key].DataType != item1.Value.GetType())
                            dt.Columns[item1.Key].DataType = item1.Value.GetType();
                        ndr[item1.Key] = item1.Value;
                    }
                    dt.Rows.Add(ndr);
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
            [Display("正确")]
            True,
            [Display("错误")]
            False
        }

        public class VerifyModel
        {
            [RequiredRule("DateTimeFieldName", Message = "{0}{3}")]
            public DateTime? DateTimeField { get; set; }

            [StringRule("StrFieldName", MaxLength = 10, Message = "{1}的{3}", Required = true)]
            public string StrField { get; set; }

            [NumericRule("IntFieldName", Equal = 10, Message = "{1}的{3}错误")]
            public int? IntField { get; set; }

            [DecimalRule("DecimalFieldName", Equal = 10.526, Precision = 3, Message = "{1}的{3}错误")]
            public decimal? DecimalField { get; set; }
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
}