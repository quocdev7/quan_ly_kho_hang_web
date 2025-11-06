


import { Component, OnInit, ViewChild, ViewEncapsulation } from '@angular/core';
import { FormBuilder, FormGroup, NgForm, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { fuseAnimations } from '@fuse/animations';
import { FuseAlertType } from '@fuse/components/alert';
import { AuthService } from 'app/core/auth/auth.service';

import { MAT_DIALOG_DATA, MatDialogRef, MatDialog } from '@angular/material/dialog';


import { HttpClient } from '@angular/common/http';

import { TranslocoService } from '@ngneat/transloco';
import { BasePopUpAddComponent } from 'app/Basecomponent/BasePopupAdd.component';
import { FuseNavigationService } from '@fuse/components/navigation';
import { ActivatedRoute } from '@angular/router';
import Swal from 'sweetalert2';
import { AppConfig } from 'app/core/config/app.config';
import { FuseConfigService } from '@fuse/services/config';
import { debounceTime, filter, map, takeUntil } from 'rxjs/operators';
import { Subject } from 'rxjs';
@Component({
    selector: 'auth-sign-up',
    templateUrl: './sign-up.component.html',
    encapsulation: ViewEncapsulation.None,
    animations: fuseAnimations
})
export class AuthSignUpComponent implements OnInit {
    alert: { type: FuseAlertType; message: string } = {
        type: 'success',
        message: ''
    };
    loading = false;
    showAlert: boolean = false;
  
    actionEnum: any = 1;
    record:any= {
        db: {
            FirstName: "",
            LastName: "",
            email: "",
            phone: "",
          

        }
    }
    
    config: AppConfig;
    private _unsubscribeAll: Subject<any> = new Subject<any>();
 
    public errorModel: any;
    public event_id: any;
    public isUser: any;
    public trang_thais: any;
    public list_trang_thai: any;
    public user: any;
    /**
     * Constructor
     */
    constructor(
        private _fuseConfigService: FuseConfigService,
        private formBuilder: FormBuilder,
        private router: Router, private route: ActivatedRoute,
        private _authService: AuthService,
        public http: HttpClient, dialog: MatDialog
        , public _translocoService: TranslocoService,) {

        this.errorModel = [];
       

    }

    gobackhomepage() {
        const url = '/homepage';

        if(this.config.layout!='empty'){
          
            this.router.navigateByUrl(url);
        }else{
            window.open(url, "_blank");
        }
    }
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
        var d = new Date();
        var n = d.getTime();
        this.srcCaptcha = '/sys_user.ctr/GetCaptchaImage?' + n;
       
    }

    get currentYear(): number {
        return new Date().getFullYear();
    }
    signUp(): void {
     
        this.loading = true;
        this.http.post('sys_user.ctr/register/',
            {
                data: this.record
            }
        ).subscribe(
            (resp) => {
                
                this.user  = resp;
                const url = '/confirmation-account/'+this.user;
                if(this.config.layout!='empty'){
  
                    this.router.navigateByUrl(url);
                }else{
                    window.open(url, "_blank");
                }
                // Swal.fire({
                //     html:
                //         '<div class="justify-center items-center md:flex rounded-t-lg  flex-auto background_success">'
                //         + '<img src="../assets/images/portal/success.jpg" class="img_sweetalert">'
                //         + '</div>'
                //         + '<div class="mt-4 text-center text-3xl uppercase text-black font-extrabold tracking-tight leading-tight">'
                //         + 'Thành công'
                //         + '</div>'
                //         + '<div class="mt-4 text-center text-3xl uppercase text-black font-extrabold tracking-tight leading-tight">'
                //         + 'Tài khoản của bạn đã được tạo thành công'
                //         + '</div>',
                //     confirmButtonColor: '#3085d6',
                //     confirmButtonText: this._translocoService.translate('close'),
                // }).then(resp => {
                //     const url = '/confirmation-account';
                //     if(this.config.layout!='empty'){
      
                //         this.router.navigateByUrl(url);
                //     }else{
                //         window.open(url, "_blank");
                //     }
                // })
                // Swal.fire(this._translocoService.translate("system.dang_ky_thanh_cong"), '', 'success').then(
                //     // Navigate to the confirmation required page
                //     res => {
                //          //const url = '/confirmation-otp/' +resp;
                //         const url = '/sign-in';
                //         if(this.config.layout!='empty'){
          
                //             this.router.navigateByUrl(url);
                //         }else{
                //             window.open(url, "_blank");
                //         }
                //     }

                // );

             
            },
            (error) => {
                if (error.status == 400) {
                    this.errorModel = error.error;
                   
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

    public showMessagewarningN(title, msg): void {
        Swal.fire({
            title: title,
            text: msg,
            icon: 'warning',
            confirmButtonColor: '#3085d6',
            confirmButtonText: this._translocoService.translate('close'),
        }).then((result) => {

        })

    }

}
