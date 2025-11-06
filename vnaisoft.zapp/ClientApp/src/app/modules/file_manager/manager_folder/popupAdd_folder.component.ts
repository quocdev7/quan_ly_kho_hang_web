import { Component, Inject } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef, MatDialog } from '@angular/material/dialog';


import { HttpClient } from '@angular/common/http';
import { BasePopUpAddComponent } from 'app/Basecomponent/BasePopupAdd.component';
import { TranslocoService } from '@ngneat/transloco';
import { FuseNavigationService } from '@fuse/components/navigation';
import { ActivatedRoute } from '@angular/router';
import { BasePopUpAddTypeComponent } from 'app/Basecomponent/BasePopupAddType.component';
import Swal from 'sweetalert2';


@Component({
    selector: 'file_manager_popupAdd_folder',
    templateUrl: 'popupAdd_folder.component.html',
    styleUrls: ['./popupAdd_folder.component.scss']

})
export class file_manager_popupAdd_folderComponent extends BasePopUpAddComponent {
    public plugintiny = [
        "advlist autolink lists link image charmap print preview anchor",
        "searchreplace visualblocks code fullscreen",
        "insertdatetime media table paste imagetools wordcount"
    ];
    public toolbartiny = "insertfile undo redo | styleselect | bold italic | alignleft aligncenter alignright alignjustify | bullist numlist outdent indent | link image";

    public timyconfig = {
        base_url: '/tinymce'
        , suffix: '.min',
        height: 500,
        images_upload_url: '/FileManager/uploadimagenew',
        plugins: this.plugintiny,
        toolbar: this.toolbartiny
    }
    public list_phong_ban: any;
    public list_chuc_danh: any;
    public openTab: any;
    public lst_folder: any = [];

    constructor(public dialogRef: MatDialogRef<file_manager_popupAdd_folderComponent>,
        http: HttpClient, _translocoService: TranslocoService,
        _fuseNavigationService: FuseNavigationService, route: ActivatedRoute,
        @Inject('BASE_URL') baseUrl: string,
        public dialogModal: MatDialog,
        @Inject(MAT_DIALOG_DATA) data: any) {
        super(_translocoService, _fuseNavigationService, route, baseUrl, http, 'storage_file_manager', dialogRef, dialogModal);

        this.record = data;
        this.Oldrecord = JSON.parse(JSON.stringify(data));
        this.actionEnum = data.actionEnum;
        if (this.actionEnum == 1) {
            this.baseInitData();
        }
        this.openTab = 1;
    }
    toggleTabs($tabNumber: number) {
        this.openTab = $tabNumber;
    }
    get_list_folder() {
        this.http
            .post('storage_file_manager.ctr/get_list_folder/',
                {}
            ).subscribe(resp => {
                this.lst_folder = resp;
            });
    }
    save_folder(): void {
        this.beforesave();
        this.loading = true;
        if (this.actionEnum == 1) {
            this.http
                .post('storage_file_manager.ctr/create_folder/',
                    {
                        data: this.record,
                    }
                ).subscribe(resp => {
                    this.record = resp;
                    this.Oldrecord = this.record;
                    this.basedialogRef.close(this.record);
                    Swal.fire('Lưu thành công', '', 'success');
                    this.aftersave();
                },
                    error => {
                        if (error.status == 400) {
                            this.errorModel = error.error;
                            this.aftersavefail();
                        }
                        if (error.status == 403) {
                            this.basedialogRef.close();
                            Swal.fire(this._translocoService.translate('no_permission'), "", "warning");
                        }
                        this.loading = false;

                    });
        }
        if (this.actionEnum == 2) {
            this.http
                .post('storage_file_manager.ctr/edit_folder/',
                    {
                        data: this.record,
                    }
                ).subscribe(resp => {
                    this.record = resp;
                    this.Oldrecord = this.record;
                    this.basedialogRef.close(this.record);
                    Swal.fire('Lưu thành công', '', 'success');
                    this.aftersave();
                },
                    error => {
                        if (error.status == 400) {
                            this.errorModel = error.error;
                            this.aftersavefail();
                        }
                        if (error.status == 403) {
                            this.basedialogRef.close();
                            Swal.fire(this._translocoService.translate('no_permission'), "", "warning");
                        }
                        this.loading = false;

                    });
        }
    }
    ngOnInit(): void {
        this.get_list_folder();
    }
}
