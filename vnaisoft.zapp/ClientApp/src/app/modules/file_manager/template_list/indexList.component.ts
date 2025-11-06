import { Component, Inject, Input } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { TranslocoService } from '@ngneat/transloco';
import { MatDialog } from '@angular/material/dialog';
import Swal from 'sweetalert2';
import { BaseIndexDatatableComponent } from 'app/Basecomponent/BaseIndexDatatable.component';
import { FuseNavigationService } from '@fuse/components/navigation';
import { ActivatedRoute, Router } from '@angular/router';
import { manager_folder_indexComponent } from '../manager_folder/index.component';
import { popupShareComponent } from '../manager_folder/popupShare.component';
import { popup_view_file_onlineComponent } from '../manager_folder/popup_view_file_online.component';
import { DOCUMENT } from '@angular/common';
import { Clipboard } from "@angular/cdk/clipboard";
@Component({
    selector: 'manager_folder_indexList',
    templateUrl: './indexList.component.html',
    styleUrls: ['./indexList.component.scss']
})

export class manager_folder_indexListComponent extends BaseIndexDatatableComponent {
    @Input() lst_data: any;
    @Input() lst_folder: any;
    @Input() list_label: any;
    @Input() lst_file: any;
    constructor(http: HttpClient, @Inject(DOCUMENT) private document: Document, dialog: MatDialog,
        private router: Router, public clipboard: Clipboard
        , _translocoService: TranslocoService
        , _fuseNavigationService: FuseNavigationService, route: ActivatedRoute
        , @Inject('BASE_URL') baseUrl: string
        , public root_main: manager_folder_indexComponent
    ) {
        super(http, baseUrl, _translocoService, _fuseNavigationService, route, dialog, 'storage_file_manager',
            { search: "", status_del: "1", id_folder: "" }
        )
        this.getUserLogin();
        this.get_list_role_share();
    }
    copy(id): void {
        var host = this.document.location.hostname
        var link = "";
        if (host == "localhost") {
            link = "https://" + host + ":44324/manager_folder_main/";
        } else {
            link = "https://" + host + '/manager_folder_main/';
        }
        link += id
        this.clipboard.copy(link);
    }
    user_login: any
    list_role_share: any
    getUserLogin() {
        this.http.post("sys_user.ctr/getUserLogin", {
        }).subscribe(resp => {
            this.user_login = resp;
        })
    }
    get_list_role_share() {
        this.list_role_share = [
            {
                id: 1,
                name: this._translocoService.translate('system.nguoi_xem')
            },
            // {
            //     id: 2,
            //     name: this._translocoService.translate('system.nguoi_nhan_xet')
            // },
            {
                id: 3,
                name: this._translocoService.translate('system.nguoi_chinh_sua')
            }
        ];
    }
    go_to_folder(id_folder, type): void {
        if (type == 1) {
            const url = 'manager_folder_main/' + id_folder;
            this.router.navigateByUrl(url);
        }
    }
    go_to_detail(id, type) {
        // 1 folder
        //2 file
        if (type == 1) {
            this.get_detail_folder(id)
        } else {
            this.get_detail_file(id)
        }
    }



    openDialogViewFileOnline(url, file_name, file_type, file_size, ten_nguoi_cap_nhat, ngay_cap_nhat, id_admin, id): void {

        if (this.listData.trang_thai_viec_lam == 1) {

        }
        // this.http
        //     .post('/doc_tailieu.ctr/save_log_view_file/', {
        //         id: id_file,
        //     }
        //     ).subscribe(resp => {

        //     });
        const dialogRef = this.dialog.open(popup_view_file_onlineComponent, {
            disableClose: true,
            maxWidth: '100vw',
            maxHeight: '100vh',
            height: '90%',
            width: '80%',
            data: {
                url: url,
                file_name: file_name,
                file_type: file_type,
                file_size: file_size,
                ten_nguoi_cap_nhat: ten_nguoi_cap_nhat,
                ngay_cap_nhat: ngay_cap_nhat,
                id_admin: id_admin,
                user_login: this.user_login.Id,
                id: id
            }
        });
        dialogRef.afterClosed().subscribe(result => {
            //this.listData[post].trang_thai_viec_lam = 2
        });
    }
    get_detail_folder(id_folder) {
        if (id_folder != null)
            this.http
                .post('storage_file_manager.ctr/get_detail_folder', {
                    id: id_folder,
                }).subscribe(resp => {
                    var data: any;
                    data = resp;

                    this.root_main.folder = data;
                    this.root_main.show_detail_folder = true
                    this.root_main.show_detail_file = false
                });
        else
            this.root_main.folder = null;
    }
    delete_file(id, status_del) {
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
                    .post('/storage_file_manager.ctr/update_status_del/',
                        {
                            id: id,
                            status_del: status_del
                        }
                    ).subscribe(resp => {
                        Swal.fire('Xóa thành công', '', 'success');
                        this.root_main.get_list_file_folder();
                        this.root_main.matDrawer.close();
                    },
                        error => {
                            if (error.status == 403) {
                                Swal.fire(this._translocoService.translate('no_permission'), "", "warning");
                            }


                        }
                    );
            }
        })
    }
    downloadFile(id, file_name) {
        const params = new HttpParams()
            .set('id', id)

            ;

        let uri = this.controller + '.ctr/downloadFile';
        this.http.get(uri, { params, responseType: 'blob', observe: 'response' })
            .subscribe(resp => {
                var res;

                res = resp;
                var downloadedFile = new Blob([res.body], { type: res.body.type });
                const a = document.createElement('a');
                a.setAttribute('style', 'display:none;');
                document.body.appendChild(a);
                a.href = URL.createObjectURL(downloadedFile);
                a.target = '_dAblank';
                a.download = file_name;
                a.click();
                document.body.removeChild(a);
            })
    }
    get_detail_file(id) {
        if (id != 0)
            this.http
                .post('storage_file_manager.ctr/get_detail_file', {
                    id: id,
                }).subscribe(resp => {
                    this.root_main.file = resp;
                    this.root_main.show_detail_file = true;
                    this.root_main.show_detail_folder = false;
                });
        else
            this.root_main.file = null;
    }

    openDialogShare(model, type) {
        // type == 4 folder
        // type == 5 file
        model.id_folder = this.root_main.id_folder
        model.actionEnum = type
        model.type = type
        const dialogRef = this.dialog.open(popupShareComponent, {
            disableClose: true,
            width: '768px',
            data: model
        });
        dialogRef.afterClosed().subscribe(result => {
            if (result.db.id == 0) return;
            this.root_main.get_list_file_folder();
        });
    }
    ngOnInit(): void {

    }
}


