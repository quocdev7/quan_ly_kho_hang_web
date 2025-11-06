import { Component, Inject } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef, MatDialog } from '@angular/material/dialog';


import { HttpClient, HttpEventType } from '@angular/common/http';
import { BasePopUpAddComponent } from 'app/Basecomponent/BasePopupAdd.component';
import { TranslocoService } from '@ngneat/transloco';
import { FuseNavigationService } from '@fuse/components/navigation';
import { ActivatedRoute } from '@angular/router';
import { BasePopUpAddTypeComponent } from 'app/Basecomponent/BasePopupAddType.component';
import { sys_don_vi_tinh_model } from './sys_don_vi_tinh.types';
import { isThisSecond } from 'date-fns';


@Component({
    selector: 'sys_don_vi_tinh_popupAdd',
    templateUrl: 'popupAdd.html',
    styleUrls: ['./popupAdd.component.scss']
})
export class sys_don_vi_tinh_popUpAddComponent extends BasePopUpAddTypeComponent<sys_don_vi_tinh_model> {
    public file_logo: any;
    public Progress_logo: any = -1;
    public group_field: any;
    constructor(public dialogRef: MatDialogRef<sys_don_vi_tinh_popUpAddComponent>,
        http: HttpClient, _translocoService: TranslocoService,
        _fuseNavigationService: FuseNavigationService, route: ActivatedRoute,
        @Inject('BASE_URL') baseUrl: string,
        public dialogModal: MatDialog,
        @Inject(MAT_DIALOG_DATA) data: any) {
        super(_translocoService, _fuseNavigationService, route, baseUrl, http, 'sys_don_vi_tinh', dialogRef, dialogModal);
        this.record = data;
        this.Oldrecord = JSON.parse(JSON.stringify(data));
        this.actionEnum = data.actionEnum;
        if (this.actionEnum == 1) {
            this.baseInitData();
            this.get_code();
        }

    }
    get_code() {
        this.http
             .post('/sys_don_vi_tinh.ctr/get_code/', {
             }
             ).subscribe(resp => {
                this.record.db.ma = "Tự động tạo";
             });
   
    }
    chose_file_logo(fileInput: any) {

        this.file_logo = fileInput.target.files;
        fileInput.target.value = null;
    }



}
