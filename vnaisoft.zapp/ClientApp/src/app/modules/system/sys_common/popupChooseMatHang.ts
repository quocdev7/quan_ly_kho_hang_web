import { Component, Inject } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef, MatDialog } from '@angular/material/dialog';


import { HttpClient } from '@angular/common/http';
import { BasePopUpAddComponent } from 'app/Basecomponent/BasePopupAdd.component';
import { TranslocoService } from '@ngneat/transloco';
import { FuseNavigationService } from '@fuse/components/navigation';
import { ActivatedRoute } from '@angular/router';


import { BasePopupDatatabbleComponent } from 'app/Basecomponent/BasePopupDatatabble.component';
import { list_thuoc_tinh } from 'app/core/data/data';


@Component({
    selector: 'sys_common_popupChooseMatHang',
    templateUrl: 'popupChooseMatHang.component.html',
})
export class sys_common_popupChooseMatHangComponent extends BasePopupDatatabbleComponent {
    public check_all_mat_hang: any;
    public list_loai_mat_hang: any;
    public actionEnum: any;
    public loading: any;
    public list_choose: any;
    public record: any;
    public ignore_ids: any;
    public list_thuoc_tinh: any;
    public list_kho: any;
    constructor(
        _translocoService: TranslocoService
        , _fuseNavigationService: FuseNavigationService
        , route: ActivatedRoute
        , @Inject('BASE_URL') baseUrl: string
        , http: HttpClient

        , public dialogRef: MatDialogRef<MatDialogRef<sys_common_popupChooseMatHangComponent>>
        , dialogModal: MatDialog

        , @Inject(MAT_DIALOG_DATA) data: any
    ) {
        super(_translocoService, _fuseNavigationService, route, baseUrl, http, "erp_mat_hang", dialogRef, dialogModal,
            {
                search: "", status_del: "1", id_loai_mat_hang: "-1", ignore_ids: "", id_thuoc_tinh: "-1", id_kho: "-1", loai_giao_dich: "", kieu_ban: "", id_doi_tuong: ""
            }
        )


        this.record = data;
        this.filter.id_doi_tuong = this.record.db.id_doi_tuong;
        //this.actionEnum = data.actionEnum;  

        // if (this.record.list_mat_hang == undefined) {
        //     this.record.list_mat_hang = []
        // }

        // if (this.record.list_mat_hang.length == 0) {
        //     this.list_choose = "";
        // } else {
        //     this.list_choose = "";
        //     this.ignore_ids = ""
        //     for (let i = 0; i < this.record.list_mat_hang.length; i++) {
        //         let model = this.record.list_mat_hang[i];
        //         let str = "";
        //         str = model.ma_mat_hang + "(" + model.ten_mat_hang + ")" + ";";
        //         this.list_choose += str;
        //         this.ignore_ids += model.db.id_mat_hang + ",";
        //     }
        //     this.filter.ignore_ids = this.ignore_ids;
        // }
        this.get_list_loai_mat_hang();
        this.get_list_thuoc_tinh();
        this.get_list_kho();
        if (this.record.db.loai_giao_dich != null && this.record.db.loai_giao_dich != "") {
            this.filter.loai_giao_dich = this.record.db.loai_giao_dich;

        }
        if (this.record.db.kieu_ban != null && this.record.db.kieu_ban != "") {
            this.filter.kieu_ban = this.record.db.kieu_ban;

        }

    }
    get_list_thuoc_tinh(): void {
        if (this.record.db.loai_giao_dich == 1) {
            this.list_thuoc_tinh = list_thuoc_tinh.filter(q => q.id != 6)
        } else {
            this.list_thuoc_tinh = list_thuoc_tinh.filter(q => q.id == 6)
        }
        this.list_thuoc_tinh.splice(0, 0, { id: '-1', name: this._translocoService.translate('system.all') })

    }
    get_list_loai_mat_hang(): void {
        this.http
            .post('/erp_loai_mat_hang.ctr/getListUse/', {}
            ).subscribe(resp => {
                this.list_loai_mat_hang = resp;
                this.list_loai_mat_hang.splice(0, 0, { id: '-1', name: this._translocoService.translate('system.all') })
            });
    }
    get_list_kho(): void {
        this.http
            .post('/erp_kho.ctr/getListUse/', {}
            ).subscribe(resp => {
                this.list_kho = resp;
                this.list_kho.splice(0, 0, { id: '-1', name: this._translocoService.translate('system.all') })
            });
    }
    set_mat_hang(): boolean {
        if (this.listData == null) {
            return false;
        }
        return this.listData.filter(t => t.is_check).length > 0 && !this.check_all_mat_hang;

    }

    set_all_mat_hang(completed: boolean) {
        this.check_all_mat_hang = completed;
        if (this.listData == null) {
            return;
        }
        this.listData.forEach(t => t.is_check = completed);
    }

    load_list_choose(listDataN): any {
        if (listDataN.length == 0) {
            //this.list_choose = "";
        }
        else {
            for (let i = 0; i < listDataN.length; i++) {
                let model = listDataN[i];
                let str = "";
                var don_gia = null;
                if (model.is_check == true) {
                    str = model.db.id + "" + model.db.ma + "(" + model.db.ten + ")" + ";";
                    this.list_choose += str;

                    if (this.record.db.kieu_ban != null) {
                        if (this.record.db.kieu_ban == 1) {
                            don_gia = model.db.gia_ban_si;
                        } else {
                            don_gia = model.db.gia_ban_le;
                        }
                    } else {
                        don_gia = model.gia_mua;
                    }



                    let obj_mat_hang = {
                        db: {
                            id_mat_hang: model.db.id,
                            id_don_vi_tinh: model.db.id_don_vi_tinh,
                            ma_vach: model.db.ma_vach,
                            don_gia: don_gia,
                            chiet_khau: model.db.ty_le_chiet_khau,
                            so_luong: 0,
                            vat: model.db.vat,
                            thanh_tien_truoc_thue: 0,
                            tien_vat: 0,
                            ghi_chu: null,
                            thanh_tien_sau_thue: 0,
                        },
                        ma_mat_hang: model.db.ma,
                        ten_mat_hang: model.db.ten,
                        ten_don_vi_tinh: model.ten_don_vi_tinh,
                        ten_thuoc_tinh: model.ten_thuoc_tinh,
                        thuoc_tinh: model.db.thuoc_tinh,
                        ton_kho: model.ton_kho,
                        he_so_quy_doi: model.db.he_so_quy_doi,
                    }


                    this.record.list_mat_hang.push(obj_mat_hang);
                }

            }
        }

    }

    chon() {


        this.load_list_choose(this.listData);

        var valid = true;
        var error = '';
        if (this.record.list_mat_hang.length == 0 || this.record.list_mat_hang == undefined) {
            error += this._translocoService.translate('erp.vuilongchonitnhat1mathang') + '<br>';
            valid = false;
        }

        if (!valid) {
            this.showMessagewarning(error);
            return;
        }

        this.dialogRef.close(this.record.list_mat_hang);


    }
    dong() {
        if (this.record.list_mat_hang.length == 0) {
            this.dialogRef.close();
        } else {
            this.dialogRef.close(this.record.list_mat_hang);
        }
    }
    update_all_mat_hang() {
        this.check_all_mat_hang = this.listData != null && this.listData.every(t => t.is_check);
    }


    ngOnInit(): void {
        this.check_all_mat_hang = false;
        this.baseInitDataOption("DataHandlerTonKho");

    }
}
