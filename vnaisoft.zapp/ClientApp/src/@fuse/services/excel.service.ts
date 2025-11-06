import { DatePipe } from '@angular/common';
import { Injectable } from '@angular/core';
import { TranslocoService } from '@ngneat/transloco';
import { Workbook } from 'exceljs';
import * as fs from 'file-saver';
import * as logoFile from './logo_thaco';


@Injectable({
    providedIn: 'root'
})
export class ExcelService {


    constructor(public datePipe: DatePipe,public _translocoService: TranslocoService) {

    }

    generateExcel(data: Array<any> = [], name_component: String) {

        console.log(name_component)
        switch (name_component) {
            case "inventory_report_import_export": {
                this.inventory_report_import_exportComponent(data)
            }
                break;
            case "inventory_report_import_export_specification": {
                this.inventory_report_import_export_specification(data)
                break;
            }
            case "inventory_position_report_import_export": {
                this.inventory_position_report_import_export(data)
                break;
            }
            case "inventory_position_report_import_export_specification": {
                this.inventory_position_report_import_export_specification(data)
                break;
            }
            case "inventory_report_min_max_stock": {
                this.inventory_report_min_max_stock(data)
                break;
            }
            case "maintenance_report_system": {
                this.maintenance_report_system(data)
                break;
            }
            case "maintenance_report_system_device_detail": {
                this.maintenance_report_system_device_detail(data)
                break;
            }
            default: {
                //statements;
                break;
            }
        }
    }
    inventory_report_min_max_stock(data: any[]) {
        console.log(data)
        const title = this._translocoService.translate('NAV.inventory_report_min_max_stock').toUpperCase();
        const header = [this._translocoService.translate('no_'), this._translocoService.translate('item'),this._translocoService.translate('NAV.sys_item_specification'), this._translocoService.translate('NAV.sys_unit'),this._translocoService.translate('inventory.quantity_min_stock'), this._translocoService.translate('inventory.quantity_max_stock'), this._translocoService.translate('inventory.quantity_current'), this._translocoService.translate('inventory.quantity_purchase_expected'), this._translocoService.translate('NAV.status')]
        //Create workbook and worksheet
        let workbook = new Workbook();
        let worksheet = workbook.addWorksheet(this._translocoService.translate('NAV.inventory_report_min_max_stock'));
        //Add Row and formatting
        let titleRow = worksheet.addRow([title]);
        titleRow.alignment = { vertical: 'middle', horizontal: "center" }
        titleRow.font = { name: 'Times New Roman', family: 4, size: 16, bold: true }
        worksheet.addRow([]);
        //let subTitleRow = worksheet.addRow(['Date : ' + this.datePipe.transform(new Date(), 'medium')])
        //Add Image
        let logo = workbook.addImage({
            base64: logoFile.logoBase64,
            extension: 'png',
        });
        worksheet.properties.defaultRowHeight = 20;
        worksheet.addImage(logo, 'E1:F2');
        worksheet.mergeCells('A1:D2');
        //Blank Row
        worksheet.addRow([]);
        //Add Header Row
        let headerRow = worksheet.addRow(header);
        // Cell Style : Fill and Border
        headerRow.font = { name: 'Times New Roman', family: 4, size: 13, bold: true }
        headerRow.alignment = { vertical: 'middle', horizontal: "center" }
        headerRow.eachCell((cell, number) => {
            cell.fill = {
                type: 'pattern',
                pattern: 'solid',
                fgColor: { argb: 'FFFFFF00' },
                bgColor: { argb: 'FF0000FF' }
            }
            cell.border = { top: { style: 'thin' }, left: { style: 'thin' }, bottom: { style: 'thin' }, right: { style: 'thin' } }
        })
        // Add Data and Conditional Formatting
        data.forEach(d => {
            let row = worksheet.addRow(d);
            row.alignment = { vertical: 'middle', horizontal: "center" }
            row.eachCell((cell, number) => {
                // cell.fill = {
                //     type: 'pattern',
                //     pattern: 'solid',
                //     fgColor: { argb: 'FFFFFF00' },
                //     bgColor: { argb: 'FF0000FF' }
                // }
                cell.border = { top: { style: 'thin' }, left: { style: 'thin' }, bottom: { style: 'thin' }, right: { style: 'thin' } }
            })
            row.font = { name: 'Times New Roman', family: 4, size: 13, bold: false }
            let lastRemainingQuantity = row.getCell(9);
            let color = 'FF99FF99';
            if (lastRemainingQuantity.value < 0) {
                color = 'FF9999'
            }
            lastRemainingQuantity.fill = {
                type: 'pattern',
                pattern: 'solid',
                fgColor: { argb: color }
            }
        }
        );
        worksheet.getColumn(2).width = 40;
        worksheet.getColumn(3).width = 20;
        worksheet.getColumn(4).width = 20;
        worksheet.getColumn(5).width = 20;
        worksheet.getColumn(6).width = 20;
        worksheet.getColumn(7).width = 20;
        worksheet.getColumn(8).width = 20;

        worksheet.addRow([]);
        //Footer Row
        let footerRow = worksheet.addRow(['Date : ' + this.datePipe.transform(new Date(), 'medium')]);
        footerRow.getCell(1).fill = {
            type: 'pattern',
            pattern: 'solid',
            fgColor: { argb: 'FFCCFFE5' }
        };
        footerRow.getCell(1).border = { top: { style: 'thin' }, left: { style: 'thin' }, bottom: { style: 'thin' }, right: { style: 'thin' } }
        //Merge Cells
        worksheet.mergeCells(`A${footerRow.number}:B${footerRow.number}`);
        //Generate Excel File with given name
        workbook.xlsx.writeBuffer().then((data) => {
            let blob = new Blob([data], { type: 'application/vnd.openxmlformats-officedocument.spreadsheetml.sheet' });
            fs.saveAs(blob, `${title}.xlsx`);
        })
    }
    inventory_position_report_import_export_specification(data: any[]) {
        console.log(data)
        const title = this._translocoService.translate('NAV.inventory_position_report_import_export_specification').toUpperCase();
        const header = [this._translocoService.translate('no_'), this._translocoService.translate('item'),this._translocoService.translate('NAV.sys_item_specification'), this._translocoService.translate('NAV.sys_unit'),this._translocoService.translate('NAV.sys_position'), this._translocoService.translate('inventory.quantity_beginning_stocks'), this._translocoService.translate('inventory.quantity_import'), this._translocoService.translate('inventory.quantity_export'), this._translocoService.translate('inventory.quantity_ending_stocks')]
        //Create workbook and worksheet
        let workbook = new Workbook();
        let worksheet = workbook.addWorksheet(this._translocoService.translate('NAV.inventory_position_report_import_export_specification'));
        //Add Row and formatting
        let titleRow = worksheet.addRow([title]);
        titleRow.alignment = { vertical: 'middle', horizontal: "center" }
        titleRow.font = { name: 'Times New Roman', family: 4, size: 16, bold: true }
        worksheet.addRow([]);
        //let subTitleRow = worksheet.addRow(['Date : ' + this.datePipe.transform(new Date(), 'medium')])
        //Add Image
        let logo = workbook.addImage({
            base64: logoFile.logoBase64,
            extension: 'png',
        });
        worksheet.properties.defaultRowHeight = 20;
        worksheet.addImage(logo, 'E1:F2');
        worksheet.mergeCells('A1:D2');
        //Blank Row
        worksheet.addRow([]);
        //Add Header Row
        let headerRow = worksheet.addRow(header);
        // Cell Style : Fill and Border
        headerRow.font = { name: 'Times New Roman', family: 4, size: 13, bold: true }
        headerRow.alignment = { vertical: 'middle', horizontal: "center" }
        headerRow.eachCell((cell, number) => {
            cell.fill = {
                type: 'pattern',
                pattern: 'solid',
                fgColor: { argb: 'FFFFFF00' },
                bgColor: { argb: 'FF0000FF' }
            }
            cell.border = { top: { style: 'thin' }, left: { style: 'thin' }, bottom: { style: 'thin' }, right: { style: 'thin' } }
        })
        // Add Data and Conditional Formatting
        data.forEach(d => {
            let row = worksheet.addRow(d);
            row.alignment = { vertical: 'middle', horizontal: "center" }
            row.eachCell((cell, number) => {
                // cell.fill = {
                //     type: 'pattern',
                //     pattern: 'solid',
                //     fgColor: { argb: 'FFFFFF00' },
                //     bgColor: { argb: 'FF0000FF' }
                // }
                cell.border = { top: { style: 'thin' }, left: { style: 'thin' }, bottom: { style: 'thin' }, right: { style: 'thin' } }
            })
            row.font = { name: 'Times New Roman', family: 4, size: 13, bold: false }
            let lastRemainingQuantity = row.getCell(9);
            let color = 'FF99FF99';
            if (lastRemainingQuantity.value < 0) {
                color = 'FF9999'
            }
            lastRemainingQuantity.fill = {
                type: 'pattern',
                pattern: 'solid',
                fgColor: { argb: color }
            }
        }
        );
        worksheet.getColumn(2).width = 40;
        worksheet.getColumn(3).width = 40;
        worksheet.getColumn(4).width = 15;
        worksheet.getColumn(5).width = 15;
        worksheet.getColumn(6).width = 20;
        worksheet.getColumn(7).width = 20;
        worksheet.getColumn(8).width = 15;
        worksheet.getColumn(9).width = 17;
        worksheet.addRow([]);
        //Footer Row
        let footerRow = worksheet.addRow(['Date : ' + this.datePipe.transform(new Date(), 'medium')]);
        footerRow.getCell(1).fill = {
            type: 'pattern',
            pattern: 'solid',
            fgColor: { argb: 'FFCCFFE5' }
        };
        footerRow.getCell(1).border = { top: { style: 'thin' }, left: { style: 'thin' }, bottom: { style: 'thin' }, right: { style: 'thin' } }
        //Merge Cells
        worksheet.mergeCells(`A${footerRow.number}:B${footerRow.number}`);
        //Generate Excel File with given name
        workbook.xlsx.writeBuffer().then((data) => {
            let blob = new Blob([data], { type: 'application/vnd.openxmlformats-officedocument.spreadsheetml.sheet' });
            fs.saveAs(blob, `${title}.xlsx`);
        })
    }
    inventory_position_report_import_export(data: any[]) {
        console.log(data)
        const title = this._translocoService.translate('NAV.inventory_position_report_import_export').toUpperCase();
        const header = [this._translocoService.translate('no_'), this._translocoService.translate('item'), this._translocoService.translate('NAV.sys_unit'),this._translocoService.translate('NAV.sys_position'), this._translocoService.translate('inventory.quantity_beginning_stocks'), this._translocoService.translate('inventory.quantity_import'), this._translocoService.translate('inventory.quantity_export'), this._translocoService.translate('inventory.quantity_ending_stocks')]
        //Create workbook and worksheet
        let workbook = new Workbook();
        let worksheet = workbook.addWorksheet(this._translocoService.translate('NAV.inventory_position_report_import_export'));
        //Add Row and formatting
        let titleRow = worksheet.addRow([title]);
        titleRow.alignment = { vertical: 'middle', horizontal: "center" }
        titleRow.font = { name: 'Times New Roman', family: 4, size: 16, bold: true }
        worksheet.addRow([]);
        //let subTitleRow = worksheet.addRow(['Date : ' + this.datePipe.transform(new Date(), 'medium')])
        //Add Image
        let logo = workbook.addImage({
            base64: logoFile.logoBase64,
            extension: 'png',
        });
        worksheet.properties.defaultRowHeight = 20;
        worksheet.addImage(logo, 'E1:F2');
        worksheet.mergeCells('A1:D2');
        //Blank Row
        worksheet.addRow([]);
        //Add Header Row
        let headerRow = worksheet.addRow(header);
        // Cell Style : Fill and Border
        headerRow.font = { name: 'Times New Roman', family: 4, size: 13, bold: true }
        headerRow.alignment = { vertical: 'middle', horizontal: "center" }
        headerRow.eachCell((cell, number) => {
            cell.fill = {
                type: 'pattern',
                pattern: 'solid',
                fgColor: { argb: 'FFFFFF00' },
                bgColor: { argb: 'FF0000FF' }
            }
            cell.border = { top: { style: 'thin' }, left: { style: 'thin' }, bottom: { style: 'thin' }, right: { style: 'thin' } }
        })
        // Add Data and Conditional Formatting
        data.forEach(d => {
            let row = worksheet.addRow(d);
            row.alignment = { vertical: 'middle', horizontal: "center" }
            row.eachCell((cell, number) => {
                // cell.fill = {
                //     type: 'pattern',
                //     pattern: 'solid',
                //     fgColor: { argb: 'FFFFFF00' },
                //     bgColor: { argb: 'FF0000FF' }
                // }
                cell.border = { top: { style: 'thin' }, left: { style: 'thin' }, bottom: { style: 'thin' }, right: { style: 'thin' } }
            })
            row.font = { name: 'Times New Roman', family: 4, size: 13, bold: false }
            let lastRemainingQuantity = row.getCell(8);
            let color = 'FF99FF99';
            if (lastRemainingQuantity.value < 0) {
                color = 'FF9999'
            }
            lastRemainingQuantity.fill = {
                type: 'pattern',
                pattern: 'solid',
                fgColor: { argb: color }
            }
        }
        );
        worksheet.getColumn(2).width = 40;
        worksheet.getColumn(3).width = 15;
        worksheet.getColumn(4).width = 15;
        worksheet.getColumn(5).width = 15;
        worksheet.getColumn(6).width = 20;
        worksheet.getColumn(7).width = 20;
        worksheet.getColumn(8).width = 15;
        worksheet.addRow([]);
        //Footer Row
        let footerRow = worksheet.addRow(['Date : ' + this.datePipe.transform(new Date(), 'medium')]);
        footerRow.getCell(1).fill = {
            type: 'pattern',
            pattern: 'solid',
            fgColor: { argb: 'FFCCFFE5' }
        };
        footerRow.getCell(1).border = { top: { style: 'thin' }, left: { style: 'thin' }, bottom: { style: 'thin' }, right: { style: 'thin' } }
        //Merge Cells
        worksheet.mergeCells(`A${footerRow.number}:B${footerRow.number}`);
        //Generate Excel File with given name
        workbook.xlsx.writeBuffer().then((data) => {
            let blob = new Blob([data], { type: 'application/vnd.openxmlformats-officedocument.spreadsheetml.sheet' });
            fs.saveAs(blob, `${title}.xlsx`);
        })
    }
    inventory_report_import_export_specification(data: any[]) {
        console.log(data)
        const title = this._translocoService.translate('NAV.inventory_report_import_export_specification').toUpperCase();
        const header = [this._translocoService.translate('no_'), this._translocoService.translate('item'),this._translocoService.translate('NAV.sys_item_specification'), this._translocoService.translate('NAV.sys_unit'), this._translocoService.translate('inventory.quantity_beginning_stocks'), this._translocoService.translate('inventory.quantity_import'), this._translocoService.translate('inventory.quantity_export'), this._translocoService.translate('inventory.quantity_ending_stocks')]
        //Create workbook and worksheet
        let workbook = new Workbook();
        let worksheet = workbook.addWorksheet(this._translocoService.translate('NAV.inventory_report_import_export_specification'));
        //Add Row and formatting
        let titleRow = worksheet.addRow([title]);
        titleRow.alignment = { vertical: 'middle', horizontal: "center" }
        titleRow.font = { name: 'Times New Roman', family: 4, size: 16, bold: true }
        worksheet.addRow([]);
        //let subTitleRow = worksheet.addRow(['Date : ' + this.datePipe.transform(new Date(), 'medium')])
        //Add Image
        let logo = workbook.addImage({
            base64: logoFile.logoBase64,
            extension: 'png',
        });
        worksheet.properties.defaultRowHeight = 20;
        worksheet.addImage(logo, 'E1:F2');
        worksheet.mergeCells('A1:D2');
        //Blank Row
        worksheet.addRow([]);
        //Add Header Row
        let headerRow = worksheet.addRow(header);
        // Cell Style : Fill and Border
        headerRow.font = { name: 'Times New Roman', family: 4, size: 13, bold: true }
        headerRow.alignment = { vertical: 'middle', horizontal: "center" }
        headerRow.eachCell((cell, number) => {
            cell.fill = {
                type: 'pattern',
                pattern: 'solid',
                fgColor: { argb: 'FFFFFF00' },
                bgColor: { argb: 'FF0000FF' }
            }
            cell.border = { top: { style: 'thin' }, left: { style: 'thin' }, bottom: { style: 'thin' }, right: { style: 'thin' } }
        })
        // Add Data and Conditional Formatting
        data.forEach(d => {
            let row = worksheet.addRow(d);
            row.alignment = { vertical: 'middle', horizontal: "center" }
            row.eachCell((cell, number) => {
                // cell.fill = {
                //     type: 'pattern',
                //     pattern: 'solid',
                //     fgColor: { argb: 'FFFFFF00' },
                //     bgColor: { argb: 'FF0000FF' }
                // }
                cell.border = { top: { style: 'thin' }, left: { style: 'thin' }, bottom: { style: 'thin' }, right: { style: 'thin' } }
            })
            row.font = { name: 'Times New Roman', family: 4, size: 13, bold: false }
            let lastRemainingQuantity = row.getCell(8);
            let color = 'FF99FF99';
            if (lastRemainingQuantity.value < 0) {
                color = 'FF9999'
            }
            lastRemainingQuantity.fill = {
                type: 'pattern',
                pattern: 'solid',
                fgColor: { argb: color }
            }
        }
        );
        worksheet.getColumn(2).width = 40;
        worksheet.getColumn(3).width = 40;
        worksheet.getColumn(4).width = 15;
        worksheet.getColumn(5).width = 20;
        worksheet.getColumn(6).width = 20;
        worksheet.getColumn(7).width = 15;
        worksheet.getColumn(8).width = 15;
        worksheet.addRow([]);
        //Footer Row
        let footerRow = worksheet.addRow(['Date : ' + this.datePipe.transform(new Date(), 'medium')]);
        footerRow.getCell(1).fill = {
            type: 'pattern',
            pattern: 'solid',
            fgColor: { argb: 'FFCCFFE5' }
        };
        footerRow.getCell(1).border = { top: { style: 'thin' }, left: { style: 'thin' }, bottom: { style: 'thin' }, right: { style: 'thin' } }
        //Merge Cells
        worksheet.mergeCells(`A${footerRow.number}:B${footerRow.number}`);
        //Generate Excel File with given name
        workbook.xlsx.writeBuffer().then((data) => {
            let blob = new Blob([data], { type: 'application/vnd.openxmlformats-officedocument.spreadsheetml.sheet' });
            fs.saveAs(blob, `${title}.xlsx`);
        })

    }
    inventory_report_import_exportComponent(data: any): void {
        console.log(data)
        const title = this._translocoService.translate('NAV.inventory_report_import_export').toUpperCase();
        const header = [this._translocoService.translate('no_'), this._translocoService.translate('item'), this._translocoService.translate('NAV.sys_unit'), this._translocoService.translate('inventory.quantity_beginning_stocks'), this._translocoService.translate('inventory.quantity_import'), this._translocoService.translate('inventory.quantity_export'), this._translocoService.translate('inventory.quantity_ending_stocks')]
        //Create workbook and worksheet
        let workbook = new Workbook();
        let worksheet = workbook.addWorksheet(this._translocoService.translate('NAV.inventory_report_import_export'));
        //Add Row and formatting
        let titleRow = worksheet.addRow([title]);
        titleRow.alignment = { vertical: 'middle', horizontal: "center" }
        titleRow.font = { name: 'Times New Roman', family: 4, size: 16, bold: true }
        worksheet.addRow([]);
        //let subTitleRow = worksheet.addRow(['Date : ' + this.datePipe.transform(new Date(), 'medium')])
        //Add Image
        let logo = workbook.addImage({
            base64: logoFile.logoBase64,
            extension: 'png',
        });
        worksheet.properties.defaultRowHeight = 20;
        worksheet.addImage(logo, 'E1:F2');
        worksheet.mergeCells('A1:D2');
        //Blank Row
        worksheet.addRow([]);
        //Add Header Row
        let headerRow = worksheet.addRow(header);
        // Cell Style : Fill and Border
        headerRow.font = { name: 'Times New Roman', family: 4, size: 13, bold: true }
        headerRow.alignment = { vertical: 'middle', horizontal: "center" }
        headerRow.eachCell((cell, number) => {
            cell.fill = {
                type: 'pattern',
                pattern: 'solid',
                fgColor: { argb: 'FFFFFF00' },
                bgColor: { argb: 'FF0000FF' }
            }
            cell.border = { top: { style: 'thin' }, left: { style: 'thin' }, bottom: { style: 'thin' }, right: { style: 'thin' } }
        })
        // Add Data and Conditional Formatting
        data.forEach(d => {
            let row = worksheet.addRow(d);
            row.alignment = { vertical: 'middle', horizontal: "center" }
            row.eachCell((cell, number) => {
                // cell.fill = {
                //     type: 'pattern',
                //     pattern: 'solid',
                //     fgColor: { argb: 'FFFFFF00' },
                //     bgColor: { argb: 'FF0000FF' }
                // }
                cell.border = { top: { style: 'thin' }, left: { style: 'thin' }, bottom: { style: 'thin' }, right: { style: 'thin' } }
            })
            row.font = { name: 'Times New Roman', family: 4, size: 13, bold: false }
            let lastRemainingQuantity = row.getCell(7);
            let color = 'FF99FF99';
            if (lastRemainingQuantity.value < 0) {
                color = 'FF9999'
            }
            lastRemainingQuantity.fill = {
                type: 'pattern',
                pattern: 'solid',
                fgColor: { argb: color }
            }
        }
        );
        worksheet.getColumn(2).width = 40;
        worksheet.getColumn(3).width = 15;
        worksheet.getColumn(4).width = 15;
        worksheet.getColumn(5).width = 20;
        worksheet.getColumn(6).width = 20;
        worksheet.getColumn(7).width = 15;
        worksheet.addRow([]);
        //Footer Row
        let footerRow = worksheet.addRow(['Date : ' + this.datePipe.transform(new Date(), 'medium')]);
        footerRow.getCell(1).fill = {
            type: 'pattern',
            pattern: 'solid',
            fgColor: { argb: 'FFCCFFE5' }
        };
        footerRow.getCell(1).border = { top: { style: 'thin' }, left: { style: 'thin' }, bottom: { style: 'thin' }, right: { style: 'thin' } }
        //Merge Cells
        worksheet.mergeCells(`A${footerRow.number}:B${footerRow.number}`);
        //Generate Excel File with given name
        workbook.xlsx.writeBuffer().then((data) => {
            let blob = new Blob([data], { type: 'application/vnd.openxmlformats-officedocument.spreadsheetml.sheet' });
            fs.saveAs(blob, `${title}.xlsx`);
        })
    }
    maintenance_report_system(data: any): void {
        console.log(data)
        const title = this._translocoService.translate('NAV.maintenance_report_system').toUpperCase();
        const header = [this._translocoService.translate('no_'), this._translocoService.translate('NAV.sys_factory_line'), this._translocoService.translate('maintenance.maintenance_system'), this._translocoService.translate('maintenance.maintenance_code'), this._translocoService.translate('maintenance.asset_code'), this._translocoService.translate('maintenance.date_purchase'), this._translocoService.translate('maintenance.install_date'), this._translocoService.translate('maintenance.maintenance_period')]
        //Create workbook and worksheet
        let workbook = new Workbook();
        let worksheet = workbook.addWorksheet(this._translocoService.translate('NAV.maintenance_report_system'));
        //Add Row and formatting
        let titleRow = worksheet.addRow([title]);
        titleRow.alignment = { vertical: 'middle', horizontal: "center" }
        titleRow.font = { name: 'Times New Roman', family: 4, size: 16, bold: true }
        worksheet.addRow([]);
        //let subTitleRow = worksheet.addRow(['Date : ' + this.datePipe.transform(new Date(), 'medium')])
        //Add Image
        let logo = workbook.addImage({
            base64: logoFile.logoBase64,
            extension: 'png',
        });
        worksheet.properties.defaultRowHeight = 20;
        worksheet.addImage(logo, 'E1:F2');
        worksheet.mergeCells('A1:D2');
        //Blank Row
        worksheet.addRow([]);
        //Add Header Row
        let headerRow = worksheet.addRow(header);
        // Cell Style : Fill and Border
        headerRow.font = { name: 'Times New Roman', family: 4, size: 13, bold: true }
        headerRow.alignment = { vertical: 'middle', horizontal: "center" }
        headerRow.eachCell((cell, number) => {
            cell.fill = {
                type: 'pattern',
                pattern: 'solid',
                fgColor: { argb: 'FFFFFF00' },
                bgColor: { argb: 'FF0000FF' }
            }
            cell.border = { top: { style: 'thin' }, left: { style: 'thin' }, bottom: { style: 'thin' }, right: { style: 'thin' } }
        })
        // Add Data and Conditional Formatting
        data.forEach(d => {
            let row = worksheet.addRow(d);
            row.alignment = { vertical: 'middle', horizontal: "center" }
            row.eachCell((cell, number) => {
                // cell.fill = {
                //     type: 'pattern',
                //     pattern: 'solid',
                //     fgColor: { argb: 'FFFFFF00' },
                //     bgColor: { argb: 'FF0000FF' }
                // }
                cell.border = { top: { style: 'thin' }, left: { style: 'thin' }, bottom: { style: 'thin' }, right: { style: 'thin' } }
            })
            row.font = { name: 'Times New Roman', family: 4, size: 13, bold: false }
            let lastRemainingQuantity = row.getCell(7);
            let color = 'FF99FF99';
            if (lastRemainingQuantity.value < 0) {
                color = 'FF9999'
            }
            lastRemainingQuantity.fill = {
                type: 'pattern',
                pattern: 'solid',
                fgColor: { argb: color }
            }
        }
        );
        worksheet.getColumn(2).width = 15;
        worksheet.getColumn(3).width = 15;
        worksheet.getColumn(4).width = 15;
        worksheet.getColumn(5).width = 15;
        worksheet.getColumn(6).width = 15;
        worksheet.getColumn(7).width = 15;
        worksheet.addRow([]);
        //Footer Row
        let footerRow = worksheet.addRow(['Date : ' + this.datePipe.transform(new Date(), 'medium')]);
        footerRow.getCell(1).fill = {
            type: 'pattern',
            pattern: 'solid',
            fgColor: { argb: 'FFCCFFE5' }
        };
        footerRow.getCell(1).border = { top: { style: 'thin' }, left: { style: 'thin' }, bottom: { style: 'thin' }, right: { style: 'thin' } }
        //Merge Cells
        worksheet.mergeCells(`A${footerRow.number}:B${footerRow.number}`);
        //Generate Excel File with given name
        workbook.xlsx.writeBuffer().then((data) => {
            let blob = new Blob([data], { type: 'application/vnd.openxmlformats-officedocument.spreadsheetml.sheet' });
            fs.saveAs(blob, `${title}.xlsx`);
        })
    }
    maintenance_report_system_device_detail(data: any): void {
        console.log(data)
        const title = this._translocoService.translate('NAV.maintenance_report_system_device_detail').toUpperCase();
        const header = [this._translocoService.translate('no_'), this._translocoService.translate('maintenance.maintenance_system_device_detail'), this._translocoService.translate('maintenance.model'), this._translocoService.translate('maintenance.specification_name'), this._translocoService.translate('maintenance.quantity')]
        //Create workbook and worksheet
        let workbook = new Workbook();
        let worksheet = workbook.addWorksheet(this._translocoService.translate('NAV.maintenance_report_system_device_detail'));
        //Add Row and formatting
        let titleRow = worksheet.addRow([title]);
        titleRow.alignment = { vertical: 'middle', horizontal: "center" }
        titleRow.font = { name: 'Times New Roman', family: 4, size: 16, bold: true }
        worksheet.addRow([]);
        //let subTitleRow = worksheet.addRow(['Date : ' + this.datePipe.transform(new Date(), 'medium')])
        //Add Image
        let logo = workbook.addImage({
            base64: logoFile.logoBase64,
            extension: 'png',
        });
        worksheet.properties.defaultRowHeight = 20;
        worksheet.addImage(logo, 'E1:F2');
        worksheet.mergeCells('A1:D2');
        //Blank Row
        worksheet.addRow([]);
        //Add Header Row
        let headerRow = worksheet.addRow(header);
        // Cell Style : Fill and Border
        headerRow.font = { name: 'Times New Roman', family: 4, size: 13, bold: true }
        headerRow.alignment = { vertical: 'middle', horizontal: "center" }
        headerRow.eachCell((cell, number) => {
            cell.fill = {
                type: 'pattern',
                pattern: 'solid',
                fgColor: { argb: 'FFFFFF00' },
                bgColor: { argb: 'FF0000FF' }
            }
            cell.border = { top: { style: 'thin' }, left: { style: 'thin' }, bottom: { style: 'thin' }, right: { style: 'thin' } }
        })
        // Add Data and Conditional Formatting
        data.forEach(d => {
            let row = worksheet.addRow(d);
            row.alignment = { vertical: 'middle', horizontal: "center" }
            row.eachCell((cell, number) => {
                // cell.fill = {
                //     type: 'pattern',
                //     pattern: 'solid',
                //     fgColor: { argb: 'FFFFFF00' },
                //     bgColor: { argb: 'FF0000FF' }
                // }
                cell.border = { top: { style: 'thin' }, left: { style: 'thin' }, bottom: { style: 'thin' }, right: { style: 'thin' } }
            })
            row.font = { name: 'Times New Roman', family: 4, size: 13, bold: false }
            let lastRemainingQuantity = row.getCell(7);
            let color = 'FF99FF99';
            if (lastRemainingQuantity.value < 0) {
                color = 'FF9999'
            }
            lastRemainingQuantity.fill = {
                type: 'pattern',
                pattern: 'solid',
                fgColor: { argb: color }
            }
        }
        );
        worksheet.getColumn(2).width = 40;
        worksheet.getColumn(3).width = 15;
        worksheet.getColumn(4).width = 15;
        worksheet.getColumn(5).width = 20;
        worksheet.getColumn(6).width = 20;
        worksheet.getColumn(7).width = 15;
        worksheet.addRow([]);
        //Footer Row
        let footerRow = worksheet.addRow(['Date : ' + this.datePipe.transform(new Date(), 'medium')]);
        footerRow.getCell(1).fill = {
            type: 'pattern',
            pattern: 'solid',
            fgColor: { argb: 'FFCCFFE5' }
        };
        footerRow.getCell(1).border = { top: { style: 'thin' }, left: { style: 'thin' }, bottom: { style: 'thin' }, right: { style: 'thin' } }
        //Merge Cells
        worksheet.mergeCells(`A${footerRow.number}:B${footerRow.number}`);
        //Generate Excel File with given name
        workbook.xlsx.writeBuffer().then((data) => {
            let blob = new Blob([data], { type: 'application/vnd.openxmlformats-officedocument.spreadsheetml.sheet' });
            fs.saveAs(blob, `${title}.xlsx`);
        })
    }
}
