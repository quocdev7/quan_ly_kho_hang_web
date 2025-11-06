import { ChangeDetectionStrategy, ChangeDetectorRef, Component, HostListener, OnDestroy, OnInit, ViewChild, ViewEncapsulation } from '@angular/core';
import { MatDrawer } from '@angular/material/sidenav';
import { Subject, Observable } from 'rxjs';
import { FuseNavigationItem } from '@fuse/components/navigation';
import { FuseMediaWatcherService } from '@fuse/services/media-watcher';
import { takeUntil } from 'rxjs/operators';
import { AvailableLangs, TranslocoService } from '@ngneat/transloco';
import * as AOS from 'aos';
import { HttpClient } from '@angular/common/http';
import { Title } from '@angular/platform-browser';

import { SeoService } from '@fuse/services/seo.service';
@Component({
    selector: 'about_us_index',
    templateUrl: './index.component.html',
    styleUrls: ['./index.component.scss'],
    encapsulation: ViewEncapsulation.None,
    changeDetection: ChangeDetectionStrategy.OnPush
})
export class about_us_indexComponentsComponent implements OnInit, OnDestroy {

    @ViewChild('drawer') drawer: MatDrawer;
    drawerMode: 'over' | 'side' = 'side';
    drawerOpened: boolean = true;

    public activeLang: any;
    selectedPanel: string = '1';
    private _unsubscribeAll: Subject<any> = new Subject<any>();
    public windowScrolled: any = false;
    public isScreenSmall: any = false;
    public currentUser: any = null;
    public panels_temp: any = [];
    public panels: any = [];
    public panel_to_chucs: any = [];
    public panel_gioi_thieus: any = [];
    public loading: any = false;
    public record: any = {};
    /**
     * Constructor
     */
    constructor(private _translocoService: TranslocoService,
        private titleService: Title,
        private seoService: SeoService,
        private _changeDetectorRef: ChangeDetectorRef,
        public http: HttpClient,
        private _fuseMediaWatcherService: FuseMediaWatcherService
    ) {

    }

    // -----------------------------------------------------------------------------------------------------
    // @ Lifecycle hooks
    // -----------------------------------------------------------------------------------------------------


    ngOnDestroy(): void {
        // Unsubscribe from all subscriptions
        this._unsubscribeAll.next(null);
        this._unsubscribeAll.complete();
    }
    setDocTitle(title: string) {
        this.titleService.setTitle(title);
    }
    load_panel() {
        this.loading = true;
        // this.panels_temp = [
        //     {
        //         id         : '1',
        //         type       : 1,
        //         //icon       : 'account_circle',
        //         title      : this._translocoService.translate("portal.hanh_trinh_xelex"),
        //         description: ''
        //     },
        //     {
        //         id         : '2',
        //         type       : 1,
        //        // icon       : 'edit',
        //         title      : this._translocoService.translate("portal.gia_tri_cot_loi"),
        //         description: ''
        //     },
        //     {
        //         id         : '3',
        //         type       : 1,
        //      //   icon       : 'calendar_today',
        //         title      : this._translocoService.translate("portal.tam_nhin_su_menh"),
        //         description: ''
        //     },
        //     {
        //         id         : '4',
        //         type       : 4,
        //        // icon       : 'calendar_today',
        //         title      : this._translocoService.translate("portal.chung_nhan_nang_luc"),
        //         description: ''
        //     },
        // ];
        this.http
            .post('/sys_cau_hinh_thong_tin.ctr/getListUseNew/', {}
            ).subscribe(resp => {

                let result: any = resp;
                var gioi_thieus: any = [];
                for (var i = 0; i < result.length; i++) {
                    this.panels_temp.push({
                        id: result[i].id,
                        type: 1,
                        // icon       : 'calendar_today',
                        title: result[i].name,
                        title_en: result[i].name_en,
                        noi_dung: result[i].noi_dung,
                        noi_dung_mobile: result[i].noi_dung,
                        noi_dung_en: result[i].noi_dung_en,
                        noi_dung_mobile_en: result[i].noi_dung_en,
                        description: ''

                    })

                    gioi_thieus = this.panels_temp;




                }
                this.panel_gioi_thieus = gioi_thieus;
                this.selectedPanel = this.panel_gioi_thieus[0].id;
                this.panels = this.panel_gioi_thieus
                // this.http
                // .post('/sys_cau_hinh_thong_tin.ctr/get_list_to_chuc/', {}
                // ).subscribe(resp => {

                //     let result: any= resp;
                //     var to_chucs :any;
                //     var panels_temp_to_chucs:any =[];
                //     for(var i=0;i<result.length;i++){
                //         panels_temp_to_chucs.push({
                //             id         : result[i].id,
                //             type       : 2,
                //            // icon       : 'calendar_today',
                //             title      : result[i].name,
                //             title_en      : result[i].name_en,
                //             noi_dung      : result[i].noi_dung,
                //             noi_dung_mobile      : result[i].noi_dung_mobile,
                //             noi_dung_en      : result[i].noi_dung_en,
                //             noi_dung_mobile_en      : result[i].noi_dung_mobile_en,
                //             description: ''

                //         })

                //         to_chucs=panels_temp_to_chucs;

                //     }
                //     this.panel_to_chucs = to_chucs;
                //     this.panels = this.panel_gioi_thieus.concat(this.panel_to_chucs);
                //     console.log(this.panels)

                // });

            });
        this.loading = false;
    }
    get_cong_ty() {

        this.http
            .post('/sys_truong_hoc.ctr/get_cong_ty/', {})
            .subscribe((resp) => {
                var data = resp as any;
                this.record = data;
            });
    }
    ngOnInit(): void {
        this.get_cong_ty();
        AOS.init({
            duration: 1000
        });
        //this.setDocTitle('Chăn nuôi Việt Nam -' + this._translocoService.translate('portal.gioithieu'))



        var title = 'Quản Lý Kho Hàng - ' + this._translocoService.translate('system.gioi_thieu');
        var metaTag = [
            { name: 'twitter:card', content: 'summary' },
            { property: 'og:type', content: 'article' },
            { property: 'og:url', content: 'https://school.vnaisoft.com/about_us_index' },
            { property: 'og:title', content: 'Quản Lý Kho Hàng' },
            { property: 'og:image', content: 'assets/images/logo/logo.png' },
            { property: 'og:description', content: 'Trang Giới thiệu' },


        ]

        this.seoService.updateTitle(title);
        this.seoService.updateMetaTags(metaTag);

        this.load_panel();
        this._translocoService.langChanges$.subscribe((activeLang) => {
            //en
            this.activeLang = activeLang;
        });
        this._fuseMediaWatcherService.onMediaChange$
            .pipe(takeUntil(this._unsubscribeAll))
            .subscribe(({ matchingAliases }) => {
                this.isScreenSmall = !matchingAliases.includes('md');
                if (matchingAliases.includes('md')) {
                    this.drawerMode = 'side';
                    this.drawerOpened = true;
                }
                else {
                    this.drawerMode = 'over';
                    this.drawerOpened = false;
                }

                // Mark for check
                this._changeDetectorRef.markForCheck();

            });

        // this.panels_temp = [
        //     {
        //         id         : '1',
        //         type       : 1,
        //         //icon       : 'account_circle',
        //         title      : this._translocoService.translate("portal.tong_quan"),
        //         description: ''
        //     },
        //     {
        //         id         : '2',
        //         type       : 1,
        //        // icon       : 'edit',
        //         title      : this._translocoService.translate("portal.ton_chi_va_muc_tieu"),
        //         description: ''
        //     },
        //     {
        //         id         : '3',
        //         type       : 1,
        //      //   icon       : 'calendar_today',
        //         title      : this._translocoService.translate("portal.quy_che_hoat_dong"),
        //         description: ''
        //     },
        //     {
        //         id         : '8',
        //         type       : 3,
        //        // icon       : 'calendar_today',
        //         title      : this._translocoService.translate("portal.cot_moc_hoat_dong"),
        //         description: ''
        //     },
        // ];
        // this.http
        // .post('/sys_nhom_hoi_dong.ctr/getListUse/', {}
        // ).subscribe(resp => {
        //     let result: any= resp;
        //     for(var i=0;i<result.length;i++){
        //         this.panels_temp.push({
        //             id         : result[i].id,
        //             type       : 2,
        //            // icon       : 'calendar_today',
        //             title      : result[i].name,
        //             noi_dung      : result[i].noi_dung,
        //             noi_dung_mobile      : result[i].noi_dung_mobile,
        //             description: ''
        //         })
        //         this.panels=this.panels_temp;
        //     }

        // });

    }
    // -----------------------------------------------------------------------------------------------------
    // @ Public methods
    // -----------------------------------------------------------------------------------------------------
    scrollToTop(): void {
        (function smoothscroll() {
            var currentScroll = document.documentElement.scrollTop || document.body.scrollTop;
            if (currentScroll > 0) {
                window.requestAnimationFrame(smoothscroll);
                window.scrollTo(0, currentScroll - (currentScroll / 8));
            }
        })();
    }

    /**
     * Navigate to the panel
     *
     * @param panel
     */
    goToPanel(panel: string): void {
        this.selectedPanel = panel;

        // Close the drawer on 'over' mode
        if (this.drawerMode === 'over') {
            this.drawer.close();
        }
    }
    @HostListener("window:scroll", [])
    onWindowScroll(): void {
        if ((window.pageYOffset || document.documentElement.scrollTop || document.body.scrollTop) >= 50) {

            this.windowScrolled = true;
        }
        else {
            this.windowScrolled = false;

        }



    }
    /**
     * Get the details of the panel
     *
     * @param id
     */
    getPanelInfo(id: string): any {
        return this.panels.find(panel => panel.id === id);
    }

    /**
     * Track by function for ngFor loops
     *
     * @param index
     * @param item
     */
    trackByFn(index: number, item: any): any {
        return item.id || index;
    }

}
