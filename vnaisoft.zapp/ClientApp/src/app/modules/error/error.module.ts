import { NgModule } from '@angular/core';
import { Route, RouterModule } from '@angular/router';
import { Error404Component } from './error-404/error-404.component';
import { commonModule } from "@fuse/components/commonComponent/common.module";
import { CommonModule } from "@angular/common";
const portalRoutes: Route[] = [
    {
        path: '',
        component: Error404Component
    }
];

@NgModule({
    declarations: [
        Error404Component
    ],
    imports     : [
        RouterModule.forChild(portalRoutes),
        CommonModule,
        commonModule
    ]
})
export class Error404Module
{
}
