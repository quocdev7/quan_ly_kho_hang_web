import { ChangeDetectorRef, Component, HostListener, Inject } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef, MatDialog } from '@angular/material/dialog';


import { HttpClient, HttpEventType } from '@angular/common/http';
import { BasePopUpAddComponent } from 'app/Basecomponent/BasePopupAdd.component';
import { TranslocoService } from '@ngneat/transloco';
import { FuseNavigationService } from '@fuse/components/navigation';
import { ActivatedRoute } from '@angular/router';
import { BasePopUpAddTypeComponent } from 'app/Basecomponent/BasePopupAddType.component';
import { sys_don_hang_mua_model } from './sys_don_hang_mua.types';
import Swal from 'sweetalert2';
import { forEach, isNaN } from 'lodash';

import { listbank, list_vat } from 'app/core/data/data';
import { mode } from 'crypto-js';
//import { sys_common_popupChooseMatHangTonKhoComponent } from '../sys_common/popupChooseMatHangTonKho.component';
//import { sys_common_popupChooseCaNhanToChucComponent } from '../sys_common/popupChooseCaNhanToChuc.component';
import { cm_mau_in_popupComponent } from '@fuse/components/commonComponent/cm_mau_in/cm_mau_in_popup.component';
import { doi_tuong_tu_do } from 'app/core/data/data';
import { sys_common_popupChooseMatHangComponent } from '../sys_common/popupChooseMatHang';
//import { sys_tai_khoan_ngan_hang_popUpAddComponent } from '../sys_tai_khoan_ngan_hang/popupAdd.component';
//import { sys_common_popupChooseMatHangDuocChonLaiComponent } from '../sys_common/popupChooseMatHangDuocChonLai';
@Component({
    selector: 'sys_don_hang_mua_popupAdd',
    templateUrl: 'popupAdd.html',
    // styleUrls: ['./popupAdd.component.scss']
})
export class sys_don_hang_mua_popUpAddComponent extends BasePopUpAddTypeComponent<sys_don_hang_mua_model> {
    public file_logo: any;
    public Progress_logo: any = -1;
    public list_mat_hang: any;
    public mat_hang: any;
    public list_vat: any;
    public gia_tri_vat: any;

    public list_tien_te: any;
    public gia_tri_vat_khac: any;


    public total_so_luong: any;
    public total_thanh_tien_truoc_thue: any;
    public total_so_tien_chiet_khau: any;
    public total_thanh_tien_sau_chiet_khau: any;
    public total_tien_thue: any;
    public ten_hinh_thuc: any;
    public ten_ngan_hang: any = '';
    public total_thanh_tien_sau_thue: any;
    public openTab: any = 1;
    public fileData: any;
    public list_ngan_hang: any;

    public list_don_vi_tinh: any;
    public list_kho: any;
    public list_loai_giao_dich: any;
    public list_hinh_thuc: any;
    public is_doi_tuong: any = false;
    public check_doi_tuong: any = 1;

    public data_doi_tuong: any = {};
    public ma_mat_hang: any;
    public file: any;
    public list_tai_khoan_ngan_hang: any;
    public thanh_tien_sau_thue_van_chuyen: any = 0;
    public thanh_tien_sau_thue_chi_phi_khac: any = 0;
    public list_phuong_thuc_thanh_toan: any;
    constructor(public dialogRef: MatDialogRef<sys_don_hang_mua_popUpAddComponent>,
        http: HttpClient, _translocoService: TranslocoService, private _changeDetectorRef: ChangeDetectorRef,
        _fuseNavigationService: FuseNavigationService, route: ActivatedRoute,
        @Inject('BASE_URL') baseUrl: string,
        private dialog: MatDialog,
        public dialogModal: MatDialog,
        @Inject(MAT_DIALOG_DATA) data: any) {
        super(_translocoService, _fuseNavigationService, route, baseUrl, http, 'sys_don_hang_mua', dialogRef, dialogModal);
        this.record = data;
        this.Oldrecord = JSON.parse(JSON.stringify(data));
        this.actionEnum = data.actionEnum;
        if (this.actionEnum == 1) {
            this.get_code();
            //this.record.db.ten = "Tự động tạo";
            this.record.db.ngay_dat_hang = new Date();
        }
        else{
            this.getElementById();
        }
        this.list_vat = list_vat;
    }
    openDialogEdit(item, pos): void {
        const dialogRef = this.dialog.open(sys_don_hang_mua_popUpAddComponent, {
            disableClose: true,
            autoFocus: false,
            width: '100%',
            data: {
                actionEnum: 2,
                db: {
                    id: item.db.id,
                    //id_doi_tuong: item.db.id_doi_tuong
                },
            }
            //data: model
        });
        dialogRef.afterClosed().subscribe(result => {
            if (result.db.id == 0) return;
            this.close();
        });
    }
    ngOnInit(): void {
        this.baseInitData();
        //this.get_list_don_vi_tinh();
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
            if (timming < 50 || this.combinedCode == "") {
                this.combinedCode += event.key;
            } else {
                this.combinedCode = "";
            }
        }
    }
    load_list_choose(listDataN): any {
        if (listDataN.length == 0) {
        }
        else {
            for (let i = 0; i < listDataN.length; i++) {
                let model = listDataN[i];
                let str = "";
                var don_gia = null;
                don_gia = model.gia_mua;
                let obj_mat_hang: any;
                obj_mat_hang = {
                    db: {
                        id_mat_hang: model.db.id,
                        id_don_vi_tinh: model.db.id_don_vi_tinh,
                        don_gia: don_gia,
                        //so_luong: this.record.so_luong_mh,
                        vat: model.db.vat,
                        chiet_khau: model.db.ty_le_chiet_khau,
                        thanh_tien_truoc_thue: 0,
                        tien_vat: 0,
                        ghi_chu: null,
                        thanh_tien_sau_thue: 0,
                        thanh_tien_chiec_khau: 0,

                    },
                    ma_mat_hang: model.db.ma,
                    ten_mat_hang: model.db.ten,
                    ten_don_vi_tinh: model.ten_don_vi_tinh,
                    ten_thuoc_tinh: model.ten_thuoc_tinh,
                    id_mat_hang: model.db.id,
                    he_so_quy_doi: model.db.he_so_quy_doi,
                }
                //var lengthUpdate = this.record.list_mat_hang.length;
                debugger
                //this.record.db.list_mat_hang.push(obj_mat_hang);
                //this.loadThanhTienSauThueMatHang(lengthUpdate);
            }
        }
    }


    get_code() {
        this.http
            .post('/sys_don_hang_mua.ctr/get_code/', {
            }
            ).subscribe(resp => {
                var data: any;
                data = resp;
                this.record.db.ma = "Tự động tạo";
            });
    }
    getElementById(): void {

        this.showLoading("", "", true)
        this.http
            .post('/sys_don_hang_mua.ctr/getElementById/',
                {
                    id: this.record.db.id,

                }
            ).subscribe(resp => {
                var data: any = resp;
                this.record = data;
            });
            Swal.close();
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
            debugger
            var data: any;
            data = result;
            if (data == undefined) return;
            this.record.list_mat_hang = data;
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
                    this.record = resp as sys_don_hang_mua_model;
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
                title: this._translocoService.translate('neu_ban_cap_nhat_thong_tin_nha_cung_cap'),
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
                        this.record = resp as sys_don_hang_mua_model;
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
                    this.record = resp as sys_don_hang_mua_model;
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
