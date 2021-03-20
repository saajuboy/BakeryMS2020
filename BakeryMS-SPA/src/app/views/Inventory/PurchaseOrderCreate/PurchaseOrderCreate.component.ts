import { Component, OnInit, ɵConsole } from '@angular/core';
import { FormArray, FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { BusinessPlace } from '../../../_models/businessPlace';
import { ItemForDropdown } from '../../../_models/item';
import { PurchaseOrderDetail, PurchaseOrderHeader } from '../../../_models/purchaseOrder';
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
  suppliers: SupplierForDropdown[] = [];
  items: ItemForDropdown[] = [];
  businessPlaces: BusinessPlace[] = [];
  columns: string[];
  deliveryMethods: string[];
  totalValue = 0;
  $totalRows = 0;
  type: number;
  isEditForm: boolean = false;
  PurchaseOrderID: number;

  get gettableRowArray(): FormArray {
    return this.pOCreateForm.get('poDetail') as FormArray;
  }

  constructor(private authService: AuthService,
    private alertify: AlertifyService,
    private fb: FormBuilder,
    private router: Router,
    private route: ActivatedRoute,
    private utiService: UtilityService,
    private inventoryService: InventoryService,
    private masterService: MasterService) {

    this.deliveryMethods = ['Direct', 'Pick Up', 'Shipping'];
    this.columns = ['Item Code', 'Item Name', 'Due Date', 'Quantity', 'Unit Price (Rs)', 'Line Total (Rs)', 'Actions'];

  }

  async ngOnInit() {

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

    this.masterService.getBusinessPlaces().subscribe(result => {
      this.businessPlaces = result.filter((x) => {
        if (this.type == null || this.type === 0) {
          return x;
        } else if (this.type === 1) {
          return x.name.includes('Outlet');
        } else if (this.type === 2) {
          return x.name.includes('Bakery');
        }
      });

    }, error => {
      this.alertify.error(error);
    });

    this.createPurchaseOrderForm();

    this.setInitialValues(this.pOCreateForm);

    // for edit Form
    this.route.paramMap.subscribe(params => {
      const pOID = +params.get('id');

      if (pOID) {
        this.getPurchaseOrder(pOID);
        this.isEditForm = true;
        this.PurchaseOrderID = pOID;
      }
    });

  }

  createPurchaseOrderForm() {
    this.pOCreateForm = this.fb.group({
      // suppArray: [],
      supplierId: ['', Validators.required],
      businessPlaceId: ['', Validators.required],
      userId: ['', Validators.required],
      deliveryMethod: ['', Validators.required],
      orderDate: ['', Validators.required],
      deliveryDate: [null, Validators.required],
      username: [{ value: '', disabled: true }, Validators.required],
      poDetail: this.fb.array([
        this.initiatePodRowValues()
      ])
    }, { validators: this.datesValidator(this.utiService) });
  }

  datesValidator(uti: UtilityService) {
    return (g: FormGroup) => {
      const today = uti.currentDate();
      // console.log(uti.currentDate());
      if (Date.parse(g.get('orderDate').value) > Date.parse(g.get('deliveryDate').value)) {
        return { greaterThanDelivery: true };
      }
      if (Date.parse(g.get('orderDate').value) < Date.parse(today)) {
        return { lessThanToday: true };
      }
      if (Date.parse(g.get('deliveryDate').value) < Date.parse(today)) {
        return { deliveryLessThanToday: true };
      }
      return null;
    };
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

  getPurchaseOrder(id: number) {
    this.inventoryService.getPurchaseOrder(id).subscribe(
      (purchaseOrder: PurchaseOrderHeader) => {
        this.createEditPOForm(purchaseOrder);
        // console.log(purchaseOrder);

      },
      (error: any) => {
        // console.log(error);
        this.alertify.error('some error occured');
        this.router.navigate(['/inventory/purchaseOrder']);
      }
    );
  }
  createEditPOForm(purchaseOrder: PurchaseOrderHeader) {
    this.pOCreateForm.patchValue({
      supplierId: purchaseOrder.supplierId,
      businessPlaceId: purchaseOrder.businessPlaceId,
      userId: this.authService.decodedToken.nameid,
      deliveryMethod: purchaseOrder.deliveryMethod,
      orderDate: purchaseOrder.orderDate,
      deliveryDate: purchaseOrder.deliveryDate,
      username:
        this.utiService.titleCase(this.authService.decodedToken.unique_name) + ' - ' + this.authService.decodedToken.role.join(', '),
      // poDetail: this.fb.array([
      //   this.initiatePodRowValues()
      // ])
    });
    this.ClearRows();
    this.onDeleteRow(0);
    this.totalValue = 0;
    purchaseOrder.poDetail.forEach((podRow) => {
      this.gettableRowArray.push(this.initiateEditPodRowValues(podRow));
      this.totalValue = this.totalValue + podRow.lineTotal;
    });


  }
  async createPO(isForSending: boolean) {
    // set status,
    if (this.pOCreateForm.valid) {

      this.purchaseOrder = Object.assign({}, this.pOCreateForm.getRawValue());

      this.masterService.getSupplier(this.purchaseOrder.supplierId).subscribe((result) => {
        if (result.type === 2) {
          this.purchaseOrder.isForOutlet = false;
        } else if (result.type === 1) {
          this.purchaseOrder.isForOutlet = true;
        }

        // console.log(this.purchaseOrder);

        if (this.isEditForm === false) {
          if (isForSending === false) {
            this.inventoryService.createPurchaseOrder(this.purchaseOrder).subscribe(() => {
              this.alertify.success('successfully Created');
            }, error => {
              this.alertify.error('failed to create');
              this.alertify.error('Some error occured :' + error.error);
            }, () => {
              this.backToList();
            });

          } else {

            this.inventoryService.createPurchaseOrderAndSend(this.purchaseOrder).subscribe((res) => {
              // if (res.hasOwnProperty('error')) {
              //   this.alertify.warning('PO was created but not sent');
              // } else {
              this.alertify.success('successfully Created and sent');
              // }
            }, error => {
              this.alertify.error('failed to create');
              this.alertify.error('Some error occured :' + error.error);
            }, () => {
              this.backToList();
            });
            // this.alertify.success('successfully saved and sent');
          }

        } else {

          if (isForSending === false) {
            this.inventoryService.updatePurchaseOrder(this.PurchaseOrderID, this.purchaseOrder).subscribe(() => {
              this.alertify.success('successfully Updated');
            }, error => {
              this.alertify.error('failed to Update');
              this.alertify.error('Some error occured :' + error.error);
            }, () => {
              this.backToList();
            });

          } else {
            this.inventoryService.updatePurchaseOrderAndSend(this.PurchaseOrderID, this.purchaseOrder).subscribe((res) => {
              // if (res) {
              //   this.alertify.warning('PO was created but not sent');
              // } else {
              this.alertify.success('successfully updated and sent');
              // }

              // handle email not sent

            }, error => {
              this.alertify.error('failed to update');
              this.alertify.error('Some error occured :' + error.error);
            }, () => {
              this.backToList();
            });
          }
        }

      }, (error) => {
        this.alertify.error('some error occured, try selecting supplier again or refresh');
      });
    }
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
  initiateEditPodRowValues(pORow: PurchaseOrderDetail): FormGroup {

    const formRow = this.fb.group({
      itemid: new FormControl(<any>pORow.itemId, {
        validators: [Validators.required]
      }),
      dueDate: new FormControl(pORow.dueDate, {
        validators: [Validators.required]
      }),
      orderQty: new FormControl(pORow.orderQty.toFixed(2), {
        validators: [Validators.required, Validators.pattern(/^\d+\.\d{2}$/)]
      }),
      unitPrice: new FormControl(pORow.unitPrice.toFixed(2), {
        validators: [Validators.required, Validators.pattern(/^\d+\.\d{2}$/)]
      }),
      lineTotal: new FormControl({ value: pORow.lineTotal.toFixed(2), disabled: true }, {
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

    // for (let i = 0; i < sups.length; i++) {
    //   if (sups[i].id === supId) {
    //     console.log(sups[i]);

    //   }
    // }
  }

  backToList() {
    this.router.navigateByUrl('/inventory/purchaseOrder');
  }
}
