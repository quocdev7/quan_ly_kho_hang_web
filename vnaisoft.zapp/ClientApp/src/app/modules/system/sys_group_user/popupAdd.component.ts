import { Component, Inject } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef, MatDialog } from '@angular/material/dialog';

import { HttpClient } from '@angular/common/http';
import { BasePopUpAddComponent } from 'app/Basecomponent/BasePopupAdd.component';
import { TranslocoService } from '@ngneat/transloco';
import { FuseNavigationService } from '@fuse/components/navigation';
import { ActivatedRoute } from '@angular/router';
import { translateDataTable } from '@fuse/components/commonComponent/VietNameseDataTable';
import Swal from 'sweetalert2';

@Component({
  selector: 'sys_group_user_popUpAdd',
  templateUrl: 'popupAdd.html',
  styleUrls: ['./popupAdd.component.scss'],
})
export class sys_group_user_popUpAddComponent extends BasePopUpAddComponent {
  public additem: any;
  public additemrole: any;
  public item_chose: any;
  public dtOptions: any;
  public searchRole: any = '';
  public searchUser: any = '';
  public listRoleFilter: any;
  public search_module: any;

  public list_module: any;

  public expandedCategories: { [key: number]: boolean } = {
    1: true, // Initially expanded
    2: true,
    3: true,
    4: true,
  };

  public expandedCategoriesUser: { [key: number]: boolean } = {
    1: true, // Initially expanded
    2: true,
    3: true,
    4: true,
  };

  public expanded_module: any;
  public is_check_module: any;
  constructor(
    public dialogRef: MatDialogRef<sys_group_user_popUpAddComponent>,
    http: HttpClient,
    _translocoService: TranslocoService,
    _fuseNavigationService: FuseNavigationService,
    route: ActivatedRoute,
    @Inject('BASE_URL') baseUrl: string,
    public dialogModal: MatDialog,
    @Inject(MAT_DIALOG_DATA) data: any,
  ) {
    super(_translocoService, _fuseNavigationService, route, baseUrl, http, 'sys_group_user', dialogRef, dialogModal);
    this.record = data;
    this.Oldrecord = JSON.parse(JSON.stringify(data));
    this.resetAddItem();
    this.resetAddItemRole();

    this.actionEnum = data.actionEnum;

    if (this.actionEnum == 1) {
    }

    this.http
      .post('/sys_group_user.ctr/getListItem/', {
        id: this.record.db.id,
      })
      .subscribe((resp) => {
        this.record.list_item = resp;
      });
    this.http
      .post('/sys_group_user.ctr/getListRole/', {
        id: this.record.db.id,
      })
      .subscribe((resp) => {
        this.record.list_role = resp;

        this.http.post('/sys_group_user.ctr/getListRoleFull/', {}).subscribe((resp) => {
          this.listRoleFilter = resp;
          this.resetlistRole();
        });
      });

    this.dtOptions = {
      language: translateDataTable,
      scrollY: '50vh',
      scrollCollapse: true,
      scrollX: true,
      ordering: false,
      searching: false,
      paging: false,
    };
    if (this.actionEnum != 2) {
      this.dialogRef.keydownEvents().subscribe((event) => {
        if (event.key === 'Escape') {
          //this.onCancel();
          this.dialogRef.close();
        }
      });
    }
  }

  resetAddItem(): void {
    this.additem = {
      db: {
        user_id: null,
        note: '',
      },
      user_name: '',
    };
  }
  resetAddItemRole(): void {
    this.additemrole = {
      db: {
        id_controller_role: '',
        controller_name: '',
        role_name: '',
      },
    };
  }
  beforesave(): void {
    this.record.list_role = [];

    if (this.listRoleFilter != undefined) {
      for (var i = 0; i < this.listRoleFilter.length; i++) {
        var d = this.listRoleFilter[i];
        if (this.listRoleFilter[i].completed == true) {
          this.record.list_role.push({
            db: {
              id_controller_role: d.role.id,
              controller_name: d.controller_name,
              role_name: d.role.name,
            },
          });
        } else {
        }
      }
    }
  }
  aftersavefail(): void {}
  bind_data_item_chose(): void {
    this.additem.db.user_id = this.item_chose.id;
    this.additem.user_name = this.item_chose.name;
  }

  addDetail(): void {
    var valid = true;
    var error = '';

    if (this.record.list_item.filter((d) => d.db.user_id == this.additem.db.user_id).length > 0) {
      error += this._translocoService.translate('existed') + '<br>';
      valid = false;
    }

    if (this.additem.db.user_id == null || this.additem.db.user_id == undefined) {
      error += this._translocoService.translate('must_chose_item') + '<br>';
      valid = false;
    }

    if (!valid) {
      this.showMessagewarning(error);
      return;
    }
    this.record.list_item.push(this.additem);
    this.record.list_item.sort(function (a, b) {
      return a.db.step_num - b.db.step_num;
    });

    this.resetAddItem();
  }
  resetlistRole(): void {
    for (var i = 0; i < this.listRoleFilter.length; i++) {
      this.listRoleFilter[i].name =
        this._translocoService.translate(this.listRoleFilter[i].controller_name) +
        ' ' +
        this._translocoService.translate(this.listRoleFilter[i].role.name);
      if (this.record.list_role.filter((d) => d.db.id_controller_role == this.listRoleFilter[i].role.id).length > 0) {
        this.listRoleFilter[i].completed = true;
      } else {
        this.listRoleFilter[i].completed = false;
      }
    }
    this.updateAllComplete();
  }

  deleteDetail(pos): void {
    this.record.list_item.splice(pos, 1);
    this.resetlistRole();
  }

  allCompleteUser: boolean = false;

  updateAllCompleteUser() {
    this.allCompleteUser = this.record.list_item != null && this.record.list_item.every((t) => t.isCheck);
  }

  someCompleteUser(): boolean {
    if (this.record.list_item == null) {
      return false;
    }
    return this.record.list_item.filter((t) => t.isCheck).length > 0 && !this.allCompleteUser;
  }

  setAllUser(completed: boolean) {
    this.allCompleteUser = completed;
    if (this.record.list_item == null) {
      return;
    }
    this.record.list_item.forEach((t) => (t.isCheck = completed));
  }

  allComplete: boolean = false;

  updateAllComplete() {
    this.allComplete = this.listRoleFilter != null && this.listRoleFilter.every((t) => t.completed);
  }

  someComplete(): boolean {
    if (this.listRoleFilter == null) {
      return false;
    }
    return this.listRoleFilter.filter((t) => t.completed).length > 0 && !this.allComplete;
  }

  setAll(completed: boolean) {
    this.allComplete = completed;
    if (this.listRoleFilter == null) {
      return;
    }
    this.listRoleFilter.forEach((t) => (t.completed = completed));
  }
}
