import { HttpClient } from '@angular/common/http';
import { ChangeDetectorRef, Component, Inject, Input, ViewEncapsulation } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { ActivatedRoute, Router } from '@angular/router';
import { FuseNavigationService } from '@fuse/components/navigation';
import { FuseMediaWatcherService } from '@fuse/services/media-watcher';
import { TranslocoService } from '@ngneat/transloco';
import { BaseIndexDatatableComponent } from 'app/Basecomponent/BaseIndexDatatable.component';
import { UserService } from 'app/core/user/user.service';
import { isThisSecond } from 'date-fns';

import Swal from 'sweetalert2';
import { manager_folder_indexComponent } from '../manager_folder/index.component';
import { file_manager_popupAdd_folderComponent } from '../manager_folder/popupAdd_folder.component';
@Component({
    selector: 'folder_detail',
    templateUrl: './index.component.html',
    styleUrls: ['./index.component.scss'],
    encapsulation: ViewEncapsulation.None
})
export class DetailFolderComponent extends BaseIndexDatatableComponent {
    /**
     * Constructor
     */
    @Input() folder: any
    @Input() show_detail_folder: any
    constructor(http: HttpClient, dialog: MatDialog,
        private router: Router
        , _translocoService: TranslocoService
        , _fuseNavigationService: FuseNavigationService, route: ActivatedRoute

        , @Inject('BASE_URL') baseUrl: string
        , public rootComponent: manager_folder_indexComponent
        , private _activatedRoute: ActivatedRoute,
        private _changeDetectorRef: ChangeDetectorRef,
        private _router: Router,
        private _fuseMediaWatcherService: FuseMediaWatcherService
    ) {
        super(http, baseUrl, _translocoService, _fuseNavigationService, route, dialog, 'storage_file_manager',
            { search: "", status_del: "1", id_folder: "" }
        )

        this.getUserLogin();
        this.get_list_role_share();
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
    delete_folder(id, status_del) {
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
                    .post('/storage_file_manager.ctr/delete_folder/',
                        {
                            id: id,
                            status_del: status_del
                        }
                    ).subscribe(resp => {
                        Swal.fire('Xóa thành công', '', 'success');
                        this.rootComponent.get_list_file_folder();
                        this.rootComponent.matDrawer.close();
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
    download(id) {
        if (this.folder.count_file == 0) {
            Swal.fire(this._translocoService.translate('system.chuacofile'), "", "warning");
        } else {
            var url = '/storage_file_manager.ctr/downloadFolder?id=' + id;
            window.location.href = url;
        }
    }
    openDialogEdit(model, pos): void {

        var check_role = this.folder.db.list_user_share.find(q => q.id_user == this.user_login.Id)
        if (check_role == undefined || check_role == null) {
            Swal.fire(
                'Bạn chưa được cấp quyền chỉnh sửa.',
                '',
                'warning'
            ).then(resp => {
            })
        } else if (check_role.role == 3) {
            model.actionEnum = 2;
            const dialogRef = this.dialog.open(file_manager_popupAdd_folderComponent, {
                disableClose: true,
                width: '680px',
                data: model
            });
            dialogRef.afterClosed().subscribe(result => {
                if (result != undefined && result != null) this.listData[pos] = result;
                this.rootComponent.get_list_file_folder();
                this.rootComponent.matDrawer.close();
            });
        }

        // model.actionEnum = 2;
        // const dialogRef = this.dialog.open(file_manager_popupAdd_folderComponent, {
        //     disableClose: true,
        //     width: '680px',
        //     data: model
        // });
        // dialogRef.afterClosed().subscribe(result => {
        //     if (result != undefined && result != null) this.listData[pos] = result;
        //     this.rootComponent.get_list_file_folder();
        //     this.rootComponent.matDrawer.close();
        // });
    }
}
