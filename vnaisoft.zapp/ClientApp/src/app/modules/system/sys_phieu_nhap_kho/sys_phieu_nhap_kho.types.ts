export interface sys_phieu_nhap_kho_model {
    db: sys_phieu_nhap_kho_db;
    ten_nguoi_cap_nhat: string;
    ten_loai_nhap: string;
    ma_loai_nhap: string;
    
    ten_kho: string;
    list_mat_hang: sys_phieu_nhap_kho_chi_tiet_model[];
    check_doi_tuong: number | null;
    so_luong_mh:number | null;
}
export interface sys_phieu_nhap_kho_db {
    id: string;
    ma: string;
    ten: string;
    nguon: number | null;
    id_phieu_chuyen_kho: string;
    id_don_hang_ban_thuc_hien: string;
    id_don_hang: string;
    ngay_nhap: Date ;
    id_kho: string;
    id_loai_nhap: string;
    nguoi_cap_nhat: string;
    ngay_cap_nhat: Date | null;
    status_del: number | null;
}
export interface sys_phieu_nhap_kho_chi_tiet_db {
    id: string;
    id_phieu_nhap_kho: string;
    id_mat_hang: string;
    id_loai_mat_hang: string;
    ten_mat_hang: string;
    ma_vach: string;
    id_don_vi_tinh: string;
    so_luong: number | null;
    ghi_chu: string;
    nguoi_cap_nhat: string;
    ngay_cap_nhat: Date | null;
}
export interface sys_phieu_nhap_kho_chi_tiet_model {
    db: sys_phieu_nhap_kho_chi_tiet_db;
    id_mat_hang: string;
    ten_thuoc_tinh: string;
    ten_don_vi_tinh: string;
    ten_mat_hang: string;
    ma_mat_hang: string;
    id_deatils_nhap_kho: string;
}