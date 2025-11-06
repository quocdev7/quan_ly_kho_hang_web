import { Component, Inject, ViewChild } from '@angular/core';


import { HttpClient, HttpParams, HttpResponse } from '@angular/common/http';

import { DataTablesResponse } from 'app/Basecomponent/datatable';
import { TranslocoService } from '@ngneat/transloco';
import { sys_mat_hang_popUpAddComponent } from './popupAdd.component';
import { MatDialog } from '@angular/material/dialog';
import { DataTableDirective } from 'angular-datatables';
import { Subject } from 'rxjs';
import Swal from 'sweetalert2';
import { BaseIndexDatatableComponent } from 'app/Basecomponent/BaseIndexDatatable.component';
import { FuseNavigationService } from '@fuse/components/navigation';
import { ActivatedRoute } from '@angular/router';
import { isThisSecond } from 'date-fns';
import { cm_mau_in_popupComponent } from '../../../../@fuse/components/commonComponent/cm_mau_in/cm_mau_in_popup.component';


@Component({
    selector: 'sys_mat_hang_index',
    templateUrl: './index.component.html',
    styleUrls: ['./index.component.scss']
})

export class sys_mat_hang_indexComponent extends BaseIndexDatatableComponent {
    public list_status_del: any;
    public file: any;
    public list_loai_mat_hang: any;

    constructor(http: HttpClient, dialog: MatDialog
        , _translocoService: TranslocoService
        , _fuseNavigationService: FuseNavigationService, route: ActivatedRoute
        , @Inject('BASE_URL') baseUrl: string
    ) {
        super(http, baseUrl, _translocoService, _fuseNavigationService, route, dialog, 'sys_mat_hang',
            { search: "", status_del: "1", id_loai_mat_hang: "-1", id_thuoc_tinh: "-1", ignore_ids: "", loai_giao_dich: "", kieu_ban: "", id_doi_tuong: "" }
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
    formatDate(date: Date): string {
        const day = String(date.getDate()).padStart(2, '0');
        const month = String(date.getMonth() + 1).padStart(2, '0'); // getMonth() returns 0-based month
        const year = date.getFullYear();
        return `${day}/${month}/${year}`;
    }
    openDialogPrint(model): void {
        var link = "";
        var host = window.location.hostname;
    
        if (host == "localhost") {
            link = "https://" + host + ":44324/sys_mat_hang.ctr/print?id1=" + model.db.id + "&da=" + this.formatDate(new Date()) + "&lang=vn";
        } else {
            link = "https://" + host + '/sys_mat_hang.ctr/print?id1=' + model.db.id + "&da=" + this.formatDate(new Date()) + "&lang=vn";
        }
        var host = window.location.hostname;
        var screenWidth = window.innerWidth || document.documentElement.clientWidth || document.body.clientWidth;
        window.open(link, '_blank', 'width=' + screenWidth)
    }
    openDialogPrintAll(): void {
        var link = "";
        var host = window.location.hostname;
    
        if (host == "localhost") {
            link = "https://" + host + ":44324/sys_mat_hang.ctr/print_all?lang=vn";
        } else {
            link = "https://" + host + "/sys_mat_hang.ctr/print_all?&lang=vn";
        }
        var host = window.location.hostname;
        var screenWidth = window.innerWidth || document.documentElement.clientWidth || document.body.clientWidth;
        window.open(link, '_blank', 'width=' + screenWidth)
    }
    
    // openDialogPrint(item): void {

    //     this.http
    //         .post(this.controller + '.ctr/getPrint/', {
    //             id: item.db.id
    //         }
    //         ).subscribe(resp => {
    //             var data: any;
    //             data = resp;
    //             const dialogRef = this.dialog.open(cm_mau_in_popupComponent, {
    //                 width: '878px',
    //                 disableClose: true,
    //                 data: {
    //                     tieu_de: data.tieu_de,
    //                     noi_dung: data.noi_dung,
    //                 },
    //             });
    //             dialogRef.afterClosed().subscribe(result => {
    //                 if (result != undefined && result != null) {
    //                     this.rerender();
    //                 }


    //             });

    //         });

    // }
    get_list_loai_mat_hang(): void {
        this.http
            .post('/erp_loai_mat_hang.ctr/getListUse/', {}
            ).subscribe(resp => {
                this.list_loai_mat_hang = resp;
                this.list_loai_mat_hang.splice(0, 0, { id: '-1', name: this._translocoService.translate('system.all') })
            });
    }
    onFileSelected(event: any) {
        this.file = event.target.files[0];
    }
    dowloadFileMau() {
        var url = '/sys_mat_hang.ctr/downloadtemp';
        window.location.href = url;
    }
    onSubmitFile(event: any) {

        if (this.file == null || this.file == undefined) {

            Swal.fire('Phải chọn file import', '', 'warning')

        } else {
            this.showLoading("", "", true)
            var formData = new FormData();
            formData.append('file', this.file);
            this.http.post('/sys_mat_hang.ctr/ImportFromExcel/', formData, {
                //reportProgress: true,
                //observe: 'events'
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
            .set('id_loai_mat_hang', this.filter.id_loai_mat_hang)
            .set('id_thuoc_tinh', this.filter.id_thuoc_tinh)
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
                a.download = 'MatHang.xlsx'
                a.click();
                document.body.removeChild(a);
                Swal.close();

            })


    }
    openDialogAdd(): void {
        const dialogRef = this.dialog.open(sys_mat_hang_popUpAddComponent, {
            disableClose: true,
            autoFocus: false,
            width: '768px',
            data: {
                actionEnum: 1,
                db: {
                    id: 0,
                    id_nhan_hieu: 0,
                    list_dac_tinh: [],
                }
            },
        });
        dialogRef.afterClosed().subscribe(result => {
            if (result.db.id == 0) return;
            this.rerender();

        });
    }
    openDialogEdit(id, pos): void {
        const dialogRef = this.dialog.open(sys_mat_hang_popUpAddComponent, {
            disableClose: true,
            autoFocus: false,
            width: '768px',
            data: {
                actionEnum: 2,
                db: {
                    id: id,
                },
            }
        });
        dialogRef.afterClosed().subscribe(result => {
            this.rerender();
            if (result != undefined && result != null) this.listData[pos] = result;
        });
    }
    openDialogDetail(id, pos): void {
        const dialogRef = this.dialog.open(sys_mat_hang_popUpAddComponent, {
            disableClose: true,
            autoFocus: false,
            width: '768px',
            data: {
                actionEnum: 3,
                db: {
                    id: id,
                },
            }
        });
        dialogRef.afterClosed().subscribe(result => {
            if (result != undefined && result != null) this.listData[pos] = result;
        });
    }



    ngOnInit(): void {
        this.baseInitData();
        this.get_list_loai_mat_hang();
    }

    hideshow_sdt(model, pos) {
        model.loai_thao_tac = 1;
        this.http
            .post(this.controller + '.ctr/insert_log_thao_tac/',
                {
                    data: model,
                }
            ).subscribe(resp => {

                this.listData[pos].is_visible = !model.is_visible;

            });


    }
    hideshow_email(model, pos) {
        model.loai_thao_tac = 2;
        this.http
            .post(this.controller + '.ctr/insert_log_thao_tac/',
                {
                    data: model,
                }
            ).subscribe(resp => {

                this.listData[pos].is_visible = !model.is_visible;

            });


    }

}


