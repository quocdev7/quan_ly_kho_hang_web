import { Component, ViewEncapsulation, OnInit, Input, ChangeDetectorRef, ViewChild, EventEmitter } from '@angular/core';
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
import { SwiperComponent } from 'swiper/angular';

import SwiperCore, { SwiperOptions, EffectCoverflow, Pagination, Navigation ,Autoplay} from "swiper";

SwiperCore.use([EffectCoverflow, Pagination, Navigation,Autoplay]);

@Component({
    selector: 'about_us_cot_moc',
    templateUrl: './cotmoc.component.html',
    styleUrls: ['./cotmoc.component.scss'],
   
    encapsulation: ViewEncapsulation.None
})
export class about_us_cot_mocComponentsComponent implements OnInit
{
  
  private _unsubscribeAll: Subject<any> = new Subject<any>();
   public isScreenSmall: any = false;
   public contentCotMoc: any = {id:0};
    public lst_cot_moc:any=[];
    @ViewChild('swiper', { static: false }) swiper?: SwiperComponent;
   
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
            .post('/sys_cot_moc_su_kien.ctr/getListUse/', {}
            ).subscribe(resp => {
                this.lst_cot_moc = resp;
                this.contentCotMoc = this.lst_cot_moc[0];
                this._changeDetectorRef.markForCheck();
              
              
             
            });
    }
   
    changeslide(event){
        var that= this;
        that.contentCotMoc = that.lst_cot_moc[event[0].activeIndex];
        that._changeDetectorRef.detectChanges()
    }
    activeSlide(item,pos,event){
        var that= this;
        that.contentCotMoc = item;
        event[0].slideTo(pos);
      
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
