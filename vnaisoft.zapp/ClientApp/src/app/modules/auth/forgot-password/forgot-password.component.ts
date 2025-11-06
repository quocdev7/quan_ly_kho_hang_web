import { Component, Inject, OnInit, ViewChild, ViewEncapsulation } from '@angular/core';
import { FormBuilder, FormGroup, NgForm, Validators } from '@angular/forms';
import { finalize } from 'rxjs/operators';
import { fuseAnimations } from '@fuse/animations';
import { FuseAlertType } from '@fuse/components/alert';
import { AuthService } from 'app/core/auth/auth.service';
import { HttpClient } from '@angular/common/http';
import { BaseIndexDatatableComponent } from '../../../Basecomponent/BaseIndexDatatable.component';
import { ActivatedRoute, Router } from '@angular/router';
import { FuseNavigationService } from '@fuse/components/navigation';
import { MatDialog } from '@angular/material/dialog';
import { TranslocoService } from '@ngneat/transloco';
import { AuthMockApi } from 'app/mock-api/common/auth/api';
import { AppConfig } from 'app/core/config/app.config';
import { FuseConfigService } from '@fuse/services/config';
import { debounceTime, filter, map, takeUntil } from 'rxjs/operators';
import { Subject } from 'rxjs';
@Component({
    selector: 'auth-forgot-password',
    templateUrl: './forgot-password.component.html',
    encapsulation: ViewEncapsulation.None,
    animations: fuseAnimations
})
export class AuthForgotPasswordComponent implements OnInit {
    @ViewChild('forgotPasswordNgForm') forgotPasswordNgForm: NgForm;

    alert: { type: FuseAlertType; message: string } = {
        type: 'success',
        message: ''
    };
    forgotPasswordForm: FormGroup;
    showAlert: boolean = false;
    record: any = {
        db: {

            email: null,
        },
        capcha: ""
    }
    config: AppConfig;
    private _unsubscribeAll: Subject<any> = new Subject<any>();
    public errorModel: any;
    public loading: any;
    /**
     * Constructor
     */
    constructor(public http: HttpClient, dialog: MatDialog,
        private router: Router,
        private _fuseConfigService: FuseConfigService,
        _translocoService: TranslocoService
        , _fuseNavigationService: FuseNavigationService, route: ActivatedRoute
        , @Inject('BASE_URL') baseUrl: string, private _authService: AuthService,
        private _formBuilder: FormBuilder
    ) {
        this.errorModel = [];
    }
    gobackhomepage() {
        const url = '/home';

        if (this.config.layout != 'empty') {

            this.router.navigateByUrl(url);
        } else {
            window.open(url, "_blank");
        }
    }


    get currentYear(): number {
        return new Date().getFullYear();
    }
    // -----------------------------------------------------------------------------------------------------
    // @ Lifecycle hooks
    // -----------------------------------------------------------------------------------------------------
    srcCaptcha: any;
    reloadCaptcha(): void {
        var d = new Date();
        var n = d.getTime();
        this.srcCaptcha = '/sys_user.ctr/GetCaptchaImage?' + n;

    }
    ngOnInit(): void {

        this._fuseConfigService.config$
            .pipe(takeUntil(this._unsubscribeAll))
            .subscribe((config: AppConfig) => {
                // Store the config
                this.config = config;
            });
        // Create the form
        var d = new Date();
        var n = d.getTime();

        this.srcCaptcha = '/sys_user.ctr/GetCaptchaImage?' + n;
        this.forgotPasswordForm = this._formBuilder.group({
            email: ['', [Validators.required, Validators.email]],
            capcha: ['', Validators.required],
        });
        this.getLogo();
    }

    // -----------------------------------------------------------------------------------------------------
    // @ Public methods
    // -----------------------------------------------------------------------------------------------------
    data_logo: any;
    getLogo(): void {
        this.http
            .post('/sys_cau_hinh_anh_mac_dinh.ctr/getLogo/', {
            }
            ).subscribe(resp => {
                this.data_logo = resp;
            });
    }

    /**
     * Send the reset link
     */
    sendResetLink(): void {
        // Return if the form is invalid
        if (this.forgotPasswordForm.invalid) {
            return;
        }

        // Disable the form
        this.forgotPasswordForm.disable();

        // Hide the alert
        this.showAlert = false;


        // Forgot password
        this._authService.forgotPassword(this.forgotPasswordForm.get('phone').value, this.forgotPasswordForm.get('capcha').value)
            .pipe(
                finalize(() => {

                    // Re-enable the form
                    this.forgotPasswordForm.enable();

                    // Reset the form
                    this.forgotPasswordNgForm.resetForm();

                    // Show the alert
                    this.showAlert = true;
                })
            )
            .subscribe(
                (response) => {


                    this.router.navigateByUrl('/confirmation-otp-resetpass');

                    // //// Set the alert
                    // this.alert = {
                    //     type   : 'success',
                    //     message: 'Một email đã được gửi đến bạn, vui lòng kiểm tra email!'
                    // };
                },
                (response) => {
                    this.alert = {
                        type: 'error',
                        message: response.error.message
                    };

                }
            );
    }

    sendResetLinkNew(): void {
        this.loading = true;

        this._authService.forgotPassword(this.record.db.email, this.record.capcha)
            .subscribe(
                (resp) => {
                    this.loading = false;
                    //// Set the alert
                    this.router.navigateByUrl('/confirmation-otp-resetpass');

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
}
