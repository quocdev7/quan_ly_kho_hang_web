using quan_ly_kho.common.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace quan_ly_kho.common.BaseClass
{
    public class ListControlller 
    {
        public static List<ControllerAppModel> list { get; set; }
        public static List<string> listpublicactioncontroller { get; set; }
        public static List<string> listnonloginpublicactioncontroller { get; set; }
    }
}
