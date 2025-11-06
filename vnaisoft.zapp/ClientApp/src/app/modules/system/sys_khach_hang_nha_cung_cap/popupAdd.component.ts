import { Component, Inject } from '@angular/core';
import {
    MAT_DIALOG_DATA,
    MatDialogRef,
    MatDialog,
} from '@angular/material/dialog';

import { HttpClient, HttpEventType } from '@angular/common/http';
import { BasePopUpAddComponent } from 'app/Basecomponent/BasePopupAdd.component';
import { TranslocoService } from '@ngneat/transloco';
import { FuseNavigationService } from '@fuse/components/navigation';
import { ActivatedRoute } from '@angular/router';
import { BasePopUpAddTypeComponent } from 'app/Basecomponent/BasePopupAddType.component';
import { sys_khach_hang_nha_cung_cap_model } from './sys_khach_hang_nha_cung_cap.types';
import Swal from 'sweetalert2';
import { listbank } from 'app/core/data/data';
//import { erp_tai_khoan_ngan_hang_popUpAddComponent } from '../erp_tai_khoan_ngan_hang/popupAdd.component';

@Component({
    selector: 'sys_khach_hang_nha_cung_cap_popupAdd',
    templateUrl: 'popupAdd.html',
    styleUrls: ['./popupAdd.component.scss'],
})
export class sys_khach_hang_nha_cung_cap_popUpAddComponent extends BasePopUpAddTypeComponent<sys_khach_hang_nha_cung_cap_model> {
    public plugintiny = [
        'advlist autolink lists link image charmap print preview anchor',
        'searchreplace visualblocks code fullscreen',
        'insertdatetime media table paste imagetools wordcount',
    ];
    public toolbartiny =
        'insertfile undo redo | styleselect | bold italic | alignleft aligncenter alignright alignjustify | bullist numlist outdent indent | link image';

    public timyconfig = {
        base_url: '/tinymce',
        suffix: '.min',
        height: 500,
        images_upload_url: '/FileManager/uploadimage',
        plugins: this.plugintiny,
        toolbar: this.toolbartiny,
    };
    public list_hinh_thuc: any[];
    public list_loai: any[];
    public file_logo: any;
    public lst: any;
    public ma_so_thue: any;
    public list_ma_so_thue: any;
    public Progress_logo: any = -1;
    public group_field: any;
    public check_nha_cung_cap: any;
    public check_khach_hang: any;
    public check_to_chuc: boolean;
    public check_ca_nhan: boolean;
    public list_ngan_hang: any;

    constructor(
        public dialogRef: MatDialogRef<sys_khach_hang_nha_cung_cap_popUpAddComponent>,
        http: HttpClient,
        _translocoService: TranslocoService,
        _fuseNavigationService: FuseNavigationService,
        route: ActivatedRoute,
        @Inject('BASE_URL') baseUrl: string,
        public dialogModal: MatDialog,
        @Inject(MAT_DIALOG_DATA) data: any
    ) {
        super(
            _translocoService,
            _fuseNavigationService,
            route,
            baseUrl,
            http,
            'sys_khach_hang_nha_cung_cap',
            dialogRef,
            dialogModal
        );
        this.record = data;
        this.Oldrecord = JSON.parse(JSON.stringify(data));
        this.actionEnum = data.actionEnum;
        this.list_ngan_hang = listbank;
        this.list_ngan_hang = listbank.filter((q) => q.id != '-1');
        if (this.actionEnum == 1) {
            this.baseInitData();
        }
        if (this.actionEnum != 1) {
            this.getElementByMa();
        }
        this.get_list_hinh_thuc();
    }
    getElementByMa() {
        this.http
            .post('sys_khach_hang_nha_cung_cap.ctr/getElementByMa', {
                ma: this.record.db.ma,
            })
            .subscribe((resp) => {
                var data: any;
                data = resp;
                this.record = data;
            });
    }
    load_ma_so_thue(): void {
        debugger;
        var code =
            this.record.db.hinh_thuc == 1
                ? this.record.db.dien_thoai.trim()
                : this.record.db.ma_so_thue.trim();

        this.http
            .post('/sys_khach_hang_nha_cung_cap.ctr/sync_cong_ty/', {
                hinh_thuc: this.record.db.hinh_thuc,
                code: code.trim(),
            })
            .subscribe((resp) => {
                var data: any;
                data = resp;

                if (this.record.db.hinh_thuc == 1) {
                    if (data == null) {
                    } else {
                        this.record.db.ten = data.db.ten;
                        this.record.db.dia_chi = data.db.dia_chi;
                        this.record.db.email = data.db.email;
                    }
                } else {
                    if (data == null) {
                    } else {
                        this.record.db.ten = data.db.ten;
                        this.record.db.dia_chi = data.db.dia_chi;
                        this.record.db.email = data.db.email;
                        this.record.db.dien_thoai = data.db.dien_thoai;
                    }
                }
            });
        this.record.db.ma_so_thue = this.record.db.ma_so_thue.trim();
        this.record.db.dien_thoai = this.record.db.dien_thoai.trim();
    }
    get_code() {
        this.http
            .post('/sys_khach_hang_nha_cung_cap.ctr/get_code/', {})
            .subscribe((resp) => {
                this.record.db.ma = 'Tự động tạo';
            });
        //this.record.db.ma = this.controller + "-" + this.Progress_logo
    }
    get_value_check(value) {
        if (value == 1) {
            this.record.db.hinh_thuc = 1;
        } else if (value == 2) {
            this.record.db.hinh_thuc = 2;
        } else {
        }
    }
    get_list_hinh_thuc() {
        this.list_hinh_thuc = [
            { id: 1, name: this._translocoService.translate('system.ca_nhan') },
            { id: 2, name: this._translocoService.translate('system.to_chuc') },
            // { id: 3, name: this._translocoService.translate('system.phong_ban') },
            // { id: 4, name: this._translocoService.translate('system.nhan_vien') },
        ];
        //this.save();
    }
    lst_loai() {
        this.list_loai = [
            {
                id: 1,
                name: this._translocoService.translate('system.khach_hang'),
            },
            {
                id: 2,
                name: this._translocoService.translate('system.nha_cung_cap'),
            },
        ];
    }
    chose_file_logo(fileInput: any) {
        this.file_logo = fileInput.target.files;
        var rule_image = 3 * 1048576;
        if (this.file_logo[0].size > rule_image) {
            Swal.fire(
                this._translocoService.translate('system.anh_toi_da_3mb'),
                '',
                'warning'
            );
            fileInput.target.value = null;
        } else {
            this.submitFile();
            fileInput.target.value = null;
        }
    }
    DragAndDrop_logo(files: any) {
        this.file_logo = files;
        var rule_image = 3 * 1048576;
        if (this.file_logo[0].size > rule_image) {
            Swal.fire(
                this._translocoService.translate('system.anh_toi_da_3mb'),
                '',
                'warning'
            );
        } else {
            this.submitFile();
        }
    }
    submitFile() {
        var formData = new FormData();

        this.Progress_logo = 0;
        for (var i = 0; i < this.file_logo.length; i++) {
            formData.append('list_file[]', this.file_logo[i]);
        }
        formData.append('list_file[]', this.file_logo);
        this.http
            .post('FileManager/uploadimage', formData, {
                reportProgress: true,
                observe: 'events',
            })
            .subscribe((res) => {
                if (res.type == HttpEventType.UploadProgress) {
                    this.Progress_logo = Math.round(
                        (res.loaded / res.total) * 100
                    );
                } else if (res.type === HttpEventType.Response) {
                    var item: any;
                    item = res.body;

                    //this.record.db.logo = item.location;
                    this.file_logo = null;
                    this.Progress_logo = -1;
                }
            });
    }
}
