import { NgModule } from '@angular/core';
import { FuseScrollResetDirective } from '@fuse/directives/scroll-reset/scroll-reset.directive';
import { DragDropDirective } from '../drag-drop';

@NgModule({
    declarations: [
        DragDropDirective
    ],
    exports     : [
        DragDropDirective
    ]
})
export class drag_dropModule
{
}
