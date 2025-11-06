import { MatSidenavModule } from '@angular/material/sidenav';
import { FuseMasonryModule } from '@fuse/components/masonry';
import { NzProgressModule } from 'ng-zorro-antd/progress';
import { NgModule } from "@angular/core";
import { MatButtonToggleModule } from '@angular/material/button-toggle';
import { RouterModule } from "@angular/router";
import { NgxChartsModule } from "@swimlane/ngx-charts";
import { MatBadgeModule } from "@angular/material/badge";
import { DataTablesModule } from "angular-datatables";
import { MatButtonModule } from "@angular/material/button";
import { MatChipsModule } from "@angular/material/chips";
import { MatRippleModule, MAT_DATE_FORMATS } from "@angular/material/core";
import { MatExpansionModule } from "@angular/material/expansion";
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
import { SweetAlert2Module } from "@sweetalert2/ngx-sweetalert2";


import { BrowserModule } from "@angular/platform-browser";


import { NgApexchartsModule } from "ng-apexcharts";



import { DragDropModule } from '@angular/cdk/drag-drop';
import { NgxMatSelectSearchModule } from "ngx-mat-select-search";

import { MatCardModule } from "@angular/material/card";

import { MatCheckboxModule } from "@angular/material/checkbox";




import { MatProgressSpinnerModule } from "@angular/material/progress-spinner";
import { MatTreeModule } from "@angular/material/tree";
import { MatDatepickerModule } from "@angular/material/datepicker";
import { MatTooltipModule } from "@angular/material/tooltip";
import { MatListModule } from "@angular/material/list";
import { MatSlideToggleModule } from "@angular/material/slide-toggle";
import { NgxMatDatetimePickerModule, NgxMatTimepickerModule } from "@angular-material-components/datetime-picker";
import { MatAutocompleteModule } from '@angular/material/autocomplete';

import { EditorModule, TINYMCE_SCRIPT_SRC } from '@tinymce/tinymce-angular';
import { TranslocoCoreModule } from "app/core/transloco/transloco.module";
import { commonModule } from "@fuse/components/commonComponent/common.module";
import { common_pageModule } from "@fuse/components/commonComponent/common_page.module";
import { storageRoutes } from "./file_manger.routing";

import { CommonModule } from "@angular/common";
import { FormsModule, ReactiveFormsModule } from "@angular/forms";
import { FusePipesModule } from "@fuse/pipes/pipes.module";
import { NzEmptyModule } from "ng-zorro-antd/empty";
import { TranslocoModule } from "@ngneat/transloco";
import { NzIconModule } from "ng-zorro-antd/icon";
import { MatDividerModule } from "@angular/material/divider";
import { MatTabsModule } from "@angular/material/tabs";
import { NgCircleProgressModule } from 'ng-circle-progress';

import { AutonumericModule } from '@angularfy/autonumeric';

import { CalendarModule } from 'app/calendar/calendar.module';

import { MatProgressBarModule } from '@angular/material/progress-bar';

import { SharedModule } from 'app/shared/shared.module';

import { NzPopoverModule } from 'ng-zorro-antd/popover';

import { NzCheckboxModule } from 'ng-zorro-antd/checkbox';

import { AgmCoreModule } from '@agm/core';



import { FuseDateRangeModule } from '@fuse/components/date-range';
import { FullCalendarModule } from '@fullcalendar/angular';
import { DATE_FORMATS } from 'app/Basecomponent/config';
import { NgxCaptureModule } from 'ngx-capture';
import { LottieModule } from 'ngx-lottie';
import player from 'lottie-web';
import { manager_folder_indexComponent } from './manager_folder/index.component';
import { file_manager_popupAdd_folderComponent } from './manager_folder/popupAdd_folder.component';
import { file_manager_popupAdd_fileComponent } from './manager_folder/popupAdd_file.component';
import { DetailFileFolderComponent } from './detail_file/index.component';
import { DetailFolderComponent } from './detail_folder/index.component';
import { popupShareComponent } from './manager_folder/popupShare.component';
import { MatRadioButton, MatRadioModule } from '@angular/material/radio';
import { manager_folder_indexListComponent } from './template_list/indexList.component';
import { manager_folder_indexGridComponent } from './template_list/indexGrid.component';
import { popup_view_file_onlineComponent } from './manager_folder/popup_view_file_online.component';

export function playerFactory() {
    return player;
}
@NgModule({
    providers: [
        { provide: TINYMCE_SCRIPT_SRC, useValue: 'tinymce/tinymce.min.js' },
    ],
    declarations: [
        popup_view_file_onlineComponent,
        manager_folder_indexListComponent,
        manager_folder_indexGridComponent,
        popupShareComponent,
        manager_folder_indexComponent,
        file_manager_popupAdd_folderComponent,
        file_manager_popupAdd_fileComponent,
        DetailFileFolderComponent,
        DetailFolderComponent,
    ],
    imports: [
        RouterModule.forChild(storageRoutes),
        LottieModule.forRoot({ player: playerFactory }),
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
        EditorModule,
        CalendarModule,
        DragDropModule,
        MatRadioModule,
    ],
    exports: [],
})
export class FileManagerModule { }
