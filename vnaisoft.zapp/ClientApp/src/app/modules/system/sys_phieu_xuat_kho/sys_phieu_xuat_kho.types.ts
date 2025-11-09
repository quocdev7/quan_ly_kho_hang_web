
export interface sys_phieu_xuat_kho_model {
    db: sys_phieu_xuat_kho_db;
    ten_nguoi_cap_nhat: string;
    ten_loai_xuat: string;
    ten_kho: string;
    ma_chuyen_kho: string;
    ma_don_hang: string;
    ma_loai_xuat: string;
    ten_nguon: string;
    list_mat_hang: sys_phieu_xuat_kho_chi_tiet_model[];
    check_doi_tuong: number | null;
}

export interface sys_phieu_xuat_kho_chi_tiet_model {
    db: sys_phieu_xuat_kho_chi_tiet_db;
    id_mat_hang: string;
    ma_mat_hang: string;
    ten_mat_hang: string;
    ten_don_vi_tinh: string;
    ten_thuoc_tinh: string;
    id_deatils_xuat_kho: string;
}
export interface sys_phieu_xuat_kho_db {
    id: string;
    ma: string;
    ten: string;
    ten_khong_dau: string;
    id_don_hang_ban_thuc_hien: string;
    id_don_hang_ban: string;
    id_don_hang_mua: string;
    nguon: number | null;
    hinh_thuc_doi_tuong: number | null;
    id_doi_tuong: string;
    ten_doi_tuong: string;
    ma_so_thue: string;
    dien_thoai: string;
    email: string;
    dia_chi_doi_tuong: string;
    ngay_xuat: Date | null;
    id_kho: string;
    id_loai_xuat: string;
    ghi_chu: string;
    nguoi_cap_nhat: string;
    ngay_cap_nhat: Date | null;
    status_del: number | null;
    id_file_upload: string;
    id_phieu_chuyen_kho: string;
    id_hoa_don: string;
    is_sinh_tu_dong: boolean | null;
    loai: number | null;
    so_phieu: string;
}export interface sys_phieu_xuat_kho_chi_tiet_db {
    id: string;
    id_phieu_xuat_kho: string;
    id_mat_hang: string;
    id_loai_mat_hang: string;
    ten_mat_hang: string;
    ma_vach: string;
    id_don_vi_tinh: string;
    so_luong: number | null;
    don_gia: number | null;
    gia_tri: number | null;
    don_gia_von: number | null;
    gia_tri_gia_von: number | null;
    ghi_chu: string;
    nguoi_cap_nhat: string;
    ngay_cap_nhat: Date | null;
    is_dinh_khoan: boolean | null;
    tai_khoan_no: string;
    doi_tuong_no: string;
    tai_khoan_co: string;
    doi_tuong_co: string;
    tai_khoan_no_gia_von: string;
    tai_khoan_co_gia_von: string;
    ngay_xuat: Date | null;
    status_del: number | null;
    id_kho: string;
}