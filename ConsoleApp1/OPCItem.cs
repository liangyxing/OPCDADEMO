using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    public class OPCItem
    {
        /// <summary>
        /// OPCItem变量名称
        /// </summary>
        public string Tag { get; set; }

        /// <summary>
        /// OPCItem数值
        /// </summary>

        public string Value { get; set; }
        /// <summary>
        /// OPCItem时间戳
        /// </summary>
        public string Time { get; set; }
    }
}
