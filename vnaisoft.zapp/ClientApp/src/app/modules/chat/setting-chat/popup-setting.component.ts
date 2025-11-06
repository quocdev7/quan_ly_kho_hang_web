import { Component, Inject } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef, MatDialog } from '@angular/material/dialog';


import { HttpClient, HttpEventType } from '@angular/common/http';
import { BasePopUpAddComponent } from 'app/Basecomponent/BasePopupAdd.component';
import { TranslocoService } from '@ngneat/transloco';
import { FuseNavigationService } from '@fuse/components/navigation';
import { ActivatedRoute } from '@angular/router';
import { v4 as uuidv4 } from 'uuid';
import Swal from 'sweetalert2';
import { FuseMediaWatcherService } from '../../../../@fuse/services/media-watcher';
import { takeUntil } from 'rxjs/operators';
import { Subject } from 'rxjs';

import { DataUrl, NgxImageCompressService } from "ngx-image-compress";
import { base64ToFile, ImageCroppedEvent, LoadedImage } from 'ngx-image-cropper';

@Component({
    selector: 'popup-setting',
    templateUrl: 'popup-setting.component.html',
})
export class popup_setting_chatComponent extends BasePopUpAddComponent {

    public user_info: any;
    public profile: any;

    public isScreenSmall: any = false;
    private _unsubscribeAll: Subject<any> = new Subject<any>();
    public file_image: any;
    public Progress_image: any = -1;

    croppedImage: any = '';
    public imgResultBeforeCompression: string = "";
    public imgResultAfterCompression: string = "";

    imageChangedEvent: any = '';
    constructor(
        public imageCompress: NgxImageCompressService,
        public dialogRef: MatDialogRef<popup_setting_chatComponent>,
        http: HttpClient, _translocoService: TranslocoService,
        private _fuseMediaWatcherService: FuseMediaWatcherService,
        _fuseNavigationService: FuseNavigationService, route: ActivatedRoute,
        @Inject('BASE_URL') baseUrl: string,
        public dialogModal: MatDialog,
        @Inject(MAT_DIALOG_DATA) data: any) {
        super(_translocoService, _fuseNavigationService, route, baseUrl, http, 'sys_user', dialogRef, dialogModal);
        this.record = data;

        this.record.db.avatar_path = 'assets/images/logo/logo-bka.png';
        // this.Oldrecord = JSON.parse(JSON.stringify(data));
        // this.actionEnum = data.actionEnum;
        this._fuseMediaWatcherService.onMediaChange$
            .pipe(takeUntil(this._unsubscribeAll))
            .subscribe(({ matchingAliases }) => {
                // Check if the screen is small
                this.isScreenSmall = !matchingAliases.includes('md');
            });
        this.get_profile_user()
    }
    get_profile_user() {

        this.http.post('chat.ctr/get_profile_user', {}).subscribe(resp => {
            this.profile = resp;
        })
    }
    DragAndDrop_image(files: any) {
        this.file_image = files;
        var reader = new FileReader();
        reader.readAsDataURL(this.file_image[0]);
        var that = this;
        reader.onload = function () {
            that.imageCompress.compressFile(reader.result.toString(), 1, 50, 50, 1200, 1200) // 50% ratio, 30% quality
                .then((compressedImage: DataUrl) => {
                    that.imgResultAfterCompression = compressedImage;
                });
        };
    }
    fileChangeEvent(event: any): void {
        this.file_image = event.target.files;
        var reader = new FileReader();
        reader.readAsDataURL(this.file_image[0]);
        var that = this;
        reader.onload = function () {
            that.imageCompress.compressFile(reader.result.toString(), 1, 50, 50, 1200, 1200) // 50% ratio, 30% quality
                .then((compressedImage: DataUrl) => {
                    that.imgResultAfterCompression = compressedImage;
                });
        };
    }
    imageCropped(event: ImageCroppedEvent) {
        this.croppedImage = event.base64;
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
    imageLoaded(image: LoadedImage) {
        // show cropper
    }
    cropperReady() {
        // cropper ready
    }
    loadImageFailed() {
        // show message
    }
    compressFile() {
        var img = this.croppedImage;
        this.imageCompress.compressFile(img, 1, 50, 50, 1000, 1000) // 50% ratio, 30% quality
            .then((compressedImage: DataUrl) => {
                this.imgResultAfterCompression = compressedImage;
                this.file_image = this.base64ToFile(
                    this.imgResultAfterCompression,
                    "image.png",
                );
                console.warn(
                    'Size in bytes is now:',
                    this.imageCompress.byteCount(compressedImage)
                );
                this.submitFile();
            });
    }
    update(): void {
        //1 main image , 2 avatar , 3 info , 4 link youtobe
        this.record.type_update = 2;
        this.http
            .post(this.controller + '.ctr/updateProfile/',
                {
                    data: this.record,
                }
            ).subscribe(resp => {
                this.record = resp;
                this.Oldrecord = this.record;
                this.basedialogRef.close(this.record);
                Swal.fire('Lưu thành công', '', 'success');
                this.aftersave();
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
    }
    submitFile() {
        var formData = new FormData();
        this.Progress_image = 0;
        for (var i = 0; i < this.file_image.length; i++) {
            formData.append('list_file[]', this.file_image[i]);
        }
        if (this.record.db.id == null || this.record.db.id == 0) {
            this.record.db.id = uuidv4()
        };
        formData.append('id', this.record.db.id);
        formData.append('controller', this.controller.toString());
        formData.append('list_file[]', this.file_image);
        this.http.post('FileManager/uploadimagenew', formData, {
            reportProgress: true,
            observe: 'events'
        })
            .subscribe(res => {
                if (res.type == HttpEventType.UploadProgress) {
                    this.Progress_image = Math.round((res.loaded / res.total) * 100);
                } else if (res.type === HttpEventType.Response) {
                    var item: any;
                    item = res.body;
                    this.record.db.avatar_path = item.location;
                    this.file_image = null
                    this.Progress_image = -1;
                    this.update();
                }
            })
    }
}
