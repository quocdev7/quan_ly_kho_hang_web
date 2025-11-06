import { Component, OnInit, ViewChild, ViewEncapsulation } from '@angular/core';
import { FormBuilder, FormGroup, NgForm, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { fuseAnimations } from '@fuse/animations';
import { FuseAlertType } from '@fuse/components/alert';
import { AuthService } from 'app/core/auth/auth.service';
import { Location } from '@angular/common';
import { HttpClient, HttpResponse } from '@angular/common/http';
import Swal from 'sweetalert2';
import { TranslocoService } from '@ngneat/transloco';
import { gapi } from 'gapi-script';
import { FacebookLoginProvider, SocialAuthService } from 'angularx-social-login';
@Component({
    selector: 'auth-sign-in',
    templateUrl: './sign-in.component.html',
    styleUrls: ['./index.component.scss'],
    encapsulation: ViewEncapsulation.None,
    animations: fuseAnimations
})
export class AuthSignInComponent implements OnInit {
    @ViewChild('signInNgForm') signInNgForm: NgForm;

    alert: { type: FuseAlertType; message: string } = {
        type: 'success',
        message: ''
    };
    signInForm: FormGroup;
    showAlert: boolean = false;
    false_login: number = 0;
    //showCaptcha: boolean = false;
    public year: any;
    public workpage: any
    public errorModel: any;
    public data_qrcode: any;
    public truong_hoc: any;
    actionEnum: any = 1;
    public record: any;
    loading = false;
    clientId = '880800763207-rbpv91u5ej9p9k7du7c81rcgrodgqr27.apps.googleusercontent.com';

    public ssoUrl:any;
    public list_tai_khoan: any;
    public list_truong: any;
    selectedStatus: any;
    public ma_truong: any;
    public ten_truong: any;
    public domain: any;
    /**
     * Constructor
     */
    constructor(
        public _translocoService: TranslocoService,
        public http: HttpClient,
        private _location: Location,
        private _activatedRoute: ActivatedRoute,
        private _authService: AuthService,
        private _formBuilder: FormBuilder,
        private _router: Router,
        private authService: SocialAuthService

    ) {
        // if(location.hostname =="sodangbo.hcm.edu.vn" ||location.hostname =="testsodangbo.aisava.vn") this._router.navigateByUrl('/sign-in-so');
        // else {
        //     //this._router.navigateByUrl('/sign-in');
        //     //this.get_truong_sso(); 
        // }
      
        this.errorModel = [];
        this.selectedStatus=2;
        this.list_tai_khoan = [
            {
                id: 1,
                name: this._translocoService.translate('system.tai_khoan_quan_tri')
            },
            {
                id: 2,
                name: this._translocoService.translate('system.tai_khoan_so_giao_duc')
            }
        ]; 

        this.record = {
            db: {

                email: null,
                password: null

            },
            showCaptcha: 0,
            tai_khoan: 1
        }    
        var d = new Date();
        var n = d.getTime();
   
        var date = new Date()
        this.year = date.getFullYear();
        this.srcCaptcha = '/sys_user/GetCaptchaImage?' + n;
   
    }

    toggle_tai_khoan(tai_khoan: number) {
        this.record.tai_khoan = tai_khoan;

       
    }

  
    get currentYear(): number {
        return new Date().getFullYear();
    }
    /**
     * On init
     */
    goback() {
        const { redirect } = window.history.state;

        if (redirect == '/sys_user.ctr/GetCaptchaImage') this._router.navigateByUrl('/home');
        this._router.navigateByUrl(redirect || '/home');
    }

    gobackhomepage() {
        const url = '/home';

        this._router.navigateByUrl(url);
    }
    srcCaptcha: any;
    reloadCaptcha(): void {
        var d = new Date();
        var n = d.getTime();
        this.srcCaptcha = '/sys_user.ctr/GetCaptchaImage?' + n;

    }
    ngOnInit(): void {
    
 
    }

    // -----------------------------------------------------------------------------------------------------
    // @ Public methods
    // -----------------------------------------------------------------------------------------------------

    signInNew(): void {

        this.loading = true;
        this._authService.signInNew(this.record)
            .subscribe(
                (resp) => {

                    this.loading = false;
                    this._router.navigateByUrl('/portal-module');
                    // this.http.post('sys_workspace.ctr/check_workpage', {}).subscribe(resp => {
                    //     
                    //     this.workpage = resp
                    //     if (this.workpage.is_admin == 0 && this.workpage.id_workspace != 0) {
                    //         this._router.navigateByUrl('/tools_notes');
                    //     } else
                    //         if (this.workpage.is_admin == 1 && this.workpage.id_workspace != 0) {
                    //             this._router.navigateByUrl('/home');
                    //         } else if (this.workpage.id_workspace == 0)
                    //             this._router.navigateByUrl('/home-page');
                    // })

                },
                (error) => {
                    if (error.status == 400) {

                        this.errorModel = error.error;
                        var password_fail = this.errorModel.filter(q => q.key == 'password')
                        if (password_fail.length > 0) {
                            Swal.fire({
                                html:
                                    '<div class="justify-center items-center md:flex rounded-t-lg  flex-auto background_error">'
                                    + '<img src="../assets/images/portal/error4.jpg" class="img_sweetalert">'
                                    + '</div>'
                                    + '<div class="mt-4 text-center text-3xl  uppercase  text-black font-extrabold tracking-tight leading-tight">'
                                    + 'Thất bại'
                                    + '</div>'
                                    + '<div class="mt-4 text-center text-xl text-black">'
                                    + this._translocoService.translate(password_fail[0].value)
                                    + '</div>',
                                //showCancelButton: true,
                                confirmButtonColor: '#dc3545',
                                // cancelButtonColor: '#d33',
                                confirmButtonText: 'Thử lại'
                            }).then(resp => {

                            })
                        }
                    }
                    this.false_login++;
                    if (this.false_login > 3) {
                        this.record.showCaptcha = 1;
                        this.reloadCaptcha();
                    }

                    // Set the alert
                    this.alert = {
                        type: 'error',
                        message: 'Không hợp lệ, Vui lòng kiểm tra lại'
                    };

                    // Show the alert
                    this.loading = false;
                }
            );
    }

    signInWithOTP(): void {
        this.loading = true;

        if (!this.record.db.username) {
            Swal.fire('Vui lòng nhập thông tin đăng nhập', '', 'error');
            this.loading = false;
            return;
        }

        // const isEmail = this.record.db.username.includes('@');
        // const endpoint = isEmail ? 'sys_user.ctr/send_mail_otp' : 'sys_user.ctr/signin_send_otp';
        const endpoint = 'sys_user.ctr/send_mail_otp_new';

        this.http.post(endpoint, { data: this.record }).subscribe(
            (resp) => {
                this.loading = false;
                this._router.navigateByUrl('/confirmation-otp/' + this.record.db.username);
            },
            (error) => {
                if (error.status === 400) {
                    Swal.fire('Tên đăng nhập hoặc mật khẩu không chính xác. Vui lòng thử lại.', '', 'error');
                    //Swal.fire(error.error.msg, '', 'error');
                    // if (error.error && error.error.result == "not_exist") {

                    // }
                    // else {
                    //     Swal.fire('Không hợp lệ, Vui lòng kiểm tra lại', '', 'error');
                    // }
                }
                this.loading = false;
            }
        );
    }

    signInWitSoGDHCM(): void {
        localStorage.setItem('sigin_so', "");
        localStorage.setItem('sigin', "1");
        localStorage.setItem('adb', this.ma_truong);
        if (!this.selectedStatus) {
            Swal.fire('Vui lòng chọn loại tài khoản');
            return;
        }
        //if (!this.truong_hoc_id) {
        //    Swal.fire('Vui lòng chọn trường học');
        //    return;
        //}
        this.loading = true;
        this.http
            .post('/sys_truong_hoc.ctr/get_thong_tin_truong_sign_in_so/', {
                selectedStatus: this.selectedStatus,
                ma_truong:this.ma_truong
            })
            .subscribe((resp) => {
                var data = resp as any;
                this.ssoUrl = data.ssoUrl;

  
                if (!this.ssoUrl.includes("api.hcm.edu.vn")) {
                    Swal.fire(this.ssoUrl, '', 'error');
                    this.loading = false;
                    return;
                }
                window.location.href = this.ssoUrl;
                //window.location.href = this.ssoUrl;
            
                //if (data.is_khoi_tao == 3) {
                //    if (data.token != null || data.token != "") {
                //        localStorage.setItem('token_sso', data.token);
                //        localStorage.setItem('adb', data.ma);
                //    }
                //    const url = '/authcallback'+"?token="+data.token;
                //    this._router.navigateByUrl(url);

                //}
                //else if (data.is_khoi_tao == 1) {
                //    localStorage.getItem('token_sso');;
                //    const url = '/homepage-index';
                //    this._router.navigateByUrl(url);
                //} else {
               

                //}




           
           
               
            });

      
     

    
    }

    /**
     * Sign in
     */
    signIn(): void {
        // Return if the form is invalid
        if (this.signInForm.invalid) {
            return;
        }

        // Disable the form
        this.signInForm.disable();

        // Hide the alert
        this.showAlert = false;

        // Sign in
        this._authService.signIn(this.signInForm.value)
            .subscribe(
                (res) => {
                    this._router.navigateByUrl('/home');
                },
                (response) => {

                    // Re-enable the form
                    this.signInForm.enable();

                    this.false_login++;
                    if (this.false_login > 4) this.record.showCaptcha = true;
                    // Set the alert
                    this.alert = {
                        type: 'error',
                        message: response.error.message
                    };

                    // Show the alert
                    this.showAlert = true;
                }
            );
    }
    data_logo: any = {
        avatar: "assets/images/logo/HocAI_Color@300x.png",
        image: "assets/images/logo/welcome.png"
    };
    getLogo(): void {
        this.http
            .post('/sys_cau_hinh_anh_mac_dinh.ctr/getLogo/', {}).subscribe(resp => {
                this.data_logo = resp;

                if (this.data_logo.avatar == null) {
                    this.data_logo.avatar = "assets/images/logo/HocAI_Color@300x.png"
                }
                if (this.data_logo.image == null) {
                    this.data_logo.image = "assets/images/logo/welcome.png"
                }
            });
    }
    getQRCode() {
        this.http.post('sys_cau_hinh_anh_mac_dinh.ctr/getQRCode', {}).subscribe((resp) => {

            var data: any = resp;
            this.data_qrcode = data[0];
        });
    }
}
