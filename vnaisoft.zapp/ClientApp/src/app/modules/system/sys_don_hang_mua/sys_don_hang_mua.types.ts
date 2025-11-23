export interface sys_don_hang_mua_model {
    db: sys_don_hang_mua_col;
    ten_nguoi_cap_nhat: string;
    list_mat_hang: sys_don_hang_mua_mat_hang_model[];
}

export interface sys_don_hang_mua_mat_hang_model {
    db: sys_don_hang_mua_mat_hang_col;
    ten_don_vi_tinh: string;
    ten_mat_hang: string;
}
export interface sys_don_hang_mua_col {
    id: string;
    ma: string;
    ten: string;
    ten_khong_dau: string;
    ngay_dat_hang: Date | null;
    tong_thanh_tien: number | null;
    ghi_chu: string;
    ngay_tao: Date | null;
    nguoi_tao: string;
    nguoi_cap_nhat: string;
    ngay_cap_nhat: Date | null;
    status_del: number | null;
}
export interface sys_don_hang_mua_mat_hang_col {
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