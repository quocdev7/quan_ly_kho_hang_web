export interface sys_phieu_nhap_kho_model {
    db: sys_phieu_nhap_kho_col;
    tong_so_luong: number | null;
    ten_nguoi_cap_nhat: string;
    ngay_cap_nhap_str: string;
    ngay_nhap_str: string;
    ngay_nhap: string;
    ten_loai_nhap: string;
    ma_mat_hang: string;
    ten_mat_hang: string;
    ma_don_hang: string;
    ma_loai_nhap: string;
    ten_hinh_thuc: string;
    ten_nguon: string;
    list_mat_hang: sys_phieu_nhap_kho_chi_tiet_model[];
    check_doi_tuong: number | null;
}
export interface sys_phieu_nhap_kho_col {
    id: string;
    ma: string;
    ten: string;
    ten_khong_dau: string;
    nguon: number | null;
    id_don_hang_ban: string;
    id_don_hang_mua: string;
    ngay_nhap: Date | null;
    id_loai_nhap: string;
    ghi_chu: string;
    nguoi_cap_nhat: string;
    ngay_cap_nhat: Date | null;
    status_del: number | null;
}
export interface sys_phieu_nhap_kho_chi_tiet_model {
    db: sys_phieu_nhap_kho_chi_tiet_col;
    id_deatils_nhap_kho: string;
    ten_loai_nhap: string;
    trang_thai_str: string;
    ngay_cap_nhap_str: string;
    ten_nguoi_cap_nhat: string;
    ma_don_hang: string;
    ten_phieu_nhap: string;
    ghi_chu: string;
    id_mat_hang: string;
    id_don_vi_tinh: string;
    don_gia: number | null;
    ngay_nhap: Date | null;
    ten_don_vi_tinh: string;
    ten_mat_hang: string;
    ma_mat_hang: string;
    thanh_tien: number | null;
    gia_mua: number | null;
    don_gia_mua: number | null;
    don_gia_ban: number | null;
    id_don_hang_mua: string;
    id_loai_mat_hang: string;
    vat: string;
}
export interface sys_phieu_nhap_kho_chi_tiet_col {
    id: string;
    id_phieu_nhap_kho: string;
    id_mat_hang: string;
    id_loai_mat_hang: string;
    ten_mat_hang: string;
    id_don_vi_tinh: string;
    so_luong: number | null;
    don_gia: number | null;
    gia_tri: number | null;
    ghi_chu: string;
    nguoi_cap_nhat: string;
    ngay_nhap: Date | null;
    ngay_cap_nhat: Date | null;
    status_del: number | null;
}