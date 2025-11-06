import { NgModule } from '@angular/core';
import { RouterModule } from '@angular/router';
import { MatButtonModule } from '@angular/material/button';
import { FuseCardModule } from '@fuse/components/card';
import { SharedModule } from 'app/shared/shared.module';
import { AuthConfirmationOtpComponent } from 'app/modules/auth/confirmation-otp/confirmation-otp.component';
import { authConfirmationOtpRoutes } from 'app/modules/auth/confirmation-otp/confirmation-otp.routing';

import { NgOtpInputModule } from 'ng-otp-input';
@NgModule({
    declarations: [
        AuthConfirmationOtpComponent
    ],
    imports     : [
        RouterModule.forChild(authConfirmationOtpRoutes),
        MatButtonModule,
        FuseCardModule,
        SharedModule, 
        NgOtpInputModule
    ]
})
export class AuthConfirmationOtpModule
{
   
}
