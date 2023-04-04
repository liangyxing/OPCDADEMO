using OPCAutomation;
using OPCDALIB.LIB.OPCDA;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    internal class Program
    {
        
        //定义OPC服务器
        private static OPCServer server;
        //定义OPC服务器Groups
        private static OPCGroups KepGroups;
        //定义OPC服务器Group
        private static OPCGroup KepGroup;
        //定义OPC服务器Brower
        private static OPCBrowser KepBrower;
        //定义OPC服务器Items
        private static OPCItems KepItems;


        //定义OPC变量集合
        public static List<OPCItem> OPCList = new List<OPCItem>();
       public static List<string> TempIDList = new List<string>();
       public static List<int> ClientHandles = new List<int>();
       public static List<int> ServerHandles = new List<int>();

        //错误标签
        private static Array iErrors;

        //定义要添加的OPC标签的标识符
        private static Array strTempIDs;
        private static Array strClientHandles;

        
        private static Array strServerHandles;
        private static Array readServerHandles;
        private static Array readErrors;


        private static Array writeServerHandles;
        private static Array writeArrayHandles;
        private static Array writeErrors;

        private static int readTransID;
        private static int writeTransID;
        private static int readCancelID;
        private static int writeCancelID;



        static void Main(string[] args)
        {
            #region
            {
                server = new OPCServer();
                object serverList = server.GetOPCServers("localhost");
                List<string> servers = new List<string>();
                foreach (string item in (Array)serverList)
                {
                    servers.Add(item.Trim());
                    Console.WriteLine(item);
                }

                server.Connect(servers[3], "localhost");

                //KepGroups对象赋值
                KepGroups = server.OPCGroups;
                //KepGroups属性设置
                KepGroups.DefaultGroupDeadband = 0;
                KepGroups.DefaultGroupIsActive = true;

                KepGroup = KepGroups.Add("Group1");
                KepGroup.IsActive = true;
                KepGroup.IsSubscribed = true;
                KepGroup.UpdateRate = 250;


                KepGroup.AsyncReadComplete += KepGroup_AsyncReadComplete;

                KepBrower = server.CreateBrowser();
                KepBrower.ShowBranches();
                KepBrower.ShowLeafs(true);

                foreach (object item in KepBrower)
                {
                    //Console.WriteLine(item);
                }
                //Machine05.Funcs.User.ModifyUser().Out.Error
                OPCList.Add(new OPCItem()
                {
                    Tag = "Machine05.Funcs.User.ModifyUser().Out.Error"
                });



                TempIDList.Clear();
                ClientHandles.Clear();
                TempIDList.Add("0");
                ClientHandles.Add(0);

                //需要读取点的数量
                int count = OPCList.Count;
                for (int i = 0; i < count; i++)
                {
                    TempIDList.Add(OPCList[i].Tag);
                    ClientHandles.Add(i + 1);
                }
                strTempIDs = TempIDList.ToArray();
                strClientHandles = (Array)ClientHandles.ToArray();
                KepItems = KepGroup.OPCItems;
                KepItems.AddItems(OPCList.Count, ref strTempIDs, ref strClientHandles, out strServerHandles, out iErrors);



                ServerHandles.Clear();
                ServerHandles.Add(0);
                for (int i = 0; i < count; i++)
                {
                    ServerHandles.Add(Convert.ToInt32(strServerHandles.GetValue(i + 1)));
                }
                readServerHandles = (Array)ServerHandles.ToArray();


                //opc da读取方法
                KepGroup.AsyncRead(OPCList.Count, ref readServerHandles, out readErrors, readTransID, out readCancelID);
            }
            #endregion

            {
                //DA
                DA dA=new DA();
                dA.GetServers();
            }
            Console.ReadLine();
        }

        

        //读取完成回调方法
        private static void KepGroup_AsyncReadComplete(int TransactionID, int NumItems, ref Array ClientHandles, ref Array ItemValues, ref Array Qualities, ref Array TimeStamps, ref Array Errors)
        {
            for (int i = 1; i <= NumItems; i++)
            {
                object Value = ItemValues.GetValue(i);
                if (Value != null)
                {
                    OPCList[i - 1].Value = Value.ToString();
                    OPCList[i - 1].Time = ((DateTime)TimeStamps.GetValue(i)).AddHours(8).ToLongTimeString();
                }
            }

            Console.WriteLine(NumItems);
        }

        private static void WritePoist()
        {
            int index = 0;
            int[] ServerHandle = new int[] { 0, Convert.ToInt32(strServerHandles.GetValue(index + 1)) };
            object[] Values = new object[2];
            writeServerHandles = (Array)ServerHandle;
            KepGroup.SyncWrite(1, writeServerHandles, writeArrayHandles, out writeErrors);
        }

        private static void GetMethod()
        {
          
        }
    }
}
