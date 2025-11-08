import { Component, Inject, ViewChild } from '@angular/core';
import { HttpClient, HttpParams, HttpResponse } from '@angular/common/http';
import { DataTablesResponse } from 'app/Basecomponent/datatable';
import { TranslocoService } from '@ngneat/transloco';
import { sys_khach_hang_nha_cung_cap_popUpAddComponent } from './popupAdd.component';
import { MatDialog } from '@angular/material/dialog';
import { DataTableDirective } from 'angular-datatables';
import { Subject } from 'rxjs';
import Swal from 'sweetalert2';
import { BaseIndexDatatableComponent } from 'app/Basecomponent/BaseIndexDatatable.component';
import { FuseNavigationService } from '@fuse/components/navigation';
import { ActivatedRoute } from '@angular/router';
import { SeoService } from '@fuse/services/seo.service';


@Component({
    selector: 'sys_khach_hang_nha_cung_cap_index',
    templateUrl: './index.component.html',
    styleUrls: ['./index.component.scss']
})

export class sys_khach_hang_nha_cung_cap_indexComponent extends BaseIndexDatatableComponent {


    public list_status_del: any;
    public file: any;
    public list_loai: any = [];
    public list_hinh_thuc: any = [];
    public list_loai_hinh: any = [];
    constructor(
        private seoService: SeoService,
        http: HttpClient, dialog: MatDialog
        , _translocoService: TranslocoService
        , _fuseNavigationService: FuseNavigationService, route: ActivatedRoute
        , @Inject('BASE_URL') baseUrl: string
    ) {
        super(http, baseUrl, _translocoService, _fuseNavigationService, route, dialog, 'sys_khach_hang_nha_cung_cap',
            { search: "", status_del: "1", loai: "-1", hinh_thuc: "-1", loai_hinh: "-1" }
        )

        this.list_status_del = [

            {
                id: "1",
                name: this._translocoService.translate('system.use')
            },
            {
                id: "2",
                name: this._translocoService.translate('system.not_use')
            }
        ];
        this.load_list_hinh_thuc();
    }
    load_list_hinh_thuc() {
        this.list_hinh_thuc = [
            {
                id: "-1",
                name: this._translocoService.translate('system.all')
            },
            {
                id: "1",
                name: this._translocoService.translate('system.ca_nhan')
            },
            {
                id: "2",
                name: this._translocoService.translate('system.to_chuc')
            },
            {
                id: "3",
                name: this._translocoService.translate('system.phong_ban')
            },
            {
                id: "4",
                name: this._translocoService.translate('system.nhan_vien')
            },
            // {
            //     id: "5",
            //     name: this._translocoService.translate('system.khach_hang')
            // },
            // {
            //     id: "6",
            //     name: this._translocoService.translate('system.nha_cung_cap')
            // }
        ];
    }
    load_list_loai_hinh() {
        this.list_loai_hinh = [
            {
                id: "-1",
                name: this._translocoService.translate('system.all')
            },
            {
                id: "1",
                name: this._translocoService.translate('system.khach_hang')
            },
            {
                id: "2",
                name: this._translocoService.translate('system.nha_cung_cap')
            }
        ];
    }
    openDialogAdd(): void {
        const dialogRef = this.dialog.open(sys_khach_hang_nha_cung_cap_popUpAddComponent, {
            disableClose: true,
            autoFocus: false,
            width: '768px',
            data: {
                actionEnum: 1,
                db: {
                    id: 0,
                    hinh_thuc: 1
                },
                list_loai: null
            },
        });
        dialogRef.afterClosed().subscribe(result => {
            if (result.db.id == 0) return;
            this.rerender();
        });
    }
    onFileSelected(event: any) {
        this.file = event.target.files[0];
        //event.target.value = null;
    }
    dowloadFileMau() {
        var url = '/sys_khach_hang_nha_cung_cap.ctr/downloadtemp';
        window.location.href = url;
    }
    onSubmitFile(event: any) {
        if (this.file == null || this.file == undefined) {
            Swal.fire('Phải chọn file import', '', 'warning')
        } else {
            this.showLoading("", "", true)
            var formData = new FormData();
            formData.append('file', this.file);
            this.http.post('/sys_khach_hang_nha_cung_cap.ctr/ImportFromExcel/', formData, {
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
    openDialogEdit(model, pos): void {
        model.actionEnum = 2;
        const dialogRef = this.dialog.open(sys_khach_hang_nha_cung_cap_popUpAddComponent, {
            disableClose: true,
            width: '768px',
            data: model
        });
        dialogRef.afterClosed().subscribe(result => {
            if (result != undefined && result != null) this.listData[pos] = result;
            this.rerender();
        });
    }
    exportToExcel() {
        this.showLoading("", "", true)
        const params = new HttpParams()
            .set('search', this.filter.search)
            .set('status_del', this.filter.status_del)
            .set('hinh_thuc', this.filter.hinh_thuc)
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
                a.download = 'KhachHangNhaCungCap.xlsx'
                a.click();
                document.body.removeChild(a);
                Swal.close();
            })
    }
    openDialogDetail(ma, pos): void {
        const dialogRef = this.dialog.open(sys_khach_hang_nha_cung_cap_popUpAddComponent, {
            disableClose: true,
            width: '768px',
            data: {
                actionEnum: 3,
                db: {
                    ma: ma
                }
            }
        });
        dialogRef.afterClosed().subscribe(result => {
            if (result != undefined && result != null) this.listData[pos] = result;
        });
    }



    ngOnInit(): void {
        this.baseInitData();
        this.load_list_loai_hinh();
        var title = this._translocoService.translate('NAV.sys_khach_hang_nha_cung_cap');
        var metaTag = [


            { property: 'og:title', content: 'SHUNGO' },
            { property: 'og:image', content: "" },
            { property: 'og:description', content: "" },

        ]
        this.seoService.updateTitle(title);
        this.seoService.updateMetaTags(metaTag);

    }


}


