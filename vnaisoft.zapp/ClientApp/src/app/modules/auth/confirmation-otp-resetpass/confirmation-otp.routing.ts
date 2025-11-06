import { Route } from '@angular/router';
import { AuthConfirmationOtpResetPassComponent } from 'app/modules/auth/confirmation-otp-resetpass/confirmation-otp.component';

export const authConfirmationOtpResetPassRoutes: Route[] = [
    {
        path: 'confirmation-otp-resetpass',
        component: AuthConfirmationOtpResetPassComponent
    }
];
