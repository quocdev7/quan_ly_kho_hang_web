import { Component, ViewEncapsulation, OnInit, Input, ChangeDetectorRef } from '@angular/core';
import { TranslocoService } from '@ngneat/transloco';
import { HttpClient, HttpResponse } from '@angular/common/http';
import { MatDialog } from '@angular/material/dialog';
import { Router, ActivatedRoute } from '@angular/router';
import { MatButtonToggleChange } from '@angular/material/button-toggle';
import * as AOS from 'aos';
import { FuseCardComponent } from '@fuse/components/card';
import { AuthService } from '../../../core/auth/auth.service';
import { debug } from 'console';
import { takeUntil } from 'rxjs/operators';
import { Subject } from 'rxjs';
import { FuseMediaWatcherService } from '../../../../@fuse/services/media-watcher';


@Component({
    selector: 'about_us_so_do_to_chuc',
    templateUrl: './sodotochuc.component.html',
    styleUrls: ['./sodotochuc.component.scss'],
    encapsulation: ViewEncapsulation.None
})
export class about_us_so_do_to_chucComponentsComponent implements OnInit
{
    /**
     * Constructor
     */
  @Input() id_nhom_hoi_dong: any;
  private _unsubscribeAll: Subject<any> = new Subject<any>();
   public isScreenSmall: any = false;
    public lst_hoi_dong:any=[];

    constructor(

        private router: Router, private route: ActivatedRoute,
      private _fuseMediaWatcherService: FuseMediaWatcherService,
        private _authService: AuthService,
        private _changeDetectorRef: ChangeDetectorRef,
        public http: HttpClient, dialog: MatDialog
        , public _translocoService: TranslocoService,) {

    }
    loadThongTin(): void {
        this.http
            .post('/sys_cuu_sinh_vien.ctr/getListGioiThieu/', {
                id_nhom_hoi_dong: this.id_nhom_hoi_dong
            }
            ).subscribe(resp => {
                this.lst_hoi_dong = resp;
                this._changeDetectorRef.markForCheck();
            });
    }
    ngOnInit() {
        AOS.init({
            duration:1000
        });

        this._fuseMediaWatcherService.onMediaChange$
            .pipe(takeUntil(this._unsubscribeAll))
            .subscribe(({ matchingAliases }) => {
                this.isScreenSmall = !matchingAliases.includes('md');
            });
        this.loadThongTin();
      

       
    }
}
