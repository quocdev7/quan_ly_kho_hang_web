import { Component, Inject, HostListener } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef, MatDialog } from '@angular/material/dialog';

import { HttpClient, HttpEventType, HttpParams, HttpResponse } from '@angular/common/http';

import { TranslocoService } from '@ngneat/transloco';

import { ActivatedRoute } from '@angular/router';
import { BaseIndexDatatableComponent } from 'app/Basecomponent/BaseIndexDatatable.component';
import { FuseNavigationService } from '@fuse/components/navigation';
import Swal from 'sweetalert2';
import { DomSanitizer } from '@angular/platform-browser';
import { BasePopUpAddComponent } from 'app/Basecomponent/BasePopupAdd.component';

@Component({
    selector: 'cm_view_file_upload',
    templateUrl: 'cm_view_file_upload.component.html'
})
export class cm_view_file_uploadComponent extends BaseIndexDatatableComponent {

    public loading = false;
    public srcfile: any = "";
    public title: any = "";
    public record: any;
    public file: any;
    public icon_classes: any;
    public show_details: any = false;
    constructor(public dialogRef: MatDialogRef<cm_view_file_uploadComponent>, dialog: MatDialog,
        http: HttpClient, _translocoService: TranslocoService,
        _fuseNavigationService: FuseNavigationService, route: ActivatedRoute,
        @Inject('BASE_URL') baseUrl: string,
        private sanitizer: DomSanitizer,
        public dialogModal: MatDialog,
        @Inject(MAT_DIALOG_DATA) data: any) {
        super(http, baseUrl, _translocoService, _fuseNavigationService, route, dialogModal, '',
            { search: "", status_del: "1", id_folder: "" }
        )
        this.record = data;
        debugger
        this.file = data;
    this.icon_classes = {
            "image/gif": "assets/icon_file_type/gif.png",
            "image/webp": "assets/icon_file_type/webp.png",
            // Media
            "video/mp4": "assets/icon_file_type/mp4.png",
            "video/mp3": "assets/icon_file_type/mp3.png",
            "audio/mp4": "assets/icon_file_type/mp3.png",
            "audio/wav": "assets/icon_file_type/wav.png",
            "audio/x-wav": "assets/icon_file_type/wav.png",
            "audio/mpeg": "assets/icon_file_type/mp3.png",
            "audio/mp3": "assets/icon_file_type/mp3.png",

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
        var url = data.url.replace("/downloadFile?id=", "/viewonline?id=");
        if (data.file_type == "text/plain" ||
            data.file_type == "text/html" ||
            data.file_type == "application/json" ||
            data.file_type == "application/pdf" ||
            data.file_type == "image/jpeg" ||
            data.file_type == "image/png" ||
            data.file_type == "text/html" ||
            data.file_type == "video/mp3" ||
            data.file_type == "video/mp4" ||
            data.file_type == "audio/mp4" ||
            data.file_type == "audio/wav" ||
            data.file_type == "audio/x-wav" ||
            data.file_type == "audio/mpeg" ||
            data.file_type == "audio/mp3" ||
            data.file_type == "image/webp"
        ) {
            var url = data.url.replace("/downloadFile?id=", "/viewonline?id=");
            this.srcfile = "https://" + window.location.host + "" + url;


            //this.srcfile = 'https://view.officeapps.live.com/op/view.aspx?src=' + encodeURI("https://" + window.location.host + "" + data.url);
        } else {
            //var url = data.url.replace("/downloadFile?id=", "/viewonline?id=");
            this.srcfile = 'https://view.officeapps.live.com/op/view.aspx?src=' + encodeURI("https://" + window.location.host + "" + data.url);
        }

        this.innerHeight = window.innerHeight;
        //  this.loadInfo();

    }
    public innerHeight: any;
    public formatSizeUnits(bytes) {
        if (bytes >= 1073741824) { bytes = (bytes / 1073741824).toFixed(2) + " GB"; }
        else if (bytes >= 1048576) { bytes = (bytes / 1048576).toFixed(2) + " MB"; }
        else if (bytes >= 1024) { bytes = (bytes / 1024).toFixed(2) + " KB"; }
        else if (bytes > 1) { bytes = bytes + " bytes"; }
        else if (bytes == 1) { bytes = bytes + " byte"; }
        else { bytes = "0 bytes"; }
        return bytes;
    }
    @HostListener('window:resize', ['$event'])
    onResize(event) {
        this.innerHeight = window.innerHeight;
    }

    getFontAwesomeIconFromMIME(mimeType) {
        // List of official MIME Types: http://www.iana.org/assignments/media-types/media-types.xhtml
       
        for (var key in this.icon_classes) {
            if (this.icon_classes.hasOwnProperty(key)) {
                if (mimeType.search(key) === 0) {
                    // Found it
                    return this.icon_classes[key];
                }
            } else {
                return "assets/icon_file_type/file.png";
            }
        }
    }
    close(): void {
        this.dialogRef.close(this.record);
    }
    showInfoDetails(value) {

        this.show_details = value;
    }
    downloadFile(file) {
        const params = new HttpParams()
            .set('id', file.id)
            .set('id_file', file.id_file)
            .set('file_name', file.file_name)
            ;

            
        //let uri = file.controller + '.ctr/downloadFile';
        let uri = 'FileManager/downloadFile';
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
                a.download = file.file_name;
                a.click();
                document.body.removeChild(a);
            })
    }

    trustfile(url) {
        this.sanitizer.bypassSecurityTrustResourceUrl(url);
    }
}

