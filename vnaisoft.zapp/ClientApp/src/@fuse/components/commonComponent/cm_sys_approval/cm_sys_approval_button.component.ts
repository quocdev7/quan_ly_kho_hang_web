import { Component, OnInit, ViewEncapsulation, Input, EventEmitter, Output, OnDestroy } from '@angular/core';
import { FormBuilder, FormGroup, Validators, FormControl } from '@angular/forms';
import Swal from 'sweetalert2';
import { ReplaySubject, Subject } from 'rxjs';
import { filter, tap, takeUntil, debounceTime, map, delay, debounce } from 'rxjs/operators';
import { HttpClient } from '@angular/common/http';
import { ProgressSpinnerMode } from '@angular/material/progress-spinner';
import { ThemePalette } from '@angular/material/core';
import { MatDialogRef, MatDialog } from '@angular/material/dialog';
import { cm_sys_approval_popupComponent } from './cm_sys_approval_popup.component';

import { TranslocoService } from '@ngneat/transloco';
import { FuseNavigationService } from '@fuse/components/navigation';
import { defaultNavigation } from 'app/mock-api/common/navigation/data';

@Component({
    selector: 'cm_sys_approval_button',
    templateUrl: './cm_sys_approval_button.component.html'

})
export class cm_sys_approval_buttonComponent implements OnInit {
    @Input() id_sys_approval: any;
    @Input() id_record: any;
    @Input() create_by: any;
    @Input() create_date: any;
    @Input() menu: any;
    @Input() model: any;
    @Output() modelChange: EventEmitter<any> = new EventEmitter<any>();
    @Output() id_sys_approvalChange: EventEmitter<any> = new EventEmitter<any>();
    @Input() callbackChange: Function;
    navigation: any;
    public loading: boolean = true;
    public record: any = {
        db: {
            id_sys_approval_config: 0,
        }
    };

    public currentUser: any = JSON.parse(localStorage.getItem('user'));

    constructor(
        private _fuseNavigationService: FuseNavigationService,
        public dialogRef: MatDialogRef<cm_sys_approval_buttonComponent>,
        public http: HttpClient,
        public dialog: MatDialog,
        public _translocoService: TranslocoService,
    ) {

    }
    ngOnInit(): void {
        this.record.db.menu = this.menu;
        this.record.db.id_record = this.id_record;
        this.record.db.create_by_record = this.create_by;
        this.record.db.create_date_record = this.create_date;
        if (this.id_sys_approval == null) {
            this.id_sys_approval = 0;
            this.record.db.id = this.id_sys_approval;
        } else {
            this.record.db.id = this.model.id;
        }

        this.loading = false;
    }
    approval(): void {
        this.loading = true;
        this.record.db.status_action = 2;
        this.http
            .post('sys_approval.ctr/approval/',
                this.record
            ).subscribe(resp => {
                this.loading = false;
                this.model = resp;
                this.modelChange.emit(this.model)
                this.loadMenu();
            });
    }
    cancel(): void {
        Swal.fire({
            title: this._translocoService.translate('common.reason'),
            input: 'text',
            inputAttributes: {
                autocapitalize: 'off'
            },
            showCancelButton: true,
            cancelButtonText: this._translocoService.translate('close'),
            confirmButtonText: this._translocoService.translate('common.confirm'),
            showLoaderOnConfirm: true,
            inputValidator: (value) => {
                if (!value) {
                    return this._translocoService.translate('common.must_input_reason')
                }
            },
            allowOutsideClick: () => false,
        }).then((result) => {
            if (result.value) {
                this.loading = true;
                this.record.db.status_action = 3;
                this.record.db.last_note = result.value;
                this.http
                    .post('sys_approval.ctr/cancel/',
                        this.record
                    ).subscribe(resp => {
                        this.model = resp;
                        this.modelChange.emit(this.model);
                        this.http
                            .post(this.record.db.menu + '.ctr/sync_cancel_approval/',
                                {
                                    id_approval: this.record.db.id,
                                    id_record: this.record.db.id_record
                                }
                            ).subscribe(resp => {
                                this.loading = false;
                            });
                        this.loadMenu();
                    });
            }
        })
    }
    close(): void {
        Swal.fire({
            title: this._translocoService.translate('common.reason'),
            input: 'text',
            inputAttributes: {
                autocapitalize: 'off'
            },
            showCancelButton: true,
            cancelButtonText: this._translocoService.translate('close'),
            confirmButtonText: this._translocoService.translate('common.confirm'),
            showLoaderOnConfirm: true,
            inputValidator: (value) => {
                if (!value) {
                    return this._translocoService.translate('common.must_input_reason')
                }
            },
            allowOutsideClick: () => false,
        }).then((result) => {
            if (result.value) {
                this.loading = true;
                this.record.db.status_action = 6;
                this.record.db.last_note = result.value;
                this.http
                    .post('sys_approval.ctr/close/',
                        this.record
                    ).subscribe(resp => {
                        this.loading = false;
                        this.model = resp;
                        this.modelChange.emit(this.model);
                        this.loadMenu();
                    });
            }
        })

    }
    open(): void {
        Swal.fire({
            title: this._translocoService.translate('common.reason'),
            input: 'text',
            inputAttributes: {
                autocapitalize: 'off'
            },
            showCancelButton: true,
            cancelButtonText: this._translocoService.translate('close'),
            confirmButtonText: this._translocoService.translate('common.confirm'),
            showLoaderOnConfirm: true,
            inputValidator: (value) => {
                if (!value) {
                    return this._translocoService.translate('common.must_input_reason')
                }
            },
            allowOutsideClick: () => false,
        }).then((result) => {
            if (result.value) {
                this.loading = true;
                this.record.db.status_action = 5;
                this.record.db.last_note = result.value;
                this.http
                    .post('sys_approval.ctr/open/',
                        this.record
                    ).subscribe(resp => {
                        this.model = resp;
                        this.modelChange.emit(this.model);
                        this.http
                            .post(this.record.db.menu + '.ctr/sync_cancel_approval/',
                                {
                                    id_approval: this.record.db.id,
                                    id_record: this.record.db.id_record
                                }
                            ).subscribe(resp => {
                                this.loading = false;
                            });
                        this.loadMenu();
                    });
            }
        })
    }
    return(): void {
        Swal.fire({
            title: this._translocoService.translate('common.reason'),
            input: 'text',
            inputAttributes: {
                autocapitalize: 'off'
            },
            showCancelButton: true,
            cancelButtonText: this._translocoService.translate('close'),
            confirmButtonText: this._translocoService.translate('common.return'),
            showLoaderOnConfirm: true,
            inputValidator: (value) => {
                if (!value) {
                    return this._translocoService.translate('common.must_input_reason')
                }
            },
            allowOutsideClick: () => false,
        }).then((result) => {
            if (result.value) {
                this.loading = true;
                this.record.db.status_action = 3;
                this.record.db.last_note = result.value;
                this.http
                    .post('sys_approval.ctr/approval/',
                        this.record
                    ).subscribe(resp => {
                        this.loading = false;
                        this.model = resp;
                        this.modelChange.emit(this.model);
                        this.loadMenu();
                    });
            }
        })

    }
    loadMenu(): void {
        this.http
            .post('/sys_home.ctr/getModule/', {
            }
            ).subscribe(resp => {
                this.menu = resp;
                localStorage.setItem('menu_user', JSON.stringify({ data: resp, time: new Date() }));
                this.loadModule();
            });


    }
    loadModule(): void {
        var navmodule = defaultNavigation;
        var navigationMenu = [];
        for (var i = 0; i < navmodule.length; i++) {
            var menu = this.menu.filter(d => d.menu.module == navmodule[i].id);
            if (menu.length > 0) {

                navmodule[i].children = [{
                    icon: 'receipt',
                    id: 'report',
                    title: 'report',
                    translate: 'NAV.report',
                    type: 'collapsable',
                    children: [],
                }]

                navmodule[i].children[0].children = [];
                for (var j = 0; j < menu.length; j++) {
                    menu[j].menu.badge_approval = menu[j].badge_approval;
                    menu[j].menu.badge_return = menu[j].badge_return;
                    //if (menu[j].badge_approval > 0) {
                    //        menu[j].menu.badge = {
                    //            title: menu[j].badge_approval,
                    //            bg: '#F44336',
                    //            fg: '#FFFFFF'
                    //        };
                    //}
                    if (menu[j].menu.title.includes('report')) {
                        navmodule[i].children[0].children.push(menu[j].menu);
                    }
                    else if (menu[j].menu.title.includes('schedule')) {
                        menu[j].menu.icon = 'today';
                        navmodule[i].children.push(menu[j].menu);
                    }
                    else { navmodule[i].children.push(menu[j].menu); }

                }
                navigationMenu.push(navmodule[i]);
            }
        }
    }


    openDialogApproval(): void {
        this.record.actionEnum = 1;
        const dialogRef = this.dialog.open(cm_sys_approval_popupComponent, {
            width: '60vw',
            disableClose: true,
            data: this.record,
        });
        dialogRef.afterClosed().subscribe(result => {
            if (this.id_sys_approval == 0) {
                this.loading = true;
                this.http
                    .post(this.record.db.menu + '.ctr/sync_approval/',
                        {
                            id_approval: result.id,
                            id_record: this.record.db.id_record
                        }
                    ).subscribe(resp => {
                        this.loading = false;
                    });
            }
            if (result != undefined && result != null) {
                this.model = result;
                this.record.db.id = result.id;
                this.modelChange.emit(this.model)
                this.id_sys_approvalChange.emit(this.id_sys_approval);

                if (this.callbackChange != undefined && this.callbackChange != null)
                    this.callbackChange(result);
            }
            this.loadMenu();


        });
    }
}

