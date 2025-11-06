import { DOCUMENT } from '@angular/common';
import { HttpClient } from '@angular/common/http';
import { ChangeDetectorRef, Component, Inject, ViewEncapsulation } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { Router } from '@angular/router';
import { TranslocoService } from '@ngneat/transloco';
import { UserService } from 'app/core/user/user.service';
import { isThisSecond } from 'date-fns';

import Swal from 'sweetalert2';
@Component({
    selector: 'invitation',
    templateUrl: './index.component.html',
    styleUrls: ['./index.component.scss'],
    encapsulation: ViewEncapsulation.None
})
export class InvitationComponent {
    /**
     * Constructor
     */
    public user: any;
    public loading = false;
    public errorModel: any;
    public controller: any;
    public record: any
    constructor(
        @Inject(DOCUMENT) private document: Document,
        private _changeDetectorRef: ChangeDetectorRef,
        private _router: Router,
        public _translocoService: TranslocoService,
        private _userService: UserService,
        private dialog: MatDialog,
        public http: HttpClient) {
        this.get_thong_tin_loi_moi()
    }
    get_thong_tin_loi_moi() {

        this.http.post('sys_user.ctr/get_thong_tin_loi_moi', {}).subscribe(resp => {
            this.record = resp
            if (this.record.status_del == 4) {
                Swal.fire('Lời mời đã bị gỡ bỏ', '', 'success')
                    .then(resp => {
                        this._router.navigateByUrl('/sign-in');
                    })
            }
        })


    }
    success_workspace() {
        Swal.fire({
            html:
                '<div class="justify-center items-center md:flex rounded-t-lg  flex-auto background_info">'
                + '<img src="../assets/images/portal/info.png" class="img_sweetalert">'
                + '</div>'
                + '<div class="mt-4 text-center text-3xl  uppercase  text-black font-extrabold tracking-tight leading-tight">'
                + 'Xác nhận'
                + '</div>'
                + '<div class="mt-4 text-center text-xl text-black">'
                + '<span>Bạn có chắc chắn tham gia</span><br><span>' + this.record.name_workspace + ' Workspace không?</span>'
                + '</div>',
            showCancelButton: true,
            confirmButtonColor: '#0d6efd',
            cancelButtonColor: '#6c757d',
            confirmButtonText: 'Có',
            cancelButtonText: 'Không'
        }).then(result => {
            if (result.isConfirmed) {
                this.http.post('sys_user.ctr/access_loi_moi', {
                    id_loi_moi: this.record.db.id
                }).subscribe(resp => {
                    Swal.fire({
                        html:
                            '<div class="justify-center items-center md:flex rounded-t-lg  flex-auto background_success">'
                            + '<img src="../assets/images/portal/success.jpg" class="img_sweetalert">'
                            + '</div>'
                            + '<div class="mt-4 text-center text-3xl  uppercase  text-black font-extrabold tracking-tight leading-tight">'
                            + 'Thành công'
                            + '</div>',
                    }).then(resp => {
                        var host = this.document.location.hostname;
                        var url = "";
                        if (host == "localhost") {
                            url = "https://localhost:44324/sign-in";
                        } else {
                            url = 'https://' + this.document.location.hostname + '/sign-in';
                        }
                        window.location.href = url
                        // this._router.navigateByUrl('../sign-in');
                    })
                })
            } else if (
                /* Read more about handling dismissals below */
                result.dismiss === Swal.DismissReason.cancel

            ) {
                Swal.fire({
                    html:
                        '<div class="justify-center items-center md:flex rounded-t-lg  flex-auto background_error">'
                        + '<img src="../assets/images/portal/error4.jpg" class="img_sweetalert">'
                        + '</div>'
                        + '<div class="mt-4 text-center text-3xl  uppercase  text-black font-extrabold tracking-tight leading-tight">'
                        + 'Thất bại'
                        + '</div>',
                })
            }
        })
    }

}
