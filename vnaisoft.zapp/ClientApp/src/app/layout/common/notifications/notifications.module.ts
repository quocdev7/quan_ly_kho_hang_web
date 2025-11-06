import { NgModule } from '@angular/core';
import { RouterModule } from '@angular/router';
import { OverlayModule } from '@angular/cdk/overlay';
import { PortalModule } from '@angular/cdk/portal';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatTooltipModule } from '@angular/material/tooltip';
import { NotificationsComponent } from 'app/layout/common/notifications/notifications.component';
import { SharedModule } from 'app/shared/shared.module';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { MatInputModule } from '@angular/material/input';
import { MatMenuModule } from '@angular/material/menu';
import { FullCalendarModule } from '@fullcalendar/angular';
import { FuseDateRangeModule } from '@fuse/components/date-range';
import { SweetAlert2Module } from '@sweetalert2/ngx-sweetalert2';
import { MatChipsModule } from '@angular/material/chips';
import { MatExpansionModule } from '@angular/material/expansion';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatPaginatorModule } from '@angular/material/paginator';
import { MatTreeModule } from '@angular/material/tree';
import { MatRippleModule } from '@angular/material/core';
import { MatSelectModule } from '@angular/material/select';
import { MatBadgeModule } from '@angular/material/badge';
import { MatCheckboxModule } from '@angular/material/checkbox';
import { MatSortModule } from '@angular/material/sort';
import { MatSnackBarModule } from '@angular/material/snack-bar';
import { MatTabsModule } from '@angular/material/tabs';
import { MatDialogModule } from '@angular/material/dialog';
import { DataTablesModule } from 'angular-datatables';
import { MatCardModule } from '@angular/material/card';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { NgxChartsModule } from '@swimlane/ngx-charts';
import { NgxMatSelectSearchModule } from 'ngx-mat-select-search';
import { MatAutocompleteModule } from '@angular/material/autocomplete';
import { commonModule } from '@fuse/components/commonComponent/common.module';
import { common_pageModule } from '@fuse/components/commonComponent/common_page.module';
import { EditorModule } from '@tinymce/tinymce-angular';
import { MatListModule } from '@angular/material/list';
import { MatSlideToggleModule } from '@angular/material/slide-toggle';
import { TranslocoCoreModule } from 'app/core/transloco/transloco.module';
import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { FusePipesModule } from '@fuse/pipes/pipes.module';
import { NzEmptyModule } from 'ng-zorro-antd/empty';
import { MatButtonToggleModule } from '@angular/material/button-toggle';
import { TranslocoModule } from '@ngneat/transloco';
import { NzIconModule } from 'ng-zorro-antd/icon';
import { MatProgressBarModule } from '@angular/material/progress-bar';
import { NgApexchartsModule } from 'ng-apexcharts';
import { NgxCaptureModule } from 'ngx-capture';
import { NgCircleProgressModule } from 'ng-circle-progress';
import { AgmCoreModule } from '@agm/core';
import { NgxMatTimepickerModule } from '@angular-material-components/datetime-picker';
import { MatDividerModule } from '@angular/material/divider';
import { NzProgressModule } from 'ng-zorro-antd/progress';
import { MatSidenavModule } from '@angular/material/sidenav';
import { FuseMasonryModule } from '@fuse/components/masonry';
import { CalendarModule } from 'app/calendar/calendar.module';
import { DragDropModule } from '@angular/cdk/drag-drop';

@NgModule({
    declarations: [
        NotificationsComponent
    ],
    imports     : [
        RouterModule,
        OverlayModule,
        PortalModule,
        MatButtonModule,
        MatIconModule,
        MatTooltipModule,
        MatProgressSpinnerModule,
        MatInputModule,
        MatMenuModule,
        SharedModule,









        FullCalendarModule,
        FuseDateRangeModule,
        SweetAlert2Module,
        MatChipsModule,
        MatExpansionModule,
        MatFormFieldModule,
        MatPaginatorModule,
        MatTreeModule,
        MatRippleModule,
        MatSelectModule,
        MatBadgeModule,
        MatCheckboxModule,
        MatSortModule,
        MatSnackBarModule,
        MatTabsModule,
        MatDialogModule,
        DataTablesModule,
        MatCardModule,
        MatDatepickerModule,
        NgxChartsModule,
        NgxMatSelectSearchModule,
        MatAutocompleteModule,
        commonModule,
        common_pageModule,
        EditorModule,
        MatListModule,
        MatSlideToggleModule,
        TranslocoCoreModule,
        CommonModule,
        FormsModule,
        FusePipesModule,
        NzEmptyModule,
        MatButtonToggleModule,
        TranslocoModule,
        NzIconModule,
        MatProgressBarModule,
        NgApexchartsModule,
        NgxCaptureModule,
        NgCircleProgressModule.forRoot({
            // set defaults here
            radius: 100,
            outerStrokeWidth: 16,
            innerStrokeWidth: 8,
            outerStrokeColor: "#78C000",
            innerStrokeColor: "#C7E596",
            animationDuration: 300,

        }),
        AgmCoreModule.forRoot({
            apiKey: 'AIzaSyA1T28g5sfWOnkimOZBJutBACuO91CC4o0',
            libraries: ['places']
        }),
        ReactiveFormsModule,
        NgxMatTimepickerModule,
        MatDividerModule,
        NzProgressModule,
        MatSidenavModule,
        FuseMasonryModule,
        CalendarModule,
        DragDropModule
    ],
    exports     : [
        NotificationsComponent
    ]
})
export class NotificationsModule
{
}
