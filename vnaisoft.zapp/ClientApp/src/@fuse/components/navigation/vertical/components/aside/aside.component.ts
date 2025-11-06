import { ChangeDetectionStrategy, ChangeDetectorRef, Component, Input, OnChanges, OnDestroy, OnInit, SimpleChanges } from '@angular/core';
import { NavigationEnd, Router } from '@angular/router';
import { BooleanInput } from '@angular/cdk/coercion';
import { Subject } from 'rxjs';
import { filter, takeUntil } from 'rxjs/operators';
import { FuseVerticalNavigationComponent } from '@fuse/components/navigation/vertical/vertical.component';
import { FuseNavigationService } from '@fuse/components/navigation/navigation.service';
import { FuseNavigationItem } from '@fuse/components/navigation/navigation.types';
import { TranslocoService } from '@ngneat/transloco';

@Component({
    selector: 'fuse-vertical-navigation-aside-item',
    templateUrl: './aside.component.html',
    styles: [],
    changeDetection: ChangeDetectionStrategy.OnPush
})
export class FuseVerticalNavigationAsideItemComponent implements OnChanges, OnInit, OnDestroy {
    /* eslint-disable @typescript-eslint/naming-convention */
    static ngAcceptInputType_autoCollapse: BooleanInput;
    static ngAcceptInputType_skipChildren: BooleanInput;
    /* eslint-enable @typescript-eslint/naming-convention */

    @Input() activeItemId: string;
    @Input() autoCollapse: boolean;
    @Input() item: FuseNavigationItem;
    @Input() name: string;
    @Input() skipChildren: boolean;

    active: boolean = false;
    private _fuseVerticalNavigationComponent: FuseVerticalNavigationComponent;
    private _unsubscribeAll: Subject<any> = new Subject<any>();

    /**
     * Constructor
     */
    constructor(
        public _translocoService: TranslocoService,
        private _changeDetectorRef: ChangeDetectorRef,
        private _router: Router,
        private _fuseNavigationService: FuseNavigationService
    ) {

    }

    // -----------------------------------------------------------------------------------------------------
    // @ Lifecycle hooks
    // -----------------------------------------------------------------------------------------------------

    /**
     * On changes
     *
     * @param changes
     */
    ngOnChanges(changes: SimpleChanges): void {
        // Active item id
        if ('activeItemId' in changes) {
            // Mark if active
            this._markIfActive(this._router.url);
        }
    }
    removeAccents(str) {
        var AccentsMap = [
            "aàảãáạăằẳẵắặâầẩẫấậ",
            "AÀẢÃÁẠĂẰẲẴẮẶÂẦẨẪẤẬ",
            "dđ", "DĐ",
            "eèẻẽéẹêềểễếệ",
            "EÈẺẼÉẸÊỀỂỄẾỆ",
            "iìỉĩíị",
            "IÌỈĨÍỊ",
            "oòỏõóọôồổỗốộơờởỡớợ",
            "OÒỎÕÓỌÔỒỔỖỐỘƠỜỞỠỚỢ",
            "uùủũúụưừửữứự",
            "UÙỦŨÚỤƯỪỬỮỨỰ",
            "yỳỷỹýỵ",
            "YỲỶỸÝỴ"
        ];
        for (var i = 0; i < AccentsMap.length; i++) {
            var re = new RegExp('[' + AccentsMap[i].substr(1) + ']', 'g');
            var char = AccentsMap[i][0];
            str = str.replace(re, char);
        }
        return str;
    }

    public list_menu: FuseNavigationItem[];
    filterByQuery(query: string): void {
        var text = this.removeAccents(query.replace(" ", "").toLocaleLowerCase())
        this.list_menu = JSON.parse(localStorage.getItem("navigation")) as FuseNavigationItem[]
        if (query === "")
            this.item.children = this.list_menu
        else {
            var list: FuseNavigationItem[] = []
            this.item.children = this.list_menu
            for (let index = 0; index < this.item.children.length; index++) {
                const element = this.item.children[index];
                element.children.forEach(item => {
                    item.check = false;
                    var translate = this.removeAccents(this._translocoService.translate(item.translate).replace(" ", "").toLocaleLowerCase())
                    var title = this.removeAccents(this._translocoService.translate(item.title).replace(" ", "").toLocaleLowerCase())
                    var check1 = translate.includes(text)
                    var check2 = title.includes(text)
                    if (check1 == true || check2 == true) {
                        item.check = true
                    }
                });
                element.children = element.children.filter(q => q.check == true)
                if (element.children.length != 0) {
                    list.push(element)
                }
                else {
                    this.item.children = this.item.children.filter(q => q.id != element.id)
                }
            }
            this.item.children = list
        }
    }
    /**
     * On init
     */
    ngOnInit(): void {
        // Mark if active
        this._markIfActive(this._router.url);

        // Attach a listener to the NavigationEnd event
        this._router.events
            .pipe(
                filter((event): event is NavigationEnd => event instanceof NavigationEnd),
                takeUntil(this._unsubscribeAll)
            )
            .subscribe((event: NavigationEnd) => {

                // Mark if active
                this._markIfActive(event.urlAfterRedirects);
            });

        // Get the parent navigation component
        this._fuseVerticalNavigationComponent = this._fuseNavigationService.getComponent(this.name);

        // Subscribe to onRefreshed on the navigation component
        this._fuseVerticalNavigationComponent.onRefreshed.pipe(
            takeUntil(this._unsubscribeAll)
        ).subscribe(() => {

            // Mark for check
            this._changeDetectorRef.markForCheck();
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
     * Track by function for ngFor loops
     *
     * @param index
     * @param item
     */
    trackByFn(index: number, item: any): any {
        return item.id || index;
    }

    // -----------------------------------------------------------------------------------------------------
    // @ Private methods
    // -----------------------------------------------------------------------------------------------------

    /**
     * Check if the given item has the given url
     * in one of its children
     *
     * @param item
     * @param currentUrl
     * @private
     */
    private _hasActiveChild(item: FuseNavigationItem, currentUrl: string): boolean {
        const children = item.children;

        if (!children) {
            return false;
        }

        for (const child of children) {
            if (child.children) {
                if (this._hasActiveChild(child, currentUrl)) {
                    return true;
                }
            }

            // Skip items other than 'basic'
            if (child.type !== 'basic') {
                continue;
            }

            // Check if the child has a link and is active
            if (child.link && this._router.isActive(child.link, child.exactMatch || false)) {
                return true;
            }
        }

        return false;
    }

    /**
     * Decide and mark if the item is active
     *
     * @private
     */
    private _markIfActive(currentUrl: string): void {
        // Check if the activeItemId is equals to this item id
        this.active = this.activeItemId === this.item.id;

        // If the aside has a children that is active,
        // always mark it as active
        if (this._hasActiveChild(this.item, currentUrl)) {
            this.active = true;
        }

        // Mark for check
        this._changeDetectorRef.markForCheck();
    }
}
