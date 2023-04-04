
using OPCAutomation;
using OPCDALIB.LIB.Entity;
using System;
using System.Collections.Generic;
using System.Text;

namespace OPCDALIB.LIB.OPCDA
{
    public class DA
    {
        //定义OPC服务器
        private  OPCServer server;
        //定义OPC服务器Groups
        private  OPCGroups KepGroups;
        //定义OPC服务器Group
        private  OPCGroup KepGroup;
        //定义OPC服务器Brower
        private  OPCBrowser KepBrower;
        //定义OPC服务器Items
        private  OPCItems KepItems;


        //定义OPC变量集合
        private  List<OPCAutomation.OPCItem> OPCList = new List<OPCAutomation.OPCItem>();
        private List<string> TempIDList = new List<string>();
        private List<int> ClientHandles = new List<int>();
        private List<int> ServerHandles = new List<int>();

        //错误标签
        private  Array iErrors;

        //定义要添加的OPC标签的标识符
        private  Array strTempIDs;
        private  Array strClientHandles;


        private  Array strServerHandles;
        private  Array readServerHandles;
        private  Array readErrors;


        private  Array writeServerHandles;
        private  Array writeArrayHandles;
        private  Array writeErrors;

        private  int readTransID;
        private  int writeTransID;
        private  int readCancelID;
        private  int writeCancelID;


        public void GetServers()
        {
            server = new OPCServer();
            object serverList = server.GetOPCServers("localhost");
            List<string> servers = new List<string>();
            foreach (string item in (Array)serverList)
            {
                servers.Add(item.Trim());
                Console.WriteLine(item);
            }
        }
    }
}
