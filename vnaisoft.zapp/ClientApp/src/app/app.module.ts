import { NgModule } from '@angular/core';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { ExtraOptions, PreloadAllModules, RouterModule } from '@angular/router';
import { MarkdownModule } from 'ngx-markdown';
import { FuseModule } from '@fuse';
import { FuseConfigModule } from '@fuse/services/config';
import { FuseMockApiModule } from '@fuse/lib/mock-api';
import { CoreModule } from 'app/core/core.module';
import { appConfig } from 'app/core/config/app.config';
import { mockApiServices } from 'app/mock-api';
import { LayoutModule } from 'app/layout/layout.module';
import { AppComponent } from 'app/app.component';
import { appRoutes } from 'app/app.routing';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { AsyncPipe, DatePipe } from '@angular/common';
import { DateAdapter, MAT_DATE_FORMATS, MAT_DATE_LOCALE } from '@angular/material/core';
import { DATE_FORMATS, MomentUtcDateAdapter } from './Basecomponent/config';
import { BrowserModule } from '@angular/platform-browser';
import { AuthInterceptor } from './core/auth/auth.interceptor';
import { AutonumericModule } from '@angularfy/autonumeric';
import { environment } from '../environments/environment';
import { AgmCoreModule } from '@agm/core';
import { AngularFireMessagingModule } from '@angular/fire/messaging';
import { AngularFireDatabaseModule } from '@angular/fire/database';
import { MessagingService } from './core/firebase/messaging.service';
import { AngularFireModule } from '@angular/fire';
import { AngularFirestoreModule } from '@angular/fire/firestore';
import { AngularFireStorageModule } from '@angular/fire/storage';
import { AngularFireAuthModule } from '@angular/fire/auth';
import { MatCarouselModule } from '@ngmodule/material-carousel';
import { QRCodeModule } from 'angular2-qrcode';
import { DragDropDirective } from '@fuse/directives/drag-drop';


export function getBaseUrl() {
    return document.getElementsByTagName('base')[0].href;
}
const providers = [

    { provide: 'BASE_URL', useFactory: getBaseUrl, deps: [] }
];
const routerConfig: ExtraOptions = {
    scrollPositionRestoration: 'enabled',
    preloadingStrategy: PreloadAllModules
};
@NgModule({
    providers: [
        MessagingService,
        AsyncPipe,
        DatePipe,
        providers,
        { provide: HTTP_INTERCEPTORS, useClass: AuthInterceptor, multi: true },
        { provide: DateAdapter, useClass: MomentUtcDateAdapter, deps: [MAT_DATE_LOCALE] },
        { provide: MAT_DATE_FORMATS, useValue: DATE_FORMATS },
      
    ],
    declarations: [
        AppComponent,
    ],
    imports: [
      
        BrowserAnimationsModule,
        RouterModule.forRoot(appRoutes, routerConfig),
        HttpClientModule,
        // Fuse, FuseConfig & FuseMockAPI
        FuseModule,
        FuseConfigModule.forRoot(appConfig),
        FuseMockApiModule.forRoot(mockApiServices),

        // Core module of your application
        CoreModule,
       
        // Layout module of your application
        LayoutModule,
        
        // 3rd party modules that require global configuration via forRoot
        MarkdownModule.forRoot({}),

        //Firebase notifi
        AngularFireDatabaseModule,
        AngularFirestoreModule,
        AngularFireAuthModule,
        AngularFireStorageModule,
        AngularFireMessagingModule,
        AngularFireModule.initializeApp(environment.firebase),
        QRCodeModule,
        MatCarouselModule.forRoot(),
    ],
    bootstrap: [
        AppComponent
    ]
})
export class AppModule {
}
