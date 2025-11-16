export interface sys_don_hang_ban_model {
    ten_nguoi_cap_nhat: string;
    db: sys_don_hang_ban_col;
    list_mat_hang: sys_don_hang_ban_mat_hang_col[];
}
export interface sys_don_hang_ban_mat_hang_col {
    id: string;
    id_don_hang: string;
    id_mat_hang: string;
    id_loai_mat_hang: string;
    ten_mat_hang: string;
    id_don_vi_tinh: string;
    so_luong: number | null;
    don_gia: number | null;
    vat: string;
    thanh_tien: number | null;
    ghi_chu: string;
    ngay_tao: Date | null;
    nguoi_tao: string;
    nguoi_cap_nhat: string;
    ngay_cap_nhat: Date | null;
    status_del: number | null;
}
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
    tong_thanh_tien: number | null;
    ghi_chu: string;
    ngay_tao: Date | null;
    nguoi_tao: string;
    nguoi_cap_nhat: string;
    ngay_cap_nhat: Date | null;
    status_del: number | null;
}