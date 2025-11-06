import { Component, Inject } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef, MatDialog } from '@angular/material/dialog';
import { v4 as uuidv4 } from 'uuid';

import { HttpClient, HttpEventType } from '@angular/common/http';
import { BasePopUpAddComponent } from 'app/Basecomponent/BasePopupAdd.component';
import { TranslocoService } from '@ngneat/transloco';
import { FuseNavigationService } from '@fuse/components/navigation';
import { ActivatedRoute } from '@angular/router';
import { Upload } from 'tus-js-client';
import { Subject } from 'rxjs';
import Swal from 'sweetalert2';
export interface FileStatus {
    filename: string;
    progress: number;
    hash: string;
    uuid: string;
}
@Component({
    selector: 'file_manager_popupAdd_file',
    templateUrl: 'popupAdd_file.component.html',
    styleUrls: ['./popupAdd_file.component.scss']
})

export class file_manager_popupAdd_fileComponent extends BasePopUpAddComponent {
    public plugintiny = [
        "advlist autolink lists link image charmap print preview anchor",
        "searchreplace visualblocks code fullscreen",
        "insertdatetime media table paste imagetools wordcount"
    ];
    public toolbartiny = "insertfile undo redo | styleselect | bold italic | alignleft aligncenter alignright alignjustify | bullist numlist outdent indent | link image";

    public timyconfig = {
        base_url: '/tinymce'
        , suffix: '.min',
        height: 500,
        images_upload_url: '/FileManager/uploadimagenew',
        plugins: this.plugintiny,
        toolbar: this.toolbartiny
    }
    public file_image: any;
    public lst_folder: any = [];
    public lst_sub_folder: any = [];
    public file_image_mobile: any;
    public Progress_image: any = -1;
    public Progress_image_mobile: any = -1;
    public list_san_pham: any = [];
    fileData: any;
    previewUrl: any = null;
    fileUploadProgress: any = -1;
    uploadedFilePath: string = null;
    constructor(public dialogRef: MatDialogRef<file_manager_popupAdd_fileComponent>,
        http: HttpClient, _translocoService: TranslocoService,
        _fuseNavigationService: FuseNavigationService, route: ActivatedRoute,
        @Inject('BASE_URL') baseUrl: string,
        public dialogModal: MatDialog,
        @Inject(MAT_DIALOG_DATA) data: any) {
        super(_translocoService, _fuseNavigationService, route, baseUrl, http, 'storage_file_manager', dialogRef, dialogModal);

        this.record = data;
        this.Oldrecord = JSON.parse(JSON.stringify(data));
        this.actionEnum = data.actionEnum;
        if (this.actionEnum == 1) {
            this.baseInitData();
        } else {
            this.get_list_file_edit();
        }
    }

    save_file(): void {
        this.beforesave();
        this.loading = true;
        if (this.actionEnum == 1) {
            this.http
                .post('storage_file_manager.ctr/save_file/',
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
        if (this.actionEnum == 2) {
            this.http
                .post('storage_file_manager.ctr/edit/',
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

    }
    //FILE
    //File
    downloadfile(id: any): string {
        var url = '/sys_anh_san_pham.ctr/download?id=' + id;
        return url;
    }

    get_list_file(): void {
        this.http.post(this.controller + '.ctr/get_list_file', {
            id: this.record.db.id_folder,
            //code:this.record.db.controller
        }).subscribe(resp => {
            var data: any;
            data = resp;
            this.record.list_file = data;
            if (data.db.list_file != null || data.db.list_file != undefined || data.db.list_file.length != 0)
                this.record.list_file = data.db.list_file.filter(q => q.status_del == 1);
            else
                this.record.list_file = []
        })
    }
    fileProgress(fileInput: any) {
        this.fileData = fileInput.target.files;
        if (this.record.role == 3)
            this.submitFile();
        else {
            Swal.fire(
                'Bạn chưa được cấp quyền để chỉnh sửa!',
                '',
                'warning'
            ).then(resp => {
            })
        }
        fileInput.target.value = null;
    }
    DragAndDropProgress(files: any) {
        this.fileData = files;
        if (this.record.role == 3)
            this.submitFile();
        else {
            Swal.fire(
                'Bạn chưa được cấp quyền để chỉnh sửa!',
                '',
                'warning'
            ).then(resp => {
            })
        }
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
        formData.append('controller', this.controller.toString());
        this.http.post('FileManager/upload_file_common', formData, {
            reportProgress: true,
            observe: 'events'
        })
            .subscribe(res => {
                if (res.type == HttpEventType.UploadProgress) {
                    this.fileUploadProgress = Math.round((res.loaded / res.total) * 100);

                } else if (res.type === HttpEventType.Response) {
                    Swal.fire('Lưu Thành công', '', 'success');
                    // var item: any;
                    // item = res.body;
                    // this.fileData = [];
                    // this.fileUploadProgress = -1;
                    // //this.get_list_file();
                    // this.record.list_file = item;
                    this.basedialogRef.close();
                }
            })
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
                //this.record.list_file.splice(pos, 1)
                this.http
                    .post(this.controller + '.ctr/delete_file/',
                        {
                            data: this.record,
                            pos: pos,
                        }
                    ).subscribe(resp => {
                        //this.record = resp;
                        //this.load_file();
                        Swal.fire('Xóa thành công', '', 'success');
                        //this.rerender();
                        this.get_list_file();
                        //this.aftersave();
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

    get_list_file_edit() {
        this.http.post('storage_file_manager.ctr/getElementById', {
            id: this.record.db.id,
        }).subscribe(resp => {

            this.record = resp;
        })
    }

    ngOnInit(): void {
    }
    formatSizeUnits(bytes) {
        if (bytes >= 1073741824) { bytes = (bytes / 1073741824).toFixed(2) + " GB"; }
        else if (bytes >= 1048576) { bytes = (bytes / 1048576).toFixed(2) + " MB"; }
        else if (bytes >= 1024) { bytes = (bytes / 1024).toFixed(2) + " KB"; }
        else if (bytes > 1) { bytes = bytes + " bytes"; }
        else if (bytes == 1) { bytes = bytes + " byte"; }
        else { bytes = "0 bytes"; }
        return bytes;
    }

    getFontAwesomeIconFromMIME(mimeType) {
        var icon_classes = {
            // Media
            "image/svg+xml": "assets/icon_file_type/jpg.png",
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
            "application/octet-stream": "assets/icon_file_type/zip-1.png",
            "application/vnd.openxmlformats-officedocument.presentationml.presentation": "assets/icon_file_type/ppt.png",
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
}
