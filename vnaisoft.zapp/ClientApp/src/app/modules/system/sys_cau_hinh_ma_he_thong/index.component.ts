import { Component, Inject, ViewChild } from '@angular/core';


import { HttpClient, HttpResponse } from '@angular/common/http';

import { DataTablesResponse } from 'app/Basecomponent/datatable';
import { TranslocoService } from '@ngneat/transloco';
import { MatDialog } from '@angular/material/dialog';
import { DataTableDirective } from 'angular-datatables';
import { Subject } from 'rxjs';
import Swal from 'sweetalert2';
import { BaseIndexDatatableComponent } from 'app/Basecomponent/BaseIndexDatatable.component';
import { FuseNavigationService } from '@fuse/components/navigation';
import { ActivatedRoute } from '@angular/router';
import { list_controller } from 'app/core/data/data';

import { v4 as uuidv4 } from 'uuid';
@Component({
    selector: 'sys_cau_hinh_ma_he_thong_index',
    templateUrl: './index.component.html',
    styleUrls: ['./index.component.scss']
})

export class sys_cau_hinh_ma_he_thong_indexComponent extends BaseIndexDatatableComponent {
    public errorModel: any;
    public record: any;
    public list_cau_hinh_ma: any;
    list_controller: any
    constructor(http: HttpClient, dialog: MatDialog
        , _translocoService: TranslocoService
        , _fuseNavigationService: FuseNavigationService, route: ActivatedRoute
        , @Inject('BASE_URL') baseUrl: string
    ) {
        super(http, baseUrl, _translocoService, _fuseNavigationService, route, dialog, 'sys_cau_hinh_ma_he_thong',
            { search: "", status_del: "1", controller: "" })
        this.errorModel = [];
        this.list_controller = list_controller
    }
    ngOnInit(): void {
        this.InitData();
    }
    public InitData(): void {
        this.http
            .post('/sys_cau_hinh_ma_he_thong.ctr/getInitCode/', {
                controller: this.filter.controller,
            }
            ).subscribe(resp => {
                this.record = resp;
                this.changeMaTaiLieu();
                this.changeSoTuTang();
            });
    }
    deleteDetail(pos): void {
        this.listData.splice(pos, 1);
    }
    public sudung(id1): void {
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
                this.http
                    .post('sys_cauhinh_doc_code.ctr/sudung/',
                        {
                            id: id1,
                        }
                    ).subscribe(resp => {
                        this.rerender();
                    });
            }
        })
    }
    public loadData(): void {
        this.errorModel = [];
        this.InitData();

    }
    public changeSoTuTang(): void {
        if (this.record.db.so_chu_so_tu_tang == 4) {
            var sotutang = '0000';
        }
        if (this.record.db.so_chu_so_tu_tang == 5) {
            var sotutang = '00000';
        }
        if (this.record.db.so_chu_so_tu_tang == 6) {
            var sotutang = '000000';
        }
        if (this.record.db.so_chu_so_tu_tang > 6) {
            Swal.fire({
                title: this._translocoService.translate('khongduocvuotqua6chuso'),
                text: "",
                icon: 'warning',
            })
            var sotutang = '';
        }
        if (this.record.db.so_chu_so_tu_tang < 4) {
            Swal.fire({
                title: this._translocoService.translate('khongduocithon4chuso'),
                text: "",
                icon: 'warning',
            })
            var sotutang = '';
        }
        this.record.db.so_tu_tang = sotutang;
    }
    ngaygio: any = ""
    public changeMaTaiLieu(): void {

        this.record.db.is_ngay_gio = this.record.db.is_ngay_gio
        if (this.record.db.is_ngay_gio == true) {
            this.record.ngaygio = "yy_MM_dd-";
        }
        else {
            this.record.ngaygio = "";
            //this.record.ngaygio = "dd_MM_yy-";
        }
    }
    saveCauHinh(): void {
        //this.loading = true;
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
                this.http
                    .post('sys_cau_hinh_ma_he_thong.ctr/create/',
                        {
                            data: this.record,
                        }
                    ).subscribe(resp => {
                        this.errorModel = [];
                        this.loadData();
                        Swal.fire(this._translocoService.translate("save_successfully"), '', 'success')
                    },
                        error => {
                            if (error.status == 400) {
                                this.errorModel = error.error;
                            }
                        }
                    );
            }
        })
    }


}


