import { Component, Inject, ViewChild } from '@angular/core';
import { HttpClient, HttpParams, HttpResponse } from '@angular/common/http';
import { DataTablesResponse } from 'app/Basecomponent/datatable';
import { TranslocoService } from '@ngneat/transloco';
import { sys_don_hang_ban_popUpAddComponent } from './popupAdd.component';
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
//import { sys_don_hang_ban_popupHistoryEditComponent } from './popupHistoryEdit.component';

@Component({
    selector: 'sys_don_hang_ban_index',
    templateUrl: './index.component.html',
    styleUrls: ['./index.component.scss']
})

export class sys_don_hang_ban_indexComponent extends BaseIndexDatatableComponent {

    public file: any;
    public list_doi_tuong: any;
    public list_status_del: any;
    public list_hinh_thuc_van_chuyen: any
    public list_hinh_thuc: any
    public list_loai_giao_dich: any
    public list_tinh_trang_don_hang: any
    public cau_hinh_gia_ban: any
    config: AppConfig;
    private _unsubscribeAll: Subject<any> = new Subject<any>();
    constructor(
        private seoService: SeoService,
        private router: Router,
        private _fuseConfigService: FuseConfigService,
        http: HttpClient, dialog: MatDialog
        , _translocoService: TranslocoService
        , _fuseNavigationService: FuseNavigationService, route: ActivatedRoute
        , @Inject('BASE_URL') baseUrl: string
    ) {
        super(http, baseUrl, _translocoService, _fuseNavigationService, route, dialog, 'sys_don_hang_ban',
            { search: "", status_del: "1", tu_ngay: "", den_ngay: "" }
        )
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
        this.load_date();
    }
    update_status_del(model, status_del): void {
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
                if(model.db.id_hoa_don == null || model.db.id_hoa_don == "")
                {
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
                else
                {
                    Swal.fire(this._translocoService.translate('huy_hoa_don_tuong_ung_truoc_khi_huy_don_hang'), "", "warning");
                }
            }
        })


    }
    load_date(): void {
        this.filter.tu_ngay = new Date();
        this.filter.tu_ngay.setDate(this.filter.tu_ngay.getDate() - 365);
        this.filter.den_ngay = new Date();
    }

    openDialogAdd(): void {
        const dialogRef = this.dialog.open(sys_don_hang_ban_popUpAddComponent, {
            disableClose: true,
            autoFocus: false,
            width: '100%',
            height: '100%',
            data: {
                actionEnum: 1,
                db: {
                    id: 0,
                },
                list_mat_hang: [],
            },
        });
        dialogRef.afterClosed().subscribe(result => {
            this.rerender();
            if (result.db.id == 0) return;
        });

    }
    openDialogEdit(item, pos): void {
        const dialogRef = this.dialog.open(sys_don_hang_ban_popUpAddComponent, {
            disableClose: true,
            autoFocus: false,
            width: '100%',
            height: '100%',
            data: {
                actionEnum: 2,
                db: {
                    id: item.db.id,
                    id_doi_tuong: item.db.id_doi_tuong,
                },
            }
            //data: model
        });
        dialogRef.afterClosed().subscribe(result => {
            this.rerender();
            if (result.db.id == 0) return;
        });
    }
    openDialogDetail(item, pos): void {
        const dialogRef = this.dialog.open(sys_don_hang_ban_popUpAddComponent, {
            disableClose: true,
            autoFocus: false,
            width: '100%',
            height: '100%',
            data: {
                actionEnum: 3,
                db: {
                    id: item.db.id,
                    id_doi_tuong: item.db.id_doi_tuong,
                },
            }
            //data: model
        });
        dialogRef.afterClosed().subscribe(result => {
            this.rerender();
            if (result.db.id == 0) return;
        });
    }
    ngOnInit(): void {
        this.baseInitData();

        var title = this._translocoService.translate('NAV.sys_don_hang_ban');
        var metaTag = [


            { property: 'og:title', content: 'ERP' },
            { property: 'og:image', content: "" },
            { property: 'og:description', content: "" },

        ]
        this._fuseConfigService.config$
            .pipe(takeUntil(this._unsubscribeAll))
            .subscribe((config: AppConfig) => {
                // Store the config
                this.config = config;
            });
        this.seoService.updateTitle(title);
        this.seoService.updateMetaTags(metaTag);

    }
}


