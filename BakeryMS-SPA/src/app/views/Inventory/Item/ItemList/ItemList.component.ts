import { Component, OnInit, ViewChild } from '@angular/core';
import { Router } from '@angular/router';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { Item, ItemCategory, Unit } from '../../../../_models/item';
import { AlertifyService } from '../../../../_services/alertify.service';
import { MasterService } from '../../../../_services/master.service';

@Component({
  // tslint:disable-next-line: component-selector
  selector: 'app-ItemList',
  templateUrl: './ItemList.component.html',
  styleUrls: ['./ItemList.component.scss']
})
export class ItemListComponent implements OnInit {
  @ViewChild('infoModal') public infoModal: ModalDirective;

  items: Item[];
  search: string = '';
  itemInfo: Item = <Item>{ itemCategory: <ItemCategory>{}, unit: <Unit>{} };
  sortOrder = { one: false, two: false, three: false, four: false };
  pageOfItems: Array<any>;

  constructor(private masterService: MasterService, private alertify: AlertifyService, private router: Router) { }

  ngOnInit() {

    this.masterService.getItems(false).subscribe(result => {
      this.items = result;
      this.items.sort((a, b) => b.id - a.id);
    }, error => {
      this.alertify.error(error);
    });
  }

  addItem() {
    this.router.navigateByUrl('/master/item/create');
  }

  delete(id: number) {
    this.alertify.confirm('Are you sure?',
      'Are you sure you want to delete this item? This action cannot be undone',
      () => {
        this.masterService.deleteItem(id).subscribe((next) => {
          this.alertify.success('Item deleted succesfully');
          this.items = this.items.filter(function (obj) {
            return obj.id !== id;
          });
        }, () => {
          this.alertify.error('Failed to Delete Item');
        });
      },
      () => { });

  }
  editItem(id: number) {
    this.router.navigate(['master/item/edit', id]);
  }
  ShowItemInfo(id: number) {
    this.masterService.getItem(id).subscribe((result) => {
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
          a.description.localeCompare(b.description) :
          b.description.localeCompare(a.description));
        this.sortOrder.three = !this.sortOrder.three;
        break;
      case 4:
        this.items.sort((a, b) => this.sortOrder.four === false ? a.type - b.type : b.type - a.type);
        this.sortOrder.four = !this.sortOrder.four;
        break;
      default:
        this.items.sort((a, b) => this.sortOrder.one === false ? a.name.localeCompare(b.name) : b.name.localeCompare(a.name));
        this.sortOrder.one = !this.sortOrder.one;
        break;
    }
  }
  onChangePage(pageOfItems: Array<any>) {
    // update current page of items
    this.pageOfItems = pageOfItems;
  }

}
