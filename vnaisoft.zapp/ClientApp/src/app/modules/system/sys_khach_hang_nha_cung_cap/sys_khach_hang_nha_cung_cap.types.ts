export interface sys_khach_hang_nha_cung_cap_col {
    id: string;
    ma: string;
    laNhaCungCap: boolean | null;
    laKhachHang: boolean | null;
    hinh_thuc: number | null;
    ten: string;
    ten_khong_dau: string;
    ma_so_thue: string;
    dien_thoai: string;
    email: string;
    dia_chi: string;
    nguoi_tao: string;
    ngay_tao: Date | null;
    nguoi_cap_nhat: string;
    ngay_cap_nhat: Date | null;
    status_del: number | null;
    ghi_chu: string;
    so_tai_khoan: string;
    id_ngan_hang: string;
}
export interface sys_khach_hang_nha_cung_cap_model {
    nguoi_cap_nhat: string;
    ten_hinh_thuc: string;
    db: sys_khach_hang_nha_cung_cap_col;
}