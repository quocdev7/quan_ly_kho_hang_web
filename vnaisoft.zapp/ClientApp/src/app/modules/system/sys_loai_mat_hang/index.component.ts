import { Component, Inject, ViewChild } from '@angular/core';


import { HttpClient, HttpParams, HttpResponse } from '@angular/common/http';

import { DataTablesResponse } from 'app/Basecomponent/datatable';
import { TranslocoService } from '@ngneat/transloco';
import { sys_loai_mat_hang_popUpAddComponent } from './popupAdd.component';
import { MatDialog } from '@angular/material/dialog';
import { DataTableDirective } from 'angular-datatables';
import { Subject } from 'rxjs';
import Swal from 'sweetalert2';
import { BaseIndexDatatableComponent } from 'app/Basecomponent/BaseIndexDatatable.component';
import { FuseNavigationService } from '@fuse/components/navigation';
import { ActivatedRoute } from '@angular/router';

import { v4 as uuidv4 } from 'uuid';
@Component({
    selector: 'sys_loai_mat_hang_index',
    templateUrl: './index.component.html',
    styleUrls: ['./index.component.scss']
})

export class sys_loai_mat_hang_indexComponent extends BaseIndexDatatableComponent {

    public list_status_del: any;
    public file: any;

    constructor(http: HttpClient, dialog: MatDialog
        , _translocoService: TranslocoService
        , _fuseNavigationService: FuseNavigationService, route: ActivatedRoute
        , @Inject('BASE_URL') baseUrl: string
    ) {
        super(http, baseUrl, _translocoService, _fuseNavigationService, route, dialog, 'sys_loai_mat_hang',
            { search: "", type: -1, status_del: "1" }
        )
        this.list_status_del = [
            {
                id: "1",
                name: this._translocoService.translate('system.use')
            },
            {
                id: "2",
                name: this._translocoService.translate('system.not_use')
            }
        ];
    }
    openDialogAdd(): void {
        const dialogRef = this.dialog.open(sys_loai_mat_hang_popUpAddComponent, {
            disableClose: true,
            autoFocus: false,
            width: '768px',
            data: {
                actionEnum: 1,
                db: {
                    id: 0,
                }
            },
        });
        dialogRef.afterClosed().subscribe(result => {
            if (result.db.id == 0) return;
            this.rerender();
        });
    }
    openDialogEdit(model, pos): void {
        model.actionEnum = 2;
        const dialogRef = this.dialog.open(sys_loai_mat_hang_popUpAddComponent, {
            disableClose: true,
            autoFocus: false,
            width: '768px',
            data: model
        });
        dialogRef.afterClosed().subscribe(result => {
            if (result != undefined && result != null) this.listData[pos] = result;
        });
    }
    openDialogDetail(model, pos): void {
        model.actionEnum = 3;
        const dialogRef = this.dialog.open(sys_loai_mat_hang_popUpAddComponent, {
            disableClose: true,
            autoFocus: false,
            width: '768px',
            data: model
        });
        dialogRef.afterClosed().subscribe(result => {
            if (result != undefined && result != null) this.listData[pos] = result;
        });
    }
    onFileSelected(event: any) {
        this.file = event.target.files[0];
    }
    dowloadFileMau() {
        var url = '/sys_loai_mat_hang.ctr/downloadtemp';
        window.location.href = url;
    }
    onSubmitFile(event: any) {

        if (this.file == null || this.file == undefined) {

            Swal.fire('Phải chọn file import', '', 'warning')

        } else {
            var formData = new FormData();

            formData.append('file', this.file);
            this.http.post('/sys_loai_mat_hang.ctr/ImportFromExcel/', formData, {
                //reportProgress: true,
                //observe: 'events'
            })
                .subscribe(resp => {
                    var res;
                    res = resp;
                    if (res == "") {
                        Swal.fire('Lưu thành công', '', 'success');
                        this.rerender();
                        event.target.value = null;
                        this.pageLoading = false;
                    } else {

                        Swal.close();
                        var res
                        res = resp
                        if (res == "1") {
                            Swal.fire('Lưu thành công', '', 'success');
                            this.file = null;
                            this.pageLoading = false;
                            this.rerender();
                        }
                        if (res == "-1") {
                            Swal.fire("File không đúng định dạng", '', 'warning');
                        }
                        if (res != "-1" && res != "1") {
                            Swal.fire({
                                title: this._translocoService.translate('system.khong_the_import_duoc_vui_long_tai_ve_xem_chi_tiet'),
                                text: "",
                                icon: 'warning',
                                showCancelButton: true,
                                confirmButtonColor: '#3085d6',
                                cancelButtonColor: '#d33',
                                confirmButtonText: this._translocoService.translate('system.tai_ve'),
                                cancelButtonText: this._translocoService.translate('system.close')
                            }).then((result) => {
                                if (result.value) {
                                    var url = '/sys_home.ctr/downloadtempFileError?path=' + res;
                                    window.location.href = url;
                                }
                            })
                        }
                        this.pageLoading = false;
                    }

                })
        }

    }
    public file_chi_tiet: any;
    onFileSelectedChiTiet(event: any) {
        this.file_chi_tiet = event.target.files[0];
        //event.target.value = null;
    }
    dowloadFileMauChiTiet() {
        var url = '/sys_loai_mat_hang.ctr/downloadtempdetail';
        window.location.href = url;
    }
    onSubmitFileChietTiet(event: any) {
        if (this.file_chi_tiet == null || this.file_chi_tiet == undefined) {
            Swal.fire('Phải chọn file import', '', 'warning')

        } else {
            this.showLoading("", "", true)
            var formData = new FormData();
            formData.append('file', this.file_chi_tiet);
            this.http.post('/sys_loai_mat_hang.ctr/ImportFromExcelChietKhau/', formData, {
            })
                .subscribe(resp => {
                        Swal.close();
                        var res
                        res = resp
                        if (res == "1") {
                            Swal.fire('Lưu thành công', '', 'success');
                            this.file = null;
                            this.rerender();
                        }
                        if (res == "-1") {
                            Swal.fire("File không đúng định dạng", '', 'warning');
                        }
                        if (res != "-1" && res != "1") {
                            Swal.fire({
                                title: this._translocoService.translate('system.khong_the_import_duoc_vui_long_tai_ve_xem_chi_tiet'),
                                text: "",
                                icon: 'warning',
                                showCancelButton: true,
                                confirmButtonColor: '#3085d6',
                                cancelButtonColor: '#d33',
                                confirmButtonText: this._translocoService.translate('system.tai_ve'),
                                cancelButtonText: this._translocoService.translate('system.close')
                            }).then((result) => {
                                if (result.value) {
                                    var url = '/sys_home.ctr/downloadtempFileError?path=' + res;
                                    window.location.href = url;
                                }
                            })
                        }
                })
        }

    }
    exportToExcel() {
        this.showLoading("", "", true)
        const params = new HttpParams()
            .set('search', this.filter.search)
            .set('status_del', this.filter.status_del)
            ;
        let uri = this.controller + '.ctr/exportExcel';
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
                a.download = 'LoaiMatHang.xlsx'
                a.click();
                document.body.removeChild(a);
                Swal.close();
            })
    }
    ngOnInit(): void {
        this.baseInitData();
    }

}


