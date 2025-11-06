
import { Injectable } from '@angular/core';
import { cloneDeep } from 'lodash-es';
import { FuseNavigationItem } from '@fuse/components/navigation';
import { FuseMockApiService } from '@fuse/lib/mock-api';
import { compactNavigation, defaultNavigation, futuristicNavigation, horizontalNavigation } from 'app/mock-api/common/navigation/data';
import { HttpClient } from '@angular/common/http';
import { TranslocoService } from '@ngneat/transloco';
import { switchMap } from 'rxjs/operators';
import { of } from 'rxjs';


@Injectable({
    providedIn: 'root'
})
export class NavigationMockApi {
    public menu: any;
    private _compactNavigation: FuseNavigationItem[] = compactNavigation;
    private _defaultNavigation: FuseNavigationItem[] = defaultNavigation;
    private _futuristicNavigation: FuseNavigationItem[] = futuristicNavigation;
    private _horizontalNavigation: FuseNavigationItem[] = horizontalNavigation;

    /**
     * Constructor
     */
    constructor(private _fuseMockApiService: FuseMockApiService,
        private _httpClient: HttpClient,
        public _translocoService: TranslocoService,) {
        // Register Mock API handlers
        this.registerHandlers();
    }

    // -----------------------------------------------------------------------------------------------------
    // @ Public methods
    // -----------------------------------------------------------------------------------------------------

    /**
     * Register Mock API handlers
     */
    registerHandlers(): void {
        // -----------------------------------------------------------------------------------------------------
        // @ Navigation - GET
        // -----------------------------------------------------------------------------------------------------
        this._fuseMockApiService
            .onGet('api/common/navigation');
    }

}
