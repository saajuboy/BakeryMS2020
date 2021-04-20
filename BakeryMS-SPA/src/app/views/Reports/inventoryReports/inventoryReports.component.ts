import { Component, OnInit } from '@angular/core';
import { AlertifyService } from '../../../_services/alertify.service';
import { ReportingService } from '../../../_services/reporting.service';
import { UtilityService } from '../../../_services/utility.service';
import { saveAs } from 'file-saver';

@Component({
  selector: 'app-inventoryReports',
  templateUrl: './inventoryReports.component.html',
  styleUrls: ['./inventoryReports.component.scss']
})
export class InventoryReportsComponent implements OnInit {
  reportType: number = 0;
  wildCard: string = '';
  reportRange: number = 0;
  date: string = '';
  month: number;
  year: number;

  showRange: boolean = false;
  showWildCard: boolean = false;

  loading: boolean = false;

  constructor(private reportSvc: ReportingService, private alertify: AlertifyService, private uti: UtilityService) { }

  ngOnInit() {

    this.showRange = true;
    this.showWildCard = true;
    this.reportType = 0;
    this.reportRange = 0;

    const date = new Date();
    this.date = this.uti.currentDate();
    this.month = date.getMonth() + 1;
    this.year = date.getFullYear();
  }

  getReport() {
    let reportName = '';
    this.loading = true;
    const rangeText = this.reportRange == 0 ? 'Daily' : this.reportRange == 1 ? 'Monthly' : 'Yearly' as string;
    if (this.reportType == 0) { reportName = 'Stock Report ' + rangeText + ' .pdf'; }
    if (this.reportType == 1) { reportName = 'Expense Report ' + rangeText + '.pdf'; }
    if (this.reportType == 2) { reportName = 'Expense/Income Report ' + rangeText + '.pdf'; }

    this.reportSvc.getInventoryReports(this.reportType, this.reportRange, this.date, this.month, this.year, this.wildCard).subscribe((data) => {
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
