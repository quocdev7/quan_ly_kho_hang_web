import { Component, Inject, ViewChild } from '@angular/core';
import { HttpClient, HttpParams, HttpResponse } from '@angular/common/http';
import { DataTablesResponse } from 'app/Basecomponent/datatable';
import { TranslocoService } from '@ngneat/transloco';
import { sys_loai_nhap_xuat_popUpAddComponent } from './popupAdd.component';
import { MatDialog } from '@angular/material/dialog';
import { DataTableDirective } from 'angular-datatables';
import { Subject } from 'rxjs';
import Swal from 'sweetalert2';
import { BaseIndexDatatableComponent } from 'app/Basecomponent/BaseIndexDatatable.component';
import { FuseNavigationService } from '@fuse/components/navigation';
import { ActivatedRoute } from '@angular/router';
import { SeoService } from '@fuse/services/seo.service';


@Component({
    selector: 'sys_loai_nhap_xuat_index',
    templateUrl: './index.component.html',
    styleUrls: ['./index.component.scss']
})

export class sys_loai_nhap_xuat_indexComponent extends BaseIndexDatatableComponent {
    public list_status_del: any;
    public list_type: any;
    public file: any;
    constructor(
        private seoService: SeoService,
        http: HttpClient, dialog: MatDialog
        , _translocoService: TranslocoService
        , _fuseNavigationService: FuseNavigationService, route: ActivatedRoute
        , @Inject('BASE_URL') baseUrl: string
    ) {
        super(http, baseUrl, _translocoService, _fuseNavigationService, route, dialog, 'sys_loai_nhap_xuat',
            { search: "", status_del: "1", loai: -1 }
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
        this.list_type = [
            {
                id: -1,
                name: this._translocoService.translate('system.all')
            },
            {
                id: 1,
                name: this._translocoService.translate('system.nhap')
            },
            {
                id: 2,
                name: this._translocoService.translate('system.xuat')
            }
        ];
    }
    openDialogAdd(): void {
        const dialogRef = this.dialog.open(sys_loai_nhap_xuat_popUpAddComponent, {
            disableClose: true,
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
        const dialogRef = this.dialog.open(sys_loai_nhap_xuat_popUpAddComponent, {
            disableClose: true,
            width: '768px',
            data: model
        });
        dialogRef.afterClosed().subscribe(result => {
            this.rerender();
            if (result != undefined && result != null) this.listData[pos] = result;
        });
    }
    openDialogDetail(model, pos): void {
        model.actionEnum = 3;
        const dialogRef = this.dialog.open(sys_loai_nhap_xuat_popUpAddComponent, {
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

        var title = this._translocoService.translate('NAV.sys_loai_nhap_xuat');
        var metaTag = [


            { property: 'og:title', content: 'SHUNGO' },
            { property: 'og:image', content: "" },
            { property: 'og:description', content: "" },

        ]
        this.seoService.updateTitle(title);
        this.seoService.updateMetaTags(metaTag);

    }


}


