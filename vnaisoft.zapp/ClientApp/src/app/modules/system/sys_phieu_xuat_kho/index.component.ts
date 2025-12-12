
import { Component, Inject, ViewChild } from '@angular/core';


import { HttpClient, HttpResponse, HttpParams } from '@angular/common/http';

import { DataTablesResponse } from 'app/Basecomponent/datatable';
import { TranslocoService } from '@ngneat/transloco';
import { sys_phieu_xuat_kho_popUpAddComponent } from './popupAdd.component';
import { MatDialog } from '@angular/material/dialog';
import { DataTableDirective } from 'angular-datatables';
import { Subject } from 'rxjs';
import Swal from 'sweetalert2';
import { BaseIndexDatatableComponent } from 'app/Basecomponent/BaseIndexDatatable.component';
import { FuseNavigationService } from '@fuse/components/navigation';
import { ActivatedRoute, Router } from '@angular/router';
import { SeoService } from '@fuse/services/seo.service';
import { FuseConfigService } from '@fuse/services/config';
import { takeUntil } from 'rxjs/operators';
import { AppConfig } from 'app/core/config/app.config';
import { cm_file_upload_popupComponent } from '@fuse/components/commonComponent/cm_file_upload/cm_file_upload_popup.component';
import { cm_mau_in_popupComponent } from '@fuse/components/commonComponent/cm_mau_in/cm_mau_in_popup.component';


@Component({
    selector: 'sys_phieu_xuat_kho_index',
    templateUrl: './index.component.html',
    styleUrls: ['./index.component.scss']
})

export class sys_phieu_xuat_kho_indexComponent extends BaseIndexDatatableComponent {
    private _unsubscribeAll: Subject<any> = new Subject<any>();
    config: AppConfig;

    public list_kho: any = [];
    public list_ngan_hang: any;
    public list_tien_te: any;
    public list_status_del: any;
    public list_don_hang_ban: any;
    public list_loai_xuat_kho: any;
    public list_nguon: any;
    public list_don_hang: any;
    public code: any;
    public file: any;
    constructor(
        private router: Router,
        private seoService: SeoService,
        private _fuseConfigService: FuseConfigService,

        http: HttpClient, dialog: MatDialog
        , _translocoService: TranslocoService
        , _fuseNavigationService: FuseNavigationService, route: ActivatedRoute
        , @Inject('BASE_URL') baseUrl: string
    ) {
        super(http, baseUrl, _translocoService, _fuseNavigationService, route, dialog, 'sys_phieu_xuat_kho',
            {
                search: "",
                status_del: "1",
                id_loai_xuat: "-1",
                id_don_hang: "-1",
                nguon: "-1",
                tu_ngay: null,
                den_ngay: null,
                //id_don_hang:""
            }
        )
    }
    
    get_list_loai_nhap_kho() {
        this.http
            .post('sys_loai_nhap_xuat.ctr/getListUse', {
            }
            ).subscribe(resp => {
                var data: any = resp;
                this.list_loai_xuat_kho = data.filter(q => q.loai == 2);
                this.list_loai_xuat_kho.splice(0, 0, { id: '-1', name: this._translocoService.translate('system.all') })
            });
    }

    ngOnInit(): void {
        this.baseInitData();
        this.get_list_loai_nhap_kho();
        this.load_trang_thai();
        this.load_date();

        this._fuseConfigService.config$
            .pipe(takeUntil(this._unsubscribeAll))
            .subscribe((config: AppConfig) => {
                this.config = config;
            });

        var title = this._translocoService.translate('NAV.sys_phieu_xuat_kho');
        var metaTag = [


            { property: 'og:title', content: 'SHUNGO' },
            { property: 'og:image', content: "" },
            { property: 'og:description', content: "" },

        ]
        this.seoService.updateTitle(title);
        this.seoService.updateMetaTags(metaTag);

    }
    load_trang_thai(): void {
        this.list_status_del = [
            {
                id: "1",
                name: this._translocoService.translate('system.use')
            },
            {
                id: "2",
                name: this._translocoService.translate('system.huy')
            }
        ];
    }

    load_date(): void {
        this.filter.tu_ngay = new Date();
        this.filter.tu_ngay.setDate(this.filter.tu_ngay.getDate() - 365);
        this.filter.den_ngay = new Date();
    }
    openDialogAdd(): void {
        const dialogRef = this.dialog.open(sys_phieu_xuat_kho_popUpAddComponent, {
            disableClose: true,
            autoFocus: false,
            width: '878px',
            data: {
                actionEnum: 1,
                db: {
                    id: 0,
                    ngay_xuat: new Date(),
                },
                id_mat_hangs: "",
                list_mat_hang: []
            },
        });
        dialogRef.afterClosed().subscribe(result => {
            this.rerender();
            if (result.db.id == 0) return;
        });
    }


    openDialogEdit(item, pos): void {

        const dialogRef = this.dialog.open(sys_phieu_xuat_kho_popUpAddComponent, {
            disableClose: true,
            autoFocus: false,
            width: '878px',
            data: {
                actionEnum: 2,
                db: {
                    id: item.db.id,
                    id_doi_tuong: item.db.id_doi_tuong,
                },
            },
        });
        dialogRef.afterClosed().subscribe(result => {
            if (result != undefined && result != null) this.listData[pos] = result;
            this.rerender();
        });
    }
    openDialogDetail(item, pos): void {
        const dialogRef = this.dialog.open(sys_phieu_xuat_kho_popUpAddComponent, {
            disableClose: true,
            autoFocus: false,
            width: '878px',
            data: {
                actionEnum: 3,
                db: {
                    id: item.db.id,
                    id_doi_tuong: item.db.id_doi_tuong,
                },
            },
        });
        dialogRef.afterClosed().subscribe(result => {
            if (result != undefined && result != null) this.listData[pos] = result;
            this.rerender();
        });
    }
    
}
