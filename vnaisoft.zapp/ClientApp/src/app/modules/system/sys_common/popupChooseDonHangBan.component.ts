import { Component, Inject } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef, MatDialog } from '@angular/material/dialog';


import { HttpClient } from '@angular/common/http';
import { BasePopUpAddComponent } from 'app/Basecomponent/BasePopupAdd.component';
import { TranslocoService } from '@ngneat/transloco';
import { FuseNavigationService } from '@fuse/components/navigation';
import { ActivatedRoute } from '@angular/router';

import { BasePopupDatatabbleComponent } from 'app/Basecomponent/BasePopupDatatabble.component';
import { list_thuoc_tinh } from 'app/core/data/data';


@Component({
    selector: 'sys_common_popupChooseDonHangBan',
    templateUrl: 'popupChooseDonHangBan.component.html',
})
export class sys_common_popupChooseDonHangBanComponent extends BasePopupDatatabbleComponent {
    public check_all: any;
    public actionEnum: any;
    public loading: any;
    public list_choose: any;
    public record: any;
    public ignore_ids: any;
    public list_hinh_thuc_van_chuyen: any = [];
    public list_don_hang: any = [];
    constructor(
        _translocoService: TranslocoService
        , _fuseNavigationService: FuseNavigationService
        , route: ActivatedRoute
        , @Inject('BASE_URL') baseUrl: string
        , http: HttpClient

        , public dialogRef: MatDialogRef<MatDialogRef<sys_common_popupChooseDonHangBanComponent>>
        , dialogModal: MatDialog

        , @Inject(MAT_DIALOG_DATA) data: any
    ) {
        super(_translocoService, _fuseNavigationService, route, baseUrl, http, "sys_don_hang_ban", dialogRef, dialogModal,
            {
                search: "", status_del: "1", id_cong_ty: "-1", tu_ngay: "", den_ngay: "", id_doi_tuong: -1, id_kieu_ban: -1, id_hinh_thuc_van_chuyen: -1, id_loai_giao_dich: 1, open: false, is_xuat_kho: null, tinh_trang_don_hang: "2"
                , is_nhap_du: "-1", is_xuat_du: "0"
            }
        )

        this.record = data;
        this.filter.nguon = this.record.db.nguon;
        //this.actionEnum = data.actionEnum;  

        this.get_list_hinh_thuc_van_chuyen();
        this.load_date();
    }
    load_date(): void {
        this.filter.tu_ngay = new Date();
        this.filter.tu_ngay.setDate(this.filter.tu_ngay.getDate() - 365);
        this.filter.den_ngay = new Date();
    }
    get_list_hinh_thuc_van_chuyen() {
        this.list_hinh_thuc_van_chuyen = [
            {
                id: -1,
                name: this._translocoService.translate('system.all')
            },
            {
                id: 1,
                name: this._translocoService.translate('system.chua_giao_hang')
            },
            {
                id: 2,
                name: this._translocoService.translate('system.da_giao_hang')
            }
        ];
    }
    filterchange() {
        if (this.filter.open == true) {
            this.filter.open = false;
        } else {
            this.filter.open = true;
        }
    }

    set_one(): boolean {
        if (this.listData == null) {
            return false;
        }
        return this.listData.filter(t => t.is_check).length > 0 && !this.check_all;

    }
    set_all(completed: boolean) {
        this.check_all = completed;
        if (this.listData == null) {
            return;
        }
        this.listData.forEach(t => t.is_check = completed);
    }

    load_list_choose(item): any {

        this.list_don_hang = [];

        let model = item;
        let str = "";
        var don_gia = null;

        var obj = {
            id: model.db.id,
            ma: model.db.ma,
            loai_giao_dich: model.db.loai_giao_dich,
            loai_giao_dich_str: model.loai_giao_dich_str,
            ten_hinh_thuc: model.hinh_thuc_doi_tuong_str,

            id_doi_tuong: model.db.id_doi_tuong,
            hinh_thuc_doi_tuong: model.db.hinh_thuc_doi_tuong,
            dien_thoai: model.db.dien_thoai,
            email: model.db.email,
            ma_so_thue: model.db.ma_so_thue,
            dia_chi_doi_tuong: model.db.dia_chi_doi_tuong,
            ten_doi_tuong: model.db.ten_doi_tuong,
            noi_dung: model.db.ghi_chu,
            thanh_tien_sau_thue: model.db.thanh_tien_sau_thue,
            tong_tien_sau_thue: model.db.tong_tien_sau_thue,
            list_mat_hang: model.list_mat_hang,
            //thuoc_tinh:model.db.thuoc_tinh
        }
        this.list_don_hang.push(obj);


    }

    chon(item) {
        this.load_list_choose(item);
        this.dialogRef.close(this.list_don_hang[0]);
    }
    dong() {

        this.dialogRef.close();

    }
    update_all() {
        this.check_all = this.listData != null && this.listData.every(t => t.is_check);
    }


    ngOnInit(): void {
        this.check_all = false;
        this.baseInitDataOption("DataHandlerDonHangBanHH");


        if (this.record.db.id == undefined || this.record.db.id == "") {

        } else {
            this.listData.filter(t => t.id == this.record.db.id)[0].is_check = true;

            // for (let i = 0; i < this.listData.length; i++) {
            //     let model = listDataN[i];
            //     let str = "";
            //     var don_gia = null;
            //     if (model.is_check == true) {
            //         var obj ={
            //             id:model.db.id,
            //             ma:model.db.ma
            //         } 
            //         this.list_don_hang.push(obj);
            //     }

            // }
        }


    }
}
