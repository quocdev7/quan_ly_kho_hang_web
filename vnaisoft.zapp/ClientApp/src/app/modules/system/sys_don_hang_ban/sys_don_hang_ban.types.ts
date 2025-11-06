export interface sys_don_hang_ban_col {
    id: string;
    ma: string;
    ten: string;
    ten_khong_dau: string;
    id_khach_hang_nha_cung_cap: string;
    phuong_thuc_thanh_toan: number | null;
    id_tai_khoan_ngan_hang: string;
    ma_ngan_hang: string;
    so_tai_khoan: string;
    ngay_dat_hang: Date | null;
    list_mat_hang: sys_don_hang_ban_mat_hang;
    tong_thanh_tien: number | null;
    ghi_chu: string;
    nguoi_cap_nhat: string;
    ngay_cap_nhat: Date | null;
    status_del: number | null;
}

export interface sys_don_hang_ban_model {
    db: sys_don_hang_ban_col;
    ten_nguoi_cap_nhat: string;
    ten_doi_tuong: string;
}
export interface sys_don_hang_ban_mat_hang {
    id: string;
    so_luong: number | null;
    don_gia: number | null;
    thanh_tien: number | null;
}
