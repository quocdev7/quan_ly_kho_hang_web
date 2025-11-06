import { Pipe, PipeTransform } from '@angular/core';

@Pipe({ name: 'strLimit'})
export class StrLimitPipe implements PipeTransform
{
    /**
     * Transform
     *
     * @param {string} value
     * @param {any[]} args
     * @returns {string}
     */
    transform(value: string, limit: number, args: any[] = []): string
    {

        if (!value) return;
        if (value.length <= limit) {
            return value;
        } else {
            value = value.substring(0, limit);
        }

        return value + '...';

        
    }
}
