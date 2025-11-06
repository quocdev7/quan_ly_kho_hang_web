import { NgModule } from '@angular/core';
import { RouterModule } from '@angular/router';
import { SharedModule } from 'app/shared/shared.module';
import { EmptyLayoutComponent } from 'app/layout/layouts/empty/empty.component';
import { UserMenuModule } from 'app/layout/common/user-menu/user-menu.module';

@NgModule({
    declarations: [
        EmptyLayoutComponent
    ],
    imports     : [
        UserMenuModule,
        RouterModule,
        SharedModule
    ],
    exports     : [
        EmptyLayoutComponent
    ]
})
export class EmptyLayoutModule
{
}
