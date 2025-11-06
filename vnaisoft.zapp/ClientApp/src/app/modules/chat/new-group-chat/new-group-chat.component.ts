import { HttpClient, HttpEventType } from '@angular/common/http';
import { ChangeDetectionStrategy, Component, Inject, Input, OnDestroy, OnInit, ViewEncapsulation } from '@angular/core';
import { MatDialog, MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { MatDrawer } from '@angular/material/sidenav';
import { Subject } from 'rxjs';
import { chat_conversation_model } from '../chat.types';
import Swal from 'sweetalert2';
import { v4 as uuidv4 } from 'uuid';
import { DataUrl, NgxImageCompressService } from 'ngx-image-compress';
//import { popupAvatarComponent } from 'app/modules/portal/profile/popupAvatar.component';
import { popupAvatarGroupChatComponent } from './popupAvatar.component';
import { ChatServiceFireBase } from '../chat-service';
import { isThisSecond } from 'date-fns';
import { forEach } from 'lodash';
@Component({
    selector: 'new-group-chat',
    templateUrl: './new-group-chat.component.html',
    styleUrls: ['./new-group-chat.component.scss'],
    encapsulation: ViewEncapsulation.None,
})
export class NewChatGroupComponent {
    @Input() drawer: MatDrawer;
    contacts: any;
    name: any;
    private _unsubscribeAll: Subject<any> = new Subject<any>();
    public group_chat :any
    public currentUser: any = JSON.parse(localStorage.getItem('user'));
    /**
     * Constructor
     */
    constructor(
        public http: HttpClient, public dialog: MatDialog,
        public dialogRef: MatDialogRef<NewChatGroupComponent>, private imageCompress: NgxImageCompressService,
        @Inject(MAT_DIALOG_DATA) data: any,
        private _chatService: ChatServiceFireBase) {
        this.group_chat = data;
        
        this.group_chat.conversation_group_image = 'assets/images/logo/logo-bka.png'
        this.group_chat.conversation_group_user_id_owner = this.currentUser.id
        this.group_chat.status_del = 1
        this.group_chat.type = 2
        this.group_chat.list_user_ids = new Array();
        this.group_chat.id=uuidv4()
    }

    // -----------------------------------------------------------------------------------------------------
    // @ Lifecycle hooks
    // -----------------------------------------------------------------------------------------------------

    /**
     * On init
     */
    openDialogAvatar(): void {
        // model.actionEnum = 2;
        const dialogRef = this.dialog.open(popupAvatarGroupChatComponent, {
            disableClose: true,
            width: '768px',

            data: {
                db: {
                    avatar_path: this.group_chat.conversation_group_image
                }
            }
        });
        dialogRef.afterClosed().subscribe(result => {

        });
    }
    openDialogDetailAvatar(): void {
        const dialogRef = this.dialog.open(popupAvatarGroupChatComponent, {
            disableClose: true,
            width: '768px',
        });
        dialogRef.afterClosed().subscribe(result => {

        });
    }

    close() {
        Swal.fire({
            title: 'Bạn muốn hủy tạo nhóm này?',
            text: "",
            icon: 'warning',
            showCancelButton: true,
            confirmButtonColor: '#3085d6',
            cancelButtonColor: '#d33',
            confirmButtonText: 'Có',
            cancelButtonText: 'Không',
        }).then((result) => {
            if (result.isConfirmed) {
                this.dialogRef.close();
            }
        })
    }
    push_list_id(){
        this.group_chat.list_user_ids.push(this.currentUser.id)
        this.list_friend_chat_chip.forEach(element => {
            this.group_chat.list_user_ids.push(element.id)
        });
    }
    save() {
        this.push_list_id();
        this._chatService.create(this.group_chat).then(resp => {
            Swal.fire({
                title: 'Nhóm tạo thành công.',
                text: "",
                icon: 'success',
            }).then((result) => {
                this.dialogRef.close();
            })
        }).catch(resp => {
            Swal.fire({
                title: 'Nhóm tạo thất bại.',
                text: "",
                icon: 'warning',
            }).then((result) => {
                this.dialogRef.close();
            })
        })
    }
    public list_friend_chat: any;
    public list_friend_chat_chip = new Array();
    get_list_friends() {
        this.http.post('chat.ctr/get_list_friends', {}).subscribe(resp => {
            this.list_friend_chat = resp;
            this.contacts = resp
        })
    }
    add_friend(pos) {
        var friend: any = this.contacts[pos];
        this.list_friend_chat_chip.unshift(friend)
        this.contacts.splice(pos, 1);
        this.list_friend_chat = this.contacts
    }
    remove_friend(pos) {
        var friend = this.list_friend_chat_chip[pos];
        this.contacts.unshift(friend)
        this.list_friend_chat = this.contacts
        this.list_friend_chat_chip.splice(pos, 1);
    }
    filterChats(query: string): void {
        // Reset the filter
        if (!query) {
            this.contacts = this.list_friend_chat;
            return;
        }
        this.contacts = this.contacts.filter(chat => chat.name.toLowerCase().includes(query.toLowerCase()));
    }
    base64ToFile(data, filename) {
        const arr = data.split(',');
        const mime = arr[0].match(/:(.*?);/)[1];
        const bstr = atob(arr[1]);
        let n = bstr.length;
        let u8arr = new Uint8Array(n);

        while (n--) {
            u8arr[n] = bstr.charCodeAt(n);
        }

        return new File([u8arr], filename, { type: mime });
    }
    public imgResultBeforeCompression: string = "";
    public imgResultAfterCompression: string = "";
    chose_file_image(fileInput: any) {
        
        this.file_image = fileInput.target.files;
        var rule_image = 3 * 1048576;
        if (this.file_image[0].size > rule_image) {
            Swal.fire("this._translocoService.translate('system.anh_toi_da_3mb')", "", "warning");
            fileInput.target.value = null;
        } else {
            this.compressFile();
            fileInput.target.value = null;
        }
    }
    DragAndDrop_image(files: any) {


        this.file_image = files;
        var rule_image = 3 * 1048576;
        if (this.file_image[0].size > rule_image) {
            Swal.fire("this._translocoService.translate('system.anh_toi_da_3mb')", "", "warning");
        } else {
            this.compressFile();

        }
    }
    compressFile() {
        var reader = new FileReader();
        reader.readAsDataURL(this.file_image[0]);
        var that = this;
        reader.onload = function () {
            that.imageCompress.compressFile(reader.result.toString(), 1, 50, 50, 3000, 3000) // 50% ratio, 30% quality
                .then((compressedImage: DataUrl) => {
                    that.imgResultAfterCompression = compressedImage;
                    that.file_image = that.base64ToFile(
                        that.imgResultAfterCompression,
                        "image.png"
                    );
                    console.warn(
                        'Size in bytes is now:',
                        that.imageCompress.byteCount(compressedImage)
                    );
                    that.submitFile(false);
                });
            // that.imageCompress.compressFile(reader.result.toString(), 1, 50, 50, 800, 800) // 50% ratio, 30% quality
            //     .then((compressedImage: DataUrl) => {
            //         that.imgResultAfterCompression = compressedImage;
            //         that.file_image = that.base64ToFile(
            //             that.imgResultAfterCompression,
            //             "image.png"
            //         );
            //         console.warn(
            //             'Size in bytes is now:',
            //             that.imageCompress.byteCount(compressedImage)
            //         );
            //         that.submitFile(true);
            //     });
        };


    }
    public file_image: any;
    public Progress_image: any = -1;
    public image: any
    public record: any
    public controller: any
    public actionEnum: any
    submitFile(is_thumbnail: boolean) {

        if (this.file_image != null && this.file_image != undefined) {
            var formData = new FormData();
            this.Progress_image = 0;
            for (var i = 0; i < this.file_image.length; i++) {
                formData.append('list_file[]', this.file_image[i]);
            }
            if (this.record.db.id == null || this.record.db.id == 0) {
                this.record.db.id = uuidv4()
            };
            if (is_thumbnail) {
                if (this.actionEnum == 2) {
                    formData.append('id', this.record.db.id + "editthumbnail");
                } else {
                    formData.append('id', this.record.db.id + "thumbnail");
                }

            } else {
                if (this.actionEnum == 2) {
                    formData.append('id', this.record.db.id + "edit");
                } else {
                    formData.append('id', this.record.db.id);
                }

            }
            formData.append('type', "web");
            formData.append('controller', this.controller.toString());
            formData.append('list_file[]', this.file_image);
            this.record.db.image = "";
            this.http.post('FileManager/uploadimagenew', formData, {
                reportProgress: true,
                observe: 'events'
            })
                .subscribe(res => {
                    
                    if (res.type == HttpEventType.UploadProgress) {

                        this.Progress_image = Math.round((res.loaded / res.total) * 100);


                    } else if (res.type === HttpEventType.Response) {
                        var item: any;
                        item = res.body;
                        if (is_thumbnail) {
                            this.record.db.thumnail = item.location + "&v=" + uuidv4();
                        } else {
                            this.record.db.image = item.location + "&v=" + uuidv4();
                        }

                        this.file_image = null
                        this.Progress_image = -1;
                    }

                })

        } else {

        }


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

}
