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
    actionEnum: any = 1;
    public record: any;
    loading = false;

    public ssoUrl:any;
    selectedStatus: any;
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
      
        this.errorModel = [];
        this.selectedStatus=2;

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

    signInWithOTP(): void {
        this.loading = true;

        if (!this.record.db.username) {
            Swal.fire('Vui lòng nhập thông tin đăng nhập', '', 'error');
            this.loading = false;
            return;
        }

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
}
