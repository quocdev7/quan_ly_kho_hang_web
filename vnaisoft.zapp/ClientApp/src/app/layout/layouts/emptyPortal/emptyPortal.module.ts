import { NgModule } from '@angular/core';
import { RouterModule } from '@angular/router';
import { SharedModule } from 'app/shared/shared.module';
import { EmptyPortalLayoutComponent } from 'app/layout/layouts/emptyPortal/emptyPortal.component';

@NgModule({
    declarations: [
        EmptyPortalLayoutComponent
    ],
    imports     : [
        RouterModule,
        SharedModule
    ],
    exports     : [
        EmptyPortalLayoutComponent
    ]
})
export class EmptyPortalLayoutModule
{
}
