import { Route } from '@angular/router';
import { AuthConfirmationOtpComponent } from 'app/modules/auth/confirmation-otp/confirmation-otp.component';
import { lien_heComponent } from './lien_he.component';

export const lien_heRoutes: Route[] = [
    {
        path: 'lien_he',
        component: lien_heComponent
    }
];
