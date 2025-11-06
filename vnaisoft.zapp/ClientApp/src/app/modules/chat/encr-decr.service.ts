import { Injectable } from "@angular/core";
import * as CryptoJS from 'crypto-js';
@Injectable({
    providedIn:'root'
})
export class EncrDecrService{
set(keys:any,value:any){
    var key =CryptoJS.enc.Utf8.parse(keys);
    var iv =CryptoJS.enc.Utf8.parse(keys);
    var encrypted=CryptoJS.AES.encrypt(CryptoJS.enc.Utf8.parse(value.toString()),key,
    {
     keySize:128/8,
     iv:iv,
     mode:CryptoJS.mode.CBC,
     padding:CryptoJS.pad.Pkcs7   
    });
    return encrypted.toString();
}
get(keys:any,value:any){
    var key =CryptoJS.enc.Utf8.parse(keys);
    var iv =CryptoJS.enc.Utf8.parse(keys);
    var decrypted=CryptoJS.AES.decrypt(value,key,{
        keySize:128/8,
     iv:iv,
     mode:CryptoJS.mode.CBC,
     padding:CryptoJS.pad.Pkcs7 
    });
    return decrypted.toString(CryptoJS.enc.Utf8);
}
}

// String encryptAESCryptoJSAnyKey(String plainText, String keya) {
//     try {
//       List<int> listint = [];
//       listint.add(1);
//       listint.add(5);
//       listint.add(4);
//       listint.add(2);
//       listint.add(3);
//       listint.add(2);
//       listint.add(1);
//       listint.add(5);
//       final salt = Uint8List.fromList(listint);
//       var keyndIV = deriveKeyAndIV(keya + "a123xczxza234aaxxz@#@z", salt);
//       final key = encrypt.Key(keyndIV["keyBtyes"]);
//       final iv = encrypt.IV(keyndIV["ivBtyes"]);
  
//       final encrypter = encrypt.Encrypter(
//           encrypt.AES(key, mode: encrypt.AESMode.cbc, padding: "PKCS7"));
//       final encrypted = encrypter.encrypt(plainText, iv: iv);
//       return base64.encode(encrypted.bytes);
//     } catch (error) {
//       throw error;
//     }
//   }
  
//   String decryptAESCryptoJSAnyKey(String encrypted, String keya) {
//     try {
//       Uint8List encryptedBytesWithSalt = base64.decode(encrypted);
  
//       List<int> listint = [];
//       listint.add(1);
//       listint.add(5);
//       listint.add(4);
//       listint.add(2);
//       listint.add(3);
//       listint.add(2);
//       listint.add(1);
//       listint.add(5);
//       final salt = Uint8List.fromList(listint);
//       var keyndIV = deriveKeyAndIV(keya + "a123xczxza234aaxxz@#@z", salt);
//       final key = encrypt.Key(keyndIV["keyBtyes"]);
//       final iv = encrypt.IV(keyndIV["ivBtyes"]);
//       final encrypter = encrypt.Encrypter(
//           encrypt.AES(key, mode: encrypt.AESMode.cbc, padding: "PKCS7"));
//       final decrypted =
//           encrypter.decrypt64(base64.encode(encryptedBytesWithSalt), iv: iv);
//       return decrypted;
//     } catch (error) {}
//     return "";
//   }
  
//   deriveKeyAndIV(String passphrase, Uint8List salt) {
//     var password = createUint8ListFromString(passphrase);
//     Uint8List concatenatedHashes = Uint8List(0);
//     Uint8List currentHash = Uint8List(0);
//     bool enoughBytesForKey = false;
//     Uint8List preHash = Uint8List(0);
  
//     while (!enoughBytesForKey) {
//       if (currentHash.length > 0)
//         preHash = Uint8List.fromList(currentHash + password + salt);
//       else
//         preHash = Uint8List.fromList(password + salt);
  
//       var a = preHash.toList();
//       currentHash = Uint8List.fromList(md5.convert(a).bytes);
  
//       concatenatedHashes = Uint8List.fromList(concatenatedHashes + currentHash);
//       if (concatenatedHashes.length >= 48) enoughBytesForKey = true;
//     }
  
//     var keyBtyes = concatenatedHashes.sublist(0, 32);
//     var ivBtyes = concatenatedHashes.sublist(32, 48);
//     return {"keyBtyes": keyBtyes, "ivBtyes": ivBtyes};
//   }
  
//   Uint8List createUint8ListFromString(String s) {
//     var ret = new Uint8List(s.length);
//     for (var i = 0; i < s.length; i++) {
//       ret[i] = s.codeUnitAt(i);
//     }
//     return ret;
//   }
  