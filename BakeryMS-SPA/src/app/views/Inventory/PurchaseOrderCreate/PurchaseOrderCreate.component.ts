import { Component, OnInit, ɵConsole } from '@angular/core';
import { FormArray, FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { Observable } from 'rxjs';
import { ItemForDropdown } from '../../../_models/item';
import { PODRow, PurchaseOrderDetail, PurchaseOrderHeader } from '../../../_models/purchaseOrder';
import { SupplierForDropdown } from '../../../_models/supplier';
import { AlertifyService } from '../../../_services/alertify.service';
import { AuthService } from '../../../_services/auth.service';
import { InventoryService } from '../../../_services/inventory.service';
import { MasterService } from '../../../_services/master.service';
import { UtilityService } from '../../../_services/utility.service';

@Component({
  // tslint:disable-next-line: component-selector
  selector: 'app-PurchaseOrderCreate',
  templateUrl: './PurchaseOrderCreate.component.html',
  styleUrls: ['./PurchaseOrderCreate.component.scss']
})
export class PurchaseOrderCreateComponent implements OnInit {

  purchaseOrder: PurchaseOrderHeader;
  pOCreateForm: FormGroup;
  pOdetailRowList: PODRow[] = [];
  suppliers: SupplierForDropdown[] = [];
  items: ItemForDropdown[] = [];
  columns: string[];
  deliveryMethods: string[];
  totalValue = 0;
  $totalRows = 0;
  type: number;

  get gettableRowArray(): FormArray {
    return this.pOCreateForm.get('poDetail') as FormArray;
  }

  constructor(private authService: AuthService,
    private alertify: AlertifyService,
    private fb: FormBuilder,
    private router: Router,
    private utiService: UtilityService,
    private inventoryService: InventoryService,
    private masterService: MasterService) {

    this.deliveryMethods = ['Direct', 'Pick Up', 'Shipping'];
    this.columns = ['Item Code', 'Item Name', 'Due Date', 'Quantity', 'Unit Price (Rs)', 'Line Total (Rs)', 'Actions'];

  }

  ngOnInit() {

    if (this.authService.isUserAdmin()) {
      this.type = null;
    } else if (this.authService.isUserBakeryManager()) {
      this.type = 2;
    } else if (this.authService.isUserOutletManager()) {
      this.type = 1;
    } else {
      this.type = null;
    }

    this.masterService.getSuppliers(true, this.type).subscribe(result => {
      this.suppliers = result;

    }, error => {
      this.alertify.error(error);
    });

    this.masterService.getItems(true, this.type).subscribe(result => {
      this.items = result;

    }, error => {
      this.alertify.error(error);
    });

    this.createPurchaseOrderForm();

    this.setInitialValues(this.pOCreateForm);

  }



  createPurchaseOrderForm() {
    this.pOCreateForm = this.fb.group({
      // suppArray: [],
      supplierId: ['', Validators.required],
      userId: ['', Validators.required],
      deliveryMethod: ['', Validators.required],
      orderDate: ['', Validators.required],
      deliveryDate: [null, Validators.required],
      username: [{ value: '', disabled: true }, Validators.required],
      poDetail: this.fb.array([
        this.initiatePodRowValues()
      ])
    });
  }

  setInitialValues(g: FormGroup) {
    const decodedToken = this.authService.decodedToken;
    const userDetails: string = this.utiService.titleCase(decodedToken.unique_name) + ' - ' + decodedToken.role.join(', ');
    const userID: number = decodedToken.nameid;
    g.patchValue({
      username: userDetails,
      userId: userID,
      orderDate: this.utiService.currentDate(),
      deliveryDate: this.utiService.addDate(new Date(), 3),
      deliveryMethod: this.deliveryMethods[0]
      // , suppArray: this.suppliers
    });
  }
  createPO() {
    // set status,
    this.purchaseOrder = Object.assign({}, this.pOCreateForm.getRawValue());
    console.log(this.purchaseOrder);
    this.inventoryService.CreatePurchaseOrder(this.purchaseOrder).subscribe(() => {
      this.alertify.success('successfully Created');
    }, error => {
      this.alertify.error('failed to create');
      this.alertify.error('Some error occured :' + error.error);
    }, () => {
      this.backToList();
    });

    // this.alertify.success('submit clicked');
  }

  initiatePodRowValues(): FormGroup {

    const formRow = this.fb.group({
      itemid: new FormControl('', {
        validators: [Validators.required]
      }),
      dueDate: new FormControl(this.utiService.currentDate(), {
        validators: [Validators.required]
      }),
      orderQty: new FormControl(0, {
        validators: [Validators.required, Validators.pattern(/^\d+\.\d{2}$/)]
      }),
      unitPrice: new FormControl(0, {
        validators: [Validators.required, Validators.pattern(/^\d+\.\d{2}$/)]
      }),
      lineTotal: new FormControl({ value: 0, disabled: true }, {
        validators: [Validators.pattern(/^\d+\.\d{2}$/)]
      })
    },
    );
    return formRow;

  }

  ItemSelectedValidator(g: FormGroup, value: any) {
    g.patchValue({
      itemid: value
    });

  }

  addNewRow() {
    this.gettableRowArray.push(this.initiatePodRowValues());
  }
  ClearRows() {

    this.gettableRowArray.clear();
    this.totalValue = 0;
    this.addNewRow();
  }

  onDeleteRow(rowIndex: number): void {

    this.gettableRowArray.removeAt(rowIndex);
  }

  getTotal(g: FormGroup, i: number) {

    const formArray = this.gettableRowArray;
    this.totalValue = 0;
    formArray.getRawValue().forEach(x => {
      x.lineTotal = parseFloat(x.orderQty) * parseFloat(x.unitPrice);
      this.totalValue += +x.lineTotal;
    });

    g.patchValue({
      lineTotal: g.get('orderQty').value * g.get('unitPrice').value
    });
  }

  supplierChange(supId: number, sups: any) {

    for (let i = 0; i < sups.length; i++) {
      if (sups[i].id === supId) {
        console.log(sups[i]);

      }
    }
  }

  backToList() {
    this.router.navigateByUrl('/inventory/purchaseOrder');
  }
}
