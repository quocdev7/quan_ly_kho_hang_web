import { Component, HostListener, Inject } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef, MatDialog } from '@angular/material/dialog';


import { HttpClient, HttpEventType } from '@angular/common/http';
import { BasePopUpAddComponent } from 'app/Basecomponent/BasePopupAdd.component';
import { TranslocoService } from '@ngneat/transloco';
import { FuseNavigationService } from '@fuse/components/navigation';
import { ActivatedRoute } from '@angular/router';
import { BasePopUpAddTypeComponent } from 'app/Basecomponent/BasePopupAddType.component';
import { sys_phieu_xuat_kho_model } from './sys_phieu_xuat_kho.types';
// import { sys_mat_hang_popUpAddComponent } from 'app/modules/erp/sys_mat_hang/popupAdd.component';

import Swal from 'sweetalert2';
import { list_nguon } from 'app/core/data/data';
// import { sys_common_popupChooseMatHangComponent } from '../sys_common/popupChooseMatHang.component';
import { sys_don_hang_ban_popUpAddComponent } from '../sys_don_hang_ban/popupAdd.component';
import { sys_don_hang_mua_popUpAddComponent } from '../sys_don_hang_mua/popupAdd.component';
import { cm_mau_in_popupComponent } from '@fuse/components/commonComponent/cm_mau_in/cm_mau_in_popup.component';
import { doi_tuong_tu_do } from 'app/core/data/data';
import { sys_common_popupChooseDonHangBanComponent } from '../sys_common/popupChooseDonHangBan.component';
import { sys_common_popupChooseDonHangMuaComponent } from '../sys_common/popupChooseDonHangMua.component';
// import { sys_common_popupChooseDonHangMuaHHComponent } from '../sys_common/popupChooseDonHangMuaHH.component';
// import { sys_common_popupChooseDonHangBanHHComponent } from '../sys_common/popupChooseDonHangBanHH.component';
@Component({
    selector: 'sys_phieu_xuat_kho_popupAdd',
    templateUrl: 'popupAdd.html',
    styleUrls: ['./popupAdd.component.scss']
})
export class sys_phieu_xuat_kho_popUpAddComponent extends BasePopUpAddTypeComponent<sys_phieu_xuat_kho_model> {
    public openTab: any = 1;
    public ma_mat_hang: any;

    public file_logo: any;
    public Progress_logo: any = -1;
    public group_field: any;
    public additem: any;
    public item_chose: any;
    public dtOptions: any;
    public list_cong_ty: any;
    public id_don_hang: any;

    public list_doi_tuong: any;
    public record: any;
    public list_don_hang_mua: any;
    public list_loai_xuat: any;
    public list_kho: any;
    public total_so_luong: any;
    public list_don_hang_ban: any;
    public list_vi_tri_kho: any;
    public list_mat_hang: any;
    public current_date: any;
    public list_mat_hang_xuat: any;
    public check_all_mat_hang: any;
    public dataFilter: any;
    public list_nguon_tao_phieu: any;
    public list_tien_te: any;
    public id_mat_hangs: any = [];
    public is_tien_te_chinh: any;
    public total_thanh_tien_truoc_thue: any;
    public total_so_tien_chiet_khau: any;
    public total_thanh_tien_sau_chiet_khau: any;
    public total_tien_thue: any;
    public total_thanh_tien_sau_thue: any;

    public list_loai_nhap_kho: any = [];
    public list_don_hang: any = [];
    public list_don_hang_ban_thuc_hien: any = [];
    public list_nguon: any = [];
    public fileData: any;
    public file: any;
    public check_doi_tuong: any = false;

    public data_doi_tuong: any = {};

    constructor(public dialogRef: MatDialogRef<sys_phieu_xuat_kho_popUpAddComponent>,
        http: HttpClient, _translocoService: TranslocoService,
        _fuseNavigationService: FuseNavigationService, route: ActivatedRoute,
        @Inject('BASE_URL') baseUrl: string,
        private dialog: MatDialog,
        public dialogModal: MatDialog,
        @Inject(MAT_DIALOG_DATA) data: any) {
        super(_translocoService, _fuseNavigationService, route, baseUrl, http, 'sys_phieu_xuat_kho', dialogRef, dialogModal);

        this.record = data;

        this.Oldrecord = JSON.parse(JSON.stringify(data));
        this.actionEnum = data.actionEnum;
        if (this.actionEnum == 1) {

            this.record.check_doi_tuong = 1;
            //this.record.db.ten = "Tự động tạo";
            this.record.db.ngay_xuat = new Date();
            //this.record.db.nguon = 1;
            this.get_code();
            this.record.list_mat_hang = [];
            this.baseInitData();

        } else {
            this.getElementById();

        }
        this.record.so_luong_mh = 1
        this.get_list_loai();
        this.get_list_don_hang();


    }
    combinedCode = ""
    startFrom = new Date().getTime();
    @HostListener('document:keypress', ['$event'])
    keyEvent(event: KeyboardEvent) {
        if (event.key === 'Enter') {
            if (this.combinedCode != "" && this.combinedCode.length > 6 && (this.ma_mat_hang ?? "") == "") {
                this.ma_mat_hang = this.combinedCode;
            }
            this.combinedCode = "";
        } else {
            var timming = new Date().getTime() - this.startFrom;
            this.startFrom = new Date().getTime();
            console.log(timming);
            console.log(event.key.toString());
            if (timming < 50 || this.combinedCode == "") {
                this.combinedCode += event.key;
            } else {
                this.combinedCode = "";
            }
        }
    }

    getElementById(): void {
        this.http
            .post('/sys_phieu_xuat_kho.ctr/getElementById/', {
                id: this.record.db.id
            }
            ).subscribe(resp => {

                this.record = resp;
            });
    }
    public get_list_loai(): void {
        this.http
            .post('/sys_loai_nhap_xuat.ctr/getListUse/', {
            }
            ).subscribe(resp => {
                    this.list_loai_xuat = resp;
            });
    }


    change_loai_xuat(): void {
        this.record.db.nguon = this.list_loai_xuat.filter(q => q.id == this.record.db.id_loai_xuat)[0].nguon;
        this.record.ma_don_hang = null;
        this.record.db.id_don_hang_ban = null;
        this.record.db.id_don_hang_mua = null;
        this.record.list_mat_hang = [];
        // this.record.list_mat_hang = [];
    }
    public get_list_don_hang(): void {
        this.http
            .post('/sys_don_hang_ban.ctr/getListUse/', {
            }
            ).subscribe(resp => {
                this.list_don_hang = resp;
            });
    }
    get_code() {
        this.http
            .post('/sys_phieu_xuat_kho.ctr/get_code/', {
            }
            ).subscribe(resp => {
                this.record.db.ma = "Tự động tạo";
            });
    }
    openDialogChooseDonHang(id, type): void {
        if (type == 2) {
            const dialogRef = this.dialogModal
                .open(sys_common_popupChooseDonHangMuaComponent, {
                    disableClose: true,
                    width: '100%',
                    height: '100%',
                    data: {
                        db: {
                            id: id,
                            nguon: type + "px"
                        },

                    }
                });
            dialogRef.afterClosed().subscribe(result => {
                var data: any;
                data = result;
                if (data == undefined) return;
                this.record.db.id_don_hang_mua = data.id;
                this.record.db.nguon = 2;
                this.record.ma_don_hang = data.ma;
                this.record.ghi_chu_don_hang = data.ghi_chu;
                this.record.list_mat_hang = [];
                this.get_list_mat_hang_theo_don_hang_mua();
            })
        } else {
            const dialogRef = this.dialogModal
                .open(sys_common_popupChooseDonHangBanComponent, {
                    disableClose: true,
                    width: '100%',
                    height: '100%',
                    data: {
                        db: {
                            id: id,
                            nguon: type + "px"
                        },
                    }
                });
            dialogRef.afterClosed().subscribe(result => {
                var data: any;
                data = result;
                if (data == undefined) return;
                this.record.db.nguon = 1;
                this.record.db.id_don_hang_ban = data.id;
                this.record.ma_don_hang = data.ma;
                this.record.ghi_chu_don_hang = data.ghi_chu;
                this.record.list_mat_hang = [];
                this.get_list_mat_hang_theo_don_hang_ban();
            })

        }
    }
    openDialogDetailDonHangBan(id, pos): void {
        const dialogRef = this.dialog.open(sys_don_hang_ban_popUpAddComponent, {
            disableClose: true,
            autoFocus: false,
            width: '878px',
            data: {
                actionEnum: 3,
                db: {
                    id: id,
                },
            }
            //data: model
        });
        dialogRef.afterClosed().subscribe(result => {
            if (result.db.id == 0) return;
        });
    }
    openDialogDetailDonHangMua(id, pos): void {
        const dialogRef = this.dialog.open(sys_don_hang_mua_popUpAddComponent, {
            disableClose: true,
            autoFocus: false,
            width: '878px',
            data: {
                actionEnum: 3,
                db: {
                    id: id,
                },
            }
            //data: model
        });
        dialogRef.afterClosed().subscribe(result => {
            if (result.db.id == 0) return;
        });
    }
    get_list_mat_hang_theo_don_hang_mua() {
        this.http
            .post('/sys_don_hang_mua.ctr/getElementById/', {
                id: this.record.db.id_don_hang_mua,
            }
            ).subscribe(resp => {
                var data: any;
                data = resp;

                if (data.list_mat_hang.length == 0) {

                    Swal.fire(this._translocoService.translate('system.khong_co_mat_hang'), "", "warning");

                } else {

                    this.load_list_choose_mat_hang_don_hang(data.list_mat_hang);
                    //this.record.list_mat_hang.push(data.list_mat_hang);
                }

            });
    }
    get_list_mat_hang_theo_don_hang_ban() {
        this.http
            .post('/sys_don_hang_ban.ctr/getElementById/', {
                id: this.record.db.id_don_hang_ban,
            }
            ).subscribe(resp => {
                debugger
                var data: any;
                data = resp;
                if (data.list_mat_hang.length == 0) {

                    Swal.fire(this._translocoService.translate('system.khong_co_mat_hang'), "", "warning");

                } else {
                    this.load_list_choose_mat_hang_don_hang(data.list_mat_hang);
                }
            });
    }

    delete_mat_hang(pos, ten_mat_hang): void {

        Swal.fire({
            html: `<div>
            <div><span class="mb-0 text-4xl font-bold">Bạn có chắc muốn xóa mặt hàng ${ten_mat_hang}</span><span class="mb-0 text-4xl font-bold"> không?</span></div>
            </div>`,
            icon: 'warning',
            showCancelButton: true,
            confirmButtonColor: '#3085d6',
            cancelButtonColor: '#d33',
            cancelButtonText: 'Không',
            confirmButtonText: 'Xóa mặt hàng'
        }).then((result) => {
            if (result.isConfirmed) {
                this.record.list_mat_hang.splice(pos, 1);

            }
        })
    }
    load_list_choose_mat_hang_don_hang(listDataN): any {

        if (listDataN.length == 0) {
        }
        else {
            for (let i = 0; i < listDataN.length; i++) {
                let model = listDataN[i];
                let str = "";
                let obj_mat_hang: any;
                obj_mat_hang = {
                    db: {

                        id_mat_hang: model.db.id_mat_hang,
                        id_don_vi_tinh: model.db.id_don_vi_tinh,
                        so_luong: model.db.so_luong,
                        ghi_chu: null,

                    },
                    ma_mat_hang: model.ma_mat_hang,
                    ten_mat_hang: model.ten_mat_hang,
                    ten_don_vi_tinh: model.ten_don_vi_tinh,
                    ten_thuoc_tinh: model.ten_thuoc_tinh,
                    id_mat_hang: model.db.id_mat_hang

                }

                this.record.list_mat_hang.push(obj_mat_hang);

            }
        }

    }
    load_list_choose(listDataN): any {

        if (listDataN.length == 0) {
        }
        else {
            for (let i = 0; i < listDataN.length; i++) {
                let model = listDataN[i];
                let str = "";
                var don_gia = null;
                don_gia = model.gia_mua;
                let obj_mat_hang: any;
                obj_mat_hang = {
                    db: {

                        id_mat_hang: model.db.id,
                        id_don_vi_tinh: model.db.id_don_vi_tinh,
                        ma_vach: model.db.ma_vach,
                        so_luong: this.record.so_luong_mh,
                        ghi_chu: model.db.ghi_chu,
                    },
                    ma_mat_hang: model.db.ma,
                    ten_mat_hang: model.db.ten,
                    ten_don_vi_tinh: model.ten_don_vi_tinh,
                    ten_thuoc_tinh: model.ten_thuoc_tinh,
                    id_mat_hang: model.db.id
                }

                this.record.list_mat_hang.push(obj_mat_hang);

            }
        }

    }
    public tableapi: any;
    ngOnInit(): void {
        var that = this;
        this.dtOptions = {
            language: {
                zeroRecords: ""
            },
            responsive: true,
            ordering: false,
            dom: 'Bfrtip',
            "drawCallback": function (settings) {
                var api = this.api();
                that.tableapi = api;
                setTimeout(function () {
                    api.columns.adjust();
                    $('tbody').on('click', 'tr', function () {
                        if ($(this).hasClass('selected')) {
                            $(this).removeClass('selected');
                        }
                        else {
                            $('tr.selected').removeClass('selected');
                            $(this).addClass('selected');
                        }
                    });

                }, 500);


            },
            "searching": false,
        };
    }
}
