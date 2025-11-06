import { Component, Inject, ViewChild } from '@angular/core';


import { HttpClient, HttpParams, HttpResponse } from '@angular/common/http';

import { DataTablesResponse } from 'app/Basecomponent/datatable';
import { TranslocoService } from '@ngneat/transloco';
import { sys_don_vi_tinh_popUpAddComponent } from './popupAdd.component';
import { MatDialog } from '@angular/material/dialog';
import { DataTableDirective } from 'angular-datatables';
import { Subject } from 'rxjs';
import Swal from 'sweetalert2';
import { BaseIndexDatatableComponent } from 'app/Basecomponent/BaseIndexDatatable.component';
import { FuseNavigationService } from '@fuse/components/navigation';
import { ActivatedRoute } from '@angular/router';
import { SeoService } from '@fuse/services/seo.service';


@Component({
    selector: 'sys_don_vi_tinh_index',
    templateUrl: './index.component.html',
    styleUrls: ['./index.component.scss']
})

export class sys_don_vi_tinh_indexComponent extends BaseIndexDatatableComponent {


    public list_status_del: any;
    public file: any;

    constructor(
        private seoService: SeoService,
        http: HttpClient, dialog: MatDialog
        , _translocoService: TranslocoService
        , _fuseNavigationService: FuseNavigationService, route: ActivatedRoute
        , @Inject('BASE_URL') baseUrl: string
    ) {
        super(http, baseUrl, _translocoService, _fuseNavigationService, route, dialog, 'sys_don_vi_tinh',
            { search: "", status_del: "1" }
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
                a.download = 'DonViTinh.xlsx'
                a.click();
                document.body.removeChild(a);
                Swal.close();
            })
    }
    onFileSelected(event: any) {
        this.file = event.target.files[0]; console.log(this.file);
        event.target.value = null;
    }
    dowloadFileMau() {
        var url = '/sys_don_vi_tinh.ctr/downloadtemp';
        window.location.href = url;
    }
    onSubmitFile(event: any) {
        if (this.file == null || this.file == undefined) {
            Swal.fire('Phải chọn file import', '', 'warning')
            this.pageLoading = false;
        } else {
            this.showLoading("", "", true)
            var formData = new FormData();
            formData.append('file', this.file);
            this.http.post('/sys_don_vi_tinh.ctr/ImportFromExcel/', formData, {
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

    openDialogAdd(): void {
        const dialogRef = this.dialog.open(sys_don_vi_tinh_popUpAddComponent, {
            disableClose: true,
            autoFocus: false,
            width: '768px',
            data: {
                actionEnum: 1,
                db: {
                    id: "",
                }
            },
        });
        dialogRef.afterClosed().subscribe(result => {
            if (result.db.id == "") return;
            this.rerender();
        });
    }
    openDialogEdit(model, pos): void {
        model.actionEnum = 2;
        const dialogRef = this.dialog.open(sys_don_vi_tinh_popUpAddComponent, {
            disableClose: true,
            width: '768px',
            data: model
        });
        dialogRef.afterClosed().subscribe(result => {
            if (result != undefined && result != null) this.listData[pos] = result;
        });
    }
    openDialogDetail(model, pos): void {
        model.actionEnum = 3;
        const dialogRef = this.dialog.open(sys_don_vi_tinh_popUpAddComponent, {
            disableClose: true,
            width: '768px',
            data: model
        });
        dialogRef.afterClosed().subscribe(result => {
            if (result != undefined && result != null) this.listData[pos] = result;
        });
    }



    ngOnInit(): void {
        this.baseInitData();

        var title = 'SHUNGO-' + this._translocoService.translate('NAV.sys_don_vi_tinh');
        var metaTag = [


            { property: 'og:title', content: 'SHUNGO' },
            { property: 'og:image', content: "" },
            { property: 'og:description', content: "" },

        ]
        this.seoService.updateTitle(title);
        this.seoService.updateMetaTags(metaTag);

    }


}


