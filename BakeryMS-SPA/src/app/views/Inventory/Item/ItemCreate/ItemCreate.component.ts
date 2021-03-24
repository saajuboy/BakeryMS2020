import { Component, OnInit } from '@angular/core';
import { async } from '@angular/core/testing';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { Item, ItemCategory, Unit } from '../../../../_models/item';
import { AlertifyService } from '../../../../_services/alertify.service';
import { AuthService } from '../../../../_services/auth.service';
import { MasterService } from '../../../../_services/master.service';

@Component({
  // tslint:disable-next-line: component-selector
  selector: 'app-ItemCreate',
  templateUrl: './ItemCreate.component.html',
  styleUrls: ['./ItemCreate.component.scss']
})
export class ItemCreateComponent implements OnInit {

  item: Item;
  createForm: FormGroup;
  isEditForm: boolean = false;
  itemID: number;
  itemCategories: ItemCategory[];
  units: Unit[];
  itemCodeForEdit = { itemCategoryId: 0, code: '' };

  isProductionType: boolean = false;
  get r() { return this.createForm; }

  constructor(private authService: AuthService,
    private alertify: AlertifyService,
    private fb: FormBuilder,
    private router: Router,
    private route: ActivatedRoute,
    private masterService: MasterService) { }

  ngOnInit() {
    this.InitiateForm();

    this.masterService.getItemCategories().subscribe(
      (result) => {
        this.itemCategories = result;
      }, error => {
        this.alertify.error(error);
      }
    );

    this.masterService.getUnits().subscribe(
      (result) => { this.units = result; },
      (error) => { this.alertify.error(error); }
    );

    // for edit Form
    this.route.paramMap.subscribe(params => {
      const itmID = +params.get('id');

      if (itmID) {
        this.getItem(itmID);
        this.isEditForm = true;
        this.itemID = itmID;
      }
    });
  }

  InitiateForm() {
    this.createForm = this.fb.group({
      name: ['', [Validators.required, Validators.pattern('^[a-zA-Z0-9 -]+')]],
      code: [{ value: '', disabled: true }, Validators.required],
      description: [''],
      itemCategory: ['', Validators.required],
      unit: ['', Validators.required],
      type: ['', Validators.required],
      sellingPrice: ['', Validators.pattern('^[0-9.]+')],
      expireDays: ['', Validators.pattern('^[0-9]+')]
    }, { validators: this.priceAndDateValidator });
  }

  priceAndDateValidator(g: FormGroup) {
    if (+g.get('type').value === 0) {

      const sellPrice: string = g.get('sellingPrice').value;
      const expDays: string = g.get('expireDays').value;

      if (+sellPrice === 0) {
        return { priceRequired: true };
      }
      if (+expDays === 0) {
        return { daysRequired: true };
      }
    }
    return null;
  }

  createItem() {
    if (this.createForm.valid) {
      this.item = Object.assign({}, this.createForm.getRawValue());
      this.item.itemCategory = <any>{ id: 0, code: 'ABCD', description: 'ABCD' }; // code and description ADDED to avoid validation from API
      this.item.unit = <any>{ id: 0 };

      this.item.itemCategory.id = this.createForm.controls['itemCategory'].value;
      this.item.unit.id = this.createForm.controls['unit'].value;

      if (this.item.type !== 0) {
        this.item.sellingPrice = 0;
        this.item.expireDays = 0;
      }

      if (this.isEditForm === false) {
        this.masterService.CreateItem(this.item).subscribe(() => {
          this.alertify.success('Successfully created Item ');
        }, error => {
          this.alertify.error(error.error);
        }, () => {
          this.router.navigate(['/master/item']);
        });
      } else {
        this.item.id = this.itemID;
        this.masterService.updateItem(this.itemID, this.item).subscribe(() => {
          this.alertify.success('Item updated successfully');
        }, error => {
          this.alertify.error(error.error);
        },
          () => {
            this.router.navigate(['/master/item']);
          });
        // this.alertify.success('updated Succes');
      }
    }

    // console.log(this.item);


  }

  getItem(id: number) {
    this.masterService.getItem(id).subscribe(
      (item: Item) => this.createEditItemForm(item),
      (error: any) => {
        console.log(error);
        this.alertify.error('some error occured');
        this.router.navigate(['/master/item']);
      }
    );
  }

  createEditItemForm(item: Item) {
    this.createForm.patchValue({
      name: item.name,
      code: item.code,
      description: item.description,
      type: item.type,
      itemCategory: item.itemCategory.id,
      unit: item.unit.id,
      sellingPrice: item.sellingPrice,
      expireDays: item.expireDays
    });

    this.itemCodeForEdit.itemCategoryId = item.itemCategory.id;
    this.itemCodeForEdit.code = item.code;

    this.isProductionType = item.type === 0 ? true : false;

  }

  backToList() {
    this.router.navigate(['/master/item']);
  }

  setCode(id) {

    if (this.isEditForm) {
      // tslint:disable-next-line: triple-equals
      if (id == this.itemCodeForEdit.itemCategoryId) {
        this.r.patchValue({ code: this.itemCodeForEdit.code });
        return null;
      }

    }
    this.masterService.getItemCode(id).subscribe(
      (result: any) => {
        const Code = result;
        this.r.patchValue({ code: Code.code });
      }, (error) => { console.log(error); });
  }


  typeChanged(type: any) {
    if (+type === 0) {
      this.isProductionType = true;
    } else {
      this.isProductionType = false;
    }
  }
}
