import { Component, OnInit, ViewEncapsulation, Input, EventEmitter, Output, OnDestroy } from '@angular/core';
import { FormBuilder, FormGroup, Validators, FormControl } from '@angular/forms';


import { NgxMatDateFormats } from '@angular-material-components/datetime-picker';
import { TranslocoService } from '@ngneat/transloco';
import { isThisSecond } from 'date-fns';
import {CdkTextareaAutosize} from '@angular/cdk/text-field';


import Swal from 'sweetalert2';
import { ReplaySubject, Subject } from 'rxjs';
import { filter, tap, takeUntil, debounceTime, map, delay, debounce } from 'rxjs/operators';
import { HttpClient } from '@angular/common/http';
import { ProgressSpinnerMode } from '@angular/material/progress-spinner';
import { ThemePalette } from '@angular/material/core';
import { MatDialogRef, MatDialog } from '@angular/material/dialog';

import { FuseNavigationService } from '../../navigation/navigation.service';
import { cm_mau_in_popupComponent } from './cm_mau_in_popup.component';



@Component({
    selector     : 'cm_mau_in_button',
    templateUrl: './cm_mau_in_button.component.html'
    
})
export class cm_mau_in_buttonComponent implements OnInit
{
   
    public record: any ={};
    @Input() id_mau_in: any;

    @Output() modelChange: EventEmitter<any> = new EventEmitter<any>();
    @Input() callbackChange: Function;


    constructor(
        private _fuseNavigationService: FuseNavigationService,
        public dialogRef: MatDialogRef<cm_mau_in_buttonComponent>,
        public http: HttpClient,
        private _translocoService: TranslocoService,
        public dialog: MatDialog,
        
    ) {
        

    }

    ngOnInit(): void {

    
    }
   
}

