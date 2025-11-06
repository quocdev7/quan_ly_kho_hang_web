import { NgModule } from '@angular/core';
import { RouterModule } from '@angular/router';
import { MatButtonModule } from '@angular/material/button';
import { FuseCardModule } from '@fuse/components/card';
import { SharedModule } from 'app/shared/shared.module';

import { ResetPassSuccessComponent } from 'app/modules/auth/confirmation-resetpass-success/confirmation-success.component';
import { resetPassSuccessRoutes } from 'app/modules/auth/confirmation-resetpass-success/confirmation-success.routing';

import { NgOtpInputModule } from 'ng-otp-input';
@NgModule({
    declarations: [
        ResetPassSuccessComponent
    ],
    imports     : [
        RouterModule.forChild(resetPassSuccessRoutes),
        MatButtonModule,
        FuseCardModule,
        SharedModule, 
        NgOtpInputModule
    ]
})
export class ResetPassSuccessModule
{
   
}
