export interface sys_mat_hang_col {
    id: string;
    ma: string;
    ten: string;
    ten_khong_dau: string;
    tien_te: string;
    id_loai_mat_hang: string;
    id_don_vi_tinh: string;
    gia_ban_le: number | null;
    gia_ban_si: number | null;
    gia_von: number | null;
    ty_le_chiet_khau: number | null;
    vat: string;
    ghi_chu: string;
    nguoi_cap_nhat: string;
    ngay_cap_nhat: Date | null;
    status_del: number | null;
}
export interface sys_mat_hang_model {
    ten_loai_mat_hang: string;
    nguoi_tao: string;
    nguoi_cap_nhat: string;
    db: sys_mat_hang_col;
}