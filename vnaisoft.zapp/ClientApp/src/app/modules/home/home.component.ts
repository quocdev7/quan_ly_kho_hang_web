import { ChangeDetectorRef, Component, QueryList, ViewChildren, ViewEncapsulation } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { ActivatedRoute, Router } from '@angular/router';
import { HubConnection, HubConnectionBuilder } from '@microsoft/signalr';
import { TranslocoService } from '@ngneat/transloco';
import { DataTableDirective } from 'angular-datatables';
import { UserService } from 'app/core/user/user.service';
import { AnimationOptions } from 'ngx-lottie';
import { Subscription } from 'rxjs';
import { AuthService } from 'app/core/auth/auth.service';


import { HttpClient } from '@angular/common/http';
import { trigger, state, style, transition, animate } from '@angular/animations';
import { NgIf } from '@angular/common';
import Swal from 'sweetalert2';
@Component({
    selector: 'home',
    templateUrl: './home.component.html',
    encapsulation: ViewEncapsulation.None,
    //animations: [
    //    trigger('typewriter', [
    //        state('off', style({ opacity: 0 })),
    //        state('on', style({ opacity: 1 })),
    //        transition('off => on', animate('300ms ease-in'))
    //    ])
    //]
})
export class HomeComponent {
    @ViewChildren(DataTableDirective) dtElements: QueryList<DataTableDirective>;
    /**
     * Constructor
     */
    public search: any;

    public kq: any;
    public is_loading: boolean = false;
    public show_text: boolean = false;
    public currentUser: any = JSON.parse(localStorage.getItem('user'));
    public is_mic: boolean = false;
    transcript: string = '';
    subscription: Subscription | undefined;
    typedText = '';
    typingSpeed = 100; // Adjust typing speed in milliseconds
    private hubConnection: HubConnection | undefined;
    constructor(
        private _changeDetectorRef: ChangeDetectorRef,
        private _router: Router,
        public route: ActivatedRoute,
        public _translocoService: TranslocoService,
        private _userService: UserService, public dialog: MatDialog,
        private _authService: AuthService,
        public http: HttpClient
    ) {

    }
    public rerender(): void {
        this.dtElements.forEach((dtElement: DataTableDirective) => {
            dtElement.dtInstance.then((dtInstance: DataTables.Api) => {
                dtInstance.ajax.reload(null, true);
            });
        });
        // Destroy the table first
    };
    lottieOptions_search: AnimationOptions = {
        path: '/assets/jsonfile/siri_processing.json' // path to your JSON file
    };
    lottieOptions_mic: AnimationOptions = {
        path: '/assets/jsonfile/listen.json' // path to your JSON file
    };


    mic(): void {

        this.is_mic = true;
        // this.is_search = false;
        // setTimeout(() => {
        // }, 500);
        // this.is_search = false;
    }
    init_hub(): void {
        var token = this._authService.accessToken;


        this.hubConnection = new HubConnectionBuilder()
            .withUrl('/SignalChatGPTHub?accesstoken=' + token)
            .build();


        this.hubConnection.on('ReceiveChat', (message) => {
            this.is_loading = true;
            //   this.show_text = false;
            console.log(message);
            // Handle incoming message
        });
        this.hubConnection.on('FinishReceiveChat', (message) => {
            this.is_loading = false;

            this.kq = message;


        });
        this.hubConnection.on('ErrorReceiveChat', (message) => {
            this.is_loading = false;

            console.log(message);
            // Handle incoming message
        });
        this.hubConnection.start()
            .catch(err => console.error(err));
    }

    ngOnInit(): void {

        // this.init_hub()


    }
    send_cau_hoi(): void {
        this.is_loading = true;
        //var message ="tôi muốn làm phiếu mua hàng";
        if (this.search == "") {
            Swal.fire('Vui lòng nhập thông tin tìm kiếm', '', 'warning');
            this.is_loading = false;
            return
        }

        var data = {
            search: this.search,
            host: this.currentUser.host
        }
        var jsonString = JSON.stringify(data);
        this.hubConnection?.send('shungo_hoi_dap', jsonString);
    }

}
