import { Component, Inject } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef, MatDialog } from '@angular/material/dialog';


import { HttpClient } from '@angular/common/http';

import { TranslocoService } from '@ngneat/transloco';
import { BasePopUpAddComponent } from 'app/Basecomponent/BasePopupAdd.component';
import { FuseNavigationService } from '@fuse/components/navigation';
import { ActivatedRoute } from '@angular/router';
import Swal from 'sweetalert2';


@Component({
    selector: 'changePass',
    templateUrl: 'changePass.html',
})
export class changePassComponent extends BasePopUpAddComponent {
    constructor(public dialogRef: MatDialogRef<changePassComponent>,
        http: HttpClient, _translocoService: TranslocoService,
        _fuseNavigationService: FuseNavigationService, route: ActivatedRoute,
        @Inject('BASE_URL') baseUrl: string,
        public dialogModal: MatDialog,
        @Inject(MAT_DIALOG_DATA) data: any) {
        super(_translocoService, _fuseNavigationService, route, baseUrl, http, 'sys_user', dialogRef, dialogModal);
        this.record = data;
        this.Oldrecord = JSON.parse(JSON.stringify(data));
        this.record.password = ""
        //this.record.passwordRegex = /(?=(.*[0-9]))(?=.*[\!@#$%^&*()\\[\]{}\-_+=~`|:;"'<>,./?])(?=.*[a-z])(?=(.*[A-Z]))(?=(.*)).{8,}/;

    }
    changePass(): void {
        const passwordRegex = /(?=(.*[0-9]))(?=.*[\!@#$%^&*()\\[\]{}\-_+=~`|:;"'<>,./?])(?=.*[a-z])(?=(.*[A-Z]))(?=(.*)).{8,}/;
        if (this.record.password === "") {
            Swal.fire({
                title: "Mật khẩu không được trống",
                text: "",
                icon: 'warning',
                confirmButtonColor: '#3085d6',
                confirmButtonText: "Đóng",
            })
        }
        else if (!passwordRegex.test(this.record.password)) {
            Swal.fire({
                title: "Mật khẩu không hợp lệ",
                text: "Mật khẩu phải chứa ít nhất 8 ký tự, bao gồm chữ hoa, chữ thường, số và ký tự đặc biệt.",
                icon: 'warning',
                confirmButtonColor: '#3085d6',
                confirmButtonText: "Đóng",
            })
        }
        else {
            if (this.record.type == 1) {
                this.http
                    .post('/' + this.record.controller + '.ctr/changePasswordByAdmin/', {
                        data: this.record
                    }
                    ).subscribe(resp => {
                        Swal.fire({
                            title: "Đổi mật khẩu thành công",
                            text: "",
                            icon: 'success',
                            confirmButtonColor: '#3085d6',
                            confirmButtonText: "Đóng",
                        }).then((result) => {
                            this.dialogRef.close();
                        })
                    }
                    )
            }
            else {
                this.http
                    .post('/sys_user.ctr/changePasswordCustomer/', {
                        data: this.record
                    }
                    ).subscribe(resp => {
                        Swal.fire({
                            title: "Đổi mật khẩu thành công",
                            text: "",
                            icon: 'success',
                            confirmButtonColor: '#3085d6',
                            confirmButtonText: "Đóng",
                        }).then((result) => {
                            this.dialogRef.close();
                        })
                    }
                    )
            }

        }

    }

}
