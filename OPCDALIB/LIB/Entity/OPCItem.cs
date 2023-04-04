using System;
using System.Collections.Generic;
using System.Text;

namespace OPCDALIB.LIB.Entity
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
