import { Component, Inject } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef, MatDialog } from '@angular/material/dialog';


import { HttpClient, HttpEventType } from '@angular/common/http';
import { BasePopUpAddComponent } from 'app/Basecomponent/BasePopupAdd.component';
import { TranslocoService } from '@ngneat/transloco';
import { FuseNavigationService } from '@fuse/components/navigation';
import { ActivatedRoute } from '@angular/router';
import { BasePopUpAddTypeComponent } from 'app/Basecomponent/BasePopupAddType.component';
import { sys_loai_nhap_xuat_model } from './sys_loai_nhap_xuat.types';


@Component({
    selector: 'sys_loai_nhap_xuat_popupAdd',
    templateUrl: 'popupAdd.html',
    styleUrls: ['./popupAdd.component.scss']
})
export class sys_loai_nhap_xuat_popUpAddComponent extends BasePopUpAddTypeComponent<sys_loai_nhap_xuat_model> {
    public list_type: any;
    public list_nguon: any;
    constructor(public dialogRef: MatDialogRef<sys_loai_nhap_xuat_popUpAddComponent>,
        http: HttpClient, _translocoService: TranslocoService,
        _fuseNavigationService: FuseNavigationService, route: ActivatedRoute,
        @Inject('BASE_URL') baseUrl: string,
        public dialogModal: MatDialog,
        @Inject(MAT_DIALOG_DATA) data: any) {
        super(_translocoService, _fuseNavigationService, route, baseUrl, http, 'sys_loai_nhap_xuat', dialogRef, dialogModal);
        this.record = data;
        this.Oldrecord = JSON.parse(JSON.stringify(data));
        this.actionEnum = data.actionEnum;
        if (this.actionEnum == 1) {
            this.baseInitData();
        }
        this.list_type = [
            {
                id: 1,
                name: this._translocoService.translate('system.nhap')
            },
            {
                id: 2,
                name: this._translocoService.translate('system.xuat')
            }
        ];
        this.list_nguon = [
            {
                id: "1",
                name: this._translocoService.translate('erp.don_hang_ban')
            },
            {
                id: "2",
                name: this._translocoService.translate('erp.don_hang_mua')
            }, {
                id: "3",
                name: this._translocoService.translate('erp.chuyen_kho')
            }
        ];
    }
}
