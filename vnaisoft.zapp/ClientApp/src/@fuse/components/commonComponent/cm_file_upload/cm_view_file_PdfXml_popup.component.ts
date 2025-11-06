import { Component, HostListener, Inject } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef, MatDialog } from '@angular/material/dialog';


import { HttpClient, HttpEventType, HttpParams } from '@angular/common/http';
import { BasePopUpAddComponent } from 'app/Basecomponent/BasePopupAdd.component';
import { TranslocoService } from '@ngneat/transloco';
import { FuseNavigationService } from '@fuse/components/navigation';
import { ActivatedRoute } from '@angular/router';
import { translateDataTable } from '@fuse/components/commonComponent/VietNameseDataTable';

import Swal from 'sweetalert2';


@Component({
    selector: 'cm_view_file_PdfXml_popup',
    templateUrl: 'cm_view_file_PdfXml_popup.component.html',
})
export class cm_view_file_PdfXml_popupComponent extends BasePopUpAddComponent {
    public showSignButton: boolean = false;
    public list_nam_nhap_hoc: any = [];

    public file_image: any
    public Progress_image: any = -1;
    public Progress_cover_image: any = -1;

    public show_details: any = false;
    public show_details_xml: any = false;
    public file: any;
    public file_xml: any;
    public innerHeight: any;
    public srcfile: any = "";
    public srcfile_xml: any = "";
    public list_giao_vien: any;
    hocSinhList = [
    {
      ho_ten: 'Nguyễn Đức Phong',
      gioi_tinh: 'Nam',
      ngay_sinh: '13/08/2009',
      noi_sinh: 'Đà Nẵng',
      dan_toc: 'Kinh - Con liệt sĩ',
      cho_o: 'Khu phố 3, Q.Thủ Đức',
      ho_ten_bo: 'Nguyễn Thế D',
      ho_ten_me: 'Nguyễn Thị M',
    },
    {
      ho_ten: 'Hiếu Thúy Hai',
      gioi_tinh: 'Nữ',
      ngay_sinh: '06/10/2009',
      noi_sinh: 'Hà Nội',
      dan_toc: 'Kinh',
      cho_o: 'Q. Bình Ngô, TP.HCM',
      ho_ten_bo: 'Nguyễn Văn T',
      ho_ten_me: 'Nguyễn Thị A',
    },
    // Thêm các học sinh khác...
  ];
    
    constructor(public dialogRef: MatDialogRef<cm_view_file_PdfXml_popupComponent>,
        http: HttpClient, _translocoService: TranslocoService,
        _fuseNavigationService: FuseNavigationService, route: ActivatedRoute, public dialog: MatDialog,
        @Inject('BASE_URL') baseUrl: string,
        public dialogModal: MatDialog,
        @Inject(MAT_DIALOG_DATA) data: any) {
        super(_translocoService, _fuseNavigationService, route, baseUrl, http, 'cm_view_file_PdfXml_popup', dialogRef, dialogModal);
        this.showSignButton = data.showSignButton || false;
        this.record = data.model;
        this.Oldrecord = JSON.parse(JSON.stringify(data));
        this.actionEnum = data.model.actionEnum;
        if (this.actionEnum == 1) {

        }else {
        }
       this.file = data.data;
       var url = data.data.url.replace("/downloadFile?id=", "/viewonline?id=");
        if (data.data.file_type == "text/plain" ||
            data.data.file_type == "text/html" ||
            data.data.file_type == "application/json" ||
            data.data.file_type == "application/pdf" ||
            data.data.file_type == "image/jpeg" ||
            data.data.file_type == "image/png" ||
            data.data.file_type == "text/html" ||
            data.data.file_type == "video/mp3" ||
            data.data.file_type == "video/mp4"
        ) {
            var url = data.data.url.replace("/downloadFile?id=", "/viewonline?id=");
            this.srcfile = "https://" + window.location.host + "" + url;


            //this.srcfile = 'https://view.officeapps.live.com/op/view.aspx?src=' + encodeURI("https://" + window.location.host + "" + data.url);
        } else {
            //var url = data.url.replace("/downloadFile?id=", "/viewonline?id=");
            this.srcfile = 'https://view.officeapps.live.com/op/view.aspx?src=' + encodeURI("https://" + window.location.host + "" + data.data.url);
        }
       this.innerHeight = window.innerHeight;


       this.file_xml = data.data_file_xml;
       //var url = data.data_file_xml.url.replace("/downloadFile?id=", "/viewonline?id=");
        // if (data.data_file_xml.file_type == "text/plain" ||
        //     data.data_file_xml.file_type == "text/html" ||
        //     data.data_file_xml.file_type == "application/json" ||
        //     data.data_file_xml.file_type == "application/pdf" ||
        //     data.data_file_xml.file_type == "image/jpeg" ||
        //     data.data_file_xml.file_type == "image/png" ||
        //     data.data_file_xml.file_type == "text/html" ||
        //     data.data_file_xml.file_type == "video/mp3" ||
        //     data.data_file_xml.file_type == "video/mp4"
        // ) {
        //     var url = data.data_file_xml.url.replace("/downloadFile?id=", "/viewonline?id=");
        //     this.srcfile_xml = "https://" + window.location.host + "" + url;


        //     //this.srcfile = 'https://view.officeapps.live.com/op/view.aspx?src=' + encodeURI("https://" + window.location.host + "" + data.url);
        // } else {
        //     //var url = data.url.replace("/downloadFile?id=", "/viewonline?id=");
        //     this.srcfile_xml = 'https://view.officeapps.live.com/op/view.aspx?src=' + encodeURI("https://" + window.location.host + "" + data.data_file_xml.url);
        // }
        var url_xml = data.data_file_xml.url.replace("/downloadFile?id=", "/viewonline?id=");
        this.srcfile_xml = "https://" + window.location.host + "" + url_xml;
       this.innerHeight = window.innerHeight;
       this.loadXmlFromUrl(this.srcfile_xml);
    }
    formattedXml: string = '';

    loadXmlFromUrl(xmlUrl: string) {
        this.http.get(xmlUrl, { responseType: 'text' }).subscribe(xml => {
            const formatted = this.formatXml(xml);
            this.formattedXml = this.escapeHtml(formatted);
        });
    }
    formatXml(xml: string): string {
        const PADDING = '  ';
        const reg = /(>)(<)(\/*)/g;
        let formatted = '';
        let pad = 0;

        xml = xml.replace(reg, '$1\r\n$2$3');
        xml.split('\r\n').forEach((node) => {
            let indent = 0;
            if (node.match(/.+<\/\w[^>]*>$/)) {
                indent = 0;
            } else if (node.match(/^<\/\w/)) {
                if (pad !== 0) pad -= 1;
            } else if (node.match(/^<\w([^>]*[^\/])?>.*$/)) {
                indent = 1;
            }

            const padding = PADDING.repeat(pad);
            formatted += padding + node + '\r\n';
            pad += indent;
        });

        return formatted.trim();
    }

    escapeHtml(xml: string): string {
        return xml.replace(/&/g, '&amp;')
                .replace(/</g, '&lt;')
                .replace(/>/g, '&gt;');
    }
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
    showInfoDetails(value) {
        this.show_details = value;
    }
    showInfoDetailsXml(value) {
        this.show_details_xml = value;
    }
    downloadFile(file) {
        const params = new HttpParams()
            .set('id', file.id_file)
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
    dowloadFileXml(file) {
        const params = new HttpParams()
            .set('id', file.id_file)
            .set('file_name', file.file_name)
            ;

            
        //let uri = file.controller + '.ctr/downloadFile';
        let uri = 'FileManager/downloadFileXml';
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
    
}
