import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
    name: 'amPm'
})
export class AmPmPipe implements PipeTransform {

    transform(value: Date | string): string {
        let date = new Date(value);
        return date.getHours() >= 12 ? 'PM' : 'AM';
    }

}