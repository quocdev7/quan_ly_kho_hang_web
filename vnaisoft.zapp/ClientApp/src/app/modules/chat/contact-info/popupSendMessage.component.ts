import { Component, Inject } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef, MatDialog } from '@angular/material/dialog';


import { HttpClient, HttpEventType } from '@angular/common/http';
import { BasePopUpAddComponent } from 'app/Basecomponent/BasePopupAdd.component';
import { TranslocoService } from '@ngneat/transloco';
import { FuseNavigationService } from '@fuse/components/navigation';
import { ActivatedRoute } from '@angular/router';
import { v4 as uuidv4 } from 'uuid';
import Swal from 'sweetalert2';
import { FuseMediaWatcherService } from '../../../../@fuse/services/media-watcher';
import { takeUntil } from 'rxjs/operators';
import { Subject } from 'rxjs';

import { DataUrl, NgxImageCompressService } from "ngx-image-compress";
import { base64ToFile, ImageCroppedEvent, LoadedImage } from 'ngx-image-cropper';
import { ChatServiceFireBase } from '../chat-service';
import * as _moment from 'moment';

@Component({
    selector: 'popupSendMessage',
    templateUrl: 'popupSendMessage.component.html',
})
export class popupSendMessageComponent extends BasePopUpAddComponent {

    public list_chat: any = [];
    public contacts: any = [];
    public list_chat_chip: any = [];
    imageChangedEvent: any = '';
    id: any;
    chat: any;
    public currentUser: any = JSON.parse(localStorage.getItem('user'));
    constructor(
        public imageCompress: NgxImageCompressService,
        public dialogRef: MatDialogRef<popupSendMessageComponent>,
        http: HttpClient, _translocoService: TranslocoService,
        private chatService: ChatServiceFireBase,
        _fuseNavigationService: FuseNavigationService, route: ActivatedRoute,
        @Inject('BASE_URL') baseUrl: string,
        public dialogModal: MatDialog,
        @Inject(MAT_DIALOG_DATA) data: any) {
        super(_translocoService, _fuseNavigationService, route, baseUrl, http, 'sys_user', dialogRef, dialogModal);
        this.record = data;

        this.chat = this.record.item
        this.init()
    }
    change_time_ago(date) {
        _moment.locale('vi');
        var time = _moment(date).toNow(true);
        return time
    }
    init() {
        this.chatService.init().subscribe(resp => {
            this.list_chat = resp
            this.list_chat = this.list_chat.filter(q => q.list_user_ids.includes(this.currentUser.id) && q.type == 2 && q.id != this.record.id)
            this.contacts = this.list_chat;
        })
    }
    close_popup(): void {
        this.basedialogRef.close();
    }
    add_friend(pos) {
        var friend: any = this.contacts[pos];
        this.list_chat_chip.unshift(friend)
        this.contacts.splice(pos, 1);
        this.list_chat = this.contacts
    }
    remove_friend(pos) {
        var friend = this.list_chat_chip[pos];
        this.contacts.unshift(friend)
        this.list_chat = this.contacts
        this.list_chat_chip.splice(pos, 1);
    }
    filterChats(query: string): void {

        // Reset the filter
        if (!query) {
            this.list_chat = this.contacts;
            return;
        }
        this.list_chat = this.list_chat.filter(chat => chat.conversation_group_name.toLowerCase().includes(query.toLowerCase()));
    }
}
