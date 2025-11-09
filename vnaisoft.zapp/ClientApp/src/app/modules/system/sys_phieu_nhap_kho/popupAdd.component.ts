import { Component, HostListener, Inject } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef, MatDialog } from '@angular/material/dialog';


import { HttpClient, HttpEventType } from '@angular/common/http';
import { BasePopUpAddComponent } from 'app/Basecomponent/BasePopupAdd.component';
import { TranslocoService } from '@ngneat/transloco';
import { FuseNavigationService } from '@fuse/components/navigation';
import { ActivatedRoute } from '@angular/router';
import { BasePopUpAddTypeComponent } from 'app/Basecomponent/BasePopupAddType.component';
import { sys_phieu_nhap_kho_model } from './sys_phieu_nhap_kho.types';
// import { sys_mat_hang_popUpAddComponent } from 'app/modules/erp/sys_mat_hang/popupAdd.component';

import Swal from 'sweetalert2';
// import { sys_common_popupChooseMatHangComponent } from '../sys_common/popupChooseMatHang.component';
import { list_nguon } from 'app/core/data/data';
import { sys_don_hang_ban_popUpAddComponent } from '../sys_don_hang_ban/popupAdd.component';
import { sys_don_hang_mua_popUpAddComponent } from '../sys_don_hang_mua/popupAdd.component';
import { cm_mau_in_popupComponent } from '@fuse/components/commonComponent/cm_mau_in/cm_mau_in_popup.component';
import { doi_tuong_tu_do } from 'app/core/data/data';
// import { sys_common_popupChooseDonHangMuaHHComponent } from '../sys_common/popupChooseDonHangMuaHH.component';
// import { sys_common_popupChooseDonHangBanHHComponent } from '../sys_common/popupChooseDonHangBanHH.component';


@Component({
    selector: 'sys_phieu_nhap_kho_popupAdd',
    templateUrl: 'popupAdd.html',
})
export class sys_phieu_nhap_kho_popUpAddComponent extends BasePopUpAddTypeComponent<sys_phieu_nhap_kho_model> {
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
    public list_loai_nhap_kho: any;
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
    public openTab: any = 1;
    public list_loai_nhap: any = [];
    public list_don_hang: any = [];
    public list_don_hang_ban_thuc_hien: any = [];
    public list_nguon: any = [];
    public file: any;
    public check_doi_tuong: any = true;
    public doi_tuong_tu_do: any;
    public data_doi_tuong: any;
    constructor(public dialogRef: MatDialogRef<sys_phieu_nhap_kho_popUpAddComponent>,
        http: HttpClient, _translocoService: TranslocoService,
        _fuseNavigationService: FuseNavigationService, route: ActivatedRoute,
        @Inject('BASE_URL') baseUrl: string,
        private dialog: MatDialog,
        public dialogModal: MatDialog,
        @Inject(MAT_DIALOG_DATA) data: any) {
        super(_translocoService, _fuseNavigationService, route, baseUrl, http, 'sys_phieu_nhap_kho', dialogRef, dialogModal);

        this.record = data;
        this.Oldrecord = JSON.parse(JSON.stringify(data));
        this.actionEnum = data.actionEnum;
        if (this.actionEnum != 2) {
            this.dialogRef.keydownEvents().subscribe(event => {
                if (event.key === "Escape") {
                    this.dialogRef.close();
                }
            });
        }
        if (this.actionEnum == 1) {

            this.get_code();
            this.record.db.ten = "Tự động tạo";
            this.record.db.ngay_nhap = new Date();
            this.doi_tuong_tu_do = doi_tuong_tu_do;
            this.baseInitData();
        }
        if (this.actionEnum != 1) {
            this.load_list_mat_hang_nhap_kho();
        }
        this.record.so_luong_mh = 1
        this.get_list_loai_nhap();
        this.get_list_don_hang();
        this.get_list_kho();
        this.load_nguon()
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
                    }
                });
            });
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
    get_list_mat_hang_theo_don_hang_mua() {
        this.http
            .post('/sys_don_hang_mua.ctr/getElementById/', {
                id: this.record.db.id_don_hang_mua,
            }
            ).subscribe(resp => {
                var data: any;
                data = resp;

                if (data.list_mat_hang.length == 0) {

                    Swal.fire(this._translocoService.translate('erp.khong_co_mat_hang'), "", "warning");

                } else {
                    this.load_list_choose_mat_hang_don_hang(data.list_mat_hang);
                }
            });
    }
    get_list_mat_hang_theo_don_hang_ban() {
        this.http
            .post('/sys_don_hang_ban.ctr/getElementById/', {
                id: this.record.db.id_don_hang_ban,
            }
            ).subscribe(resp => {
                var data: any;
                data = resp;
                if (data.list_mat_hang.length == 0) {
                    Swal.fire(this._translocoService.translate('erp.khong_co_mat_hang'), "", "warning");
                } else {
                    this.load_list_choose_mat_hang_don_hang(data.list_mat_hang);
                }
            });
    }
    change_loai_nhap(): void {
        this.record.db.nguon = this.list_loai_nhap.filter(q => q.id == this.record.db.id_loai_nhap)[0].nguon;
        this.record.ma_don_hang = null;
        this.record.db.id_don_hang_ban = null;
        this.record.db.id_don_hang_mua = null;
        this.record.list_mat_hang = [];
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
    // openDialogChooseDonHang(id, type): void {

    //     //type == 1 nhap mua
    //     //type == 2 nhap tra hang
    //     if (type == 2) {
    //         const dialogRef = this.dialogModal
    //             .open(sys_common_popupChooseDonHangMuaHHComponent, {
    //                 disableClose: true,
    //                 width: '100%',
    //                 height: '100%',
    //                 data: {
    //                     db: {
    //                         id: id,
    //                         nguon: type + "pn"
    //                     },
    //                 }
    //             });
    //         dialogRef.afterClosed().subscribe(result => {

    //             var data: any;
    //             this.data_doi_tuong = result;
    //             data = result;
    //             if (data == undefined) return;
    //             this.record.db.nguon = 2;
    //             this.record.db.id_don_hang_mua = data.id;
    //             this.record.ma_don_hang = data.ma;
    //             this.record.ghi_chu_don_hang = data.ghi_chu;
    //             this.record.loai_giao_dich = data.loai_giao_dich;
    //             this.record.list_mat_hang = [];

    //             this.record.db.hinh_thuc_doi_tuong = data.hinh_thuc_doi_tuong;
    //             this.record.db.id_doi_tuong = data.id_doi_tuong;
    //             this.record.db.ma_so_thue = data.ma_so_thue;
    //             this.record.db.dien_thoai = data.dien_thoai;
    //             this.record.db.email = data.email;
    //             this.record.db.dia_chi_doi_tuong = data.dia_chi_doi_tuong;
    //             this.record.db.ten_doi_tuong = data.ten_doi_tuong;
    //             if (data.id_doi_tuong == "DTTD")
    //                 this.record.check_doi_tuong = 1;
    //             else
    //                 this.record.check_doi_tuong = 2;

    //             this.get_list_mat_hang_theo_don_hang_mua();



    //         })

    //     } else {
    //         const dialogRef = this.dialogModal
    //             .open(sys_common_popupChooseDonHangBanHHComponent, {
    //                 disableClose: true,
    //                 width: '100%',
    //                 height: '100%',
    //                 data: {
    //                     db: {
    //                         id: id,
    //                         nguon: type + "pn"
    //                     },

    //                 }
    //             });
    //         dialogRef.afterClosed().subscribe(result => {

    //             var data: any;
    //             this.data_doi_tuong = result;
    //             data = result;
    //             if (data == undefined) return;
    //             this.record.db.nguon = 1;
    //             this.record.db.id_don_hang_ban = data.id;
    //             this.record.ma_don_hang = data.ma;
    //             this.record.ghi_chu_don_hang = data.ghi_chu;
    //             this.record.loai_giao_dich = data.loai_giao_dich;
    //             this.record.list_mat_hang = [];

    //             this.record.db.hinh_thuc_doi_tuong = data.hinh_thuc_doi_tuong;
    //             this.record.db.id_doi_tuong = data.id_doi_tuong;
    //             this.record.db.ma_so_thue = data.ma_so_thue;
    //             this.record.db.dien_thoai = data.dien_thoai;
    //             this.record.db.email = data.email;
    //             this.record.db.dia_chi_doi_tuong = data.dia_chi_doi_tuong;
    //             this.record.db.ten_doi_tuong = data.ten_doi_tuong;
    //             if (data.id_doi_tuong == "DTTD")
    //                 this.record.check_doi_tuong = 1;
    //             else
    //                 this.record.check_doi_tuong = 2;
    //             this.get_list_mat_hang_theo_don_hang_ban();
    //         })
    //     }
    // }
    load_list_mat_hang_nhap_kho(): void {

        this.http
            .post('/sys_phieu_nhap_kho.ctr/getElementById/',
                {
                    id: this.record.db.id,

                }
            ).subscribe(resp => {
                var data: any = resp;
                this.record = data;
                // this.record.list_mat_hang.forEach(q => {
                //     q.id_deatils_nhap_kho = q.db.id
                // });
                this.record.db.id_don_hang_ban = data.db.id_don_hang_ban;
                this.record.db.id_don_hang_mua = data.db.id_don_hang_mua;
                this.record.check_doi_tuong = doi_tuong_tu_do.id == this.record.db.id_doi_tuong ? 1 : 2;
                if(this.actionEnum == 2)
                {
                    this.baseInitData();
                }
            });
    }
    go_to_print_phieu_nhap_kho(id): void {
        var host = window.location.hostname;
        var link = "";
        if (host == "localhost") {
            link = "https://" + host + ":44324/sys_in_phieu_nhap_kho.ctr/in_phieu_nhap_kho?id=" + id;
        } else {
            link = "https://" + host + '/sys_in_phieu_nhap_kho.ctr/in_phieu_nhap_kho?id=' + id;
        }
        window.open(link);
    }
    load_nguon(): void {
        this.list_nguon = [
            {
                id: 1,
                name: this._translocoService.translate('erp.khac')
            },
            // {
            //     id: 2,
            //     name: this._translocoService.translate('erp.thu_tien_tu_don_hang_ban')
            // },
            // {
            //     id: 3,
            //     name: this._translocoService.translate('erp.nhan_hoan_tien_tu_don_hang_mua')
            // }
        ];
    }
    public get_list_loai_nhap(): void {
        this.http
            .post('/sys_loai_nhap_xuat.ctr/getListUse/', {
            }
            ).subscribe(resp => {
                if (this.actionEnum == 1) {
                    this.list_loai_nhap = resp;
                    this.list_loai_nhap = this.list_loai_nhap.filter(q => q.loai == '1' && q.ma != "NCK");
                    this.record.db.id_loai_nhap = this.list_loai_nhap.filter(q => q.ma == "NGT")[0].id;
                    this.record.ma_loai_nhap = this.list_loai_nhap.filter(q => q.ma == "NGT")[0].ma;
                    this.record.nguon = this.list_loai_nhap.filter(q => q.ma == "NGT")[0].nguon;
                }
                if (this.actionEnum != 1) {
                    this.list_loai_nhap = resp;
                    this.list_loai_nhap = this.list_loai_nhap.filter(q => q.loai == '1');
                    this.record.db.nguon = this.record.db.nguon;
                    this.record.db.id_loai_nhap = this.record.db.id_loai_nhap;
                }
            });
    }
    public get_list_don_hang(): void {
        this.http
            .post('/sys_don_hang_ban.ctr/getListUse/', {
            }
            ).subscribe(resp => {
                this.list_don_hang = resp;
            });
    }
    public load_don_hang_ban(): void {
        this.http
            .post('/sys_don_hang_ban.ctr/getListUse/', {
                //id_cong_ty: this.record.db.id_cong_ty
            }
            ).subscribe(resp => {
                this.list_don_hang_ban = resp;
                this.id_don_hang = this.list_don_hang_ban[0].id;
            });


    }
    get_list_kho(): void {
        this.http
            .post('/sys_kho.ctr/getListUse/', {
            }
            ).subscribe(resp => {
                this.list_kho = resp;
            });
    }
    get_code() {
        this.http
            .post('/sys_phieu_nhap_kho.ctr/get_code/', {
            }
            ).subscribe(resp => {
                this.record.db.ma = "Tự động tạo";
            });
        //this.record.db.ma = this.controller + "-" + this.Progress_logo

    }



    public getListDetail(): void {
        this.http
            .post('/sys_phieu_nhap_kho.ctr/getListDetail/', {
                id: this.record.db.id
            }
            ).subscribe(resp => {
                //this.list_mat_hang = resp;
                var data: any;
                data = resp;
                // this.record.list_mat_hang = resp as Array<qlmv_mat_hang_dich_vu_ref_model>;

                this.record.list_mat_hang.push(data);
                this.list_mat_hang.shift();
                this.list_mat_hang = this.list_mat_hang.filter(q => q.id != data.id);
            });
    }
    // openDialogChooseMatHang(): void {
    //     this.record.db.loai_giao_dich = 1;
    //     const dialogRef = this.dialogModal
    //         .open(sys_common_popupChooseMatHangComponent, {
    //             disableClose: true,
    //             width: '95%',
    //             height: '95%',
    //             data: this.record
    //         });
    //     dialogRef.afterClosed().subscribe(result => {
    //         var data: any;
    //         data = result;
    //         if (data == undefined) return;

    //         this.record.list_mat_hang = data;
    //     })
    // }
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
    
    combinedCode = ""
    startFrom = new Date().getTime();
    @HostListener('document:keypress', ['$event'])
    keyEvent(event: KeyboardEvent) {
        if (event.key === 'Enter') {
            if (this.combinedCode != "" && this.combinedCode.length > 6 && (this.ma_mat_hang ?? "") == "") {
                this.ma_mat_hang = this.combinedCode;
                this.add_mat_hang()
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
    add_mat_hang() {
        var ma_mat_hang = this.ma_mat_hang + "";
        this.ma_mat_hang = "";
        var findIndexExist = this.record.list_mat_hang.findIndex(d => d.db.id_mat_hang == ma_mat_hang.trim() || d.db.ma_vach == ma_mat_hang.trim());
        if (findIndexExist >= 0) {
            // this.record.list_mat_hang[findIndexExist].db.so_luong++;
            this.record.list_mat_hang[findIndexExist].db.so_luong = Number(this.record.list_mat_hang[findIndexExist].db.so_luong) + Number(this.record.so_luong_mh);
            return;
        } else {
            if (this.record.so_luong_mh <= 0 || this.record.so_luong_mh == null) {
                Swal.fire("Số lượng phải lớn hơn 0", "", "warning");
            }
            else {
                this.http
                    .post('/sys_mat_hang.ctr/add_mat_hang/', {
                        ma: ma_mat_hang.trim(),
                        loai_giao_dich: "1",
                        id_doi_tuong: this.record.db.id_doi_tuong ?? "",
                        list_mat_hang: this.record.list_mat_hang
                    }
                    ).subscribe(resp => {
                        var data: any;
                        data = resp;

                        if (data.list_mat_hang.length == 0) {
                            if (data.result == 1) {
                                Swal.fire("mã " + ma_mat_hang + ", " + this._translocoService.translate('erp.mat_hang_khong_ton_tai_trong_he_thong'), "", "warning");
                            }
                            else if (data.result == 2) {
                                Swal.fire(this._translocoService.translate('erp.mat_hang_phai_la_hang_hoa'), "", "warning");
                            } else if (data.result == 3) {
                                Swal.fire(this._translocoService.translate('erp.mat_hang_phai_la_dich_vu'), "", "warning");
                            } else {
                                Swal.fire(this._translocoService.translate('erp.mat_hang_da_ngung_su_dung'), "", "warning");
                            }

                        } else {
                            this.load_list_choose(data.list_mat_hang);

                            //this.record.list_mat_hang.push(data.list_mat_hang);
                        }

                    });
            }

        }
    }
    load_list_choose(listDataN): any {
        debugger
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
                        don_gia: don_gia,
                        so_luong: this.record.so_luong_mh,
                        vat: model.db.vat,
                        thanh_tien_truoc_thue: 0,
                        tien_vat: 0,
                        ghi_chu: model.db.ghi_chu,
                        thanh_tien_sau_thue: 0,

                    },
                    ma_mat_hang: model.db.ma,
                    ten_mat_hang: model.db.ten,
                    ten_don_vi_tinh: model.ten_don_vi_tinh,
                    ten_thuoc_tinh: model.ten_thuoc_tinh,
                    id_mat_hang: model.db.id
                    //ten_thuoc_tinh: model.ten_thuoc_tinh

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
    chose_file_logo(fileInput: any) {

        this.file_logo = fileInput.target.files;
        fileInput.target.value = null;
    }
}
