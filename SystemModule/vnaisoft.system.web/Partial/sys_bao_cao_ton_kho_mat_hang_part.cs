using System.Collections.Generic;
using vnaisoft.common.Models;
using vnaisoft.system.data.Models;

namespace vnaisoft.system.web.Controller
{
    partial class bao_cao_ton_kho_mat_hangController
    {
        public static ControllerAppModel declare = new ControllerAppModel()
        {
            controller = "bao_cao_ton_kho_mat_hang",
            icon = "badge",
            icon_image = "/assets/images/shungo/blue_feed_icon.png",
            module = "bao_cao",
            id = "bao_cao_ton_kho_mat_hang",
            url = "/bao_cao_ton_kho_mat_hang_index",
            title = "bao_cao_ton_kho_mat_hang",
            translate = "NAV.bao_cao_ton_kho_mat_hang",
            type = "item",
            //  is_show_all_user = true,
            list_controller_action_public = new List<string>(){

                     "bao_cao_ton_kho_mat_hang;DataHandler",

                       "bao_cao_ton_kho_mat_hang;exportExcel",
            },
            list_controller_action_publicNonLogin = new List<string>()
            {


            },
            list_role = new List<ControllerRoleModel>()
            {

                  new ControllerRoleModel()
                {
                    id="bao_cao_ton_kho_mat_hang;list",
                    name="list",
                    list_controller_action = new List<string>()
                    {
                          "bao_cao_ton_kho_mat_hang;DataHandler",
                    }
                }
            }
        };
        public string CheckErrorImport(bao_cao_ton_kho_mat_hang_model model, int ct, string error)
        {

            return error;
        }



    }
}

