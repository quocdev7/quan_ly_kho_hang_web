import { NgModule } from '@angular/core';
import { RouterModule } from '@angular/router';
import { MatButtonModule } from '@angular/material/button';
import { MatCheckboxModule } from '@angular/material/checkbox';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatIconModule } from '@angular/material/icon';
import { MatInputModule } from '@angular/material/input';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { FuseCardModule } from '@fuse/components/card';
import { FuseAlertModule } from '@fuse/components/alert';
import { SharedModule } from 'app/shared/shared.module';
import { AuthSignInComponent } from 'app/modules/auth/sign-in/sign-in.component';
import { authSignInRoutes } from 'app/modules/auth/sign-in/sign-in.routing';
import { TranslocoCoreModule } from '../../../core/transloco/transloco.module';
import { TranslocoModule } from '@ngneat/transloco';
import { FusePipesModule } from '../../../../@fuse/pipes/pipes.module';
import { SocialLoginModule, SocialAuthServiceConfig } from 'angularx-social-login';
import { FacebookLoginProvider } from 'angularx-social-login';
import { MatRadioModule } from '@angular/material/radio';
@NgModule({
    providers: [
      
        {
            provide: 'SocialAuthServiceConfig',
            useValue: {
              autoLogin: false,
              providers: [
                {
                  id: FacebookLoginProvider.PROVIDER_ID,
                  provider: new FacebookLoginProvider('791235879807418')
                }
              ]
            } as SocialAuthServiceConfig,
          }
    ],
    declarations: [
        AuthSignInComponent
    ],
    imports: [
        SocialLoginModule,
        RouterModule.forChild(authSignInRoutes),
        MatButtonModule,
        MatRadioModule,
        MatCheckboxModule,
        MatFormFieldModule,
        MatIconModule,
        MatInputModule,
        TranslocoCoreModule,
        TranslocoModule,
        MatProgressSpinnerModule,
        FuseCardModule,
        FuseAlertModule,
        SharedModule,
        FusePipesModule
    ]
})
export class AuthSignInModule
{
}
