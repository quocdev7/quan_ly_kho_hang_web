import { Component, ViewEncapsulation, OnInit } from '@angular/core';
import { fuseAnimations } from '@fuse/animations';
import { AppConfig } from 'app/core/config/app.config';
import { FuseConfigService } from '@fuse/services/config';
import { debounceTime, filter, map, takeUntil } from 'rxjs/operators';
import { Subject } from 'rxjs';
import { NgOtpInputModule } from 'ng-otp-input';
import { Router, ActivatedRoute, ParamMap } from '@angular/router';
import { HttpClient, HttpResponse } from '@angular/common/http';
import { AuthService } from 'app/core/auth/auth.service';
import { TranslocoService } from '@ngneat/transloco';
import Swal from 'sweetalert2';
@Component({
  selector: 'auth-confirmation-otp',
  templateUrl: './confirmation-otp.component.html',
  encapsulation: ViewEncapsulation.None,
  animations: fuseAnimations,
})
export class AuthConfirmationOtpComponent implements OnInit {
  /**
   * Constructor
   */
  config: AppConfig;
  private _unsubscribeAll: Subject<any> = new Subject<any>();
  public count: any;
  public code: any;
  public username: any;
  public loading: any;
  public phone: any;

  constructor(
    private _fuseConfigService: FuseConfigService,
    public http: HttpClient,
    public _translocoService: TranslocoService,
    public _authService: AuthService,
    public router: Router,
    public route: ActivatedRoute,
  ) {
    this.count = 0;
  }
  ngOnInit() {
    this._fuseConfigService.config$.pipe(takeUntil(this._unsubscribeAll)).subscribe((config: AppConfig) => {
      // Store the config
      this.config = config;
    });
    this.route.params.subscribe((params) => {
      this.phone = params['id'];
      this.username = params['id'];
    });
  }
  onOtpChange(otp) {
    this.code = otp;
  }

  gobackhomepage() {
    const url = '/sign-in';

    if (this.config.layout != 'empty') {
      this.router.navigateByUrl(url);
    } else {
      window.open(url, '_blank');
    }
  }
  xac_nhan(): void {
    if (!this.code) {
      Swal.fire('Vui lòng nhập thông tin', '', 'error');
      this.loading = false;
      return;
    }
    this.loading = true;
    this._authService.signInWithOTP(this.code, this.phone).subscribe(
      (resp) => {
        var data: any;
        data = resp;
        this.loading = false;

        this.router.navigateByUrl('/home');
      },
      (error) => {
        if (error.status == 400) {
          this.count = this.count + 1;
          Swal.fire('Mã xác thực không hợp lệ, Vui lòng kiểm tra lại', '', 'error');
          //      if (error.error && error.error.result != "") {
          //      Swal.fire(error.error.msg, '', 'error');
          //          }
          //  else {
          //     Swal.fire('Không hợp lệ, Vui lòng kiểm tra lại', '', 'error');
          //      }

          //Swal.fire(this._translocoService.translate("system.msgmaxacthuc"), "", "warning");
          // Show the alert
          this.loading = false;
        }
      },
    );
  }
}
