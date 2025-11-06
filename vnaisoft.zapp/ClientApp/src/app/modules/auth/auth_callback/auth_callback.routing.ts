import { Route } from '@angular/router';
import { AuthConfirmationOtpComponent } from 'app/modules/auth/confirmation-otp/confirmation-otp.component';
import { AuthCallbackComponent } from './auth_callback.component';

export const authCallbackRoutes: Route[] = [
    {
        path: 'authcallback',
        component: AuthCallbackComponent
    }
];
