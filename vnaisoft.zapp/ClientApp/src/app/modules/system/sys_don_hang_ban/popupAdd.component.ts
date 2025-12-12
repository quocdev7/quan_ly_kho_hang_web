import { ChangeDetectorRef, Component, HostListener, Inject } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef, MatDialog } from '@angular/material/dialog';


import { HttpClient, HttpEventType } from '@angular/common/http';
import { BasePopUpAddComponent } from 'app/Basecomponent/BasePopupAdd.component';
import { TranslocoService } from '@ngneat/transloco';
import { FuseNavigationService } from '@fuse/components/navigation';
import { ActivatedRoute } from '@angular/router';
import { BasePopUpAddTypeComponent } from 'app/Basecomponent/BasePopupAddType.component';
//import { sys_don_hang_ban_mat_hang_model, sys_don_hang_ban_model } from './sys_don_hang_ban.types';
import Swal from 'sweetalert2';
import { forEach, isNaN, trim } from 'lodash';

import { list_vat } from 'app/core/data/data';
import { mode } from 'crypto-js';
//import { sys_common_popupChooseMatHangComponent } from '../sys_common/popupChooseMatHang.component';
//import { sys_common_popupChooseDoiTuongComponent } from '../sys_common/popupChooseDoiTuong.component';
//import { sys_tai_khoan_ngan_hang_popUpAddComponent } from '../sys_tai_khoan_ngan_hang/popupAdd.component';
import { cm_mau_in_popupComponent } from '@fuse/components/commonComponent/cm_mau_in/cm_mau_in_popup.component';
import { doi_tuong_tu_do } from 'app/core/data/data';
import { sys_don_hang_ban_model } from './sys_don_hang_ban.types';
import { sys_common_popupChooseMatHangComponent } from '../sys_common/popupChooseMatHang';
//import { sys_don_vi_van_chuyen_popUpAddComponent } from '../sys_don_vi_van_chuyen/popupAdd.component';
//import { sys_common_popupChooseCaNhanToChucComponent } from '../sys_common/popupChooseCaNhanToChuc.component';
// import { sys_common_popupChooseMatHangTonKhoComponent } from '../sys_common/popupChooseMatHangTonKho.component';
// import { sys_common_popupChooseMatHangDuocChonLaiComponent } from '../sys_common/popupChooseMatHangDuocChonLai';
// import { sys_don_hang_ban_popupHistoryEditComponent } from './popupHistoryEdit.component';


@Component({
    selector: 'sys_don_hang_ban_popupAdd',
    templateUrl: 'popupAdd.html',
    // styleUrls: ['./popupAdd.component.scss']
})
export class sys_don_hang_ban_popUpAddComponent extends BasePopUpAddTypeComponent<sys_don_hang_ban_model> {
    public file_logo: any;
    public Progress_logo: any = -1;
    public list_mat_hang: any;
    public mat_hang: any;
    public list_vat: any;
    public gia_tri_vat: any;

    public total_so_luong: any;
    public total_thanh_tien_truoc_thue: any;
    public total_so_tien_chiet_khau: any;
    public total_thanh_tien_sau_chiet_khau: any;
    public total_tien_thue: any;
    public total_thanh_tien_sau_thue: any;
    public fileData: any;
    public list_ngan_hang: any;
    public list_don_vi_tinh: any;
    public ma_mat_hang: any;
    public file: any;
    public record: any;
    constructor(public dialogRef: MatDialogRef<sys_don_hang_ban_popUpAddComponent>,
        http: HttpClient, _translocoService: TranslocoService,
        _fuseNavigationService: FuseNavigationService, route: ActivatedRoute,
        private _changeDetectorRef: ChangeDetectorRef,
        @Inject('BASE_URL') baseUrl: string,
        private dialog: MatDialog,
        public dialogModal: MatDialog,
        @Inject(MAT_DIALOG_DATA) data: any) {
        super(_translocoService, _fuseNavigationService, route, baseUrl, http, 'sys_don_hang_ban', dialogRef, dialogModal);

        this.record = data;
        this.Oldrecord = JSON.parse(JSON.stringify(data));
        this.actionEnum = data.actionEnum;
        if (this.actionEnum == 1) {
            this.get_code();
            this.record.db.ngay_dat_hang = new Date();
            this.baseInitData();
        }
        if (this.actionEnum != 1) {
            this.getElementById();
        }
        this.list_vat = list_vat;
    }
    ngOnInit(): void {
        this.get_list_vat();
        this.get_list_don_vi_tinh();
    };
    combinedCode = ""
    startFrom = new Date().getTime();
    @HostListener('document:keypress', ['$event'])
    keyEvent(event: KeyboardEvent) {
        if (event.key === 'Enter') {
            if (this.combinedCode != "" && this.combinedCode.length > 6 && (this.ma_mat_hang ?? "") == "") {
                this.ma_mat_hang = this.combinedCode;
            }
            this.combinedCode = "";
        } else {
            var timming = new Date().getTime() - this.startFrom;
            this.startFrom = new Date().getTime();
            console.log(timming);
            console.log(event.key.toString());
            if (timming < 50 || this.combinedCode == "") {
                this.combinedCode += event.key;
            } else {
                this.combinedCode = "";
            }
        }
    }
    openDialogEdit(item, pos): void {
        const dialogRef = this.dialog.open(sys_don_hang_ban_popUpAddComponent, {
            disableClose: true,
            autoFocus: false,
            width: '100%',
            height: '100%',
            data: {
                actionEnum: 2,
                db: {
                    id: item.db.id,
                },
            }
            //data: model
        });
        dialogRef.afterClosed().subscribe(result => {
            if (result.db.id == 0) return;
            this.close();
        });
    }
    
    get_code() {
        this.http
            .post('/sys_don_hang_ban.ctr/get_code/', {
            }
            ).subscribe(resp => {
                var data: any;
                data = resp;
                this.record.db.ma = "Tự động tạo";
            });
    }
    openDialogChooseMatHang(): void {
         const dialogRef = this.dialogModal
            .open(sys_common_popupChooseMatHangComponent, {
                disableClose: true,
                width: '95%',
                height: '95%',
                data: this.record
            });
        dialogRef.afterClosed().subscribe(result => {
            var data: any;
            data = result;
            if (data == undefined) return;
            this.record.list_mat_hang = data;
            //this.loadThanhTienSauThueMatHangPopup();
        })
    }
    loadThanhTienSauThueMatHang(pos): void {
        debugger
        var mat_hang = this.record.list_mat_hang[pos];
        var vat_mat_hang = this.list_vat.filter(q => q.id == mat_hang.db.vat)[0].value;
        if (vat_mat_hang == undefined || vat_mat_hang == null) {
            vat_mat_hang = 0;
        }
        var don_gia = mat_hang.db.don_gia ?? null;
        this.record.list_mat_hang[pos].db.don_gia = Number(don_gia);
        this.record.list_mat_hang[pos].db.so_luong = Number(mat_hang.db.so_luong ?? 0);
        // this.record.list_mat_hang[pos].db.tien_vat = Math.round(tien_vat);
        this.record.list_mat_hang[pos].db.ghi_chu = mat_hang.db.ghi_chu;
        this.record.list_mat_hang[pos].db.vat = mat_hang.db.vat;
        const thanh_tien = this.record.list_mat_hang[pos].db.don_gia * this.record.list_mat_hang[pos].db.so_luong * (1 + vat_mat_hang / 100);
        this.record.list_mat_hang[pos].db.thanh_tien = Math.round(thanh_tien);
        this._changeDetectorRef.markForCheck();
        this.generate_total_mat_hang();
    }
    public generate_total_mat_hang(): void {
        this.record.db.tong_thanh_tien = this.record.list_mat_hang.reduce((prev, next) => {
            const donGia = Number(next.db.don_gia) || 0;
            const soLuong = Number(next.db.so_luong) || 0;
            const vat = Number(next.db.vat) || 0; 
            const thanhTien = (donGia * soLuong) * (1 + vat / 100);

        return prev + thanhTien;
        }, 0);
    }
    get_list_vat() {
        this.list_vat = list_vat;
    }
    get_list_don_vi_tinh(): void {
        this.http.post('/sys_don_vi_tinh.ctr/getListUse/', {
        }).subscribe(resp => {
            this.list_don_vi_tinh = resp;
        })
    }
    getElementById(): void {
        this.showLoading("", "", true),
            this.http
                .post('/sys_don_hang_ban.ctr/getElementById/',
                    {
                        id: this.record.db.id ?? "",
                    }
                ).subscribe(resp => {

                    var data: any = resp;
                    this.record = data;
                });
        Swal.close();
    }
    
    delete_mat_hang(pos, ten_mat_hang): void {
        Swal.fire({
            html: `<div>
            <div><span class="mb-0 text-4xl font-bold">Bạn có chắc muốn xóa mặt hàng ${ten_mat_hang}</span><span class="mb-0 text-4xl font-bold"> không?</span></div>
            </div>`,
            icon: 'warning',
            showCancelButton: true,
            confirmButtonColor: '#3085d6',
            cancelButtonColor: '#d33',
            cancelButtonText: 'Không',
            confirmButtonText: 'Xóa mặt hàng'
        }).then((result) => {
            if (result.isConfirmed) {
                this.record.list_mat_hang.splice(pos, 1);
                this.generate_total_mat_hang();
            }
        })
    }
    save(first_submit): void {
        this.beforesave();
        this.loading = true;
        if (this.actionEnum == 1) {
            this.http
                .post(this.controller + '.ctr/create/',
                    {
                        data: this.record,
                    }
                ).subscribe(resp => {
                    this.record = resp as sys_don_hang_ban_model;
                    this.Oldrecord = this.record;
                    // this.basedialogRef.close(this.record);
                    Swal.fire('Lưu thành công', '', 'success');
                    this.aftersave();
                    this.actionEnum = 3;
                    this.loading = false;
                    this.errorModel = []
                },
                    error => {
                        debugger
                        if (error.status == 400) {
                            this.errorModel = error.error;
                            this.aftersavefail();
                            if (first_submit == true) {

                            } else {
                                Swal.fire(this._translocoService.translate('thong_tin_nhap_lieu_khong_dung_vui_long_kiem_tra_lai'), "", "warning");
                            }


                        }
                        if (error.status == 403) {
                            this.basedialogRef.close();
                            Swal.fire(this._translocoService.translate('no_permission'), "", "warning");
                        }
                        this.loading = false;

                    }
                );
        } else if (this.actionEnum == 2) {
            Swal.fire({
                title: this._translocoService.translate('neu_ban_cap_nhat_thong_tin_khach_hang'),
                text: "",
                icon: 'warning',
                showCancelButton: true,
                width:'600px',
                confirmButtonColor: '#3085d6',
                cancelButtonColor: '#d33',
                confirmButtonText: this._translocoService.translate('yes'),
                cancelButtonText: this._translocoService.translate('no')
            }).then((result) => {
                if (result.value) {
                    this.http
                .post(this.controller + '.ctr/edit/',
                    {
                        data: this.record,
                    }
                ).subscribe(resp => {
                    this.record = resp as sys_don_hang_ban_model;
                    this.Oldrecord = this.record;
                    // this.basedialogRef.close(this.record);
                    Swal.fire('Lưu thành công', '', 'success');
                    this.aftersave();
                    this.actionEnum = 3;
                    this.loading = false;
                    this.errorModel = []
                },
                    error => {
                        if (error.status == 400) {
                            this.errorModel = error.error;
                            this.aftersavefail();
                        }
                        if (error.status == 403) {
                            this.basedialogRef.close();
                            Swal.fire(this._translocoService.translate('no_permission'), "", "warning");
                        }
                        this.loading = false;

                    });
                } else {
                    this.loading = false;
                }
            });
        } else if (this.actionEnum == 4) {
            this.http
                .post(this.controller + '.ctr/copy/',
                    {
                        data: this.record,
                    }
                ).subscribe(resp => {
                    this.record = resp as sys_don_hang_ban_model;
                    this.basedialogRef.close(this.record);
                    this.aftersave();
                },
                    error => {
                        if (error.status == 400) {
                            this.errorModel = error.error;

                        }
                        if (error.status == 403) {
                            Swal.fire(this._translocoService.translate('no_permission'), "", "warning");
                        }
                        this.loading = false;

                    });
        }
    }
}
