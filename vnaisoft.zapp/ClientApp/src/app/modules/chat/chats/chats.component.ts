import { HttpClient } from '@angular/common/http';
import { ChangeDetectionStrategy, ChangeDetectorRef, Component, OnDestroy, OnInit, ViewEncapsulation } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { ActivatedRoute, Router } from '@angular/router';
import { AppConfig } from 'app/core/config/app.config';
import { Observable, Subject } from 'rxjs';
import { ChatServiceFireBase } from '../chat-service';
import { NewChatGroupComponent } from '../new-group-chat/new-group-chat.component';
import { popup_setting_chatComponent } from '../setting-chat/popup-setting.component';
import { AngularFirestore, AngularFirestoreCollection } from '@angular/fire/firestore';
import { BehaviorSubject } from 'rxjs';


const getObservable = (collection: AngularFirestoreCollection<Task>) => {
    const subject = new BehaviorSubject<Task[]>([]);
    collection.valueChanges({ idField: 'id' }).subscribe((val: Task[]) => {
        subject.next(val);
    });
    return subject;
};

@Component({
    selector: 'chat-chats',
    templateUrl: './chats.component.html',
})
export class ChatsComponent implements OnInit, OnDestroy {
    chats: any;
    drawerComponent: 'profile' | 'new-chat';
    drawerOpened: boolean = false;
    filteredChats: any;
    profile: any;
    selectedChat: any;
    public list_friend_chat: any;
    public list_friend_chat_back_up: any;
    list_chat_conversation: any
    private _unsubscribeAll: Subject<any> = new Subject<any>();

    config: AppConfig;
    public currentUser: any = JSON.parse(localStorage.getItem('user'));
    /**
     * Constructor
     */
    constructor(
        public http: HttpClient, public dialog: MatDialog,
        private chatService: ChatServiceFireBase,
        private store: AngularFirestore,
        private router: Router, private route: ActivatedRoute,
        private _changeDetectorRef: ChangeDetectorRef
    ) {
        this.init();
    }

    // -----------------------------------------------------------------------------------------------------
    // @ Lifecycle hooks
    // -----------------------------------------------------------------------------------------------------
    list_chat: any
    init() {
        this.chatService.init().subscribe(resp => {
            this.list_chat = resp
            this.list_chat = this.list_chat.filter(q => q.list_user_ids.includes(this.currentUser.id) && q.type==2)
        })
    }
    chat_conversation_message: any
    get_list_message(id) {
        this.chatService.get_list_message(id).subscribe(resp => {
            this.chat_conversation_message = resp
        })
    }
    get_list_friends() {
        this.http.post('sys_user_chat.ctr/get_list_friend', {}).subscribe(resp => {
            this.list_friend_chat = resp;
            this.list_friend_chat_back_up = resp
        })
    }
    get_profile_user() {
        this.http.post('sys_user.ctr/getUserLogin', {}).subscribe(resp => {
            this.profile = resp;
        })
    }
    get_list_chat_conversation() {
        this.http.post('sys_user_chat.ctr/get_list_chat_conversation', {}).subscribe(resp => {
            this.list_chat_conversation = resp;
        })
    }
    openDialogNewGroupChat(): void {
        const dialogRef = this.dialog.open(NewChatGroupComponent, {
            disableClose: true,
            width: '500px',
            data:{
                group_chat:null
            }
        });
        dialogRef.afterClosed().subscribe(result => {
            this.init();
        });
    }
    openDialogSetting(): void {
        const dialogRef = this.dialog.open(popup_setting_chatComponent, {
            disableClose: true,
            width: '700px',
            data: {
                db: {
                    avatar_path: 'assets/images/logo/logo-bka.png'
                }
            }
        });
        dialogRef.afterClosed().subscribe(result => {
        });
    }
    /**
     * On init
     */
    ngOnInit(): void {
        // Chats
        this.get_profile_user();
        this.get_list_friends();
    }

    /**
     * On destroy
     */
    ngOnDestroy(): void {
        // Unsubscribe from all subscriptions
        this._unsubscribeAll.next(null);
        this._unsubscribeAll.complete();
    }

    // -----------------------------------------------------------------------------------------------------
    // @ Public methods
    // -----------------------------------------------------------------------------------------------------

    /**
     * Filter the chats
     *
     * @param query
     */
    filterChats(query: string): void {
        // Reset the filter
        if (!query) {
            this.list_friend_chat = this.list_friend_chat_back_up;
            return;
        }

        this.list_friend_chat = this.list_friend_chat.filter(chat => chat.name.toLowerCase().includes(query.toLowerCase()));
    }

    /**
     * Open the new chat sidebar
     */
    openNewChat(): void {
        this.drawerComponent = 'new-chat';
        this.drawerOpened = true;
        // Mark for check
    }

    /**
     * Open the profile sidebar
     */
    openProfile(): void {
        this.drawerComponent = 'profile';
        this.drawerOpened = true;

        // Mark for check
    }

    /**
     * Track by function for ngFor loops
     *
     * @param index
     * @param item
     */
    trackByFn(index: number, item: any): any {
        return item.id || index;
    }
}
