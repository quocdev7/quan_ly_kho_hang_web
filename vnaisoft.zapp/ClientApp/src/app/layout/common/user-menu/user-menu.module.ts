import { NgModule } from '@angular/core';
import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';
import { MatDividerModule } from '@angular/material/divider';
import { MatIconModule } from '@angular/material/icon';
import { MatMenuModule } from '@angular/material/menu';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { UserMenuComponent } from 'app/layout/common/user-menu/user-menu.component';
import { SharedModule } from 'app/shared/shared.module';
import { changePassComponent } from './changePass.component';
import { NotificationsModule } from 'app/layout/common/notifications/notifications.module';
@NgModule({
    declarations: [
        UserMenuComponent,
        changePassComponent
    ],
    imports     : [
        MatButtonModule,
        MatDividerModule,
        MatIconModule,
        MatMenuModule,
        SharedModule,
        MatCardModule,
        MatFormFieldModule,
        MatInputModule,
        NotificationsModule
    ],
    exports     : [
        UserMenuComponent
    ]
})
export class UserMenuModule
{
}
