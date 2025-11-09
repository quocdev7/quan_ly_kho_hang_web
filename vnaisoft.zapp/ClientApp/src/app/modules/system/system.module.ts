import { MatSidenavModule } from '@angular/material/sidenav';
import { FuseMasonryModule } from '@fuse/components/masonry';
import { NzProgressModule } from 'ng-zorro-antd/progress';
import { NgModule } from '@angular/core';
import { MatButtonToggleModule } from '@angular/material/button-toggle';
import { RouterModule } from '@angular/router';
import { NgxChartsModule } from '@swimlane/ngx-charts';
import { MatBadgeModule } from '@angular/material/badge';
import { DataTablesModule } from 'angular-datatables';
import { MatButtonModule } from '@angular/material/button';
import { MatChipsModule } from '@angular/material/chips';
import { MatRippleModule } from '@angular/material/core';
import { MatExpansionModule } from '@angular/material/expansion';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatIconModule } from '@angular/material/icon';
import { MatInputModule } from '@angular/material/input';
import { MatPaginatorModule } from '@angular/material/paginator';
import { MatSelectModule } from '@angular/material/select';
import { MatSnackBarModule } from '@angular/material/snack-bar';
import { MatSortModule } from '@angular/material/sort';
import { MatTableModule } from '@angular/material/table';
import { MatDialogModule } from '@angular/material/dialog';
import { MatMenuModule } from '@angular/material/menu';
import { SweetAlert2Module } from '@sweetalert2/ngx-sweetalert2';
import { NgApexchartsModule } from 'ng-apexcharts';
import { DragDropModule } from '@angular/cdk/drag-drop';
import { NgxMatSelectSearchModule } from 'ngx-mat-select-search';
import { MatCardModule } from '@angular/material/card';
import { MatCheckboxModule } from '@angular/material/checkbox';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { MatTreeModule } from '@angular/material/tree';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatTooltipModule } from '@angular/material/tooltip';
import { MatListModule } from '@angular/material/list';
import { MatSlideToggleModule } from '@angular/material/slide-toggle';
import { NgxMatTimepickerModule } from '@angular-material-components/datetime-picker';
import { MatAutocompleteModule } from '@angular/material/autocomplete';
import { EditorModule, TINYMCE_SCRIPT_SRC } from '@tinymce/tinymce-angular';
import { TranslocoCoreModule } from 'app/core/transloco/transloco.module';
import { commonModule } from '@fuse/components/commonComponent/common.module';
import { common_pageModule } from '@fuse/components/commonComponent/common_page.module';
import { systemsRoutes } from './system.routing';
import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { FusePipesModule } from '@fuse/pipes/pipes.module';
import { NzEmptyModule } from 'ng-zorro-antd/empty';
import { TranslocoModule } from '@ngneat/transloco';
import { NzIconModule } from 'ng-zorro-antd/icon';
import { MatDividerModule } from '@angular/material/divider';
import { MatTabsModule } from '@angular/material/tabs';
import { NgCircleProgressModule } from 'ng-circle-progress';
import { CalendarModule } from 'app/calendar/calendar.module';
import { MatProgressBarModule } from '@angular/material/progress-bar';
import { SharedModule } from 'app/shared/shared.module';
import { AgmCoreModule } from '@agm/core';
import { FuseDateRangeModule } from '@fuse/components/date-range';
import { FullCalendarModule } from '@fullcalendar/angular';
import { sys_group_user_indexComponent } from './sys_group_user/index.component';
import { sys_group_user_popUpAddComponent } from './sys_group_user/popupAdd.component';
import { sys_user_indexComponent } from './sys_user/index.component';
import { sys_user_popUpAddComponent } from './sys_user/popupAdd.component';
import { changePassComponent } from './sys_user/changePass.component';
import { MatRadioModule } from '@angular/material/radio';
import { LottieModule } from 'ngx-lottie';
import player from 'lottie-web';
import { ImageCropperModule } from 'ngx-image-cropper';
import { FuseNavigationModule } from '@fuse/components/navigation';
import { sys_cau_hinh_anh_mac_dinh_indexComponent } from './sys_cau_hinh_anh_mac_dinh/index.component';
import { sys_cau_hinh_anh_mac_dinh_popUpAddComponent } from './sys_cau_hinh_anh_mac_dinh/popupAdd.component';
import { sys_template_mail_indexComponent } from './sys_template_mail/index.component';
import { sys_template_mail_popUpAddComponent } from './sys_template_mail/popupAdd.component';

import { NgxCaptureModule } from 'ngx-capture';
import { DragDropDirective } from '@fuse/directives/drag-drop';
import { drag_dropModule } from '@fuse/directives/drag_drop';

import { ScrollingModule } from '@angular/cdk/scrolling';


import { sys_cau_hinh_ma_he_thong_indexComponent } from './sys_cau_hinh_ma_he_thong/index.component';
import { sys_loai_mat_hang_indexComponent } from './sys_loai_mat_hang/index.component';
import { sys_loai_mat_hang_popUpAddComponent } from './sys_loai_mat_hang/popupAdd.component';
import { sys_don_vi_tinh_indexComponent } from './sys_don_vi_tinh/index.component';
import { sys_don_vi_tinh_popUpAddComponent } from './sys_don_vi_tinh/popupAdd.component';
import { sys_khach_hang_nha_cung_cap_indexComponent } from './sys_khach_hang_nha_cung_cap/index.component';
import { sys_khach_hang_nha_cung_cap_popUpAddComponent } from './sys_khach_hang_nha_cung_cap/popupAdd.component';
import { sys_mat_hang_indexComponent } from './sys_mat_hang/index.component';
import { sys_mat_hang_popUpAddComponent } from './sys_mat_hang/popupAdd.component';
import { sys_mat_hang_popupChooseDacTinhComponent } from './sys_mat_hang/popupChooseDacTinh.component';
import { sys_don_hang_ban_indexComponent } from './sys_don_hang_ban/index.component';
import { sys_don_hang_ban_popUpAddComponent } from './sys_don_hang_ban/popupAdd.component';
import { sys_don_hang_mua_indexComponent } from './sys_don_hang_mua/index.component';
import { sys_don_hang_mua_popUpAddComponent } from './sys_don_hang_mua/popupAdd.component';
import { sys_common_popupChooseMatHangComponent } from './sys_common/popupChooseMatHang';
import { sys_phieu_nhap_kho_indexComponent } from './sys_phieu_nhap_kho/index.component';
import { sys_phieu_nhap_kho_popUpAddComponent } from './sys_phieu_nhap_kho/popupAdd.component';
import { sys_phieu_xuat_kho_indexComponent } from './sys_phieu_xuat_kho/index.component';
import { sys_phieu_xuat_kho_popUpAddComponent } from './sys_phieu_xuat_kho/popupAdd.component';
import { bao_cao_ton_kho_mat_hang_indexComponent } from './sys_bao_cao_ton_kho/index.component';
import { bao_cao_nhap_kho_indexComponent } from './sys_bao_cao_nhap_kho/index.component';
import { bao_cao_xuat_kho_indexComponent } from './sys_bao_cao_xuat_kho/index.component';




//import { sys_lop_giao_vien_popUpAddComponent } from './sys_lop_giao_vien/popupAdd.component';

// Note we need a separate function as it's required
// by the AOT compiler.
export function playerFactory() {
    return player;
}
@NgModule({
    providers: [
        { provide: TINYMCE_SCRIPT_SRC, useValue: 'tinymce/tinymce.min.js' },
        // { provide: MAT_DATE_LOCALE, useValue: 'vi-VN' }
    ],
    declarations: [
        bao_cao_xuat_kho_indexComponent,
        bao_cao_nhap_kho_indexComponent,
        bao_cao_ton_kho_mat_hang_indexComponent,
        sys_phieu_xuat_kho_popUpAddComponent,
        sys_phieu_xuat_kho_indexComponent,
        sys_phieu_nhap_kho_popUpAddComponent,
        sys_phieu_nhap_kho_indexComponent,
        sys_common_popupChooseMatHangComponent,
        sys_mat_hang_indexComponent,
        sys_mat_hang_popUpAddComponent,
        sys_mat_hang_popupChooseDacTinhComponent,
        sys_don_hang_ban_indexComponent,
        sys_don_hang_ban_popUpAddComponent,
        sys_don_hang_mua_indexComponent,
        sys_don_hang_mua_popUpAddComponent,
        sys_khach_hang_nha_cung_cap_indexComponent,
        sys_khach_hang_nha_cung_cap_popUpAddComponent,
        sys_don_vi_tinh_popUpAddComponent,
        sys_don_vi_tinh_indexComponent,
        sys_loai_mat_hang_popUpAddComponent,
        sys_loai_mat_hang_indexComponent,
        sys_cau_hinh_ma_he_thong_indexComponent,
        sys_user_popUpAddComponent,
        sys_user_indexComponent,
        changePassComponent,
        sys_group_user_indexComponent,
        sys_group_user_popUpAddComponent,
        sys_cau_hinh_anh_mac_dinh_indexComponent,
        sys_cau_hinh_anh_mac_dinh_popUpAddComponent,
        sys_template_mail_indexComponent,
        sys_template_mail_popUpAddComponent,
        
    ],
    imports: [
        ScrollingModule,
        drag_dropModule,
        LottieModule.forRoot({ player: playerFactory }),
        RouterModule.forChild(systemsRoutes),
        FullCalendarModule,
        ImageCropperModule,
        MatTableModule,
        FuseNavigationModule,
        FuseDateRangeModule,
        SweetAlert2Module,
        MatButtonModule,
        MatRadioModule,
        MatChipsModule,
        MatExpansionModule,
        MatFormFieldModule,
        MatIconModule,
        MatInputModule,
        MatMenuModule,
        MatPaginatorModule,
        MatTreeModule,
        MatRippleModule,
        MatSelectModule,
        MatBadgeModule,
        MatCheckboxModule,
        MatSortModule,
        MatSnackBarModule,
        MatTabsModule,
        MatDialogModule,
        DataTablesModule,
        MatCardModule,
        MatDatepickerModule,
        NgxChartsModule,
        NgxMatSelectSearchModule,
        MatAutocompleteModule,
        MatTooltipModule,
        commonModule,
        common_pageModule,
        EditorModule,
        MatListModule,
        MatSlideToggleModule,
        TranslocoCoreModule,
        CommonModule,
        FormsModule,
        FusePipesModule,
        NzEmptyModule,
        MatButtonToggleModule,
        TranslocoModule,
        NzIconModule,
        MatProgressBarModule,
        SharedModule,
        NgApexchartsModule,
        NgCircleProgressModule.forRoot({
            // set defaults here
            radius: 100,
            outerStrokeWidth: 16,
            innerStrokeWidth: 8,
            outerStrokeColor: '#78C000',
            innerStrokeColor: '#C7E596',
            animationDuration: 300,
        }),
        AgmCoreModule.forRoot({
            apiKey: 'AIzaSyA1T28g5sfWOnkimOZBJutBACuO91CC4o0',
            libraries: ['places'],
        }),
        ReactiveFormsModule,
        NgxMatTimepickerModule,
        MatDividerModule,
        NzProgressModule,
        MatSidenavModule,
        FuseMasonryModule,
        EditorModule,
        CalendarModule,
        DragDropModule,
        MatProgressSpinnerModule,
        NgxCaptureModule,
    ],
    exports: [],
})
export class SystemModule { }
