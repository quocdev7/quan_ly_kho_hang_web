import { Component, OnInit, ViewEncapsulation, Input, EventEmitter, Output } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { DateAdapter, MAT_DATE_FORMATS, MAT_DATE_LOCALE, ThemePalette } from '@angular/material/core';
import { NgxMatDateFormats } from '@angular-material-components/datetime-picker';
import { TranslocoService } from '@ngneat/transloco';
import { isThisSecond } from 'date-fns';
import { CdkTextareaAutosize } from '@angular/cdk/text-field';
import { MatDatepicker } from '@angular/material/datepicker';
import * as _moment from 'moment';
import { default as _rollupMoment, Moment } from 'moment';
import { MomentDateAdapter } from '@angular/material-moment-adapter';
//import { MY_FORMATS } from 'app/modules/bao_cao/bao_cao_ton_kho_mat_hang_theo_thang/index.component';
import { MAT_MOMENT_DATE_ADAPTER_OPTIONS } from '../CustomNgxDatetimeAdapter';
const moment_tu_ngay = _rollupMoment || _moment;
const moment_den_ngay = _rollupMoment || _moment;
export const MY_FORMATS = {
    parse: {
        dateInput: ['MM/yyyy', 'DD/MM/yyyy'],
    },
    display: {
        dateInput: 'MM/YYYY',
        monthYearLabel: 'MMM YYYY',
        dateA11yLabel: 'LL',
        monthYearA11yLabel: 'MMMM YYYY',
    },
};

@Component({
    selector: 'cm_month_year',
    templateUrl: './cm_month_year.component.html',
    styleUrls: ['./cm_month_year.component.scss'],
    providers: [
        {
            provide: DateAdapter,
            useClass: MomentDateAdapter,
            deps: [MAT_DATE_LOCALE, MAT_MOMENT_DATE_ADAPTER_OPTIONS],
        },

        //{ provide: MAT_DATE_FORMATS, useValue: MY_FORMATS },
        { provide: MAT_DATE_FORMATS, useValue: MY_FORMATS },
    ],
    encapsulation: ViewEncapsulation.None,
})
export class cm_month_yearComponent implements OnInit {

    @Input() maxlength: any;
    @Input() errorModel: any;
    @Input() enterAction: Function;
    @Input() keyError: string;
    @Input() type: string;
    @Input() attribute: string;
    @Input() label: string;
    @Input() labelAddString: string;
    @Input() suffixstring: string;
    @Input() placeholder: string;
    @Input() decimalPlaces: any = 4;
    @Input() model: any;
    @Input() actionEnum: any = 1;
    @Input() callbackChange: Function;
    @Input() callbackChangeWithParam: Function;
    public id: any;
    @Output() modelChange: EventEmitter<any> = new EventEmitter<any>();
    myOptions: any;
    myOptions_int: any;
    public range = new FormGroup({
        start: new FormControl(new Date(new Date().getFullYear(), new Date().getMonth(), 1).toISOString()),
        end: new FormControl(new Date(new Date().getFullYear(), new Date().getMonth() + 1, 0).toISOString())
    });
    public date: moment.Moment;
    public disabled = false;
    public showSpinners = true;
    public showSeconds = false;
    public touchUi = false;
    public enableMeridian = false;
    public minDate: moment.Moment;
    public maxDate: moment.Moment;
    public stepHour = 1;
    public stepMinute = 1;
    public stepSecond = 1;
    public color: ThemePalette = 'accent';
    timemask = [/\d/, /\d/, ':', /\d/, /\d/];;
    public hide = true;
    constructor(
        private _translocoService: TranslocoService,
    ) {
        if (this.maxlength == '' || this.maxlength == null || this.maxlength == undefined) this.maxlength = 200;
        if (this.model == '' || this.model == null || this.model == undefined) this.model = "";
        if (this.label == 'so_phieu') this.model = this.model.trim();
        if (this.type == '' || this.type == null) this.type = 'text';
        this.model = new Date();
    }

    date_den_ngay = new FormControl(moment_den_ngay());
    date_tu_ngay = new FormControl(moment_tu_ngay());
    chosenYearHandler_tu_ngay(normalizedMonth_tu: Moment) {
        const ctrlValue = this.date_tu_ngay.value;
        ctrlValue.year(normalizedMonth_tu.year());
        this.date_tu_ngay.setValue(ctrlValue);
    }
    chosenMonthHandler_tu_ngay(normalizedMonth_tu: Moment, datepicker: MatDatepicker<Moment>) {
        const ctrlValue = this.date_tu_ngay.value;
        ctrlValue.month(normalizedMonth_tu.month());
        this.date_tu_ngay.setValue(ctrlValue);
        this.model = this.date_tu_ngay.value.format();
        datepicker.close();
    }
    ngOnInit() {

        if (this.myOptions == '' || this.myOptions == null || this.myOptions == undefined) this.myOptions = {
            allowDecimalPadding: 'floats',
            minimumValue: '0',
            decimalPlaces: this.decimalPlaces
        }
        if ((this.placeholder == '' || this.placeholder == null || this.placeholder == undefined) && this.actionEnum != 3)
            this.placeholder = this._translocoService.translate("input_placeholder");
        else
            this.placeholder = this._translocoService.translate(this.placeholder);
        if (this.type == 'number' || this.type == 'readonly_number') {
            if ((this.model == '' || this.model == null || this.model == undefined) && this.model != 0) this.model = null;
        }
        else {
            if ((this.model == '' || this.model == null || this.model == undefined) && this.model != 0) this.model = "";
        }
        if (this.maxlength == '' || this.maxlength == null || this.maxlength == undefined) {
            this.maxlength = 200;
            if (this.type == "number") {
                this.maxlength = 18;
            }
        }

        if (this.type == '' || this.type == null) this.type = 'text';

    }

    setChose(): void {
        if (this.callbackChange != undefined && this.callbackChange != null)
            this.callbackChange();
        if (this.callbackChangeWithParam != undefined && this.callbackChangeWithParam != null)
            this.callbackChangeWithParam(this.label, this.model);
    }
}

