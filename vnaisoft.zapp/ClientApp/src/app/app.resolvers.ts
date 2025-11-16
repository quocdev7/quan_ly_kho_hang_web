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
            //Báo cáo
            var bao_cao = this.menu.filter((d) =>
              this.checkInclueFn(d.menu.id, [
                'bao_cao_nhap_kho',
                'bao_cao_xuat_kho',
                'bao_cao_ton_kho_mat_hang',
              ]),
            );
            var menu_bao_cao: FuseNavigationItem = {
                id: 'bao_cao',
                module: 'system',
                title: '',
                translate: 'administrative.bao_cao',
                type: 'group',
                icon: 'apps',
                icon_img: "/assets/icons/erp.png",
                children: [],
            };
            bao_cao.forEach((item) => {
              menu_bao_cao.children.push({
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

            if (menu_bao_cao.children.length > 0) {
              this._defaultNavigation.push(menu_bao_cao);
            }
            Swal.close();
            
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
