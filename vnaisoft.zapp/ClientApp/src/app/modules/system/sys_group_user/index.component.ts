import { Component, Inject, ViewChild } from '@angular/core';


import { HttpClient, HttpResponse } from '@angular/common/http';

import { DataTablesResponse } from 'app/Basecomponent/datatable';
import { TranslocoService } from '@ngneat/transloco';
import { sys_group_user_popUpAddComponent } from './popupAdd.component';
import { MatDialog } from '@angular/material/dialog';
import { DataTableDirective } from 'angular-datatables';
import { Subject } from 'rxjs';
import Swal from 'sweetalert2';
import { BaseIndexDatatableComponent } from 'app/Basecomponent/BaseIndexDatatable.component';
import { FuseNavigationService } from '@fuse/components/navigation';
import { ActivatedRoute } from '@angular/router';


@Component({
    selector: 'sys_group_user_index',
    templateUrl: './index.component.html',
    styleUrls: ['./index.component.scss']
})

export class sys_group_user_indexComponent extends BaseIndexDatatableComponent {
    public list_status_del: any;
    constructor(http: HttpClient, dialog: MatDialog
        , _translocoService: TranslocoService
        , _fuseNavigationService: FuseNavigationService, route: ActivatedRoute
        , @Inject('BASE_URL') baseUrl: string
    ) {
        super(http, baseUrl, _translocoService, _fuseNavigationService, route, dialog, 'sys_group_user',
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
    // revertStatus(id): void {
    //     this.http
    //         .post(this.controller + '.ctr/revert/',
    //             {
    //                 id: id,
    //             }
    //         ).subscribe(resp => {
    //             this.rerender();
    //         },
    //             error => {
    //                 console.log(error);

    //             });
    //     this.rerender();
    // }
    openDialogAdd(): void {
        const dialogRef = this.dialog.open(sys_group_user_popUpAddComponent, {
            disableClose: true,
            width: '768px',
            data: {
                actionEnum: 1,
                db: {
                    id: 0,
                },
                list_item: [],
                list_role: []
            },
        });
        dialogRef.afterClosed().subscribe(result => {

            if (result.db.id == 0) return;
            this.rerender();
        });
    }
    openDialogEdit(model, pos): void {
        model.actionEnum = 2;
        const dialogRef = this.dialog.open(sys_group_user_popUpAddComponent, {
            disableClose: true,
            width: '768px',
            data: model
        });
        dialogRef.afterClosed().subscribe(result => {
            if (result != undefined && result != null) this.listData[pos] = result;
            this.rerender();
        });
    }
    openDialogDetail(model, pos): void {
        model.actionEnum = 3;
        const dialogRef = this.dialog.open(sys_group_user_popUpAddComponent, {
            disableClose: true,
            width: '768px',
            data: model
        });
        dialogRef.afterClosed().subscribe(result => {
            if (result != undefined && result != null) this.listData[pos] = result;
            this.rerender();
        });
    }

    ngOnInit(): void {
        this.baseInitData();
    }
    ngAfterViewInit(): void {
        //Called after ngAfterContentInit when the component's view has been initialized. Applies to components only.
        //Add 'implements AfterViewInit' to the class.
    }


}


