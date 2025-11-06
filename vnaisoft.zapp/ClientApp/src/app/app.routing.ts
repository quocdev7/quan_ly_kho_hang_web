/* eslint-disable @typescript-eslint/explicit-function-return-type */
import { Route } from '@angular/router';
import { AuthGuard } from 'app/core/auth/guards/auth.guard';
import { NoAuthGuard } from 'app/core/auth/guards/noAuth.guard';
import { LayoutComponent } from 'app/layout/layout.component';
import { InitialDataResolver } from 'app/app.resolvers';
import { AuthCallbackComponent } from './modules/auth/auth_callback/auth_callback.component';


// @formatter:off
// tslint:disable:max-line-length
export const appRoutes: Route[] = [

    // Redirect empty path to '/example'
    { path: '', pathMatch: 'full', redirectTo: '/home' },
    // { path: '', pathMatch: 'full', redirectTo: 'homepage-index'},

    // Redirect signed in user to the '/example'
    //
    // After the user signs in, the sign in page will redirect the user to the 'signed-in-redirect'
    // path. Below is another redirection for that path to redirect the user to the desired
    // location. This is a small convenience to keep all main routes together here on this file.
    //{ path: 'signed-in-redirect', pathMatch: 'full', redirectTo: 'home' },

    { path: 'signed-in-redirect', pathMatch: 'full', redirectTo: '/home' },


    // { path: 'signed-in-redirect', pathMatch: 'full', redirectTo: 'homepage-index' },
    // Auth routes for guests


    // {
    //     path: '',
    //     component: LayoutComponent,
    //     data: {
    //         layout: 'empty'
    //     },
    //     children: [
    //         { path: '', loadChildren: () => import('app/modules/help/help.module').then(m => m.helpModule) },
    //     ]
    // },
    // {
    //     // ĐÂY LÀ ROUTE ĐÃ ĐƯỢC SỬA LẠI
    //     // Nó phải khớp chính xác với đường dẫn mà Sở GD&ĐT trả về
    //     path: 'authcallback',
    //     component: AuthCallbackComponent
    // },
    // {
    //     path: '',
    //     component: LayoutComponent,
    //     data: {
    //         layout: 'empty'
    //     },
    //     children: [
    //         { path: 'chat', loadChildren: () => import('app/modules/chat/chat.module').then(m => m.ChatModule) },
    //     ]
    // },

    {
        path: 'portal_ky_yeu_index',
        component: LayoutComponent,
        data: {
            layout: 'portalempty'
        },
        children: [
            { path: '', loadChildren: () => import('app/modules/portal/portal.module').then(m => m.PortalModule) },
        ]
    },
    {
        path: '',
        component: LayoutComponent,
        data: {
            layout: 'portal'
        },
        children: [
            { path: '', loadChildren: () => import('app/modules/portal/portal.module').then(m => m.PortalModule) },
        ]
    },    {
        path: '',
        component: LayoutComponent,
        data: {
            layout: 'empty'
        },
        children: [
            { path: '', loadChildren: () => import('app/modules/auth/lien_he/lien_he.module').then(m => m.lien_heModule) },
        ]
    },

        // { path: '', loadChildren: () => import('app/modules/auth/lien_he/lien_he.module').then(m => m.lien_heModule) },
    {
        path: '',
        canActivate: [NoAuthGuard],
        canActivateChild: [NoAuthGuard],
        component: LayoutComponent,
        data: {
            layout: 'empty'
        },
        children: [
            // eslint-disable-next-line max-len
            { path: '', loadChildren: () => import('app/modules/auth/auth_callback/auth_callback.module').then(m => m.AuthCallBackModule) },
       
            { path: '', loadChildren: () => import('app/modules/auth/confirmation-otp/confirmation-otp.module').then(m => m.AuthConfirmationOtpModule) },
            { path: 'confirmation-required', loadChildren: () => import('app/modules/auth/confirmation-required/confirmation-required.module').then(m => m.AuthConfirmationRequiredModule) },
            { path: 'forgot-password', loadChildren: () => import('app/modules/auth/forgot-password/forgot-password.module').then(m => m.AuthForgotPasswordModule) },
            { path: 'reset-password', loadChildren: () => import('app/modules/auth/reset-password/reset-password.module').then(m => m.AuthResetPasswordModule) },
            { path: 'sign-in', loadChildren: () => import('app/modules/auth/sign-in/sign-in.module').then(m => m.AuthSignInModule) },
            { path: '', loadChildren: () => import('app/modules/auth/confirmation-otp-resetpass/confirmation-otp.module').then(m => m.AuthConfirmationOtpResetPassModule) },
            { path: '', loadChildren: () => import('app/modules/auth/confirmation-resetpass-success/confirmation-success.module').then(m => m.ResetPassSuccessModule) },
        ]
    },
    // Auth routes for authenticated users
    {
        path: '',
        canActivate: [AuthGuard],
        canActivateChild: [AuthGuard],
        component: LayoutComponent,
        data: {
            layout: 'empty'
        },
        children: [
            { path: 'sign-out', loadChildren: () => import('app/modules/auth/sign-out/sign-out.module').then(m => m.AuthSignOutModule) },
            { path: 'unlock-session', loadChildren: () => import('app/modules/auth/unlock-session/unlock-session.module').then(m => m.AuthUnlockSessionModule) }
        ]
    },

    {
        path: '',
        component: LayoutComponent,
        data: {
            layout: 'empty'
        },
        children: [
            { path: 'sign-up', loadChildren: () => import('app/modules/auth/sign-up/sign-up.module').then(m => m.AuthSignUpModule) },
            { path: 'confirmation-account/:id', loadChildren: () => import('app/modules/auth/confirmation-account/confirmation-account.module').then(m => m.ConfirmationAccountModule) },
        ]
    },

    {
        path: '',
        component: LayoutComponent,
        data: {
            layout: 'emptyweb'
        },
        children: [
            { path: 'home-page', loadChildren: () => import('app/modules/home_page/home.module').then(m => m.HomePageModule) },
        ]
    },



    {
        path: '',
        component: LayoutComponent,
        data: {
            layout: 'emptyNonlogin'
        },
        children: [
            { path: '', loadChildren: () => import('app/modules/invitationNonLogin/index.module').then(m => m.InvitationNonLoginModule) },
        ]
    },
    // Admin routes
    {
        path: '',
        canActivate: [AuthGuard],
        canActivateChild: [AuthGuard],
        component: LayoutComponent,
        resolve: {
            initialData: InitialDataResolver,
        },
        children: [
            { path: 'home', loadChildren: () => import('app/modules/home/home.module').then(m => m.HomeModule) },
            { path: '', loadChildren: () => import('app/modules/system/system.module').then(m => m.SystemModule) },
            //   { path: '', loadChildren: () => import('app/modules/giao_vien/giao_vien.module').then(m => m.GiaoVienModule) },
            // { path: '', loadChildren: () => import('app/modules/thoi_khoa_bieu/thoi_khoa_bieu.module').then(m => m.thoi_khoa_bieuModule) },
            //  { path: '', loadChildren: () => import('app/modules/ke_hoach_tuyen_sinh/ke_hoach_tuyen_sinh.module').then(m => m.ke_hoach_tuyen_sinhModule) },
            { path: '', loadChildren: () => import('app/modules/file_manager/file_manger.module').then(m => m.FileManagerModule) },
            //{ path: '', loadChildren: () => import('app/modules/portal_ky_yeu/ky_yeu.module').then(m => m.KyYeuModule) },
            
        ]
    },



    {
        path: '**',
        loadChildren: () => import('app/modules/error/error.module').then(m => m.Error404Module)
    },
];
