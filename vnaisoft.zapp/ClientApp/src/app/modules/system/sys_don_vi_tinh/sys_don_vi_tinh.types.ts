
export interface sys_don_vi_tinh_col {
    id: string;
    ma: string;
    ten: string;
    ghi_chu: string;
    nguoi_tao: string;
    ngay_tao:Date | null;
    nguoi_cap_nhat: string;
    ngay_cap_nhat:Date | null;
    status_del: number | null;
}
export interface sys_don_vi_tinh_model {
    nguoi_tao: string;
    nguoi_cap_nhat: string;
    db: sys_don_vi_tinh_col;
}