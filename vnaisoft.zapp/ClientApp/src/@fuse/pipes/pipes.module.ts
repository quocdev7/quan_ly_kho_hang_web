import { NgModule } from '@angular/core';
import { CamelCaseToDashPipe } from './camelCaseToDash.pipe';

import { filterequalPipe, FilterPipe } from './find-by-key/filter/filter.pipe';
import { FileTypePipe } from './formatFileType.pipe';
import { GetByIdPipe } from './getById.pipe';
import { HtmlToPlaintextPipe } from './htmlToPlaintext.pipe';
import { KeysPipe } from './keys.pipe';
import { safeHtml, SafePipe } from './safe.pipe';
import { StrLimitPipe } from './strLimit.pipe';
import { ThousandSuffixesPipe } from './ThousandSuffixes.pipe';
import { moneyPipe } from './money.pipe';
import { LinkifyPipe } from './linkify.pipe';
import { AmPmPipe } from './AmPmPipe/AmPmPipe.pipe';



@NgModule({
    declarations: [
        KeysPipe,
        GetByIdPipe,
        HtmlToPlaintextPipe,
        SafePipe,
        safeHtml,
        CamelCaseToDashPipe,
        FilterPipe,
        AmPmPipe,
        LinkifyPipe,
        filterequalPipe,
        ThousandSuffixesPipe,
        FileTypePipe,
        StrLimitPipe,
        moneyPipe,
        FilterPipe, 
    ],
    providers: [ThousandSuffixesPipe],
    imports: [],
    exports: [
        KeysPipe,
        GetByIdPipe,
        HtmlToPlaintextPipe,
        ThousandSuffixesPipe,
        SafePipe,
        safeHtml,
        CamelCaseToDashPipe,
        FilterPipe,
        AmPmPipe,
        filterequalPipe,
        FileTypePipe,
        StrLimitPipe,
        moneyPipe
    ]
})
export class FusePipesModule {
}
