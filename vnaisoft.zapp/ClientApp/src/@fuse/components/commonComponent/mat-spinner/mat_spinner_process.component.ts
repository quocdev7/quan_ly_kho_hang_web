import { Component, OnInit, ViewEncapsulation, Input, EventEmitter, Output, OnDestroy } from '@angular/core';
import { FormBuilder, FormGroup, Validators, FormControl } from '@angular/forms';


import { NgxMatDateFormats } from '@angular-material-components/datetime-picker';
import { TranslocoService } from '@ngneat/transloco';
import { isThisSecond } from 'date-fns';
import { CdkTextareaAutosize } from '@angular/cdk/text-field';


import Swal from 'sweetalert2';
import { ReplaySubject, Subject } from 'rxjs';
import { filter, tap, takeUntil, debounceTime, map, delay, debounce } from 'rxjs/operators';
import { HttpClient } from '@angular/common/http';
import { ProgressSpinnerMode } from '@angular/material/progress-spinner';
import { ThemePalette } from '@angular/material/core';
import { MatDialogRef, MatDialog } from '@angular/material/dialog';

import { FuseNavigationService } from '../../navigation/navigation.service';




@Component({
    selector: 'mat_spinner_process',
    templateUrl: './mat_spinner_process.component.html'

})
export class mat_spinner_processComponent implements OnInit {
    public record: any = {};
    public title: any;
    @Input() header: any;
    @Input() id_file_upload: any;
    @Output() modelChange: EventEmitter<any> = new EventEmitter<any>();
    @Input() callbackChange: Function;
    constructor(
        private _fuseNavigationService: FuseNavigationService,
        public dialogRef: MatDialogRef<mat_spinner_processComponent>,
        public http: HttpClient,
        private _translocoService: TranslocoService,
        public dialog: MatDialog,

    ) {
    }
    ngOnInit(): void {

    }


}

