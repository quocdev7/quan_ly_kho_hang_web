import { Injectable } from '@angular/core';
import { HubConnection, HubConnectionBuilder } from '@microsoft/signalr';

import { BehaviorSubject, Observable, Subject } from 'rxjs'; 
@Injectable({
  providedIn: 'root'
})
export class SignalRService {
  private hubConnection: HubConnection | undefined;


  private messageSubject = new BehaviorSubject<any>(null);
  message$: Observable<any> = this.messageSubject.asObservable();
  private errorSubject = new Subject<any>();
  error$: Observable<any> = this.errorSubject.asObservable();
  private disconnectedSubject = new Subject<void>();
  disconnected$: Observable<void> = this.disconnectedSubject.asObservable();
  private finishedSubject = new Subject<void>();
  finished$: Observable<void> = this.finishedSubject.asObservable();


  constructor() {


  }

  public startConnection(accessToken: string) {
    this.hubConnection = new HubConnectionBuilder()
      .withUrl('/SignalChatGPTHub?accesstoken=' + accessToken)
      .withAutomaticReconnect()
      .build();

    this.hubConnection.on('ReceiveChat', (message) => {
      this.messageSubject.next(message);
    });

    this.hubConnection.on('ErrorReceiveChat', (error) => {
      this.errorSubject.next(error);
    });

    this.hubConnection.on('FinishReceiveChat', () => {
      this.finishedSubject.next();
    });

    this.hubConnection.onclose(() => {
      this.disconnectedSubject.next();
    });

    this.hubConnection.start()
      .catch(err => {
        this.errorSubject.next(err);
      });
  }

  public stopConnection() {
    this.hubConnection.stop();
  }


  
  ngOnInit() {

  }

 
}