import { DOCUMENT } from '@angular/common';
import { HttpClient } from '@angular/common/http';
import { ChangeDetectorRef, Component, Inject, ViewEncapsulation } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { ActivatedRoute, Router } from '@angular/router';
import { TranslocoService } from '@ngneat/transloco';
import { UserService } from 'app/core/user/user.service';
import CryptoJS from 'crypto-js';
import { isThisSecond } from 'date-fns';
import Swal from 'sweetalert2';
@Component({
    selector: 'invitation_nonlogin',
    templateUrl: './index.component.html',
    styleUrls: ['./index.component.scss'],
})
export class InvitationNonLoginComponent {
    public user: any;
    public loading = false;
    public loading_loi_moi = false;
    public controller: any;
    public record: any = null
    public user_id: any = null
    public full_name: any = ""
    public phone: any = ""
    public password: any = ""
    public errorModel: any = []
    public tokenFromUI: string = "432646294A404E63";
    public accessToken: any = "";
    public _authenticated: any;
    constructor(
        @Inject(DOCUMENT) private document: Document,
        private _changeDetectorRef: ChangeDetectorRef,
        private _router: Router,
        public route: ActivatedRoute,
        public _translocoService: TranslocoService,
        private _userService: UserService,
        private dialog: MatDialog,
        public http: HttpClient) {
    }
    check_error() {
        this.http
            .post('sys_user.ctr/check_error_loi_moi/',
                {
                    full_name: this.full_name,
                    phone: this.phone,
                    password: this.password
                }
            ).subscribe(resp => {

                this.success_workspace()
            },
                error => {
                    if (error.status == 400) {
                        this.errorModel = error.error;
                    }
                    if (error.status == 403) {
                        Swal.fire(this._translocoService.translate('no_permission'), "", "warning");
                    }
                }
            );
    }
    encryptUsingAES256(text) {
        let _key = CryptoJS.enc.Utf8.parse(this.tokenFromUI);
        let _iv = CryptoJS.enc.Utf8.parse(this.tokenFromUI);
        let encrypted = CryptoJS.AES.encrypt(text, _key, {
            keySize: 16,
            iv: _iv,
            mode: CryptoJS.mode.ECB,
            padding: CryptoJS.pad.Pkcs7,
        });
        return encrypted.toString();
    }
    ngOnInit() {


        localStorage.removeItem('menu_user');
        localStorage.removeItem('user');
        localStorage.removeItem(this.encryptUsingAES256('vnaisoft_zxc11_accessToken'));
        // Set the authenticated flag to false
        this._authenticated = false;

        this.route.params.subscribe(params => {

            this.user_id = params["id"];
            this.get_thong_tin_loi_moi();
            this.check_error();
        });
    }
    get_thong_tin_loi_moi() {
        this.http.post('sys_user.ctr/get_thong_tin_loi_moi_nonlogin', {
            user_id: this.user_id
        }).subscribe(resp => {
            this.record = resp
            if (this.record.status_del == 2) {
                this._router.navigateByUrl('/sign-in');
                Swal.fire('Lời mời đã hết hiệu lực', '', 'warning')
                    .then(resp => {
                    })
            } else {
                this.loading_loi_moi = true
            }
        })
    }
    success_workspace() {

        this.http.post('sys_user.ctr/access_loi_moi', {
            user_id: this.user_id,
            full_name: this.full_name,
            phone: this.phone,
            password: this.password,
        }).subscribe((response: any) => {

            this.accessToken = response.token;
            this._authenticated = true;
            this._userService.user = {
                id: response.id,
                name: response.full_name,
                type: response.type,
                status_del: response.status_del,
                email: response.email,
                avatar: response.avatar_path,
                status: 'online'
            };
            this._userService.user$.subscribe((user) => {
                localStorage.setItem('user', JSON.stringify(user));
            });
            Swal.fire('Đăng ký thành công', '', 'success')
                .then(resp => {
                    this._router.navigateByUrl('/sign-in');

                })
        })
    }
}
