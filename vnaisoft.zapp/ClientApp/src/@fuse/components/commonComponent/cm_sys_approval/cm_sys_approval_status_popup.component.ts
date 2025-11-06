import { Component, OnInit, ViewEncapsulation, Input, EventEmitter, Output, OnDestroy, Inject } from '@angular/core';
import { FormBuilder, FormGroup, Validators, FormControl } from '@angular/forms';
import Swal from 'sweetalert2';
import { ReplaySubject, Subject } from 'rxjs';
import { filter, tap, takeUntil, debounceTime, map, delay } from 'rxjs/operators';
import { HttpClient } from '@angular/common/http';
import { ProgressSpinnerMode } from '@angular/material/progress-spinner';
import { ThemePalette } from '@angular/material/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';

import { ActivatedRoute } from '@angular/router';
import { FuseNavigationService } from '@fuse/components/navigation';
import { translateDataTable } from '../VietNameseDataTable';
@Component({
    selector: 'cm_sys_approval_status_popup',
    templateUrl: './cm_sys_approval_status_popup.component.html'
})
export class cm_sys_approval_status_popupComponent {
    public record: any;
    public loading: any = true;
    public dtOptions: any;
    constructor(public dialogRef: MatDialogRef<cm_sys_approval_status_popupComponent>,
        public http: HttpClient,
        public route: ActivatedRoute,
        _fuseNavigationService: FuseNavigationService,
        @Inject('BASE_URL') baseUrl: string,
        @Inject(MAT_DIALOG_DATA) data: any) {
        this.record = data;
        this.http
            .post('/sys_approval.ctr/getElementById/',
                {
                    id: this.record.id
                }
            ).subscribe((resp) => {
                this.record = resp;

            });
        this.loading = false;
        this.dtOptions = {
            'language': translateDataTable,
            'retrieve': true,
            'ordering': false,
            'paging': false,
            'searching': false,
            'scrollY': '50vh',
            'scrollCollapse': true,
            'scrollX': true,
        };

    }
    close(): void {
        this.dialogRef.close();
    }

}

