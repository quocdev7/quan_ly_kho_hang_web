import { Component, Inject } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef, MatDialog } from '@angular/material/dialog';


import { HttpClient, HttpEventType } from '@angular/common/http';
import { BasePopUpAddComponent } from 'app/Basecomponent/BasePopupAdd.component';
import { TranslocoService } from '@ngneat/transloco';
import { FuseNavigationService } from '@fuse/components/navigation';
import { ActivatedRoute } from '@angular/router';
import Swal from 'sweetalert2';
import { v4 as uuidv4 } from 'uuid';
import { DataUrl, DOC_ORIENTATION, NgxImageCompressService, UploadResponse } from 'ngx-image-compress';


@Component({
    selector: 'sys_loai_mat_hang_popupAdd',
    templateUrl: 'popupAdd.html',
})
export class sys_loai_mat_hang_popUpAddComponent extends BasePopUpAddComponent {
    public list_status_del: any;
    public list_gioi_tinh: any;
    public list_dinh_khoan_mat_hang: any = [];
    constructor(public dialogRef: MatDialogRef<sys_loai_mat_hang_popUpAddComponent>,
        http: HttpClient, _translocoService: TranslocoService,
        _fuseNavigationService: FuseNavigationService, route: ActivatedRoute,
        @Inject('BASE_URL') baseUrl: string,
        public dialogModal: MatDialog,
        private imageCompress: NgxImageCompressService,

        @Inject(MAT_DIALOG_DATA) data: any) {
        super(_translocoService, _fuseNavigationService, route, baseUrl, http, 'sys_loai_mat_hang', dialogRef, dialogModal);
        this.record = data;
        this.Oldrecord = JSON.parse(JSON.stringify(data));
        this.actionEnum = data.actionEnum;
        if (this.actionEnum == 1) {
            this.baseInitData();
            this.get_code();
        }
        if (this.actionEnum != 2) {
            this.getElementById();
        }
    }

    // openDialogAddDoiTuong(): void {
    //     const dialogRef = this.dialogModal.open(erp_khach_hang_nha_cung_cap_popUpAddComponent, {
    //         disableClose: true,
    //         autoFocus: false,
    //         width: '768px',

    //         data: {
    //             actionEnum: 1,
    //             db: {
    //                 id: 0,
    //             },
    //         },
    //     });
    //     dialogRef.afterClosed().subscribe(result => {
    //         if (result.db.id == 0) return;
    //     });

    // }
    // openDialogChooseDoiTuongChietKhau(id): void {
    //     const dialogRef = this.dialogModal
    //         .open(erp_common_popupChooseDoiTuongChietKhauComponent, {
    //             disableClose: true,
    //             width: '100%',
    //             height: '100%',
    //             data: {
    //                 actionEnum: 1, //Không cho phép thêm đối tượng
    //                 db: {
    //                     id: id,
    //                     list_doi_tuong_chiet_khau: this.record.db.list_doi_tuong_chiet_khau
    //                 },
    //             }
    //         });
    //     dialogRef.afterClosed().subscribe(result => {

    //         var data: any;
    //         data = result;
    //         if (data == undefined) return;
    //         this.record.db.list_doi_tuong_chiet_khau = data;
    //     })
    // }
    delete_doi_tuong(pos, ten_chiet_khau_theo_doi_tuong): void {
        Swal.fire({
            html: `<div>
            <div><span class="mb-0 text-4xl font-bold">Bạn có chắc muốn xóa chiết khấu theo ${ten_chiet_khau_theo_doi_tuong}</span><span class="mb-0 text-4xl font-bold"> không?</span></div>
            </div>`,
            icon: 'warning',
            showCancelButton: true,
            confirmButtonColor: '#3085d6',
            cancelButtonColor: '#d33',
            cancelButtonText: 'Không',
            confirmButtonText: 'Xóa chiết khấu'
        }).then((result) => {
            if (result.isConfirmed) {
                this.record.db.list_doi_tuong_chiet_khau.splice(pos, 1);
            }
        })
    }
    getElementById() {
        this.http.post("sys_loai_mat_hang.ctr/getElementById", {
            id: this.record.db.id
        }).subscribe(resp => {
            var model: any
            model = resp;
            this.record = model
        })
    }
    get_list_dinh_khoan_mat_hang() {
        this.http.post("erp_loai_dinh_khoan_mat_hang.ctr/getListUse", {
        }).subscribe(resp => {
            var model: any
            model = resp;
            this.list_dinh_khoan_mat_hang = model
        })
    }
    get_code() {
        this.http
            .post('/sys_loai_mat_hang.ctr/get_code/', {
            }
            ).subscribe(resp => {
                this.record.db.ma = "Tự động tạo";
            });
    }
    ngOnInit(): void {
        this.get_list_dinh_khoan_mat_hang();
    }
}
