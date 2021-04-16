import { Component, OnInit } from '@angular/core';
import { ReportingService } from '../../../_services/reporting.service';
import { saveAs } from 'file-saver';
import { AlertifyService } from '../../../_services/alertify.service';
@Component({
  selector: 'app-masterReports',
  templateUrl: './masterReports.component.html',
  styleUrls: ['./masterReports.component.scss']
})
export class MasterReportsComponent implements OnInit {
  reportType: number = 0;
  wildCard: string = '';
  itemType: number = 0;

  showItemType: boolean = false;
  showWildCard: boolean = false;

  loading: boolean = false;

  constructor(private reportSvc: ReportingService, private alertify: AlertifyService) { }

  ngOnInit() {

    this.showItemType = true;
    this.showWildCard = true;
    this.itemType = 4;
  }

  getReport() {
    let reportName = '';
    this.loading = true;
    if (this.reportType == 0) { reportName = 'Item Report.pdf'; }
    if (this.reportType == 1) { reportName = 'Supplier Report.pdf'; }
    if (this.reportType == 2) { reportName = 'Customer Report.pdf'; }
    if (this.reportType == 3) { reportName = 'Business Place Report.pdf'; }
    if (this.reportType == 4) { reportName = 'Unit Report.pdf'; }
    if (this.reportType == 5) { reportName = 'Item Cat Report.pdf'; }

    this.reportSvc.getMasterReports(this.reportType, this.itemType, this.wildCard).subscribe((data) => {
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
    if (this.reportType == 0) {
      this.showItemType = true;
      return;
    }
    this.showItemType = false;
  }

}
