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
                this.add_mat_hang()
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
    // set_phuong_thuc_thanh_toan() {
    //     debugger
    //     if (this.actionEnum != 3) {
    //         //this.record.ten_ngan_hang = null;
    //         this.record.db.id_tai_khoan_ngan_hang = null;
    //     }
    // };
    // clear_doi_tuong() {
    //     this.record.db.hinh_thuc_doi_tuong = null;
    //     this.record.db.id_doi_tuong = null;
    //     this.record.db.ma_so_thue = null;
    //     this.record.db.dien_thoai = null;
    //     this.record.db.email = null;
    //     this.record.db.dia_chi_doi_tuong = null;
    //     this.record.db.ten_doi_tuong = null;
    //     this.record.db.so_tai_khoan_doi_tuong = null;
    //     this.record.db.id_ngan_hang_doi_tuong = null;
    // };
    // set_doi_tuong(v) {
    //     if (this.actionEnum == 1) {
    //         this.record.check_doi_tuong = v;
    //         if (this.record.check_doi_tuong == 1) {
    //             this.record.db.hinh_thuc_doi_tuong = doi_tuong_tu_do.hinh_thuc;
    //             this.record.db.id_doi_tuong = doi_tuong_tu_do.id;
    //             this.record.db.ma_so_thue = doi_tuong_tu_do.ma_so_thue;
    //             this.record.db.dien_thoai = doi_tuong_tu_do.dien_thoai;
    //             this.record.db.email = doi_tuong_tu_do.email;
    //             this.record.db.dia_chi_doi_tuong = doi_tuong_tu_do.dia_chi;
    //             this.record.db.ten_doi_tuong = doi_tuong_tu_do.ten;
    //             this.record.db.so_tai_khoan_doi_tuong = null;
    //             this.record.db.id_ngan_hang_doi_tuong = null;

    //         } else {
    //             this.clear_doi_tuong();

    //         }
    //     }
    // }
    // openDialogPrint(item): void {

    //     this.http
    //         .post(this.controller + '.ctr/getPrint/', {
    //             id: item.db.id
    //         }
    //         ).subscribe(resp => {
    //             var data: any;
    //             data = resp;
    //             const dialogRef = this.dialogModal.open(cm_mau_in_popupComponent, {
    //                 width: '878px',
    //                 disableClose: true,
    //                 data: {
    //                     tieu_de: data.tieu_de,
    //                     noi_dung: data.noi_dung,
    //                 },
    //             });
    //             dialogRef.afterClosed().subscribe(result => {
    //                 if (result != undefined && result != null) {
    //                 }
    //             });
    //         });
    // }
    // get_list_phuong_thuc_thanh_toan() {
    //     this.list_phuong_thuc_thanh_toan = [
    //         {
    //             id: 1,
    //             name: this._translocoService.translate('system.tien_mat')
    //         },
    //         {
    //             id: 2,
    //             name: this._translocoService.translate('system.chuyen_khoan')
    //         }
    //         // {
    //         //     id: 3,
    //         //     name: this._translocoService.translate('system.vi_dien_tu')
    //         // }
    //     ];

    // }
    // load_ngay_du_kien() {
    //     var so_ngay_du_kien_giao = this.record.db.so_ngay_du_kien;
    //     var date = new Date(this.record.db.ngay_dat_hang);
    //     var ngay_du_kien_giao = date.setDate(date.getDate() + Number(so_ngay_du_kien_giao));
    //     var ngay_du_kien_giao_hang = new Date(ngay_du_kien_giao);
    //     this.record.db.ngay_du_kien_nhan_hang = ngay_du_kien_giao_hang;
    // }
    // get_list_ngan_hang() {
    //     this.http.post("sys_tai_khoan_ngan_hang.ctr/getListUse", {}).subscribe(resp => {
    //         this.list_ngan_hang = resp
    //     })
    // }
    // change_ma_ngan_hang() {
    //     var bank = this.list_ngan_hang.filter(q => q.id == this.record.db.id_tai_khoan_ngan_hang)[0]
    //     this.record.db.ma_ngan_hang = bank.ma_ngan_hang;
    //     this.record.db.so_tai_khoan = bank.so_tai_khoan;
    // }
    
    // resetDonHang(): void {
    //     var loai_giao_dich = this.record.db.loai_giao_dich;
    //     var ma = this.record.db.ma;
    //     var ten = this.record.db.ten;
    //     this.record = this.Oldrecord;
    //     this.record.db.ma = ma;
    //     this.record.db.ten = ten;
    //     if (this.record.db.tien_van_chuyen == null) {
    //         this.record.chi_phi_van_chuyen = 0;
    //     }
    //     this.record.db.phuong_thuc_thanh_toan = 2;
    //     this.record.list_mat_hang = [];
    //     this.record.db.loai_giao_dich = loai_giao_dich;
    //     this.set_doi_tuong(1);
    // }
    // openDialogChooseDoiTuong(id): void {

    //     const dialogRef = this.dialogModal
    //         .open(sys_common_popupChooseCaNhanToChucComponent, {
    //             disableClose: true,
    //             width: '100%',
    //             height: '100%',
    //             data: {
    //                 actionEnum: 2, //Không cho phép thêm đối tượng
    //                 title: "Nhà cung cấp",
    //                 db: {
    //                     id: id
    //                 },
    //             }
    //         });
    //     dialogRef.afterClosed().subscribe(result => {

    //         var data: any;
    //         data = result;
    //         if (data == undefined) return;
    //         this.record.db.hinh_thuc_doi_tuong = data.hinh_thuc_doi_tuong;
    //         this.record.db.id_doi_tuong = data.id_doi_tuong;
    //         this.record.db.ma_so_thue = data.ma_so_thue;
    //         this.record.db.dien_thoai = data.dien_thoai;
    //         this.record.db.email = data.email;
    //         this.record.db.dia_chi_doi_tuong = data.dia_chi_doi_tuong;
    //         this.record.db.ten_doi_tuong = data.ten_doi_tuong;
    //         this.record.db.id_ngan_hang_doi_tuong = data.id_ngan_hang;
    //         this.record.db.so_tai_khoan_doi_tuong = data.so_tai_khoan;
    //         this.ten_hinh_thuc = data.ten_hinh_thuc;
    //         this.record.ten_ngan_hang_doi_tuong = data.ten_ngan_hang;
    //         // var list_ngan_hang = this.list_ngan_hang.filter(q=>q.ma_ngan_hang == data.id_ngan_hang).map(q=>q.id);
    //         // this.record.db.id_tai_khoan_ngan_hang = list_ngan_hang[0];
    //     })
    // }
    add_mat_hang() {
        var ma_mat_hang = this.ma_mat_hang + "";
        this.ma_mat_hang = "";
        // if (this.record.so_luong_mh <= 0 || this.record.so_luong_mh == null) {
        //     Swal.fire("Số lượng phải lớn hơn 0", "", "warning");
        // }
        // else {
        //     this.http
        //         .post('/sys_mat_hang.ctr/add_mat_hang_da_chon/', {
        //             ma: ma_mat_hang.trim(),
        //             id_doi_tuong: this.record.db.id_doi_tuong ?? "",
        //             loai_giao_dich: this.record.db.loai_giao_dich,
        //             list_mat_hang: this.record.list_mat_hang
        //         }
        //         ).subscribe(resp => {
        //             var data: any;
        //             data = resp;


        //             if (data.list_mat_hang.length == 0) {
        //                 if (data.result == 1) {
        //                     Swal.fire("mã " + ma_mat_hang + ", " + this._translocoService.translate('erp.mat_hang_khong_ton_tai_trong_he_thong'), "", "warning");
        //                 }
        //                 else if (data.result == 2) {
        //                     Swal.fire(this._translocoService.translate('erp.mat_hang_phai_la_hang_hoa'), "", "warning");
        //                 } else if (data.result == 3) {
        //                     Swal.fire(this._translocoService.translate('erp.mat_hang_phai_la_dich_vu'), "", "warning");
        //                 } else {
        //                     Swal.fire(this._translocoService.translate('erp.mat_hang_da_ngung_su_dung'), "", "warning");
        //                 }
        //             } else {
        //                 this.load_list_choose(data.list_mat_hang);
        //             }
        //         });
        // }
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
    // get_hinh_thuc_doi_tuong() {
    //     this.list_hinh_thuc = [
    //         {
    //             id: 1,
    //             name: this._translocoService.translate('system.ca_nhan')
    //         },
    //         {
    //             id: 2,
    //             name: this._translocoService.translate('system.to_chuc')
    //         }
    //     ];
    // }
    // get_list_kho() {
    //     this.http.post('/sys_kho.ctr/getListUse/', {

    //     }).subscribe(resp => {
    //         this.list_kho = resp;

    //     })
    // }
    // get_loai_giao_dich() {
    //     this.list_loai_giao_dich = [
    //         {
    //             id: 1,
    //             name: this._translocoService.translate('system.hang_hoa')
    //         },
    //         {
    //             id: 2,
    //             name: this._translocoService.translate('system.dich_vu')
    //         }
    //     ];
    // }
    // get_list_vat() {

    //     this.list_vat = list_vat;

    // }
    // get_list_don_vi_tinh(): void {
    //     this.http.post('/sys_don_vi_tinh.ctr/getListUse/', {

    //     }).subscribe(resp => {
    //         this.list_don_vi_tinh = resp;

    //     })
    // }
    toggleTabs($tabNumber: number) {
        this.openTab = $tabNumber;


        if (this.openTab == 2 && this.record.db.id == "0") {
            Swal.fire("erp.ban_phai_tao_don_hang_truoc_khi_upload_file", "", "warning")
            this.openTab == 1;
        }
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
                //this.thanh_tien_sau_thue_van_chuyen = Number(this.record.db.tien_van_chuyen) + Number(this.record.db.tien_vat_van_chuyen);
                // this.record.check_doi_tuong = doi_tuong_tu_do.id == this.record.db.id_doi_tuong ? 1 : 2;
                // this.record.chi_phi_van_chuyen = this.record.db.tien_van_chuyen;
                // this.record.db.ly_do_chinh_sua = ""
                
                if(this.actionEnum == 2)
                {
                    //this.baseInitData();
                }
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
    // load_thanh_tien_sau_thue_van_chuyen() {
    //     this.gia_tri_vat = this.list_vat.filter(q => q.id == this.record.db.vat_van_chuyen)[0].value;
    //     if (this.gia_tri_vat > 100) {
    //         this.gia_tri_vat = 100;
    //     } else if (this.gia_tri_vat < 0 || this.gia_tri_vat == null || this.gia_tri_vat == undefined) {
    //         this.gia_tri_vat = 0;
    //     }
    //     this.record.db.tien_vat_van_chuyen = (Number(this.record.db.tien_van_chuyen) * (Number(this.gia_tri_vat)) / 100);
    //     this.thanh_tien_sau_thue_van_chuyen = Number(this.record.db.tien_van_chuyen) + Number(this.record.db.tien_vat_van_chuyen);
    //     this.record.chi_phi_van_chuyen = this.record.db.tien_van_chuyen;
    //     this.generate_total_mat_hang();
    // }
    // load_thanh_tien_sau_thue_chi_phi_khac(): void {

    //     this.gia_tri_vat_khac = this.list_vat.filter(q => q.id == this.record.db.vat_khac)[0].value;
    //     if (this.gia_tri_vat_khac > 100) {
    //         this.gia_tri_vat_khac = 100;
    //     } else if (this.gia_tri_vat_khac < 0 || this.gia_tri_vat_khac == null || this.gia_tri_vat_khac == undefined) {
    //         this.gia_tri_vat_khac = 0;
    //     }
    //     this.record.db.tien_vat_khac = (Number(this.record.db.tien_khac) * Number(this.gia_tri_vat_khac)) / 100
    //     this.thanh_tien_sau_thue_chi_phi_khac = Number(this.record.db.tien_khac) + Number(this.record.db.tien_vat_khac);
    //     this.record.db.thanh_tien_sau_thue = Number(this.thanh_tien_sau_thue_van_chuyen) + Number(this.thanh_tien_sau_thue_chi_phi_khac);
    // }
    // loadThanhTienSauThueMatHangPopup(): void {

    //     for (var i = 0; i < this.record.list_mat_hang.length; i++) {
    //         var pos = i;
    //         var mat_hang = this.record.list_mat_hang[pos];
    //         var vat_mat_hang = this.list_vat.filter(q => q.id == mat_hang.db.vat)[0].value;
    //         if (vat_mat_hang == undefined || vat_mat_hang == null) {
    //             vat_mat_hang = 0;
    //         }
    //         mat_hang.db.thanh_tien_truoc_thue = Math.round(Number(mat_hang.db.so_luong ?? 0) * Number(mat_hang.db.don_gia));
    //         if (isNaN(mat_hang.db.thanh_tien_truoc_thue)) {
    //             mat_hang.db.thanh_tien_truoc_thue = 0;
    //         }
    //         ;
    //         // gan gia tri cho list mat hang ban
    //         this.record.list_mat_hang[pos].db.don_gia = mat_hang.db.don_gia ?? null;
    //         this.record.list_mat_hang[pos].db.so_luong = Number(mat_hang.db.so_luong ?? 0);
    //         this.record.list_mat_hang[pos].so_luong_quy_doi = Number(mat_hang.he_so_quy_doi ?? 0) * Number(mat_hang.db.so_luong ?? 0);
    //         this.record.list_mat_hang[pos].don_gia_quy_doi = Number(mat_hang.db.thanh_tien_truoc_thue ?? 0) / Number(this.record.list_mat_hang[pos].so_luong_quy_doi ?? 0);
    //         this.record.list_mat_hang[pos].db.thanh_tien_truoc_thue = Math.round(Number(mat_hang.db.thanh_tien_truoc_thue));
    //         this.record.list_mat_hang[pos].db.chiet_khau = Number(mat_hang.db.chiet_khau);
    //         this.record.list_mat_hang[pos].db.thanh_tien_chiet_khau = Math.round(this.record.list_mat_hang[pos].db.thanh_tien_truoc_thue - (this.record.list_mat_hang[pos].db.thanh_tien_truoc_thue * Number(mat_hang.db.chiet_khau / 100)));

    //         this.record.list_mat_hang[pos].db.tien_vat = Math.round(Number(mat_hang.db.thanh_tien_chiet_khau) * Number(vat_mat_hang / 100));
    //         this.record.list_mat_hang[pos].db.thanh_tien_sau_thue = Math.round(Number(this.record.list_mat_hang[pos].db.thanh_tien_chiet_khau) + Number(mat_hang.db.tien_vat));
    //         this.record.list_mat_hang[pos].db.ghi_chu = mat_hang.db.ghi_chu;
    //         this.record.list_mat_hang[pos].db.vat = mat_hang.db.vat;
    //         this._changeDetectorRef.markForCheck();
    //     }


    //     this.generate_total_mat_hang();
    // }
    loadThanhTienSauThueMatHang(pos): void {
        debugger
        var mat_hang = this.record.list_mat_hang[pos];
        var vat_mat_hang = this.list_vat.filter(q => q.id == mat_hang.vat)[0].value;
        if (vat_mat_hang == undefined || vat_mat_hang == null) {
            vat_mat_hang = 0;
        }
        var don_gia = mat_hang.don_gia ?? null;
        this.record.list_mat_hang[pos].don_gia = Number(don_gia);
        this.record.list_mat_hang[pos].so_luong = Number(mat_hang.so_luong ?? 0);
        // this.record.list_mat_hang[pos].db.tien_vat = Math.round(tien_vat);
        this.record.list_mat_hang[pos].ghi_chu = mat_hang.ghi_chu;
        this.record.list_mat_hang[pos].vat = mat_hang.vat;
        const thanh_tien = this.record.list_mat_hang[pos].don_gia * this.record.list_mat_hang[pos].so_luong * (1 + vat_mat_hang / 100);
        this.record.list_mat_hang[pos].thanh_tien = Math.round(thanh_tien);
        this._changeDetectorRef.markForCheck();
        this.generate_total_mat_hang();
    }
    public generate_total_mat_hang(): void {
        this.record.db.tong_thanh_tien = this.record.list_mat_hang.reduce((prev, next) => {
            const donGia = Number(next.don_gia) || 0;
            const soLuong = Number(next.so_luong) || 0;
            const vat = Number(next.vat) || 0; 
            const thanhTien = (donGia * soLuong) * (1 + vat / 100);

        return prev + thanhTien;
        }, 0);
        //this.record.list_mat_hang.reduce((prev, next) => prev + Number(next.don_gia), 0) + Number(this.record.db.tien_van_chuyen);
    }
    // loadThanhTienSauThueMatHangChangeTienThue(pos): void {
    //     var mat_hang = this.record.list_mat_hang[pos];
    //     var vat_mat_hang = this.list_vat.filter(q => q.id == mat_hang.db.vat)[0].value;
    //     if (vat_mat_hang == undefined || vat_mat_hang == null) {
    //         vat_mat_hang = 0;
    //     }
    //     mat_hang.db.thanh_tien_truoc_thue = Math.round(Number(mat_hang.db.so_luong ?? 0) * Number(mat_hang.db.don_gia));
    //     if (isNaN(mat_hang.db.thanh_tien_truoc_thue)) {
    //         mat_hang.db.thanh_tien_truoc_thue = 0;
    //     }
    //     ;
    //     // gan gia tri cho list mat hang ban
    //     var don_gia = mat_hang.db.don_gia ?? null;
    //     this.record.list_mat_hang[pos].db.don_gia = Number(don_gia);
    //     this.record.list_mat_hang[pos].db.so_luong = Number(mat_hang.db.so_luong ?? 0);
    //     this.record.list_mat_hang[pos].db.thanh_tien_truoc_thue = Math.round(Number(mat_hang.db.thanh_tien_truoc_thue));
    //     this.record.list_mat_hang[pos].db.chiet_khau = Number(mat_hang.db.chiet_khau);
    //     // var thanh_tien_chiet_khau = this.record.list_mat_hang[pos].db.thanh_tien_truoc_thue - (this.record.list_mat_hang[pos].db.thanh_tien_truoc_thue * Number(mat_hang.db.chiet_khau / 100))
    //     var thanh_tien_chiet_khau = mat_hang.db.thanh_tien_truoc_thue - (mat_hang.db.thanh_tien_truoc_thue * Number(mat_hang.db.chiet_khau / 100));
    //     this.record.list_mat_hang[pos].db.thanh_tien_chiet_khau = Math.round(thanh_tien_chiet_khau);
    //     // var tien_vat = Number(mat_hang.db.thanh_tien_chiet_khau) * Number(vat_mat_hang / 100)
    //     // this.record.list_mat_hang[pos].db.tien_vat = Math.round(tien_vat);
    //     var thanh_tien_sau_thue = Number(this.record.list_mat_hang[pos].db.thanh_tien_chiet_khau) + Number(mat_hang.db.tien_vat)
    //     this.record.list_mat_hang[pos].db.thanh_tien_sau_thue = Math.round(thanh_tien_sau_thue);
    //     this.record.list_mat_hang[pos].db.ghi_chu = mat_hang.db.ghi_chu;
    //     this.record.list_mat_hang[pos].db.vat = mat_hang.db.vat;
    //     this._changeDetectorRef.markForCheck();
    //     this.generate_total_mat_hang();
    // }
    

    // public sum_so_luong(arr): any {
    //     var value = 0;
    //     if (arr == undefined) {

    //     } else {
    //         if (arr.length > 0) {
    //             for (var i = 0; i < arr.length; i++) {
    //                 if (arr[i].db.so_luong == null || arr[i].db.so_luong == '') {
    //                     arr[i].db.so_luong = 0;
    //                 }
    //                 value += parseFloat(arr[i].db.so_luong);
    //             }
    //         }
    //     }

    //     return value;

    // };
    // public sum_thanh_tien_truoc_thue(arr): any {
    //     var value = 0;
    //     if (arr == undefined) {
    //     } else {
    //         if (arr.length > 0) {
    //             for (var i = 0; i < arr.length; i++) {
    //                 value += parseFloat(arr[i].db.thanh_tien_truoc_thue);
    //             }
    //         }
    //     }
    //     return value;
    // };
    // public sum_tien_thue(arr): any {
    //     var value = 0;
    //     if (arr == undefined) {
    //     } else {
    //         if (arr.length > 0) {
    //             for (var i = 0; i < arr.length; i++) {
    //                 value += parseFloat(arr[i].db.tien_thue);
    //             }
    //         }
    //     }
    //     return value;
    // };
    // public sum_thanh_tien_sau_thue(arr): any {
    //     var value = 0;
    //     if (arr == undefined) {
    //     } else {
    //         if (arr.length > 0) {
    //             for (var i = 0; i < arr.length; i++) {
    //                 value += parseFloat(arr[i].db.thanh_tien_sau_thue);
    //             }
    //         }
    //     }
    //     return value;
    // };
    // delete_mat_hang(pos, ten_mat_hang): void {
    //     Swal.fire({
    //         html: `<div>
    //         <div><span class="mb-0 text-4xl font-bold">Bạn có chắc muốn xóa mặt hàng ${ten_mat_hang}</span><span class="mb-0 text-4xl font-bold"> không?</span></div>
    //         </div>`,
    //         icon: 'warning',
    //         showCancelButton: true,
    //         confirmButtonColor: '#3085d6',
    //         cancelButtonColor: '#d33',
    //         cancelButtonText: 'Không',
    //         confirmButtonText: 'Xóa mặt hàng'
    //     }).then((result) => {
    //         if (result.isConfirmed) {
    //             this.record.db.list_mat_hang.splice(pos, 1);
    //             this.generate_total_mat_hang();
    //         }
    //     })
    // }
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
