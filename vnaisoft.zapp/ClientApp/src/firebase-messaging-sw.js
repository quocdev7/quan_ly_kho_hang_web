importScripts('https://www.gstatic.com/firebasejs/8.10.0/firebase-app.js');
importScripts('https://www.gstatic.com/firebasejs/8.10.0/firebase-messaging.js');


// Initialize the Firebase app in the service worker by passing the generated config
var firebaseConfig = {
    apiKey: "AIzaSyCYTpf-tRjoEKWI3-GVmgoUmozr2f1rMAs",
    authDomain: "vnaisoftbka.firebaseapp.com",
    projectId: "vnaisoftbka",
    storageBucket: "vnaisoftbka.appspot.com",
    messagingSenderId: "57721982673",
    appId: "1:57721982673:web:ac3a88aa4c2363ed5dbff0",
    measurementId: "G-ET4ZHK0SH6"
  };

  firebase.initializeApp(firebaseConfig);

  // Retrieve firebase messaging
  const messaging = firebase.messaging();

  messaging.onBackgroundMessage(function(payload) {
    console.log('Received background message ', payload);

    const notificationTitle = payload.notification.title;
    const notificationOptions = {
      body: payload.notification.body,
    };

    self.registration.showNotification(notificationTitle,
      notificationOptions);
  });

