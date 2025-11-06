import { Component, OnInit, ViewEncapsulation, Input, EventEmitter, Output, OnDestroy, Inject, ElementRef, ViewChild } from '@angular/core';
import { FormBuilder, FormGroup, Validators, FormControl } from '@angular/forms';
import { TranslocoService } from '@ngneat/transloco';
import Swal from 'sweetalert2';
import { ReplaySubject, Subject } from 'rxjs';
import { filter, tap, takeUntil, debounceTime, map, delay, startWith } from 'rxjs/operators';
import { HttpClient, HttpEventType } from '@angular/common/http';
import { ProgressSpinnerMode } from '@angular/material/progress-spinner';
import { ThemePalette } from '@angular/material/core';
import { MatDialog, MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { FuseNavigationService } from '../../navigation/navigation.service';

import { BasePopUpAddComponent } from 'app/Basecomponent/BasePopupAdd.component';
import { ActivatedRoute } from '@angular/router';
import { listLoaiIn, listBienIn } from 'app/core/data/data';

import { Observable } from 'rxjs';
import { COMMA, ENTER } from '@angular/cdk/keycodes';
import { MatAutocompleteSelectedEvent, MatAutocomplete } from '@angular/material/autocomplete';
import { MatChipInputEvent } from '@angular/material/chips';

@Component({
    selector: 'cm_mau_in_popup',
    templateUrl: './cm_mau_in_popup.component.html'
})
export class cm_mau_in_popupComponent extends BasePopUpAddComponent {


    tieu_de:string;
    noi_dung:string;
    constructor(public dialogRef: MatDialogRef<cm_mau_in_popupComponent>,
        http: HttpClient, _translocoService: TranslocoService,
        _fuseNavigationService: FuseNavigationService, route: ActivatedRoute,
        @Inject('BASE_URL') baseUrl: string,
        public dialogModal: MatDialog,
        @Inject(MAT_DIALOG_DATA) data: any) {
        super(_translocoService, _fuseNavigationService, route, baseUrl, http, 'erp_mau_in', dialogRef, dialogModal);
        this.tieu_de = data.tieu_de;
        this.noi_dung = data.noi_dung;
    }

  
    print():void{
        (document.getElementById("view_print")  as HTMLIFrameElement).contentWindow.print();
    }
}

