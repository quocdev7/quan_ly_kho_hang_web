using System;
using System.Collections.Generic;
using System.Text;
using vnaisoft.common.Models;

namespace vnaisoft.common.BaseClass
{
    public class ListControlller 
    {
        public static List<ControllerAppModel> list { get; set; }
        public static List<string> listpublicactioncontroller { get; set; }
        public static List<string> listnonloginpublicactioncontroller { get; set; }
    }
}
