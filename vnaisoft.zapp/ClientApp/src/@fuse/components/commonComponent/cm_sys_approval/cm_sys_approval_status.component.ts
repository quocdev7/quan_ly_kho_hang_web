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
import { cm_sys_approval_status_popupComponent } from './cm_sys_approval_status_popup.component';
@Component({
    selector     : 'cm_sys_approval_status',
    templateUrl: './cm_sys_approval_status.component.html'

})
export class cm_sys_approval_statusComponent implements OnInit
{

    @Input() model: any;

    constructor(
        public http: HttpClient,
        public dialog: MatDialog,
    ) {    }
    ngOnInit(): void {
        if (this.model == null) this.model = 0;
    }
    openpopupview(): void {
        const dialogRef = this.dialog.open(cm_sys_approval_status_popupComponent, {
            width: '60vw',
            data: this.model
        });
        dialogRef.afterClosed().subscribe(result => { });
    }

}

