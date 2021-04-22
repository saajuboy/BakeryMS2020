import { Component, OnInit } from '@angular/core';
import { AlertifyService } from '../../../_services/alertify.service';
import { ReportingService } from '../../../_services/reporting.service';
import { UtilityService } from '../../../_services/utility.service';
import { saveAs } from 'file-saver';
import { ItemForDropdown } from '../../../_models/item';
import { MasterService } from '../../../_services/master.service';

@Component({
  selector: 'app-manufacturingReports',
  templateUrl: './manufacturingReports.component.html',
  styleUrls: ['./manufacturingReports.component.scss']
})
export class ManufacturingReportsComponent implements OnInit {
  reportType: number = 0;
  wildCard: string = '';
  reportRange: number = 0;
  date: string = '';
  month: number;
  year: number;
  items: ItemForDropdown[] = [];
  showRange: boolean = false;
  showWildCard: boolean = false;

  loading: boolean = false;

  constructor(private reportSvc: ReportingService,
    private alertify: AlertifyService,
    private uti: UtilityService,
    private masterSvc: MasterService) { }

  ngOnInit() {

    this.masterSvc.getItems(true, 0).subscribe((result: ItemForDropdown[]) => {
      this.items = result;
    });
    this.showRange = true;
    this.showWildCard = true;
    this.reportType = 0;
    this.reportRange = 0;

    // const date = new Date();
    // this.date = this.uti.currentDate();
    // this.month = date.getMonth() + 1;
    // this.year = date.getFullYear();
  }

  getReport() {
    let reportName = '';
    this.loading = true;
    const rangeText = this.reportRange == 0 ? ' All items' : '' as string;
    if (this.reportType == 0) { reportName = 'Ingredients Report' + rangeText + '.pdf'; }

    this.reportSvc.getManufacturingReports(this.reportType, this.reportRange, this.wildCard).subscribe((data) => {
      const blob = new Blob([data], { type: 'application/pdf' });
      saveAs(blob, reportName);
      this.loading = false;
    },
      (res) => {
        if (res.error.message) {
          this.alertify.error(res.error.message + '');
        } else {
          this.alertify.error('report not available');
        }
        this.loading = false;
      }
    );

  }

  reportTypeChanged() {
    // if (this.reportType == 0 || this.reportType == 1) {
    //   this.showRange = true;
    //   return;
    // }
    // this.showRange = false;
  }

}
