import { Tracing } from "trace_events";


export interface sys_file_upload_col {
    id: string;
    id_phieu: string;
    ma_cong_viec: string;
    extension_file: string;
    file_name: string;
    file_path: string;
    file_path_download: string;
    file_size: number | null;
    create_time_micro_epoch: number | null;
    thumbnail_file: string;
    length_video: number | null;
    status_del: number | null;
    ngay_tao: Date | null;
    nguoi_tao: string;
    ngay_cap_nhat: Date | null;
    nguoi_cap_nhat: string;
}