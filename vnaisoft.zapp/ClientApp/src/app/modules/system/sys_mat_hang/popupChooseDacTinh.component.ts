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
    selector: 'sys_mat_hang_popupChooseDacTinh',
    templateUrl: 'popupChooseDacTinh.html',
})
export class sys_mat_hang_popupChooseDacTinhComponent extends BasePopupDatatabbleComponent {
    public check_all_mat_hang: any;
    public list_loai_mat_hang: any;
    public actionEnum: any;
    public loading: any;
    public list_choose: any;
    public record: any;
    public ignore_ids: any;
    public list_thuoc_tinh: any;

    constructor(
        _translocoService: TranslocoService
        , _fuseNavigationService: FuseNavigationService
        , route: ActivatedRoute
        , @Inject('BASE_URL') baseUrl: string
        , http: HttpClient

        , public dialogRef: MatDialogRef<MatDialogRef<sys_mat_hang_popupChooseDacTinhComponent>>
        , dialogModal: MatDialog

        , @Inject(MAT_DIALOG_DATA) data: any
    ) {
        super(_translocoService, _fuseNavigationService, route, baseUrl, http, "erp_dac_tinh_mat_hang", dialogRef, dialogModal,
            {
                search: "", status_del: "1", ignore_ids: "", loai_giao_dich: "", kieu_ban: "", id_doi_tuong: ""
            }
        )
        this.record = data;
        if (this.record.db.list_dac_tinh == undefined) {
            this.record.db.list_dac_tinh = []
        }

        if (this.record.db.list_dac_tinh.length == 0) {
            this.list_choose = "";
        } else {
            this.list_choose = "";
            this.ignore_ids = ""
            for (let i = 0; i < this.record.db.list_dac_tinh.length; i++) {
                let model = this.record.db.list_dac_tinh[i];
                let str = "";
                str = model.name + ";";
                this.list_choose += str;
                this.ignore_ids += model.id + ",";
            }
            this.filter.ignore_ids = this.ignore_ids;
        }
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
                if (model.is_check == true) {
                    str = model.db.id + "" + model.db.ten + ";";
                    this.list_choose += str;
                    let obj_dac_tinh = {
                        id: model.db.id,
                        name: model.db.ten,
                        code: model.db.loai_dac_tinh,
                        list_dac_tinh_mat_hang: model.list_dac_tinh_mat_hang,
                    }
                    this.record.db.list_dac_tinh.push(obj_dac_tinh);
                }
            }
        }
    }
    chon() {
        this.load_list_choose(this.listData);
        var valid = true;
        var error = '';
        if (this.record.db.list_dac_tinh.length == 0 || this.record.db.list_dac_tinh == undefined) {
            error += this._translocoService.translate('erp.vuilongchonitnhat1dactinh') + '<br>';
            valid = false;
        }
        if (!valid) {
            this.showMessagewarning(error);
            return;
        }
        this.dialogRef.close(this.record.db.list_dac_tinh);
    }
    dong() {
        if (this.record.db.list_dac_tinh.length == 0) {
            this.dialogRef.close();
        } else {
            this.dialogRef.close(this.record.db.list_dac_tinh);
        }
    }
    update_all_mat_hang() {
        this.check_all_mat_hang = this.listData != null && this.listData.every(t => t.is_check);
    }


    ngOnInit(): void {
        this.check_all_mat_hang = false;
        this.baseInitDataTable();
    }
}
