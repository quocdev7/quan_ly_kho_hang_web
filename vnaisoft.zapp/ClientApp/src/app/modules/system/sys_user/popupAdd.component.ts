import { Component, Inject } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef, MatDialog } from '@angular/material/dialog';


import { HttpClient, HttpEventType } from '@angular/common/http';
import { BasePopUpAddComponent } from 'app/Basecomponent/BasePopupAdd.component';
import { TranslocoService } from '@ngneat/transloco';
import { FuseNavigationService } from '@fuse/components/navigation';
import { ActivatedRoute } from '@angular/router';
import { BasePopUpAddTypeComponent } from 'app/Basecomponent/BasePopupAddType.component';
import { sys_user_model } from './sys_user.types';
import Swal from 'sweetalert2';
import { DataUrl, DOC_ORIENTATION, NgxImageCompressService, UploadResponse } from 'ngx-image-compress';
@Component({
    selector: 'sys_user_popupAdd',
    templateUrl: 'popupAdd.html',
})
export class sys_user_popUpAddComponent extends BasePopUpAddTypeComponent<sys_user_model> {

    public list_phong_ban: any;
    public list_chuc_danh: any;
    public list_nhom_quyen: any;
    public file_logo: any;
    public Progress_logo: any = -1;
    public group_field: any;
    public Progress_image: any = -1;
    public file_image: any;
    public Progress_cover_image: any = -1;
    public file_cover_image: any;
    constructor(public dialogRef: MatDialogRef<sys_user_popUpAddComponent>,
        http: HttpClient, _translocoService: TranslocoService,
        _fuseNavigationService: FuseNavigationService, route: ActivatedRoute,
        @Inject('BASE_URL') baseUrl: string,
        public dialogModal: MatDialog,
        private imageCompress: NgxImageCompressService,
        @Inject(MAT_DIALOG_DATA) data: any) {
        super(_translocoService, _fuseNavigationService, route, baseUrl, http, 'sys_user', dialogRef, dialogModal);
        this.record = data;

        this.Oldrecord = JSON.parse(JSON.stringify(data));
        this.actionEnum = data.actionEnum;
        if (this.actionEnum == 1) {
            this.baseInitData();
        }
        this.get_list_phong_ban();
    }

    get_list_phong_ban() {
        this.http.post("sys_phong_ban.ctr/getListUse", {}).subscribe(resp => {
            this.list_phong_ban = resp
        })
    }

    get_list_nhom_quyen(): void {
        this.http
            .post('/sys_group_user.ctr/getListUse', {
            }
            ).subscribe(resp => {
                var data: any = resp;
                this.list_nhom_quyen = data;
            });
    }

    change_username(): void {
        //this.record.Username = this.record.email;
    }
    chose_file_image(fileInput: any) {

        this.file_image = fileInput.target.files;
        var rule_image = 3 * 1048576;
        if (this.file_image[0].size > rule_image) {
            Swal.fire(this._translocoService.translate('system.anh_toi_da_3mb'), "", "warning");
            fileInput.target.value = null;
        } else {
            this.submitFile();
            fileInput.target.value = null;
        }
    }
    DragAndDrop_image(files: any) {

        this.file_image = files;
        var rule_image = 3 * 1048576;
        if (this.file_image[0].size > rule_image) {
            Swal.fire(this._translocoService.translate('system.anh_toi_da_3mb'), "", "warning");
        } else {
            this.submitFile();
        }
    }
    submitFile() {
        var formData = new FormData();

        this.Progress_image = 0;
        for (var i = 0; i < this.file_image.length; i++) {
            formData.append('list_file[]', this.file_image[i]);
            formData.append('controller', this.controller.toString());
        }
        formData.append('list_file[]', this.file_image);
        this.http.post('FileManager/upload_file_common', formData, {
            reportProgress: true,
            observe: 'events'
        })
            .subscribe(res => {
                if (res.type == HttpEventType.UploadProgress) {

                    this.Progress_image = Math.round((res.loaded / res.total) * 100);


                } else if (res.type === HttpEventType.Response) {
                    var item: any;
                    item = res.body;

                    this.record.hinh_anh_dai_dien = item.file_path;
                    this.record.file = item;

                    this.file_image = null
                    this.Progress_image = -1;
                }

            })
    }


    ngOnInit(): void {
        //this.get_list_nhom_quyen();
    }
}
