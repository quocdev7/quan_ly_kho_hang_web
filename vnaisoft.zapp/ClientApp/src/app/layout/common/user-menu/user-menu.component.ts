import { ChangeDetectionStrategy, ChangeDetectorRef, Component, Input, OnDestroy, OnInit, ViewEncapsulation } from '@angular/core';
import { Router } from '@angular/router';
import { BooleanInput } from '@angular/cdk/coercion';
import { Subject } from 'rxjs';
import { takeUntil } from 'rxjs/operators';
import { User } from 'app/core/user/user.model';
import { UserService } from 'app/core/user/user.service';
import { MatDialog } from '@angular/material/dialog';
import { changePassComponent } from './changePass.component';

import { HttpClient, HttpEventType } from '@angular/common/http';
@Component({
    selector       : 'user-menu',
    templateUrl    : './user-menu.component.html',
    encapsulation  : ViewEncapsulation.None,
    changeDetection: ChangeDetectionStrategy.OnPush,
    exportAs       : 'userMenu'
})
export class UserMenuComponent implements OnInit, OnDestroy
{
    /* eslint-disable @typescript-eslint/naming-convention */
    static ngAcceptInputType_showAvatar: BooleanInput;
    /* eslint-enable @typescript-eslint/naming-convention */

    @Input() showAvatar: boolean = true;
    @Input() isScreenSmall: boolean = false;
    //user: User;
    @Input() is_hide: boolean = false;

    public user: any={};

    private _unsubscribeAll: Subject<any> = new Subject<any>();

    /**
     * Constructor
     */
    constructor(
        private _changeDetectorRef: ChangeDetectorRef,
        private _router: Router,
        private _userService: UserService,
        private dialog: MatDialog,
        public http: HttpClient
    )
    {
    }

    // -----------------------------------------------------------------------------------------------------
    // @ Lifecycle hooks
    // -----------------------------------------------------------------------------------------------------

    /**
     * On init
     */
    loadUser(): void {
      
        this.http
            .post('/sys_user.ctr/getUserLogin/', {}
            ).subscribe(resp => {
                this.user = resp;
                this._changeDetectorRef.markForCheck();
            });
    }
    // init_tk(): void {
      
    //     this.http
    //         .post('/sys_user.ctr/getUserLogin/', {}
    //         ).subscribe(resp => {
    //             this.user = resp;
    //             this._changeDetectorRef.markForCheck();
    //         });
    // }
    ngOnInit(): void
    {
        this.loadUser();
        //this.init_tk();
        // Subscribe to user changes

        //this._userService.user$
        //    .pipe(takeUntil(this._unsubscribeAll))
        //    .subscribe((user: User) => {
        //        this.user = user;

        //        // Mark for check
        //        this._changeDetectorRef.markForCheck();
        //    });

        
    }
    changePass():void{
        const dialogRef = this.dialog.open(changePassComponent, {
            width:"768px",
            disableClose: true,
        });
    }

    /**
     * On destroy
     */
    ngOnDestroy(): void
    {
        // Unsubscribe from all subscriptions
        this._unsubscribeAll.next();
        this._unsubscribeAll.complete();
    }

    // -----------------------------------------------------------------------------------------------------
    // @ Public methods
    // -----------------------------------------------------------------------------------------------------

    /**
     * Update the user status
     *
     * @param status
     */
    updateUserStatus(status: string): void
    {
        // Return if user is not available
        if ( !this.user )
        {
            return;
        }

        // Update the user
        this._userService.update({
            ...this.user,
            status
        }).subscribe();
    }

    /**
     * Sign out
     */
    signOut(): void
    {
        this._router.navigate(['/sign-out']);
    }
    profile(): void {
        this._router.navigate(['/portal-profile']);
    }
    portal_hop_tac_xa(): void {
        this._router.navigate(['/portal_hop_tac_xa_index']);
    }
    mypost(): void {
        this._router.navigate(['/portal_person_news_index']);
    }
    approval_post(): void {
        this._router.navigate(['/portal_approved_person_news_index']);
    }
    approval_user(): void {
        this._router.navigate(['/portal_approved_user_index']);
    }
}
