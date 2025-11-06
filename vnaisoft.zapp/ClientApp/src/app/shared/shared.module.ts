import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { NzEmptyModule } from 'ng-zorro-antd/empty';
import { registerLocaleData } from '@angular/common';
import vi from '@angular/common/locales/en';
registerLocaleData(vi);
import { NZ_I18N, vi_VN } from 'ng-zorro-antd/i18n';
import { commonModule } from '@fuse/components/commonComponent/common.module';
import { common_pageModule } from '@fuse/components/commonComponent/common_page.module';
import { NzSpinModule } from 'ng-zorro-antd/spin';
import { NzSkeletonModule } from 'ng-zorro-antd/skeleton';
import { NzTagModule } from 'ng-zorro-antd/tag';
import { NzPopoverModule } from 'ng-zorro-antd/popover';
import { NzCheckboxModule } from 'ng-zorro-antd/checkbox';
import { FuseCardModule } from '@fuse/components/card';
import { NzProgressModule } from 'ng-zorro-antd/progress';
import { NzSliderModule } from 'ng-zorro-antd/slider';
import { NzLayoutModule } from 'ng-zorro-antd/layout';
import { NzGridModule } from 'ng-zorro-antd/grid';
import { NzSelectModule } from 'ng-zorro-antd/select';
import { NzInputModule } from 'ng-zorro-antd/input';
import { NzInputNumberModule } from 'ng-zorro-antd/input-number';
import { NzListModule } from 'ng-zorro-antd/list';
import { NzTableModule } from 'ng-zorro-antd/table';
import { TranslocoModule } from '@ngneat/transloco';
import { MatSidenavModule } from '@angular/material/sidenav';
import { FullCalendarModule } from '@fullcalendar/angular';
@NgModule({
    providers   : [
        { provide: NZ_I18N, useValue: vi_VN }
      ],
    imports: [
        CommonModule,
        FormsModule,
        ReactiveFormsModule,
        commonModule,
        common_pageModule,
        // NzSkeletonModule,
    ],
    exports: [
        CommonModule,
        FormsModule,
        ReactiveFormsModule,
        NzEmptyModule,
        commonModule,
        common_pageModule,
        NzSpinModule,
        NzSkeletonModule,
        NzListModule,
        NzTagModule,
        NzEmptyModule,
        NzPopoverModule,
        NzCheckboxModule,
        FuseCardModule,
        NzProgressModule,
        NzSliderModule,
        NzLayoutModule,
        NzGridModule,
        NzInputModule,
        NzInputNumberModule,
        NzSelectModule,
        NzTableModule,
        TranslocoModule,
        commonModule,
        MatSidenavModule,
        FullCalendarModule,
    ]
})
export class SharedModule
{
}
