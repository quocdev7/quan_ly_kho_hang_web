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
    selector: 'emptyweb-layout',
    templateUrl: './empty_web.component.html',
    encapsulation: ViewEncapsulation.None

})
export class EmptyWebLayoutComponent implements OnDestroy {
    private _unsubscribeAll: Subject<any> = new Subject<any>();

    /**
     * Constructor
     */
    public user: any
    constructor(
        private _changeDetectorRef: ChangeDetectorRef,
        private _router: Router,
        private _userService: UserService,
        private dialog: MatDialog,
        public http: HttpClient) {
        this.loadUser();
    }

    signOut(): void {
        this._router.navigate(['/sign-out']);
    }
    loadUser(): void {
        this.http
            .post('/sys_user.ctr/getUserLogin/', {}
            ).subscribe(resp => {
                this.user = resp;
                this._changeDetectorRef.markForCheck();
            });
    }
    // -----------------------------------------------------------------------------------------------------
    // @ Lifecycle hooks
    // -----------------------------------------------------------------------------------------------------

    /**
     * On destroy
     */
    ngOnDestroy(): void {
        // Unsubscribe from all subscriptions
        this._unsubscribeAll.next();
        this._unsubscribeAll.complete();
    }
}
