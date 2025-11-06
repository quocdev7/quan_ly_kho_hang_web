import { Injectable } from "@angular/core";
import { AngularFirestore } from "@angular/fire/firestore";
@Injectable({
    providedIn: 'root'
})
export class ChatServiceFireBase {
    public currentUser: any = JSON.parse(localStorage.getItem('user'));
    public list_user_firebase: any
    constructor(public fireservice: AngularFirestore) { }
    create(record) {

        return this.fireservice.collection("chat_conversation").add(record);
    }
    init() {
        return this.fireservice.collection('chat_conversation').valueChanges();
    }
    get_list_message(id) {
        return this.fireservice.collection('chat_conversation').doc(id).collection('chat_conversation_message').valueChanges();
    }
    get_info_group_chat(id) {
        return this.fireservice.collection('chat_conversation').doc(id).valueChanges();
    }
}