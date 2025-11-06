import { Component, Inject, ViewChild } from '@angular/core';


import { HttpClient, HttpResponse } from '@angular/common/http';

import { DataTablesResponse } from 'app/Basecomponent/datatable';
import { TranslocoService } from '@ngneat/transloco';
import { sys_template_mail_popUpAddComponent } from './popupAdd.component';
import { MatDialog } from '@angular/material/dialog';
import { DataTableDirective } from 'angular-datatables';
import { Subject } from 'rxjs';
import { BaseIndexDatatableComponent } from 'app/Basecomponent/BaseIndexDatatable.component';
import Swal from 'sweetalert2';
import { FuseNavigationService } from '@fuse/components/navigation';
import { ActivatedRoute } from '@angular/router';


@Component({
    selector: 'sys_template_mail_index',
    templateUrl: './index.component.html',
    styleUrls: ['./index.component.scss']
})

export class sys_template_mail_indexComponent extends BaseIndexDatatableComponent {
    public list_status_del: any;
    public list_type: any = [];

    constructor(http: HttpClient, dialog: MatDialog
        , _translocoService: TranslocoService
        , _fuseNavigationService: FuseNavigationService, route: ActivatedRoute
        , @Inject('BASE_URL') baseUrl: string
    ) {
        super(http, baseUrl, _translocoService, _fuseNavigationService, route, dialog, 'sys_template_mail',
            { search: "", status_del: "1", id_type: "-1" }
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
        this.http
            .post('/sys_type_mail.ctr/getListUse/', {}
            ).subscribe(resp => {
                var obj = {
                    id: "-1",
                    name: this._translocoService.translate('all')
                }
                this.list_type = resp;
                this.list_type.unshift(obj);
            });
    }
    openDialogAdd(): void {
        const dialogRef = this.dialog.open(sys_template_mail_popUpAddComponent, {
            disableClose: true,
            width: '768px',
            data: {
                actionEnum: 1,
                db: {
                    id: 0,
                    id_help: this.filter.id_help
                }
            },
        });
        dialogRef.afterClosed().subscribe(result => {
            if (result.db.id == 0) return;
            this.rerender();
        });
    }
    revertStatus(model, status_del): void {

        this.http
            .post(this.controller + '.ctr/update_status_del/',
                {
                    id: model.db.id,
                    status_del: status_del,
                }
            ).subscribe(resp => {
                this.rerender();
            },
                error => {
                    if (error.status == 403) {
                        Swal.fire(this._translocoService.translate('no_permission'), "", "warning");
                    }

                });
        this.rerender();
    }
    openDialogEdit(model, pos): void {
        model.actionEnum = 2;
        const dialogRef = this.dialog.open(sys_template_mail_popUpAddComponent, {
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
        const dialogRef = this.dialog.open(sys_template_mail_popUpAddComponent, {
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
    }


}


