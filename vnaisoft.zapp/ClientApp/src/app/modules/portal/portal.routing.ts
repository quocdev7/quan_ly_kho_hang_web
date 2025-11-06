/* eslint-disable @typescript-eslint/explicit-function-return-type */
import { ExtraOptions, Route, RouterModule, Routes } from '@angular/router';
import { AuthGuard } from 'app/core/auth/guards/auth.guard';
import { NoAuthGuard } from 'app/core/auth/guards/noAuth.guard';
import { LayoutComponent } from 'app/layout/layout.component';
import { InitialDataResolver } from 'app/app.resolvers';

import { PortalContactUsComponent } from './contact_us/contact_us.component';
import { about_us_indexComponentsComponent } from './about_us/index.component';
import { portal_ky_yeu_indexComponent } from './ky_yeu/index.component';


// @formatter:off
// tslint:disable:max-line-length6

export const portalRoutes: Route[] = [
    
    //{
    //    path: 'portal-module',
    //    component: PortalModuleComponent
    //},
    {
        path: 'portal_ky_yeu_index',
        component: portal_ky_yeu_indexComponent
    },
     {
         path: 'about_us_index',
        component: about_us_indexComponentsComponent
    },
    
      {
        path: 'portal-contact-us',
        component: PortalContactUsComponent
    },
];
