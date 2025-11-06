import { Route } from '@angular/router';

import { ResetPassSuccessComponent } from 'app/modules/auth/confirmation-resetpass-success/confirmation-success.component';

export const resetPassSuccessRoutes: Route[] = [
    {
        path: 'confirmation-resetpass-success',
        component: ResetPassSuccessComponent
    }
];
