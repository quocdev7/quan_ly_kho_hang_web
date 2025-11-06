import { HttpClient } from '@angular/common/http';
import { ChangeDetectionStrategy, Component, Input, ViewEncapsulation } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { MatDrawer } from '@angular/material/sidenav';
import { ActivatedRoute, Router } from '@angular/router';
import { isThisSecond } from 'date-fns';
import { ChatServiceFireBase } from '../chat-service';
import { popupAddUserComponent } from './popupAddUser.component';
import { popupAvatarEditComponent } from './popupAvatar.component';
import { popupEditNameComponent } from './popupEditName.component';
import { popupNotificationComponent } from './popupNotification.component';
import Swal from 'sweetalert2'
import * as AOS from 'aos';
import { popupBackgroundEditComponent } from './popupBackground.component';
@Component({
    selector: 'chat-contact-info',
    templateUrl: './contact-info.component.html',
    styleUrls: ['./contact-info.component.scss'],
})
export class ContactInfoComponent {
    @Input() chat: any
    @Input() chat_conversation_message: any
    @Input() drawer: MatDrawer;
    /**
     * Constructor
     */
    public openTab: any = 3;
    public check_show_file: boolean = false
    public currentUser: any = JSON.parse(localStorage.getItem('user'));
    public id: any
    public list_user: any
    public info_group_chat: any
    public panelOpenStateMedia: boolean = false
    public list_file: any
    public list_image: any
    public list_link: any
    constructor(
        private chatService: ChatServiceFireBase,
        private router: Router, private route: ActivatedRoute,
        public http: HttpClient, public dialog: MatDialog) {
        this.ngOnInit()
    }
    exit_group() {
        Swal.fire({
            title: 'Bạn có chắc muốn rời nhóm?',
            icon: 'warning',
            showCancelButton: true,
            confirmButtonColor: '#3085d6',
            cancelButtonColor: '#d33',
            confirmButtonText: 'Có',
            cancelButtonText: 'Không'
        }).then((result) => {
            if (result.isConfirmed) {
                Swal.fire(
                    'Deleted!',
                    'Your file has been deleted.',
                    'success'
                )
            }
        })
    }
    change_parse_json_url(item) {

        var data = JSON.parse(item)
        return data
    }
    close_show_file() {
        this.check_show_file = !this.check_show_file
    }
    open_show_file(number_tab) {
        this.check_show_file = !this.check_show_file
        this.openTab = number_tab;

        this.list_link = this.chat_conversation_message.filter(q => q.type_message == 1 && q.meta_tag != null)
        this.list_image = this.chat_conversation_message.filter(q => q.status != 2 && q.type_message == 2 && q.extensionFile != 'pdf' && q.extensionFile != null)
        this.list_file = this.chat_conversation_message.filter(q => q.type_message == 2 && q.extensionFile == 'pdf')
        console.log(this.list_image);
    }
    toggleTabs($tabNumber: number) {
        this.openTab = $tabNumber;
    }

    get_list_user() {
        this.http.post('sys_user_chat.ctr/get_list_user', {
            list_id_user: this.chat.list_user_ids
        }).subscribe(resp => {
            this.list_user = resp
        })
    }
    ngOnInit(): void {
        AOS.init({
            duration: 1000
        });
        this.route.params.subscribe(params => {
            this.id = params["id"];
            this.get_list_user();
        });
    }
    formatSizeUnits(bytes) {
        if (bytes >= 1073741824) { bytes = (bytes / 1073741824).toFixed(2) + " GB"; }
        else if (bytes >= 1048576) { bytes = (bytes / 1048576).toFixed(2) + " MB"; }
        else if (bytes >= 1024) { bytes = (bytes / 1024).toFixed(2) + " KB"; }
        else if (bytes > 1) { bytes = bytes + " bytes"; }
        else if (bytes == 1) { bytes = bytes + " byte"; }
        else { bytes = "0 bytes"; }
        return bytes;
    }

    toggle_div(): void {
        document.getElementById("collapseWidthExample").classList.toggle("hidden_div");
    }
    openDialogAvatar(): void {
        const dialogRef = this.dialog.open(popupAvatarEditComponent, {
            disableClose: true,
            width: '768px',

            data: {
                db: {
                    avatar_path: this.chat.conversation_group_image
                }
            }
        });
        dialogRef.afterClosed().subscribe(result => {
            this.chat.conversation_group_image = result
        });
    }
    openDialogBackground(): void {
        const dialogRef = this.dialog.open(popupBackgroundEditComponent, {
            disableClose: true,
            width: '768px',

            data: {
                db: {
                    avatar_path: this.chat.conversation_group_image
                }
            }
        });
        dialogRef.afterClosed().subscribe(result => {

            this.chat.conversation_group_image = result
        });
    }
    openDialogName(): void {
        const dialogRef = this.dialog.open(popupEditNameComponent, {
            disableClose: true,
            width: '768px',

            data: {
                db: {
                    conversation_group_name: this.chat.conversation_group_name
                }
            }
        });
        dialogRef.afterClosed().subscribe(result => {
            this.chat.conversation_group_name = result
        });
    }
    openDialogAddUser(): void {
        const dialogRef = this.dialog.open(popupAddUserComponent, {
            disableClose: true,
            width: '768px',
            data: this.chat
        });
        dialogRef.afterClosed().subscribe(result => {
            this.chat = result;
        });
    }
    openDialogNotification(): void {
        const dialogRef = this.dialog.open(popupNotificationComponent, {
            disableClose: true,
            width: '350px',
            data: this.chat
        });
        dialogRef.afterClosed().subscribe(result => {
            this.chat = result;
        });
    }
}
