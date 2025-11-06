import { Component, ViewEncapsulation, OnInit, ViewChild } from '@angular/core';
import { TranslocoService } from '@ngneat/transloco';
import { HttpClient, HttpResponse } from '@angular/common/http';
import { MatDialog } from '@angular/material/dialog';
import { Router, ActivatedRoute } from '@angular/router';
import { MatButtonToggleChange } from '@angular/material/button-toggle';
import { takeUntil } from 'rxjs/operators';
import { FuseCardComponent } from '@fuse/components/card';
import { AuthService } from '../../../core/auth/auth.service';
import { FuseMediaWatcherService } from '../../../../@fuse/services/media-watcher';
import { debug } from 'console';
import Swal from 'sweetalert2';
import { Subject } from 'rxjs';
import { FuseAlertType } from '../../../../@fuse/components/alert';
import SwiperCore, { SwiperOptions, EffectCoverflow, Pagination, Navigation, Autoplay } from "swiper";
import { SwiperComponent } from 'swiper/angular';
import { Title } from '@angular/platform-browser';
import { SeoService } from '@fuse/services/seo.service';
import { FuseConfigService } from '@fuse/services/config';
import { AppConfig } from 'app/core/config/app.config';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import * as AOS from 'aos';
SwiperCore.use([EffectCoverflow, Pagination, Navigation, Autoplay]);
@Component({
    selector: 'portal-contact-us',
    templateUrl: './contact_us.component.html',
    encapsulation: ViewEncapsulation.None
})

export class PortalContactUsComponent implements OnInit {
    @ViewChild('swiper', { static: false }) swiper?: SwiperComponent;
    slideNext() {
        this.swiper.swiperRef.slideNext(100);
    }
    slidePrev() {
        this.swiper.swiperRef.slidePrev(100);
    }
    /**
     * Constructor
     */
    alert: { type: FuseAlertType; message: string } = {
        type: 'success',
        message: ''
    };
    thong_tin: any;
    public loading: any = false;
    showAlert: boolean = false;
    list_khoa: any;
    public id: any;
    public truong_hoc: any;
    public activeLang: any;
    actionEnum: any = 1;
    public slides_web: any = [];
    public isScreenSmall: any = false;
    public slides_mobile: any = [];
    private _unsubscribeAll: Subject<any> = new Subject<any>();
    config: AppConfig;

    record: any = {
        db: {
            name: "",
            content: "",
            email: "",
            phone: "",
            id_khoa: "",
        }
    }
    get currentYear(): number {
        return new Date().getFullYear();
    }
    srcCaptcha: any;
    reloadCaptcha(): void {
        var d = new Date();
        var n = d.getTime();
        this.http
        .post('/sys_contactus.ctr/GetCaptchaImage/', {}
        ).subscribe(resp => {
            this.list_khoa = resp;
        });
        this.srcCaptcha = '/sys_contactus/GetCaptchaImage?' + n;

    }
    public errorModel: any;
    /**
     * Constructor
     */
    constructor(
        private titleService: Title,
        private _fuseConfigService: FuseConfigService,
        private formBuilder: FormBuilder,
        private seoService: SeoService,
        private router: Router, private route: ActivatedRoute,
        private _authService: AuthService,
        public http: HttpClient, dialog: MatDialog,
        private _fuseMediaWatcherService: FuseMediaWatcherService,
        public _translocoService: TranslocoService,) {
        this.errorModel = [];
        var d = new Date();
        var n = d.getTime();
        this.srcCaptcha = '/Captcha/GetCaptchaImage?' + n;
        this.load_thong_tin_truong();
        this.SendUp();
    }
    gobackhomepage() {
        const url = '/homepage-index';

        if (this.config.layout != 'empty') {

            this.router.navigateByUrl(url);
        } else {
            window.open(url, "_blank");
        }
    }
    setDocTitle(title: string) {
        this.titleService.setTitle(title);
    }
    load_banner(): void {

        this.loading = true;
        this.http
            .post('/sys_banner.ctr/get_list_banner/', {
                loai: "2"
            }
            ).subscribe(resp => {
                var data;
                data = resp;


                for (var i = 0; i < data.length; i++) {
                    if (data[i].image_web != null) {
                        this.slides_web.push({
                            image: data[i].image_web
                        })
                    }
                    if (data[i].image_mobile != null) {
                        this.slides_mobile.push({
                            image: data[i].image_mobile
                        })
                    }


                }
                this.loading = false;
            })
    }
    loadThongTin(): void {
        this.http
            .post('/sys_cau_hinh_thong_tin_website.ctr/getCauHinhThongTin/', {
            }
            ).subscribe(resp => {
                this.thong_tin = resp;
            });
    }
    ngOnInit(): void {

        AOS.init({
            duration: 1000
        });
        this._fuseConfigService.config$
            .pipe(takeUntil(this._unsubscribeAll))
            .subscribe((config: AppConfig) => {
                // Store the config
                this.config = config;
            });
        this._fuseMediaWatcherService.onMediaChange$
            .pipe(takeUntil(this._unsubscribeAll))
            .subscribe(({ matchingAliases }) => {
                // Check if the screen is small
                this.isScreenSmall = !matchingAliases.includes('md');
            });

        this._translocoService.langChanges$.subscribe((activeLang) => {
            //en
            this.activeLang = activeLang;
        });

        this.load_banner();
        //this.loadThongTin();
        var title = 'Quản Lý Kho Hàng -' + this._translocoService.translate('portal.contactus');
        var metaTag = [

            { property: 'og:url', content: 'https://school.vnaisoft.com/portal-contact-us' },
            { property: 'og:title', content: 'Quản Lý Kho Hàng' },
            { property: 'og:image', content: 'assets/images/logo/logo.png' },


            { property: 'og:description', content: 'Trang Liên hệ' },

        ]

        this.seoService.updateTitle(title);
        this.seoService.updateMetaTags(metaTag);
    };
    load_thong_tin_truong(): void {

        this.http
            .post('/sys_truong_hoc.ctr/get_thong_tin_truong', {}
            ).subscribe(resp => {
                this.truong_hoc = resp;

            });
    }
    gotoCompanyDetailPage(id_company): void {

        const url = '/portal-company-detail/' + id_company;
        if (this.config.layout != 'empty') {

            this.router.navigateByUrl(url);
        } else {
            window.open(url, "_blank");
        }
        //this.router.navigate(["/portal-news-detail'", { id: id_news_type } ]);

    }

    SendUp(): void {
        this.http.post('sys_contactus.ctr/create/',
            {
                data: this.record
            }
        ).subscribe(
            (resp) => {
                Swal.fire('Đã gửi thành công, chúng tôi sẽ phản hồi lại sau', '', 'success').then(
                    // Navigate to the confirmation required page
                    res => {
                        //this.router.navigateByUrl('/confirmation-required');
                    }

                );
            },
            (error) => {
                if (error.status == 400) {
                    this.errorModel = error.error;

                }
                // Set the alert
                this.alert = {
                    type: 'error',
                    message: 'Không hợp lệ, Vui lòng kiểm tra lại'
                };

                // Show the alert
            }
        );
    }
}
