import {
  compactNavigation,
  defaultNavigation,
  futuristicNavigation,
  horizontalNavigation,
} from 'app/mock-api/common/navigation/data';
import { Injectable } from '@angular/core';
import { ActivatedRouteSnapshot, Resolve, RouterStateSnapshot } from '@angular/router';
import { forkJoin, Observable } from 'rxjs';
import { map, switchMap } from 'rxjs/operators';
import { InitialData } from 'app/app.types';
import { cloneDeep } from 'lodash-es';
import { FuseNavigationItem } from '@fuse/components/navigation';
import { FuseMockApiService } from '@fuse/lib/mock-api';
import { HttpClient } from '@angular/common/http';
import { TranslocoService } from '@ngneat/transloco';
import { of } from 'rxjs';
import { AuthService } from './core/auth/auth.service';
import { AngularFirestore } from '@angular/fire/firestore';
import { UserService } from './core/user/user.service';
import { DataService } from '@fuse/services/data-service';
import Swal from 'sweetalert2';
import { da } from 'date-fns/locale';

@Injectable({
  providedIn: 'root',
})
export class InitialDataResolver implements Resolve<any> {
  public menu: any;
  public count_gv_cn: any;
  private _defaultNavigation: FuseNavigationItem[] = [];
  /**
   * Constructor
   */
  constructor(
    private _httpClient: HttpClient,
    private _fuseMockApiService: FuseMockApiService,
    public _translocoService: TranslocoService,
    private db: AngularFirestore,
    private _authService: AuthService,
    private dataService: DataService,
  ) {}

  // -----------------------------------------------------------------------------------------------------
  // @ Public methods
  // -----------------------------------------------------------------------------------------------------

  /**
   * Use this resolver to resolve initial mock-api for the application
   *
   * @param route
   * @param state
   */
  public showLoading(title: any = '', html: any = '', showClose: boolean = false) {
    if (title == '') title = this._translocoService.translate('system.vui_long_doi');
    if (html == '') html = this._translocoService.translate('system.dang_tai_du_lieu');
    Swal.fire({
      title: title,
      html: html,
      allowOutsideClick: false,
      showCloseButton: showClose,
      willOpen: () => {
        Swal.showLoading();
      },
    });
  }
  resolve(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): Observable<InitialData> {
    this.showLoading('', '', true);
    // Fork join multiple API endpoint calls to wait all of them to finish
    return forkJoin([
      this._httpClient.get<any>('api/common/messages'),
      this._httpClient
        .post('/sys_home.ctr/getModule/', {
          ma: JSON.parse(localStorage.getItem('code')),
          originalUrl: window.location.pathname, // Lấy URL gốc
        })
        .pipe(
          switchMap((resp: any) => {
            this.menu = resp;
            this._defaultNavigation = [];
            var menu_id = [];
            //var user = JSON.parse(localStorage.getItem('user'));
            debugger;
            //Hệ thống
            var he_thong = this.menu.filter((d) =>
              this.checkInclueFn(d.menu.id, [
                'sys_group_user',
                    'sys_phan_quyen_tai_khoan',
                    'sys_user',
                    'sys_cau_hinh_ma_he_thong',
                    'sys_cau_hinh_anh_mac_dinh',
                    'sys_template_mail',
                    //'sys_thong_bao',
              ]),
            );
            var menu_quan_ly_he_thong: FuseNavigationItem = {
                id: 'menu_quan_ly_he_thong',
                module: 'system',
                title: '',
                translate: 'administrative.administrative',
                type: 'group',
                icon: 'apps',
                icon_img: "/assets/icons/he_thong.png",
                children: [],
            };

            he_thong.forEach((item) => {
              menu_quan_ly_he_thong.children.push({
                id: item.menu.id,
                badge: item.menu.badge_approval,
                module: 'system',
                title: '',
                link: item.menu.url,
                translate: item.menu.translate,
                icon: item.menu.icon,
                type: 'basic',
              });
            });

            if (menu_quan_ly_he_thong.children.length > 0) {
              this._defaultNavigation.push(menu_quan_ly_he_thong);
            }
            //Quản lý danh mục
            var quan_ly_danh_muc = this.menu.filter((d) =>
              this.checkInclueFn(d.menu.id, [
                'sys_don_vi_tinh',
                // 'sys_khach_hang_nha_cung_cap',
                'sys_loai_mat_hang',
                'sys_mat_hang',
                'sys_don_hang_mua',
                'sys_don_hang_ban',
                'sys_phieu_nhap_kho',
                'sys_phieu_xuat_kho',
                'bao_cao_nhap_kho',
                'bao_cao_xuat_kho',
                'bao_cao_ton_kho_mat_hang',
              ]),
            );
            var menu_quan_ly_danh_muc: FuseNavigationItem = {
                id: 'quan_ly_danh_muc',
                module: 'system',
                title: '',
                translate: 'administrative.quan_ly_danh_muc',
                type: 'group',
                icon: 'apps',
                icon_img: "/assets/icons/erp.png",
                children: [],
            };
            quan_ly_danh_muc.forEach((item) => {
              menu_quan_ly_danh_muc.children.push({
                id: item.menu.id,
                badge: item.menu.badge_approval,
                module: 'system',
                title: '',
                link: item.menu.url,
                translate: item.menu.translate,
                icon: item.menu.icon,
                type: 'basic',
              });
            });

            if (menu_quan_ly_danh_muc.children.length > 0) {
              this._defaultNavigation.push(menu_quan_ly_danh_muc);
            }

            Swal.close();
            // if (user.type == 4) {
            //   var menu_hoc_sinh = this.menu.filter((d) =>
            //     this.checkInclueFn(d.menu.id, [
            //       'hs_tin_tuc',
            //       'hs_lop_thoi_khoa_bieu',
            //       'hs_lop_bai_hoc',
            //       'hs_lop_bai_kiem_tra',
            //       'hs_lop_phieu_diem',
            //       'hs_diem_danh_hoc_sinh',
            //     ]),
            //   );

            //   menu_hoc_sinh.forEach((item) => {
            //     this._defaultNavigation.push({
            //       id: item.menu.id,
            //       badge: item.menu.badge_approval,
            //       module: 'system',
            //       title: '',
            //       link: item.menu.url,
            //       translate: item.menu.translate,
            //       icon: item.menu.icon,
            //       type: 'basic',
            //     });
            //   });
            // }
            // //  2 teacher

            // // 3 parent
            // else if (user.type == 3) {
            //   //danh mục phụ huynh
            //   var item_menu_parent: FuseNavigationItem = {
            //     id: 'parent',
            //     module: 'administrative',
            //     title: 'administrative.parent',
            //     translate: 'parent',
            //     type: 'aside',
            //     icon: 'mat_outline:circle_notifications',
            //     icon_img: '/assets/icons/hanh_chanh_nhan_su.png',
            //     children: [],
            //   };
            //   //lv2
            //   var menu_parent: FuseNavigationItem = {
            //     id: 'parent',
            //     module: 'administrative',
            //     title: 'administrative.parent',
            //     translate: 'administrative.parent',
            //     type: 'collapsable',
            //     icon: 'mat_outline:event_note',
            //     //icon_img: "/assets/images/shungo/blue_feed_icon.png",
            //     children: [],
            //   };

            //   var menu_pr = false;
            //   item_menu_parent.children.push(menu_parent);
            //   var child_menu_parent = this.menu.filter((d) => this.checkInclueFn(d.menu.id, []));
            //   if (child_menu_parent.length != 0) {
            //     for (let i = 0; i < child_menu_parent.length; i++) {
            //       menu_pr = true;
            //       menu_id.push(child_menu_parent[i].menu.id);
            //       menu_parent.children.push({
            //         id: child_menu_parent[i].menu.id,
            //         badge: child_menu_parent[i].menu.badge_approval,
            //         module: 'administrative',
            //         title: child_menu_parent[i].menu.title,
            //         link: child_menu_parent[i].menu.url,
            //         translate: child_menu_parent[i].menu.translate,
            //         icon: child_menu_parent[i].menu.icon,
            //         //icon_img: menu_administrative_personnel[i].menu.icon_image,
            //         type: 'basic',
            //       });
            //     }
            //   }
            //   if (menu_pr == true) {
            //     this._defaultNavigation.push(item_menu_parent);
            //   }
            // }

            // //admin 1  or teacher 2
            // else {
            //   //02 Quản lý thông tin Trường học
            //   //03 Quản lý Cán bộ - Giáo viên
            //   //04 Quản lý Học sinh
            //   //05 Quản lý tuyển sinh đầu cấp

            //   //01 Website Trường học
            //   if (ma == 'website_truong_hoc') {
            //     var website_truong_hoc = this.menu.filter((d) =>
            //       this.checkInclueFn(d.menu.id, [
            //         'sys_tong_quan_website',
            //         'sys_tin_tuc_website',
            //         'sys_nhom_tin_website',
            //         'sys_nhom_thu_vien_hinh_anh',
            //         'sys_thu_vien_hinh_anh',
            //         'sys_video',
            //         'sys_banner',
            //         'sys_contactus',
            //         // 'sys_loai_cau_hinh',
            //         'sys_cau_hinh_thong_tin',
            //         // "sys_huong_dan_su_dung",
            //         // "sys_huong_dan_su_dung_detail",
            //       ]),
            //     );
            //     website_truong_hoc.forEach((item) => {
            //       this._defaultNavigation.push({
            //         id: item.menu.id,
            //         badge: item.menu.badge_approval,
            //         module: 'system',
            //         title: '',
            //         link: item.menu.url,
            //         translate: item.menu.translate,
            //         icon: item.menu.icon,
            //         type: 'basic',
            //       });
            //     });
            //     //this._defaultNavigation.push(menu_website_truong_hoc);
            //   } else if (ma == 'xep_lop_phan_cong_giang_day') {
            //     var menu_xep_lop_phan_cong_giang_day = this.menu.filter((d) =>
            //       this.checkInclueFn(d.menu.id, [
            //         'sys_giao_vien',

            //         'tkb_phan_cong_giang_day',
            //         'qlgv_phan_bo_theo_to_chuyen_mon',

            //         'sys_hoc_sinh',

            //         'sys_phu_huynh',
            //         'sys_lop_hoc_sinh',
            //         'pcgd_dinh_muc_tiet_hoc',
            //       ]),
            //     );

            //     var menu_can_bo_giao_vien: FuseNavigationItem = {
            //       id: 'menu_can_bo_giao_vien',
            //       module: 'system',
            //       title: '',
            //       translate: 'system.menu_can_bo_giao_vien',
            //       type: 'group',
            //       icon: 'apps',
            //       children: [],
            //     };
            //     var menu_cap_nhat_hoc_sinh: FuseNavigationItem = {
            //       id: 'menu_cap_nhat_hoc_sinh',
            //       module: 'system',
            //       title: '',
            //       translate: 'system.menu_cap_nhat_hoc_sinh',
            //       type: 'group',
            //       icon: 'apps',
            //       children: [],
            //     };
            //     menu_xep_lop_phan_cong_giang_day.forEach((item) => {
            //       if (
            //         item.menu.id == 'sys_giao_vien' ||
            //         item.menu.id == 'pcgd_dinh_muc_tiet_hoc' ||
            //         item.menu.id == 'tkb_phan_cong_giang_day' ||
            //         item.menu.id == 'qlgv_phan_bo_theo_to_chuyen_mon'
            //       ) {
            //         menu_can_bo_giao_vien.children.push({
            //           id: item.menu.id,
            //           badge: item.menu.badge_approval,
            //           module: 'system',
            //           title: '',
            //           link: item.menu.url,
            //           translate: item.menu.translate,
            //           icon: item.menu.icon,
            //           type: 'basic',
            //         });
            //       } else if (
            //         item.menu.id == 'sys_hoc_sinh' ||
            //         item.menu.id == 'sys_phu_huynh' ||
            //         item.menu.id == 'sys_lop_hoc_sinh'
            //       ) {
            //         menu_cap_nhat_hoc_sinh.children.push({
            //           id: item.menu.id,
            //           badge: item.menu.badge_approval,
            //           module: 'system',
            //           title: '',
            //           link: item.menu.url,
            //           translate: item.menu.translate,
            //           icon: item.menu.icon,
            //           type: 'basic',
            //         });
            //       } else {
            //         menu_quan_ly_danh_muc.children.push({
            //           id: item.menu.id,
            //           badge: item.menu.badge_approval,
            //           module: 'system',
            //           title: '',
            //           link: item.menu.url,
            //           translate: item.menu.translate,
            //           icon: item.menu.icon,
            //           type: 'basic',
            //         });
            //       }
            //     });
            //     // this._defaultNavigation.push(menu_quan_ly_danh_muc);
            //     if (menu_cap_nhat_hoc_sinh.children.length > 0) {
            //       this._defaultNavigation.push(menu_cap_nhat_hoc_sinh);
            //     }

            //     if (menu_can_bo_giao_vien.children.length > 0) {
            //       this._defaultNavigation.push(menu_can_bo_giao_vien);
            //     }
            //   }
            //   //02 Quản lý thông tin Trường học
            //   else if (ma == 'thong_tin_truong_hoc') {
            //     var menu_thong_tin_truong_hoc = this.menu.filter((d) =>
            //       this.checkInclueFn(d.menu.id, ['sys_truong_hoc']),
            //     );
            //     var menu_quan_ly_danh_muc: FuseNavigationItem = {
            //       id: 'menu_quan_ly_danh_muc',
            //       module: 'system',
            //       title: '',
            //       translate: 'system.menu_quan_ly_danh_muc',
            //       type: 'group',
            //       icon: 'apps',
            //       children: [],
            //     };
            //     var menu_can_bo_giao_vien: FuseNavigationItem = {
            //       id: 'menu_can_bo_giao_vien',
            //       module: 'system',
            //       title: '',
            //       translate: 'system.menu_can_bo_giao_vien',
            //       type: 'group',
            //       icon: 'apps',
            //       children: [],
            //     };
            //     var menu_cap_nhat_hoc_sinh: FuseNavigationItem = {
            //       id: 'menu_cap_nhat_hoc_sinh',
            //       module: 'system',
            //       title: '',
            //       translate: 'system.menu_cap_nhat_hoc_sinh',
            //       type: 'group',
            //       icon: 'apps',
            //       children: [],
            //     };
            //     menu_thong_tin_truong_hoc.forEach((item) => {
            //       if (item.menu.id == 'sys_tong_quan_thong_tin_truong_hoc') {
            //         this._defaultNavigation.push({
            //           id: item.menu.id,
            //           badge: item.menu.badge_approval,
            //           module: 'system',
            //           title: '',
            //           link: item.menu.url,
            //           translate: item.menu.translate,
            //           icon: item.menu.icon,
            //           type: 'basic',
            //         });
            //       } else if (item.menu.id == 'sys_truong_hoc') {
            //         this._defaultNavigation.push({
            //           id: item.menu.id,
            //           badge: item.menu.badge_approval,
            //           module: 'system',
            //           title: '',
            //           link: item.menu.url,
            //           translate: item.menu.translate,
            //           icon: item.menu.icon,
            //           type: 'basic',
            //         });
            //       } else if (
            //         item.menu.id == 'sys_giao_vien' ||
            //         item.menu.id == 'tkb_phan_cong_giang_day' ||
            //         item.menu.id == 'qlgv_phan_bo_theo_to_chuyen_mon'
            //       ) {
            //         menu_can_bo_giao_vien.children.push({
            //           id: item.menu.id,
            //           badge: item.menu.badge_approval,
            //           module: 'system',
            //           title: '',
            //           link: item.menu.url,
            //           translate: item.menu.translate,
            //           icon: item.menu.icon,
            //           type: 'basic',
            //         });
            //       } else if (
            //         item.menu.id == 'sys_hoc_sinh' ||
            //         item.menu.id == 'sys_so_dang_bo' ||
            //         item.menu.id == 'sys_phu_huynh' ||
            //         item.menu.id == 'sys_lop_hoc_sinh'
            //       ) {
            //         menu_cap_nhat_hoc_sinh.children.push({
            //           id: item.menu.id,
            //           badge: item.menu.badge_approval,
            //           module: 'system',
            //           title: '',
            //           link: item.menu.url,
            //           translate: item.menu.translate,
            //           icon: item.menu.icon,
            //           type: 'basic',
            //         });
            //       }
            //     });

            //     // this._defaultNavigation.push(menu_can_bo_giao_vien);
            //     // this._defaultNavigation.push(menu_cap_nhat_hoc_sinh);
            //   } else if (ma == 'danh_muc_chung') {
            //     var menu_danh_muc_chung = this.menu.filter((d) =>
            //       this.checkInclueFn(d.menu.id, [
            //         'sys_nam_hoc_ky',
            //         'sys_tiet_hoc',
            //         'sys_khoi_lop',
            //         'sys_lop',
            //         'sys_danh_muc_phong_hoc',
            //         'sys_phan_bo_phong',
            //         'sys_phong_ban',
            //         'sys_to_chuyen_mon',
            //         'sys_nhom_chuc_vu',
            //         'sys_chuc_danh',
            //         'sys_chuyen_nganh_dao_tao',
            //         'sys_trinh_do_chuyen_mon',
            //         'sys_hinh_thuc_hop_dong',
            //         'sys_bac_luong',
            //         'sys_danh_muc_mon_hoc',
            //         'sys_mon_hoc',
            //         'sys_cau_hinh_ban_can_bo_lop',
            //         'sys_cau_hinh_danh_gia_kqht',
            //         'sys_pham_chat_nang_luc', ,
            //       ]),
            //     );
            //     var menu_quan_ly_danh_muc: FuseNavigationItem = {
            //       id: 'menu_quan_ly_danh_muc',
            //       module: 'system',
            //       title: '',
            //       translate: 'system.menu_quan_ly_danh_muc',
            //       type: 'group',
            //       icon: 'apps',
            //       children: [],
            //     };

            //     menu_danh_muc_chung.forEach((item) => {
            //       menu_quan_ly_danh_muc.children.push({
            //         id: item.menu.id,
            //         badge: item.menu.badge_approval,
            //         module: 'system',
            //         title: '',
            //         link: item.menu.url,
            //         translate: item.menu.translate,
            //         icon: item.menu.icon,
            //         type: 'basic',
            //       });
            //     });

            //     if (menu_quan_ly_danh_muc.children.length > 0) {
            //       this._defaultNavigation.push(menu_quan_ly_danh_muc);
            //     }

            //     // this._defaultNavigation.push(menu_can_bo_giao_vien);
            //     // this._defaultNavigation.push(menu_cap_nhat_hoc_sinh);
            //   } else if (ma == 'co_so_du_lieu_hoc_sinh') {
            //     var menu_xep_lop_phan_cong_giang_day = this.menu.filter((d) =>
            //       this.checkInclueFn(d.menu.id, [
            //         'sys_van_bang_chung_chi',
            //         'sys_hoc_sinh',
            //         'sys_phu_huynh',
            //         'sys_lop_hoc_sinh',
            //       ]),
            //     );
            //     var menu_cap_nhat_hoc_sinh: FuseNavigationItem = {
            //       id: 'menu_cap_nhat_hoc_sinh',
            //       module: 'system',
            //       title: '',
            //       translate: 'system.menu_cap_nhat_hoc_sinh',
            //       type: 'group',
            //       icon: 'apps',
            //       children: [],
            //     };
            //     menu_xep_lop_phan_cong_giang_day.forEach((item) => {
            //       if (
            //         item.menu.id == 'sys_hoc_sinh' ||
            //         item.menu.id == 'sys_phu_huynh' ||
            //         item.menu.id == 'sys_lop_hoc_sinh' ||
            //         item.menu.id == 'sys_van_bang_chung_chi'
            //       ) {
            //         menu_cap_nhat_hoc_sinh.children.push({
            //           id: item.menu.id,
            //           badge: item.menu.badge_approval,
            //           module: 'system',
            //           title: '',
            //           link: item.menu.url,
            //           translate: item.menu.translate,
            //           icon: item.menu.icon,
            //           type: 'basic',
            //         });
            //       } else {
            //         menu_quan_ly_danh_muc.children.push({
            //           id: item.menu.id,
            //           badge: item.menu.badge_approval,
            //           module: 'system',
            //           title: '',
            //           link: item.menu.url,
            //           translate: item.menu.translate,
            //           icon: item.menu.icon,
            //           type: 'basic',
            //         });
            //       }
            //     });
            //     // this._defaultNavigation.push(menu_quan_ly_danh_muc);
            //     if (menu_cap_nhat_hoc_sinh.children.length > 0) {
            //       this._defaultNavigation.push(menu_cap_nhat_hoc_sinh);
            //     }
            //   } else if (ma == 'co_so_du_lieu_giao_vien') {
            //     var menu_xep_lop_phan_cong_giang_day = this.menu.filter((d) =>
            //       this.checkInclueFn(d.menu.id, [
            //         'sys_giao_vien',
            //         'tkb_phan_cong_giang_day',
            //         'qlgv_phan_bo_theo_to_chuyen_mon',
            //         'pcgd_dinh_muc_tiet_hoc',
            //       ]),
            //     );

            //     var menu_can_bo_giao_vien: FuseNavigationItem = {
            //       id: 'menu_can_bo_giao_vien',
            //       module: 'system',
            //       title: '',
            //       translate: 'system.menu_can_bo_giao_vien',
            //       type: 'group',
            //       icon: 'apps',
            //       children: [],
            //     };
            //     menu_xep_lop_phan_cong_giang_day.forEach((item) => {
            //       if (
            //         item.menu.id == 'sys_giao_vien' ||
            //         item.menu.id == 'pcgd_dinh_muc_tiet_hoc' ||
            //         item.menu.id == 'tkb_phan_cong_giang_day' ||
            //         item.menu.id == 'qlgv_phan_bo_theo_to_chuyen_mon'
            //       ) {
            //         menu_can_bo_giao_vien.children.push({
            //           id: item.menu.id,
            //           badge: item.menu.badge_approval,
            //           module: 'system',
            //           title: '',
            //           link: item.menu.url,
            //           translate: item.menu.translate,
            //           icon: item.menu.icon,
            //           type: 'basic',
            //         });
            //       } else {
            //         menu_quan_ly_danh_muc.children.push({
            //           id: item.menu.id,
            //           badge: item.menu.badge_approval,
            //           module: 'system',
            //           title: '',
            //           link: item.menu.url,
            //           translate: item.menu.translate,
            //           icon: item.menu.icon,
            //           type: 'basic',
            //         });
            //       }
            //     });
            //     if (menu_can_bo_giao_vien.children.length > 0) {
            //       this._defaultNavigation.push(menu_can_bo_giao_vien);
            //     }
            //   } else if (ma == 'bao_cao_can_bo_giao_vien') {
            //     var quan_li_can_bo_giao_vien = this.menu.filter((d) =>
            //       this.checkInclueFn(d.menu.id, [
            //         'qlgv_bao_cao_tong_quan',

            //         // "sys_thu_vien_tai_lieu",
            //         // "tkb_phan_cong_giang_day",
            //         // "qlgv_lich_bao_giang",
            //         // "qlgv_nhat_ky_giang_day",
            //         // "qlgv_thi_dua_ca_nhan",
            //         // "qlgv_thi_dua_tap_the",
            //         // "qlgv_tong_quan",
            //         // "qlgv_bao_cao_can_bo_theo_hop_dong",
            //         // "qlgv_bao_cao_can_bo_theo_bien_che",
            //         // "qlgv_bao_cao_can_bo_theo_chuyen_mon",
            //         // "qlgv_bao_cao_khen_thuong",
            //         // "qlgv_bao_cao_ky_luat",
            //       ]),
            //     );

            //     quan_li_can_bo_giao_vien.forEach((item) => {
            //       this._defaultNavigation.push({
            //         id: item.menu.id,
            //         badge: item.menu.badge_approval,
            //         module: 'system',
            //         title: '',
            //         link: item.menu.url,
            //         translate: item.menu.translate,
            //         icon: item.menu.icon,
            //         type: 'basic',
            //       });
            //     });

            //     var menu_khai_bao_danh_muc: FuseNavigationItem = {
            //       id: 'menu_khai_bao_danh_muc',
            //       module: 'system',
            //       title: '',
            //       translate: 'system.khai_bao_danh_muc',
            //       type: 'group',
            //       icon: 'apps',
            //       children: [],
            //     };
            //     var menu_quan_ly_danh_hieu_thi_dua: FuseNavigationItem = {
            //       id: 'menu_quan_ly_danh_hieu_thi_dua',
            //       module: 'system',
            //       title: '',
            //       translate: 'system.quan_ly_danh_hieu_thi_dua',
            //       type: 'group',
            //       icon: 'apps',
            //       children: [],
            //     };
            //     var menu_bao_cao: FuseNavigationItem = {
            //       id: 'menu_bao_cao',
            //       module: 'system',
            //       title: '',
            //       translate: 'system.bao_cao',
            //       type: 'group',
            //       icon: 'apps',
            //       children: [],
            //     };

            //     // quan_li_can_bo_giao_vien.forEach(item => {
            //     //     if ( item.menu.id == "sys_phong_ban" || item.menu.id == "sys_nhom_chuc_vu"
            //     //         || item.menu.id == "sys_chuc_danh" || item.menu.id == "sys_chuyen_nganh_dao_tao"
            //     //         || item.menu.id == "sys_trinh_do_chuyen_mon" || item.menu.id == "sys_hinh_thuc_hop_dong"
            //     //         || item.menu.id == "sys_bac_luong") {
            //     //         menu_khai_bao_danh_muc.children.push(
            //     //             {
            //     //                 id: item.menu.id,
            //     //                 badge: item.menu.badge_approval,
            //     //                 module: 'system',
            //     //                 title: '',
            //     //                 link: item.menu.url,
            //     //                 translate: item.menu.translate,
            //     //                 icon: item.menu.icon,
            //     //                 type: 'basic',

            //     //             });
            //     //     }
            //     // })
            //     // this._defaultNavigation.push(menu_khai_bao_danh_muc);

            //     quan_li_can_bo_giao_vien.forEach((item) => {
            //       if (
            //         item.menu.id == 'sys_giao_vien' ||
            //         item.menu.id == 'tkb_phan_cong_giang_day' ||
            //         item.menu.id == 'sys_thu_vien_tai_lieu'
            //       ) {
            //         this._defaultNavigation.push({
            //           id: item.menu.id,
            //           badge: item.menu.badge_approval,
            //           module: 'system',
            //           title: '',
            //           link: item.menu.url,
            //           translate: item.menu.translate,
            //           icon: item.menu.icon,
            //           type: 'basic',
            //         });
            //       }
            //     });
            //     var menu_nhat_ky_giang_day: FuseNavigationItem = {
            //       id: 'menu_nhat_ky_giang_day',
            //       module: 'system',
            //       title: '',
            //       translate: 'system.nhat_ky_giang_day',
            //       type: 'group',
            //       icon: 'apps',
            //       children: [],
            //     };
            //     quan_li_can_bo_giao_vien.forEach((item) => {
            //       if (item.menu.id == 'qlgv_lich_bao_giang' || item.menu.id == 'qlgv_nhat_ky_giang_day') {
            //         menu_nhat_ky_giang_day.children.push({
            //           id: item.menu.id,
            //           badge: item.menu.badge_approval,
            //           module: 'system',
            //           title: '',
            //           link: item.menu.url,
            //           translate: item.menu.translate,
            //           icon: item.menu.icon,
            //           type: 'basic',
            //         });
            //       }

            //       if (item.menu.id == 'qlgv_thi_dua_ca_nhan' || item.menu.id == 'qlgv_thi_dua_tap_the') {
            //         menu_quan_ly_danh_hieu_thi_dua.children.push({
            //           id: item.menu.id,
            //           badge: item.menu.badge_approval,
            //           module: 'system',
            //           title: '',
            //           link: item.menu.url,
            //           translate: item.menu.translate,
            //           icon: item.menu.icon,
            //           type: 'basic',
            //         });
            //       }
            //       if (
            //         item.menu.id == 'qlgv_tong_quan' ||
            //         item.menu.id == 'qlgv_bao_cao_can_bo_theo_hop_dong' ||
            //         item.menu.id == 'qlgv_bao_cao_can_bo_theo_bien_che' ||
            //         item.menu.id == 'qlgv_bao_cao_can_bo_theo_chuyen_mon' ||
            //         item.menu.id == 'qlgv_bao_cao_ky_luat' ||
            //         item.menu.id == 'qlgv_bao_cao_khen_thuong'
            //       ) {
            //         menu_bao_cao.children.push({
            //           id: item.menu.id,
            //           badge: item.menu.badge_approval,
            //           module: 'system',
            //           title: '',
            //           link: item.menu.url,
            //           translate: item.menu.translate,
            //           icon: item.menu.icon,
            //           type: 'basic',
            //         });
            //       }
            //     });
            //     //   this._defaultNavigation.push(menu_nhat_ky_giang_day);
            //     //  this._defaultNavigation.push(menu_quan_ly_danh_hieu_thi_dua);
            //     //  this._defaultNavigation.push(menu_bao_cao);
            //   }

            //   //"qlhs_bao_cao_hoc_sinh_nhan_giay_khen",
            //   //    "qlhs_bao_cao_hoc_sinh_thi_lai", "qlhs_bao_cao_ren_luyen_hanh_kiem_trong_he",
            //   //    "qlhs_bao_cao_hoc_sinh_luu_ban"
            //   else if (ma == 'bao_cao_hoc_sinh') {
            //     var quan_li_hoc_sinh = this.menu.filter((d) =>
            //       this.checkInclueFn(d.menu.id, ['qlhs_bao_cao_tong_quan']),
            //     );
            //     quan_li_hoc_sinh.forEach((item) => {
            //       this._defaultNavigation.push({
            //         id: item.menu.id,
            //         badge: item.menu.badge_approval,
            //         module: 'system',
            //         title: '',
            //         link: item.menu.url,
            //         translate: item.menu.translate,
            //         icon: item.menu.icon,
            //         type: 'basic',
            //       });
            //     });

            //     var menu_bao_cao: FuseNavigationItem = {
            //       id: 'menu_bao_cao',
            //       module: 'system',
            //       title: '',
            //       translate: 'system.bao_cao',
            //       type: 'group',
            //       icon: 'apps',
            //       children: [],
            //     };

            //     quan_li_hoc_sinh.forEach((item) => {
            //       if (
            //         item.menu.id == 'qlhs_tong_quan' ||
            //         item.menu.id == 'qlhs_bao_cao_hoc_sinh_nhan_giay_khen' ||
            //         item.menu.id == 'qlhs_bao_cao_hoc_sinh_thi_lai' ||
            //         item.menu.id == 'qlhs_bao_cao_ren_luyen_hanh_kiem_trong_he' ||
            //         item.menu.id == 'qlhs_bao_cao_hoc_sinh_luu_ban'
            //       ) {
            //         menu_bao_cao.children.push({
            //           id: item.menu.id,
            //           badge: item.menu.badge_approval,
            //           module: 'system',
            //           title: '',
            //           link: item.menu.url,
            //           translate: item.menu.translate,
            //           icon: item.menu.icon,
            //           type: 'basic',
            //         });
            //       }
            //     });
            //     // this._defaultNavigation.push(menu_bao_cao);
            //   } else if (ma == 'hoc_ba_so') {
            //     var menu_hoc_ba_so = this.menu.filter((d) =>
            //       this.checkInclueFn(d.menu.id, [
            //         // 'hbs_tong_quan_hoc_ba_so',
            //         'hbs_bao_cao_thong_ke_hoc_ba',
            //         'hbs_ky_so_dong_dau_hoc_ba',
            //         'hbs_thong_tin_nha_truong',
            //         'sys_khoi_tao_du_lieu',
            //         'hbs_cau_hinh_hoc_ba',
            //         'hbs_quan_ly_can_bo',
            //         'hbs_ket_qua_hoc_tap',
            //         'hbs_lich_su_dong_bo',
            //         'hbs_quan_ly_lop_hoc',
            //         'sys_quan_ly_hoc_ba',
            //         'hbs_khoi_tao_hoc_ba',
            //         'hbs_duyet_khoi_tao_hoc_ba',
            //         'hbs_ban_hanh_lan_dau_hoc_ba',
            //         'hbs_ky_so_hoc_ba',
            //         'hbs_giao_vien_ky_so_hoc_ba',
            //         'hbs_giao_vien_bo_mon_ky_so_hoc_ba',
            //         'hbs_dong_dau_hoc_ba',
            //         'hbs_nop_hoc_ba',
            //         'hbs_tra_cuu_hoc_ba',
            //       ]),
            //     );

            //     menu_hoc_ba_so.forEach((item) => {
            //       if (item.menu.id == 'hbs_bao_cao_thong_ke_hoc_ba') {
            //         this._defaultNavigation.push({
            //           id: item.menu.id,
            //           badge: item.menu.badge_approval,
            //           module: 'system',
            //           title: '',
            //           link: item.menu.url,
            //           translate: item.menu.translate,
            //           icon: item.menu.icon,
            //           type: 'basic',
            //         });
            //       }
            //     });

            //     var menu_dong_bo_du_lieu: FuseNavigationItem = {
            //       id: 'menu_dong_bo_du_lieu',
            //       module: 'system',
            //       title: '',
            //       translate: 'system.dong_bo_du_lieu',
            //       type: 'group',
            //       icon: 'apps',
            //       children: [],
            //     };
            //     var menu_khoi_tao_hoc_ba: FuseNavigationItem = {
            //       id: 'menu_khoi_tao_hoc_ba',
            //       module: 'system',
            //       title: '',
            //       translate: 'system.khoi_tao_hoc_ba',
            //       type: 'group',
            //       icon: 'apps',
            //       children: [],
            //     };
            //     menu_hoc_ba_so.forEach((item) => {
            //       if (item.menu.id == 'hbs_tong_quan_hoc_ba_so') {
            //         this._defaultNavigation.push({
            //           id: item.menu.id,
            //           badge: item.menu.badge_approval,
            //           module: 'system',
            //           title: '',
            //           link: item.menu.url,
            //           translate: item.menu.translate,
            //           icon: item.menu.icon,
            //           type: 'basic',
            //         });
            //       } else if (
            //         item.menu.id == 'hbs_thong_tin_nha_truong' ||
            //         item.menu.id == 'hbs_quan_ly_lop_hoc' ||
            //         item.menu.id == 'hbs_quan_ly_can_bo' ||
            //         item.menu.id == 'hbs_ket_qua_hoc_tap' ||
            //         item.menu.id == 'hbs_lich_su_dong_bo'
            //       ) {
            //         menu_dong_bo_du_lieu.children.push({
            //           id: item.menu.id,
            //           badge: item.menu.badge_approval,
            //           module: 'system',
            //           title: '',
            //           link: item.menu.url,
            //           translate: item.menu.translate,
            //           icon: item.menu.icon,
            //           type: 'basic',
            //         });
            //       }
            //     });
            //     if (menu_dong_bo_du_lieu.children.length > 0) {
            //       this._defaultNavigation.push(menu_dong_bo_du_lieu);
            //     }

            //     menu_hoc_ba_so.forEach((item) => {
            //       if (item.menu.id == 'hbs_cau_hinh_hoc_ba') {
            //         this._defaultNavigation.push({
            //           id: item.menu.id,
            //           badge: item.menu.badge_approval,
            //           module: 'system',
            //           title: '',
            //           link: item.menu.url,
            //           translate: item.menu.translate,
            //           icon: item.menu.icon,
            //           type: 'basic',
            //         });
            //       } else if (
            //         item.menu.id == 'hbs_khoi_tao_hoc_ba' ||
            //         item.menu.id == 'hbs_duyet_khoi_tao_hoc_ba' ||
            //         item.menu.id == 'hbs_ban_hanh_lan_dau_hoc_ba'
            //       ) {
            //         menu_khoi_tao_hoc_ba.children.push({
            //           id: item.menu.id,
            //           badge: item.menu.badge_approval,
            //           module: 'system',
            //           title: '',
            //           link: item.menu.url,
            //           translate: item.menu.translate,
            //           icon: item.menu.icon,
            //           type: 'basic',
            //         });
            //       }
            //     });

            //     if (menu_khoi_tao_hoc_ba.children.length > 0) {
            //       this._defaultNavigation.push(menu_khoi_tao_hoc_ba);
            //     }

            //     var menu_quan_ly_ky_so_hoc_ba: FuseNavigationItem = {
            //       id: 'menu_quan_ly_ky_so_hoc_ba',
            //       module: 'system',
            //       title: '',
            //       translate: 'system.quan_ly_ky_so_dong_dau_hoc_ba',
            //       type: 'group',
            //       icon: 'apps',
            //       children: [],
            //     };

            //     menu_hoc_ba_so.forEach((item) => {
            //       if (
            //         item.menu.id == 'hbs_ky_so_hoc_ba' ||
            //         item.menu.id == 'hbs_giao_vien_ky_so_hoc_ba' ||
            //         item.menu.id == 'hbs_giao_vien_bo_mon_ky_so_hoc_ba' ||
            //         item.menu.id == 'hbs_ky_so_va_dong_dau_hoc_ba' ||
            //         item.menu.id == 'hbs_dong_dau_hoc_ba'
            //       ) {
            //         menu_quan_ly_ky_so_hoc_ba.children.push({
            //           id: item.menu.id,
            //           badge: item.menu.badge_approval,
            //           module: 'system',
            //           title: '',
            //           link: item.menu.url,
            //           translate: item.menu.translate,
            //           icon: item.menu.icon,
            //           type: 'basic',
            //         });
            //       }
            //     });

            //     if (menu_quan_ly_ky_so_hoc_ba.children.length > 0) {
            //       this._defaultNavigation.push(menu_quan_ly_ky_so_hoc_ba);
            //     }

            //     menu_hoc_ba_so.forEach((item) => {
            //       if (item.menu.id == 'hbs_nop_hoc_ba') {
            //         this._defaultNavigation.push({
            //           id: item.menu.id,
            //           badge: item.menu.badge_approval,
            //           module: 'system',
            //           title: '',
            //           link: item.menu.url,
            //           translate: item.menu.translate,
            //           icon: item.menu.icon,
            //           type: 'basic',
            //         });
            //       } else if (item.menu.id == 'hbs_tra_cuu_hoc_ba') {
            //         this._defaultNavigation.push({
            //           id: item.menu.id,
            //           badge: item.menu.badge_approval,
            //           module: 'system',
            //           title: '',
            //           link: item.menu.url,
            //           translate: item.menu.translate,
            //           icon: item.menu.icon,
            //           type: 'basic',
            //         });
            //       }
            //     });
            //   } else if (ma == 'quan_li_thiet_bi') {
            //     var menu_quan_li_thiet_bi = this.menu.filter((d) =>
            //       this.checkInclueFn(d.menu.id, [
            //         'qltb_tong_quan',
            //         'qltb_bao_cao_tong_hop_tai_san',
            //         'qltb_kho',
            //         'qltb_loai_thiet_bi',
            //         'qltb_thiet_bi',
            //         'qltb_bao_cao_hien_trang_su_dung',
            //         'qltb_bao_cao_kiem_ke',
            //       ]),
            //     );

            //     menu_quan_li_thiet_bi.forEach((item) => {
            //       this._defaultNavigation.push({
            //         id: item.menu.id,
            //         badge: item.menu.badge_approval,
            //         module: 'system',
            //         title: '',
            //         link: item.menu.url,
            //         translate: item.menu.translate,
            //         icon: item.menu.icon,
            //         type: 'basic',
            //       });

            //       // else if (item.menu.id == "sdb_khoi_tao" || item.menu.id == "sdb_duyet_khoi_tao" || item.menu.id == "hbs_ban_hanh_lan_dau_hoc_ba") {
            //       //     menu_khoi_tao_so_dang_bo.children.push(
            //       //     {
            //       //         id: item.menu.id,
            //       //         badge: item.menu.badge_approval,
            //       //         module: 'system',
            //       //         title: '',
            //       //         link: item.menu.url,
            //       //         translate: item.menu.translate,
            //       //         icon: item.menu.icon,
            //       //         type: 'basic',

            //       //     });
            //       // }
            //     });

            //     this._defaultNavigation.push(menu_quan_li_thiet_bi);
            //   } else if (ma == 'thiet_lap_lms') {
            //     var menu_thiet_lap_lms = this.menu.filter((d) =>
            //       this.checkInclueFn(d.menu.id, [
            //         'khl_do_kho',
            //         'khl_nguon_bai_tap',
            //         'khl_dvkt',
            //         'khl_ma_dinh_danh_hoc_lieu',
            //       ]),
            //     );

            //     menu_thiet_lap_lms.forEach((item) => {
            //       this._defaultNavigation.push({
            //         id: item.menu.id,
            //         badge: item.menu.badge_approval,
            //         module: 'system',
            //         title: '',
            //         link: item.menu.url,
            //         translate: item.menu.translate,
            //         icon: item.menu.icon,
            //         type: 'basic',
            //       });
            //     });
            //     this._defaultNavigation.push(menu_thiet_lap_lms);
            //   } else if (ma == 'kho_hoc_lieu') {
            //     var menu = this.menu.filter((d) => this.checkInclueFn(d.menu.id, ['khl_sach_giao_khoa']));

            //     menu.forEach((item) => {
            //       this._defaultNavigation.push({
            //         id: item.menu.id,
            //         badge: item.menu.badge_approval,
            //         module: 'system',
            //         title: '',
            //         link: item.menu.url,
            //         translate: item.menu.translate,
            //         icon: item.menu.icon,
            //         type: 'basic',
            //       });
            //     });
            //     this._defaultNavigation.push(menu);
            //   } else if (ma == 'chuong_trinh_hoc') {
            //     var menu = this.menu.filter((d) => this.checkInclueFn(d.menu.id, ['khl_xay_dung_khoa_hoc']));

            //     menu.forEach((item) => {
            //       this._defaultNavigation.push({
            //         id: item.menu.id,
            //         badge: item.menu.badge_approval,
            //         module: 'system',
            //         title: '',
            //         link: item.menu.url,
            //         translate: item.menu.translate,
            //         icon: item.menu.icon,
            //         type: 'basic',
            //       });
            //     });
            //     this._defaultNavigation.push(menu);
            //   } else if (ma == 'duyet_hoc_lieu') {
            //     var menu = this.menu.filter((d) => this.checkInclueFn(d.menu.id, ['khl_duyet_kho_hoc_lieu']));

            //     menu.forEach((item) => {
            //       this._defaultNavigation.push({
            //         id: item.menu.id,
            //         badge: item.menu.badge_approval,
            //         module: 'system',
            //         title: '',
            //         link: item.menu.url,
            //         translate: item.menu.translate,
            //         icon: item.menu.icon,
            //         type: 'basic',
            //       });
            //     });
            //     this._defaultNavigation.push(menu);
            //   } else if (ma == 'quan_li_so_suc_khoe') {
            //     var menu_quan_li_so_suc_khoe = this.menu.filter((d) =>
            //       this.checkInclueFn(d.menu.id, [
            //         'qlssk_tong_quan',
            //         'qlssk_so_suc_khoe',
            //         'qlssk_so_suc_khoe_hang_ngay',
            //         'qlssk_so_quan_ly_benh_truyen_nhiem',
            //         'qlssk_can_do_suc_khoe',
            //         'qlssk_bao_hiem_y_te',
            //         'qlssk_tra_cuu',
            //         //'qlssk_so_suc_khoe_view',
            //       ]),
            //     );
            //     menu_quan_li_so_suc_khoe.forEach((item) => {
            //       this._defaultNavigation.push({
            //         id: item.menu.id,
            //         badge: item.menu.badge_approval,
            //         module: 'system',
            //         title: '',
            //         link: item.menu.url,
            //         translate: item.menu.translate,
            //         icon: item.menu.icon,
            //         type: 'basic',
            //       });
            //     });
            //     this._defaultNavigation.push(menu_quan_li_so_suc_khoe);
            //   } else if (ma == 'quan_li_khoan_thu') {
            //     var menu_quan_li_khoan_thu = this.menu.filter((d) =>
            //       this.checkInclueFn(d.menu.id, [
            //         'qlkt_tong_quan',
            //         'qlkt_loai_khoan_thu',
            //         'qlkt_khoan_thu',
            //         'qlkt_dien_chinh_sach',
            //         'qlkt_ho_so_mien_giam',
            //         'qlkt_duyet_ho_so_mien_giam',
            //         'qlkt_khoan_thu_hoc_sinh_tu_nguyen',
            //       ]),
            //     );

            //     menu_quan_li_khoan_thu.forEach((item) => {
            //       this._defaultNavigation.push({
            //         id: item.menu.id,
            //         badge: item.menu.badge_approval,
            //         module: 'system',
            //         title: '',
            //         link: item.menu.url,
            //         translate: item.menu.translate,
            //         icon: item.menu.icon,
            //         type: 'basic',
            //       });

            //       // else if (item.menu.id == "sdb_khoi_tao" || item.menu.id == "sdb_duyet_khoi_tao" || item.menu.id == "hbs_ban_hanh_lan_dau_hoc_ba") {
            //       //     menu_khoi_tao_so_dang_bo.children.push(
            //       //     {
            //       //         id: item.menu.id,
            //       //         badge: item.menu.badge_approval,
            //       //         module: 'system',
            //       //         title: '',
            //       //         link: item.menu.url,
            //       //         translate: item.menu.translate,
            //       //         icon: item.menu.icon,
            //       //         type: 'basic',

            //       //     });
            //       // }
            //     });

            //     this._defaultNavigation.push(menu_quan_li_khoan_thu);
            //   } else if (ma == 'so_chu_nhiem') {
            //     var menu_so_chu_nhiem = this.menu.filter((d) =>
            //       this.checkInclueFn(d.menu.id, [
            //         'scn_khoi_tao',
            //         'scn_danh_sach_hoc_sinh',
            //         'scn_danh_sach_giao_vien_bo_mon',
            //         'scn_ban_dai_dien_cha_me',
            //         //'scn_cau_hinh_ban_can_bo_lop',
            //         'scn_ban_can_bo_lop',
            //         'scn_ke_hoach_chu_nhiem',
            //         'scn_cau_hinh_to',
            //         'scn_cau_hinh_so_do_lop',
            //         'scn_danh_sach_hoc_sinh_dac_biet',
            //         'scn_tinh_hinh_lop',
            //         'scn_dac_diem_tinh_hinh_lop',
            //         'scn_noi_dung_ke_hoach',
            //         'scn_noi_dung_hoat_dong_thang',
            //         'scn_quy_dinh_phong_cach_hoc_sinh',
            //         'scn_so_ket_hoc_ky_1',
            //         'scn_phuong_huong_hoc_ky_2',
            //         'scn_ket_qua_ren_luyen_theo_thang',
            //         'scn_theo_doi_phu_huynh_di_hop',
            //         'scn_noi_dung_hop_phu_huynh',
            //         'scn_nhan_xet_to_khoi_truong_lanh_dao',
            //         'scn_theo_doi_hoc_sinh_can_quan_tam',
            //         'scn_cac_hoat_dong_da_thuc_hien',
            //       ]),
            //     );

            //     var menu_khoi_tao_scn: FuseNavigationItem = {
            //       id: 'menu_khoi_tao_scn',
            //       module: 'system',
            //       title: '',
            //       translate: 'system.khoi_tao_so_chu_nhiem',
            //       type: 'group',
            //       icon: 'apps',
            //       children: [],
            //     };

            //     var menu_xem_danh_sach_scn: FuseNavigationItem = {
            //       id: 'menu_xem_danh_sach_scn',
            //       module: 'system',
            //       title: '',
            //       translate: 'system.xem_danh_sach_so_chu_nhiem',
            //       type: 'group',
            //       icon: 'apps',
            //       children: [],
            //     };

            //     var menu_cap_nhat_thong_tin_scn: FuseNavigationItem = {
            //       id: 'menu_cap_nhat_thong_tin_scn',
            //       module: 'system',
            //       title: '',
            //       translate: 'system.cap_nhat_thong_tin_so_chu_nhiem',
            //       type: 'group',
            //       icon: 'apps',
            //       children: [],
            //     };

            //     menu_so_chu_nhiem.forEach((item) => {
            //       if (item.menu.id == 'scn_khoi_tao') {
            //         menu_khoi_tao_scn.children.push({
            //           id: item.menu.id,
            //           badge: item.menu.badge_approval,
            //           module: 'system',
            //           title: '',
            //           link: item.menu.url,
            //           translate: item.menu.translate,
            //           icon: item.menu.icon,
            //           type: 'basic',
            //         });
            //       }
            //     });
            //     if (menu_khoi_tao_scn.children.length > 0) {
            //       this._defaultNavigation.push(menu_khoi_tao_scn);
            //     }
            //     menu_so_chu_nhiem.forEach((item) => {
            //       if (item.menu.id == 'scn_danh_sach_giao_vien_bo_mon' || item.menu.id == 'scn_danh_sach_hoc_sinh') {
            //         menu_xem_danh_sach_scn.children.push({
            //           id: item.menu.id,
            //           badge: item.menu.badge_approval,
            //           module: 'system',
            //           title: '',
            //           link: item.menu.url,
            //           translate: item.menu.translate,
            //           icon: item.menu.icon,
            //           type: 'basic',
            //         });
            //       }
            //     });
            //     if (menu_xem_danh_sach_scn.children.length > 0) {
            //       this._defaultNavigation.push(menu_xem_danh_sach_scn);
            //     }
            //     menu_so_chu_nhiem.forEach((item) => {
            //       if (
            //         item.menu.id != 'scn_khoi_tao' &&
            //         item.menu.id != 'scn_danh_sach_giao_vien_bo_mon' &&
            //         item.menu.id != 'scn_danh_sach_hoc_sinh'
            //       ) {
            //         menu_cap_nhat_thong_tin_scn.children.push({
            //           id: item.menu.id,
            //           badge: item.menu.badge_approval,
            //           module: 'system',
            //           title: '',
            //           link: item.menu.url,
            //           translate: item.menu.translate,
            //           icon: item.menu.icon,
            //           type: 'basic',
            //         });
            //       }
            //     });
            //     if (menu_cap_nhat_thong_tin_scn.children.length > 0) {
            //       this._defaultNavigation.push(menu_cap_nhat_thong_tin_scn);
            //     }

            //     this._defaultNavigation.push(menu_so_chu_nhiem);
            //   } else if (ma == 'so_ghi_dau_bai') {
            //     var menu_so_ghi_dau_bai = this.menu.filter((d) =>
            //       this.checkInclueFn(d.menu.id, ['sgdb_tong_quan', 'sgdb_so_dau_bai']),
            //     );

            //     menu_so_ghi_dau_bai.forEach((item) => {
            //       this._defaultNavigation.push({
            //         id: item.menu.id,
            //         badge: item.menu.badge_approval,
            //         module: 'system',
            //         title: '',
            //         link: item.menu.url,
            //         translate: item.menu.translate,
            //         icon: item.menu.icon,
            //         type: 'basic',
            //       });
            //     });

            //     this._defaultNavigation.push(menu_so_ghi_dau_bai);
            //   } else if (ma == 'so_dang_bo') {
            //     var menu_so_dang_bo = this.menu.filter((d) =>
            //       this.checkInclueFn(d.menu.id, [
            //         //"sdb_tong_quan",
            //         'sdb_khoi_tao',
            //         'sdb_duyet_khoi_tao',
            //         'sdb_ban_hanh',
            //         //"sdb_ky_va_dong_dau",
            //         //"sdb_dong_bo_du_lieu",
            //         'sdb_cau_hinh_ma_so_dang_bo',
            //         'sdb_quan_ly_so_dang_bo',
            //         'sdb_tra_cuu',
            //         'sdb_bao_cao_thong_ke',
            //       ]),
            //     );
            //     var menu_khoi_tao_so_dang_bo: FuseNavigationItem = {
            //       id: 'menu_khoi_tao_so_dang_bo',
            //       module: 'system',
            //       title: '',
            //       translate: 'system.khoi_tao_so_dang_bo',
            //       type: 'group',
            //       icon: 'apps',
            //       children: [],
            //     };

            //     menu_so_dang_bo.forEach((item) => {
            //       this._defaultNavigation.push({
            //         id: item.menu.id,
            //         badge: item.menu.badge_approval,
            //         module: 'system',
            //         title: '',
            //         link: item.menu.url,
            //         translate: item.menu.translate,
            //         icon: item.menu.icon,
            //         type: 'basic',
            //       });
            //       // if (item.menu.id == "sdb_tong_quan") {
            //       //     this._defaultNavigation.push({
            //       //         id: item.menu.id,
            //       //         badge: item.menu.badge_approval,
            //       //         module: 'system',
            //       //         title: '',
            //       //         link: item.menu.url,
            //       //         translate: item.menu.translate,
            //       //         icon: item.menu.icon,
            //       //         type: 'basic',
            //       //     });
            //       // }
            //       // else if (item.menu.id == "sdb_khoi_tao" || item.menu.id == "sdb_duyet_khoi_tao" || item.menu.id == "sdb_ban_hanh") {
            //       //     menu_khoi_tao_so_dang_bo.children.push(
            //       //     {
            //       //         id: item.menu.id,
            //       //         badge: item.menu.badge_approval,
            //       //         module: 'system',
            //       //         title: '',
            //       //         link: item.menu.url,
            //       //         translate: item.menu.translate,
            //       //         icon: item.menu.icon,
            //       //         type: 'basic',

            //       //     });
            //       // }
            //     });

            //     // if (menu_khoi_tao_so_dang_bo.children.length > 0) {
            //     //     this._defaultNavigation.push(menu_khoi_tao_so_dang_bo);
            //     // }
            //     // menu_so_dang_bo.forEach(item => {
            //     //     if (item.menu.id == "sdb_dong_bo_du_lieu") {
            //     //         this._defaultNavigation.push({
            //     //             id: item.menu.id,
            //     //             badge: item.menu.badge_approval,
            //     //             module: 'system',
            //     //             title: '',
            //     //             link: item.menu.url,
            //     //             translate: item.menu.translate,
            //     //             icon: item.menu.icon,
            //     //             type: 'basic',
            //     //         });
            //     //     }
            //     //     if (item.menu.id == "sdb_cau_hinh_ma_so_dang_bo") {
            //     //         this._defaultNavigation.push({
            //     //             id: item.menu.id,
            //     //             badge: item.menu.badge_approval,
            //     //             module: 'system',
            //     //             title: '',
            //     //             link: item.menu.url,
            //     //             translate: item.menu.translate,
            //     //             icon: item.menu.icon,
            //     //             type: 'basic',
            //     //         });
            //     //     }
            //     //     if (item.menu.id == "sdb_quan_ly_so_dang_bo") {
            //     //         this._defaultNavigation.push({
            //     //             id: item.menu.id,
            //     //             badge: item.menu.badge_approval,
            //     //             module: 'system',
            //     //             title: '',
            //     //             link: item.menu.url,
            //     //             translate: item.menu.translate,
            //     //             icon: item.menu.icon,
            //     //             type: 'basic',
            //     //         });
            //     //     }
            //     //     if (item.menu.id == "sdb_tra_cuu") {
            //     //         this._defaultNavigation.push({
            //     //             id: item.menu.id,
            //     //             badge: item.menu.badge_approval,
            //     //             module: 'system',
            //     //             title: '',
            //     //             link: item.menu.url,
            //     //             translate: item.menu.translate,
            //     //             icon: item.menu.icon,
            //     //             type: 'basic',
            //     //         });
            //     //     }
            //     //     if (item.menu.id == "sdb_bao_cao_thong_ke") {
            //     //         this._defaultNavigation.push({
            //     //             id: item.menu.id,
            //     //             badge: item.menu.badge_approval,
            //     //             module: 'system',
            //     //             title: '',
            //     //             link: item.menu.url,
            //     //             translate: item.menu.translate,
            //     //             icon: item.menu.icon,
            //     //             type: 'basic',
            //     //         });
            //     //     }
            //     // })
            //   } else if (ma == 'so_dang_bo_giao_vien') {
            //     var menu_so_dang_bo_giao_vien = this.menu.filter((d) =>
            //       this.checkInclueFn(d.menu.id, [
            //         //"sdbgv_tong_quan",
            //         'sdbgv_khoi_tao',
            //         'sdbgv_duyet_khoi_tao',
            //         'sdbgv_ban_hanh',
            //         //"sdbgv_ky_va_dong_dau",
            //         //"sdbgv_dong_bo_du_lieu",
            //         'sdbgv_cau_hinh_ma_so_dang_bo',
            //         'sdbgv_quan_ly_so_dang_bo',
            //         'sdbgv_tra_cuu',
            //         'sdbgv_bao_cao_thong_ke',
            //       ]),
            //     );
            //     menu_so_dang_bo_giao_vien.forEach((item) => {
            //       this._defaultNavigation.push({
            //         id: item.menu.id,
            //         badge: item.menu.badge_approval,
            //         module: 'system',
            //         title: '',
            //         link: item.menu.url,
            //         translate: item.menu.translate,
            //         icon: item.menu.icon,
            //         type: 'basic',
            //       });
            //     });
            //   } else if (ma == 'so_theo_doi_va_danh_gia_hoc_sinh') {
            //     var menu_so_theo_doi_va_danh_gia_hoc_sinh = this.menu.filter((d) =>
            //       this.checkInclueFn(d.menu.id, [
            //         // 'std_tong_quan',
            //         'std_khoi_tao',
            //         'std_ky_so',
            //         'std_ky_so_gvcn',
            //         'std_ky_so_gvbm',
            //         //"std_ky_so_va_dong_dau",
            //         //"std_ky_so_va_dong_dau_gvbm",
            //         'std_dong_dau',
            //         // 'std_ky_so_va_dong_dau',
            //         'std_tra_cuu',
            //       ]),
            //     );
            //     menu_so_theo_doi_va_danh_gia_hoc_sinh.forEach((item) => {
            //       this._defaultNavigation.push({
            //         id: item.menu.id,
            //         badge: item.menu.badge_approval,
            //         module: 'system',
            //         title: '',
            //         link: item.menu.url,
            //         translate: item.menu.translate,
            //         icon: item.menu.icon,
            //         type: 'basic',
            //       });
            //     });
            //   } else if (ma == 'quan_li_ho_so') {
            //     var menu_quan_li_ho_so = this.menu.filter((d) => this.checkInclueFn(d.menu.id, ['qlhoso_tong_quan']));
            //     menu_quan_li_ho_so.forEach((item) => {
            //       this._defaultNavigation.push({
            //         id: item.menu.id,
            //         badge: item.menu.badge_approval,
            //         module: 'system',
            //         title: '',
            //         link: item.menu.url,
            //         translate: item.menu.translate,
            //         icon: item.menu.icon,
            //         type: 'basic',
            //       });
            //     });
            //   } else if (ma == 'ket_qua_hoc_tap') {
            //     var menu_ket_qua_hoc_tap = this.menu.filter((d) =>
            //       this.checkInclueFn(d.menu.id, [
            //         //2 trang này phân quyền cho user giáo viên
            //         'qlhs_quan_ly_theo_bo_mon',
            //         'gv_diem_danh_hoc_sinh_vang',
            //         'gv_diem_danh_hoc_sinh',
            //         'qlhs_quan_ly_theo_lop_hoc',
            //         //
            //         'qlhs_tong_quan',
            //         'qlhs_so_theo_doi',
            //         'qlhs_ket_qua_hoc_tap',
            //         'qlhs_xet_lop',
            //         'qlhs_tong_hop_ket_qua_sau_thi_lai',
            //         //'qlhs_xem_lich_su',
            //       ]),
            //     );
            //     menu_ket_qua_hoc_tap.forEach((item) => {
            //       this._defaultNavigation.push({
            //         id: item.menu.id,
            //         badge: item.menu.badge_approval,
            //         module: 'system',
            //         title: '',
            //         link: item.menu.url,
            //         translate: item.menu.translate,
            //         icon: item.menu.icon,
            //         type: 'basic',
            //       });
            //     });
            //   } else if (ma == 'tuyen_sinh') {
            //     var menu_website_truong_hoc = this.menu.filter((d) =>
            //       this.checkInclueFn(d.menu.id, [
            //         'khts_tong_quan',
            //         'khts_lap_ke_hoach',
            //         'khts_thong_bao',
            //         'khts_ho_so_tuyen_sinh',
            //         'khts_xep_lop',
            //       ]),
            //     );
            //     menu_website_truong_hoc.forEach((item) => {
            //       this._defaultNavigation.push({
            //         id: item.menu.id,
            //         badge: item.menu.badge_approval,
            //         module: 'system',
            //         title: '',
            //         link: item.menu.url,
            //         translate: item.menu.translate,
            //         icon: item.menu.icon,
            //         type: 'basic',
            //       });
            //     });
            //   } else if (ma == 'thoi_khoa_bieu') {
            //     var menu_website_truong_hoc = this.menu.filter((d) =>
            //       this.checkInclueFn(d.menu.id, [
            //         //"sys_lop_thoi_khoa_bieu",
            //         'sys_danh_muc_mon_hoc',
            //         'sys_mon_hoc',
            //         'sys_quan_li_lop',
            //         'tkb_lich_hoc',
            //         'tkb_phan_phoi_chuong_trinh',
            //         'tkb_phan_cong_giang_day',
            //         'tkb_thiet_lap_rang_buoc',
            //         'tkb_thoi_gian_ban',
            //         'tkb_so_tiet_toi_da_trong_ngay',
            //         'tkb_tiet_giang_khong_chong_lan',
            //         'tkb_tiet_hoc_co_dinh',
            //         'tkb_thoi_khoa_bieu',
            //         'sys_thoi_khoa_bieu_admin',
            //       ]),
            //     );
            //     menu_website_truong_hoc.forEach((item) => {
            //       this._defaultNavigation.push({
            //         id: item.menu.id,
            //         badge: item.menu.badge_approval,
            //         module: 'system',
            //         title: '',
            //         link: item.menu.url,
            //         translate: item.menu.translate,
            //         icon: item.menu.icon,
            //         type: 'basic',
            //       });
            //     });
            //   } else if (ma == 'he_thong') {
            //     var he_thong = this.menu.filter((d) =>
            //       this.checkInclueFn(d.menu.id, [
            //         'sys_group_user',
            //         'sys_phan_quyen_tai_khoan',
            //         'sys_user',
            //         'sys_cau_hinh_ma_he_thong',
            //         'sys_loai_mat_hang',
            //         'sys_cau_hinh_anh_mac_dinh',
            //         'sys_template_mail',
            //         'sys_thong_bao',
            //       ]),
            //     );

            //     var menu_quan_li_tai_khoan: FuseNavigationItem = {
            //       id: 'menu_quan_li_tai_khoan',
            //       module: 'system',
            //       title: '',
            //       translate: 'system.quan_li_tai_khoan',
            //       type: 'group',
            //       icon: 'apps',
            //       children: [],
            //     };
            //     var menu_cau_hinh_he_thong: FuseNavigationItem = {
            //       id: 'menu_cau_hinh_he_thong',
            //       module: 'system',
            //       title: '',
            //       translate: 'system.cau_hinh_he_thong',
            //       type: 'group',
            //       icon: 'apps',
            //       children: [],
            //     };
            //     he_thong.forEach((item) => {
            //       if (
            //         item.menu.id == 'sys_module' ||
            //         item.menu.id == 'sys_group_user' ||
            //         item.menu.id == 'sys_user' ||
            //         item.menu.id == 'sys_phan_quyen_tai_khoan'
            //       ) {
            //         menu_quan_li_tai_khoan.children.push({
            //           id: item.menu.id,
            //           badge: item.menu.badge_approval,
            //           module: 'system',
            //           title: '',
            //           link: item.menu.url,
            //           translate: item.menu.translate,
            //           icon: item.menu.icon,
            //           type: 'basic',
            //         });
            //       }
            //       if (
            //         item.menu.id != 'sys_group_user' &&
            //         item.menu.id != 'sys_user' &&
            //         item.menu.id != 'sys_module' &&
            //         item.menu.id != 'sys_phan_quyen_tai_khoan'
            //       ) {
            //         menu_cau_hinh_he_thong.children.push({
            //           id: item.menu.id,
            //           badge: item.menu.badge_approval,
            //           module: 'system',
            //           title: '',
            //           link: item.menu.url,
            //           translate: item.menu.translate,
            //           icon: item.menu.icon,
            //           type: 'basic',
            //         });
            //       }
            //       //if (item.menu.id == "sys_giao_vien" || item.menu.id == "sys_lop_giao_vien") {
            //       //    this._defaultNavigation.push({
            //       //        id: item.menu.id,
            //       //        badge: item.menu.badge_approval,
            //       //        module: 'system',
            //       //        title: '',
            //       //        link: item.menu.url,
            //       //        translate: item.menu.translate,
            //       //        icon: item.menu.icon,
            //       //        type: 'basic',
            //       //    });

            //       //}
            //     });

            //     if (menu_quan_li_tai_khoan.children.length > 0) {
            //       this._defaultNavigation.push(menu_quan_li_tai_khoan);
            //     }

            //     if (menu_cau_hinh_he_thong.children.length > 0) {
            //       this._defaultNavigation.push(menu_cau_hinh_he_thong);
            //     }
            //   } else {
            //     this.menu.forEach((item) => {
            //       this._defaultNavigation.push({
            //         id: item.menu.id,
            //         badge: item.menu.badge_approval,
            //         module: 'system',
            //         title: '',
            //         link: item.menu.url,
            //         translate: item.menu.translate,
            //         icon: item.menu.icon,
            //         type: 'basic',
            //       });
            //     });
            //     //var allMenuItems = this.menu.map(item => ({
            //     //    id: item.menu.id,
            //     //    badge: item.menu.badge_approval,
            //     //    module: 'system',
            //     //    title: '',
            //     //    link: item.menu.url,
            //     //    translate: item.menu.translate,
            //     //    icon: item.menu.icon,
            //     //    type: 'basic',
            //     //}));

            //     // Thêm tất cả menu vào _defaultNavigation
            //     //var allMenuGroups: FuseNavigationItem = {
            //     //    id: "all_menus",
            //     //    module: 'system',
            //     //    title: "",
            //     //    translate: "Tất cả menu",
            //     //    type: 'group',
            //     //    icon: "menu",
            //     //    children: allMenuItems
            //     //};
            //     //this._defaultNavigation.push(allMenuItems);
            //   }

            //   //var menu_quan_li_website: FuseNavigationItem = {
            //   //    id: 'administrative',
            //   //    module: 'administrative',
            //   //    title: 'administrative.quan_li_website',
            //   //    translate: 'administrative.quan_li_website',
            //   //    type: 'collapsable',
            //   //    icon: 'mat_outline:event_note', //icon_img: "/assets/images/shungo/blue_feed_icon.png",

            //   //    children: [],
            //   //};
            //   Swal.close();
            // }

            // Return the response
            resp = {
              compact: this._defaultNavigation,
              default: this._defaultNavigation,
              futuristic: this._defaultNavigation,
              horizontal: this._defaultNavigation,
            };
            return of(resp);
          }),
        ),
      this._httpClient.get<any>('api/common/notifications'),
      this._httpClient.get<any>('api/common/shortcuts'),
      this._httpClient
        .post<any>('sys_home.ctr/checkLogin', {
          accessToken: this._authService.accessToken,
        })
        .pipe(
          switchMap((resp: any) => {
            resp = JSON.parse(localStorage.getItem('user'));
            return of(resp);
          }),
        ),
    ]).pipe(
      map(([messages, navigation, notifications, shortcuts, user]) => ({
        messages,
        navigation: {
          compact: navigation.compact,
          default: navigation.default,
          futuristic: navigation.futuristic,
          horizontal: navigation.horizontal,
          name_module: navigation.name_module,
        },
        notifications,
        shortcuts,
        user,
      })),
    );
  }

  checkInclueFn(listInclude: any, stringValue: any): boolean {
    let isInclude = false;
    stringValue.every((element) => {
      if (listInclude === element) {
        isInclude = true;
        return false;
      } else return true;
    });
    return isInclude;
  }
}
