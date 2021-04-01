import { Component, OnInit, ViewChild } from '@angular/core';
import { Router } from '@angular/router';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { AvailableItemForList, SalesHeader } from '../../../_models/availableItems';
import { BusinessPlace } from '../../../_models/businessPlace';
import { AlertifyService } from '../../../_services/alertify.service';
import { InventoryService } from '../../../_services/inventory.service';
import { MasterService } from '../../../_services/master.service';
import { PosService } from '../../../_services/pos.service';
import { UtilityService } from '../../../_services/utility.service';

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
    this.toDate = this.utiService.currentDate();

    this.masterService.getBusinessPlaces().subscribe(result => {
      this.businessPlaces = result;
      this.businessPlaces.sort((a, b) => b.id - a.id);
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
}
