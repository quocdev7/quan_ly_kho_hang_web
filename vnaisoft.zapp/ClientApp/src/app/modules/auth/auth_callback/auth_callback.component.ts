import { Component, OnInit,HostListener } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { HttpClient, HttpResponse } from '@angular/common/http';
import Swal from 'sweetalert2';
import { Observable, of, throwError, Subject, timer  } from 'rxjs';
import { switchMap,  finalize, takeUntil, takeWhile, tap } from 'rxjs/operators';
import { AuthService } from 'app/core/auth/auth.service';
import { TranslocoService } from '@ngneat/transloco';



export interface Task {
  name: string;
  completed: boolean;

  subtasks?: Task[];
}

@Component({
    selector: 'auth-callback',
    templateUrl: './auth_callback.component.html',
    
})
export class AuthCallbackComponent implements OnInit {
 public loading: any;
 public is_khoi_tao: any;
 countdown: number = 5;
    countdownMapping: any = {
        '=1'   : '# second',
        'other': '# seconds'
    };
    private _unsubscribeAll: Subject<any> = new Subject<any>();
 
  // Properties to manage checkbox states

  public errorModel = [];
    windowScrolledHeader: boolean;

    windowScrolled: boolean;
    constructor(
        public _httpClient: HttpClient,
                public _translocoService: TranslocoService,
        private _router: Router,
        private _authService: AuthService,
          public http: HttpClient,
        private route: ActivatedRoute, // Để đọc tham số từ URL
        private router: Router         // Để điều hướng sau khi xử lý
    ) {
this.loading =false;
   
   
  };
  go_to_sigin(){
      
    this.router.navigateByUrl('/portal-module');
       
  }

 public showLoading(title: any = '', html: any = '', showClose: boolean = false) {
    this.loading =true;
    if (title == '') title = this._translocoService.translate('system.vui_long_doi');
    if (html == '') html = this._translocoService.translate('system.dang_tai_du_lieu');
    Swal.fire({
      title: title,
      html: html,
      allowOutsideClick: false,
      //showCloseButton: showClose,
      willOpen: () => {
        Swal.showLoading();
      },
    });
  }

 get currentYear(): number {
        return new Date().getFullYear();
    }
    ngOnInit(): void {
        // Logic này sẽ đọc tham số 'token' từ URL:
        // .../sys_sync_data.ctr/save_token/?token=ABCXYZ...
        this.route.queryParamMap.subscribe(params => {
            const token = params.get('token'); // Lấy giá trị của 'token'
            localStorage.setItem('token_sso', token);
            this.save_token(token);
           
        });
    }

   
    public check_cho_phep_su_dung(): void {

      

    }

    private save_token(token: string): void {
        var signin_so = localStorage.getItem("sigin_so");
        var signin = localStorage.getItem("sigin");
        var title="";
        var html="";
        if(signin_so =="1"){
              title='Đang xử lý yêu cầu...';
              html=`Quá trình liên kết với cơ sở dữ liệu ngành lần đầu có thể mất vài phút.
                   <br>
                   <b>Vui lòng không đóng trình duyệt.</b>`;
        }else{
                if (title == '') title = this._translocoService.translate('system.vui_long_doi');
                if (html == '') html = this._translocoService.translate('system.dang_tai_du_lieu');
        }

 
       
        this.showLoading(title, html, true);
         this.loading = true;
              this._authService.signInSSO(token)
                  .subscribe(
                      (resp) => {
                        Swal.close();
                          this.loading = false;
                          
                          if (resp == "err1") {
                              this._authService.signOut();
                              // 1. Điều hướng ngay lập tức. Đây là hành động logic chính.

                              this._router.navigateByUrl('/sign-in-so');

                              // 2. Hiển thị thông báo cho người dùng. Đây là hành động phụ về UI.
                              Swal.fire({
                                  icon: 'error',
                                  title: 'Thất bại',
                                  text: 'Tài khoản không có quyền quản trị để liên kết với CSLD ngành!',
                                  confirmButtonColor: '#dc3545',
                                  timer: 3000, // Tự động đóng sau 3 giây để không làm phiền
                                  timerProgressBar: true
                              });
                          }else if (resp == "err") {
                              this._authService.signOut();
                              // 1. Điều hướng ngay lập tức. Đây là hành động logic chính.

                              this._router.navigateByUrl('/sign-in-so');

                              // 2. Hiển thị thông báo cho người dùng. Đây là hành động phụ về UI.
                              Swal.fire({
                                  icon: 'error',
                                  title: 'Thất bại',
                                  text: 'Đăng nhập sso không thành công !',
                                  confirmButtonColor: '#dc3545',
                                  timer: 3000, // Tự động đóng sau 3 giây để không làm phiền
                                  timerProgressBar: true
                              });
                          } 
                                       
                          else {

                       
                          this.http
                            .post('/sys_truong_hoc.ctr/cho_phep_su_dung/', {

                            })
                            .subscribe((resp1) => {
                                debugger
                                if(resp1!=null){
                                      if (resp.type == 4) {
                                      this._router.navigateByUrl('/hs_lop_bai_hoc_index');
                                  }
                                  else {
                                    if(signin_so =="1")
                                    {
                                      this._router.navigateByUrl('/portal-module-so');

                                    }else
                                    {
                                        this._router.navigateByUrl('/portal-module');
                                    }


                                  }
                                }else{
                                  debugger
                                              const url = '/lien_he';
                                              window.location.href=url
                                              //this._router.navigateByUrl(url);
                                }
                            });
                              
                          }
              
      
                      },
                      (error) => {
                           // Show the alert
                             Swal.close(); 
                          this.loading = false;
                          if (error.status === 400) {
                        this.errorModel = error.error;
                        var password_fail = this.errorModel.filter(q => q.key == 'err');
                        if (password_fail.length > 0) {
                            Swal.fire({
                                icon: 'error',
                                title: 'Thất bại',
                                text: 'Token hết hạn hoặc không hợp lệ.',
                                confirmButtonColor: '#dc3545',
                                confirmButtonText: 'Thử lại'
                            });
                        }
                    } else {
                        // Xử lý các lỗi khác (ví dụ: 500 - Lỗi server, hoặc timeout)
                         Swal.fire({
                            icon: 'error',
                            title: 'Đã có lỗi xảy ra',
                            text: 'Không thể hoàn tất quá trình đồng bộ. Vui lòng thử lại sau.',
                            confirmButtonColor: '#dc3545',
                            confirmButtonText: 'Đóng'
                        });
                    }
                        
      
                       
                      }
                  );

    }




}
