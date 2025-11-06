import { ChangeDetectorRef, Component, OnDestroy, OnInit, ViewEncapsulation, HostListener } from '@angular/core';
import { ActivatedRoute, Data, Router } from '@angular/router';
import { Subject } from 'rxjs';
import { takeUntil } from 'rxjs/operators';
import { FuseMediaWatcherService } from '@fuse/services/media-watcher';
import { FuseNavigationItem, FuseNavigationService, FuseVerticalNavigationComponent } from '@fuse/components/navigation';
import { InitialData } from 'app/app.types';
import { AuthService } from '../../../../core/auth/auth.service';
import { HttpClient } from '@angular/common/http';
import { TranslocoService, AvailableLangs } from '@ngneat/transloco';
interface nhom_tin_tuc {
    id: string;
    has_tin_tuc: boolean;
    id_nhom: string | null;
    ten: string;
    ten_en: string | null;
    lst_nhom: nhom_tin_tuc[];
}
@Component({
    selector: 'portalempty-layout',
    templateUrl: './portalempty.component.html',
    styleUrls: ['./portalempty.component.scss'],
    encapsulation: ViewEncapsulation.None
})
export class portalemptyLayoutComponent implements OnInit, OnDestroy {
    private _compactNavigation: FuseNavigationItem[];
    private _defaultNavigation: FuseNavigationItem[];
    private _futuristicNavigation: FuseNavigationItem[];
    private _horizontalNavigation: FuseNavigationItem[];
    isScreenSmall: boolean;
    private _unsubscribeAll: Subject<any> = new Subject<any>();
    public user: any = {
        type: 0
    };
    public unreadCount: any;
    windowScrolledHeader: boolean;

    windowScrolled: boolean;
    public is_login: any = false;
    public truong_hoc: any;
    public activeLang: any;
    public thong_tin_website: any = [];
    public list_dieu_khoan_footer: any;
    public navigation: any = {};
    public is_show_hide: any = true;
    /**
     * Constructor
     */
    constructor(
        private _authService: AuthService,
        private changeDetectorRef: ChangeDetectorRef,
        private _activatedRoute: ActivatedRoute,
        private _router: Router,
        private http: HttpClient,
        private _changeDetectorRef: ChangeDetectorRef,
        private _fuseMediaWatcherService: FuseMediaWatcherService,
        private _fuseNavigationService: FuseNavigationService,
        private _translocoService: TranslocoService

    ) {
     
    }
    public notifications: any = [];
   
    openPanel(): void {
        const url = '/portal_empty_notification';
        this._router.navigateByUrl(url);
    }

    @HostListener("window:scroll", [])
    onWindowScroll(): void {
        if ((window.pageYOffset || document.documentElement.scrollTop || document.body.scrollTop) >= 50) {
            this.windowScrolledHeader = true;
            this.windowScrolled = true;
        }
        else {
            this.windowScrolled = false;
            this.windowScrolledHeader = false;
        }



    }
    scrollToTop(): void {
        (function smoothscroll() {
            var currentScroll = document.documentElement.scrollTop || document.body.scrollTop;
            if (currentScroll > 0) {
                window.requestAnimationFrame(smoothscroll);
                window.scrollTo(0, currentScroll - (currentScroll / 8));
            }
        })();
    }



   
    showHide(): void {
        this.is_show_hide = !this.is_show_hide;
    }
   

    // -----------------------------------------------------------------------------------------------------
    // @ Accessors
    // -----------------------------------------------------------------------------------------------------

    /**
     * Getter for current year
     */
    get currentYear(): number {
        return new Date().getFullYear();
    }
    loadUser(): void {
        this.http
            .post('/sys_user.ctr/getUserLogin/', {}
            ).subscribe(resp => {
              
                this.loadmenu();

            });
    }

    //collapsable
    buildNavigation = (lst_nhom: nhom_tin_tuc[]): FuseNavigationItem[] => {
        return lst_nhom.map(nhom => ({
            id: nhom.id,
            title: nhom.ten,
            type: nhom.lst_nhom && nhom.lst_nhom.length > 0 ? 'group' : 'basic',
            translate: '',//`NAV.${nhom.id}`, // Or a more descriptive key
            //icon: 'default_icon', // Set default icon or map dynamically
            link: '/ky-yeu-detail/' + nhom.id,
            children: nhom.lst_nhom && nhom.lst_nhom.length > 0 ? this.buildNavigation(nhom.lst_nhom) : [],
            // ... other properties as needed
        }));
    };

    loadmenu(): void {
        var trang_chu: FuseNavigationItem = {
            id: 'trangchu',
            module: 'system',
            title: 'portal.trang_chu',
            translate: 'portal.trang_chu',
            link: "/homepage-index",
            type: 'basic',
            // icon_img: "/assets/images/portal/Ve_chung_toi.png",
        };
        var gioi_thieu: FuseNavigationItem = {
            id: 'aboutus',
            module: 'system',
            title: 'portal.gioithieu',
            translate: 'portal.gioithieu',
            link: "/about_us_index",
            type: 'basic',
            // icon_img: "/assets/images/portal/Ve_chung_toi.png",
        };
        var ky_yeu: FuseNavigationItem = {
            id: 'kyyeu',
            module: 'system',
            title: 'NAV.sys_ky_yeu',
            translate: 'NAV.sys_ky_yeu',
            link: "/portal_ky_yeu_index",
            type: 'basic',
            // icon_img: "/assets/images/portal/Ve_chung_toi.png",
        };
        var contact_us: FuseNavigationItem = {
            id: 'faculty',
            module: 'system',
            title: 'portal.contactus',
            translate: 'portal.contactus',
            type: 'basic',
            //  icon_img: "/assets/images/portal/Lien_he.png",
            link: "portal-contact-us"

        }



        this._defaultNavigation = [];
        this._defaultNavigation.push(trang_chu);
     
       // this._defaultNavigation.push(gioi_thieu);
        //this._defaultNavigation.push(ky_yeu);
        this.http
            .post('/sys_nhom_ky_yeu.ctr/get_lst_ky_yeu', {}
            ).subscribe(resp_news => {
                var model: any;
                model = resp_news;
                //this.list_group_new = resp_news



                var navigation_new = [] as FuseNavigationItem[];
                navigation_new = this.buildNavigation(model);


                this._defaultNavigation = this._defaultNavigation.concat(navigation_new);



               // this._defaultNavigation.push(contact_us);

                this._compactNavigation = [...this._defaultNavigation];
                this._futuristicNavigation = [...this._defaultNavigation];
                this._horizontalNavigation = [...this._defaultNavigation];
                this.navigation =
                {
                    compact: this._compactNavigation,
                    default: this._defaultNavigation,
                    futuristic: this._futuristicNavigation,
                    horizontal: this._horizontalNavigation
                };




            });

        this.load_thong_tin_truong();
    }

    // -----------------------------------------------------------------------------------------------------
    // @ Lifecycle hooks
    // -----------------------------------------------------------------------------------------------------

    /**
     * On init
     */
    ngOnInit(): void {

 
        this._authService.getUser().subscribe((data: any) => {
            if (data != undefined) this.user = data;
        }
        );
        this._authService.check().subscribe((data: any) => {
            this.is_login = data
            if (this.is_login == true) {
                this.loadUser();
            } else {
                this.loadmenu();
            }



        });



        this._translocoService.langChanges$.subscribe((activeLang) => {
            //en
            this.activeLang = activeLang;
        });

        // Subscribe to media changes
        this._fuseMediaWatcherService.onMediaChange$
            .pipe(takeUntil(this._unsubscribeAll))
            .subscribe(({ matchingAliases }) => {

                // Check if the screen is small
                this.isScreenSmall = !matchingAliases.includes('md');
            });

        this.load_dieu_khoan_footer();

    }
    load_dieu_khoan_footer() {
        this.list_dieu_khoan_footer = [
            {
                id: "1",
                name: this._translocoService.translate('common.term'),
                link: "portal-term",
            },
            {
                id: "2",
                name: this._translocoService.translate('common.privacy'),
                link: "portal-privacy"
            },

        ]
    }
    load_thong_tin_truong(): void {

        this.http
            .post('/sys_truong_hoc.ctr/get_thong_tin_truong', {}
            ).subscribe(resp => {
                this.truong_hoc = resp;

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
