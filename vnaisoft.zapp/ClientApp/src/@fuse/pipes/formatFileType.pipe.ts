import { Pipe, PipeTransform } from '@angular/core';

@Pipe({name: 'FileType'})
export class FileTypePipe implements PipeTransform
{
    /**
     * Transform
     *
     * @param value
     * @param {string[]} args
     * @returns {any}
     */
    transform(value: string, args: string[]): any
    {


        let FileType = value.split("/");
        switch(FileType[1]) {
            case 'vnd.openxmlformats-officedocument.spreadsheetml.sheet': {
                FileType[1]='xls';
               break;
            }
            case 'vnd.openxmlformats-officedocument.wordprocessingml.document': {
                FileType[1]='doc';
               break;
            }
            case 'msword': {
                FileType[1]='doc';
               break;
            }
            case 'png': {
                FileType[1]='png';
               break;
            }
            case 'jpeg': {
                FileType[1]='jpeg';
               break;
            }
            case 'pdf': {
                FileType[1]='pdf';
               break;
            }
            case 'vnd.ms-excel': {
                FileType[1]='xls';
               break;
            }
            default: {
                FileType[1]='none';
               break;
            }
         }

        let result = FileType[1].toLocaleUpperCase();
        return result;
    }
}
