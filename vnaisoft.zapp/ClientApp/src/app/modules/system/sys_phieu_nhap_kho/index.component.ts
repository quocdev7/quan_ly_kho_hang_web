
import { Component, Inject, ViewChild } from '@angular/core';
import { HttpClient, HttpParams, HttpResponse } from '@angular/common/http';
import { DataTablesResponse } from 'app/Basecomponent/datatable';
import { TranslocoService } from '@ngneat/transloco';
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
import { sys_phieu_nhap_kho_popUpAddComponent } from './popupAdd.component';
import { cm_file_upload_popupComponent } from '@fuse/components/commonComponent/cm_file_upload/cm_file_upload_popup.component';
import { cm_mau_in_popupComponent } from '@fuse/components/commonComponent/cm_mau_in/cm_mau_in_popup.component';
@Component({
    selector: 'sys_phieu_nhap_kho_index',
    templateUrl: './index.component.html',
    styleUrls: ['./index.component.scss']
})

export class sys_phieu_nhap_kho_indexComponent extends BaseIndexDatatableComponent {
    private _unsubscribeAll: Subject<any> = new Subject<any>();
    config: AppConfig;

    public list_kho: any = [];
    public list_ngan_hang: any;
    public list_tien_te: any;
    public list_status_del: any;
    public list_don_hang_ban: any;
    public list_loai_nhap_kho: any;
    public list_don_hang: any;
    public list_nguon: any;
    public code: any;
    public file: any;
    constructor(
        private router: Router,
        private seoService: SeoService,
        private _fuseConfigService: FuseConfigService,

        http: HttpClient, dialog: MatDialog
        , _translocoService: TranslocoService
        , _fuseNavigationService: FuseNavigationService, route: ActivatedRoute
        , @Inject('BASE_URL') baseUrl: string
    ) {
        super(http, baseUrl, _translocoService, _fuseNavigationService, route, dialog, 'sys_phieu_nhap_kho',
            {
                search: "",
                status_del: "1",
                id_kho: "-1",
                id_loai_nhap: "-1",
                id_don_hang: "-1",
                nguon: "-1",
                tu_ngay: null,
                den_ngay: null,
                is_xuat_kho: 0,
                open: false
                //id_don_hang:""
            }
        )
    }

    onFileSelected(event: any) {
        this.file = event.target.files[0];
        //this.onSubmitFile();
        //event.target.value = null;
    }
    dowloadFileMau() {
        var url = '/sys_phieu_nhap_kho.ctr/downloadtemp';
        window.location.href = url;
    }
    onSubmitFile(event: any) {
        if (this.file == null || this.file == undefined) {
            Swal.fire('Phải chọn file import', '', 'warning')
        } else {
            this.showLoading("", "", true)
            var formData = new FormData();
            formData.append('file', this.file);
            this.http.post('/sys_phieu_nhap_kho.ctr/ImportFromExcel/', formData, {
            })
                .subscribe(res => {
                    Swal.close();
                    var resp: any
                    resp = res
                    if (res == "1") {
                        Swal.fire('Lưu thành công', '', 'success');
                        this.rerender();
                        event.target.value = null;
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
                                var url = 'sys_home.ctr/downloadtempFileError?path=' + res;
                                window.location.href = url;
                            }
                        })
                    }
                })
        }
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
    go_to_create_edit_phieu_nhap_kho(id, action): void {

        const url = '/sys_phieu_nhap_kho_createEdit/' + id + "/" + action;
        if (this.config.layout != 'empty') {
            this.router.navigateByUrl(url);
        } else {
            window.open(url, "_blank");
        }
    }
    load_trang_thai(): void {
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
    }
    load_don_hang_ban(): void {
        this.http
            .post('/sys_don_hang_ban.ctr/getListUse/', {
            }
            ).subscribe(resp => {
                this.list_don_hang_ban = resp;
            });


    }
    get_list_kho(): void {
        this.http
            .post('/sys_kho.ctr/getListUse/', {}
            ).subscribe(resp => {
                this.list_kho = resp;
                this.listData.forEach(q => {
                    this.list_kho = this.list_kho.filter(d => d.id == q.db.id_kho);
                })
                this.list_kho.splice(0, 0, { id: '-1', name: this._translocoService.translate('system.all') })
                this.filter.list_kho = this.list_kho[0].id;
            });
    }

    load_date(): void {
        this.filter.tu_ngay = new Date();
        this.filter.tu_ngay.setDate(this.filter.tu_ngay.getDate() - 365);
        this.filter.den_ngay = new Date();
    }
    // openDialogPrint(item): void {
    //     const dialogRef = this.dialog.open(cm_mau_in_popupComponent, {
    //         width: '80%',
    //         height: '80%',
    //         disableClose: true,
    //         data: {
    //             controller: this.controller,
    //             item: item
    //         },
    //     });
    //     dialogRef.afterClosed().subscribe(result => {
    //         if (result != undefined && result != null) {
    //             this.rerender();
    //         }


    //     });
    // }
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
            }
        });
    }
    openDialogAdd(): void {
        const dialogRef = this.dialog.open(sys_phieu_nhap_kho_popUpAddComponent, {
            disableClose: true,
            autoFocus: false,
            width: '878px',
            data: {
                actionEnum: 1,
                db: {
                    id: 0,
                    ngay_nhap: new Date(),
                },
                id_mat_hangs: "",
                list_mat_hang: []
            },
        });
        dialogRef.afterClosed().subscribe(result => {
            this.load_don_hang_ban();
            this.rerender();
            if (result.db.id == 0) return;
        });
    }
    openDialogEdit(model, pos): void {
        model.actionEnum = 2;
        const dialogRef = this.dialog.open(sys_phieu_nhap_kho_popUpAddComponent, {
            disableClose: true,
            autoFocus: false,
            width: '878px',
            data: model
        });
        dialogRef.afterClosed().subscribe(result => {
            if (result != undefined && result != null) this.listData[pos] = result;
            this.load_don_hang_ban();
        });
    }
    openDialogDetail(model, pos): void {
        model.actionEnum = 3;
        const dialogRef = this.dialog.open(sys_phieu_nhap_kho_popUpAddComponent, {
            disableClose: true,
            autoFocus: false,
            width: '878px',
            data: model
        });
        dialogRef.afterClosed().subscribe(result => {
            if (result != undefined && result != null) this.listData[pos] = result;
        });
    }
    exportToExcel() {
        this.showLoading("", "", true)
        const params = new HttpParams()
            .set('search', this.filter.search)
            .set('status_del', this.filter.status_del)
            .set('open', this.filter.open)
            .set('tu_ngay', this.filter.tu_ngay.toISOString())
            .set('den_ngay', this.filter.den_ngay.toISOString())
            .set('id_loai_nhap', this.filter.id_loai_nhap)
            .set('id_kho', this.filter.id_kho)
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
                a.download = 'PhieuNhapKho.xlsx'
                a.click();
                document.body.removeChild(a);
                Swal.close();
            })
    }
    public sudung(id1): void {
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
                this.http
                    .post(this.controller + '.ctr/sudung/',
                        {
                            id: id1,
                        }
                    ).subscribe(resp => {
                        Swal.fire('Ngưng sử dụng thành công', '', 'success');
                        this.rerender();
                    },
                        error => {
                            if (error.status == 403) {
                                Swal.fire(this._translocoService.translate('no_permission'), "", "warning");
                            }


                        }
                    );
            }
        })

    }
    ngOnInit(): void {
        this.baseInitData();
        this.load_trang_thai();
        //this.load_tien_te();
        this.load_date();
        this.get_list_kho();
        this.load_don_hang_ban();
        this._fuseConfigService.config$
            .pipe(takeUntil(this._unsubscribeAll))
            .subscribe((config: AppConfig) => {
                // Store the config
                this.config = config;
            });

        var title = 'SHUNGO-' + this._translocoService.translate('NAV.sys_phieu_nhap_kho');
        var metaTag = [


            { property: 'og:title', content: 'SHUNGO' },
            { property: 'og:image', content: "" },
            { property: 'og:description', content: "" },

        ]
        this.seoService.updateTitle(title);
        this.seoService.updateMetaTags(metaTag);

    }
    public generateCode(id_cong_ty): void {
        this.http
            .post('/kt_xuat_kho.ctr/get_ma_don_hang/', {

            }
            ).subscribe(resp => {

                this.code = resp;
            });
    }


}
