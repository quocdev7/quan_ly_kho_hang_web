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
import { cm_file_upload_popupComponent } from './cm_file_upload_popup.component';




@Component({
    selector     : 'cm_file_upload_button',
    templateUrl: './cm_file_upload_button.component.html'
    
})
export class cm_file_upload_buttonComponent implements OnInit
{
   
    public record: any ={};
    @Input() id_file_upload: any;
    @Output() modelChange: EventEmitter<any> = new EventEmitter<any>();
    @Input() callbackChange: Function;
    //public currentUser: any =JSON.parse(localStorage.getItem('currentUser'));

    constructor(
        private _fuseNavigationService: FuseNavigationService,
        public dialogRef: MatDialogRef<cm_file_upload_buttonComponent>,
        public http: HttpClient,
        private _translocoService: TranslocoService,
        public dialog: MatDialog,
        
    ) {
        

    }
   
    reload(): void {
        
    }
    ngOnInit(): void {
     
    }
   

    openDialogFile(): void {

        
        if(this.id_file_upload ==null && this.id_file_upload == undefined){
            Swal.fire(this._translocoService.translate('erp.ban_phai_tao_phieu_truoc_khi_upload_file'),"","warning");
            return
        }
        //this.record.actionEnum = 3;
        const dialogRef = this.dialog.open(cm_file_upload_popupComponent, {
            width: '80%',
            height: '80%',
            disableClose: true,
            data: {
                db: {
                    id: this.id_file_upload,
                    list_file: []
                },
                actionEnum:3
            },
        });
        dialogRef.afterClosed().subscribe(result => {
           
          
            if (result != undefined && result != null) {
                //this.model = result;
                //this.record.db.id = result.id;
                //this.modelChange.emit(this.model)
              
              
                if (this.callbackChange != undefined && this.callbackChange != null)
                    this.callbackChange(result);
            }

          
        });
    }
}

