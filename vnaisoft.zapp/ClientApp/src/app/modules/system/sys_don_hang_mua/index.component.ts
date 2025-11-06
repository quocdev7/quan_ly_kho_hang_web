import { Component, Inject, ViewChild } from '@angular/core';


import { HttpClient, HttpParams, HttpResponse } from '@angular/common/http';

import { DataTablesResponse } from 'app/Basecomponent/datatable';
import { TranslocoService } from '@ngneat/transloco';
import { sys_don_hang_mua_popUpAddComponent } from './popupAdd.component';
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
import { time } from 'console';
//import { sys_don_hang_mua_popupImportComponent } from './popupImport..component';
//import { sys_don_hang_mua_log_popupComponent } from '../sys_don_hang_mua_log/popupHistoryEdit.component';

@Component({
    selector: 'sys_don_hang_mua_index',
    templateUrl: './index.component.html',
    styleUrls: ['./index.component.scss']
})

export class sys_don_hang_mua_indexComponent extends BaseIndexDatatableComponent {
    public list_doi_tuong: any;
    public list_status_del: any;
    public list_hinh_thuc: any;
    public list_kho: any;
    public list_loai_giao_dich: any;
    public file: any;
    public config: AppConfig;
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
        super(http, baseUrl, _translocoService, _fuseNavigationService, route, dialog, 'sys_don_hang_mua',
            { search: "", status_del: "1", id_cong_ty: "-1", tu_ngay: null, den_ngay: null, id_doi_tuong: -1, id_kho: "-1", open: false, id_loai_giao_dich: -1, is_nhap_du: "-1", is_chi_du: "-1" }
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
        this.get_hinh_thuc_doi_tuong();
        this.get_list_kho();
        this.get_loai_giao_dich();
        this.load_date();
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
    // openDialogHistoryEdit(item, i): void {
    //     const dialogRef = this.dialog.open(sys_don_hang_mua_log_popupComponent, {
    //         width: '80%',
    //         height: '80%',
    //         disableClose: true,
    //         data: item,
    //     });
    //     dialogRef.afterClosed().subscribe(result => {

    //         var data: any;
    //         data = result;
    //         if (data == undefined) return;
    //     })
    // }
    load_date(): void {
        this.filter.tu_ngay = new Date();
        this.filter.tu_ngay.setDate(this.filter.tu_ngay.getDate() - 365);
        this.filter.den_ngay = new Date();
    }
    exportToExcel() {
        this.showLoading("", "", true)
        const params = new HttpParams()
            .set('search', this.filter.search)
            .set('status_del', this.filter.status_del)
            .set('open', this.filter.open)
            .set('tu_ngay', this.filter.tu_ngay.toISOString())
            .set('den_ngay', this.filter.den_ngay.toISOString())
            .set('id_doi_tuong', this.filter.id_doi_tuong)
            .set('id_loai_giao_dich', this.filter.id_loai_giao_dich)
            ;
        let uri = '/sys_don_hang_mua.ctr/exportExcel';
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
                a.download = 'DonHangMua.xlsx';

                a.click();
                document.body.removeChild(a);
                Swal.close();
            })

    }
    exportExcelDetails() {
        this.showLoading("", "", true)
        const params = new HttpParams()
            ;
        let uri = '/sys_don_hang_mua.ctr/exportExcelDetails';
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
                a.download = 'DonHangMuaChiTiet.xlsx';

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
    filterchange() {
        if (this.filter.open == true) {
            this.filter.open = false;
        } else {
            this.filter.open = true;
        }
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
    get_list_kho() {
        this.http.post('/sys_kho.ctr/getListUse/', {

        }).subscribe(resp => {
            this.list_kho = resp;
            this.list_kho.splice(0, 0, { id: '-1', name: this._translocoService.translate('system.all') })
        })
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
    openDialogAdd(): void {
        this.http
            .post(this.controller + '.ctr/check_kho/', {
            }
            ).subscribe(resp => {
                var id_kho = resp;
                const dialogRef = this.dialog.open(sys_don_hang_mua_popUpAddComponent, {
                    disableClose: true,
                    autoFocus: false,
                    width: '100%',
                    data: {
                        actionEnum: 1,
                        db: {
                            id: 0,
                            loai_giao_dich: 1,
                            phuong_thuc_thanh_toan: 2,
                            ngay_dat_hang: new Date(),
                            ngay_du_kien_nhan_hang: new Date(),
                            tien_van_chuyen: null,
                            tien_vat_van_chuyen: null,
                            thanh_tien_sau_thue_van_chuyen: null,
                            tien_khac: null,
                            vat_khac: null,
                            thanh_tien_sau_thue_chi_phi_khac: null,
                            tong_tien_truoc_thue: null,
                            tong_tien_thue: null,
                            tong_tien_sau_thue: null,
                            id_kho_nhap: id_kho,

                        },
                        id_mat_hangs: "",
                        list_mat_hang: []
                    },
                });
                dialogRef.afterClosed().subscribe(result => {
                    this.rerender();
                    if (result.db.id == 0) return;
                });
            });
    }
    openDialogEdit(item, pos): void {
        const dialogRef = this.dialog.open(sys_don_hang_mua_popUpAddComponent, {
            disableClose: true,
            autoFocus: false,
            width: '100%',
            data: {
                actionEnum: 2,
                db: {
                    id: item.db.id,
                    id_doi_tuong: item.db.id_doi_tuong
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
        const dialogRef = this.dialog.open(sys_don_hang_mua_popUpAddComponent, {
            disableClose: true,
            autoFocus: false,
            width: '100%',
            data: {
                actionEnum: 3,
                db: {
                    id: item.db.id,
                    id_doi_tuong: item.db.id_doi_tuong
                },
            }
            //data: model
        });
        dialogRef.afterClosed().subscribe(result => {
            this.rerender();
            if (result.db.id == 0) return;
        });
    }
    onFileSelected(event: any) {
        this.file = event.target.files[0];
    }
    dowloadFileMau() {
        var url = '/sys_don_hang_mua.ctr/downloadtemp';
        window.location.href = url;
    }
    onSubmitFile(event: any) {
        if (this.file == null || this.file == undefined) {
            Swal.fire('Phải chọn file import', '', 'warning')
        } else {
            this.showLoading("", "", true)
            var formData = new FormData();
            formData.append('file', this.file);
            this.http.post('/sys_don_hang_mua.ctr/ImportFromExcel/', formData, {
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
    ngOnInit(): void {
        this.baseInitData();

        var title = 'ERP-' + this._translocoService.translate('NAV.sys_don_hang_mua');
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


