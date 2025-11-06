import { Component, Inject, OnInit, ViewChild, ViewEncapsulation } from '@angular/core';
import { FormBuilder, FormGroup, NgForm, Validators } from '@angular/forms';
import { finalize } from 'rxjs/operators';
import { fuseAnimations } from '@fuse/animations';
import { FuseValidators } from '@fuse/validators';
import { FuseAlertType } from '@fuse/components/alert';
import { AuthService } from 'app/core/auth/auth.service';
import { ActivatedRoute, Router } from '@angular/router';
import { BaseIndexDatatableComponent } from '../../../Basecomponent/BaseIndexDatatable.component';
import { HttpClient } from '@angular/common/http';
import { FuseNavigationService } from '@fuse/components/navigation';
import { MatDialog } from '@angular/material/dialog';
import { TranslocoService } from '@ngneat/transloco';

import { FuseConfigService } from '@fuse/services/config';
import { AppConfig } from 'app/core/config/app.config';
import { Subject } from 'rxjs';
import { takeUntil } from 'rxjs/operators';
@Component({
    selector: 'auth-reset-password',
    templateUrl: './reset-password.component.html',
    encapsulation: ViewEncapsulation.None,
    animations: fuseAnimations
})
export class AuthResetPasswordComponent extends BaseIndexDatatableComponent implements OnInit {
    @ViewChild('resetPasswordNgForm') resetPasswordNgForm: NgForm;

    alert: { type: FuseAlertType; message: string } = {
        type: 'success',
        message: ''
    };
    config: AppConfig;
    private _unsubscribeAll: Subject<any> = new Subject<any>();
    resetPasswordForm: FormGroup;
    showAlert: boolean = false;
    record: any = {
        password: null,
        repassword: null
    }
    /**
     * Constructor
     */
    public loading: any;
    public errorModel: any;
    public actionEnum: any;
    public token: any;
    public iduser: any;
    constructor(
        http: HttpClient
        , _translocoService: TranslocoService, dialog: MatDialog
        , _fuseNavigationService: FuseNavigationService
        , @Inject('BASE_URL') baseUrl: string,
        private _authService: AuthService,
        private _fuseConfigService: FuseConfigService,
        private _formBuilder: FormBuilder,
        public route: ActivatedRoute,
        private router: Router
    ) {
        super(http, baseUrl, _translocoService, _fuseNavigationService, route, dialog, 'sys_user',
            {

            })
        this.errorModel = [];

        try {
            this.token = this.route.snapshot.queryParamMap.get("token");
            this.http
                .post('sys_user.ctr/checkResetPass/',
                    {
                        token: this.token,
                    }
                ).subscribe(resp => {
                    if (resp == false) {
                        this.router.navigate(["/sign-in"])
                    }
                    else {
                        this.iduser = resp;
                    }
                },
                    error => {
                        this.router.navigate(["/sign-in"])

                    });
        }
        catch
        {
            this.router.navigate(["/sign-in"])
        }


    }
    get currentYear(): number {
        return new Date().getFullYear();
    }
    data_logo:any;
    getLogo(): void {
        this.http
            .post('/sys_cau_hinh_anh_mac_dinh.ctr/getLogo/', { 
            }
            ).subscribe(resp => {
                this.data_logo = resp;
            });
    }
    goSignIn() {
        const url = '/sign-in';

        if (this.config.layout != 'empty') {

            this.router.navigateByUrl(url);
        } else {
            window.open(url, "_blank");
        }
    }

    // -----------------------------------------------------------------------------------------------------
    // @ Lifecycle hooks
    // -----------------------------------------------------------------------------------------------------

    /**
     * On init
     */
    ngOnInit(): void {
        this._fuseConfigService.config$
            .pipe(takeUntil(this._unsubscribeAll))
            .subscribe((config: AppConfig) => {
                // Store the config
                this.config = config;
            });
        this.getLogo();
        this.resetPasswordNew();
        // Create the form
        // this.resetPasswordForm = this._formBuilder.group({
        //         password       : ['', Validators.required],
        //         passwordConfirm: ['', Validators.required]
        //     },
        //     {
        //         validators: FuseValidators.mustMatch('password', 'passwordConfirm')
        //     }
        // );
    }

    // -----------------------------------------------------------------------------------------------------
    // @ Public methods
    // -----------------------------------------------------------------------------------------------------


    /**
     * Reset password
     */

    resetPasswordNew(): void {
        this.loading = true;

        this._authService.resetPassword(this.record.password, this.record.repassword, this.iduser)
            .subscribe(
                (resp) => {
                    this.loading = false;

                    this.router.navigateByUrl('/confirmation-resetpass-success');
                    //this.router.navigateByUrl('/confirmation-otp-resetpass');

                    //   this.showAlert = true;

                    //   this.alert = {
                    //     type   : 'success',
                    //     message: 'Một email đã được gửi đến bạn, vui lòng kiểm tra email!'
                    //     };

                },
                (error) => {

                    if (error.status == 400) {
                        this.errorModel = error.error;

                    }

                    // Show the alert
                    this.loading = false;
                }
            );
    }

    resetPassword(): void {
        // Return if the form is invalid
        if (this.resetPasswordForm.invalid) {
            return;
        }

        // Disable the form
        this.resetPasswordForm.disable();

        // Hide the alert
        this.showAlert = false;

        // Send the request to the server
        this._authService.resetPassword(this.record.password, this.record.repassword, this.iduser)
            .pipe(
                finalize(() => {

                    // Re-enable the form
                    this.resetPasswordForm.enable();

                    // Reset the form
                    this.resetPasswordNgForm.resetForm();

                    // Show the alert
                    this.showAlert = true;
                })
            )
            .subscribe(
                (response) => {

                    // Set the alert
                    this.alert = {
                        type: 'success',
                        message: 'Thay đổi mật khẩu thành công. Vui lòng đăng nhập'
                    };
                },
                (response) => {

                    // Set the alert
                    this.alert = {
                        type: 'error',
                        message: 'Bạn đã thay đổi mật khẩu, link này không còn hiệu lực'
                    };
                }
            );
    }
}
