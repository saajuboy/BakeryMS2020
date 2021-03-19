import { Component, OnInit } from '@angular/core';
import { FormArray, FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { ProductionOrderDetail, ProductionOrderHeader } from '../../../_models/productionOrder';
import { ProductionPlanHeader } from '../../../_models/productionPlan';
import { AlertifyService } from '../../../_services/alertify.service';
import { AuthService } from '../../../_services/auth.service';
import { ManufacturingService } from '../../../_services/manufacturing.service';
import { MasterService } from '../../../_services/master.service';
import { UtilityService } from '../../../_services/utility.service';

@Component({
  selector: 'app-ItemAcceptance',
  templateUrl: './ItemAcceptance.component.html',
  styleUrls: ['./ItemAcceptance.component.scss']
})
export class ItemAcceptanceComponent implements OnInit {

  planList: ProductionPlanHeader[] = [];
  ProdOrderList: any[] = [];
  selectedPlanId: number = 0;
  selectedProdOrdrId: number = 0;
  acceptanceForm: FormGroup;
  currentProdOrder: ProductionOrderHeader = <ProductionOrderHeader>{ productionOrderDetails: [] };

  get gettableRowArray(): FormArray {
    return this.acceptanceForm.get('items') as FormArray;
  }
  constructor(private authService: AuthService,
    private alertify: AlertifyService,
    private fb: FormBuilder,
    private router: Router,
    private utiService: UtilityService,
    private manufacturingService: ManufacturingService,
    private masterService: MasterService) { }


  ngOnInit() {
    this.createAcceptanceForm();
    this.getPlans();
  }

  createAcceptanceForm() {
    this.acceptanceForm = this.fb.group({
      // suppArray: [],
      productionOrderId: ['', Validators.required],
      items: this.fb.array([], Validators.required)
    });
  }
  CreateItemRowValues(detailRow: ProductionOrderDetail): FormGroup {

    const formRow = this.fb.group({
      itemId: new FormControl(<any>detailRow.itemId, {
        validators: [Validators.required]
      }),
      item: new FormControl(<any>detailRow.item, {
        validators: [Validators.required]
      }),
      requestedQuantity: new FormControl(detailRow.quantity.toFixed(2), {
        validators: [Validators.required]
      }),
      quantity: new FormControl(detailRow.quantity.toFixed(2), {
        validators: [Validators.required, Validators.pattern(/^\d+\.\d{2}$/)]
      })
    },
    );
    return formRow;

  }

  acceptItems() {
    this.alertify.confirm('Confirm?', 'This action is irreversible', () => {
      if (this.acceptanceForm.valid) {
        const obj = Object.assign({}, this.acceptanceForm.getRawValue());
        this.currentProdOrder.productionOrderDetails.splice(0, this.currentProdOrder.productionOrderDetails.length);
        obj.items.forEach(x => {
          this.currentProdOrder.productionOrderDetails.push(<ProductionOrderDetail>{ itemId: x.itemId, quantity: x.quantity });
        });
        console.log(this.currentProdOrder);
        this.manufacturingService.acceptItems(this.selectedPlanId, this.currentProdOrder).subscribe(() => {
          this.alertify.success('successfully accepted');
        }, (res) => {
          this.alertify.error('code ' + res.error.code + ': ' + res.error.message);
        }, () => {
          this.planList = [];
          this.ProdOrderList = [];
          this.selectedPlanId = 0;
          this.selectedProdOrdrId = 0;
          this.ngOnInit();
        });

      }
    }, () => { });
  }
  getPlans(callback?) {
    this.manufacturingService.getRecentProductionPlans().subscribe((result) => {
      this.planList = result;

      if (callback) {
        callback();
      }

    }, () => {
      this.alertify.error('some server error occured,pls try again');
    });
  }
  getProdOrders(id: number, callback?) {
    this.manufacturingService.getProductionOrdersForSelectedPlan(id).subscribe((result) => {
      this.ProdOrderList = result;

      if (callback) {
        callback();
      }

    }, () => {
      this.alertify.error('some server error occured,pls try again');
    });
  }
  getProdOrder(id: number, callback?) {
    this.manufacturingService.getProductionOrder(id).subscribe((result) => {
      this.currentProdOrder = result;

      if (callback) {
        callback();
      }

    }, () => {
      this.alertify.error('some server error occured,pls try again');
    });
  }
  planChange(id) {
    this.getProdOrders(id);
  }
  prodOrderChange(id) {
    this.getProdOrder(id, () => {
      this.acceptanceForm.patchValue({
        productionOrderId: id
      });
      this.gettableRowArray.clear();
      this.currentProdOrder.productionOrderDetails.forEach(x => {
        this.gettableRowArray.push(this.CreateItemRowValues(x));
      });

      console.log(this.gettableRowArray.getRawValue());

    });
  }
}
