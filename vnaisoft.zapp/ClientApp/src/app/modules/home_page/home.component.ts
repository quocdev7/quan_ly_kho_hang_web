import { ChangeDetectorRef, Component, OnInit, ViewChild, ViewEncapsulation } from '@angular/core';
import { FormBuilder, FormGroup, NgForm, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { fuseAnimations } from '@fuse/animations';
import { FuseAlertType } from '@fuse/components/alert';
import { AuthService } from 'app/core/auth/auth.service';
import { Location } from '@angular/common';
import { HttpClient, HttpResponse } from '@angular/common/http';
import Swal from 'sweetalert2';
import { UserService } from 'app/core/user/user.service';
import { MatDialog } from '@angular/material/dialog';
@Component({
    selector: 'home-page',
    templateUrl: './home.component.html',
})
export class HomePageComponent {
    /**
     * Constructor
     */
    public user: any;
    public data: any;
    constructor(
        private _changeDetectorRef: ChangeDetectorRef,
        private _router: Router,
        private _userService: UserService,
        private dialog: MatDialog,
        public http: HttpClient) {


        this.loadUser();
    }
    ngOnInit() {

        this.loadUser();
    }
    loadUser(): void {
        this.http
            .post('/sys_user.ctr/getUserLogin/', {}
            ).subscribe(resp => {

                this.user = resp;
                this.data = resp;


            });
    }
    signOut(): void {
        this._router.navigate(['/sign-out']);
    }
}
