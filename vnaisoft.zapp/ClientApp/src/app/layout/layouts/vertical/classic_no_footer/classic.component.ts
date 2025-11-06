import { Component, OnDestroy, OnInit, ViewEncapsulation } from '@angular/core';
import { ActivatedRoute, Data, Event, NavigationCancel, NavigationEnd, NavigationError, NavigationStart, Router } from '@angular/router';
import { Subject } from 'rxjs';
import { FuseMediaWatcherService } from '@fuse/services/media-watcher';
import { FuseNavigationService, FuseVerticalNavigationComponent } from '@fuse/components/navigation';
import { InitialData } from 'app/app.types';
import { filter, map, startWith, distinctUntilChanged, takeUntil } from 'rxjs/operators';


@Component({
    selector: 'classic-nofooter-layout',
    templateUrl: './classic.component.html',
    encapsulation: ViewEncapsulation.None
})
export class ClassicLayoutNoFooterComponent implements OnInit, OnDestroy {
    data: InitialData;
    isScreenSmall: boolean;
    public barNotNeeded = false;

    private _unsubscribeAll: Subject<any> = new Subject<any>();
    loading = false;
    public name: any;
    public currentUser: any = JSON.parse(localStorage.getItem('user'));
    /**
     * Constructor
     */
    constructor(
        private _activatedRoute: ActivatedRoute,
        private _router: Router,
        private _fuseMediaWatcherService: FuseMediaWatcherService,
        private _fuseNavigationService: FuseNavigationService
    ) { }

    // -----------------------------------------------------------------------------------------------------
    // @ Accessors
    // -----------------------------------------------------------------------------------------------------

    /**
     * Getter for current year
     */
    get currentYear(): number {
        return new Date().getFullYear();
    }

    _barNotNeeded(rawUrl: string): boolean {
        this._activatedRoute.data.subscribe((data: Data) => {
            this.data = data.initialData;
        });
        console.log("data no footer:", this.data.navigation);
        if (this.data.navigation.default.length <= 2) return true;
         else return false;
    }

    // -----------------------------------------------------------------------------------------------------
    // @ Lifecycle hooks
    // -----------------------------------------------------------------------------------------------------

    /**
     * On init
     */
    ngOnInit(): void {

        this._router.events
            .pipe(filter((e): e is NavigationEnd => e instanceof NavigationEnd),
                map(e => e.urlAfterRedirects),
                startWith(this._router.url),
                distinctUntilChanged(),
                takeUntil(this._unsubscribeAll))
            .subscribe(url => {
                this.barNotNeeded = this._barNotNeeded(url);
                // debugger;
            });

        // Subscribe to the resolved route data
        this._activatedRoute.data.subscribe((data: Data) => {
            this.data = data.initialData;
            this.name = data.initialData.navigation.name_module;
        });

        // Subscribe to media changes
        this._fuseMediaWatcherService.onMediaChange$
            .pipe(takeUntil(this._unsubscribeAll))
            .subscribe(({ matchingAliases }) => {

                // Check if the screen is small
                this.isScreenSmall = !matchingAliases.includes('md');
            });
    }

    /**
     * On destroy
     */
    ngOnDestroy(): void {
        // Unsubscribe from all subscriptions
        this._unsubscribeAll.next();
        this._unsubscribeAll.complete();
    }

    // -----------------------------------------------------------------------------------------------------
    // @ Public methods
    // -----------------------------------------------------------------------------------------------------

    /**
     * Toggle navigation
     *
     * @param name
     */
    toggleNavigation(name: string): void {
        // Get the navigation
        const navigation = this._fuseNavigationService.getComponent<FuseVerticalNavigationComponent>(name);

        if (navigation) {
            // Toggle the opened status
            navigation.toggle();
        }
    }
}
