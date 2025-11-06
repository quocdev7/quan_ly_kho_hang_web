import { Component, OnInit,HostListener } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { HttpClient, HttpResponse } from '@angular/common/http';
import Swal from 'sweetalert2';
import { Observable, of, throwError } from 'rxjs';
import { switchMap } from 'rxjs/operators';
import { AuthService } from 'app/core/auth/auth.service';
import { TranslocoService } from '@ngneat/transloco';
export interface Task {
  name: string;
  completed: boolean;

  subtasks?: Task[];
}

@Component({
    selector: 'lien_he',
    templateUrl: './lien_he.component.html',
    
})
export class lien_heComponent implements OnInit {
 public loading: any=true;
 public ten_truong: any;

 
  // Properties to manage checkbox states
private _router: Router;
  public errorModel = [];
    windowScrolledHeader: boolean;

    windowScrolled: boolean;
    constructor(
        public _httpClient: HttpClient,
        public _translocoService: TranslocoService,
        private _authService: AuthService,
          public http: HttpClient,
        private route: ActivatedRoute, // Để đọc tham số từ URL
        private router: Router         // Để điều hướng sau khi xử lý
    ) {

  };


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
      debugger
       this.check_khoi_tao_domain();

    }
    check_khoi_tao_domain(){
      debugger
        this.showLoading('', '', true);
        var ma_truong =localStorage.getItem('adb');
        this.http
                    .post('/sys_truong_hoc.ctr/check_khoi_tao_domain/', {
                        ma_truong:ma_truong
                
                    })
                    .subscribe((resp) => {
                        var data = resp as any;
                        this.loading =false;
                        Swal.close();
                        if(data =="1"){                                            
                                this.router.navigateByUrl('/portal-module-so');
                              
                        }else{
                            this.ten_truong = data;
                        }
            });
        }

}
