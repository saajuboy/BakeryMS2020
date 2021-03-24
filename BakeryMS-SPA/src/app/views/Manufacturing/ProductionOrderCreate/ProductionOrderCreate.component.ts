import { Component, OnInit, ViewChild } from '@angular/core';
import { AbstractControl, FormArray, FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { BusinessPlace } from '../../../_models/businessPlace';
import { ItemForDropdown } from '../../../_models/item';
import { ProductionOrderDetail, ProductionOrderHeader, ProductionSession } from '../../../_models/productionOrder';
import { AlertifyService } from '../../../_services/alertify.service';
import { AuthService } from '../../../_services/auth.service';
import { ManufacturingService } from '../../../_services/manufacturing.service';
import { MasterService } from '../../../_services/master.service';
import { UtilityService } from '../../../_services/utility.service';

@Component({
  // tslint:disable-next-line: component-selector
  selector: 'app-ProductionOrderCreate',
  templateUrl: './ProductionOrderCreate.component.html',
  styleUrls: ['./ProductionOrderCreate.component.scss']
})
export class ProductionOrderCreateComponent implements OnInit {
  @ViewChild('infoModal') public infoModal: ModalDirective;

  productionOrder: ProductionOrderHeader;
  pOCreateForm: FormGroup;
  // pOdetailRowList: PODRow[] = [];
  businessPlaces: BusinessPlace[] = [];
  productionSessions: ProductionSession[] = [];
  items: ItemForDropdown[] = [];
  columns: string[];
  deliveryMethods: string[];
  totalValue = 0;
  // $totalRows = 0;
  type: number;
  isEditForm: boolean = false;
  ProductionOrderID: number;

  productionOrderList: ProductionOrderHeader[] = [];

  get gettableRowArray(): FormArray {
    return this.pOCreateForm.get('productionOrderDetails') as FormArray;
  }

  constructor(private authService: AuthService,
    private alertify: AlertifyService,
    private fb: FormBuilder,
    private router: Router,
    private route: ActivatedRoute,
    private utiService: UtilityService,
    private manufacturingService: ManufacturingService,
    private masterService: MasterService) {

    this.deliveryMethods = ['Direct', 'Pick Up', 'Shipping'];
    this.columns = ['Item Code', 'Item Name', 'Description', 'Quantity', 'Actions'];

  }

  async ngOnInit() {

    this.type = 0;
    this.masterService.getBusinessPlaces().subscribe(result => {
      this.businessPlaces = result;
    }, error => {
      this.alertify.error(error);
    });

    this.manufacturingService.getProductionSessions().subscribe(result => {
      this.productionSessions = result;
    }, error => {
      this.alertify.error(error);
    });

    this.masterService.getItems(true, this.type).subscribe(result => {
      this.items = result;

    }, error => {
      this.alertify.error(error);
    });

    this.createProductionOrderForm();

    this.setInitialValues(this.pOCreateForm);

    // for edit Form
    this.route.paramMap.subscribe(params => {
      const pOID = +params.get('id');

      if (pOID) {
        this.getProductionOrder(pOID);
        this.isEditForm = true;
        this.ProductionOrderID = pOID;
      }
    });

  }

  createProductionOrderForm() {
    this.pOCreateForm = this.fb.group({
      // suppArray: [],
      businessPlaceId: ['', Validators.required],
      userId: ['', Validators.required],
      sessionId: ['', Validators.required],
      requiredDate: ['', Validators.required],
      enteredDate: [null, Validators.required],
      userName: [{ value: '', disabled: true }, Validators.required],
      productionOrderDetails: this.fb.array([
        this.initiatePodRowValues()
      ])
    }, { validators: this.datesValidator(this.utiService) });
  }

  datesValidator(uti: UtilityService) {
    return (g: FormGroup) => {
      // console.log(Date.parse(g.get('enteredDate').value) > Date.parse(g.get('requiredDate').value));
      const today = uti.currentDate();
      // console.log(uti.currentDate());
      if (Date.parse(g.get('enteredDate').value) > Date.parse(g.get('requiredDate').value)) {
        return { greaterThanRequired: true };
      }
      if (Date.parse(g.get('enteredDate').value) < Date.parse(today)) {
        return { lessThanToday: true };
      }
      if (Date.parse(g.get('requiredDate').value) < Date.parse(today)) {
        return { requiredLessThanToday: true };
      }
      return null;
    };
  }

  setInitialValues(g: FormGroup) {
    const decodedToken = this.authService.decodedToken;
    const userDetails: string = this.utiService.titleCase(decodedToken.unique_name) + ' - ' + decodedToken.role.join(', ');
    const userID: number = decodedToken.nameid;
    g.patchValue({
      userName: userDetails,
      userId: userID,
      enteredDate: this.utiService.currentDate(),
      requiredDate: this.utiService.addDate(new Date(), 1)
    });
  }

  getProductionOrder(id: number) {
    this.manufacturingService.getProductionOrder(id).subscribe(
      (productionOrder: ProductionOrderHeader) => {
        this.createEditPOForm(productionOrder);
        console.log(productionOrder);
      },
      (error: any) => {
        console.log(error);
        this.alertify.error('some error occured');
        this.router.navigate(['/manufacturing/productionOrder']);
      }
    );
  }
  createEditPOForm(productionOrder: ProductionOrderHeader) {
    this.pOCreateForm.patchValue({
      sessionId: productionOrder.sessionId,
      userId: this.authService.decodedToken.nameid,
      businessPlaceId: productionOrder.businessPlaceId,
      enteredDate: productionOrder.enteredDate,
      requiredDate: productionOrder.requiredDate,
      userName:
        this.utiService.titleCase(this.authService.decodedToken.unique_name) + ' - ' + this.authService.decodedToken.role.join(', '),
    });
    this.ClearRows();
    this.onDeleteRow(0);
    this.totalValue = 0;
    productionOrder.productionOrderDetails.forEach((podRow) => {
      this.gettableRowArray.push(this.initiateEditPodRowValues(podRow));
      // this.totalValue = this.totalValue + podRow.lineTotal;
    });


  }

  async createPO() {
    // set status,
    if (this.pOCreateForm.valid) {

      this.productionOrder = Object.assign({}, this.pOCreateForm.getRawValue());
      console.log(this.productionOrder);

      if (this.isEditForm === false) {
        // if (isForSending === false) {
        this.manufacturingService.createProductionOrder(this.productionOrder).subscribe(() => {
          this.alertify.success('successfully Created');
        }, error => {
          this.alertify.error('failed to create');
          this.alertify.error('Some error occured :' + error.error);
        }, () => {
          this.backToList();
        });
      } else {
        this.manufacturingService.updateProductionOrder(this.ProductionOrderID, this.productionOrder).subscribe(() => {
          this.alertify.success('successfully Updated');
        }, error => {
          this.alertify.error('failed to Update');
          this.alertify.error('Some error occured :' + error.error);
        }, () => {
          this.backToList();
        });
      }
    }
  }

  initiatePodRowValues(): FormGroup {

    const formRow = this.fb.group({
      itemId: new FormControl('', {
        validators: [Validators.required]
      }),
      quantity: new FormControl(0, {
        validators: [Validators.required, Validators.pattern(/^\d+\.\d{2}$/)]
      }),
      description: new FormControl('Regular Order', {
        validators: [Validators.required]
      })
    },
    );
    return formRow;

  }
  initiateEditPodRowValues(pORow: ProductionOrderDetail): FormGroup {

    const formRow = this.fb.group({
      itemId: new FormControl(<any>pORow.itemId, {
        validators: [Validators.required]
      }),
      quantity: new FormControl(pORow.quantity.toFixed(2), {
        validators: [Validators.required, Validators.pattern(/^\d+\.\d{2}$/)]
      }),
      description: new FormControl(pORow.description, {
        validators: [Validators.required]
      })
    },
    );
    return formRow;

  }

  ItemSelectedValidator(g: FormGroup, value: any) {
    g.patchValue({
      itemId: value
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

  backToList() {
    this.router.navigateByUrl('/manufacturing/productionOrder');
  }

  getPreviousProdOrders() {
    this.manufacturingService.getProductionOrders().subscribe((result) => {
      this.productionOrderList = result.sort((a, b) => b.productionOrderNo - a.productionOrderNo);
    }, () => {
      this.alertify.error('failed To Process');
    }, () => {
      this.infoModal.show();
    });
  }

  retrievePO(id: number) {

    this.manufacturingService.getProductionOrder(id).subscribe(
      (productionOrder: ProductionOrderHeader) => {
        this.createEditPOForm(productionOrder);
        // console.log(productionOrder);
        this.pOCreateForm.patchValue({
          enteredDate: this.utiService.currentDate(),
          requiredDate: this.utiService.addDate(new Date(), 1),
        });
      },
      (error: any) => {
        console.log(error);
        this.alertify.error('some error occured');
        this.infoModal.hide();
      },
      () => this.infoModal.hide()
    );
  }

  autoGenerateProdOrder(sessionId: number, placeId: number, requiredDate) {
    this.manufacturingService.getAutoGeneratedProductionOrder(sessionId, placeId, requiredDate).subscribe(
      (productionOrder: ProductionOrderHeader) => {
        this.createEditPOForm(productionOrder);
      }, (res) => {
        const status = res.error.status;
        const code = res.error.code;
        const message = res.error.message;

        if (status === 400 && code !== 7) {
          this.alertify.error(message + ': error code - ' + code);
        } else if (status === 400 && code === 7) {
          this.alertify.warning(message + ': error code - ' + code);
        } else {
          this.alertify.error('some error occured try again');
        }
      }, () => {

      });
  }
}

