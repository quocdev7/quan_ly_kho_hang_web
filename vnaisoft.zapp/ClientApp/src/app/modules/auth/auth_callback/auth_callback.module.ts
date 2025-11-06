import { NgModule } from '@angular/core';
import { RouterModule } from '@angular/router';
import { MatButtonModule } from '@angular/material/button';
import { FuseCardModule } from '@fuse/components/card';
import { SharedModule } from 'app/shared/shared.module';
import { AuthConfirmationOtpComponent } from 'app/modules/auth/confirmation-otp/confirmation-otp.component';
import { authConfirmationOtpRoutes } from 'app/modules/auth/confirmation-otp/confirmation-otp.routing';

import { NgOtpInputModule } from 'ng-otp-input';
import { AuthCallbackComponent } from './auth_callback.component';
import { authCallbackRoutes } from './auth_callback.routing';
import { MatCheckboxModule } from '@angular/material/checkbox';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { MatIconModule } from '@angular/material/icon';
@NgModule({
    declarations: [
        AuthCallbackComponent
    ],
    imports     : [
        RouterModule.forChild(authCallbackRoutes),
        MatButtonModule,
        FuseCardModule,
        SharedModule, 
        NgOtpInputModule,
        MatCheckboxModule,
        MatProgressSpinnerModule,
        MatIconModule
    ]
})
export class AuthCallBackModule
{
   
}
