using System.Collections.Generic;

namespace vnaisoft.common.Models
{
    public class sys_common_model
    {
        public string id { get; set; }
        public string name { get; set; }
        public string name_en { get; set; }
        public string ten_viet_tat { get; set; }
        public string id_parent { get; set; }

    }

    public class sys_common_new_model
    {
        public string id { get; set; }
        public int? value { get; set; }
        public string name { get; set; }


    }
    public class sys_common_value_model
    {

        public string id { get; set; }
        public string name { get; set; }
        public decimal? value { get; set; }

    }
    public class sys_common_ref_model
    {

        public int? id { get; set; }
        public string name { get; set; }



    }
    public class sys_common_check_model
    {
        public string? id { get; set; }
        public bool? isCheck { get; set; }

    }
    public class sys_common_group_model
    {
        public string id { get; set; }
        public string name { get; set; }
        public string id_nhom { get; set; }

    }
    public class erp_xuat_bao_cao_thue_sheet1_model
    {
        public string chi_tieu { get; set; }
        public string ma_so { get; set; }
        public string thuyet_minh { get; set; }
        public decimal? so_cuoi_nam { get; set; }
        public decimal? so_dau_nam { get; set; }
    }
    public class erp_xuat_bao_cao_thue_sheet4_model
    {
        public string ma_tai_khoan { get; set; }
        public string ten_tai_khoan { get; set; }
        public decimal? stDauKyNo { get; set; }
        public decimal? stDauKyCo { get; set; }
        public decimal? stPhatSinhNo { get; set; }
        public decimal? stPhatSinhCo { get; set; }
        public decimal? stCuoiKyNo { get; set; }
        public decimal? stCuoiKyCo { get; set; }
    }
    public class sys_select_common_group_model
    {
        public sys_select_common_group_model()
        {
            listData = new List<sys_common_model>();
        }
        public string id { get; set; }
        public string name { get; set; }
        public List<sys_common_model> listData { get; set; }
        public string id_loai_parent { get; set; }

    }
    public class cell
    {

        public string value { get; set; }
    }
    public class row
    {
        public row()
        {
            list_cell = new List<cell>();
        }


        public string key { get; set; }
        public List<cell> list_cell { get; set; }
    }

}

