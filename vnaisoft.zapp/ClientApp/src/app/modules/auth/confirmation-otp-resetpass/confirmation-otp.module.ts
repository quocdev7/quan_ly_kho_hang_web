import { NgModule } from '@angular/core';
import { RouterModule } from '@angular/router';
import { MatButtonModule } from '@angular/material/button';
import { FuseCardModule } from '@fuse/components/card';
import { SharedModule } from 'app/shared/shared.module';
import { AuthConfirmationOtpResetPassComponent } from 'app/modules/auth/confirmation-otp-resetpass/confirmation-otp.component';
import { authConfirmationOtpResetPassRoutes } from 'app/modules/auth/confirmation-otp-resetpass/confirmation-otp.routing';



import { NgOtpInputModule } from 'ng-otp-input';
@NgModule({
    declarations: [
        AuthConfirmationOtpResetPassComponent,
     
    ],
    imports     : [
        RouterModule.forChild(authConfirmationOtpResetPassRoutes),
        MatButtonModule,
        FuseCardModule,
        SharedModule, 
        NgOtpInputModule
    ]
})
export class AuthConfirmationOtpResetPassModule
{
   
}
