import { Component } from '@angular/core';
import { AngularFirestore } from '@angular/fire/firestore';
import moment from 'moment';
import { Subject } from 'rxjs';
import { takeUntil } from 'rxjs/operators';
import { MessagingService } from './core/firebase/messaging.service';
import { sub_device, token_noti_user } from './core/models/firebase_model';
import { User } from './core/user/user.model';
import { UserService } from './core/user/user.service';
import { DeviceUUID } from 'device-uuid';
import { TranslocoService } from '@ngneat/transloco';
import { HubConnection, HubConnectionBuilder } from '@microsoft/signalr';
@Component({
    selector: 'app-root',
    templateUrl: './app.component.html',
    styleUrls: ['./app.component.scss']
})
export class AppComponent {
    title = 'push-notification';
    message;
    user: User;
    private hubConnection: HubConnection | undefined;
    private _unsubscribeAll: Subject<any> = new Subject<any>();
    /**
     * Constructor
     */
    constructor(private messagingService: MessagingService, private db: AngularFirestore, private _userService: UserService, private translocoService: TranslocoService) {
     
        this._userService.user$
            .pipe(takeUntil(this._unsubscribeAll))
            .subscribe((user: User) => {
                this.user = user;
              
            });
    }
    // init_hub(): void {
    //     this.hubConnection = new HubConnectionBuilder()
    //     .withUrl('/SignalChatGPTHub')
    //     .build();

    //     this.hubConnection.on('ReceiveMessage', ( message) => {
    //         // Handle incoming message
    //     });
    //     this.hubConnection.on('FinishReceiveMessage', ( message) => {
    //     // Handle incoming message
    // });
    //     this.hubConnection.start()
    //         .catch(err => console.error(err));
    // }
    ngOnInit() {
        this.messagingService.requestPermission();
        this.messagingService.receiveMessage();
        this.message = this.messagingService.currentMessage;
       //this.init_hub();


    }
}
