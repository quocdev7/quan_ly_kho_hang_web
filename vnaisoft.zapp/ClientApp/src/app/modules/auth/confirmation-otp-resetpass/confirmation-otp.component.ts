import { Component, ViewEncapsulation, OnInit} from '@angular/core';
import { fuseAnimations } from '@fuse/animations';

import { NgOtpInputModule } from 'ng-otp-input';
import { Router, ActivatedRoute, ParamMap } from '@angular/router';
import { HttpClient, HttpResponse } from '@angular/common/http';
import { AuthService } from 'app/core/auth/auth.service';
import { TranslocoService } from '@ngneat/transloco';
import Swal from 'sweetalert2';
import { FuseConfigService } from '@fuse/services/config';
import { AppConfig } from 'app/core/config/app.config';
import { Subject } from 'rxjs';
import { takeUntil } from 'rxjs/operators';
@Component({
    selector     : 'auth-confirmation-otp-reset-pass',
    templateUrl  : './confirmation-otp.component.html',
    encapsulation: ViewEncapsulation.None,
    animations   : fuseAnimations
})
export class AuthConfirmationOtpResetPassComponent implements OnInit
{
    /**
     * Constructor
     */
     config: AppConfig;
     private _unsubscribeAll: Subject<any> = new Subject<any>();
    public count: any;
    public code: any;
    public users: any;
    public loading: any;
    public user_id: any;
    constructor(public http: HttpClient, public _translocoService: TranslocoService, public _authService: AuthService,private _fuseConfigService: FuseConfigService,
        public router: Router, public route: ActivatedRoute,

    )
    {
        this.count = 0;
    
    }
    ngOnInit() {
        // this.route.params.subscribe(params => {
        //     this.user_id = params["id"];
        // });
        //this.getUser();

        
        // fix không chạy được funtion gobackhomepage()
        this._fuseConfigService.config$
        .pipe(takeUntil(this._unsubscribeAll))
        .subscribe((config: AppConfig) => {
            // Store the config
            this.config = config;
        });
        this.getLogo();
    }
    get currentYear(): number {
        return new Date().getFullYear();
    }
    onOtpChange(otp) {
        this._fuseConfigService.config$
            .pipe(takeUntil(this._unsubscribeAll))
            .subscribe((config: AppConfig) => {
                // Store the config
                this.config = config;
            });
        this.code = otp;
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

    gobackhomepage() {
        const url = '/sign-in';

        if(this.config.layout!='empty'){
          
            this.router.navigateByUrl(url);
        }else{
            window.open(url, "_blank");
        }
    }
 
}
