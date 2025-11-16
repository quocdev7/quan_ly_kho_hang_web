export interface sys_loai_nhap_xuat_model {
    ten_nguoi_cap_nhat: string;
    ngay_cap_nhat_str: string;
    ten_loai: string;
    db: sys_loai_nhap_xuat_col;
}
export interface sys_loai_nhap_xuat_col {
    id: string;
    loai: number | null;
    ma: string;
    ten: string;
    ghi_chu: string;
    ngay_tao: Date | null;
    nguoi_tao: string;
    ngay_cap_nhat: Date | null;
    nguoi_cap_nhat: string;
    status_del: number | null;
    stt: number | null;
    nguon: string;
}