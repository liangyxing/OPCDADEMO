using Opc.Da;
using OpcCom;
using Opc;
using System.Security.Cryptography.X509Certificates;
using OpcRcw.Comn;
using System.Linq;
using OpcRcw.Da;
using OpcRcw.Batch;
using OPCAutomation;
using System.Xml.Linq;
using System.Collections.Generic;
using System.Formats.Asn1;
using CsvHelper;
using System.Globalization;

namespace OPCDADEMO
{
    internal class Program
    {
        public static List<List<string>> itemlist = new List<List<string>>();
        public static List<string> tempItemList = new List<string>();
        static void Main(string[] args)
        {
                    

            #region

            {
                // OpcCom.ServerEnumerator enumerator = new ServerEnumerator();
                // Opc.Server[] servers = enumerator.GetAvailableServers(Specification.COM_DA_20);
                // Opc.Server[] res = enumerator.GetAvailableServers(Specification.COM_DA_10);
                // Opc.Server[] res1 = enumerator.GetAvailableServers(Specification.COM_DA_30);
                // Opc.Server[] res2 = enumerator.GetAvailableServers(Specification.COM_BATCH_10);
                // Opc.Server[] res3 = enumerator.GetAvailableServers(Specification.COM_BATCH_20);
                // Opc.Server[] res4 = enumerator.GetAvailableServers(Specification.COM_AE_10);
                // Opc.Server[] res5 = enumerator.GetAvailableServers(Specification.COM_DX_10);
                // Opc.Server[] res6 = enumerator.GetAvailableServers(Specification.COM_HDA_10);
                //Console.WriteLine();


                //    foreach (Opc.Server DAserver in servers)
                //    {
                //        Console.WriteLine("Name: {0}", DAserver.Name);
                //        Console.WriteLine("Description: {0}", DAserver.Name);
                //        Console.WriteLine("--------------------");
                //    }
            }
            #endregion

            #region
            {
                //链接 指定opc da服务
                OpcCom.Factory factory = new OpcCom.Factory();
                Opc.Da.Server server = new Opc.Da.Server(factory, null);
                //Kepware.KEPServerEX.V6   Kemro.opc.4.IF1.1.94a
                server.Url = new Opc.URL("opcda://localhost/Kemro.opc.4.IF1.1.94a");
                server.Connect();


                Opc.Da.BrowseFilters filters = new Opc.Da.BrowseFilters();
                filters.BrowseFilter = Opc.Da.browseFilter.all;
                Opc.Da.BrowsePosition pos;

                Opc.Da.BrowseElement[] elements = server.Browse(null, filters, out pos);

                ItemIdentifier rootItem = new ItemIdentifier();
                foreach (var element in elements)
                {
                    Console.WriteLine(element.Name + " " + element.HasChildren + " " + element.Properties);

                                      
                }

                #region 遍历所有节点
                rootItem.ItemName = elements[0].Name;
                bool isChildren = elements[0].HasChildren;
                ChildrenItem(filters, rootItem, server, isChildren, elements[0].Name);


                using (var writer = new StringWriter())
                using (var csv = new CsvWriter(writer,CultureInfo.CurrentUICulture))
                {
                    csv.WriteRecords(tempItemList);
                    string csvText = writer.ToString();
                    // TODO: 将 csvText 输出到文件或其它位置
                }
                #endregion



                #region 读取指定节点数据
                ItemIdentifier rootItem1 = new ItemIdentifier();
                rootItem1.ItemName = "Machine05.SVs";

                Opc.Da.BrowseElement[] elementss = server.Browse(rootItem, filters, out pos);
                foreach (Opc.Da.BrowseElement element in elementss)
                {
                    Console.WriteLine(element.Name + " " + element.HasChildren + " " + element.Properties);

                }
                #endregion


                #region 调用方法节点
               

               #endregion




                    //设定组状态
                    var state = new Opc.Da.SubscriptionState();//组（订阅者）状态，相当于OPC规范中组的参数
                state.Name = "测试";//组名
                state.ServerHandle = null;//服务器给该组分配的句柄。
                state.ClientHandle = Guid.NewGuid().ToString();//客户端给该组分配的句柄。
                state.Active = true;//激活该组。
                state.UpdateRate = 100;//刷新频率为1秒。
                state.Deadband = 0;// 死区值，设为0时，服务器端该组内任何数据变化都通知组。
                state.Locale = null;//不设置地区值。
                //添加组
                var subscription = (Opc.Da.Subscription)server.CreateSubscription(state);
                string itemName = elements[0].Name;
                List<Item> items = new List<Item>();
                items.Add(new Item()
                {
                    ClientHandle = Guid.NewGuid().ToString(),
                    ItemPath = null,
                    ItemName = "Machine05.SVs.Core7.sv_bCoreIn"
                });
                var res = subscription.AddItems(items.ToArray());

                ItemValueResult[] values = subscription.Read(subscription.Items);

                foreach (var item in values)
                {
                    Console.WriteLine(item.Value);
                }
            }
            #endregion


            #region
            {
                //var server = new OPCServer();
                //object serverList = server.GetOPCServers("localhost");
                //Console.WriteLine();

            }
            #endregion
        }

        public static void ChildrenItem(BrowseFilters filters,ItemIdentifier rootItem,
        Opc.Da.Server server,bool HasChildren,string itemName)
        {
            Opc.Da.BrowseFilters filtersopc = new Opc.Da.BrowseFilters();
            filtersopc.BrowseFilter = Opc.Da.browseFilter.all;
            var Item = new ItemIdentifier();
            bool isChildren =HasChildren;
            Opc.Da.BrowsePosition pos;
            Item = rootItem;
            Opc.Da.BrowseElement[] elementss = server.Browse(Item, filtersopc, out pos);         
             foreach (Opc.Da.BrowseElement element in elementss)
             {                 
                
                
                Item.ItemName = element.ItemName;
                isChildren = element.HasChildren;
                //itemlist.Add(tempItemList);
                if (isChildren)
                {
                    ChildrenItem(filters, Item, server, isChildren, element.ItemName);

                }
                else
                {
                    var res = tempItemList.Contains(element.ItemName);
                    if (res==false)
                    {
                        Console.WriteLine(element.ItemName);
                        tempItemList.Add(element.ItemName);                       
                    }
                }
            }           
         }
    }


}
