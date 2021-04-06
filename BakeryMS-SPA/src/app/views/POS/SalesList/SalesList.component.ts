import { Component, OnInit, ViewChild } from '@angular/core';
import { Router } from '@angular/router';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { SalesHeader } from '../../../_models/availableItems';
import { BusinessPlace } from '../../../_models/businessPlace';
import { AlertifyService } from '../../../_services/alertify.service';
import { MasterService } from '../../../_services/master.service';
import { PosService } from '../../../_services/pos.service';
import { UtilityService } from '../../../_services/utility.service';
import { jsPDF } from 'jspdf';

@Component({
  selector: 'app-SalesList',
  templateUrl: './SalesList.component.html',
  styleUrls: ['./SalesList.component.scss']
})
export class SalesListComponent implements OnInit {
  @ViewChild('infoModal') public infoModal: ModalDirective;

  businessPlaces: BusinessPlace[] = [];
  businessPlace: number = 0;
  sales: SalesHeader[] = [];
  search: string = '';
  salesInfo: SalesHeader = <SalesHeader>{};
  sortOrder = { one: false, two: false, three: false, four: false, five: false, six: false };
  fromDate: string;
  toDate: string;
  // pageOfItems: Array<any>;

  constructor(private masterService: MasterService,
    private posService: PosService,
    private alertify: AlertifyService,
    private utiService: UtilityService,
    private router: Router) { }

  ngOnInit() {

    this.fromDate = this.utiService.addDate(new Date(), -1);
    this.toDate = this.utiService.addDate(new Date(), 1);

    this.masterService.getBusinessPlaces().subscribe(result => {
      this.businessPlaces = result;
      this.businessPlaces.sort((a, b) => b.id - a.id);

      this.businessPlace = +localStorage.getItem('BusinessPlaceId');
      this.bpOrdateChange();
    }, error => {
      this.alertify.error(error);
    });
  }
  bpOrdateChange() {
    this.posService.getSales(this.businessPlace, this.fromDate, this.toDate).subscribe((res) => {
      this.sales = res;
    }, (result) => {
      this.alertify.warning(result.error.message + ' : ' + result.error.code);
    });
  }
  addItem() {
    this.router.navigateByUrl('/pos/sales/create');
  }

  // delete(id: number) {
  //   this.alertify.confirm('Are you sure?',
  //     'Are you sure you want to delete this item? This action cannot be undone',
  //     () => {
  //       this.masterService.deleteItem(id).subscribe((next) => {
  //         this.alertify.success('Item deleted succesfully');
  //         this.sales = this.sales.filter(function (obj) {
  //           return obj.id !== id;
  //         });
  //       }, () => {
  //         this.alertify.error('Failed to Delete Item');
  //       });
  //     },
  //     () => { });

  // }
  // editItem(id: number) {
  //   this.router.navigate(['master/item/edit', id]);
  // }
  ShowItemInfo(id: number) {
    this.posService.getSale(id).subscribe((result) => {
      this.salesInfo = result;
      this.infoModal.show();
    }, () => {
      this.alertify.error('some Error occured');
    });
  }
  printPdf() {
    this.downloadPdf(this.salesInfo);
  }
  sort(propertyNumber: number) {

    switch (propertyNumber) {
      case 1:
        this.sales.sort((a, b) => this.sortOrder.one === false ?
          a.salesNo - b.salesNo :
          b.salesNo - a.salesNo);
        this.sortOrder.one = !this.sortOrder.one;
        break;

      case 2:
        this.sales.sort((a, b) => {
          return this.sortOrder.two === false ?
            <any>new Date(a.date) - <any>new Date(b.date) :
            <any>new Date(b.date) - <any>new Date(a.date);
        });
        this.sortOrder.two = !this.sortOrder.two;
        break;

      case 3:
        this.sales.sort((a, b) => this.sortOrder.three === false
          ? a.customerName.localeCompare(b.customerName) : b.customerName.localeCompare(a.customerName));
        this.sortOrder.three = !this.sortOrder.three;
        break;
      case 4:
        this.sales.sort((a, b) => this.sortOrder.four === false ?
          a.discount - b.discount : b.discount - a.discount);
        this.sortOrder.four = !this.sortOrder.four;
        break;
      case 5:
        this.sales.sort((a, b) => this.sortOrder.five === false ?
          a.total - b.total : b.total - a.total);
        this.sortOrder.five = !this.sortOrder.five;
        break;
      case 6:
        this.sales.sort((a, b) => this.sortOrder.six === false ?
          a.userName.localeCompare(b.userName) : b.userName.localeCompare(a.userName));
        this.sortOrder.six = !this.sortOrder.six;
        break;
      default:
        this.sales.sort((a, b) => this.sortOrder.one === false ?
          a.salesNo - b.salesNo :
          b.salesNo - a.salesNo);
        this.sortOrder.one = !this.sortOrder.one;
        break;
    }
  }
  // onChangePage(pageOfItems: Array<any>) {
  //   // update current page of items
  //   this.pageOfItems = pageOfItems;
  // }

  downloadPdf(sales: SalesHeader) {
    // const starry = [{ name: 'asdf1', qty: 10, prc: 20, ttl: 200 }
    //   , { name: 'asdf2', qty: 10, prc: 20, ttl: 200 },
    // { name: 'asdf3', qty: 10, prc: 20, ttl: 200 }]
    const doc = new jsPDF('portrait', 'pt', 'a4', true);

    let text = 'Upland Bake house';
    let yPos = 40;
    doc.setFontSize(12);
    doc.text(text, this.ofsetCenter(text, doc), yPos, {}, 'center');

    doc.setFontSize(10);
    text = sales.businessPlaceName; // businessPlaceName
    yPos += 15;
    doc.text(text, this.ofsetCenter(text, doc), yPos, {}, {});

    doc.setFontSize(8);
    text = sales.businessPlace.address
      .substring(0, 40); // businessPlaceName
    yPos += 15;
    doc.text(text, this.ofsetCenter(text, doc), yPos, {}, {});

    text = sales.businessPlace.address;
    if (text.length > 40) { // add adress here
      text = text.substring(41, 81);
      yPos += 10;
      doc.text(text, this.ofsetCenter(text, doc), yPos, {}, {});
    }

    doc.setFontSize(10);
    text = 'Tel - 052-2300034';
    yPos += 15;
    doc.text(text, this.ofsetCenter(text, doc), yPos, {}, {});

    doc.setFontSize(8);
    text = 'Invoice No - ' + sales.salesNo;
    yPos += 15;
    doc.text(text, 210, yPos, {}, {});

    text = 'Customer  - ' + sales.customerName.substring(0, 32);
    yPos += 15;
    doc.text(text, 210, yPos, {}, {});

    doc.setFontSize(10);
    text = '------------------------------------------------------'; // 55 characters
    yPos += 10;
    doc.text(text, this.ofsetCenter(text, doc), yPos, {}, {});

    text = 'Cashier     - ' + sales.userName;
    yPos += 10;
    doc.text(text, 210, yPos, {}, {});

    text = 'Date          - ' + sales.date;
    yPos += 15;
    doc.text(text, 210, yPos, {}, {});

    text = 'Time          - ' + sales.time;
    yPos += 15;
    doc.text(text, 210, yPos, {}, {});

    text = '------------------------------------------------------'; // 55 characters
    yPos += 10;
    doc.text(text, this.ofsetCenter(text, doc), yPos, {}, {});

    text = 'Item          Price        Qty           Amt';
    yPos += 10;
    doc.text(text, 210, yPos, {}, {});

    text = '------------------------------------------------------'; // 55 characters
    yPos += 10;
    doc.text(text, this.ofsetCenter(text, doc), yPos, {}, {});

    doc.setFontSize(8);
    let count = 0;
    sales.salesDetails.forEach(x => {
      count += 1;
      text = count + '. ' + x.itemName
        .substring(0, 37);
      yPos += 10;
      doc.text(text, 210, yPos, {}, {});

      yPos += 10;
      text = x.price.toFixed(2);
      doc.text(text, 260, yPos, {}, {});
      text = x.quantity.toFixed(2);
      doc.text(text, 300, yPos, {}, {});
      text = x.lineTotal.toFixed(2);
      doc.text(text, 340, yPos, {}, {});
    });

    doc.setFontSize(10);
    text = '------------------------------------------------------'; // 55 characters
    yPos += 10;
    doc.text(text, this.ofsetCenter(text, doc), yPos, {}, {});

    doc.setFontSize(7);
    text = 'Total Items - ' + count; // 55 characters
    yPos += 7;
    doc.text(text, 210, yPos, {}, {});

    doc.setFontSize(10);
    text = '      Total                  - ' + (sales.total + sales.discount).toFixed(2); // 55 characters
    yPos += 15;
    doc.text(text, 210, yPos, {}, {});

    text = '      Discount            - (' + (sales.discount).toFixed(2) + ')'; // 55 characters
    yPos += 13;
    doc.text(text, 210, yPos, {}, {});

    text = '      Net Total           - ' + (sales.total).toFixed(2); // 55 characters
    yPos += 13;
    doc.text(text, 210, yPos, {}, {});

    text = '      Received Cash - ' + (sales.receivedAmount).toFixed(2); // 55 characters
    yPos += 13;
    doc.text(text, 210, yPos, {}, {});

    text = '      Change             - ' + (sales.changeAmount).toFixed(2); // 55 characters
    yPos += 13;
    doc.text(text, 210, yPos, {}, {});

    doc.setFontSize(10);
    text = '------------------------------------------------------'; // 55 characters
    yPos += 10;
    doc.text(text, this.ofsetCenter(text, doc), yPos, {}, {});

    doc.setFontSize(8);
    text = 'Thank you For Dealing with us !!!!'; // 55 characters
    yPos += 10;
    doc.text(text, this.ofsetCenter(text, doc), yPos, {}, {});

    doc.save('receipt' + sales.salesNo + '.pdf');

  }

  ofsetCenter(text, doc) {
    const xOffset = (doc.internal.pageSize.width / 2) - (doc.getStringUnitWidth(text) * doc.getFontSize() / 2);
    return xOffset;
  }
}
