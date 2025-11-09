import { Route } from '@angular/router';
import { sys_group_user_indexComponent } from './sys_group_user/index.component';
import { sys_user_indexComponent } from './sys_user/index.component';
import { sys_cau_hinh_anh_mac_dinh_indexComponent } from './sys_cau_hinh_anh_mac_dinh/index.component';
import { sys_template_mail_indexComponent } from './sys_template_mail/index.component';
import { sys_cau_hinh_ma_he_thong_indexComponent } from './sys_cau_hinh_ma_he_thong/index.component';
import { sys_loai_mat_hang_indexComponent } from './sys_loai_mat_hang/index.component';
import { sys_don_vi_tinh_indexComponent } from './sys_don_vi_tinh/index.component';
import { sys_khach_hang_nha_cung_cap_indexComponent } from './sys_khach_hang_nha_cung_cap/index.component';
import { sys_mat_hang_indexComponent } from './sys_mat_hang/index.component';
import { sys_don_hang_mua_indexComponent } from './sys_don_hang_mua/index.component';
import { sys_don_hang_ban_indexComponent } from './sys_don_hang_ban/index.component';
import { sys_phieu_nhap_kho_indexComponent } from './sys_phieu_nhap_kho/index.component';
import { sys_phieu_xuat_kho_indexComponent } from './sys_phieu_xuat_kho/index.component';
import { bao_cao_ton_kho_mat_hang_indexComponent } from './sys_bao_cao_ton_kho/index.component';
import { bao_cao_nhap_kho_indexComponent } from './sys_bao_cao_nhap_kho/index.component';
import { bao_cao_xuat_kho_indexComponent } from './sys_bao_cao_xuat_kho/index.component';

export const systemsRoutes: Route[] = [  

    {
        path: 'bao_cao_xuat_kho_index',
        component: bao_cao_xuat_kho_indexComponent,
    },
    {
        path: 'bao_cao_nhap_kho_index',
        component: bao_cao_nhap_kho_indexComponent,
    },
    {
        path: 'bao_cao_ton_kho_mat_hang_index',
        component: bao_cao_ton_kho_mat_hang_indexComponent,
    },
    {
        path: 'sys_phieu_xuat_kho_index',
        component: sys_phieu_xuat_kho_indexComponent,
    },
    {
        path: 'sys_phieu_nhap_kho_index',
        component: sys_phieu_nhap_kho_indexComponent,
    },
    {
        path: 'sys_don_hang_ban_index',
        component: sys_don_hang_ban_indexComponent,
    },
    {
        path: 'sys_don_hang_mua_index',
        component: sys_don_hang_mua_indexComponent,
    },
    {
        path: 'sys_mat_hang_index',
        component: sys_mat_hang_indexComponent,
    },
    {
        path: 'sys_khach_hang_nha_cung_cap_index',
        component: sys_khach_hang_nha_cung_cap_indexComponent,
    },
    {
        path: 'sys_don_vi_tinh_index',
        component: sys_don_vi_tinh_indexComponent,
    },
    {
        path: 'sys_loai_mat_hang_index',
        component: sys_loai_mat_hang_indexComponent,
    },
    {
        path: 'sys_cau_hinh_ma_he_thong_index',
        component: sys_cau_hinh_ma_he_thong_indexComponent,
    },
    {
        path: 'sys_user_index',
        component: sys_user_indexComponent,
    },
    {
        path: 'sys_group_user_index',
        component: sys_group_user_indexComponent,
    },
    {
        path: 'sys_cau_hinh_anh_mac_dinh_index',
        component: sys_cau_hinh_anh_mac_dinh_indexComponent,
    },
    {
        path: 'sys_template_mail_index',
        component: sys_template_mail_indexComponent,
    },
];
