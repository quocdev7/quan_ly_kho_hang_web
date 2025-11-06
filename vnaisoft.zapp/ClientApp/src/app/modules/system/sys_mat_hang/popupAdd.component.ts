import { Component, Inject } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef, MatDialog } from '@angular/material/dialog';


import { HttpClient } from '@angular/common/http';
import { BasePopUpAddComponent } from 'app/Basecomponent/BasePopupAdd.component';
import { TranslocoService } from '@ngneat/transloco';
import { FuseNavigationService } from '@fuse/components/navigation';
import { ActivatedRoute } from '@angular/router';
import { BasePopUpAddTypeComponent } from 'app/Basecomponent/BasePopupAddType.component';
import { sys_mat_hang_model } from './sys_mat_hang.types';
//import * as data from "assets/jsonfile/config.json";
//import { erp_nhan_hieu_popUpAddComponent } from '../erp_nhan_hieu/popupAdd.component';
import { sys_loai_mat_hang_popUpAddComponent } from '../sys_loai_mat_hang/popupAdd.component';
import { list_tien_te, list_vat, } from 'app/core/data/data';
import { sys_don_vi_tinh_popUpAddComponent } from '../sys_don_vi_tinh/popupAdd.component';
import { sys_mat_hang_popupChooseDacTinhComponent } from './popupChooseDacTinh.component';
import Swal from 'sweetalert2';

@Component({
    selector: 'sys_mat_hang_popupAdd',
    templateUrl: 'popupAdd.html',
})
export class sys_mat_hang_popUpAddComponent extends BasePopUpAddTypeComponent<sys_mat_hang_model> {

    public list_status_del: any;
    public list_don_gia: any
    public list_vat: any;
    public list_don_vi_tinh: any;
    public list_loai_mat_hang: any;
    public list_phan_loai_mat_hang: any;
    public list_tien_te: any;
    public list_type: any;
    constructor(public dialogRef: MatDialogRef<sys_mat_hang_popUpAddComponent>,
        http: HttpClient, _translocoService: TranslocoService,
        _fuseNavigationService: FuseNavigationService, route: ActivatedRoute,
        @Inject('BASE_URL') baseUrl: string,
        public dialogModal: MatDialog,
        @Inject(MAT_DIALOG_DATA) data: any) {
        super(_translocoService, _fuseNavigationService, route, baseUrl, http, 'sys_mat_hang', dialogRef, dialogModal);

        this.record = data;
        this.Oldrecord = JSON.parse(JSON.stringify(data));
        this.actionEnum = data.actionEnum;
        if (this.actionEnum == 1) {
            this.baseInitData();
            this.get_code();
        }
        if (this.actionEnum != 1) {

            this.getElementById();
        }
        this.get_list_loai_mat_hang();
        this.get_list_tien_te();
        this.list_type = [
            { id: 1, name: 'Numeric' },
            { id: 2, name: 'String' },
            { id: 3, name: 'Datetime' },
            { id: 4, name: 'Select' },
            { id: 5, name: 'Mutiple Select' },
        ]
    }

    getElementById() {
        this.http.post("sys_mat_hang.ctr/getElementById", {
            id: this.record.db.id
        }).subscribe(resp => {
            var model: any
            model = resp;
            this.record = model
        })
    }
    get_list_loai_mat_hang(): void {
        this.http
            .post('/sys_loai_mat_hang.ctr/getListUse/',
                {}
            ).subscribe(resp => {
                this.list_loai_mat_hang = resp;
            });
    }
    get_list_don_vi_tinh(): void {
        this.http
            .post('/sys_don_vi_tinh.ctr/getListUse/',
                {}
            ).subscribe(resp => {
                this.list_don_vi_tinh = resp;
            });
    }
    get_list_don_gia(): void {
        this.list_don_gia = [
            {
                id: 1,
                name: this._translocoService.translate('system.don_gia_co_dinh')
            },
            {
                id: 2,
                name: this._translocoService.translate('system.don_gia_gan_nhat')
            }
        ];
    }
    get_list_tien_te(): void {
        this.list_tien_te = list_tien_te
        this.record.db.tien_te = "VND"
    }
    get_code() {
        this.http
            .post('/sys_mat_hang.ctr/get_code/', {
            }
            ).subscribe(resp => {
                this.record.db.ma = resp as any;
            });
    }
    openDialogAddLoaiMatHang(): void {
        const dialogRef = this.dialogModal.open(sys_loai_mat_hang_popUpAddComponent, {
            disableClose: true,
            autoFocus: false,
            width: '768px',
            data: {
                actionEnum: 1,
                db: {
                    id: 0,
                },
            },
        });
        dialogRef.afterClosed().subscribe(result => {
            if (result.db.id == 0) return;
            this.get_list_loai_mat_hang();
            this.record.db.id_loai_mat_hang = result.db.id
        });
    }
    openDialogAddDonViTinh(): void {
        const dialogRef = this.dialogModal.open(sys_don_vi_tinh_popUpAddComponent, {
            disableClose: true,
            autoFocus: false,
            width: '768px',
            data: {
                actionEnum: 1,
                db: {
                    id: 0,
                },
            },
        });
        dialogRef.afterClosed().subscribe(result => {
            if (result.db.id == 0) return;
            this.get_list_don_vi_tinh();
            this.record.db.id_don_vi_tinh = result.db.id
        });
    }
    ngOnInit(): void {
        this.list_vat = list_vat;
        this.get_list_loai_mat_hang();
        this.get_list_don_gia();
        this.get_list_don_vi_tinh();
    }
}
