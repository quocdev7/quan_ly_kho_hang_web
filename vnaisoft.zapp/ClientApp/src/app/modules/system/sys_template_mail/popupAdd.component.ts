import { Component, Inject } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef, MatDialog } from '@angular/material/dialog';


import { HttpClient } from '@angular/common/http';

import { TranslocoService } from '@ngneat/transloco';
import { BasePopUpAddComponent } from 'app/Basecomponent/BasePopupAdd.component';
import { FuseNavigationService } from '@fuse/components/navigation';
import { ActivatedRoute } from '@angular/router';


import { DataUrl, NgxImageCompressService } from 'ngx-image-compress';

import { v4 as uuidv4 } from 'uuid';
@Component({
    selector: 'sys_template_mail_popupAdd',
    templateUrl: 'popupAdd.html',
})
export class sys_template_mail_popUpAddComponent extends BasePopUpAddComponent {
    
    public list_type: any = [];
    constructor(public dialogRef: MatDialogRef<sys_template_mail_popUpAddComponent>,
        http: HttpClient, _translocoService: TranslocoService,
        _fuseNavigationService: FuseNavigationService, route: ActivatedRoute,
        @Inject('BASE_URL') baseUrl: string,
        private imageCompress: NgxImageCompressService,
        public dialogModal: MatDialog,
        @Inject(MAT_DIALOG_DATA) data: any) {
        super(_translocoService, _fuseNavigationService, route, baseUrl, http, 'sys_template_mail', dialogRef, dialogModal);

        this.record = data;
        this.Oldrecord = JSON.parse(JSON.stringify(data));
        this.actionEnum = data.actionEnum;
        if (this.actionEnum == 1) {
            this.baseInitData();
        }

        this.load_config_editor("sys_template_mail");
        this.http
            .post('/sys_type_mail.ctr/getListUse/', {}
            ).subscribe(resp => {
                this.list_type = resp;
            });
        //"mail_template_1": "Đăng ký tài khoản",
        //    "mail_template_2": "Quên mật khẩu",
        //        "mail_template_3": "Thư mời sự kiện",
        //            "mail_template_4": "Thư cảm ơn",
        //                "mail_template_5": "Tài khoản đã được duyệt",
        //                    "mail_template_6": "Bài viết đã được duyệt",

    }

}
