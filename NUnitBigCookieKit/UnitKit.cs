using BigCookieKit;
using BigCookieKit.File;
using BigCookieKit.Office;
using BigCookieKit.Resources;
using BigCookieKit.XML;
using BigCookieSequelize;
using MiniExcelLibs;
using MySql.Data.MySqlClient;
using Newtonsoft.Json;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;
using System.Xml.Linq;

namespace NUnitBigCookieKit
{
    public class UnitKit
    {
        public readonly string resource = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Resources");

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

        /// <summary>
        /// 空值断言单元测试
        /// </summary>
        [Test]
        public void NullAssertUnit()
        {
            //类型
            VerifyModel model = null;
            model.IsNull();
            model.NotNull();

            //引用类型
            string str = "";
            //默认值和NULL都为true
            str.IsNull();
            str.NotNull();
            str.IsNull(false);
            str.NotNull(false);

            //值类型
            int value = 0;
            //默认值和NULL都为true
            value.IsNull();
            value.IsNull(false);
            value.NotNull();
            value.NotNull(false);

            Assert.IsTrue(true);
        }

        /// <summary>
        /// 获取特性附带值单元测试
        /// </summary>
        [Test]
        public void EnumDisplayUnit()
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

        /// <summary>
        /// 集合断言单元测试
        /// </summary>
        [Test]
        public void ExistCountAssertUnit()
        {
            List<string> test = new List<string>();
            test.Exist();
            test.NotExist();

            ICollection<string> test1 = new List<string>();
            test1.Exist();
            test1.NotExist();

            IEnumerable<string> test2 = new List<string>();
            test2.Exist();
            test2.NotExist();

            Dictionary<string, string> test3 = new Dictionary<string, string>();
            test3.Exist();
            test3.NotExist();

            DataTable test4 = new DataTable();
            test4.Exist();
            test4.NotExist();

            DataSet test5 = new DataSet();
            test5.Exist();
            test5.NotExist();

            Assert.IsTrue(true);
        }

        /// <summary>
        /// DataRow安全取值赋值单元测试
        /// </summary>
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
            dr.CellSetValue("Filed2", null);//自动会解决问题

            dt.Rows.Add(dr);
        }

        /// <summary>
        /// Xml转成实体单元测试
        /// </summary>
        [Test]
        public void XmlToEntityUnit()
        {
            var ele = XElement.Parse($@"
<Root attr1=""attr1"" attr2=""attr2"">
	<Node attr3=""attr3"" attr4=""attr4"">
		<Field attr5=""attr5"" attr6=""attr6"" />
		<Field attr5=""attr5"" attr6=""attr6"" />
		<Field attr5=""attr5"" attr6=""attr6"" />
	</Node>
</Root>");

            XmlRoot test1 = ele.XmlToEntity<XmlRoot>();
        }

        /// <summary>
        /// 金额数字转大写单元测试
        /// </summary>
        [Test]
        public void MoneyToUpperUnit()
        {
            decimal test = 8436.44868M;

            string temp = test.MoneyUpper();
        }

        /// <summary>
        /// 尝试转换单元测试
        /// </summary>
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

        /// <summary>
        /// 基础类型断言单元测试
        /// </summary>
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

        /// <summary>
        /// Xml流读取Excel2007转集合单元测试
        /// </summary>
        [Test]
        public void XmlReadSetUnit()
        {
            string path = Path.Combine(resource, "test.xlsx");
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

        /// <summary>
        /// Xml流读取Excel2007转字典单元测试
        /// </summary>
        [Test]
        public void XmlReadDictionaryUnit()
        {
            string path = Path.Combine(resource, "test.xlsx");
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

        /// <summary>
        /// Xml流读取Excel2007转DataTable单元测试
        /// </summary>
        [Test]
        public void XmlReadDataTableUnit()
        {
            string path = Path.Combine(resource, "test.xlsx");
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

        /// <summary>
        /// Actor模型（并集线程）单元测试
        /// </summary>
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

        /// <summary>
        /// 阻塞并行单元测试
        /// </summary>
        [Test]
        public void ActionBlockUnit()
        {
            ThreadPool.SetMinThreads(100, 100);
            var block = new ActionBlock<int>(index =>
            {
                Console.WriteLine(index);
                Task.Delay(500);
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

        /// <summary>
        /// 在一堆集合中找出一组连续性集合的位置单元测试
        /// </summary>
        [Test]
        public void FastIndexOfUnit()
        {
            int[] t1 = new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28, 29, 30, 31, 32, 33 };
            int[] t2 = new int[] { 8, 9, 10, 11 };

            var index = Kit.FastIndexOf(t1, t2);
        }

        /// <summary>
        /// 深拷贝(性能飘逸)单元测试
        /// </summary>
        [Test]
        public void DeepCopyUnit()
        {
            //模型Copy
            DeepCopyModel model = new DeepCopyModel();
            model.Field1 = "klsdjflsdlflsdf";
            model.Field2 = 100;
            model.Field3 = 101;
            model.Field4 = 105;
            model.Field5 = 106;
            model.Field6 = 106.45f;
            model.Field7 = 106.46f;
            model.Field8 = 106.4646598d;
            model.Field9 = 106.4646599d;
            model.Field10 = 106.46465996548m;
            model.Field11 = 106.46465996549m;
            model.Field12 = DateTime.Now;
            model.Field13 = DateTime.Now.AddDays(1);
            model.Field14 = TimeSpan.FromSeconds(30);
            model.Field15 = TimeSpan.FromSeconds(40);
            model.Field16 = TestEnum.True;
            model.Field17 = TestEnum.False;
            model.Field18 = 'A';
            model.Field19 = 'B';
            model.Field20 = new byte[] { 0, 1, 2, 3, 4 };

            var newmodel = model.MapTo<DeepCopyModel, DeepCopyModel>();
            newmodel.Field2 = 5464646;

            //流Copy
            var stream = new MemoryStream();
            byte[] bytes = Encoding.UTF8.GetBytes("测试环境");
            stream.Write(bytes, 0, bytes.Length);
            var tstram = stream.MapTo<Stream, Stream>();

        }

        /// <summary>
        /// 使用命令行单元测试
        /// </summary>
        [Test]
        public void RunCmdUnit()
        {
            var str = Kit.RunShell("ipconfig");
        }

        /// <summary>
        /// 操作Ini文件
        /// </summary>
        [Test]
        public void IniMapUnit()
        {
            string path = Path.Combine(resource, "iniTest.ini");
            IniMap iniMap = new IniMap(path);
            iniMap["nember"]["Name"] = "张炳彬";
            iniMap["nember1"]["Name1"] = "张炳彬";
            iniMap.Remove("nember");
            iniMap.Remove("nember1");
            iniMap.Save();
        }

        [Test]
        public void CommonUnit()
        {
            var info = Common.GetXlsxResource("[Content_Types].xml");
        }

        [Test]
        public void WriteExcelUnit()
        {
            string path1 = @"C:\Users\zbb58\Desktop\Execl测试\unitExcel1.xlsx";
            using (WriteExcelKit writeExcel = new WriteExcelKit(path1))
            {
                DataSet ds = new DataSet();
                DataTable dt = new DataTable();

                dt.Columns.Add("Field1");
                dt.Columns.Add("Field2");
                dt.Columns.Add("Field3");
                dt.Columns.Add("Field4");

                dt.Rows.Add(new object[] { "value1", "value2", "value3", "value4" });
                dt.Rows.Add(new object[] { "value1", "value2", "value3", "value4" });
                dt.Rows.Add(new object[] { "value1", "value2", "value3", "value4" });
                dt.Rows.Add(new object[] { "value1", "value2", "value3", "value4" });
                dt.Rows.Add(new object[] { "value1", "value2", "value3", "value4" });

                ds.Tables.Add(dt);

                DataTable dt1 = new DataTable();

                dt1.Columns.Add("Field5");
                dt1.Columns.Add("Field6");
                dt1.Columns.Add("Field7");
                dt1.Columns.Add("Field8");

                dt1.Rows.Add(new object[] { "value5", "value6", "value7", "value8" });
                dt1.Rows.Add(new object[] { "value5", "value6", "value7", "value8" });
                dt1.Rows.Add(new object[] { "value5", "value6", "value7", "value8" });
                dt1.Rows.Add(new object[] { "value5", "value6", "value7", "value8" });
                dt1.Rows.Add(new object[] { "value5", "value6", "value7", "value8" });

                ds.Tables.Add(dt1);

                writeExcel.Save(ds);
            }
        }


        #region 性能比较单元测试

        [Test]
        public void MiniExcelUnit()
        {
            string path = Path.Combine(resource, "test.xlsx");
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

        #endregion

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

        public class XmlRoot
        {
            public string attr1 { get; set; }
            public string attr2 { get; set; }
            public List<XmlNode> Node { get; set; }
        }

        public class XmlNode
        {
            public string attr3 { get; set; }
            public string attr4 { get; set; }
            public List<XmlField> Field { get; set; }
        }

        public class XmlField
        {
            public string attr5 { get; set; }
            public string attr6 { get; set; }
        }

        public class DeepCopyModel
        {
            public string Field1 { get; set; }
            public int Field2 { get; set; }
            public int? Field3 { get; set; }
            public long Field4 { get; set; }
            public long? Field5 { get; set; }
            public float Field6 { get; set; }
            public float? Field7 { get; set; }
            public double Field8 { get; set; }
            public double? Field9 { get; set; }
            public decimal Field10 { get; set; }
            public decimal? Field11 { get; set; }
            public DateTime Field12 { get; set; }
            public DateTime? Field13 { get; set; }
            public TimeSpan Field14 { get; set; }
            public TimeSpan? Field15 { get; set; }
            public TestEnum Field16 { get; set; }
            public TestEnum? Field17 { get; set; }
            public char Field18 { get; set; }
            public char? Field19 { get; set; }
            public byte[] Field20 { get; set; }
        }
    }
}