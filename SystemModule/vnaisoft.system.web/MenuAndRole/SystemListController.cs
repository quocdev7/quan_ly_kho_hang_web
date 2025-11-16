using System.Collections.Generic;

using vnaisoft.common.Models;
using vnaisoft.system.web.Controller;
//using vnaisoft.so_chu_nhiem.web.Controller;

namespace vnaisoft.system.web.MenuAndRole
{
    public static class SystemListController
    {
        public static List<ControllerAppModel> listController = new List<ControllerAppModel>()

        {
            
            //hệ thống
            sys_userController.declare,
            sys_group_userController.declare,
            //scn_ban_can_bo_lopController.declare,
            sys_template_mailController.declare,
            //sys_videoController.declare,

            //Quản lý danh mục
            sys_don_vi_tinhController.declare,
            sys_loai_nhap_xuatController.declare,
            sys_loai_mat_hangController.declare,
            sys_mat_hangController.declare,
            sys_don_hang_muaController.declare,
            sys_don_hang_banController.declare,
            sys_phieu_nhap_khoController.declare,
            sys_phieu_xuat_khoController.declare,
            bao_cao_nhap_khoController.declare,
            bao_cao_xuat_khoController.declare,
            bao_cao_ton_kho_mat_hangController.declare,

        };
    }
}
