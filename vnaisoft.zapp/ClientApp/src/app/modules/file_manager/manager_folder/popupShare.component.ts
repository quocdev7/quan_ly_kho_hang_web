import { Component, Inject } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef, MatDialog } from '@angular/material/dialog';


import { HttpClient, HttpParams } from '@angular/common/http';
import { BasePopUpAddComponent } from 'app/Basecomponent/BasePopupAdd.component';
import { TranslocoService } from '@ngneat/transloco';
import { FuseNavigationService } from '@fuse/components/navigation';
import { ActivatedRoute } from '@angular/router';
import Swal from 'sweetalert2';


@Component({
    selector: 'popupShare',
    templateUrl: 'popupShare.component.html',
    styleUrls: ['./popupShare.component.scss']

})
export class popupShareComponent extends BasePopUpAddComponent {
    public list: any = []
    public list_backup: any = []
    public list_user_chip: any = []
    public list_role_share: any = []
    public list_role_share_show: any = []
    public list_role_main: any = []
    public user_login: any;
    public searchField: any = '';
    constructor(public dialogRef: MatDialogRef<popupShareComponent>,
        http: HttpClient, _translocoService: TranslocoService,
        _fuseNavigationService: FuseNavigationService, route: ActivatedRoute,
        @Inject('BASE_URL') baseUrl: string,
        public dialogModal: MatDialog,
        @Inject(MAT_DIALOG_DATA) data: any) {
        super(_translocoService, _fuseNavigationService, route, baseUrl, http, 'storage_file_manager', dialogRef, dialogModal);
        // type == 4 folder
        // type == 5 file

        this.record = data;
        this.record.list_user = []
        this.Oldrecord = JSON.parse(JSON.stringify(data));
        this.actionEnum = data.actionEnum;
        this.getListUse()
        this.get_list_role_main()
        this.get_list_role_share()
        this.getUserLogin()
        this.get_list_user_share()
        this.record.db.role = 1
    }
    downloadFile(id, file_name) {
        const params = new HttpParams()
            .set('id', id)
            ;
        let uri = 'storage_file_manager.ctr/downloadFile';
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
    download(id) {
        if (this.record.count_file == 0) {
            Swal.fire(this._translocoService.translate('system.chuacofile'), "", "warning");
        } else {
            var url = '/storage_file_manager.ctr/downloadFolder?id=' + id;
            window.location.href = url;
        }
    }
    get_list_role_share() {
        this.list_role_share = [
            {
                id: 1,
                name: this._translocoService.translate('system.nguoi_xem')
            },
            {
                id: 4,
                name: this._translocoService.translate('system.nguoi_chinh_sua')
            },
            {
                id: 3,
                name: this._translocoService.translate('system.nguoi_so_huu')
            }
            // {
            //     id: 2,
            //     name: this._translocoService.translate('system.nguoi_nhan_xet')
            // },
        ];
    }
    get_list_role_main() {

        this.list_role_main = [
            {
                id: 1,
                name: this._translocoService.translate('system.han_che'),
                label: this._translocoService.translate('system.han_che_label'),
            },
            {
                id: 2,
                name: this._translocoService.translate('system.bat_ky'),
                label: this._translocoService.translate('system.bat_ky_label'),
            }
        ];
    }
    share() {
        var url_controller = "";
        if (this.record.actionEnum == 4) {
            url_controller = "share_folder"
        } else {
            url_controller = "share_file"
        }
        this.http.post("storage_file_manager.ctr/" + url_controller, {
            data: this.record.db
        }).subscribe(resp => {
            Swal.fire(
                'Thành công!',
                '',
                'success'
            ).then(resp => {
                this.close()
            })
        })
        //var check_role = this.record.db.list_user_share.find(q => q.id_user == this.user_login.Id)
        // if (check_role == undefined || check_role == null) {
        //     Swal.fire(
        //         'Bạn chưa được cấp quyền chia sẻ.',
        //         '',
        //         'warning'
        //     ).then(resp => {
        //         this.remove_user(0)
        //         this.close()
        //     })
        // } else {
        //     var name_role = this.list_role_share.find(q => q.id == check_role.role).name
        //     var url_controller = "";
        //     if (this.record.actionEnum == 4) {
        //         url_controller = "share_folder"
        //     } else {
        //         url_controller = "share_file"
        //     }
        //     if (check_role.role == 3) {
        //         this.http.post("storage_file_manager.ctr/" + url_controller, {
        //             data: this.record.db
        //         }).subscribe(resp => {
        //             Swal.fire(
        //                 'Thành công!',
        //                 '',
        //                 'success'
        //             ).then(resp => {
        //                 this.close()
        //             })
        //         })
        //     } else {
        //         var ten = ""
        //         if (this.record.db.ten != null || this.record.db.ten != undefined)
        //             ten = this.record.db.ten
        //         else
        //             ten = this.record.db.file_name
        //         Swal.fire({
        //             text: `Quyền của bạn là ${name_role} và không thể quản lý quyền truy cập ?`,
        //             title: `Yêu cầu chủ sở hữu chia sẻ "${ten}".`,
        //             icon: 'warning',
        //             showCancelButton: true,
        //             confirmButtonColor: '#3085d6',
        //             cancelButtonColor: '#d33',
        //             confirmButtonText: 'Đồng ý',
        //             cancelButtonText: "Không"
        //         }).then((result) => {
        //             if (result.isConfirmed) {
        //                 this.http.post("storage_file_manager.ctr/sendMailShare", {
        //                     list_user: this.record.db.list_user_share,
        //                     id_folder: this.record.db.id,
        //                     id_admin: this.record.db.id_admin,
        //                 }).subscribe(resp => {
        //                     Swal.fire(
        //                         'Thành công!',
        //                         'Bạn đã gởi yêu cầu đến chủ sở hữu.',
        //                         'success'
        //                     ).then(resp => {
        //                         this.close()
        //                     })
        //                 })
        //             } else {
        //                 this.close()
        //             }
        //         })
        //     }
        // }
    }
    share_role() {
        var url_controller = "";
        if (this.record.actionEnum == 4) {
            url_controller = "share_folder"
        } else {
            url_controller = "share_file"
        }
        this.http.post("storage_file_manager.ctr/" + url_controller, {
            data: this.record.db
        }).subscribe(resp => {

        })
    }
    getListUse() {
        this.http.post("sys_user.ctr/getListUseNew", {
            search: ""
        }).subscribe(resp => {
            this.list_backup = resp;
            this.list = resp;
        })
    }
    getUserLogin() {
        this.http.post("sys_user.ctr/getUserLogin", {
        }).subscribe(resp => {
            this.user_login = resp;
            this.user_login.role = this.record.db.list_user_share.find(q => this.user_login.Id == q.id_user).role
            if (this.user_login.role == 4)
                this.list_role_share_show = this.list_role_share.filter(q => q.id != 3)
            else
                this.list_role_share_show = this.list_role_share
        })
    }
    get_list_user_share() {
        this.http.post("storage_file_manager.ctr/get_list_user_share", {
            id: this.record.db.id,
            type: this.record.type
        }).subscribe(resp => {
            var data: any
            data = resp

            // this.record.db.list_user_share = data.list_user_share
            // this.share_role()
            if (data != null && this.record.db.list_user_share.length == 1) {
                this.record.db.list_user_share = data.list_user_share
                this.share_role()
            }
        })
    }
    add_user(pos) {
        var user: any = this.list[pos];
        user.role = 1;
        user.id_user = user.id;
        this.list_user_chip.unshift(user)
        this.record.db.list_user_share.unshift(user)
        this.list.splice(pos, 1);
        this.searchField = ""
    }
    remove_user(pos) {

        var user = this.record.list_user[pos];
        this.list.unshift(user)
        this.list = this.list
        this.list_user_chip.splice(pos, 1);
        this.record.db.list_user_share.splice(pos, 1);
    }
    removeVietnameseTones(str) {
        str = str.replace(/à|á|ạ|ả|ã|â|ầ|ấ|ậ|ẩ|ẫ|ă|ằ|ắ|ặ|ẳ|ẵ/g, "a");
        str = str.replace(/è|é|ẹ|ẻ|ẽ|ê|ề|ế|ệ|ể|ễ/g, "e");
        str = str.replace(/ì|í|ị|ỉ|ĩ/g, "i");
        str = str.replace(/ò|ó|ọ|ỏ|õ|ô|ồ|ố|ộ|ổ|ỗ|ơ|ờ|ớ|ợ|ở|ỡ/g, "o");
        str = str.replace(/ù|ú|ụ|ủ|ũ|ư|ừ|ứ|ự|ử|ữ/g, "u");
        str = str.replace(/ỳ|ý|ỵ|ỷ|ỹ/g, "y");
        str = str.replace(/đ/g, "d");
        str = str.replace(/À|Á|Ạ|Ả|Ã|Â|Ầ|Ấ|Ậ|Ẩ|Ẫ|Ă|Ằ|Ắ|Ặ|Ẳ|Ẵ/g, "a");
        str = str.replace(/È|É|Ẹ|Ẻ|Ẽ|Ê|Ề|Ế|Ệ|Ể|Ễ/g, "e");
        str = str.replace(/Ì|Í|Ị|Ỉ|Ĩ/g, "i");
        str = str.replace(/Ò|Ó|Ọ|Ỏ|Õ|Ô|Ồ|Ố|Ộ|Ổ|Ỗ|Ơ|Ờ|Ớ|Ợ|Ở|Ỡ/g, "o");
        str = str.replace(/Ù|Ú|Ụ|Ủ|Ũ|Ư|Ừ|Ứ|Ự|Ử|Ữ/g, "u");
        str = str.replace(/Ỳ|Ý|Ỵ|Ỷ|Ỹ/g, "y");
        str = str.replace(/Đ/g, "d");
        // Some system encode vietnamese combining accent as individual utf-8 characters
        // Một vài bộ encode coi các dấu mũ, dấu chữ như một kí tự riêng biệt nên thêm hai dòng này
        str = str.replace(/\u0300|\u0301|\u0303|\u0309|\u0323/g, ""); // ̀ ́ ̃ ̉ ̣  huyền, sắc, ngã, hỏi, nặng
        str = str.replace(/\u02C6|\u0306|\u031B/g, ""); // ˆ ̆ ̛  Â, Ê, Ă, Ơ, Ư
        // Remove extra spaces
        // Bỏ các khoảng trắng liền nhau
        str = str.replace(/ + /g, " ");
        str = str.trim();
        // Remove punctuations
        // Bỏ dấu câu, kí tự đặc biệt
        str = str.replace(/!|@|%|\^|\*|\(|\)|\+|\=|\<|\>|\?|\/|,|\.|\:|\;|\'|\"|\&|\#|\[|\]|~|\$|_|`|-|{|}|\||\\/g, " ");
        str = str.toLowerCase();
        return str;
    }
    filterUsers(): void {
        if (!this.searchField) {
            this.list = [];
            return;
        }
        var search = this.removeVietnameseTones(this.searchField)
        this.list = this.list_backup.filter(q => this.removeVietnameseTones(q.name).toLowerCase().includes(search.toLowerCase()));
    }
    change_role_name(item, type) {
        // nếu type == 1 change role=> admin
        if (type == 1)
            return this.list_role_main.find(q => q.id == item)
        // nếu type == 2 change role=> user
        if (type == 2)
            return this.list_role_share.find(q => q.id == item)
    }
    set_role(pos, role, type) {
        // nếu type == 1 set role=> admin role_share
        if (type == 1) {
            this.record.db.role_share = role
        }
        // nếu type == 3 set role=> admin role
        if (type == 3) {
            this.record.db.role = role
        }
        // nếu type == 2 set role=> user
        if (type == 2)
            this.record.db.list_user_share[pos].role = role

    }
    ngOnInit(): void {
    }
}
