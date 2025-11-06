import { NgModule } from '@angular/core';
import { RouterModule } from '@angular/router';
import { MatButtonModule } from '@angular/material/button';
import { FuseCardModule } from '@fuse/components/card';
import { SharedModule } from 'app/shared/shared.module';
import { AuthConfirmationOtpComponent } from 'app/modules/auth/confirmation-otp/confirmation-otp.component';
import { authConfirmationOtpRoutes } from 'app/modules/auth/confirmation-otp/confirmation-otp.routing';

import { NgOtpInputModule } from 'ng-otp-input';
import { lien_heComponent } from './lien_he.component';
import { lien_heRoutes } from './lien_he.routing';
import { MatCheckboxModule } from '@angular/material/checkbox';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { MatIconModule } from '@angular/material/icon';
@NgModule({
    declarations: [
        lien_heComponent
    ],
    imports     : [
        RouterModule.forChild(lien_heRoutes),
        MatButtonModule,
        FuseCardModule,
        SharedModule, 
        NgOtpInputModule,
        MatCheckboxModule,
        MatProgressSpinnerModule,
        MatIconModule
    ]
})
export class lien_heModule
{
   
}
