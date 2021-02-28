import { Component, OnInit, ViewChild } from '@angular/core';
import { Router } from '@angular/router';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { ProductionOrderHeader } from '../../../_models/productionOrder';
import { AlertifyService } from '../../../_services/alertify.service';
import { ManufacturingService } from '../../../_services/manufacturing.service';
import { MasterService } from '../../../_services/master.service';

@Component({
  // tslint:disable-next-line: component-selector
  selector: 'app-ProductionOrderList',
  templateUrl: './ProductionOrderList.component.html',
  styleUrls: ['./ProductionOrderList.component.scss']
})
export class ProductionOrderListComponent implements OnInit {
  @ViewChild('infoModal') public infoModal: ModalDirective;

  productionOrders: ProductionOrderHeader[];
  search: string = '';
  productionOrderInfo: ProductionOrderHeader = <ProductionOrderHeader>{};
  sortOrder = { one: false, two: false, three: false, four: false, five: false };

  constructor(private masterService: MasterService,
    private alertify: AlertifyService,
    private router: Router,
    private manufacturingService: ManufacturingService) { }

  ngOnInit() {

    this.manufacturingService.getProductionOrders().subscribe(result => {
      this.productionOrders = result;
      // console.log(result);
      console.log(this.productionOrders);


    }, error => {
      this.alertify.error(error);
    });
  }

  add() {
    this.router.navigateByUrl('/manufacturing/productionOrder/create');
  }

  delete(id: number) {
    this.alertify.confirm('Are you sure?',
      'Are you sure you want to delete this Production Order? This action cannot be undone',
      () => {
        this.manufacturingService.deleteProductionOrder(id).subscribe((next) => {
          this.alertify.success('Production Order deleted succesfully');
          this.productionOrders = this.productionOrders.filter(function (obj) {
            return obj.id !== id;
          });
        }, () => {
          this.alertify.error('Failed to Delete Production Order');
        });
      },
      () => { });

  }
  edit(id: number) {
    this.router.navigate(['/manufacturing/productionOrder/edit', id]);
  }
  ShowInfo(id: number) {

    this.manufacturingService.getProductionOrderDetailsOfHeader(id).subscribe(result => {
      this.productionOrderInfo = this.productionOrders.find(a => a.id === id);
      this.productionOrderInfo.productionOrderDetails = result;
      this.infoModal.show();
    });

  }

  sort(propertyNumber: number) {

    switch (propertyNumber) {
      case 1:
        this.productionOrders.sort((a, b) => this.sortOrder.one === false ?
         a.productionOrderNo - b.productionOrderNo : b.productionOrderNo - a.productionOrderNo);
        this.sortOrder.one = !this.sortOrder.one;
        break;
      case 2:
        this.productionOrders.sort((a, b) => this.sortOrder.two === false ?
          a.sessionName.localeCompare(b.sessionName) :
          b.sessionName.localeCompare(a.sessionName));
        this.sortOrder.two = !this.sortOrder.two;
        break;
      case 3:
        this.productionOrders.sort((a, b) => {
          return this.sortOrder.three === false ?
            <any>new Date(b.requiredDate) - <any>new Date(a.requiredDate) :
            <any>new Date(a.requiredDate) - <any>new Date(b.requiredDate);
        });
        this.sortOrder.three = !this.sortOrder.three;
        break;
      case 4:
        this.productionOrders.sort((a, b) => this.sortOrder.four === false ?
          +a.isNotEditable - +b.isNotEditable : +b.isNotEditable - +a.isNotEditable);
        this.sortOrder.four = !this.sortOrder.four;
        break;
      case 5:
        this.productionOrders.sort((a, b) => this.sortOrder.five === false ?
          a.businessPlaceName.localeCompare(b.businessPlaceName) :
          b.businessPlaceName.localeCompare(a.businessPlaceName));
        this.sortOrder.five = !this.sortOrder.five;
        break;
      default:
        this.productionOrders.sort((a, b) => this.sortOrder.one === false ?
        a.productionOrderNo - b.productionOrderNo : b.productionOrderNo - a.productionOrderNo);
       this.sortOrder.one = !this.sortOrder.one;
       break;
    }
  }

}

