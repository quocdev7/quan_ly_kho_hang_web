import { Component, HostListener, OnDestroy, OnInit, ViewEncapsulation } from '@angular/core';
import { ActivatedRoute, Data, Router } from '@angular/router';
import { Subject } from 'rxjs';
import { takeUntil } from 'rxjs/operators';
import { FuseMediaWatcherService } from '@fuse/services/media-watcher';
import { FuseNavigationItem, FuseNavigationService, FuseVerticalNavigationComponent } from '@fuse/components/navigation';
import { InitialData } from 'app/app.types';
import { TranslocoService } from '@ngneat/transloco';
import { forEach } from 'lodash';
import { ScrollStrategy, ScrollStrategyOptions } from '@angular/cdk/overlay';
import {  HubConnection, HubConnectionBuilder } from '@microsoft/signalr';
import { AuthService } from 'app/core/auth/auth.service';
import Swal from 'sweetalert2';
import { HttpClient } from '@angular/common/http';
import { SignalRService } from '@fuse/services/signalr.service';
import { AnimationOptions } from 'ngx-lottie';

export interface Contact
{
    id?: string;
    avatar?: string;
    name?: string;
    about?: string;
    details?: {
        emails?: {
            email?: string;
            label?: string;
        }[];
        phoneNumbers?: {
            country?: string;
            phoneNumber?: string;
            label?: string;
        }[];
        title?: string;
        company?: string;
        birthday?: string;
        address?: string;
    };
    attachments?: {
        media?: any[];
        docs?: any[];
        links?: any[];
    };
}

export interface Chat
{
    id?: string;
    contactId?: string;
    contact?: Contact;
    unreadCount?: number;
    muted?: boolean;
    lastMessage?: string;
    lastMessageAt?: string;
    messages?: {
        id?: string;
        chatId?: string;
        contactId?: string;
        isMine?: boolean;
        value?: string;
        createdAt?: string;
    }[];
}
export interface Messages
{
  
    id?: string;
    id_hoi_dap?: string;
    contactId?: string;
    isMine?: boolean;
    new_messsage?: string;
    ngay_tao?: string;
    nguoi_tao?: string;
    
  
}
@Component({
    selector: 'compact-layout',
    templateUrl: './compact.component.html',
    styleUrls: ['./compact.component.scss'],
    encapsulation: ViewEncapsulation.None
})
export class CompactLayoutComponent implements OnInit, OnDestroy {
    data: InitialData;
    isScreenSmall: boolean;
    private _unsubscribeAll: Subject<any> = new Subject<any>();
    listOfOption: Array<{ label: string; value: string }> = [];
    listOfTagOptions = [];
    private _scrollStrategy: ScrollStrategy = this._scrollStrategyOptions.block();
    windowScrolledHeader: boolean;
    public search: any;
    public kq: any;
    public host: any;
    public is_loading: boolean = false;
    public show_text: boolean = false;
    public currentUser: any = JSON.parse(localStorage.getItem('user'));
    chat: Chat;
    chats: Chat[];
    lst_message: Messages[];
    opened: boolean = false;
    selectedChat: Chat;
    private hubConnection: HubConnection | undefined;
    windowScrolled: boolean;
    lottieOptions_search: AnimationOptions;

    message: any;
   
    errorMessage: string;
    /**
     * Constructor
     */
    constructor(
        public http: HttpClient,
        private _activatedRoute: ActivatedRoute,
        private router: Router,
        private _translocoService: TranslocoService,
        private _fuseMediaWatcherService: FuseMediaWatcherService,
        private _fuseNavigationService: FuseNavigationService,
        private _scrollStrategyOptions: ScrollStrategyOptions,
        private _authService: AuthService,
        public  _signalRService: SignalRService
    ) {
        this.lottieOptions_search = {
            path: 'assets/jsonfile/siri_processing.json' // path to your JSON file
        };
        this.opened = false;
        this.chats =[];
this.lst_message =[];
     

        
    }
    get_host(){
        this.http.post('/sys_user.ctr/get_host/', {
           
        }).subscribe(resp => {    
            this.host= resp as any;
        });
    }

    get_list_message(){
        this.http.post('/sys_user.ctr/get_list_message/', {
           
        }).subscribe(resp => {    
            this.lst_message= resp as any;
        });
    }

    // this._signalRService.startConnection(token);
       
    //     this._signalRService.message$.subscribe(message => {
    //         this.message = message;
    //         // Handle message
    //       });
      
    //       this._signalRService.error$.subscribe(error => {
    //         this.errorMessage = error.message || 'An error occurred';
    //       });
      
    //       this._signalRService.disconnected$.subscribe(() => {
    //         //this.isConnected = false;
    //         // Handle disconnection
    //       });
      
    //       this._signalRService.finished$.subscribe(() => {
    //         //this.isFinished = true;
    //         // Handle finish message
    //       });
    init_hub(): void {
        var token = this._authService.accessToken; 
               
 
        this.hubConnection = new HubConnectionBuilder()
         .withUrl('/SignalChatGPTHub?accesstoken='+token)
         .build();

         this.hubConnection.on('ReceiveChat', (message) => {
             this.is_loading = true;
          //   this.show_text = false;
             console.log(message);
             // Handle incoming message
         });
         this.hubConnection.on('FinishReceiveChat', ( message) => {
             this.is_loading = false;

             this.get_list_message();
        //   var rs = message 
        //      const urlRegex = /(https?:\/\/[^\s]+)/g;
        //       var t = rs.replace(urlRegex, '<a href="$1" target="_blank">$1</a>');
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

    openChatBot() {
        this.opened = !this.opened;
    }
    scrollToTop(): void {
        (function smoothscroll() {
            var currentScroll = document.documentElement.scrollTop || document.body.scrollTop;
            if (currentScroll > 0) {
                window.requestAnimationFrame(smoothscroll);
                window.scrollTo(0, currentScroll - (currentScroll / 8));
            }
        })();
    }
    // -----------------------------------------------------------------------------------------------------
    // @ Accessors
    // -----------------------------------------------------------------------------------------------------

    /**
     * Getter for current year
     */
    get currentYear(): number {
        return new Date().getFullYear();
    }

    // -----------------------------------------------------------------------------------------------------
    // @ Lifecycle hooks
    // -----------------------------------------------------------------------------------------------------

    /**
     * On init
     */
    go_to_link(url: any) {
        this.router.navigateByUrl(url);
    }
    send_cau_hoi(): void {
        this.is_loading = true;
        //var message ="tôi muốn làm phiếu mua hàng";
        if (this.search == "" || this.search == null || this.search == undefined) {
           // Swal.fire('Vui lòng nhập thông tin tìm kiếm', '', 'warning');
            this.is_loading = false;
            return
        }
     
        var data = {
          search:this.search,
          host:this.host
        }
        var jsonString = JSON.stringify(data);
        this.hubConnection?.send('shungo_hoi_dap', jsonString);
    }
    selectedValue: ""
    public list: any = []
    ngOnInit(): void {
        //this.init_hub()
        //this.get_list_message();
        // Subscribe to the resolved route data
        this._activatedRoute.data.subscribe((data: Data) => {
            this.data = data.initialData;
        });

        // Subscribe to media changes
        this._fuseMediaWatcherService.onMediaChange$
            .pipe(takeUntil(this._unsubscribeAll))
            .subscribe(({ matchingAliases }) => {

                // Check if the screen is small
                this.isScreenSmall = !matchingAliases.includes('md');
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

    // -----------------------------------------------------------------------------------------------------
    // @ Public methods
    // -----------------------------------------------------------------------------------------------------

    /**
     * Toggle navigation
     *
     * @param name
     */
    toggleNavigation(name: string): void {
        // Get the navigation
        const navigation = this._fuseNavigationService.getComponent<FuseVerticalNavigationComponent>(name);

        if (navigation) {
            // Toggle the opened status
            navigation.toggle();
        }
    }
}
