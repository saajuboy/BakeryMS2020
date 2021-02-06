import { Component, OnInit, ɵConsole } from '@angular/core';
import { FormArray, FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { Observable } from 'rxjs';
import { ItemForDropdown } from '../../../_models/item';
import { PODRow, PurchaseOrderDetail, PurchaseOrderHeader } from '../../../_models/purchaseOrder';
import { SupplierForDropdown } from '../../../_models/supplier';
import { AlertifyService } from '../../../_services/alertify.service';
import { AuthService } from '../../../_services/auth.service';
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


  get gettableRowArray(): FormArray {
    return this.pOCreateForm.get('tableRowArray') as FormArray;
  }

  constructor(private authService: AuthService,
    private alertify: AlertifyService,
    private fb: FormBuilder,
    private router: Router,
    private utiService: UtilityService,
    private masterService: MasterService) {

    this.deliveryMethods = ['Direct', 'Pick Up', 'Shipping'];
    this.columns = ['Item Code', 'Item Name', 'Due Date', 'Quantity', 'Unit Price', 'Line Total', 'Actions'];

  }

  ngOnInit() {

    this.masterService.getSuppliers(true).subscribe(result => {
      this.suppliers = result;

    }, error => {
      this.alertify.error(error);
    });

    this.masterService.getItems(true).subscribe(result => {
      this.items = result;

    }, error => {
      this.alertify.error(error);
    });
    // this.getSuppliers().then((sups) => {
    //   this.suppliers = sups;
    //   console.log('inside subscribe:' + this.suppliers);

    //   console.log('outside subscribe:' + this.suppliers);
    // });

    this.createPurchaseOrderForm();

    this.setInitialValues(this.pOCreateForm);

  }



  createPurchaseOrderForm() {
    this.pOCreateForm = this.fb.group({
      // suppArray: [],
      supplierId: ['', Validators.required],
      deliveryMethod: ['', Validators.required],
      orderDate: ['', Validators.required],
      deliveryDate: [null, Validators.required],
      username: [{ value: '', disabled: true }, Validators.required],
      tableRowArray: this.fb.array([
        this.initiatePodRowValues()
      ])
    });
  }

  setInitialValues(g: FormGroup) {
    const decodedToken = this.authService.decodedToken;
    const userDetails: string = this.utiService.titleCase(decodedToken.unique_name) + ' - ' + decodedToken.role.join(', ');
    g.patchValue({
      username: userDetails,
      orderDate: this.utiService.currentDate(),
      deliveryDate: this.utiService.addDate(new Date(), 3),
      deliveryMethod: this.deliveryMethods[0]
      // , suppArray: this.suppliers
    });
  }
  createPO() {
    this.totalValue = this.totalValue + 1;
    this.alertify.success('submit clicked');
  }

  initiatePodRowValues(): FormGroup {

    const formRow = this.fb.group({
      itemCodeId: new FormControl('', {
        validators: [Validators.required]
      }),
      itemNameId: new FormControl('', {
        validators: [Validators.required]
      }),
      dueDate: new FormControl(this.utiService.currentDate(), {
        validators: [Validators.required]
      }),
      quantity: new FormControl(0, {
        validators: [Validators.required, Validators.pattern(/^\d+\.\d{2}$/)]
      }),
      unitPrice: new FormControl(0, {
        validators: [Validators.required, Validators.pattern(/^\d+\.\d{2}$/)]
      }),
      lineTotal: new FormControl({ value: 0, disabled: true }, {
        validators: [Validators.pattern(/^\d+\.\d{2}$/)]
      })
    },
      // { validators: this.ItemSelectedValidator }
    );

    // formRow.patchValue({});
    return formRow;

  }

  ItemSelectedValidator(g: FormGroup, value: any) {
    g.patchValue({
      itemNameId: value,
      itemCodeId: value
    });
    // return g.get('itemCodeId').value === g.get('itemNameId').value ? null : { mismatch: true };
    // }

  }

  addNewRow() {
    this.gettableRowArray.push(this.initiatePodRowValues());
  }
  ClearRows() {
    // this.gettableRowArray.push(this.initiatePodRowValues());
    this.gettableRowArray.clear();
    this.addNewRow();
  }

  onDeleteRow(rowIndex: number): void {

    this.gettableRowArray.removeAt(rowIndex);
  }

  getTotal(g: FormGroup) {
    const formArray = this.gettableRowArray;
    this.totalValue = 0;
    formArray.getRawValue().forEach(x => {
      x.lineTotal = parseFloat(x.quantity) * parseFloat(x.unitPrice);
      this.totalValue += +x.lineTotal;
    });
  }

  backToList() {
    this.router.navigateByUrl('/inventory/purchaseOrder');
  }
}
