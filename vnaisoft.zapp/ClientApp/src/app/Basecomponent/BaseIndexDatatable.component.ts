import { HttpClient } from '@angular/common/http';

import { MatDialog } from '@angular/material/dialog';
import { Component, ViewChild, Inject, Directive, OnDestroy, QueryList, ViewChildren } from '@angular/core';
import { DataTableDirective } from 'angular-datatables';
import { Subject } from 'rxjs';
import Swal from 'sweetalert2';


import { ActivatedRoute } from '@angular/router';
import { FuseNavigationService } from '@fuse/components/navigation';
import { TranslocoService } from '@ngneat/transloco';

import { DataTablesResponse } from 'app/Basecomponent/datatable';
// import { doc_tailieu_view_file_onlineComponent } from 'app/modules/system/sys_user/viewfileonline.component';
import { clear } from 'console';
import { translateDataTable } from '@fuse/components/commonComponent/VietNameseDataTable';
import moment from 'moment';

import { v4 as uuidv4 } from 'uuid';
declare var MathJax: any;
@Directive()
export abstract class BaseIndexDatatableComponent implements OnDestroy {

    public action: string;
    public controller: String;
    public filter: any;
    public table: any;
    @ViewChildren(DataTableDirective) dtElements: QueryList<DataTableDirective>;
    public pageLoading: Boolean = false;
    public dtOptions: DataTables.Settings = {};
    public currentIndex: number;
    public listData: any = [];
    public total: any = [];
    public baseurl: String;
     public plugintiny = [
        "advlist autolink lists link image charmap print preview anchor",
        "searchreplace visualblocks code fullscreen",
        "insertdatetime media table paste imagetools wordcount"
    ];
    public toolbartiny = "insertfile undo redo | styleselect | bold italic | alignleft aligncenter alignright alignjustify | bullist numlist outdent indent | link image";
     public timyconfig: any;
    constructor(public http: HttpClient,
        _baseUrl: string,
        public _translocoService: TranslocoService,
        public _fuseNavigationService: FuseNavigationService,
        public route: ActivatedRoute,
        public dialog: MatDialog, _controller: String, _filter: any) {
        this.controller = _controller;
        this.baseurl = _baseUrl;
        this.filter = _filter;
        this.pageLoading = false;
        $(document).on('click', '.mat-focus-indicator.mat-icon-button.mat-button-base, .mat-tab-label',
            () => setTimeout(() => {
                if (this.dtElements.length > 0) {
                    this.dtElements.forEach((dtElement: DataTableDirective) => {
                        dtElement.dtInstance.then((dtInstance: DataTables.Api) => {
                            dtInstance.columns.adjust();
                        });
                    });
                }
            }
                , 500)
        )
    }
    ngOnDestroy(): void {

    }
  updateMathJax() {
        setTimeout(() => {
            if (window['MathJax']) {
                MathJax.typesetPromise();
            }
        }, 100);
    } 
    load_config_editor(controller): void {
        this.timyconfig = {
            base_url: '/tinymce'
            , suffix: '.min',
            height: 500,
            file_picker_callback: function (cb, value, meta) {
              
                var input = document.createElement('input');

         
                var type = "editor";
                input.setAttribute('type', 'file');
                
                if (meta.filetype === 'image') {
                input.setAttribute('accept', 'image/*');
                } else if (meta.filetype === 'media') {
                    // Đã cập nhật: Chấp nhận bất kỳ video/audio nào, và các phần mở rộng cụ thể
                    input.setAttribute('accept', 'video/*,audio/*,.mp4,.mov,.avi');
                } else if (meta.filetype === 'file') {
                    // Hỗ trợ nút "insert/edit file"
                    input.setAttribute('accept', '*/*');
                }
                input.onchange = function () {
                    var file = input.files[0];
                     if (!file) {
                    return;
                    }
                 

                
                 // KHÔNG CẦN FileReader để tải lên.
                // Chúng ta sẽ sử dụng trực tiếp đối tượng 'file' gốc.

                var fd = new FormData();
                var type = "editor"; // Hoặc bất kỳ thư mục nào bạn muốn

                // Sử dụng một UUID duy nhất cho mỗi lần tải lên nếu cần
                fd.append('id', uuidv4());
                fd.append('filetype', meta.filetype);

                // GIỮ LẠI TÊN TỆP GỐC VÀ LOẠI TỆP
                // Đây là thay đổi quan trọng nhất.
                fd.append("file", file, file.name);

                fd.append("folder", type);
                fd.append('controller', controller);

                // AJAX
                var xhr = new XMLHttpRequest();
                xhr.withCredentials = false;
                xhr.open('POST', '/FileManager/upload_file_common');

                        xhr.onload = function () {
                            if (xhr.status !== 200) {
                                alert('Lỗi HTTP: ' + xhr.status);
                                console.error('Upload failed with status', xhr.status, xhr.responseText);
                                return;
                            }
                             try {
                        var json = JSON.parse(xhr.responseText);

                        // TinyMCE thường mong đợi một thuộc tính 'location' chứa URL của tệp.
                        // Chúng ta kiểm tra cả 'location' và 'file_path' để linh hoạt hơn.
                        var fileUrl = json.location || json.file_path;

                        if (!fileUrl || typeof fileUrl != 'string') {
                            alert('Phản hồi JSON không hợp lệ: ' + xhr.responseText);
                            console.error('Invalid JSON response', xhr.responseText);
                            return;
                        }

                        // GỌI TRỰC TIẾP CALLBACK với URL trả về từ server.
                        // Cung cấp 'title' để hiển thị tên tệp gốc trong giao diện người dùng.
                        cb(fileUrl, { title: file.name });

                    } catch (e) {
                        alert('Lỗi xử lý JSON: ' + xhr.responseText);
                        console.error('Error parsing JSON', e, xhr.responseText);
                    }
                        };
                           xhr.onerror = function () {
                    alert('Lỗi mạng khi đang tải tệp lên.');
                };
                        xhr.send(fd);
                        return
                        //imageCompress.compressFile(reader.result.toString(), 1, 50, 50, 1000, 1000) // 50% ratio, 30% quality
                        //    .then((compressedImage: DataUrl) => {
                        //        imgResultAfterCompression = compressedImage;


                        //    });

                    

                };

                input.click();
            },
            plugins: this.plugintiny,
            toolbar: this.toolbartiny,
            extended_valid_elements: 'mathjax[*]',
            content_style: 'body { font-size: 18px; }',
            setup: (editor: any) => {
                editor.on('init', () => {
                    this.updateMathJax();
                });
                editor.on('change', () => {
                    this.updateMathJax();
                });
            }
        };
    }
    public removeAccents(str) {
        var AccentsMap = [
            "aàảãáạăằẳẵắặâầẩẫấậ",
            "AÀẢÃÁẠĂẰẲẴẮẶÂẦẨẪẤẬ",
            "dđ", "DĐ",
            "eèẻẽéẹêềểễếệ",
            "EÈẺẼÉẸÊỀỂỄẾỆ",
            "iìỉĩíị",
            "IÌỈĨÍỊ",
            "oòỏõóọôồổỗốộơờởỡớợ",
            "OÒỎÕÓỌÔỒỔỖỐỘƠỜỞỠỚỢ",
            "uùủũúụưừửữứự",
            "UÙỦŨÚỤƯỪỬỮỨỰ",
            "yỳỷỹýỵ",
            "YỲỶỸÝỴ"
        ];
        for (var i = 0; i < AccentsMap.length; i++) {
            var re = new RegExp('[' + AccentsMap[i].substr(1) + ']', 'g');
            var char = AccentsMap[i][0];
            str = str.replace(re, char);
        }

        return str;
    }
    public rerender(): void {
        this.handleDataBefore();
        this.dtElements.forEach((dtElement: DataTableDirective) => {
            dtElement.dtInstance.then((dtInstance: DataTables.Api) => {
                dtInstance.ajax.reload(null, true);
            });
        });
        // Destroy the table first
    };
    public getStartOfMonth(date: Date): Date {
        var month = date.getMonth();
        var year = date.getFullYear();
        return new Date(year, month, 1)
    }
    public getEndOfMonth(date: Date): Date {
        var month = date.getMonth();
        var year = date.getFullYear();
        if (month == 12) {
            return new Date(year, month, 31)
        }
        return new Date(year, month + 1, 0)
    }
    public showLoading(title: any = "", html: any = "", showClose: boolean = false) {
        if (title == "")
            title = this._translocoService.translate('system.vui_long_doi')
        if (html == "")
            html = this._translocoService.translate('system.dang_tai_du_lieu')
        Swal.fire({
            title: title,
            html: html,
            allowOutsideClick: false,
            showCloseButton: showClose,
            willOpen: () => {
                Swal.showLoading()
            }
        })
    }

    public getFontAwesomeIconFromMIME(mimeType) {
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
    public formatSizeUnits(bytes) {
        if (bytes >= 1073741824) { bytes = (bytes / 1073741824).toFixed(2) + " GB"; }
        else if (bytes >= 1048576) { bytes = (bytes / 1048576).toFixed(2) + " MB"; }
        else if (bytes >= 1024) { bytes = (bytes / 1024).toFixed(2) + " KB"; }
        else if (bytes > 1) { bytes = bytes + " bytes"; }
        else if (bytes == 1) { bytes = bytes + " byte"; }
        else { bytes = "0 bytes"; }
        return bytes;
    }
    // openDialogViewFileOnline(url_download, file_name, file_type, file_size, url_view_online): void {
    //     var dialogRef = this.dialog.open(
    //         doc_tailieu_view_file_onlineComponent,
    //         {
    //             disableClose: true,
    //             panelClass: ['full-screen-modal'],
    //             data: {
    //                 url: url_view_online,
    //                 url_download: url_download,
    //                 file_name: file_name,
    //                 file_type: file_type,
    //                 file_size: file_size,
    //             },
    //         }
    //     );
    //     dialogRef.afterClosed().subscribe((result) => { });
    // }
    public rerenderfilter(): void {
        this.before_filter();
        this.dtElements.forEach((dtElement: DataTableDirective) => {
            dtElement.dtInstance.then((dtInstance: DataTables.Api) => {
                dtInstance.ajax.reload(null, true);
            });
        });
    }
    public before_filter(): void {

    }
    public showMessagewarning(msg): void {
        Swal.fire({
            title: msg,
            text: "",
            icon: 'warning',
            confirmButtonColor: '#3085d6',
            confirmButtonText: this._translocoService.translate('close'),
        }).then((result) => {

        })

    }
    public showMessagewarning2(title, msg): void {
        Swal.fire({
            title: title,
            text: msg,
            icon: 'warning',
            confirmButtonColor: '#3085d6',
            confirmButtonText: this._translocoService.translate('close'),
        }).then((result) => {

        })

    }


    public showMessageSuccess(msg): void {
        Swal.fire({
            title: msg,
            text: "",
            icon: 'success',
            confirmButtonColor: '#3085d6',
            confirmButtonText: this._translocoService.translate('close'),
        }).then((result) => {

        })

    }

    revertStatus(model, status_del): void {
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
                    .post(this.controller + '.ctr/update_status_del/',
                        {
                            id: model.db.id,
                            status_del: status_del

                        }
                    ).subscribe(resp => {
                        Swal.fire('Thành công', '', 'success');
                        this.rerender();
                    },
                        error => {
                            if (error.status == 403) {
                                Swal.fire(this._translocoService.translate('no_permission'), "", "warning");
                            }
                        });
            }
        })
    }



    // revertStatus(model, status_del): void {
    //     Swal.fire({
    //         title: this._translocoService.translate('areYouSure'),
    //         text: "",
    //         icon: 'warning',
    //         showCancelButton: true,
    //         confirmButtonColor: '#3085d6',
    //         cancelButtonColor: '#d33',
    //         confirmButtonText: this._translocoService.translate('yes'),
    //         cancelButtonText: this._translocoService.translate('no')
    //     }).then((result) => {
    //         if (result.value) {
    //             this.http
    //                 .post(this.controller + '.ctr/revert/',
    //                     {
    //                         id: model.db.id,
    //                         status_del: status_del

    //                     }
    //                 ).subscribe(resp => {
    //                     Swal.fire('Thành công', '', 'success');
    //                     this.rerender();
    //                 },
    //                     error => {
    //                         if (error.status == 403) {
    //                             Swal.fire(this._translocoService.translate('no_permission'), "", "warning");
    //                         }
    //                     });
    //         }
    //     })


    // }
    public delete(id1): void {
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
                    .post(this.controller + '.ctr/delete/',
                        {
                            id: id1,
                        }
                    ).subscribe(resp => {
                        Swal.fire('Ngưng sử dụng thành công', '', 'success');
                        this.rerender();
                    },
                        error => {
                            if (error.status == 403) {
                                Swal.fire(this._translocoService.translate('no_permission'), "", "warning");
                            }


                        }
                    );
            }
        })

    }
    public delete_new(id1): void {
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
                    .post(this.controller + '.ctr/delete/',
                        {
                            id: id1,
                        }
                    ).subscribe(resp => {
                        Swal.fire('Xóa thành công', '', 'success');
                        this.rerender();
                    },
                        error => {
                            if (error.status == 403) {
                                Swal.fire(this._translocoService.translate('no_permission'), "", "warning");
                            }


                        }
                    );
            }
        })

    }

    public close(id1): void {
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
                    .post(this.controller + '.ctr/close/',
                        {
                            id: id1,
                        }
                    ).subscribe(resp => {
                        this.rerender();
                    });
            }
        })

    }

    public baseInitData(): void {

        this.handleDataBefore();
        const that = this;
        this.dtOptions = {
            language: {
                processing: "Đang xử lý...",
                lengthMenu: "Xem _MENU_ dòng",
                zeroRecords: "Không tìm thấy dòng nào phù hợp",
                info: "Đang xem _START_ đến _END_ trong tổng số _TOTAL_ dòng",
                infoEmpty: "Đang xem 0 đến 0 trong tổng số 0 dòng",
                infoFiltered: "(được lọc từ _MAX_ mục)",
                search: "Tìm:",
                paginate: {
                    first: "Đầu",
                    previous: "Trước",
                    next: "Tiếp",
                    last: "Cuối"
                }
            },

            scrollCollapse: true,
            retrieve: true,
            scrollX: true,
            responsive: true,
            dom: '<"top"pli>rt<"bottom"p><"clear">',
            ordering: false,
            serverSide: true,
            processing: true,
            lengthMenu: [25, 50, 100,"Tất cả"],
            "drawCallback": function (settings) {
                var api = this.api();
                that.table = api;
                setTimeout(function () { api.columns.adjust(); }, 300);
                $('tbody').on('click', 'tr', function () {
                    if ($(this).hasClass('selected')) {
                        $(this).removeClass('selected');
                    }
                    else {
                        $('tr.selected').removeClass('selected');
                        $(this).addClass('selected');
                    }
                });

            },

            "searching": true,
            ajax: (data, callback, settings) => {
                this.pageLoading = true;
                this.http
                    .post<DataTablesResponse>(this.baseurl + '' + this.controller + '.ctr/DataHandler/',
                        {
                            param1: data,
                            data: this.filter
                        }
                    ).subscribe(resp => {
                        var data: any;
                        data = resp;
                        that.listData = resp.data;
                        this.pageLoading = false;
                        that.total = data.total;
                        that.currentIndex = resp.start;
                        callback({
                            recordsTotal: resp.recordsTotal,
                            recordsFiltered: resp.recordsFiltered,
                            data: []
                        });
                    });
            },

        };



    }

    public baseInitDataParam(): void {

        this.handleDataBefore();
        const that = this;
        this.dtOptions = {
            language: {
                processing: "Đang xử lý...",
                lengthMenu: "Xem _MENU_ dòng",
                zeroRecords: "Không tìm thấy dòng nào phù hợp",
                info: "Đang xem _START_ đến _END_ trong tổng số _TOTAL_ dòng",
                infoEmpty: "Đang xem 0 đến 0 trong tổng số 0 dòng",
                infoFiltered: "(được lọc từ _MAX_ mục)",
                search: "Tìm:",
                paginate: {
                    first: "Đầu",
                    previous: "Trước",
                    next: "Tiếp",
                    last: "Cuối"
                }
            },

            scrollCollapse: true,
            retrieve: true,
            scrollX: true,
            responsive: true,
            dom: '<"top"pli>rt<"bottom"p><"clear">',
            ordering: false,
            serverSide: true,
            processing: true,
            lengthMenu: [50, 75, 100],
            "drawCallback": function (settings) {
                var api = this.api();
                that.table = api;
                setTimeout(function () { api.columns.adjust(); }, 300);
                $('tbody').on('click', 'tr', function () {
                    if ($(this).hasClass('selected')) {
                        $(this).removeClass('selected');
                    }
                    else {
                        $('tr.selected').removeClass('selected');
                        $(this).addClass('selected');
                    }
                });

            },

            "searching": false,
            ajax: (data, callback, settings) => {
                this.pageLoading = true;

                this.http
                    .post<DataTablesResponse>(this.baseurl + '' + this.controller + '.ctr/DataHandler/',
                        {
                            param1: data,
                            data: this.filter
                        }
                    ).subscribe(resp => {
                        var data: any;
                        data = resp;
                        that.listData = data.data;
                        that.total = data.total;
                        this.pageLoading = false;
                        that.currentIndex = resp.start;
                        callback({
                            recordsTotal: resp.recordsTotal,
                            recordsFiltered: resp.recordsFiltered,
                            data: []
                        });
                    });
            },

        };



    }

    public handleDataBefore(): void {


    }




}
