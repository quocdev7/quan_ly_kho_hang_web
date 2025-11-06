import { Component, Inject, ViewChild } from '@angular/core';


import { HttpClient, HttpResponse } from '@angular/common/http';

import { DataTablesResponse } from 'app/Basecomponent/datatable';
import { TranslocoService } from '@ngneat/transloco';
import { sys_cau_hinh_anh_mac_dinh_popUpAddComponent } from './popupAdd.component';
import { MatDialog } from '@angular/material/dialog';
import { DataTableDirective } from 'angular-datatables';
import { Subject } from 'rxjs';
import Swal from 'sweetalert2';
import { BaseIndexDatatableComponent } from 'app/Basecomponent/BaseIndexDatatable.component';
import { FuseNavigationService } from '@fuse/components/navigation';
import { ActivatedRoute } from '@angular/router';

import { v4 as uuidv4 } from 'uuid';
@Component({
    selector: 'sys_cau_hinh_anh_mac_dinh_index',
    templateUrl: './index.component.html',
    styleUrls: ['./index.component.scss']
})

export class sys_cau_hinh_anh_mac_dinh_indexComponent extends BaseIndexDatatableComponent {


    public list_type: any;

    public list_status_del: any;
    constructor(http: HttpClient, dialog: MatDialog
        , _translocoService: TranslocoService
        , _fuseNavigationService: FuseNavigationService, route: ActivatedRoute
        , @Inject('BASE_URL') baseUrl: string
    ) {
        super(http, baseUrl, _translocoService, _fuseNavigationService, route, dialog, 'sys_cau_hinh_anh_mac_dinh',
            { search: "", type: -1, status_del: "1" }
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
                name: this._translocoService.translate("system.all")
            },
            {
                id: 1,
                name: this._translocoService.translate("system.hinh_anh_dai_dien")
            },
            {
                id: 2,
                name: this._translocoService.translate("system.logo")
            },
            {
                id: 3,
                name: this._translocoService.translate("system.hoc_sinh")
            },
            {
                id: 4,
                name: this._translocoService.translate("system.giao_vien")
            },
            {
                id: 5,
                name: this._translocoService.translate("system.phu_huynh")
            },
            {
                id: 6,
                name: this._translocoService.translate("system.mon_hoc")
            },
            {
                id: 7,
                name: this._translocoService.translate("system.khac")
            },
            {
                id: 8,
                name: this._translocoService.translate("system.ma_qr")
            },

            {
                id: 9,
                name: this._translocoService.translate("system.icon_fb")
            },
            {
                id: 10,
                name: this._translocoService.translate("system.icon_zl")
            },
            {
                id: 11,
                name: this._translocoService.translate("system.icon_lk")
            },
            {
                id: 12,
                name: this._translocoService.translate("system.icon_tw")
            },
            {
                id: 13,
                name: this._translocoService.translate("system.icon_bl")
            },
        ]
    }
    //revertStatus(model, pos): void {
    //    model.db.status_del = 1;
    //    this.http
    //        .post(this.controller + '.ctr/edit/',
    //            {
    //                data: model,
    //            }
    //    ).subscribe(resp => {
    //        this.rerender();
    //        },
    //           );
    //    this.rerender();
    //}
    openDialogAdd(): void {
        const dialogRef = this.dialog.open(sys_cau_hinh_anh_mac_dinh_popUpAddComponent, {
            disableClose: true,
            width: '768px',
            data: {
                actionEnum: 1,
                db: {
                    id: uuidv4(),
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
        const dialogRef = this.dialog.open(sys_cau_hinh_anh_mac_dinh_popUpAddComponent, {
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
        const dialogRef = this.dialog.open(sys_cau_hinh_anh_mac_dinh_popUpAddComponent, {
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


