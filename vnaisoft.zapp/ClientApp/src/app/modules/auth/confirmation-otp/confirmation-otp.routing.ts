import { Route } from '@angular/router';
import { AuthConfirmationOtpComponent } from 'app/modules/auth/confirmation-otp/confirmation-otp.component';

export const authConfirmationOtpRoutes: Route[] = [
    {
        path: 'confirmation-otp/:id',
        component: AuthConfirmationOtpComponent
    }
];
