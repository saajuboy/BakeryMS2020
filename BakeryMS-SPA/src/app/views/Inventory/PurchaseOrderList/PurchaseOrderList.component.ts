import { Component, OnInit, ViewChild } from '@angular/core';
import { Router } from '@angular/router';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { subscribeOn } from 'rxjs/operators';
import { PurchaseOrderHeader } from '../../../_models/purchaseOrder';
import { AlertifyService } from '../../../_services/alertify.service';
import { InventoryService } from '../../../_services/inventory.service';
import { MasterService } from '../../../_services/master.service';

@Component({
  // tslint:disable-next-line: component-selector
  selector: 'app-PurchaseOrderList',
  templateUrl: './PurchaseOrderList.component.html',
  styleUrls: ['./PurchaseOrderList.component.scss']
})
export class PurchaseOrderListComponent implements OnInit {
  @ViewChild('infoModal') public infoModal: ModalDirective;

  purchaseOrders: PurchaseOrderHeader[];
  search: string = '';
  purchaseOrderInfo: PurchaseOrderHeader = <PurchaseOrderHeader>{};
  sortOrder = { one: false, two: false, three: false, four: false, five: false, six: false };

  constructor(private masterService: MasterService,
    private alertify: AlertifyService,
    private router: Router,
    private inventoryService: InventoryService) { }

  ngOnInit() {

    this.inventoryService.getPurchaseOrders().subscribe(result => {
      this.purchaseOrders = result;
      this.purchaseOrders.sort((a, b) => b.id - a.id);
      // console.log(result);
      console.log(this.purchaseOrders);


    }, error => {
      this.alertify.error(error);
    });
  }

  add() {
    this.router.navigateByUrl('/inventory/purchaseOrder/create');
  }

  delete(id: number) {
    this.alertify.confirm('Are you sure?',
      'Are you sure you want to delete this Purchase Order? This action cannot be undone',
      () => {
        this.inventoryService.deletePurchaseOrder(id).subscribe((next) => {
          this.alertify.success('Purchase Order deleted succesfully');
          this.purchaseOrders = this.purchaseOrders.filter(function (obj) {
            return obj.id !== id;
          });
        }, () => {
          this.alertify.error('Failed to Delete Purchase Order');
        });
      },
      () => { });

  }
  edit(id: number) {
    this.router.navigate(['/inventory/purchaseOrder/edit', id]);
  }
  ShowInfo(id: number) {

    this.inventoryService.getPurchaseOrderDetailsOfHeader(id).subscribe(result => {
      this.purchaseOrderInfo = this.purchaseOrders.find(a => a.id === id);
      this.purchaseOrderInfo.poDetail = result;
      this.infoModal.show();
    });

  }

  sort(propertyNumber: number) {

    switch (propertyNumber) {
      case 1:
        this.purchaseOrders.sort((a, b) => this.sortOrder.one === false ? a.poNumber - b.poNumber : b.poNumber - a.poNumber);
        this.sortOrder.one = !this.sortOrder.one;
        break;
      case 2:
        this.purchaseOrders.sort((a, b) => this.sortOrder.two === false ?
          a.supplierName.localeCompare(b.supplierName) :
          b.supplierName.localeCompare(a.supplierName));
        this.sortOrder.two = !this.sortOrder.two;
        break;
      case 3:
        this.purchaseOrders.sort((a, b) => this.sortOrder.three === false ?
          a.status - b.status : b.status - a.status);
        this.sortOrder.three = !this.sortOrder.three;
        break;
      case 4:
        this.purchaseOrders.sort((a, b) => {
          return this.sortOrder.four === false ?
            <any>new Date(b.deliveryDate) - <any>new Date(a.deliveryDate) :
            <any>new Date(a.deliveryDate) - <any>new Date(b.deliveryDate);
        });
        this.sortOrder.four = !this.sortOrder.four;
        break;
      case 5:
        this.purchaseOrders.sort((a, b) => {
          return this.sortOrder.five === false ?
            <any>new Date(b.orderDate) - <any>new Date(a.orderDate) :
            <any>new Date(a.orderDate) - <any>new Date(b.orderDate);
        });
        this.sortOrder.five = !this.sortOrder.five;
        break;
      case 6:
        this.purchaseOrders.sort((a, b) => this.sortOrder.six === false ?
          a.businessPlaceName.localeCompare(b.businessPlaceName) :
          b.businessPlaceName.localeCompare(a.businessPlaceName));
        this.sortOrder.six = !this.sortOrder.six;
        break;
      default:
        this.purchaseOrders.sort((a, b) => this.sortOrder.one === false ? a.poNumber - b.poNumber : b.poNumber - a.poNumber);
        this.sortOrder.one = !this.sortOrder.one;
        break;
    }
  }

}
