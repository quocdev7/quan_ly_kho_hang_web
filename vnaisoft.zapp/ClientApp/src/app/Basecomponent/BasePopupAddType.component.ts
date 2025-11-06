import { Component, Directive, Inject, QueryList, ViewChildren } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { ActivatedRoute } from '@angular/router';
import Swal from 'sweetalert2';
import { MatDialog } from '@angular/material/dialog';

import { TranslocoService } from '@ngneat/transloco';
import { FuseNavigationService } from '@fuse/components/navigation';
// import { doc_tailieu_view_file_onlineComponent } from 'app/modules/system/sys_user/viewfileonline.component';
import { DataTableDirective } from 'angular-datatables';

import { v4 as uuidv4 } from 'uuid';
declare var MathJax: any;
@Directive()
export abstract class BasePopUpAddTypeComponent<T> {
 public plugintiny = [
        "advlist autolink lists link image charmap print preview anchor",
        "searchreplace visualblocks code fullscreen",
        "insertdatetime media table paste imagetools wordcount"
    ];
    public toolbartiny = "insertfile undo redo | styleselect | bold italic | alignleft aligncenter alignright alignjustify | bullist numlist outdent indent | link image";
    public Oldrecord: T;
    public record: T;
    public errorModel: any;
    public actionEnum: number;
    public baseurl: String;
    public pageLoading: Boolean = false;
    public loading: boolean = false;
    public timyconfig: any;
    public timyconfigoption: any;
    ngOnInit(): void {

    }
    constructor(
        public _translocoService: TranslocoService,
        public _fuseNavigationService: FuseNavigationService,
        public route: ActivatedRoute,
        _baseUrl: string,
        public http: HttpClient,
        public controller: String,
        public basedialogRef: any,
        public dialogModal: any,
    ) {
        this.errorModel = [];
        this.baseurl = _baseUrl;
    }

    public showLoading(title: any, html: any, showClose: boolean) {
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
    public beforesave(): void {


    }
    public aftersavefail(): void {


    }
    public aftersave(): void {


    }
    public baseInitData(): void {
        this.save(true);
    };
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

    public getFontAwesomeIconFromMIME(mimeType) {
        // List of official MIME Types: http://www.iana.org/assignments/media-types/media-types.xhtml
        var icon_classes = {
            // Media
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
    //     var dialogRef = this.dialogModal.open(
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
    save(first_submit): void {
        this.beforesave();
        this.loading = true;
        if (this.actionEnum == 1) {
            this.http
                .post(this.controller + '.ctr/create/',
                    {
                        data: this.record,
                    }
                ).subscribe(resp => {
                    this.record = resp as T;
                    this.Oldrecord = this.record;
                    this.basedialogRef.close(this.record);
                    Swal.fire('Lưu thành công', '', 'success');
                    this.aftersave();
                    this.actionEnum = 3;
                    this.loading = false;
                    this.errorModel = []
                },
                    error => {
                       
                        if (error.status == 400) {
                            this.errorModel = error.error;
                            this.aftersavefail();
                            if (first_submit == true) {

                            } else {
                                Swal.fire(this._translocoService.translate('thong_tin_nhap_lieu_khong_dung_vui_long_kiem_tra_lai'), "", "warning");
                            }


                        }
                        if (error.status == 403) {
                            this.basedialogRef.close();
                            Swal.fire(this._translocoService.translate('no_permission'), "", "warning");
                        }
                        this.loading = false;

                    }
                );
        } else if (this.actionEnum == 2) {
            this.http
                .post(this.controller + '.ctr/edit/',
                    {
                        data: this.record,
                    }
                ).subscribe(resp => {
                    this.record = resp as T;
                    this.Oldrecord = this.record;
                     this.basedialogRef.close(this.record);
                    Swal.fire('Lưu thành công', '', 'success');
                    this.aftersave();
                    this.actionEnum = 3;
                    this.loading = false;
                    this.errorModel = []
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
        } else if (this.actionEnum == 4) {
            this.http
                .post(this.controller + '.ctr/copy/',
                    {
                        data: this.record,
                    }
                ).subscribe(resp => {
                    this.record = resp as T;
                    this.basedialogRef.close(this.record);
                    this.aftersave();
                },
                    error => {
                        if (error.status == 400) {
                            this.errorModel = error.error;

                        }
                        if (error.status == 403) {
                            Swal.fire(this._translocoService.translate('no_permission'), "", "warning");
                        }
                        this.loading = false;

                    });
        }
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
      load_config_editor_option(controller,height): void {
        this.timyconfigoption = {
            base_url: '/tinymce'
            , suffix: '.min',
            height: height,
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
     updateMathJax() {
        setTimeout(() => {
            if (window['MathJax']) {
                MathJax.typesetPromise();
            }
        }, 100);
    } 
    close(): void {
        if (this.actionEnum == 3) {
            this.basedialogRef.close(this.record);
        } else {
            this.basedialogRef.close(this.Oldrecord);
        }

    }


}
