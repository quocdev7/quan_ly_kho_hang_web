import { Component, Directive, Inject, QueryList, ViewChildren } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { ActivatedRoute, RouterLink } from '@angular/router';
import Swal from 'sweetalert2';
import { MatDialog } from '@angular/material/dialog';

import { TranslocoService } from '@ngneat/transloco';
import { FuseNavigationService } from '@fuse/components/navigation';
// import { doc_tailieu_view_file_onlineComponent } from 'app/modules/system/sys_user/viewfileonline.component';
import { DataTableDirective } from 'angular-datatables';
import { v4 as uuidv4 } from 'uuid';
declare var MathJax: any;
@Directive()
export abstract class BasePopUpAddComponent {
    public plugintiny = [
        "advlist autolink lists link image charmap print preview anchor",
        "searchreplace visualblocks code fullscreen",
        "insertdatetime media table paste imagetools wordcount"
    ];
    public toolbartiny = "insertfile undo redo | styleselect | bold italic | alignleft aligncenter alignright alignjustify | bullist numlist outdent indent | link image";
    public Oldrecord: any;
    public record: any;
    public errorModel: any;
    public actionEnum: number;
    public baseurl: String;
    public pageLoading: Boolean = false;
    public loading: boolean = false;
    public timyconfigoption: any;
    private readonly _keyStr = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789+/=";

    private readonly apiUrls = "https://127.0.0.1:14424/process";
    public process = false;
    public checkOCSP = 0;
     public xml_ky  :any;

    ngOnInit(): void {

    }
    public timyconfig: any;
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
    updateMathJax() {
        setTimeout(() => {
            if (window['MathJax']) {
                MathJax.typesetPromise();
            }
        }, 100);
    }
    ki_so_common(xml, xml_tag, sessionId): void {
        if (this.process == true)
            return;


        if (xml == "") {
            alert("Please enter xmlData to sign");
            return;
        }
        var algDigest = "SHA_256";
        var urlTimestamp = "http://time.certum.pl";
        var DataToBeSign = this.encodeString(xml);
        var xmlDSig = 0;
        var xades_Version = "XADES_v1_4_1";
        var xades_Form = "XADES_T";
        const payload = {
            OperationId: 2,
            SessionId: sessionId,
            checkOCSP: this.checkOCSP,
            algDigest: algDigest,
            urlTimestamp: urlTimestamp,
            DataToBeSign: DataToBeSign,
            tagXML: xml_tag,
            XMLDSig: xmlDSig,
            xades_Version: xades_Version,
            xades_Form: xades_Form
        };
        var json_req = this.encodeString(JSON.stringify(payload));
        console.log(json_req);
        var httpReq;
        var response = "";
        if (window.XMLHttpRequest) {// code for IE7+, Firefox, Chrome, Opera, Safari
            httpReq = new XMLHttpRequest();
        }
        else {// code for IE6, IE5
            // httpReq = new ActiveXObject("Microsoft.XMLHTTP");
        }
        httpReq.onreadystatechange = () => { // <--- THAY ĐỔI Ở ĐÂY
            if (httpReq.readyState == 4 && httpReq.status == 200) {
                // Gọi 'this.decodeString' sẽ hoạt động
                const response = this.decodeString(httpReq.responseText);
                this.process = false;
                try {
                    var json_res = JSON.parse(response);
                    if (json_res.ResponseCode == 0) {
                        this.xml_ky = this.decodeString(json_res.Base64Result);

                        this.record.xml_ky = this.xml_ky;
                    } else {
                        this.xml_ky = "";
                        alert(json_res.ResponseMsg);
                    }
                }
                catch (err) {
                    alert("Error: " + err.message);
                }
            } else if (httpReq.readyState == 4 && httpReq.status != 200) {
                console.error("Yêu cầu thất bại với mã trạng thái:", httpReq.status);
            }

        }
        httpReq.open("POST", this.apiUrls + "process", true);
        httpReq.setRequestHeader("Content-type", "application/x-www-form-urlencoded");
        httpReq.send("request=" + json_req);

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
    load_config_editor_option(controller, height): void {
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

    public showMessageSuccessNew(title): void {
        Swal.fire({
            html:
                '<div class="justify-center items-center md:flex rounded-t-lg  flex-auto background_success">'
                + '<img src="../assets/images/portal/success.jpg" class="img_sweetalert">'
                + '</div>'
                + '<div class="mt-4 text-center text-3xl uppercase text-black font-extrabold tracking-tight leading-tight">'
                + title
                + '</div>',
            confirmButtonColor: '#3085d6',
            confirmButtonText: this._translocoService.translate('close'),
        }).then(resp => {

        })
    }
    public showMessageWarningNew(title): void {
        //#CC6331 background color
        Swal.fire({
            html:
                '<div class="justify-center items-center md:flex rounded-t-lg  flex-auto background_success">'
                + '<img src="../assets/images/portal/warning.png" class="img_sweetalert">'
                + '</div>'
                + '<div class="mt-4 text-center text-3xl uppercase text-black font-extrabold tracking-tight leading-tight">'
                + title
                + '</div>',
            confirmButtonColor: '#3085d6',
            confirmButtonText: this._translocoService.translate('close'),
        }).then(resp => {

        })
    }
    public showMessageErrorNew(title): void {
        //#CC6331 background color
        Swal.fire({
            html:
                '<div class="justify-center items-center md:flex rounded-t-lg  flex-auto background_error">'
                + '<img src="../assets/images/portal/error4.jpg" class="img_sweetalert">'
                + '</div>'
                + '<div class="mt-4 text-center text-3xl  uppercase  text-black font-extrabold tracking-tight leading-tight">'
                + 'Thất bại'
                + '</div>'
                + '<div class="mt-4 text-center text-xl text-black">'
                + title
                + '</div>',
            //showCancelButton: true,
            confirmButtonColor: '#dc3545',
            // cancelButtonColor: '#d33',
            confirmButtonText: 'Truy cập Workplace'
        }).then(resp => {

        })
    }
    public showMessageInfoNew(title): void {
        //#CC6331 background color
        Swal.fire({
            html:
                '<div class="justify-center items-center md:flex rounded-t-lg  flex-auto background_error">'
                + '<img src="../assets/images/portal/error4.jpg" class="img_sweetalert">'
                + '</div>'
                + '<div class="mt-4 text-center text-3xl  uppercase  text-black font-extrabold tracking-tight leading-tight">'
                + title
                + '</div>',
            //showCancelButton: true,
            confirmButtonColor: '#dc3545',
            // cancelButtonColor: '#d33',
            confirmButtonText: 'Truy cập Workplace'
        }).then(resp => {

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
        this.save();
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
    removeVietnameseTones(str) {
        str = str.replace(/à|á|ạ|ả|ã|â|ầ|ấ|ậ|ẩ|ẫ|ă|ằ|ắ|ặ|ẳ|ẵ/g, "a");
        str = str.replace(/è|é|ẹ|ẻ|ẽ|ê|ề|ế|ệ|ể|ễ/g, "e");
        str = str.replace(/ì|í|ị|ỉ|ĩ/g, "i");
        str = str.replace(/ò|ó|ọ|ỏ|õ|ô|ồ|ố|ộ|ổ|ỗ|ơ|ờ|ớ|ợ|ở|ỡ/g, "o");
        str = str.replace(/ù|ú|ụ|ủ|ũ|ư|ừ|ứ|ự|ử|ữ/g, "u");
        str = str.replace(/ỳ|ý|ỵ|ỷ|ỹ/g, "y");
        str = str.replace(/đ/g, "d");
        str = str.replace(/À|Á|Ạ|Ả|Ã|Â|Ầ|Ấ|Ậ|Ẩ|Ẫ|Ă|Ằ|Ắ|Ặ|Ẳ|Ẵ/g, "a");
        str = str.replace(/È|É|Ẹ|Ẻ|Ẽ|Ê|Ề|Ế|Ệ|Ể|Ễ/g, "e");
        str = str.replace(/Ì|Í|Ị|Ỉ|Ĩ/g, "i");
        str = str.replace(/Ò|Ó|Ọ|Ỏ|Õ|Ô|Ồ|Ố|Ộ|Ổ|Ỗ|Ơ|Ờ|Ớ|Ợ|Ở|Ỡ/g, "o");
        str = str.replace(/Ù|Ú|Ụ|Ủ|Ũ|Ư|Ừ|Ứ|Ự|Ử|Ữ/g, "u");
        str = str.replace(/Ỳ|Ý|Ỵ|Ỷ|Ỹ/g, "y");
        str = str.replace(/Đ/g, "d");
        // Some system encode vietnamese combining accent as individual utf-8 characters
        // Một vài bộ encode coi các dấu mũ, dấu chữ như một kí tự riêng biệt nên thêm hai dòng này
        str = str.replace(/\u0300|\u0301|\u0303|\u0309|\u0323/g, ""); // ̀ ́ ̃ ̉ ̣  huyền, sắc, ngã, hỏi, nặng
        str = str.replace(/\u02C6|\u0306|\u031B/g, ""); // ˆ ̆ ̛  Â, Ê, Ă, Ơ, Ư
        // Remove extra spaces
        // Bỏ các khoảng trắng liền nhau
        str = str.replace(/ + /g, " ");
        str = str.trim();
        // Remove punctuations
        // Bỏ dấu câu, kí tự đặc biệt
        str = str.replace(/!|@|%|\^|\*|\(|\)|\+|\=|\<|\>|\?|\/|,|\.|\:|\;|\'|\"|\&|\#|\[|\]|~|\$|_|`|-|{|}|\||\\/g, " ");
        str = str.toLowerCase();
        return str;
    }
    save(): void {

        this.beforesave();
        this.loading = true;
        if (this.actionEnum == 1) {
            this.http
                .post(this.controller + '.ctr/create/',
                    {
                        data: this.record,
                    }
                ).subscribe(resp => {
                    this.record = resp;
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
                    }
                );
        } else if (this.actionEnum == 2) {
            this.http
                .post(this.controller + '.ctr/edit/',
                    {
                        data: this.record,
                    }
                ).subscribe(resp => {
                    this.record = resp;
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
                    this.record = resp;
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
    encodeString(input: string): string {
        let output = '';
        let chr1: number, chr2: number, chr3: number;
        let enc1: number, enc2: number, enc3: number, enc4: number;
        let i = 0;

        input = this._utf8_encode(input);

        while (i < input.length) {
            chr1 = input.charCodeAt(i++);
            chr2 = input.charCodeAt(i++);
            chr3 = input.charCodeAt(i++);

            enc1 = chr1 >> 2;
            enc2 = ((chr1 & 3) << 4) | (chr2 >> 4);
            enc3 = ((chr2 & 15) << 2) | (chr3 >> 6);
            enc4 = chr3 & 63;

            if (isNaN(chr2)) {
                enc3 = enc4 = 64;
            } else if (isNaN(chr3)) {
                enc4 = 64;
            }

            output = output +
                this._keyStr.charAt(enc1) + this._keyStr.charAt(enc2) +
                this._keyStr.charAt(enc3) + this._keyStr.charAt(enc4);
        }
        return output;
    }
    decodeString(input: string): string {
        let output = '';
        let chr1: number, chr2: number, chr3: number;
        let enc1: number, enc2: number, enc3: number, enc4: number;
        let i = 0;

        input = input.replace(/[^A-Za-z0-9\+\/\=]/g, '');

        while (i < input.length) {
            enc1 = this._keyStr.indexOf(input.charAt(i++));
            enc2 = this._keyStr.indexOf(input.charAt(i++));
            enc3 = this._keyStr.indexOf(input.charAt(i++));
            enc4 = this._keyStr.indexOf(input.charAt(i++));

            chr1 = (enc1 << 2) | (enc2 >> 4);
            chr2 = ((enc2 & 15) << 4) | (enc3 >> 2);
            chr3 = ((enc3 & 3) << 6) | enc4;

            output = output + String.fromCharCode(chr1);

            if (enc3 !== 64) {
                output = output + String.fromCharCode(chr2);
            }
            if (enc4 !== 64) {
                output = output + String.fromCharCode(chr3);
            }
        }
        output = this._utf8_decode(output);
        return output;
    }
    byteArrayToBase64(byteArr: Uint8Array): string {
        let encOut = "";
        let bits: number;
        let i = 0;

        while (byteArr.length >= i + 3) {
            bits = (byteArr[i++] & 0xff) << 16 | (byteArr[i++] & 0xff) << 8 | (byteArr[i++] & 0xff);
            encOut += this._keyStr.charAt((bits & 0x00fc0000) >> 18) +
                this._keyStr.charAt((bits & 0x0003f000) >> 12) +
                this._keyStr.charAt((bits & 0x00000fc0) >> 6) +
                this._keyStr.charAt((bits & 0x0000003f));
        }

        if (byteArr.length - i > 0 && byteArr.length - i < 3) {
            const dual = (byteArr.length - i - 1) > 0;
            bits = ((byteArr[i++] & 0xff) << 16) | (dual ? (byteArr[i] & 0xff) << 8 : 0);
            encOut += this._keyStr.charAt((bits & 0x00fc0000) >> 18) +
                this._keyStr.charAt((bits & 0x0003f000) >> 12) +
                (dual ? this._keyStr.charAt((bits & 0x00000fc0) >> 6) : '=') + '=';
        }

        console.log("Base64 Len: " + encOut.length);
        return encOut;
    }
    base64ToByteArray(encStr: string): Uint8Array {
        const decOut: number[] = [];
        let bits: number;

        for (let i = 0, j = 0; i < encStr.length; i += 4, j += 3) {
            bits = (this._keyStr.indexOf(encStr.charAt(i)) & 0xff) << 18 |
                (this._keyStr.indexOf(encStr.charAt(i + 1)) & 0xff) << 12 |
                (this._keyStr.indexOf(encStr.charAt(i + 2)) & 0xff) << 6 |
                (this._keyStr.indexOf(encStr.charAt(i + 3)) & 0xff);

            decOut[j] = (bits & 0xff0000) >> 16;

            if (encStr.charCodeAt(i + 2) !== 61) { // 61 is '='
                decOut[j + 1] = (bits & 0xff00) >> 8;
            }
            if (encStr.charCodeAt(i + 3) !== 61) {
                decOut[j + 2] = bits & 0xff;
            }
        }
        // Lọc ra các giá trị undefined có thể xuất hiện ở cuối mảng
        return new Uint8Array(decOut.filter(val => val !== undefined));
    }
    _utf8_encode(str: string): string {
        str = str.replace(/\r\n/g, '\n');
        let utftext = '';
        for (let n = 0; n < str.length; n++) {
            const c = str.charCodeAt(n);
            if (c < 128) {
                utftext += String.fromCharCode(c);
            } else if ((c > 127) && (c < 2048)) {
                utftext += String.fromCharCode((c >> 6) | 192);
                utftext += String.fromCharCode((c & 63) | 128);
            } else {
                utftext += String.fromCharCode((c >> 12) | 224);
                utftext += String.fromCharCode(((c >> 6) & 63) | 128);
                utftext += String.fromCharCode((c & 63) | 128);
            }
        }
        return utftext;
    }

    _utf8_decode(utftext: string): string {
        let str = '';
        let i = 0;
        let c = 0, c2 = 0, c3 = 0;

        while (i < utftext.length) {
            c = utftext.charCodeAt(i);
            if (c < 128) {
                str += String.fromCharCode(c);
                i++;
            } else if ((c > 191) && (c < 224)) {
                c2 = utftext.charCodeAt(i + 1);
                str += String.fromCharCode(((c & 31) << 6) | (c2 & 63));
                i += 2;
            } else {
                c2 = utftext.charCodeAt(i + 1);
                c3 = utftext.charCodeAt(i + 2);
                str += String.fromCharCode(((c & 15) << 12) | ((c2 & 63) << 6) | (c3 & 63));
                i += 3;
            }
        }
        return str;
    }

    close(): void {
        if (this.actionEnum == 3) {
            this.basedialogRef.close(this.record);
        } else {
            this.basedialogRef.close(this.Oldrecord);
        }

    }


}
