import { Component, OnInit, ViewEncapsulation, Input, EventEmitter, Output, OnDestroy, Inject, ElementRef, ViewChild, Directive, HostBinding, HostListener } from '@angular/core';
import { FormBuilder, FormGroup, Validators, FormControl } from '@angular/forms';
import { TranslocoService } from '@ngneat/transloco';
import Swal from 'sweetalert2';
import { ReplaySubject, Subject } from 'rxjs';
import { filter, tap, takeUntil, debounceTime, map, delay, startWith } from 'rxjs/operators';
import { HttpClient, HttpEventType, HttpParams } from '@angular/common/http';
import { ProgressSpinnerMode } from '@angular/material/progress-spinner';
import { ThemePalette } from '@angular/material/core';
import { MatDialog, MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { FuseNavigationService } from '../../../../@fuse/components/navigation/navigation.service';

import { BasePopUpAddComponent } from 'app/Basecomponent/BasePopupAdd.component';
import { ActivatedRoute } from '@angular/router';


import { Observable } from 'rxjs';
import { COMMA, ENTER } from '@angular/cdk/keycodes';
import { MatAutocompleteSelectedEvent, MatAutocomplete } from '@angular/material/autocomplete';
import { MatChipInputEvent } from '@angular/material/chips';

@Component({
    selector: 'cm_file_upload_popup',
    templateUrl: './cm_file_upload_popup.component.html'
})
export class cm_file_upload_popupComponent extends BasePopUpAddComponent {
    public list_file: any;
    public record: any;
    constructor(public dialogRef: MatDialogRef<cm_file_upload_popupComponent>,
        http: HttpClient, _translocoService: TranslocoService,
        _fuseNavigationService: FuseNavigationService, route: ActivatedRoute,
        @Inject('BASE_URL') baseUrl: string,
        public dialogModal: MatDialog,
        @Inject(MAT_DIALOG_DATA) data: any) {
        super(_translocoService, _fuseNavigationService, route, baseUrl, http, 'erp_file_upload', dialogRef, dialogModal);


        this.record = data;
        this.Oldrecord = JSON.parse(JSON.stringify(data));
        this.actionEnum = data.actionEnum;
        if (this.actionEnum == 1 || this.actionEnum == 3) {
            this.baseInitData();
        }
        this.get_list_file();
    }

    downloadFile(id_child, file_name) {
        const params = new HttpParams()
            .set('id', this.record.db.id)
            .set('id_child', id_child)
            ;

        let uri = this.controller + '.ctr/downloadFile';
        this.http.get(uri, { params, responseType: 'blob', observe: 'response' })
            .subscribe(resp => {
                var res;

                res = resp;
                var downloadedFile = new Blob([res.body], { type: res.body.type });
                const a = document.createElement('a');
                a.setAttribute('style', 'display:none;');
                document.body.appendChild(a);
                a.href = URL.createObjectURL(downloadedFile);
                a.target = '_dAblank';
                a.download = file_name;
                a.click();
                document.body.removeChild(a);
            })
    }
    get_list_file(): void {
        this.http.post(this.controller + '.ctr/get_list_file', {
            id: this.record.db.id,
        }).subscribe(resp => {
            var data: any;
            data = resp;
            if (data != null) {
                this.list_file = data.db.list_file.filter(q => q.status_del == 1);
                this.record.db.list_file = this.list_file
            } else
                this.list_file = []
        })
    }
    fileData: any;
    previewUrl: any = null;
    fileUploadProgress: any = -1;
    uploadedFilePath: string = null;
    formatSizeUnits(bytes) {
        if (bytes >= 1073741824) { bytes = (bytes / 1073741824).toFixed(2) + " GB"; }
        else if (bytes >= 1048576) { bytes = (bytes / 1048576).toFixed(2) + " MB"; }
        else if (bytes >= 1024) { bytes = (bytes / 1024).toFixed(2) + " KB"; }
        else if (bytes > 1) { bytes = bytes + " bytes"; }
        else if (bytes == 1) { bytes = bytes + " byte"; }
        else { bytes = "0 bytes"; }
        return bytes;
    }
    fileProgress(fileInput: any) {
        this.fileData = fileInput.target.files;
        this.submitFile();
        fileInput.target.value = null;
    }
    DragAndDropProgress(files: any) {
        this.fileData = files;
        this.submitFile();
    }
    submitFile() {

        this.fileUploadProgress = 0;
        var formData = new FormData();
        for (var i = 0; i < this.fileData.length; i++) {
            var extension = this.fileData[0].type;
            if (this.fileData[0].size > 15728640) {
                alert("File upload không được quá 15 Mb!");
                return;
            };
            formData.append('list_file[]', this.fileData[i]);
        }
        var modelsubmit = this.record;
        formData.append('model', JSON.stringify(modelsubmit));
        this.http.post('FileManager/upload_file_common', formData, {
            reportProgress: true,
            observe: 'events'
        })
            .subscribe(res => {
                if (res.type == HttpEventType.UploadProgress) {
                    this.fileUploadProgress = Math.round((res.loaded / res.total) * 100);

                } else if (res.type === HttpEventType.Response) {
                    Swal.fire('Lưu Thành công', '', 'success');
                    this.fileData = [];
                    this.fileUploadProgress = -1;
                    this.get_list_file();
                }
            })
    }
    getFontAwesomeIconFromMIME(mimeType) {
        // List of official MIME Types: http://www.iana.org/assignments/media-types/media-types.xhtml
        var icon_classes = {
            // Media
            "image/jpeg": "assets/icon_file_type/jpg.png",
            "image/png": "assets/icon_file_type/png.png",
            // Documents
            "application/pdf": "assets/icon_file_type/pdf.png",
            "application/msword": "assets/icon_file_type/doc.png",
            "application/vnd.ms-word": "assets/icon_file_type/doc.png",
            "application/vnd.oasis.opendocument.text": "assets/icon_file_type/doc.png",
            "application/vnd.openxmlformats-officedocument.wordprocessingml":
                "assets/icon_file_type/doc.png",
            "application/vnd.ms-excel": 'assets/icon_file_type/excel.png',
            "application/vnd.openxmlformats-officedocument.spreadsheetml":
                'assets/icon_file_type/excel.png',
            "application/vnd.oasis.opendocument.spreadsheet": "assets/icon_file_type/excel.png",
            "application/vnd.ms-powerpoint": "assets/icon_file_type/ppt.png",
            "application/vnd.openxmlformats-officedocument.presentationml":
                "assets/icon_file_type/ppt.png",
            "application/vnd.oasis.opendocument.presentation": "assets/icon_file_type/ppt.png",
            "text/plain": "assets/icon_file_type/txt.png",
            "text/html": "assets/icon_file_type/html.png",
            "application/json": "assets/icon_file_type/json-file.png",
            // Archives
            "application/gzip": "assets/icon_file_type/zip.png",
            "application/x-zip-compressed": "assets/icon_file_type/zip.png",
            "application/octet-stream": "assets/icon_file_type/zip-1.png"
        };

        for (var key in icon_classes) {
            if (icon_classes.hasOwnProperty(key)) {
                if (mimeType.search(key) === 0) {
                    // Found it
                    return icon_classes[key];
                }
            } else {
                return "assets/icon_file_type/file.png";
            }
        }
    }

    deleteFile(model, pos) {
        Swal.fire({
            title: this._translocoService.translate('areYouSure'),
            text: "",
            icon: 'warning',
            showCancelButton: true,
            confirmButtonColor: '#3085d6',
            cancelButtonColor: '#d33',
            confirmButtonText: this._translocoService.translate('yes'),
            cancelButtonText: this._translocoService.translate('no')
        }).then((result) => {
            if (result.value) {
                this.http
                    .post(this.controller + '.ctr/delete_file/',
                        {
                            data: this.record,
                            pos: pos,
                        }
                    ).subscribe(resp => {
                        Swal.fire('Xóa thành công', '', 'success');
                        this.get_list_file();
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
        })
    }
}




