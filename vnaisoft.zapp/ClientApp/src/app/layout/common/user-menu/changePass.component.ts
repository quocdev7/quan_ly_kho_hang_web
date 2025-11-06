import { Component, Inject, OnDestroy, OnInit, ViewEncapsulation } from '@angular/core';
import { AbstractControl, FormBuilder, FormGroup, ValidationErrors, ValidatorFn, Validators } from '@angular/forms';
import { Subject } from 'rxjs';
import { takeUntil } from 'rxjs/operators';

import { fuseAnimations } from '@fuse/animations';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { HttpClient } from '@angular/common/http';
import Swal from 'sweetalert2';
import { TranslocoService } from '@ngneat/transloco/lib/transloco.service';

@Component({
  selector: 'changePass',
  templateUrl: './changePass.component.html',
  styleUrls: ['./changePass.component.scss'],
  encapsulation: ViewEncapsulation.None,
  animations: fuseAnimations,
})
export class changePassComponent implements OnInit, OnDestroy {
  resetPasswordForm: FormGroup;
  public hide: boolean = true;
  public hideOldPassword = true;
  public hideNewPassword = true;
  public hideConfirmPassword = true;

  // Private
  private _unsubscribeAll: Subject<any>;

  constructor(
    public http: HttpClient,
    private _formBuilder: FormBuilder,
    public dialogRef: MatDialogRef<changePassComponent>,
    @Inject(MAT_DIALOG_DATA) public data: any,
  ) {
    // Configure the layout
    // Set the private defaults
    this._unsubscribeAll = new Subject();
  }

  // -----------------------------------------------------------------------------------------------------
  // @ Lifecycle hooks
  // -----------------------------------------------------------------------------------------------------

  /**
   * On init
   */
  onclose(): void {
    this.dialogRef.close();
  }
  ngOnInit(): void {
    this.resetPasswordForm = this._formBuilder.group({
      oldPassword: ['', [Validators.required]],
      password: [
        '',
        [Validators.required, Validators.minLength(8), Validators.maxLength(20), this.passwordComplexityValidator],
      ],
      passwordConfirm: ['', [Validators.required, this.confirmPasswordValidator.bind(this)]],
    });

    // Update the validity of the 'passwordConfirm' field
    // when the 'password' field changes
    this.resetPasswordForm
      .get('password')
      .valueChanges.pipe(takeUntil(this._unsubscribeAll))
      .subscribe(() => {
        this.resetPasswordForm.get('passwordConfirm').updateValueAndValidity();
      });
  }

  /**
   * On destroy
   */
  ngOnDestroy(): void {
    // Unsubscribe from all subscriptions
    this._unsubscribeAll.next();
    this._unsubscribeAll.complete();
  }
  passwordComplexityValidator(control: AbstractControl): ValidationErrors | null {
    const password = control.value;

    if (!password) return null;

    const hasUpperCase = /[A-Z]/.test(password);
    const hasLowerCase = /[a-z]/.test(password);
    const hasNumber = /\d/.test(password);
    const hasSpecialChar = /[!@#$%^&*(),.?":{}|<>]/.test(password);

    const passwordValid = hasUpperCase && hasLowerCase && hasNumber && hasSpecialChar;

    return !passwordValid ? { passwordComplexity: true } : null;
  }
  confirmPasswordValidator(control: AbstractControl): ValidationErrors | null {
    const password = this.resetPasswordForm?.get('password')?.value;
    const confirmPassword = control.value;

    return password === confirmPassword ? null : { passwordsNotMatching: true };
  }
  changePassword() {
    this.http
      .post('/sys_user.ctr/changePassword/', {
        data: this.resetPasswordForm.value,
      })
      .subscribe(
        (resp) => {
          Swal.fire({
            title: 'Đổi mật khẩu thành công',
            text: '',
            icon: 'success',
            confirmButtonColor: '#3085d6',
            confirmButtonText: 'Đóng',
          }).then((result) => {
            this.dialogRef.close();
          });
        },
        (error) =>
          Swal.fire({
            title: 'Mật khẩu cũ sai',
            text: '',
            icon: 'warning',
            confirmButtonColor: '#3085d6',
            confirmButtonText: 'Đóng',
          }).then((result) => {}),
      );
  }
}

/**
 * Confirm password validator
 *
 * @param {AbstractControl} control
 * @returns {ValidationErrors | null}
 */
export const confirmPasswordValidator: ValidatorFn = (control: AbstractControl): ValidationErrors | null => {
  if (!control.parent || !control) {
    return null;
  }

  const password = control.parent.get('password');
  const passwordConfirm = control.parent.get('passwordConfirm');

  if (!password || !passwordConfirm) {
    return null;
  }

  if (passwordConfirm.value === '') {
    return null;
  }

  if (password.value === passwordConfirm.value) {
    return null;
  }

  return { passwordsNotMatching: true };
};
