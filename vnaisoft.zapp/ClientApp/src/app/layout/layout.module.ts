import { NgModule } from '@angular/core';
import { MatIconModule } from '@angular/material/icon';
import { MatTooltipModule } from '@angular/material/tooltip';
import { FuseDrawerModule } from '@fuse/components/drawer';
import { LayoutComponent } from 'app/layout/layout.component';
import { EmptyLayoutModule } from 'app/layout/layouts/empty/empty.module';
import { EmptyWebLayoutModule } from 'app/layout/layouts/empty_web/empty_web.module';
import { CenteredLayoutModule } from 'app/layout/layouts/horizontal/centered/centered.module';
import { EnterpriseLayoutModule } from 'app/layout/layouts/horizontal/enterprise/enterprise.module';
import { MaterialLayoutModule } from 'app/layout/layouts/horizontal/material/material.module';
import { ModernLayoutModule } from 'app/layout/layouts/horizontal/modern/modern.module';
import { ClassicLayoutModule } from 'app/layout/layouts/vertical/classic/classic.module';
import { ClassyLayoutModule } from 'app/layout/layouts/vertical/classy/classy.module';
import { CompactLayoutModule } from 'app/layout/layouts/vertical/compact/compact.module';
import { DenseLayoutModule } from 'app/layout/layouts/vertical/dense/dense.module';
import { FuturisticLayoutModule } from 'app/layout/layouts/vertical/futuristic/futuristic.module';
import { ThinLayoutModule } from 'app/layout/layouts/vertical/thin/thin.module';
import { SharedModule } from 'app/shared/shared.module';
import { EmptyWebNonloginLayoutModule } from './layouts/emptyNonlogin/emptyNonlogin.module';
import { EmptyWebLayoutComponent } from './layouts/empty_web/empty_web.component';
import { portalLayoutModule } from './layouts/horizontal/portal/portal.module';
import { portalemptyLayoutModule } from './layouts/horizontal/portalempty/portalempty.module';
import { EmptyPortalLayoutModule } from './layouts/emptyPortal/emptyPortal.module';
import { TopBarWebNonloginLayoutModule } from './layouts/topbarNonlogin/topbarNonlogin.module';
import { ClassicLayoutNoFooterModule } from './layouts/vertical/classic_no_footer/classic.module';

const layoutModules = [
    ClassicLayoutNoFooterModule,
    TopBarWebNonloginLayoutModule,

    EmptyWebLayoutModule,
    EmptyPortalLayoutModule,
    // Empty
    EmptyWebNonloginLayoutModule,
    EmptyLayoutModule,
    // Horizontal navigation
    CenteredLayoutModule,
    EnterpriseLayoutModule,
    portalemptyLayoutModule,
    MaterialLayoutModule,
    ModernLayoutModule,
    portalLayoutModule,
    // Vertical navigation
    ClassicLayoutModule,
    ClassyLayoutModule,
    CompactLayoutModule,
    DenseLayoutModule,
    FuturisticLayoutModule,
    ThinLayoutModule
];

@NgModule({
    declarations: [
        LayoutComponent
    ],
    imports: [
        MatIconModule,
        MatTooltipModule,
        FuseDrawerModule,
        SharedModule,
        ...layoutModules
    ],
    exports: [
        LayoutComponent,
        ...layoutModules
    ]
})
export class LayoutModule {
}
