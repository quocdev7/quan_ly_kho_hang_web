import { Component, Inject, ViewChild } from '@angular/core';
import { HttpClient, HttpParams, HttpResponse } from '@angular/common/http';
import { DataTablesResponse } from 'app/Basecomponent/datatable';
import { TranslocoService } from '@ngneat/transloco';

import { MatDialog } from '@angular/material/dialog';
import { DataTableDirective } from 'angular-datatables';
import { Subject } from 'rxjs';
import Swal from 'sweetalert2';
import { BaseIndexDatatableComponent } from 'app/Basecomponent/BaseIndexDatatable.component';
import { FuseNavigationService } from '@fuse/components/navigation';
import { ActivatedRoute } from '@angular/router';
import { SeoService } from '@fuse/services/seo.service';
import { listLoaiIn, listBienIn } from 'app/core/data/data';
import { sys_loai_mat_hang_popUpAddComponent } from '../sys_loai_mat_hang/popupAdd.component';
import { sys_mat_hang_popUpAddComponent } from '../sys_mat_hang/popupAdd.component';
// import { sys_kho_popUpAddComponent } from 'app/modules/erp/sys_kho/popupAdd.component';

@Component({
    selector: 'bao_cao_ton_kho_mat_hang_index',
    templateUrl: './index.component.html',
    styleUrls: ['./index.component.scss']
})

export class bao_cao_ton_kho_mat_hang_indexComponent extends BaseIndexDatatableComponent {

    public list_loai_mat_hang: any;
    public list_mat_hang: any;
    public list_status_del: any;
    public file: any;
    public list_loai_in: any;
    public data_loai_mat_hang: any;
    public data_mat_hang: any;

    constructor(
        private seoService: SeoService,
        http: HttpClient, dialog: MatDialog
        , _translocoService: TranslocoService
        , _fuseNavigationService: FuseNavigationService, route: ActivatedRoute
        , @Inject('BASE_URL') baseUrl: string
    ) {
        super(http, baseUrl, _translocoService, _fuseNavigationService, route, dialog, 'bao_cao_ton_kho_mat_hang',
            { search: "", id_kho: "-1", id_loai_mat_hang: "-1" }
        )


        this.get_list_loai_mat_hang();

    }
    // get_list_kho() {
    //     this.http
    //         .post('sys_kho.ctr/getListUse', {
    //         }
    //         ).subscribe(resp => {
    //             this.list_kho = resp as any;

    //             this.list_kho.splice(0, 0, { id: '-1', name: this._translocoService.translate('system.all') })
    //         });
    // }
    // public get_kho(ma_kho) {
    //     this.http.post('/sys_kho.ctr/getElementByMa', {
    //         ma: ma_kho
    //     }).subscribe(resp => {
    //         this.data_kho = resp;
    //         this.openDialogDetailKho(this.data_kho);
    //     })
    // }
    // public openDialogDetailKho(model) {
    //     model.actionEnum = 3;
    //     const dialogRef = this.dialog.open(sys_kho_popUpAddComponent, {
    //         disableClose: true,
    //         autoFocus: false,
    //         width: '768px',
    //         data: model
    //     });
    //     dialogRef.afterClosed().subscribe(result => {
    //         // if (result != undefined && result != null) this.listData[pos] = result;
    //     });
    // }
    public get_loai_mat_hang(ma_loai_mat_hang) {
        this.http.post('/sys_loai_mat_hang.ctr/getElementByMa', {
            ma: ma_loai_mat_hang
        }).subscribe(resp => {
            this.data_loai_mat_hang = resp;
            this.openDialogDetailLMH(this.data_loai_mat_hang);
        })
    }
    public openDialogDetailLMH(model) {
        model.actionEnum = 3;
        const dialogRef = this.dialog.open(sys_loai_mat_hang_popUpAddComponent, {
            disableClose: true,
            autoFocus: false,
            width: '768px',
            data: model
        });
        dialogRef.afterClosed().subscribe(result => {
            // if (result != undefined && result != null) this.listData[pos] = result;
        });
    }

    public get_mat_hang(ma_mat_hang) {
        this.http.post('/sys_mat_hang.ctr/getElementByMa', {
            ma: ma_mat_hang
        }).subscribe(resp => {
            this.data_mat_hang = resp;
            this.openDialogDetailMatHang(this.data_mat_hang);
        })
    }
    public openDialogDetailMatHang(model) {
        model.actionEnum = 3;
        const dialogRef = this.dialog.open(sys_mat_hang_popUpAddComponent, {
            disableClose: true,
            autoFocus: false,
            width: '768px',
            data: model
        });
        dialogRef.afterClosed().subscribe(result => {
            // if (result != undefined && result != null) this.listData[pos] = result;
        });
    }
    get_list_loai_mat_hang() {
        this.http
            .post('sys_loai_mat_hang.ctr/getListUse', {
            }
            ).subscribe(resp => {
                this.list_loai_mat_hang = resp as any;

                this.list_loai_mat_hang.splice(0, 0, { id: '-1', name: this._translocoService.translate('system.all') })
            });
    }
    get_list_mat_hang() {
        this.http
            .post('sys_mat_hang.ctr/get_list_mat_hang_theo_loai', {
                id_mat_hang: this.filter.id_loai_mat_hang
            }
            ).subscribe(resp => {
                this.list_mat_hang = resp as any;

                this.list_mat_hang.splice(0, 0, { id: '-1', name: this._translocoService.translate('system.all') })

                this.rerender();
            });
    }

    exportToExcel() {
        this.showLoading("", "", true)
        const params = new HttpParams()
            .set('search', this.filter.search)
            .set('id_kho', this.filter.id_kho)
            ;
        var uri = '/bao_cao_ton_kho_mat_hang.ctr/exportExcel';
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
                a.download = 'bao_cao_ton_kho_mat_hang.xlsx'
                a.click();
                document.body.removeChild(a);
                Swal.close();
            })
    }


    ngOnInit(): void {
        this.baseInitData();
        var title = this._translocoService.translate('NAV.bao_cao_ton_kho_mat_hang');
        var metaTag = [


            { property: 'og:title', content: 'SHUNGO' },
            { property: 'og:image', content: "" },
            { property: 'og:description', content: "" },

        ]
        this.seoService.updateTitle(title);
        this.seoService.updateMetaTags(metaTag);

    }


}


