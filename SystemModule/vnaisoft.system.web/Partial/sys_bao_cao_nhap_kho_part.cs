using quan_ly_kho.common.Models;
using quan_ly_kho.system.data.Models;
using System.Collections.Generic;

namespace quan_ly_kho.system.web.Controller
{
    partial class bao_cao_nhap_khoController
    {
        public static ControllerAppModel declare = new ControllerAppModel()
        {
            controller = "bao_cao_nhap_kho",
            icon = "badge",
            icon_image = "/assets/images/shungo/blue_feed_icon.png",
            module = "bao_cao",
            id = "bao_cao_nhap_kho",
            url = "/bao_cao_nhap_kho_index",
            title = "bao_cao_nhap_kho",
            translate = "NAV.bao_cao_nhap_kho",
            type = "item",
            //  is_show_all_user = true,
            list_controller_action_public = new List<string>(){

                       "bao_cao_nhap_kho;exportExcel",
            },
            list_controller_action_publicNonLogin = new List<string>()
            {


            },
            list_role = new List<ControllerRoleModel>()
            {

                  new ControllerRoleModel()
                {
                    id="bao_cao_nhap_kho;list",
                    name="list",
                    list_controller_action = new List<string>()
                    {
                          "bao_cao_nhap_kho;DataHandler",
                    }
                }
            }
        };
        public string CheckErrorImport(bao_cao_nhap_kho_model model, int ct, string error)
        {

            return error;
        }



    }
}
