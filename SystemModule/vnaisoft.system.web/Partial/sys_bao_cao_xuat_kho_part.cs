//using MongoDB.Driver;
//using NPOI.SS.Formula.Functions;
//using Opc.Ua;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using vnaisoft.common.BaseClass;
//using vnaisoft.common.Models;
//using vnaisoft.system.data.Models;

//namespace vnaisoft.system.web.Controller
//{
//    partial class bao_cao_xuat_khoController
//    {
//        public static ControllerAppModel declare = new ControllerAppModel()
//        {
//            controller = "bao_cao_xuat_kho",
//            icon = "badge",
//            icon_image = "/assets/images/shungo/blue_feed_icon.png",
//            module = "bao_cao",
//            id = "bao_cao_xuat_kho",
//            url = "/bao_cao_xuat_kho_index",
//            title = "bao_cao_xuat_kho",
//            translate = "NAV.bao_cao_xuat_kho",
//            type = "item",
//            //  is_show_all_user = true,
//            list_controller_action_public = new List<string>(){

//                       "bao_cao_xuat_kho;exportExcel",
//            },
//            list_controller_action_publicNonLogin = new List<string>()
//            {


//            },
//            list_role = new List<ControllerRoleModel>()
//            {

//                  new ControllerRoleModel()
//                {
//                    id="bao_cao_xuat_kho;list",
//                    name="list",
//                    list_controller_action = new List<string>()
//                    {
//                          "bao_cao_xuat_kho;DataHandler",
//                    }
//                }
//            }
//        };
//        public string CheckErrorImport(bao_cao_xuat_kho_model model, int ct, string error)
//        {

//            return error;
//        }



//    }
//}
