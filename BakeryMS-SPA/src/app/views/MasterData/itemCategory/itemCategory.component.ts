import { Component, OnInit, ViewChild } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { ItemCategory } from '../../../_models/item';
import { AlertifyService } from '../../../_services/alertify.service';
import { MasterService } from '../../../_services/master.service';

@Component({
  // tslint:disable-next-line: component-selector
  selector: 'app-itemCategory',
  templateUrl: './itemCategory.component.html',
  styleUrls: ['./itemCategory.component.scss']
})
export class ItemCategoryComponent implements OnInit {
  @ViewChild('infoModal') public infoModal: ModalDirective;
  @ViewChild('primaryModal') public addModal: ModalDirective;

  itemCategories: ItemCategory[];
  itemCategory: ItemCategory;
  itemCategoryID: number;
  search: string = '';
  itemCategoryInfo: any = {};
  sortOrder = { one: false, two: false };
  createForm: FormGroup;
  isEditForm: boolean = false;
  get r() { return this.createForm; }

  constructor(private masterService: MasterService, private fb: FormBuilder, private alertify: AlertifyService, private router: Router) { }

  ngOnInit() {
    this.InitiateForm();
    this.getItemCategoryList();
  }

  getItemCategoryList() {
    this.masterService.getItemCategories().subscribe(result => {
      this.itemCategories = result;
      this.itemCategories.sort((a, b) => b.id - a.id);

    }, error => {
      this.alertify.error(error);
    });
  }
  InitiateForm() {
    this.createForm = this.fb.group({
      code: ['', [Validators.required, Validators.pattern('^[a-zA-Z]{4}')]],
      description: ['', Validators.required]
    });
  }

  createItemCategory() {
    if (this.createForm.valid) {
      this.itemCategory = Object.assign({}, this.createForm.getRawValue());

      if (this.isEditForm === false) {
        this.masterService.CreateItemCategory(this.itemCategory).subscribe(() => {
          this.alertify.success('Successfully created Item Category');
          this.ngOnInit();
        }, error => {
          if (error.status === 400) {
            this.alertify.error('item category already exist');
          } else {
            this.alertify.error('could\'nt create item category');
          }
        }, () => {
          this.addModal.hide();
        });
      } else {
        this.itemCategory.id = this.itemCategoryID;
        this.masterService.updateItemCategory(this.itemCategoryID, this.itemCategory).subscribe(() => {
          this.alertify.success('Item Category updated successfully');
          this.ngOnInit();
        }, error => {
          if (error.status === 400) {
            this.alertify.error('item category already exist');
          } else {
            this.alertify.error('could\'nt update item category');
          }
        },
          () => {
            this.addModal.hide();
          });
      }

      this.createForm.reset();
      // this.router.navigate(['/master/itemCategory']);
    }
  }

  addItemCategory() {
    this.createForm.reset();
    this.addModal.show();
    this.isEditForm = false;
  }

  delete(id: number) {
    this.alertify.confirm('Are you sure?',
      'Are you sure you want to delete this item Category? This action cannot be undone',
      () => {
        this.masterService.deleteItemCategory(id).subscribe((next) => {
          this.alertify.success('Item Category deleted succesfully');
          this.ngOnInit();
        }, () => {
          this.alertify.error('Failed to Delete Item Category');
          this.alertify.error('Maybe this category is associated with some items, check and try again');
        });
      },
      () => { });
  }
  editItemCategory(id: number) {
    this.isEditForm = true;
    this.itemCategoryID = id;

    this.masterService.getItemCategory(this.itemCategoryID).subscribe(result => {
      this.createForm.patchValue({
        code: result.code,
        description: result.description
      });
    }, error => {
      this.alertify.error(error);
    });
    this.addModal.show();
  }

  ShowItemCategoryInfo(id: number) {
    this.itemCategoryInfo = this.itemCategories.find(a => a.id === id);
    this.infoModal.show();
  }

  sort(propertyNumber: number) {

    switch (propertyNumber) {
      case 1:
        this.itemCategories.sort(
          (a, b) => this.sortOrder.one === false ? a.code.localeCompare(b.code) : b.code.localeCompare(a.code)
        );
        this.sortOrder.one = !this.sortOrder.one;
        break;
      case 2:
        this.itemCategories.sort(
          (a, b) => this.sortOrder.two === false ?
            a.description.localeCompare(b.description) :
            b.description.localeCompare(a.description)
        );
        this.sortOrder.two = !this.sortOrder.two;
        break;
      default:
        this.itemCategories.sort(
          (a, b) => this.sortOrder.one === false ? a.code.localeCompare(b.code) : b.code.localeCompare(a.code)
        );
        this.sortOrder.one = !this.sortOrder.one;
        break;
    }
  }

}
