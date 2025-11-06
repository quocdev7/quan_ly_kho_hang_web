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

@Component({
    selector: 'popupAddUser',
    templateUrl: 'popupAddUser.component.html',
})
export class popupAddUserComponent extends BasePopUpAddComponent {

    public list_friend_chat: any = [];
    public contacts: any = [];
    public list_friend_chat_chip: any = [];
    imageChangedEvent: any = '';
    constructor(
        public imageCompress: NgxImageCompressService,
        public dialogRef: MatDialogRef<popupAddUserComponent>,
        http: HttpClient, _translocoService: TranslocoService,
        private _fuseMediaWatcherService: FuseMediaWatcherService,
        _fuseNavigationService: FuseNavigationService, route: ActivatedRoute,
        @Inject('BASE_URL') baseUrl: string,
        public dialogModal: MatDialog,
        @Inject(MAT_DIALOG_DATA) data: any) {
        super(_translocoService, _fuseNavigationService, route, baseUrl, http, 'sys_user', dialogRef, dialogModal);
        this.record = data;
        this.get_list_friends();
        this.get_list_thuoc_nhom_chat();
    }

    close_popup(): void {
        this.basedialogRef.close(this.record);

    }
    get_list_friends() {
        this.http.post('sys_user_chat.ctr/get_list_friends', { list_id_user: this.record.list_user_ids }).subscribe(resp => {
            this.list_friend_chat = resp;
            this.contacts = resp;
        })
    }
    get_list_thuoc_nhom_chat() {
        this.http.post('sys_user_chat.ctr/get_list_thuoc_nhom_chat', { list_id_user: this.record.list_user_ids }).subscribe(resp => {
            this.list_friend_chat_chip = resp;
        })
    }
    add_friend(pos) {

        var friend: any = this.contacts[pos];
        this.list_friend_chat_chip.unshift(friend)
        this.record.list_user_ids.unshift(friend.id)
        this.contacts.splice(pos, 1);
        this.list_friend_chat = this.contacts
    }
    remove_friend(pos) {

        var friend = this.list_friend_chat_chip[pos];
        this.contacts.unshift(friend)
        this.list_friend_chat = this.contacts
        this.list_friend_chat_chip.splice(pos, 1);
        this.record.list_user_ids.splice(pos, 1);
    }
    filterChats(query: string): void {
        // Reset the filter
        if (!query) {
            this.contacts = this.list_friend_chat;
            return;
        }
        this.contacts = this.contacts.filter(chat => chat.name.toLowerCase().includes(query.toLowerCase()));
    }
}
