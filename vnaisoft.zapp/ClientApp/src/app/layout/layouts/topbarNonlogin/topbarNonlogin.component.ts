import { ChangeDetectorRef, Component, OnDestroy, OnInit, ViewEncapsulation, HostListener } from '@angular/core';
import { ActivatedRoute, Data, Router } from '@angular/router';
import { Subject } from 'rxjs';
import { takeUntil } from 'rxjs/operators';
import { FuseMediaWatcherService } from '@fuse/services/media-watcher';
import { FuseNavigationItem, FuseNavigationService, FuseVerticalNavigationComponent } from '@fuse/components/navigation';
import { InitialData } from 'app/app.types';

import { HttpClient } from '@angular/common/http';
import { TranslocoService, AvailableLangs } from '@ngneat/transloco';
import { AuthService } from 'app/core/auth/auth.service';
import { UserService } from 'app/core/user/user.service';
import { MatDialog } from '@angular/material/dialog';


@Component({
    selector: 'topbarNonlogin-layout',
    templateUrl: './topbarNonlogin.component.html',
    styleUrls: ['./topbarNonlogin.component.scss'],
    encapsulation: ViewEncapsulation.None

})
export class TopBarWebNonloginLayoutComponent implements OnInit,OnDestroy {
    private _unsubscribeAll: Subject<any> = new Subject<any>();

    /**
     * Constructor
     */
    isScreenSmall: boolean;
    windowScrolled: boolean;
    windowScrolledHeader: boolean;
    public is_login: any = false;
    public tuyen_sinh: any;

    public truong_hoc: any ;
    public user_htx: any;
    public user: any = {
        type: 0
    };
    constructor(
        private _authService: AuthService,
        private changeDetectorRef: ChangeDetectorRef,
        private _activatedRoute: ActivatedRoute,
        private _router: Router,
        private http: HttpClient,
        private _changeDetectorRef: ChangeDetectorRef,
        private _fuseMediaWatcherService: FuseMediaWatcherService,
        private _fuseNavigationService: FuseNavigationService,
        private _translocoService: TranslocoService) 
        {
            
        this.load_thong_tin_truong();
    }

    signOut(): void {
        this._router.navigate(['/sign-out']);
    }
    loadUser(): void {
        this.http
            .post('/sys_user.ctr/getUserLogin/', {}
            ).subscribe(resp => {
                this.user_htx = resp;
                //this.loadmenu();
       
            });
    }
    load_thong_tin_truong(): void {
        this.http
            .post('/sys_truong_hoc.ctr/get_thong_tin_truong', {}
            ).subscribe(resp => {
                this.truong_hoc = resp;
            });
    }
    goto_addHoSo(){
        const url = '/portal_add_ho_so_index';
        this._router.navigateByUrl(url);
    }
    // -----------------------------------------------------------------------------------------------------
    // @ Lifecycle hooks
    // -----------------------------------------------------------------------------------------------------
    @HostListener("window:scroll", [])
    onWindowScroll(): void {
        if ((window.pageYOffset || document.documentElement.scrollTop || document.body.scrollTop) >= 50) {
            this.windowScrolledHeader = true;
            this.windowScrolled = true;
        }
        else {
            this.windowScrolled = false;
            this.windowScrolledHeader = false;
        }



    }
    /**
     * On destroy
     */
    ngOnDestroy(): void {
        // Unsubscribe from all subscriptions
        this._unsubscribeAll.next();
        this._unsubscribeAll.complete();
        
    }
    ngOnInit(): void {
        this._authService.getUser().subscribe((data: any) => {
            if (data != undefined) this.user = data;
        }
        );
        this._authService.check().subscribe((data: any) => {
            this.is_login = data
            if(this.is_login==true){
                this.loadUser();
            }
        });
        this._fuseMediaWatcherService.onMediaChange$
            .pipe(takeUntil(this._unsubscribeAll))
            .subscribe(({ matchingAliases }) => {

                // Check if the screen is small
                this.isScreenSmall = !matchingAliases.includes('md');
            });
    }
}
