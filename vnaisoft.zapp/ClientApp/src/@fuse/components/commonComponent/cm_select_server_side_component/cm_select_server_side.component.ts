import { Component, OnInit, ViewEncapsulation, Input, EventEmitter, Output, OnDestroy, ViewChild, ChangeDetectorRef } from '@angular/core';
import { FormBuilder, FormGroup, Validators, FormControl } from '@angular/forms';
import Swal from 'sweetalert2';
import { ReplaySubject, Subject } from 'rxjs';
import { filter, tap, takeUntil, debounceTime, map, delay } from 'rxjs/operators';
import { HttpClient } from '@angular/common/http';
import { ProgressSpinnerMode } from '@angular/material/progress-spinner';
import { MatOption, ThemePalette } from '@angular/material/core';
import { MatSelect } from '@angular/material/select';
import { TranslocoService } from '@ngneat/transloco';
@Component({
    selector: 'cm_select_server_side',
    templateUrl: './cm_select_server_side.component.html',
    styleUrls: ['./cm_select_server_side.component.scss']
})
export class cm_select_server_sideComponent implements OnInit {
    @Input() keyError: string;
    @Input() label: string;
    @Input() placeholder: string;
    @Input() model: any;
    @Input() objectChose: any;
    @Input() objectChoseNameInit: any;
    @Input() actionEnum: any = 1;
    @Input() link: string;
    @Input() dataFilter: any;
    @Input() callbackChange: () => void;
    @Input() errorModel: any;
    @Output() modelChange: EventEmitter<any> = new EventEmitter<any>();
    @Output() objectChoseChange: EventEmitter<any> = new EventEmitter<any>();
    @Input() listData: any;
    @Input() type: string;
     
    public color: ThemePalette = 'primary';
    public mode: ProgressSpinnerMode = 'indeterminate';
    public value = 50;

    txtQueryChanged: Subject<string> = new Subject<string>();
    search: string = '';
    public loading: boolean = false;
    public old_value:any='';
    @ViewChild('mySel') skillSel: MatSelect;
    constructor(
        private _changeDetectorRef: ChangeDetectorRef,
        private _translocoService: TranslocoService,
        public http: HttpClient,
    ) {
        if (this.type == '' || this.type == null) this.type = 'single';
        this.txtQueryChanged
            .pipe(debounceTime(700))
            .subscribe((model) => {
                this.listData = [];
                this.http
                    .post(this.link,
                        {
                            search: this.search??"",
                            data: this.dataFilter
                        }
                    ).subscribe((resp) => {
                        this.listData = resp;
                        this.loading = false;
                    });
            });
    }

    ngOnInit(): void {
        if(this.objectChoseNameInit!= undefined && this.objectChoseNameInit!=null){
            this.listData = [{
                id:this.model,
                name:this.objectChoseNameInit
            }];
        }
     
    }
    setChose(data): void {
        if(this.type == 'multiple'){
            if(this.old_value.includes('-1')){
                this.skillSel.options.forEach( (item : MatOption) => item.value=='-1'? item.deselect():null );
                this.model=this.model.filter(d=>d!='-1');
            }else if(data.includes('-1') && !this.old_value.includes('-1')){
                this.model=['-1'];
                this.skillSel.options.forEach( (item : MatOption) => item.value!='-1'? item.deselect():item.select() );
            }
            this.old_value=this.model;
            this._changeDetectorRef.detectChanges();
        }

        this.objectChose = this.listData.filter(d => d.id === data)[0];
        this.objectChoseChange.emit(this.objectChose);
        if (this.callbackChange !== undefined && this.callbackChange !== null) { this.callbackChange(); }


    }
    onChange(query: string): void {

        if (this.search === '' || this.search === undefined) {
            this.loading = false;
            return;
        }
        this.loading = true;
        this.txtQueryChanged.next(query);

    }









}

