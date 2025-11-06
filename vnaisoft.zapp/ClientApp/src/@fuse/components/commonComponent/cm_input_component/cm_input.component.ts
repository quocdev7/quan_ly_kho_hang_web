import { Component, OnInit, ViewEncapsulation, Input, EventEmitter, Output } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { DateAdapter, ThemePalette } from '@angular/material/core';
import { NgxMatDateFormats } from '@angular-material-components/datetime-picker';
import { TranslocoService } from '@ngneat/transloco';
import { isThisSecond } from 'date-fns';
import { CdkTextareaAutosize } from '@angular/cdk/text-field';
@Component({
  selector: 'cm_input',
  templateUrl: './cm_input.component.html',
  styleUrls: ['./cm_input.component.scss'],
})
export class cm_inputComponent implements OnInit {
  @Input() rows: any = [];
  @Input() id_textarea: any;
  @Input() stt: any = '';
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
  @Input() decimalPlaces: any = 2;
  @Input() model: any;
  @Input() readonly: any = false;
  @Input() actionEnum: any = 1;
  @Input() is_maxlength: any = true;
  @Input() callbackChange: Function;
  @Input() callbackChangeWithParam: Function;
  @Input() callbackChangeWithParamPos: Function;
  @Output() blur: EventEmitter<any> = new EventEmitter<any>(); // Add blur output
  @Output() enter: EventEmitter<any> = new EventEmitter<any>(); // Add enter output
  // control = new FormControl('', [
  //     Validators.required,
  //     Validators.maxLength(100),
  //     Validators.pattern(/^[\p{L}\s]+$/u) // Chỉ cho phép ký tự chữ và khoảng trắng
  //   ]);

  @Input() pos: any = 0;
  public id: any;
  @Output() modelChange: EventEmitter<any> = new EventEmitter<any>();
  myOptions: any;
  myOptions_int: any;
  public range = new FormGroup({
    start: new FormControl(new Date(new Date().getFullYear(), new Date().getMonth(), 1).toISOString()),
    end: new FormControl(new Date(new Date().getFullYear(), new Date().getMonth() + 1, 0).toISOString()),
  });
  //public pos=0;
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
  timemask = [/\d/, /\d/, ':', /\d/, /\d/];

  public hide = true;
  constructor(private _translocoService: TranslocoService, private dataAdapter: DateAdapter<Date>) {
    if (this.maxlength == '' || this.maxlength == null || this.maxlength == undefined) this.maxlength = 200;
    if (this.model == '' || this.model == null || this.model == undefined) this.model = '';
    if (this.label == 'so_phieu') this.model = this.model.trim();
    if (this.type == '' || this.type == null) this.type = 'text';
  }
  ngOnInit() {
    this.dataAdapter.setLocale('vi-VN');
    if (this.myOptions == '' || this.myOptions == null || this.myOptions == undefined)
      this.myOptions = {
        allowDecimalPadding: 'floats',
        minimumValue: '0',
        decimalPlaces: this.decimalPlaces,
      };
    if ((this.placeholder == '' || this.placeholder == null || this.placeholder == undefined) && this.actionEnum != 3) this.placeholder = this._translocoService.translate('input_placeholder');
    else this.placeholder = this._translocoService.translate(this.placeholder);
    if (this.type == 'number' || this.type == 'readonly_number') {
      if ((this.model == '' || this.model == null || this.model == undefined) && this.model != 0) this.model = null;
    } else {
      if ((this.model == '' || this.model == null || this.model == undefined) && this.model != 0) this.model = '';
    }
    if (this.maxlength == '' || this.maxlength == null || this.maxlength == undefined) {
      this.maxlength = 200;
      if (this.type == 'number') {
        this.maxlength = 18;
      }
    }

    if (this.type == '' || this.type == null) this.type = 'text';
  }

  setChose(): void {
    if (this.callbackChange != undefined && this.callbackChange != null) this.callbackChange();
    if (this.callbackChangeWithParam != undefined && this.callbackChangeWithParam != null) this.callbackChangeWithParam(this.label, this.model);
    if (this.callbackChangeWithParamPos != undefined && this.callbackChangeWithParamPos != null) this.callbackChangeWithParamPos(this.pos);
  }
}
