import { NgModule } from '@angular/core';
import { MatButtonModule } from '@angular/material/button';
import { MatCheckboxModule } from '@angular/material/checkbox';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatIconModule } from '@angular/material/icon';
import { MatInputModule } from '@angular/material/input';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { Route, RouterModule } from '@angular/router';
import { FuseAlertModule } from '@fuse/components/alert';
import { FuseCardModule } from '@fuse/components/card';
import { FusePipesModule } from '@fuse/pipes/pipes.module';
import { TranslocoModule } from '@ngneat/transloco';
import { TranslocoCoreModule } from 'app/core/transloco/transloco.module';
import { SharedModule } from 'app/shared/shared.module';
import { HomePageComponent } from './home.component';


const homeRoutes: Route[] = [
    {
        path     : '',
        component: HomePageComponent
    },
];

@NgModule({
    declarations: [
        HomePageComponent,
    ],
    imports     : [
        RouterModule.forChild(homeRoutes),
        MatButtonModule,
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
export class HomePageModule
{
}
