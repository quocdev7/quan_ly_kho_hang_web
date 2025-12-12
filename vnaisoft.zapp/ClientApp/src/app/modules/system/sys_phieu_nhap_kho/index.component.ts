
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
import { ActivatedRoute, Router } from '@angular/router';
import { SeoService } from '@fuse/services/seo.service';
import { FuseConfigService } from '@fuse/services/config';
import { takeUntil } from 'rxjs/operators';
import { AppConfig } from 'app/core/config/app.config';
import { sys_phieu_nhap_kho_popUpAddComponent } from './popupAdd.component';
import { cm_file_upload_popupComponent } from '@fuse/components/commonComponent/cm_file_upload/cm_file_upload_popup.component';
import { cm_mau_in_popupComponent } from '@fuse/components/commonComponent/cm_mau_in/cm_mau_in_popup.component';
@Component({
    selector: 'sys_phieu_nhap_kho_index',
    templateUrl: './index.component.html',
    styleUrls: ['./index.component.scss']
})

export class sys_phieu_nhap_kho_indexComponent extends BaseIndexDatatableComponent {
    private _unsubscribeAll: Subject<any> = new Subject<any>();
    config: AppConfig;

    public list_kho: any = [];
    public list_ngan_hang: any;
    public list_tien_te: any;
    public list_status_del: any;
    public list_don_hang_ban: any;
    public list_loai_nhap_kho: any;
    public list_don_hang: any;
    public list_nguon: any;
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
        super(http, baseUrl, _translocoService, _fuseNavigationService, route, dialog, 'sys_phieu_nhap_kho',
            {
                search: "",
                status_del: "1",
                id_loai_nhap: "-1",
                id_don_hang: "-1",
                nguon: "-1",
                tu_ngay: null,
                den_ngay: null,
                //id_don_hang:""
            }
        )
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
    load_don_hang_ban(): void {
        this.http
            .post('/sys_don_hang_ban.ctr/getListUse/', {
            }
            ).subscribe(resp => {
                this.list_don_hang_ban = resp;
            });


    }

    load_date(): void {
        this.filter.tu_ngay = new Date();
        this.filter.tu_ngay.setDate(this.filter.tu_ngay.getDate() - 365);
        this.filter.den_ngay = new Date();
    }
    openDialogFile(item, i): void {
        const dialogRef = this.dialog.open(cm_file_upload_popupComponent, {
            width: '80%',
            height: '80%',
            disableClose: true,
            data: {
                db: {
                    id: item.db.id_file_upload,
                    list_file: []
                }
            },
        });
        dialogRef.afterClosed().subscribe(result => {
            if (result != undefined && result != null) {
            }
        });
    }
    openDialogAdd(): void {
        const dialogRef = this.dialog.open(sys_phieu_nhap_kho_popUpAddComponent, {
            disableClose: true,
            autoFocus: false,
            width: '878px',
            data: {
                actionEnum: 1,
                db: {
                    id: 0,
                    ngay_nhap: new Date(),
                },
                id_mat_hangs: "",
                list_mat_hang: []
            },
        });
        dialogRef.afterClosed().subscribe(result => {
            this.load_don_hang_ban();
            this.rerender();
            if (result.db.id == 0) return;
        });
    }
    openDialogEdit(model, pos): void {
        model.actionEnum = 2;
        const dialogRef = this.dialog.open(sys_phieu_nhap_kho_popUpAddComponent, {
            disableClose: true,
            autoFocus: false,
            width: '878px',
            data: model
        });
        dialogRef.afterClosed().subscribe(result => {
            if (result != undefined && result != null) this.listData[pos] = result;
            this.load_don_hang_ban();
        });
    }
    openDialogDetail(model, pos): void {
        model.actionEnum = 3;
        const dialogRef = this.dialog.open(sys_phieu_nhap_kho_popUpAddComponent, {
            disableClose: true,
            autoFocus: false,
            width: '878px',
            data: model
        });
        dialogRef.afterClosed().subscribe(result => {
            if (result != undefined && result != null) this.listData[pos] = result;
        });
    }
    
    ngOnInit(): void {
        this.baseInitData();
        this.load_trang_thai();
        this.load_date();
        this.load_don_hang_ban();
        this._fuseConfigService.config$
            .pipe(takeUntil(this._unsubscribeAll))
            .subscribe((config: AppConfig) => {
                this.config = config;
            });

        var title = this._translocoService.translate('NAV.sys_phieu_nhap_kho');
        var metaTag = [


            { property: 'og:title', content: 'SHUNGO' },
            { property: 'og:image', content: "" },
            { property: 'og:description', content: "" },

        ]
        this.seoService.updateTitle(title);
        this.seoService.updateMetaTags(metaTag);

    }


}
