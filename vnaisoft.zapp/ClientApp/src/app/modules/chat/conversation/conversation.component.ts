import { Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { TranslocoService } from '@ngneat/transloco';
import { HttpClient } from '@angular/common/http';
import { AppConfig } from 'app/core/config/app.config';
import { ChatServiceFireBase } from '../chat-service';
import CryptoJS from 'crypto-js';
import { FormControl } from '@angular/forms';
import * as _moment from 'moment';
import * as AOS from 'aos';
import { v4 as uuidv4 } from 'uuid';
import { MatDialog } from '@angular/material/dialog';
import { popupSendMessageComponent } from '../contact-info/popupSendMessage.component';
import { keySecret } from '../secretKey';
//import { Md5 } from 'ts-md5';
@Component({
  selector: 'chat-conversation',
  styleUrls: ['./conversation.component.scss'],
  templateUrl: './conversation.component.html',
})
export class ConversationComponent implements OnInit {
  @ViewChild('messageInput') messageInput: ElementRef;
  drawerMode: 'over' | 'side' = 'side';
  drawerOpened: boolean = false;

  tokenFromUI: string = "0123456789123456";
  encrypted: any = "";
  decrypted: string;
  request: string;
  responce: string;

  config: AppConfig;
  id_friend: any;
  windowScrolled: boolean;
  text_message: any
  chat_conversation_message: any
  info_group_chat: any
  list_sticker: any
  list_sticker_2: any
  start_record: boolean
  public currentUser: any = JSON.parse(localStorage.getItem('user'));
  open_reply: boolean = false;
  chat_reply: any

  /**
   * Constructor
   */
  constructor(
    private http: HttpClient, _translocoService: TranslocoService,
    private chatService: ChatServiceFireBase,
    private route: ActivatedRoute, public dialog: MatDialog,
    private router: Router,
  ) {
    this.start_record = false;
  }
  ngOnInit(): void {
    AOS.init({
      duration: 1000
    });
    this.test_UTF()
    this.route.params.subscribe(params => {
      this.id_friend = params["id"];
      this.get_info_group_chat();
      this.get_list_message()
      this.get_list_sticker()
      this.get_list_sticker_2()
      this.text_message = ""
    });
  }
  reply_chat(chat: any) {
    this.open_reply = true;
    this.chat_reply = chat;
  }
  close_reply_chat() {
    this.open_reply = false;
  }
  openDialogAvatar(item: any): void {
    const dialogRef = this.dialog.open(popupSendMessageComponent, {
      disableClose: true,
      width: '768px',
      data: {
        id: this.id_friend,
        item: item,
      },
    });
    dialogRef.afterClosed().subscribe(result => {
    });
  }
  change_time_ago(date) {
    _moment.locale('vi');
    var time = _moment(date).toNow(true);
    return time
  }
  check_sticker(item, number) {
    var message = {
      conversation_id: null,
      extensionFile: "string;",
      fileName: "string;",
      filePath: "string;",
      fileSize: "number | null",
      file_not_exit: "string;",
      id: "string;",
      is_forward: "string;",
      local_file_path: "string;",
      message: "string;",
      message_id: "string;",
      message_json: "string;",
      message_reply_id: "string;",
      progress_upload: "string;",
      reaction_count: "number | null;",
      reactions: "string;",
      send_time_micro_epoch: "string | null;",
      status: 1,
      status_dell: "number | null;",
      type_message: 0,
      update_time_micro_epoch: "string | null;",
      user_avatar_path: "https://bka.edu.vn/assets/images/groupchat.png",
      user_full_name: "Lê Thanh Thái;",
      user_id: this.currentUser.id,
    }
    message.message = null
    message.type_message = 13
    message.conversation_id = uuidv4()
    message.filePath = item.file_path
    this.chat_conversation_message.push(message)
    this.toggle_div(number)
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
  send() {
    var message = {
      conversation_id: null,
      extensionFile: "string;",
      fileName: "string;",
      filePath: "string;",
      fileSize: "number | null",
      file_not_exit: "string;",
      id: "string;",
      is_forward: "string;",
      local_file_path: "string;",
      message: "string;",
      message_id: "string;",
      message_json: "string;",
      message_reply_id: "string;",
      progress_upload: "string;",
      reaction_count: "number | null;",
      reactions: "string;",
      send_time_micro_epoch: "string | null;",
      status: 1,
      status_dell: "number | null;",
      type_message: 0,
      update_time_micro_epoch: "string | null;",
      user_avatar_path: "https://bka.edu.vn/assets/images/groupchat.png",
      user_full_name: "Lê Thanh Thái;",
      user_id: this.currentUser.id,
    }
    message.type_message = 1
    message.conversation_id = uuidv4()
    message.message = this.text_message
    message.filePath = null
    this.chat_conversation_message.push(message)
    this.text_message = "";
  }
  change_reaction_count(pos) {
    this.chat_conversation_message[pos].reaction_count = this.chat_conversation_message[pos].reaction_count + 1;
  }
  delete_reaction_count(pos) {
    this.chat_conversation_message[pos].reaction_count = this.chat_conversation_message[pos].reaction_count - 1;
  }
  change_parse_json_url(item) {
    var data = JSON.parse(item)
    return data
  }
  get_list_sticker() {
    this.http.post('sys_user_chat.ctr/get_list_sticker', {}).subscribe(resp => {
      this.list_sticker = resp;
    })
  }
  get_list_sticker_2() {
    this.http.post('sys_user_chat.ctr/get_list_sticker_2', {}).subscribe(resp => {
      this.list_sticker_2 = resp;
    })
  }
  toggle_div(number): void {
    if (number === 1)
      document.getElementById("collapseWidthExample").classList.toggle("hidden_div");
    else
      document.getElementById("collapseWidthExample2").classList.toggle("hidden_div");
  }
  check_show_avatar(pos) {
    if (this.chat_conversation_message.length > pos + 1) {
      if (this.chat_conversation_message[pos].user_id == this.chat_conversation_message[pos + 1].user_id)
        return false
      else
        return true
    } else
      return true
  }
  click_record() {
    this.start_record = !this.start_record
  }
  public list_file: any
  get_list_message() {
    this.chatService.get_list_message(this.id_friend).subscribe(resp => {
      this.chat_conversation_message = resp
      console.log(this.chat_conversation_message);
    })
  }
  get_info_group_chat() {
    this.chatService.get_info_group_chat(this.id_friend).subscribe(resp => {
      this.info_group_chat = resp;
    })
  }

  opened: boolean = false;
  searchControl: FormControl = new FormControl();
  open(): void {
    // Return if it's already opened
    if (this.opened) {
      this.opened = false;
      return;
    }

    // Open the search
    this.opened = true;
  }
  close(): void {
    // Return if it's already closed
    if (!this.opened) {
      return;
    }

    // Clear the search input
    this.searchControl.setValue('');

    // Close the search
    this.opened = false;
  }

  searchFunc(): void {
    var value = this.searchControl.value;
    if (value != "" && value != null && value != undefined) {

      const url = '/homepageSearch/' + encodeURIComponent(value);
      if (this.config.layout != 'empty') {

        this.router.navigateByUrl(url);
      } else {
        window.open(url, "_blank");
      }
      this.close();
    }

  }
  openContactInfo(): void {
    this.drawerOpened = !this.drawerOpened;
  }
  resetChat(): void {
  }
  toggleMuteNotifications(): void {
  }
  // encryptAESCryptoJSAnyKey(plainText, keya) {

  //   var listint: any = [1, 5, 4, 2, 3, 2, 1, 5];
  //   var salt = new Uint8Array(listint);

  //   var keyndIV = this.deriveKeyAndIV(keya + "a123xczxza234aaxxz@#@z", salt);


  //   let _key = CryptoJS.enc.Utf8.parse(keyndIV["keyBtyes"]);
  //   let _iv = CryptoJS.enc.Utf8.parse(keyndIV["ivBtyes"]);
  //   let encrypted = CryptoJS.AES.encrypt(
  //     JSON.stringify(plainText), _key, {
  //     keySize: 16,
  //     iv: _iv,
  //     mode: CryptoJS.mode.CBC,
  //     padding: CryptoJS.pad.Pkcs7
  //   });
  //   this.encrypted = encrypted.toString();
  // }

  // decryptAESCryptoJSAnyKey(cipher, keya) {
  //   

  //   var encryptedBytesWithSalt = decodeURI(cipher);


  //   var listint: any = [1, 5, 4, 2, 3, 2, 1, 5];

  //   var salt = new Uint8Array(listint);

  //   var keyndIV = this.deriveKeyAndIV(keya + "a123xczxza234aaxxz@#@z", salt);


  //   let _key = CryptoJS.enc.Utf8.parse(keyndIV["keyBtyes"]);
  //   let _iv = CryptoJS.enc.Utf8.parse(keyndIV["ivBtyes"]);
  //   let encrypte = CryptoJS.AES.encrypt(
  //     JSON.stringify(cipher), _key, {
  //     keySize: 16,
  //     iv: _iv,
  //     mode: CryptoJS.mode.CBC,
  //     padding: CryptoJS.pad.Pkcs7
  //   });
  //   var encrypted = encrypte.toString();
  //   this.decrypted = CryptoJS.AES.decrypt(
  //     encrypted, _key, {
  //     keySize: 16,
  //     iv: _iv,
  //     mode: CryptoJS.mode.ECB,
  //     padding: CryptoJS.pad.Pkcs7
  //   }).toString(CryptoJS.enc.Utf8);
  // }
  // deriveKeyAndIV(passphrase, salt) {
  //   var password = this.createUint8ListFromString(passphrase) as any;
  //   var concatenatedHashes = new Uint8Array(0) as any;
  //   var currentHash = new Uint8Array(0) as any;
  //   var enoughBytesForKey = false;
  //   var preHash = new Uint8Array(0) as any;


  //   while (!enoughBytesForKey) {
  //     if (currentHash.length > 0) {
  //       var rs = currentHash + password + salt;
  //       preHash = new Uint8Array();
  //     }
  //     else
  //       preHash = new Uint8Array(password + salt);

  //     var a = preHash as any;

  //     concatenatedHashes = new Uint8Array(concatenatedHashes + currentHash);
  //     if (concatenatedHashes.length >= 48) enoughBytesForKey = true;
  //   }
  //   const hash = CryptoJS.MD5(CryptoJS.enc.Latin1.parse(a));
  //   const md5 = hash.toString(CryptoJS.enc.Hex)

  //   let utf8Encode = new TextEncoder();
  //   utf8Encode.encode("abc");
  //   currentHash = new Uint8Array(utf8Encode.encode(md5));
  //   var keyBtyes = concatenatedHashes.slice(0, 32);
  //   var ivBtyes = concatenatedHashes.slice(32, 48);
  //   return { "keyBtyes": keyBtyes, "ivBtyes": ivBtyes };
  // }
  createUint8ListFromString(s) {
    var ret = new Uint8Array(s.length);
    for (var i = 0; i < s.length; i++) {
      ret[i] = s.charAt(i);
    }
    return ret;
  }
  set(keys: any, value: any) {
    var key = CryptoJS.enc.Utf8.parse(keys);
    var iv = CryptoJS.enc.Utf8.parse(keys);
    var encrypted = CryptoJS.AES.encrypt(CryptoJS.enc.Utf8.parse(value.toString()), key,
      {
        keySize: 128 / 8,
        iv: iv,
        mode: CryptoJS.mode.CBC,
        padding: CryptoJS.pad.Pkcs7
      });
    return encrypted.toString();
  }
  get(key: any, iv: any, value: any) {

    // var key = CryptoJS.enc.Utf8.parse(keys + this.llave);
    // var iv = CryptoJS.enc.Utf8.parse(keys + this.llave);
    var decrypted = CryptoJS.AES.decrypt(value, key, {
      keySize: 128 / 8,
      iv: iv,
      mode: CryptoJS.mode.CBC,
      padding: CryptoJS.pad.Pkcs7
    });
    return decrypted.toString(CryptoJS.enc.Utf8);
  }
  private llave = keySecret.key;
  decryptAESCryptoJSAnyKey2(cipher, keya) {

    var encryptedBytesWithSalt = decodeURI(cipher);
    var listint: any = [1, 5, 4, 2, 3, 2, 1, 5];
    var salt = new Uint8Array(listint);
    var keyndIV = this.deriveKeyAndIV2(keya + this.llave, cipher)
    let _key = CryptoJS.enc.Utf8.stringify(keyndIV["keyBtyes"]);
    let _iv = CryptoJS.enc.Utf8.stringify(keyndIV["ivBtyes"]);
    // let _key = keyndIV["keyBtyes"];
    // let _iv = keyndIV["ivBtyes"];
    let encrypte = CryptoJS.AES.encrypt(
      JSON.stringify(cipher), _key.toString(), {
      keySize: 16,
      iv: _iv.toString(),
      mode: CryptoJS.mode.CBC,
      padding: CryptoJS.pad.Pkcs7
    });
    var encrypted = encrypte.toString();
    // this.decrypted = CryptoJS.AES.decrypt(
    //   encrypted, _key, {
    //   keySize: 16,
    //   iv: _iv,
    //   mode: CryptoJS.mode.ECB,
    //   padding: CryptoJS.pad.Pkcs7
    // }).toString(CryptoJS.enc.Utf8);
    //return this.decrypted.toString();
    return this.get(_key, _iv, keya)
  }
  deriveKeyAndIV2(passphrase, salt2) {

    var password = this.createUint8ListFromString(passphrase);
    var salt = this.createUint8ListFromString(salt2);
    var concatenatedHashes = this.createUint8ListFromString(0);
    var currentHash = new Uint8Array(0);
    var enoughBytesForKey: boolean = false;
    var preHash: any = [];
    while (!enoughBytesForKey) {
      if (currentHash.length > 0) {
        var currentHash_ref = currentHash as any
        var password_ref = password as any
        var salt_ref = salt as any
        var k = currentHash_ref + password_ref + salt_ref
        preHash = k;
      }
      else {
        var currentHash_ref = currentHash as any

        // var password_ref = password as any = number[]
        var password_ref: any = []
        password_ref = password

        var salt_ref: any = []
        salt_ref = salt
        // var salt_ref = salt as any





        var check = password_ref + ',' + salt_ref


        // var k= password_ref.push(salt_ref)
        preHash = check;
      }
      concatenatedHashes = this.createUint8ListFromString(preHash);
      // concatenatedHashes = new Uint8Array(concatenatedHashes as any + this.createUint8ListFromString(preHash) as any);
      if (preHash.length >= 48)
        enoughBytesForKey = true;
    }
    var a = preHash

    //const hash = CryptoJS.MD5(a);
    // const hash = CryptoJS.MD5(CryptoJS.enc.Latin1.parse(a));
    // const md5 = hash.toString(CryptoJS.enc.Hex)
    // let utf8Encode = new TextEncoder();
    // currentHash = new Uint8Array(utf8Encode.encode(md5));
    var keyBtyes = concatenatedHashes.slice(0, 32);
    var ivBtyes = concatenatedHashes.slice(32, 48);
    return { "keyBtyes": keyBtyes, "ivBtyes": ivBtyes };
  }
  test_UTF() {

    var key_id = "1670829561643140"

    var myString = "VWe7v5KmqQ/etOalJbwk+Q==";
    var sUTF16Base64 = this.decryptAESCryptoJSAnyKey2(key_id, myString);
    console.log(sUTF16Base64);
    console.log(myString);
  }
}