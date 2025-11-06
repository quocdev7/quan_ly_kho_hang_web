import { Component, Inject, ViewChild } from '@angular/core';
import { HttpClient, HttpParams, HttpResponse } from '@angular/common/http';
import { DataTablesResponse } from 'app/Basecomponent/datatable';
import { TranslocoService } from '@ngneat/transloco';
import { sys_don_hang_ban_popUpAddComponent } from './popupAdd.component';
import { MatDialog } from '@angular/material/dialog';
import { DataTableDirective } from 'angular-datatables';
import { Subject } from 'rxjs';
import Swal from 'sweetalert2';
import { BaseIndexDatatableComponent } from 'app/Basecomponent/BaseIndexDatatable.component';
import { FuseNavigationService } from '@fuse/components/navigation';
import { ActivatedRoute, Router } from '@angular/router';
import { SeoService } from '@fuse/services/seo.service';
import { FuseConfigService } from '@fuse/services/config';
import { takeUntil } from 'rxjs/operators';
import { AppConfig } from 'app/core/config/app.config';
import { cm_file_upload_popupComponent } from '@fuse/components/commonComponent/cm_file_upload/cm_file_upload_popup.component';
import { cm_mau_in_popupComponent } from '@fuse/components/commonComponent/cm_mau_in/cm_mau_in_popup.component';
//import { sys_don_hang_ban_popupHistoryEditComponent } from './popupHistoryEdit.component';

@Component({
    selector: 'sys_don_hang_ban_index',
    templateUrl: './index.component.html',
    styleUrls: ['./index.component.scss']
})

export class sys_don_hang_ban_indexComponent extends BaseIndexDatatableComponent {

    public file: any;
    public list_doi_tuong: any;
    public list_status_del: any;
    public list_hinh_thuc_van_chuyen: any
    public list_hinh_thuc: any
    public list_loai_giao_dich: any
    public list_tinh_trang_don_hang: any
    public cau_hinh_gia_ban: any
    config: AppConfig;
    private _unsubscribeAll: Subject<any> = new Subject<any>();
    constructor(
        private seoService: SeoService,
        private router: Router,
        private _fuseConfigService: FuseConfigService,
        http: HttpClient, dialog: MatDialog
        , _translocoService: TranslocoService
        , _fuseNavigationService: FuseNavigationService, route: ActivatedRoute
        , @Inject('BASE_URL') baseUrl: string
    ) {
        super(http, baseUrl, _translocoService, _fuseNavigationService, route, dialog, 'sys_don_hang_ban',
            { search: "", status_del: "1", id_cong_ty: "-1", tu_ngay: "", den_ngay: "", id_doi_tuong: -1, id_kieu_ban: -1, id_hinh_thuc_van_chuyen: -1, id_loai_giao_dich: -1, open: false, is_xuat_kho: 0, tinh_trang_don_hang: "-1", is_xuat_du: "-1", is_thu_du: "-1" }
        )
        this.list_status_del = [
            {
                id: "1",
                name: this._translocoService.translate('system.use')
            },
            {
                id: "2",
                name: this._translocoService.translate('system.huy')
            }
        ];
        this.get_list_hinh_thuc_van_chuyen();
        this.get_hinh_thuc_doi_tuong();
        this.get_loai_giao_dich();
        this.get_tinh_trang_don_hang();
        this.load_date();
        this.get_cau_hinh_he_thong_gia_ban();
    }
    update_status_del(model, status_del): void {
        Swal.fire({
            title: this._translocoService.translate('areYouSure'),
            text: "",
            icon: 'warning',
            showCancelButton: true,
            confirmButtonColor: '#3085d6',
            cancelButtonColor: '#d33',
            confirmButtonText: this._translocoService.translate('yes'),
            cancelButtonText: this._translocoService.translate('no')
        }).then((result) => {
            if (result.value) {
                if(model.db.id_hoa_don == null || model.db.id_hoa_don == "")
                {
                    this.http
                    .post(this.controller + '.ctr/update_status_del/',
                        {
                            id: model.db.id,
                            status_del: status_del

                        }
                    ).subscribe(resp => {
                        Swal.fire('Thành công', '', 'success');
                        this.rerender();
                    },
                        error => {
                            if (error.status == 403) {
                                Swal.fire(this._translocoService.translate('no_permission'), "", "warning");
                            }
                        });
                }
                else
                {
                    Swal.fire(this._translocoService.translate('huy_hoa_don_tuong_ung_truoc_khi_huy_don_hang'), "", "warning");
                }
            }
        })


    }
    load_date(): void {
        this.filter.tu_ngay = new Date();
        this.filter.tu_ngay.setDate(this.filter.tu_ngay.getDate() - 365);
        this.filter.den_ngay = new Date();
    }
    cap_nhap(model, tinh_trang_don_hang): void {
        if (tinh_trang_don_hang == 2) {
            Swal.fire({
                title: this._translocoService.translate('ban_co_muon_chuyen_thanh_hop_dong_hay_khong'),
                text: "",
                icon: 'warning',
                showCancelButton: true,
                confirmButtonColor: '#3085d6',
                cancelButtonColor: '#d33',
                confirmButtonText: this._translocoService.translate('yes'),
                cancelButtonText: this._translocoService.translate('no')
            }).then((result) => {
                if (result.value) {
                    this.http
                        .post(this.controller + '.ctr/cap_nhap/',
                            {
                                id: model.db.id,
                                tinh_trang_don_hang: tinh_trang_don_hang

                            }
                        ).subscribe(resp => {
                            Swal.fire('Cập nhập thành công', '', 'success');
                            this.rerender();
                        },
                        );
                }
            })
        }
        else if (tinh_trang_don_hang == 3) {
            Swal.fire({
                title: this._translocoService.translate('ban_co_muon_xac_nhan_hoan_tat_don_hang_hay_khong'),
                text: "",
                icon: 'warning',
                showCancelButton: true,
                confirmButtonColor: '#3085d6',
                cancelButtonColor: '#d33',
                confirmButtonText: this._translocoService.translate('yes'),
                cancelButtonText: this._translocoService.translate('no')
            }).then((result) => {
                if (result.value) {
                    this.http
                        .post(this.controller + '.ctr/cap_nhap/',
                            {
                                id: model.db.id,
                                tinh_trang_don_hang: tinh_trang_don_hang

                            }
                        ).subscribe(resp => {
                            Swal.fire('Cập nhập thành công', '', 'success');
                            this.rerender();
                        },
                        );
                }
            })
        }



    }

    onFileSelected(event: any) {
        this.file = event.target.files[0];
    }
    dowloadFileMau() {
        var url = '/sys_don_hang_ban.ctr/downloadtemp';
        window.location.href = url;
    }
    onSubmitFile(event: any) {
        if (this.file == null || this.file == undefined) {
            Swal.fire('Phải chọn file import', '', 'warning')
        } else {
            this.showLoading("", "", true)
            var formData = new FormData();
            formData.append('file', this.file);
            this.http.post('/sys_don_hang_ban.ctr/ImportFromExcel/', formData, {
            })
            .subscribe(resp => {

                Swal.close();
                var res
                res = resp
                if (res == "1") {
                    Swal.fire('Lưu thành công', '', 'success');
                    this.file = null;
                    this.rerender();
                }
                if (res == "-1") {
                    Swal.fire("File không đúng định dạng", '', 'warning');
                }
                if (res != "-1" && res != "1") {
                    Swal.fire({
                        title: this._translocoService.translate('system.khong_the_import_duoc_vui_long_tai_ve_xem_chi_tiet'),
                        text: "",
                        icon: 'warning',
                        showCancelButton: true,
                        confirmButtonColor: '#3085d6',
                        cancelButtonColor: '#d33',
                        confirmButtonText: this._translocoService.translate('system.tai_ve'),
                        cancelButtonText: this._translocoService.translate('system.close')
                    }).then((result) => {
                        if (result.value) {
                            var url = '/sys_home.ctr/downloadtempFileError?path=' + res;
                            window.location.href = url;
                        }
                    })
                }

            })
        }

    }


    get_tinh_trang_don_hang() {
        this.list_tinh_trang_don_hang = [
            {
                id: "1",
                name: this._translocoService.translate('erp.bao_gia')
            },
            {
                id: "2",
                name: this._translocoService.translate('erp.hop_dong')
            },
            {
                id: "3",
                name: this._translocoService.translate('erp.hoan_tat')
            }
        ];
    }
    
    public file_chi_tiet: any;
    onFileSelectedChiTiet(event: any) {
        this.file_chi_tiet = event.target.files[0];
        //event.target.value = null;
    }
    dowloadFileMauChiTiet() {
        var url = '/sys_don_hang_ban.ctr/downloadtempdetail';
        window.location.href = url;
    }
    onSubmitFileChietTiet(event: any) {

        this.pageLoading = true;
        if (this.file_chi_tiet == null || this.file_chi_tiet == undefined) {
            Swal.fire('Phải chọn file import', '', 'warning')
            this.pageLoading = false;

        } else {
            var formData = new FormData();
            formData.append('file', this.file_chi_tiet);
            this.http.post('/sys_don_hang_ban.ctr/ImportFromExcelChiTiet/', formData, {
            })
                .subscribe(res => {
                    if (res == "") {
                        Swal.fire('Lưu thành công', '', 'success');
                        this.pageLoading = false;
                        this.file_chi_tiet = null;
                        event.target.value = null;
                        this.rerender();
                    } else {

                        Swal.fire(res.toString(), '', 'warning')
                        this.pageLoading = false;
                        this.file_chi_tiet = null;
                        event.target.value = null;
                    }
                })
        }

    }
    exportToExcel() {

        this.showLoading("", "", true)
        const params = new HttpParams()
            .set('search', this.filter.search)
            .set('status_del', this.filter.status_del)
            .set('open', this.filter.open)
            .set('tu_ngay', this.filter.tu_ngay.toISOString())
            .set('den_ngay', this.filter.den_ngay.toISOString())
            .set('tinh_trang_don_hang', this.filter.tinh_trang_don_hang)
            .set('id_loai_giao_dich', this.filter.id_loai_giao_dich)
            .set('id_hinh_thuc_van_chuyen', this.filter.id_hinh_thuc_van_chuyen)
            .set('id_doi_tuong', this.filter.id_doi_tuong)
            ;

        let uri = this.controller + '.ctr/exportExcel';
        this.http.get(uri, { params, responseType: 'blob', observe: 'response' })
            .subscribe(resp => {
                var res;

                res = resp;
                var downloadedFile = new Blob([res.body], { type: res.body.type });
                const a = document.createElement('a');
                a.setAttribute('style', 'display:none;');
                document.body.appendChild(a);
                a.href = URL.createObjectURL(downloadedFile);
                a.target = '_dAblank';
                a.download = 'DonHangBanBuon/BanSi.xlsx';

                a.click();
                document.body.removeChild(a);
                Swal.close();
            })
    }
    exportExcelDetails() {
        this.showLoading("", "", true)
        const params = new HttpParams()
            ;
        let uri = '/sys_don_hang_ban.ctr/exportExcelDetails';
        this.http.get(uri, { params, responseType: 'blob', observe: 'response' })
            .subscribe(resp => {
                var res;

                res = resp;
                var downloadedFile = new Blob([res.body], { type: res.body.type });
                const a = document.createElement('a');
                a.setAttribute('style', 'display:none;');
                document.body.appendChild(a);
                a.href = URL.createObjectURL(downloadedFile);
                a.target = '_dAblank';
                a.download = 'DonHangBanChiTiet.xlsx';

                a.click();
                document.body.removeChild(a);
                Swal.close();
            })

    }
    openDialogPrint(item): void {

        this.http
            .post(this.controller + '.ctr/getPrint/', {
                id: item.db.id
            }
            ).subscribe(resp => {
                var data: any;
                data = resp;
                const dialogRef = this.dialog.open(cm_mau_in_popupComponent, {
                    width: '878px',
                    disableClose: true,
                    data: {
                        tieu_de: data.tieu_de,
                        noi_dung: data.noi_dung,
                    },
                });
                dialogRef.afterClosed().subscribe(result => {
                    if (result != undefined && result != null) {
                        this.rerender();
                    }


                });

            });

    }
    filterchange() {
        if (this.filter.open == true) {
            this.filter.open = false;
        } else {
            this.filter.open = true;
        }
    }
    get_list_hinh_thuc_van_chuyen() {
        this.list_hinh_thuc_van_chuyen = [
            {
                id: -1,
                name: this._translocoService.translate('system.all')
            },
            {
                id: 1,
                name: this._translocoService.translate('erp.khong_giao_hang')
            },
            {
                id: 2,
                name: this._translocoService.translate('erp.co_giao_hang')
            }
        ];
    }
    get_hinh_thuc_doi_tuong() {
        this.list_hinh_thuc = [
            {
                id: -1,
                name: this._translocoService.translate('system.all')
            },
            {
                id: 1,
                name: this._translocoService.translate('system.ca_nhan')
            },
            {
                id: 2,
                name: this._translocoService.translate('system.to_chuc')
            }
        ];
    }
    get_loai_giao_dich() {
        this.list_loai_giao_dich = [
            {
                id: -1,
                name: this._translocoService.translate('system.all')
            },
            {
                id: 1,
                name: this._translocoService.translate('system.hang_hoa')
            },
            {
                id: 2,
                name: this._translocoService.translate('system.dich_vu')
            }
        ];
    }
    get_cau_hinh_he_thong_gia_ban(): void {
        this.http
            .post('/sys_don_hang_ban.ctr/get_cau_hinh_he_thong_gia_ban/', {
            }
            ).subscribe(resp => {
                var data: any;
                data = resp;
                this.cau_hinh_gia_ban = data.value;
            });

    }
    openDialogAdd(): void {
        this.http
            .post(this.controller + '.ctr/check_kho/', {
            }
            ).subscribe(resp => {
                var id_kho = resp;
                const dialogRef = this.dialog.open(sys_don_hang_ban_popUpAddComponent, {
                    disableClose: true,
                    autoFocus: false,
                    width: '100%',
                    height: '100%',
                    data: {
                        actionEnum: 1,
                        db: {
                            id: 0,
                            kieu_ban: 1,
                            loai_giao_dich: 1,
                            hinh_thuc_van_chuyen: 2,
                            ngay_dat_hang: new Date(),
                            ngay_den_han_thanh_toan: new Date(),
                            tien_van_chuyen: null,
                            tien_vat_van_chuyen: null,
                            thanh_tien_sau_thue_van_chuyen: null,
                            tien_khac: null,
                            vat_khac: null,
                            thanh_tien_sau_thue_chi_phi_khac: null,
                            tong_tien_truoc_thue: null,
                            tong_tien_thue: null,
                            tong_tien_sau_thue: null,
                            tinh_trang_don_hang: "1",
                            id_kho_xuat_ban_le: id_kho,

                        },
                        cau_hinh_gia_ban: this.cau_hinh_gia_ban,
                        id_mat_hangs: "",
                        list_mat_hang: [],
                    },
                });
                dialogRef.afterClosed().subscribe(result => {
                    this.rerender();
                    if (result.db.id == 0) return;
                });

            });

    }
    openDialogFile(item, i): void {
        const dialogRef = this.dialog.open(cm_file_upload_popupComponent, {
            width: '80%',
            height: '80%',
            disableClose: true,
            data: {
                db: {
                    id: item.db.id_file_upload,
                    list_file: []
                }
            },
        });
        dialogRef.afterClosed().subscribe(result => {
            if (result != undefined && result != null) {
                this.rerender();
            }
        });
    }
    openDialogEdit(item, pos): void {
        const dialogRef = this.dialog.open(sys_don_hang_ban_popUpAddComponent, {
            disableClose: true,
            autoFocus: false,
            width: '100%',
            height: '100%',
            data: {
                actionEnum: 2,
                db: {
                    id: item.db.id,
                    id_doi_tuong: item.db.id_doi_tuong,
                },
            }
            //data: model
        });
        dialogRef.afterClosed().subscribe(result => {
            this.rerender();
            if (result.db.id == 0) return;
        });
    }
    openDialogDetail(item, pos): void {
        const dialogRef = this.dialog.open(sys_don_hang_ban_popUpAddComponent, {
            disableClose: true,
            autoFocus: false,
            width: '100%',
            height: '100%',
            data: {
                actionEnum: 3,
                db: {
                    id: item.db.id,
                    id_doi_tuong: item.db.id_doi_tuong,
                },
            }
            //data: model
        });
        dialogRef.afterClosed().subscribe(result => {
            this.rerender();
            if (result.db.id == 0) return;
        });
    }
    ngOnInit(): void {
        this.baseInitData();

        var title = 'ERP-' + this._translocoService.translate('NAV.sys_don_hang_ban');
        var metaTag = [


            { property: 'og:title', content: 'ERP' },
            { property: 'og:image', content: "" },
            { property: 'og:description', content: "" },

        ]
        this._fuseConfigService.config$
            .pipe(takeUntil(this._unsubscribeAll))
            .subscribe((config: AppConfig) => {
                // Store the config
                this.config = config;
            });
        this.seoService.updateTitle(title);
        this.seoService.updateMetaTags(metaTag);

    }
}


