import { Component, OnInit } from '@angular/core';
import { FormArray, FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { NgWizardConfig, NgWizardService, StepChangedArgs, StepValidationArgs, STEP_STATE, THEME } from 'ng-wizard';
import { of } from 'rxjs';
import { GRNHeader, PurchaseOrderDetail, PurchaseOrderHeader } from '../../../_models/purchaseOrder';
import { AlertifyService } from '../../../_services/alertify.service';
import { AuthService } from '../../../_services/auth.service';
import { HumanResourceService } from '../../../_services/humanResource.service';
import { InventoryService } from '../../../_services/inventory.service';
import { ManufacturingService } from '../../../_services/manufacturing.service';
import { MasterService } from '../../../_services/master.service';
import { UtilityService } from '../../../_services/utility.service';

@Component({
  selector: 'app-GRN',
  templateUrl: './GRN.component.html',
  styleUrls: ['./GRN.component.scss']
})
export class GRNComponent implements OnInit {

  grnForm: FormGroup;
  purchaseOrderList: PurchaseOrderHeader[] = [];
  purchaseOrder: PurchaseOrderHeader = <PurchaseOrderHeader>{ poDetail: [] };
  grn: GRNHeader = <GRNHeader>{ grnDetails: [] };
  type: boolean = false;

  get getDetailArray(): FormArray {
    return this.grnForm.get('GRNDetails') as FormArray;
  }

  isValidTypeBoolean: boolean = true;

  stepStates = {
    normal: STEP_STATE.normal,
    disabled: STEP_STATE.disabled,
    error: STEP_STATE.error,
    hidden: STEP_STATE.hidden
  };


  config: NgWizardConfig = {
    selected: 0,
    theme: THEME.arrows,
    toolbarSettings: { toolbarExtraButtons: [] }
  };

  constructor(private ngWizardService: NgWizardService,
    private authService: AuthService,
    private alertify: AlertifyService,
    private fb: FormBuilder,
    private router: Router,
    private utiService: UtilityService,
    private manufacturingService: ManufacturingService,
    private invService: InventoryService,
    private masterService: MasterService) { }

  ngOnInit() {

    if (this.authService.isUserAdmin()) {
      this.type = null;
    } else if (this.authService.isUserBakeryManager()) {
      this.type = false;
    } else if (this.authService.isUserOutletManager()) {
      this.type = true;
    } else {
      this.type = null;
    }

    this.invService.getPurchaseOrders(this.type, 1).subscribe((res) => {
      this.purchaseOrderList = res;
      console.log(res);

    }, () => {
      this.alertify.error('some error occured, Contact admin');
    });

    this.createGRNForm();
  }
  createGRNForm() {
    this.grnForm = this.fb.group({
      purchaseOrderHeaderId: ['', Validators.required],
      receivedDate: [this.utiService.currentDate(), Validators.required],
      totalAmount: ['', [Validators.required, Validators.pattern('^[0-9.]+')]],
      paidAmount: ['', [Validators.required, Validators.pattern('^[0-9.]+')]],
      paymentMode: [0, Validators.required],
      GRNDetails: this.fb.array([], Validators.required),
    }, { validators: this.datesValidator(this.utiService) });
  }

  datesValidator(uti: UtilityService) {
    return (g: FormGroup) => {
      const today = uti.currentDate();
      if (Date.parse(g.get('receivedDate').value) < Date.parse(today)) {
        return { lessThanToday: true };
      }
      return null;
    };
  }

  CreateDetailRowValues(detailRow: PurchaseOrderDetail): FormGroup {

    const formRow = this.fb.group({
      itemId: new FormControl(<any>detailRow.itemId, {
        validators: [Validators.required]
      }),
      itemName: new FormControl(<any>detailRow.item, {
        validators: [Validators.required]
      }),
      quantity: new FormControl(detailRow.orderQty.toFixed(2), {
        validators: [Validators.required, Validators.pattern('^[0-9.]+')]
      }),
      unitPrice: new FormControl(detailRow.unitPrice.toFixed(2), {
        validators: [Validators.required, Validators.pattern('^[0-9.]+')]
      }),
      sellingPrice: new FormControl((detailRow.unitPrice * 1.1).toFixed(2), {}),
      lineTotal: new FormControl((detailRow.orderQty * detailRow.unitPrice).toFixed(2), {
        validators: [Validators.required]
      }),
      manufacturedDate: new FormControl(this.utiService.currentDate(), {
        validators: [Validators.required]
      }),
      expiredDate: new FormControl(this.utiService.addDate(new Date(), 20), {
        validators: [Validators.required]
      })
    },
    );
    return formRow;

  }

  createGRN() {
    if (this.grnForm.valid) {
      this.grn = Object.assign({}, this.grnForm.getRawValue());
      // console.log(this.grn);

      this.alertify.confirm('Are you sure?', 'this action cannot be reverted',
        () => {
          this.invService.createGRN(this.grn).subscribe(
            () => {
              this.alertify.success('successfully created');
            },
            (res) => {
              this.alertify.error('code ' + res.error.code + ' : ' + res.error.message);
            },
            () => {
              this.backToList();
            });
        },
        () => {
          this.alertify.warning('Cancelled');
        });
    }
  }

  purchaseOrderChanged(id) {
    this.invService.getPurchaseOrder(id).subscribe((res) => {
      this.purchaseOrder = res;
      let totalValue = 0;
      this.getDetailArray.clear();
      this.purchaseOrder.poDetail.forEach(x => {
        this.getDetailArray.push(this.CreateDetailRowValues(x));
        totalValue = totalValue + (x.orderQty * x.unitPrice);
      });
      this.grnForm.patchValue({
        totalAmount: totalValue.toFixed(2)
      });
    }, () => { });
  }

  qtyAndPriceChange() {
    let totalValue = 0;
    this.getDetailArray.controls.forEach(x => {

      const lintotal = (x.get('quantity').value * x.get('unitPrice').value).toFixed(2);
      x.patchValue({
        lineTotal: lintotal
      });
      totalValue = totalValue + (+lintotal);
    });

    this.grnForm.patchValue({
      totalAmount: totalValue.toFixed(2)
    });
  }

  paidAmountChanged() {
    const total = +this.grnForm.get('totalAmount').value;
    if (+this.grnForm.get('paidAmount').value === 0) {
      this.grnForm.patchValue({
        totalAmount: total
      });
    }
  }
  backToList() {
    this.router.navigate(['/inventory/purchaseOrder']);
  }
  showPreviousStep(event?: Event) {
    this.ngWizardService.previous();
  }

  showNextStep(event?: Event) {
    this.ngWizardService.next();
  }

  resetWizard(event?: Event) {
    this.grnForm.reset();
    this.ngOnInit();
    this.ngWizardService.reset();

  }

  setTheme(theme: THEME) {
    this.ngWizardService.theme(theme);
  }

  stepChanged(args: StepChangedArgs) {

    // args.step.index === 1
    // console.log(args.step);
  }
  checkStepState(step: number) {
    const grnForm = this.grnForm;
    const stepStates = this.stepStates;
    const mainInfoValid = grnForm.get('purchaseOrderHeaderId').valid &&
      grnForm.get('receivedDate').valid &&
      !grnForm.hasError('lessThanToday') &&
      this.getDetailArray.length > 0;
    const subInfovalid = grnForm.get('totalAmount').valid &&
      grnForm.get('paidAmount').valid &&
      grnForm.get('paymentMode').valid;
    switch (step) {
      case 1:
        return mainInfoValid
          ? stepStates.normal
          : stepStates.error;
      case 2:
        return mainInfoValid
          ? subInfovalid
            ? stepStates.normal
            : stepStates.error
          : stepStates.normal;
      default:
        break;
    }
  }

  isValidFunctionReturnsBoolean(isValid: boolean) {
    return isValid ? true : false;
  }

  isValidFunctionReturnsObservable(args: StepValidationArgs) {
    return of(true);
  }

}
