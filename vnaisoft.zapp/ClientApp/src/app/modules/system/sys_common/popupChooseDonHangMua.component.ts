import { Component, Inject } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef, MatDialog } from '@angular/material/dialog';


import { HttpClient } from '@angular/common/http';
import { BasePopUpAddComponent } from 'app/Basecomponent/BasePopupAdd.component';
import { TranslocoService } from '@ngneat/transloco';
import { FuseNavigationService } from '@fuse/components/navigation';
import { ActivatedRoute } from '@angular/router';

import { BasePopupDatatabbleComponent } from 'app/Basecomponent/BasePopupDatatabble.component';
import { list_thuoc_tinh } from 'app/core/data/data';
import { mode } from 'crypto-js';


@Component({
    selector: 'sys_common_popupChooseDonHangMua',
    templateUrl: 'popupChooseDonHangMua.component.html',
})
export class sys_common_popupChooseDonHangMuaComponent extends BasePopupDatatabbleComponent {
    public check_all: any;
    public actionEnum: any;
    public loading: any;
    public list_choose: any;
    public record: any;
    public ignore_ids: any;
    public list_don_hang: any = [];
    constructor(
        _translocoService: TranslocoService
        , _fuseNavigationService: FuseNavigationService
        , route: ActivatedRoute
        , @Inject('BASE_URL') baseUrl: string
        , http: HttpClient

        , public dialogRef: MatDialogRef<MatDialogRef<sys_common_popupChooseDonHangMuaComponent>>
        , dialogModal: MatDialog

        , @Inject(MAT_DIALOG_DATA) data: any
    ) {
        super(_translocoService, _fuseNavigationService, route, baseUrl, http, "sys_don_hang_mua", dialogRef, dialogModal,
            {
                search: "", status_del: "1", tu_ngay: "", den_ngay: "", nguon: ""
            }
        )

        this.record = data;
        this.filter.nguon = this.record.db.nguon;
        //this.actionEnum = data.actionEnum;  

        this.load_date();
    }
    load_date(): void {
        this.filter.tu_ngay = new Date();
        this.filter.tu_ngay.setDate(this.filter.tu_ngay.getDate() - 365);
        this.filter.den_ngay = new Date();
    }
    load_list_choose(item): any {
        let model = item
        let str = "";
        var don_gia = null;
        var obj = {
            id: model.db.id,
            ma: model.db.ma,
            noi_dung: model.db.ghi_chu,
            thanh_tien_sau_thue: model.db.thanh_tien_sau_thue,
            tong_tien_sau_thue: model.db.tong_tien_sau_thue,
        }
        this.list_don_hang.push(obj);
    }

    chon(item) {
        this.load_list_choose(item);
        this.dialogRef.close(this.list_don_hang[0]);
    }
    dong() {
        if (this.list_don_hang.length == 0) {
            this.dialogRef.close();
        } else {
            this.dialogRef.close(this.list_don_hang[0]);
        }
    }
    update_all() {
        this.check_all = this.listData != null && this.listData.every(t => t.is_check);
    }


    ngOnInit(): void {
        this.check_all = false;
        this.baseInitDataOption("DataHandlerDonHangMua");

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
