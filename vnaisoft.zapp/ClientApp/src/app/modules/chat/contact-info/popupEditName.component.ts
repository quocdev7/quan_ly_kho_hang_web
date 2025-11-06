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
    selector: 'popupEditName',
    templateUrl: 'popupEditName.component.html',
})
export class popupEditNameComponent extends BasePopUpAddComponent {

    public user_info: any;

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
        public dialogRef: MatDialogRef<popupEditNameComponent>,
        http: HttpClient, _translocoService: TranslocoService,
        private _fuseMediaWatcherService: FuseMediaWatcherService,
        _fuseNavigationService: FuseNavigationService, route: ActivatedRoute,
        @Inject('BASE_URL') baseUrl: string,
        public dialogModal: MatDialog,
        @Inject(MAT_DIALOG_DATA) data: any) {
        super(_translocoService, _fuseNavigationService, route, baseUrl, http, 'sys_user', dialogRef, dialogModal);
        this.record = data;
        this.Oldrecord = JSON.parse(JSON.stringify(data));
        this.actionEnum = data.actionEnum;
    }

    update_name() {

        var name = this.record.db.conversation_group_name
        this.dialogRef.close(name);
    }
}
