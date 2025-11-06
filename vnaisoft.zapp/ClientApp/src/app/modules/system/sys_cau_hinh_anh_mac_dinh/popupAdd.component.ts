import { Component, Inject } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef, MatDialog } from '@angular/material/dialog';


import { HttpClient, HttpEventType } from '@angular/common/http';
import { BasePopUpAddComponent } from 'app/Basecomponent/BasePopupAdd.component';
import { TranslocoService } from '@ngneat/transloco';
import { FuseNavigationService } from '@fuse/components/navigation';
import { ActivatedRoute } from '@angular/router';
import Swal from 'sweetalert2';
import { v4 as uuidv4 } from 'uuid';
import { DataUrl, DOC_ORIENTATION, NgxImageCompressService, UploadResponse } from 'ngx-image-compress';


@Component({
    selector: 'sys_cau_hinh_anh_mac_dinh_popupAdd',
    templateUrl: 'popupAdd.html',
})
export class sys_cau_hinh_anh_mac_dinh_popUpAddComponent extends BasePopUpAddComponent {
    public file_image: any;
    public file_image_avatar: any;
    public Progress_image: any = -1;
    public Progress_image_avatar: any = -1;
    public list_type: any;
    public plugintiny = [
        "advlist autolink lists link image charmap print preview anchor",
        "searchreplace visualblocks code fullscreen",
        "insertdatetime media table paste imagetools wordcount"
    ];
    public toolbartiny = "insertfile undo redo | styleselect | bold italic | alignleft aligncenter alignright alignjustify | bullist numlist outdent indent | link image";

    public timyconfig: any;

    constructor(public dialogRef: MatDialogRef<sys_cau_hinh_anh_mac_dinh_popUpAddComponent>,
        http: HttpClient, _translocoService: TranslocoService,
        _fuseNavigationService: FuseNavigationService, route: ActivatedRoute,
        @Inject('BASE_URL') baseUrl: string,
        public dialogModal: MatDialog,
        private imageCompress: NgxImageCompressService,

        @Inject(MAT_DIALOG_DATA) data: any) {
        super(_translocoService, _fuseNavigationService, route, baseUrl, http, 'sys_cau_hinh_anh_mac_dinh', dialogRef, dialogModal);
        this.record = data;
        this.Oldrecord = JSON.parse(JSON.stringify(data));
        this.actionEnum = data.actionEnum;
        if (this.actionEnum == 1) {
            this.baseInitData();
        }
        this.list_type = [
            {
                id: 1,
                name: this._translocoService.translate("system.hinh_anh_dai_dien")
            },
            {
                id: 2,
                name: this._translocoService.translate("system.logo")
            },
            {
                id: 3,
                name: this._translocoService.translate("system.khac")
            },
            
        ]
    }

    base64ToFile(data, filename) {
        const arr = data.split(',');
        const mime = arr[0].match(/:(.*?);/)[1];
        const bstr = atob(arr[1]);
        let n = bstr.length;
        let u8arr = new Uint8Array(n);

        while (n--) {
            u8arr[n] = bstr.charCodeAt(n);
        }

        return new File([u8arr], filename, { type: mime });
    }
    public imgResultBeforeCompression: string = "";
    public imgResultAfterCompression: string = "";
    chose_file_image(fileInput: any) {

        this.file_image = fileInput.target.files;
        var rule_image = 3 * 1048576;
        if (this.file_image[0].size > rule_image) {
            Swal.fire(this._translocoService.translate('system.anh_toi_da_3mb'), "", "warning");
            fileInput.target.value = null;
        } else {
            this.compressFile();
            fileInput.target.value = null;
        }
    }
    DragAndDrop_image(files: any) {


        this.file_image = files;
        var rule_image = 3 * 1048576;
        if (this.file_image[0].size > rule_image) {
            Swal.fire(this._translocoService.translate('system.anh_toi_da_3mb'), "", "warning");
        } else {
            this.compressFile();

        }
    }
    compressFile() {
        var reader = new FileReader();
        reader.readAsDataURL(this.file_image[0]);
        var that = this;
        reader.onload = function () {
            that.imageCompress.compressFile(reader.result.toString(), 1, 50, 50, 3000, 3000) // 50% ratio, 30% quality
                .then((compressedImage: DataUrl) => {
                    that.imgResultAfterCompression = compressedImage;
                    that.file_image = that.base64ToFile(
                        that.imgResultAfterCompression,
                        "image.png"
                    );
                    console.warn(
                        'Size in bytes is now:',
                        that.imageCompress.byteCount(compressedImage)
                    );
                    that.submitFile(false);
                });
            // that.imageCompress.compressFile(reader.result.toString(), 1, 50, 50, 800, 800) // 50% ratio, 30% quality
            //     .then((compressedImage: DataUrl) => {
            //         that.imgResultAfterCompression = compressedImage;
            //         that.file_image = that.base64ToFile(
            //             that.imgResultAfterCompression,
            //             "image.png"
            //         );
            //         console.warn(
            //             'Size in bytes is now:',
            //             that.imageCompress.byteCount(compressedImage)
            //         );
            //         that.submitFile(true);
            //     });
        };


    }
    submitFile(is_thumbnail: boolean) {

        var formData = new FormData();

        this.Progress_image = 0;
        for (var i = 0; i < this.file_image.length; i++) {
            formData.append('list_file[]', this.file_image[i]);      
            formData.append('id_phieu', this.record.db.id);
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

                    this.record.image = item.file_path;
                    this.record.file = item;

                    this.file_image = null
                    this.Progress_image = -1;
                }

            })


    }
    compressFileMobile() {
        var reader = new FileReader();
        reader.readAsDataURL(this.file_image_avatar[0]);
        var that = this;
        reader.onload = function () {
            that.imageCompress.compressFile(reader.result.toString(), 1, 50, 50, 3000, 3000) // 50% ratio, 30% quality
                .then((compressedImage: DataUrl) => {
                    that.imgResultAfterCompression = compressedImage;
                    that.file_image_avatar = that.base64ToFile(
                        that.imgResultAfterCompression,
                        "image.png"
                    );
                    console.warn(
                        'Size in bytes is now:',
                        that.imageCompress.byteCount(compressedImage)
                    );
                    that.submitFileMobile(false);
                });
            // that.imageCompress.compressFile(reader.result.toString(), 1, 50, 50, 800, 800) // 50% ratio, 30% quality
            //     .then((compressedImage: DataUrl) => {
            //         that.imgResultAfterCompression = compressedImage;
            //         that.file_image_avatar = that.base64ToFile(
            //             that.imgResultAfterCompression,
            //             "image.png"
            //         );
            //         console.warn(
            //             'Size in bytes is now:',
            //             that.imageCompress.byteCount(compressedImage)
            //         );
            //         that.submitFileMobile(true);
            //     });
        };


    }
    chose_file_image_avatar(fileInput: any) {

        this.file_image_avatar = fileInput.target.files;
        var rule_image = 3 * 1048576;
        if (this.file_image_avatar[0].size > rule_image) {
            Swal.fire(this._translocoService.translate('system.anh_toi_da_3mb'), "", "warning");
            fileInput.target.value = null;
        } else {
            this.compressFileMobile();
            fileInput.target.value = null;
        }
    }
    DragAndDrop_image_avatar(files: any) {


        this.file_image_avatar = files;
        var rule_image = 3 * 1048576;
        if (this.file_image_avatar[0].size > rule_image) {
            Swal.fire(this._translocoService.translate('system.anh_toi_da_3mb'), "", "warning");

        } else {
            this.compressFileMobile();

        }
    }
    submitFileMobile(is_thumbnail: boolean) {
        var formData = new FormData();

        this.Progress_image = 0;
        for (var i = 0; i < this.file_image.length; i++) {
            formData.append('list_file[]', this.file_image[i]);      
            formData.append('id_phieu', this.record.db.id);
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

                    this.record.avatar = item.file_path;
                    this.record.file_avatar = item;

                    this.file_image_avatar = null
                    this.Progress_image_avatar = -1;
                }

            })}

}
