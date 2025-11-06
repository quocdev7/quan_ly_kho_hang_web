import { Component, Inject, ViewChild } from '@angular/core';


import { HttpClient, HttpResponse } from '@angular/common/http';

import { DataTablesResponse } from 'app/Basecomponent/datatable';
import { TranslocoService } from '@ngneat/transloco';
import { sys_user_popUpAddComponent } from './popupAdd.component';
import { MatDialog } from '@angular/material/dialog';
import { DataTableDirective } from 'angular-datatables';
import { Subject } from 'rxjs';
import Swal from 'sweetalert2';
import { BaseIndexDatatableComponent } from 'app/Basecomponent/BaseIndexDatatable.component';
import { FuseNavigationService } from '@fuse/components/navigation';
import { ActivatedRoute, Router } from '@angular/router';
import { MatMenuTrigger } from '@angular/material/menu';

import { changePassComponent } from './changePass.component';


@Component({
    selector: 'sys_user_index',
    templateUrl: './index.component.html',
    styleUrls: ['./index.component.scss']
})

export class sys_user_indexComponent extends BaseIndexDatatableComponent {


    public list_status_del: any;
    public list_loai: any;
    public file: any;
    public list_phong_ban: any;
    public list_thanh_vien: any;
    public list_chuc_danh: any;
    constructor(http: HttpClient, dialog: MatDialog
        , _translocoService: TranslocoService, private _router: Router
        , _fuseNavigationService: FuseNavigationService, route: ActivatedRoute
        , @Inject('BASE_URL') baseUrl: string
    ) {
        super(http, baseUrl, _translocoService, _fuseNavigationService, route, dialog, 'sys_user',
            { search: "", status_del: "1", phong_ban: "-1", chuc_danh: "-1" , loai:"-1"}
        )
        this.list_loai = [
            {
                id: "-1",
                name: this._translocoService.translate('system.all')
            },
            {
                id: "1",
                name: this._translocoService.translate('system.admin')
            },
            {
                id: "2",
                name: this._translocoService.translate('system.giao_vien')
            },
            {
                id: "3",
                name: this._translocoService.translate('system.phu_huynh')
            },
            {
                id: "4",
                name: this._translocoService.translate('system.hoc_sinh')
            }
        ];

        this.list_status_del = [
            // {
            //     id: "-1",
            //     name: this._translocoService.translate('system.all')
            // },
            {
                id: "1",
                name: this._translocoService.translate('system.use')
            },
            {
                id: "2",
                name: this._translocoService.translate('system.not_use')
            },
            {
                id: "3",
                name: this._translocoService.translate('system.cho_xac_nhan')
            }
        ];
    }
    changePasss(model, pos): void {
        model.type = 1;
        model.controller = "sys_user";
        const dialogRef = this.dialog.open(changePassComponent, {
            width: '768px',
            disableClose: true,
            data: model
        });
    }
    go_to_moi_thanh_vien() {
        var url = "sys_loi_moi_tham_du_index"
        this._router.navigateByUrl(url);

    }
    onFileSelected(event: any) {
        this.file = event.target.files[0];
        event.target.value = null;
    }
    dowloadFileMau() {
        var url = '/sys_user.ctr/downloadtemp';
        window.location.href = url;
    }
    onSubmitFile(event: any) {
        if (this.file == null || this.file == undefined) {
            Swal.fire('Phải chọn file import', '', 'warning')
        } else {
            this.showLoading("", "", true)
            var formData = new FormData();
            formData.append('file', this.file);
            this.http.post('/sys_user.ctr/ImportFromExcel/', formData, {
            })
                .subscribe(resp => {

                    Swal.close();
                    var res
                    res = resp
                    if (res == "1") {
                        Swal.fire('Lưu thành công', '', 'success');
                        this.file = null;
                        this.rerender();
                    }
                    if (res == "-1") {
                        Swal.fire("File không đúng định dạng", '', 'warning');
                    }
                    if (res != "-1" && res != "1") {
                        Swal.fire({
                            title: this._translocoService.translate('system.khong_the_import_duoc_vui_long_tai_ve_xem_chi_tiet'),
                            text: "",
                            icon: 'warning',
                            showCancelButton: true,
                            confirmButtonColor: '#3085d6',
                            cancelButtonColor: '#d33',
                            confirmButtonText: this._translocoService.translate('system.tai_ve'),
                            cancelButtonText: this._translocoService.translate('system.close')
                        }).then((result) => {
                            if (result.value) {
                                var url = '/sys_home.ctr/downloadtempFileError?path=' + res;
                                window.location.href = url;
                            }
                        })
                    }

                })
        }

    }


    openDialogAdd(): void {
        const dialogRef = this.dialog.open(sys_user_popUpAddComponent, {
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
        const dialogRef = this.dialog.open(sys_user_popUpAddComponent, {
            disableClose: true,
            width: '680px',
            data: model
        });
        dialogRef.afterClosed().subscribe(result => {
            if (result != undefined && result != null) this.listData[pos] = result;

            this.rerender();
        });
    }
    revertStatus(model, status_del): void {
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
                        .post(this.controller + '.ctr/update_status_del/',
                            {
                                id: model.id,
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
            })
        }
    edit_quan_tri(model, pos, value): void {
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
                model.db.quan_tri = value;
                this.http
                    .post(this.controller + '.ctr/edit/',
                        {
                            data: model,
                        }
                    ).subscribe(resp => {
                        if (value == 1) {
                            Swal.fire('Chọn làm quản trị', '', 'success');
                            this.rerender();
                        } else {
                            Swal.fire('Hủy quyền quản trị', '', 'success');
                            this.rerender();
                        }

                    },
                        error => {
                            if (error.status == 403) {
                                Swal.fire(this._translocoService.translate('no_permission'), "", "warning");
                            }

                        });
                this.rerender();
            }
        })
    }
    openDialogDetail(model, pos): void {
        model.actionEnum = 3;
        const dialogRef = this.dialog.open(sys_user_popUpAddComponent, {
            disableClose: true,
            width: '768px',
            data: model
        });
        dialogRef.afterClosed().subscribe(result => {
            if (result != undefined && result != null) this.listData[pos] = result;
        });
    }
    // get_list_thanh_vien(): void {
    //     
    //     this.http
    //         .post('/sys_user.ctr/get_list_thanh_vien', {
    //             search: this.filter.search,
    //             phong_ban: this.filter.phong_ban,
    //             chuc_danh: this.filter.chuc_danh,
    //         }
    //         ).subscribe(resp => {
    //             var data: any = resp;
    //             this.list_thanh_vien = data;
    //         });
    // }
    openMyMenu(model, menuTrigger: MatMenuTrigger, event) {

        menuTrigger.openMenu();
    }

    closeMyMenu(model, menuTrigger: MatMenuTrigger, event) {
        let id = document.getElementsByTagName("a")[0].id;
        console.log(id);
        menuTrigger.closeMenu()

    }
    moi_lai(Id): void {
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
                    .post('/sys_user.ctr/moi_lai', {
                        id: Id,
                    }
                    ).subscribe(resp => {
                        var data: any = resp;
                        Swal.fire('Gửi lời mời thành công', '', 'success')
                        this.rerender();
                    });

            }
        })

    }
    go_bo(Id): void {
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
                    .post('/sys_user.ctr/go_bo', {
                        id: Id,
                    }
                    ).subscribe(resp => {
                        var data: any = resp;
                        Swal.fire('Gỡ bỏ lời mời thành công', '', 'success')
                        this.rerender();
                    });

            }
        })


    }


    get_list_phong_ban(): void {
        var all = { id: "-1", name: this._translocoService.translate("common.all") };
        this.http
            .post('/sys_phong_ban.ctr/getListUse', {
            }
            ).subscribe(resp => {
                var data: any = resp;
                this.list_phong_ban = data;
                this.list_phong_ban.splice(0, 0, all);
            });
    }
    get_list_chuc_danh(): void {
        var all = { id: "-1", name: this._translocoService.translate("common.all") };
        this.http
            .post('/sys_chuc_danh.ctr/getListUse', {
            }
            ).subscribe(resp => {
                var data: any = resp;
                this.list_chuc_danh = data;
                this.list_chuc_danh.splice(0, 0, all);
            });
    }

    ngOnInit(): void {
        this.get_list_chuc_danh();
        this.get_list_phong_ban();
        this.baseInitData();
    }
}


