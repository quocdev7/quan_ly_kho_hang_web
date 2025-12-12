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
    
    get_list_loai_mat_hang(): void {
        this.http
            .post('/erp_loai_mat_hang.ctr/getListUse/', {}
            ).subscribe(resp => {
                this.list_loai_mat_hang = resp;
                this.list_loai_mat_hang.splice(0, 0, { id: '-1', name: this._translocoService.translate('system.all') })
            });
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


}


