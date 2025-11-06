import { Tracing } from "trace_events";
import { sys_file_upload_col } from "../sys_file_upload/sys_file_upload_col.types";

export interface sys_user_db {
    id: number;
    id_user: string;
    id_nhom: string;
    id_workspace: number | null;
    id_phong_ban: number | null;
    id_chuc_danh: number | null;
    create_by: string;
    create_date: string | null;
    update_by: string;
    update_date: string | null;
    status_del: number | null;
    quan_tri: number | null;

}
export interface sys_user_model {
    ho_va_ten: string;
    ten:string;
    id_nhom_quyen: string;
    id_phong_ban: string; 
    email: string;
    phone: string;
    nguoi_them: string;
    db: sys_user_db;
    hinh_anh_dai_dien:string;
    Username:string;
    password:string;
    file: sys_file_upload_col;
}