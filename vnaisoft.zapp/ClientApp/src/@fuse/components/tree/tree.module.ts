import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FuseTreeComponent } from '@fuse/components/tree/tree.component';

@NgModule({
    declarations: [
        FuseTreeComponent
    ],
    imports     : [
        CommonModule
    ],
    exports     : [
        FuseTreeComponent
    ]
})
export class FuseTreeModule
{
}
