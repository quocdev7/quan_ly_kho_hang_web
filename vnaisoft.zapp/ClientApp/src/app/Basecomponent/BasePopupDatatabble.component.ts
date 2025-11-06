import { Component, Directive, Inject, QueryList, ViewChildren } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { ActivatedRoute, RouterLink } from '@angular/router';
import Swal from 'sweetalert2';
import { MatDialog } from '@angular/material/dialog';

import { TranslocoService } from '@ngneat/transloco';
import { FuseNavigationService } from '@fuse/components/navigation';
// import { doc_tailieu_view_file_onlineComponent } from 'app/modules/system/sys_user/viewfileonline.component';
import { DataTableDirective } from 'angular-datatables';
import { translateDataTable } from '@fuse/components/commonComponent/VietNameseDataTable';
import { DataTablesResponse } from 'app/Basecomponent/datatable';

@Directive()
export abstract class BasePopupDatatabbleComponent {
    @ViewChildren(DataTableDirective) dtElements: QueryList<DataTableDirective>;
    public Oldrecord: any;
    public record: any;
    public errorModel: any;
    public actionEnum: number;
    public baseurl: String;
    public action: String;
    public pageLoading: Boolean = false;
    public loading: boolean = false;
    public table: any;
    public total: any = {};
    public dtOptions: any = {};
    public filter: any;
    public currentIndex: number;
    public listData: any = [];
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
        _filter: any,
        action?: string
    ) {
        this.errorModel = [];
        this.baseurl = _baseUrl;
        this.filter = _filter;
        this.action = action ? action : 'DataHandler';
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
                    // this.basedialogRef.close(this.record);
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
                    // this.basedialogRef.close(this.record);
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

    close(): void {
        if (this.actionEnum == 3) {
            this.basedialogRef.close(this.record);
        } else {
            this.basedialogRef.close(this.Oldrecord);
        }

    }

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
    public baseInitDataTable(): void {

        this.handleDataBefore();
        const that = this;
        this.dtOptions = {
            language: translateDataTable,
            scrollY: '70vh',
            scrollCollapse: true,
            retrieve: true,
            scrollX: true,
            fixedHeader: true,
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
            responsive: {
                details: {
                    renderer: function (api, rowIdx, columns) {
                        setTimeout(function () {
                            api.columns.adjust();
                        }, 300);
                    }
                }
            },
            "searching": false,
            ajax: (data, callback, settings) => {
                this.pageLoading = true;

                this.http
                    .post<DataTablesResponse>(this.baseurl + '' + this.controller + '.ctr/' + this.action + '/',
                        {
                            param1: data,
                            data: this.filter
                        }
                    ).subscribe(resp => {
                        that.listData = resp.data;


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
    public rerender(): void {

        this.dtElements.forEach((dtElement: DataTableDirective) => {
            dtElement.dtInstance.then((dtInstance: DataTables.Api) => {
                dtInstance.ajax.reload(null, true);
            });
        });
        // Destroy the table first
    };

    public baseInitDataOption(action): void {

        this.handleDataBefore();
        const that = this;
        this.dtOptions = {
            language: translateDataTable,
            scrollY: '50vh',
            scrollCollapse: true,
            retrieve: true,
            scrollX: true,
            fixedHeader: true,
            ordering: false,
            serverSide: true,
            processing: true,
            lengthMenu: [25, 50, 100],
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
            responsive: {
                details: {
                    renderer: function (api, rowIdx, columns) {
                        setTimeout(function () {
                            api.columns.adjust();
                        }, 300);
                    }
                }
            },
            "searching": false,
            ajax: (data, callback, settings) => {

                this.pageLoading = true;
                this.http
                    .post<DataTablesResponse>(this.baseurl + '' + this.controller + '.ctr/' + action + '/',
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
                        that.currentIndex = data.start;
                        callback({
                            recordsTotal: data.recordsTotal,
                            recordsFiltered: data.recordsFiltered,
                            data: []
                        });
                    });
            },

        };


    }
}
