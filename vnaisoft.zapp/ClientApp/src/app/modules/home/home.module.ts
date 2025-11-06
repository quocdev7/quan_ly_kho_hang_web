import { NgModule } from '@angular/core';
import { Route, RouterModule } from '@angular/router';
import { HomeComponent } from './home.component';
import { FullCalendarModule } from '@fullcalendar/angular';
import { FuseDateRangeModule } from '@fuse/components/date-range';
import { SweetAlert2Module } from '@sweetalert2/ngx-sweetalert2';
import { MatButtonModule } from '@angular/material/button';
import { MatChipsModule } from '@angular/material/chips';
import { MatExpansionModule } from '@angular/material/expansion';
import { MatSidenavModule } from '@angular/material/sidenav';
import { FuseMasonryModule } from '@fuse/components/masonry';
import { NzProgressModule } from 'ng-zorro-antd/progress';

import { MatButtonToggleModule } from '@angular/material/button-toggle';
import { NgxChartsModule } from "@swimlane/ngx-charts";
import { MatBadgeModule } from "@angular/material/badge";
import { DataTablesModule } from "angular-datatables";
import { MatRippleModule, MAT_DATE_FORMATS } from "@angular/material/core";
import { MatFormFieldModule } from "@angular/material/form-field";
import { MatIconModule } from "@angular/material/icon";
import { MatInputModule } from "@angular/material/input";
import { MatPaginatorModule } from "@angular/material/paginator";
import { MatSelectModule } from "@angular/material/select";
import { MatSnackBarModule } from "@angular/material/snack-bar";
import { MatSortModule } from "@angular/material/sort";
import { MatTableModule } from "@angular/material/table";
import { MatDialogModule } from "@angular/material/dialog";
import { MatMenuModule } from "@angular/material/menu";
import { NgApexchartsModule } from "ng-apexcharts";
import { DragDropModule } from '@angular/cdk/drag-drop';
import { NgxMatSelectSearchModule } from "ngx-mat-select-search";
import { MatCardModule } from "@angular/material/card";
import { MatCheckboxModule } from "@angular/material/checkbox";
import { MatTreeModule } from "@angular/material/tree";
import { MatDatepickerModule } from "@angular/material/datepicker";
import { MatTooltipModule } from "@angular/material/tooltip";
import { MatListModule } from "@angular/material/list";
import { MatSlideToggleModule } from "@angular/material/slide-toggle";
import { MatAutocompleteModule } from '@angular/material/autocomplete';
import { EditorModule, TINYMCE_SCRIPT_SRC } from '@tinymce/tinymce-angular';
import { TranslocoCoreModule } from "app/core/transloco/transloco.module";
import { commonModule } from "@fuse/components/commonComponent/common.module";
import { common_pageModule } from "@fuse/components/commonComponent/common_page.module";
import { CommonModule } from "@angular/common";
import { FormsModule, ReactiveFormsModule } from "@angular/forms";
import { FusePipesModule } from "@fuse/pipes/pipes.module";
import { NzEmptyModule } from "ng-zorro-antd/empty";
import { TranslocoModule } from "@ngneat/transloco";
import { NzIconModule } from "ng-zorro-antd/icon";
import { MatDividerModule } from "@angular/material/divider";
import { MatTabsModule } from "@angular/material/tabs";
import { NgCircleProgressModule } from 'ng-circle-progress';
import { CalendarModule } from 'app/calendar/calendar.module';

import { MatProgressBarModule } from '@angular/material/progress-bar';

import { SharedModule } from 'app/shared/shared.module';
import { AgmCoreModule } from '@agm/core';

import { LottieModule } from 'ngx-lottie';
import { MatRadioModule } from '@angular/material/radio';
import { ImageCropperModule } from 'ngx-image-cropper';
import { SwiperModule } from 'swiper/angular';
import player from 'lottie-web';// by the AOT compiler.
import { NgxMatTimepickerModule } from '@angular-material-components/datetime-picker';
export function playerFactory() {
    return player;
}
const homeRoutes: Route[] = [
    {
        path     : '',
        component: HomeComponent
    },
];

@NgModule({
    declarations: [
        HomeComponent,
    ],
    imports     : [
        RouterModule.forChild(homeRoutes),
        FullCalendarModule,
        FuseDateRangeModule,
        SweetAlert2Module,
        MatButtonModule,
        MatChipsModule,
        MatExpansionModule,
        MatFormFieldModule,
        MatIconModule,
        MatInputModule,
        MatMenuModule,
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
        MatTooltipModule,
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
        SharedModule,
        NgApexchartsModule,
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
        EditorModule,
        CalendarModule,
        DragDropModule,
        LottieModule.forRoot({ player: playerFactory }),
        MatRadioModule,
        MatIconModule,
        ImageCropperModule,
        SwiperModule
    ]
})
export class HomeModule
{
}
