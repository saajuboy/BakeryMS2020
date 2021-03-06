import { Component, OnInit, ViewChild } from '@angular/core';
import { Router } from '@angular/router';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { AvailableItemForList } from '../../../_models/availableItems';
import { BusinessPlace } from '../../../_models/businessPlace';
import { Item, ItemCategory, Unit } from '../../../_models/item';
import { AlertifyService } from '../../../_services/alertify.service';
import { InventoryService } from '../../../_services/inventory.service';
import { MasterService } from '../../../_services/master.service';

@Component({
  selector: 'app-AvailableItems',
  templateUrl: './AvailableItems.component.html',
  styleUrls: ['./AvailableItems.component.scss']
})
export class AvailableItemsComponent implements OnInit {
  @ViewChild('infoModal') public infoModal: ModalDirective;

  businessPlaces: BusinessPlace[] = [];
  businessPlace: number = 0;
  type: number = 0;
  items: AvailableItemForList[] = [];
  search: string = '';
  itemInfo: AvailableItemForList = <AvailableItemForList>{};
  sortOrder = { one: false, two: false, three: false, four: false, five: false, six: false };
  availORReorder: number = 0;
  isNeedToReorder: boolean = false;
  // pageOfItems: Array<any>;

  constructor(private masterService: MasterService,
    private invService: InventoryService,
    private alertify: AlertifyService,
    private router: Router) { }

  ngOnInit() {

    this.availORReorder = 0;
    this.masterService.getBusinessPlaces().subscribe(result => {
      this.businessPlaces = result;
      this.businessPlaces.sort((a, b) => b.id - a.id);
      this.businessPlace = +localStorage.getItem('BusinessPlaceId');
      this.bpOrTypeChange();
    }, error => {
      this.alertify.error(error);
    });
  }
  bpOrTypeChange() {
    if (this.availORReorder == 0) {
      this.invService.getAvailableItems(this.businessPlace, this.type).subscribe((res) => {
        this.items = res;
      }, (result) => {
        this.alertify.warning(result.error.message + ' : ' + result.error.code);
      });
    }
    if (this.availORReorder == 1) {
      this.invService.getReorderItems(this.businessPlace, this.type).subscribe((res) => {
        this.items = res;
        this.isNeedToReorder = this.items.some(a => a.isReorder === true);
        // console.log(this.items);

      }, (result) => {
        this.alertify.warning(result.error.message + ' : ' + result.error.code);
      });
    }

  }
  reorder() {
    if (this.availORReorder == 1 && this.isNeedToReorder === true) {
      if (this.type == 0) {
        this.router.navigate(['manufacturing/productionOrder/reOrder/' + this.businessPlace]);
      }
      if (this.type == 1 || this.type == 2) {
        this.router.navigate(['inventory/purchaseOrder/reOrder/' + this.businessPlace + '/' + this.type]);
      }
    }

  }
  ShowItemInfo(id: number) {
    this.invService.getAvailableItem(id, this.type).subscribe((result) => {
      this.itemInfo = result;
      this.infoModal.show();
    }, () => {
      this.alertify.error('some Error occured');
    });
  }

  sort(propertyNumber: number) {

    switch (propertyNumber) {
      case 1:
        this.items.sort((a, b) => this.sortOrder.one === false ? a.name.localeCompare(b.name) : b.name.localeCompare(a.name));
        this.sortOrder.one = !this.sortOrder.one;
        break;
      case 2:
        this.items.sort((a, b) => this.sortOrder.two === false ? a.code.localeCompare(b.code) : b.code.localeCompare(a.code));
        this.sortOrder.two = !this.sortOrder.two;
        break;
      case 3:
        this.items.sort((a, b) => this.sortOrder.three === false ?
          a.costPrice - b.costPrice :
          b.costPrice - a.costPrice);
        this.sortOrder.three = !this.sortOrder.three;
        break;
      case 4:
        this.items.sort((a, b) => this.sortOrder.four === false ?
          a.availableQuantity - b.availableQuantity : b.availableQuantity - a.availableQuantity);
        this.sortOrder.four = !this.sortOrder.four;
        break;
      case 5:
        this.items.sort((a, b) => {
          return this.sortOrder.five === false ?
            <any>new Date(b.expireDate) - <any>new Date(a.expireDate) :
            <any>new Date(a.expireDate) - <any>new Date(b.expireDate);
        });
        this.sortOrder.five = !this.sortOrder.five;
        break;
      case 6:
        this.items.sort((a, b) => this.sortOrder.six === false ?
          a.batchNo - b.batchNo : b.batchNo - a.batchNo);
        this.sortOrder.six = !this.sortOrder.six;
        break;
      default:
        this.items.sort((a, b) => this.sortOrder.one === false ? a.name.localeCompare(b.name) : b.name.localeCompare(a.name));
        this.sortOrder.one = !this.sortOrder.one;
        break;
    }
  }
  // onChangePage(pageOfItems: Array<any>) {
  //   // update current page of items
  //   this.pageOfItems = pageOfItems;
  // }
}
