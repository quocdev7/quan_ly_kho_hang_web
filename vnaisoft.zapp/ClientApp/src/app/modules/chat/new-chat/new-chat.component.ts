import { HttpClient } from '@angular/common/http';
import { ChangeDetectionStrategy, Component, Input, OnDestroy, OnInit, ViewEncapsulation } from '@angular/core';
import { MatDrawer } from '@angular/material/sidenav';
import { Subject } from 'rxjs';
import { ChatServiceFireBase } from '../chat-service';

@Component({
    selector: 'chat-new-chat',
    templateUrl: './new-chat.component.html',
    encapsulation: ViewEncapsulation.None,
    changeDetection: ChangeDetectionStrategy.OnPush
})
export class NewChatComponent implements OnInit, OnDestroy {
    @Input() drawer: MatDrawer;
    contacts: any;
    private _unsubscribeAll: Subject<any> = new Subject<any>();

    /**
     * Constructor
     */
    constructor(
        public http: HttpClient, private _chatService: ChatServiceFireBase) {
    }

    // -----------------------------------------------------------------------------------------------------
    // @ Lifecycle hooks
    // -----------------------------------------------------------------------------------------------------

    /**
     * On init
     */public list_friend_chat: any;
    get_list_friends() {

        this.http.post('chat.ctr/get_list_friends', {}).subscribe(resp => {
            this.list_friend_chat = resp;
            this.contacts = resp
        })
    }
    filterChats(query: string): void {
        // Reset the filter
        if (!query) {
            this.contacts = this.list_friend_chat;
            return;
        }

        this.contacts = this.contacts.filter(chat => chat.name.toLowerCase().includes(query.toLowerCase()));
    }
    ngOnInit(): void {
        this.get_list_friends();
        // Contacts
        // this._chatService.contacts$
        //     .pipe(takeUntil(this._unsubscribeAll))
        //     .subscribe((contacts: Contact[]) => {
        //         this.contacts = contacts;
        //     });
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
     * Track by function for ngFor loops
     *
     * @param index
     * @param item
     */
    trackByFn(index: number, item: any): any {
        return item.id || index;
    }
}
