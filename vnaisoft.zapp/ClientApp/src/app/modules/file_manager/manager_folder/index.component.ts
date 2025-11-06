import { ChangeDetectorRef, Component, Inject, ViewChild } from '@angular/core';


import { HttpClient, HttpResponse } from '@angular/common/http';

import { DataTablesResponse } from 'app/Basecomponent/datatable';
import { TranslocoService } from '@ngneat/transloco';
// import { storage_file_manager_popupAdd_folderComponent } from './popupAdd_folder.component';
import { MatDialog } from '@angular/material/dialog';
import { DataTableDirective } from 'angular-datatables';
import { Subject } from 'rxjs';
import Swal from 'sweetalert2';
import { AppConfig } from 'app/core/config/app.config';
import { BaseIndexDatatableComponent } from 'app/Basecomponent/BaseIndexDatatable.component';
import { FuseNavigationService } from '@fuse/components/navigation';
import { ActivatedRoute, Router } from '@angular/router';
import { FuseMediaWatcherService } from '@fuse/services/media-watcher';
// import { storage_file_manager_popupAdd_fileComponent } from './popupAdd_file.component';
import { FuseConfigService } from '@fuse/services/config';
import { takeUntil } from 'rxjs/operators';
import { MatDrawer } from '@angular/material/sidenav';


import { v4 as uuidv4 } from 'uuid';
import { file_manager_popupAdd_folderComponent } from './popupAdd_folder.component';
import { isThisSecond } from 'date-fns';
import { file_manager_popupAdd_fileComponent } from './popupAdd_file.component';
import { popupShareComponent } from './popupShare.component';
import { popup_view_file_onlineComponent } from './popup_view_file_online.component';
@Component({
    selector: 'manager_folder_index',
    templateUrl: './index.component.html',
    styleUrls: ['./index.component.scss']
})

export class manager_folder_indexComponent extends BaseIndexDatatableComponent {
    @ViewChild('matDrawer', { static: true }) matDrawer: MatDrawer;
    @ViewChild('matDrawer1', { static: true }) matDrawer1: MatDrawer;
    private _unsubscribeAll: Subject<any> = new Subject<any>();
    drawerMode: 'side' | 'over';
    public search: any;
    public file: any;
    public folder: any;
    public lst_folder: any = [];
    public lst_file: any = [];
    public lst_data: any = [];
    public show_detail_file: boolean = false;
    public show_detail_folder: boolean = false;
    public id_folder: any
    public list_label: any
    public user_login: any
    public check_view: any = 2;
    public list_template_view: any
    constructor(http: HttpClient, dialog: MatDialog,
        private router: Router
        , _translocoService: TranslocoService
        , _fuseNavigationService: FuseNavigationService, route: ActivatedRoute
        , @Inject('BASE_URL') baseUrl: string
        , private _activatedRoute: ActivatedRoute,
        private _router: Router,
        private _fuseMediaWatcherService: FuseMediaWatcherService
    ) {
        super(http, baseUrl, _translocoService, _fuseNavigationService, route, dialog, 'storage_file_manager',
            { search: "", status_del: "1", id_folder: "" }
        )
        this.getUserLogin();


        this.list_template_view = [
            {
                id: 1,
                name: this._translocoService.translate('system.bo_cuc_dang_luoi')
            },
            {
                id: 2,
                name: this._translocoService.translate('system.bo_cuc_dang_danh_sach')
            }
        ];
    }
    update_view_user(type) {
        this.check_view = type
        this.http.post("sys_user.ctr/update_view_file", {
            type: this.check_view
        }).subscribe(resp => {
        })
    }
    getUserLogin() {
        this.http.post("sys_user.ctr/getUserLogin", {
        }).subscribe(resp => {
            this.user_login = resp;
            if (this.user_login.type_view_file == null || this.user_login.type_view_file == undefined || this.user_login.type_view_file == '')
                this.user_login.type_view_file = 1
            this.check_view = this.user_login.type_view_file
        })
    }

    openDialogAdd(): void {

        let check_role = 3;
        if (this.id_folder != "0") {
            check_role = this.folder_main.db.list_user_share.find(q => q.id_user == this.user_login.Id).role
        }
        if (check_role != 3 || check_role == undefined) {
            Swal.fire(
                'Bạn chưa được cấp quyền để chỉnh sửa!',
                '',
                'warning'
            ).then(resp => {
            })
        } else {
            const dialogRef = this.dialog.open(file_manager_popupAdd_folderComponent, {
                disableClose: true,
                width: '768px',
                data: {
                    actionEnum: 1,
                    db: {
                        id: uuidv4(),
                        id_parent: this.id_folder
                    },
                    role: check_role
                },
            });
            dialogRef.afterClosed().subscribe(result => {
                if (result.db.id == 0) return;
                this.get_list_file_folder();

            });
        }
    }

   
    openDialogFile(): void {

        let check_role = 3;
        if (this.id_folder != "0") {
            check_role = this.folder_main.db.list_user_share.find(q => q.id_user == this.user_login.Id).role
        }
        if (check_role != 3 || check_role == undefined) {
            Swal.fire(
                'Bạn chưa được cấp quyền để chỉnh sửa!',
                '',
                'warning'
            ).then(resp => {
            })
        } else {
            const dialogRef = this.dialog.open(file_manager_popupAdd_fileComponent, {
                disableClose: true,
                width: '768px',
                data: {
                    actionEnum: 1,
                    db: {
                        id_folder: this.id_folder
                    },
                    list_document: [],
                    role: check_role
                }
            });
            dialogRef.afterClosed().subscribe(result => {
                this.get_list_file_folder();
            });
        }
    }
    goback(id): void {
        const url = '/manager_folder_main/' + id;
        this.router.navigateByUrl(url);
    }
    get_list_file_folder() {
        this.http
            .post('storage_file_manager.ctr/get_list_file_folder/',
                {
                    search: this.filter.search,
                    id_folder: this.id_folder,
                    type: 2,
                }
            ).subscribe(resp => {
                var data: any;
                data = resp;
                //this.lst_data = data;
                var err = data.err;
                if (err != "") {
                    Swal.fire("Không được phân quyền", "", "warning").then(resp => {
                        this._router.navigateByUrl('/manager_folder_main/0');
                    })
                } else {
                    this.lst_folder = data.list_folder;
                    this.lst_file = data.list_file;
                    this.lst_data = [...this.lst_folder, ...this.lst_file];
                }
            });
    }
    get_list_label() {
        this.http.post('storage_file_manager.ctr/get_list_parent', {
            id: this.id_folder
        }).subscribe(resp => {
            var data: any;
            data = resp;
            this.list_label = data;

        })
    }
    onBackdropClicked(): void {
        this._router.navigate(['./'], { relativeTo: this._activatedRoute });
    }
    ngOnInit(): void {
        this._fuseMediaWatcherService.onMediaQueryChange$('(min-width: 1440px)')
            .pipe(takeUntil(this._unsubscribeAll))
            .subscribe((state) => {
                this.drawerMode = state.matches ? 'side' : 'over';
            });
        this.route.params.subscribe(params => {
            var id = params["id"];
            if (id == null || id == undefined || id == '')
                this.id_folder = "0";
            else
                this.id_folder = id;
            this.get_list_file_folder();
            if (this.id_folder != "0") {
                this.get_list_label();
                this.check_role_folder();
                this.getElementByIdFolder();
            }
            else
                this.list_label = []
        });

    }
    check_role_folder() {
        this.http.post('storage_file_manager.ctr/check_role_folder', {
            id_folder: this.id_folder
        }).subscribe(resp => {
            console.log(resp);
            if (resp == false) {
                Swal.fire(
                    'Bạn không có quyền truy cập!',
                    '',
                    'warning'
                ).then(resp => {
                    const url = '/manager_folder_main/0';
                    this.router.navigateByUrl(url);
                })
            }
        })
    }
    folder_main: any
    getElementByIdFolder() {
        if (this.id_folder != "0")
            this.http
                .post('storage_file_manager.ctr/getElementByIdFolder', {
                    id: this.id_folder,
                }).subscribe(resp => {
                    var data: any;
                    data = resp;
                    this.folder_main = data;
                });
        else
            this.folder_main = null;
    }
    getFontAwesomeIconFromMIME(mimeType) {
        // List of official MIME Types: http://www.iana.org/assignments/media-types/media-types.xhtml
        var icon_classes = {
            // Media
            "image/jpeg": "assets/icon_file_type/jpg.png",
            "image/svg+xml": "assets/icon_file_type/jpg.png",
            "image/png": "assets/icon_file_type/png.png",
            // Documents
            "application/pdf": "assets/icon_file_type/pdf.png",
            "application/msword": "assets/icon_file_type/doc.png",
            "application/vnd.ms-word": "assets/icon_file_type/doc.png",
            "application/vnd.oasis.opendocument.text": "assets/icon_file_type/doc.png",
            "application/vnd.openxmlformats-officedocument.wordprocessingml":
                "assets/icon_file_type/doc.png",
            "application/vnd.ms-excel": 'assets/icon_file_type/excel.png',
            "application/vnd.openxmlformats-officedocument.spreadsheetml":
                'assets/icon_file_type/excel.png',
            "application/vnd.oasis.opendocument.spreadsheet": "assets/icon_file_type/excel.png",
            "application/vnd.ms-powerpoint": "assets/icon_file_type/ppt.png",
            "application/vnd.openxmlformats-officedocument.presentationml":
                "assets/icon_file_type/ppt.png",
            "application/vnd.oasis.opendocument.presentation": "assets/icon_file_type/ppt.png",
            "text/plain": "assets/icon_file_type/txt.png",
            "text/html": "assets/icon_file_type/html.png",
            "application/json": "assets/icon_file_type/json-file.png",
            // Archives
            "application/gzip": "assets/icon_file_type/zip.png",
            "application/x-zip-compressed": "assets/icon_file_type/zip.png",
            "application/octet-stream": "assets/icon_file_type/zip-1.png"
        };

        for (var key in icon_classes) {
            if (icon_classes.hasOwnProperty(key)) {
                if (mimeType.search(key) === 0) {
                    // Found it
                    return icon_classes[key];
                }
            } else {
                return "assets/icon_file_type/file.png";
            }
        }
    }
}


