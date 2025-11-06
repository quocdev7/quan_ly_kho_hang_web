import { ChangeDetectionStrategy, ChangeDetectorRef, Component, Input, OnChanges, OnDestroy, OnInit, SimpleChanges, TemplateRef, ViewChild, ViewContainerRef, ViewEncapsulation } from '@angular/core';
import { Overlay, OverlayRef } from '@angular/cdk/overlay';
import { TemplatePortal } from '@angular/cdk/portal';
import { MatButton } from '@angular/material/button';
import { MatDialog } from '@angular/material/dialog';
import { Subject } from 'rxjs';
import { takeUntil } from 'rxjs/operators';
import { Router, ActivatedRoute } from '@angular/router';
import { Notification } from 'app/layout/common/notifications/notifications.types';
import { HttpClient, HttpResponse } from '@angular/common/http';
import { NotificationsService } from 'app/layout/common/notifications/notifications.service';
import { FuseNavigationService } from '@fuse/components/navigation';
import { AvailableLangs, TranslocoService } from '@ngneat/transloco';

import * as _moment from 'moment';
import { isThisQuarter } from 'date-fns';
import { debug } from 'console';

@Component({
    selector: 'notifications',
    templateUrl: './notifications.component.html',
    encapsulation: ViewEncapsulation.None,
    changeDetection: ChangeDetectionStrategy.OnPush,
    exportAs: 'notifications'
})
export class NotificationsComponent implements OnChanges, OnInit, OnDestroy {
    @Input() notifications: Notification[];
    @ViewChild('notificationsOrigin') private _notificationsOrigin: MatButton;
    @ViewChild('notificationsPanel') private _notificationsPanel: TemplateRef<any>;

    unreadCount: number = 0;
    private _overlayRef: OverlayRef;
    private _unsubscribeAll: Subject<any> = new Subject<any>();
    public is_load_more: any = false;
    sum = 20;
    public list_notification: any = {};
    public pageList: any = [];
    public total: any = 0;
    public start: any = 0;
    public total_item: any = 0;
    public loading: any = false;
    public notifications2: Notification[];
    /**
     * Constructor
     */
    constructor(
        private _changeDetectorRef: ChangeDetectorRef, public dialog: MatDialog,
        private _notificationsService: NotificationsService,
        private _overlay: Overlay,
        private route: ActivatedRoute,
        public http: HttpClient, _fuseNavigationService: FuseNavigationService, _translocoService: TranslocoService,
        private _viewContainerRef: ViewContainerRef
    ) {

    }

    getElementById(notification) {
        this.http.post("sys_thong_bao.ctr/getElementById", { id: notification.id_thong_bao }).subscribe(resp => {
            var model = resp;
        })
    }

    change_time_ago(date) {
        _moment.locale('vi');
        var time = _moment(date).toNow(true);
        return time
    }
    load_more() {
        if (this.notifications.length > this.total) return;
        this.is_load_more = true;
        this.start = this.notifications.length;
        this.load_thong_bao(this.is_load_more);
    }

    load_thong_bao(is_load_more): void {
        // if (is_load_more) {
        //     this.http.post('/sys_thong_bao.ctr/get_list_notification_web', { start: this.start }).subscribe(resp => {
        //         var data: any = [];
        //         data = resp;
        //         this.total = data.total;
        //         this.notifications2 = data.notifications;
        //         this.notifications = this.notifications.concat(this.notifications2);
        //         this.unreadCount = data.unreadCount;
        //         this.loading = false;
        //         this.is_load_more = false;
        //         this._changeDetectorRef.markForCheck();
        //     });

        // } else {
        //     this.loading = true;
        //     this.notifications = [];
        //     this.http.post('/sys_thong_bao.ctr/get_list_notification_web', { start: 0 }).subscribe(resp => {
        //         var data: any = [];
        //         data = resp;
        //         this.total = data.total;

        //         this.notifications = data.notifications;
        //         this.unreadCount = data.unreadCount;
        //         this._changeDetectorRef.markForCheck();
        //         this.loading = false;
        //         this.is_load_more = false;
        //     });
        // }

    }
    update_thong_bao_all(): void {
        this.http.post('/sys_thong_bao.ctr/update_thong_bao_all', {}).subscribe(resp => {
            this.load_thong_bao(false);
        });
    }
    update_thong_bao(id, status, link): void {
        this.http.post('/sys_thong_bao.ctr/update_thong_bao/', {
            id: id,
            status: status,
            link: link
        }).subscribe(resp => {
            this.load_thong_bao(false);
        });
    }
    delete_thong_bao(id): void {
        this.http.post('/sys_thong_bao.ctr/delete_thong_bao/', { id: id }).subscribe(resp => {
            this.load_thong_bao(false);
        });
    }
    // -----------------------------------------------------------------------------------------------------
    // @ Lifecycle hooks
    // -----------------------------------------------------------------------------------------------------

    /**
     * On changes
     *
     * @param changes
     */
    ngOnChanges(changes: SimpleChanges): void {
    }

    /**
     * On init
     */
    ngOnInit(): void {
        this.load_thong_bao(false);
        //update UnreadCount real time
        this._changeDetectorRef.markForCheck();
    }

    /**
     * On destroy
     */
    ngOnDestroy(): void {
        // Unsubscribe from all subscriptions
        this._unsubscribeAll.next(null);
        this._unsubscribeAll.complete();

        // Dispose the overlay
        if (this._overlayRef) {
            this._overlayRef.dispose();
        }

        this.load_thong_bao(false);
    }

    // -----------------------------------------------------------------------------------------------------
    // @ Public methods
    // -----------------------------------------------------------------------------------------------------

    /**
     * Open the notifications panel
     */
    openPanel(): void {
        // Return if the notifications panel or its origin is not defined
        if (!this._notificationsPanel || !this._notificationsOrigin) {
            return;
        }

        // Create the overlay if it doesn't exist
        if (!this._overlayRef) {
            this._createOverlay();
        }
        this.load_thong_bao(false)
        // Attach the portal to the overlay
        this._overlayRef.attach(new TemplatePortal(this._notificationsPanel, this._viewContainerRef));
    }

    /**
     * Close the messages panel
     */
    closePanel(): void {
        this._overlayRef.detach();
    }

    /**
     * Mark all notifications as read
     */
    markAllAsRead(): void {
        // Mark all as read
        //this._notificationsService.markAllAsRead().subscribe();
        //this.update_thong_bao_all();
    }

    /**
     * Toggle read status of the given notification
     */
    toggleRead(notification: Notification): void {
        // Toggle the read status
        // Update the notification
        // this._notificationsService.update(notification.id, notification).subscribe();
        notification.read = !notification.read;

    }

    /**
     * Delete the given notification
     */
    delete(notification: Notification): void {
        // Delete the notification
        //this._notificationsService.delete(notification.id).subscribe();
        this.delete_thong_bao(notification.id);

    }



    /**
     * Track by function for ngFor loops
     *
     * @param index
     * @param item
     */
    trackByFn(index: number, item: any): any {
        return item.id || index;
    }

    // -----------------------------------------------------------------------------------------------------
    // @ Private methods
    // -----------------------------------------------------------------------------------------------------

    /**
     * Create the overlay
     */
    private _createOverlay(): void {
        // Create the overlay
        this._overlayRef = this._overlay.create({
            hasBackdrop: true,
            backdropClass: 'fuse-backdrop-on-mobile',
            scrollStrategy: this._overlay.scrollStrategies.block(),
            positionStrategy: this._overlay.position()
                .flexibleConnectedTo(this._notificationsOrigin._elementRef.nativeElement)
                .withLockedPosition()
                .withPush(true)
                .withPositions([
                    {
                        originX: 'start',
                        originY: 'bottom',
                        overlayX: 'start',
                        overlayY: 'top'
                    },
                    {
                        originX: 'start',
                        originY: 'top',
                        overlayX: 'start',
                        overlayY: 'bottom'
                    },
                    {
                        originX: 'end',
                        originY: 'bottom',
                        overlayX: 'end',
                        overlayY: 'top'
                    },
                    {
                        originX: 'end',
                        originY: 'top',
                        overlayX: 'end',
                        overlayY: 'bottom'
                    }
                ])
        });

        // Detach the overlay from the portal on backdrop click
        this._overlayRef.backdropClick().subscribe(() => {
            this._overlayRef.detach();
        });
        this.load_thong_bao(false);
    }

    /**
     * Calculate the unread count
     *
     * @private
     */
    private _calculateUnreadCount(): void {
        let count = 0;
        if (this.notifications && this.notifications.length) {
            count = this.notifications.filter(notification => !notification.read).length;
        }
        this.unreadCount = count;
    }
}
