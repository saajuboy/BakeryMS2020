import { Component, OnInit } from '@angular/core';
import { FormArray, FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { NgWizardConfig, NgWizardService, StepChangedArgs, StepValidationArgs, STEP_DIRECTIN, STEP_STATE, THEME } from 'ng-wizard';
import { of, pipe } from 'rxjs';
import { BusinessPlace } from '../../../_models/businessPlace';
import { Employee } from '../../../_models/employee';
import { Machinery } from '../../../_models/machinery';
import { ProductionOrderHeader, ProductionSession } from '../../../_models/productionOrder';
import { ProductionPlanDetail, ProductionPlanDetailList, ProductionPlanHeader, ProductionPlanMachine, ProductionPlanRecipe, ProductionPlanWorker } from '../../../_models/productionPlan';
import { AlertifyService } from '../../../_services/alertify.service';
import { AuthService } from '../../../_services/auth.service';
import { HumanResourceService } from '../../../_services/humanResource.service';
import { ManufacturingService } from '../../../_services/manufacturing.service';
import { MasterService } from '../../../_services/master.service';
import { UtilityService } from '../../../_services/utility.service';

@Component({
  // tslint:disable-next-line: component-selector
  selector: 'app-ProductionPlanCreate',
  templateUrl: './ProductionPlanCreate.component.html',
  styleUrls: ['./ProductionPlanCreate.component.scss']
})
export class ProductionPlanCreateComponent implements OnInit {

  isEditForm: boolean = false;
  planCreateForm: FormGroup;
  productionPlanId: number;
  productionPlan: ProductionPlanHeader;
  productionPlanforEdit: ProductionPlanHeader;
  businessPlaces: BusinessPlace[] = [];
  productionSessions: ProductionSession[] = [];
  productionOrdersAvailable: ProductionOrderHeader[] = [];
  workersAvailable: Employee[] = [];
  machineriesAvailable: Machinery[] = [];
  planDetailList: ProductionPlanDetail[] = [];
  planRecipeList: ProductionPlanRecipe[] = [];
  planWorkerList: ProductionPlanWorker[] = [];
  planMachineList: ProductionPlanMachine[] = [];

  isPoSelectAll: boolean = false;
  isEmpSelectAll: boolean = false;
  isMchnSelectAll: boolean = false;

  get getDetailArray(): FormArray {
    return this.planCreateForm.get('productionPlanDetails') as FormArray;
  }
  get getRecipeArray(): FormArray {
    return this.planCreateForm.get('productionPlanRecipes') as FormArray;
  }
  get getWorkersArray(): FormArray {
    return this.planCreateForm.get('productionPlanWorkers') as FormArray;
  }
  get getMachineArray(): FormArray {
    return this.planCreateForm.get('productionPlanMachines') as FormArray;
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
    toolbarSettings: {
      toolbarExtraButtons: [
        // { text: 'Reset', class: 'btn btn-warning', event: () => { this.resetWizard(); } }
      ]
    }
  };

  constructor(private ngWizardService: NgWizardService,
    private authService: AuthService,
    private alertify: AlertifyService,
    private fb: FormBuilder,
    private router: Router,
    private route: ActivatedRoute,
    private utiService: UtilityService,
    private manufacturingService: ManufacturingService,
    private hrService: HumanResourceService,
    private masterService: MasterService) {
  }

  ngOnInit() {

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

    this.createProductionPlanForm();

    this.setInitialValues(this.planCreateForm);

    this.route.paramMap.subscribe(params => {
      const planID = +params.get('id');

      if (planID) {
        this.getProductionPlan(planID);
        this.isEditForm = true;
        this.productionPlanId = planID;
        this.showNextStep();
      }
    });
  }


  async createPlan() {
    // set status,
    if (this.planCreateForm.valid) {

      this.productionPlan = Object.assign({}, this.planCreateForm.getRawValue());
      this.productionPlan.prodOrdrIds = [];
      this.productionOrdersAvailable.forEach(x => {
        if (x.isChecked) {
          this.productionPlan.prodOrdrIds.push(x.id);
        }
      });
      // console.log(this.productionPlan);

      if (this.isEditForm === false) {
        // if (isForSending === false) {
        this.manufacturingService.createProductionPlan(this.productionPlan).subscribe(() => {
          this.alertify.success('successfully Created');
        }, error => {
          this.alertify.error('failed to create');
          this.alertify.error('Some error occured :' + error.error);
        }, () => {
          this.backToList();
        });
      } else {
        this.manufacturingService.updateProductionPlan(this.productionPlanId, this.productionPlan).subscribe(() => {
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

  getProductionPlan(id: number) {
    this.manufacturingService.getProductionPlan(id).subscribe(
      (productionPlan: ProductionPlanHeader) => {
        this.createEditPlanForm(productionPlan, this.planCreateForm);
        this.productionPlanforEdit = productionPlan;
        // console.log(productionPlan);
      },
      (error: any) => {
        console.log(error);
        this.alertify.error('some error occured');
        this.router.navigate(['/manufacturing/productionPlan']);
      }
    );
  }
  createEditPlanForm(productionPlan: ProductionPlanHeader, g: FormGroup) {
    const decodedToken = this.authService.decodedToken;
    const userDetails: string = this.utiService.titleCase(decodedToken.unique_name) + ' - ' + decodedToken.role.join(', ');
    const userID: number = decodedToken.nameid;
    g.patchValue({
      userName: userDetails,
      userId: userID,
      date: productionPlan.date,
      description: productionPlan.description,
      businessPlaceId: productionPlan.businessPlaceId,
      productionSessionId: productionPlan.productionSessionId
    });

    // g.get('businessPlaceId').disable();
    // g.get('productionSessionId').disable();
    // g.get('date').disable();
    this.getProdOrders(() => {
      this.productionOrdersAvailable.forEach(x => {
        if (productionPlan.prodOrdrIds.some(pID => pID === x.id)) {
          x.isChecked = true;
        }
      });
    });
    productionPlan.productionPlanDetails.forEach(x => {
      this.getDetailArray.push(this.CreateDetailRowValues(x));
    });

    productionPlan.productionPlanRecipes.forEach(y => {
      this.getRecipeArray.push(this.CreateDetailRowValues(y));
    });

    this.getEmployees(() => {
      this.workersAvailable.forEach(x => {
        if (productionPlan.productionPlanWorkers.some(emp => emp.employeeId === x.id)) {
          x.isChecked = true;
        }
      });
    });
    productionPlan.productionPlanWorkers.forEach(y => {
      this.getWorkersArray.push(this.CreateWorkerRowValues(y));
    });

    this.getMachineries(() => {
      this.machineriesAvailable.forEach(x => {
        if (productionPlan.productionPlanMachines.some(mchn => mchn.machineryId === x.id)) {
          x.isChecked = true;
        }
      });
    });
    productionPlan.productionPlanMachines.forEach(y => {
      this.getMachineArray.push(this.CreateMachineRowValues(y));
    });

  }
  setInitialValues(g: FormGroup) {
    const decodedToken = this.authService.decodedToken;
    const userDetails: string = this.utiService.titleCase(decodedToken.unique_name) + ' - ' + decodedToken.role.join(', ');
    const userID: number = decodedToken.nameid;
    g.patchValue({
      userName: userDetails,
      userId: userID,
      date: this.utiService.addDate(new Date(), 1),
      description: 'Production Plan For ' +
        this.utiService.addDate(new Date(), 1) +
        ' by ' + this.utiService.titleCase(decodedToken.unique_name)
    });
  }
  createProductionPlanForm() {
    this.planCreateForm = this.fb.group({
      // suppArray: [],
      businessPlaceId: ['', Validators.required],
      userId: ['', Validators.required],
      productionSessionId: ['', Validators.required],
      date: ['', Validators.required],
      userName: [{ value: '', disabled: true }, Validators.required],
      description: [''],
      productionPlanDetails: this.fb.array([], Validators.required),
      productionPlanRecipes: this.fb.array([], Validators.required),
      productionPlanWorkers: this.fb.array([], Validators.required),
      productionPlanMachines: this.fb.array([], Validators.required)
    }, { validators: this.datesValidator(this.utiService) });
  }
  datesValidator(uti: UtilityService) {
    return (g: FormGroup) => {
      const today = uti.currentDate();
      if (Date.parse(g.get('date').value) < Date.parse(today)) {
        return { lessThanToday: true };
      }
      return null;
    };
  }

  CreateDetailRowValues(planRow: ProductionPlanDetail | ProductionPlanRecipe): FormGroup {

    const formRow = this.fb.group({
      itemId: new FormControl(<any>planRow.itemId, {
        validators: [Validators.required]
      }),
      itemName: new FormControl(<any>planRow.itemName, {
        validators: [Validators.required]
      }),
      quantity: new FormControl(planRow.quantity.toFixed(2), {
        validators: [Validators.required, Validators.pattern(/^\d+\.\d{2}$/)]
      }),
      description: new FormControl(planRow.description, {
        validators: [Validators.required]
      })
    },
    );
    return formRow;

  }

  CreateWorkerRowValues(workerRow: ProductionPlanWorker): FormGroup {

    const formRow = this.fb.group({
      employeeId: new FormControl(<any>workerRow.employeeId, {
        validators: [Validators.required]
      }),
      employeeName: new FormControl(<any>workerRow.employeeName, {
        validators: [Validators.required]
      })
    },
    );
    return formRow;
  }
  CreateMachineRowValues(machineRow: ProductionPlanMachine): FormGroup {

    const formRow = this.fb.group({
      machineryId: new FormControl(<any>machineRow.machineryId, {
        validators: [Validators.required]
      }),
      machineryName: new FormControl(<any>machineRow.machineryName, {
        validators: [Validators.required]
      })
    },
    );
    return formRow;
  }

  getProdOrders(onComplete?) {
    this.manufacturingService
      .getFilteredProductionOrders(this.planCreateForm.get('productionSessionId').value,
        this.planCreateForm.get('date').value, this.isEditForm).subscribe((result) => {
          this.productionOrdersAvailable = result;
          this.productionOrdersAvailable.forEach((a) => {
            a.sessionName = this.productionSessions.find(x => x.id === a.sessionId).session;
            a.businessPlaceName = this.businessPlaces.find(x => x.id === a.businessPlaceId).name;
          });
          if (this.productionOrdersAvailable.length === 0) {
            this.alertify.warning('No production order available for this session and date, try adding a production Order');
          }
          // console.log(this.productionOrdersAvailable);
        }, (res) => {
          if (res.error.status === 400) {
            this.alertify.error(res.error.message + ' : code' + res.error.code);
          }
        }, () => {
          if (onComplete) { onComplete(); }
        });
  }

  getEmployees(onComplete?) {
    this.hrService
      .getEmployeesForProductionPlan(this.planCreateForm.get('productionSessionId').value,
        this.planCreateForm.get('date').value, this.planCreateForm.get('businessPlaceId').value).subscribe((result) => {
          this.workersAvailable = result;
          // console.log(this.workersAvailable);

          if (this.workersAvailable.length === 0) {
            this.alertify.warning('No workers available for this session, date and place, try adding a routine schedule');
          }
        }, (res) => {
          if (res.error.status === 400) {
            this.alertify.error(res.error.message + ' : code' + res.error.code);
          }
        }, () => {
          if (onComplete) { onComplete(); }
        });
  }

  getMachineries(onComplete?) {
    this.masterService
      .getMachineries(this.planCreateForm.get('businessPlaceId').value).subscribe((result) => {
        this.machineriesAvailable = result;
        // console.log(this.machineriesAvailable);
        if (this.machineriesAvailable.length === 0) {
          this.alertify.warning('No machines available for this place, try adding a machine at this place');
        }
        // console.log(this.productionOrdersAvailable);
      }, (res) => {
        if (res.error.status === 400) {
          this.alertify.error(res.error.message + ' : code' + res.error.code);
        }
      }, () => {
        if (onComplete) { onComplete(); }
      });
  }

  selectAll(x: number) {
    switch (x) {
      case 1:
        this.isPoSelectAll = !this.isPoSelectAll;
        this.productionOrdersAvailable.forEach(x => {
          x.isChecked = this.isPoSelectAll;
        });
        this.pOChecked();
        break;
      case 2:
        this.isEmpSelectAll = !this.isEmpSelectAll;
        this.workersAvailable.forEach(x => {
          x.isChecked = this.isEmpSelectAll;
        });
        this.empChecked();
        break;
      case 3:
        this.isMchnSelectAll = !this.isMchnSelectAll;
        this.machineriesAvailable.forEach(x => {
          x.isChecked = this.isMchnSelectAll;
        });
        this.mchnChecked();
        break;
      default:
        break;
    }
  }
  pOChecked() {
    this.getDetailArray.clear();
    this.planDetailList = [];
    // console.log(this.productionOrdersAvailable);
    this.productionOrdersAvailable.forEach(ProdOH => {
      if (ProdOH.isChecked !== undefined && ProdOH.isChecked === true) {
        ProdOH.productionOrderDetails.forEach(detail => {
          const item = this.planDetailList.find(a => a.itemId === detail.itemId);
          if (item === undefined) {
            this.planDetailList.push(<ProductionPlanDetail>{
              itemId: detail.itemId,
              itemName: detail.item,
              description: detail.description,
              quantity: detail.quantity
            });
          } else {
            this.planDetailList.find(a => a.itemId === detail.itemId).quantity = item.quantity + detail.quantity;
          }
        });
      }
    });

    // console.log(this.planDetailList);

    this.planDetailList.forEach(x => {
      this.getDetailArray.push(this.CreateDetailRowValues(x));
    });


  }
  empChecked() {
    this.getWorkersArray.clear();
    this.planWorkerList = [];
    // console.log(this.productionOrdersAvailable);
    this.workersAvailable.forEach(emp => {
      if (emp.isChecked !== undefined && emp.isChecked === true) {
        this.planWorkerList.push(<ProductionPlanWorker>{
          employeeId: emp.id,
          employeeName: emp.name
        });
      }
    });
    this.planWorkerList.forEach(x => {
      this.getWorkersArray.push(this.CreateWorkerRowValues(x));
    });

  }
  mchnChecked() {
    this.getMachineArray.clear();
    this.planMachineList = [];
    // console.log(this.productionOrdersAvailable);
    this.machineriesAvailable.forEach(mchn => {
      if (mchn.isChecked !== undefined && mchn.isChecked === true) {
        this.planMachineList.push(<ProductionPlanMachine>{
          machineryId: mchn.id,
          machineryName: mchn.name
        });
      }
    });
    this.planMachineList.forEach(x => {
      this.getMachineArray.push(this.CreateMachineRowValues(x));
    });

  }
  dateSessionChanged() {
    this.planDetailList = [];
    this.planRecipeList = [];
    this.planWorkerList = [];
    this.planMachineList = [];

    this.getDetailArray.clear();
    this.getWorkersArray.clear();
    this.getMachineArray.clear();
    if (this.isEditForm) {
      this.planCreateForm.patchValue({
        date: this.productionPlanforEdit.date,
        businessPlaceId: this.productionPlanforEdit.businessPlaceId,
        productionSessionId: this.productionPlanforEdit.productionSessionId
      });
    }
    if (this.planCreateForm.get('productionSessionId').value !== '' &&
      this.planCreateForm.get('date').value !== '') {
      this.getProdOrders();
      if (this.planCreateForm.get('businessPlaceId').value !== '') {
        this.getEmployees();
        this.getMachineries();
      }
    }
  }
  backToList() {
    this.router.navigate(['/manufacturing/productionPlan']);
  }
  showPreviousStep(event?: Event) {
    this.ngWizardService.previous();
  }

  showNextStep(event?: Event) {
    this.ngWizardService.next();
  }

  resetWizard(event?: Event) {
    this.planCreateForm.reset();
    this.ngOnInit();
    this.ngWizardService.reset();

  }

  setTheme(theme: THEME) {
    this.ngWizardService.theme(theme);
  }

  stepChanged(args: StepChangedArgs) {
    // if (args.step.index === 0 && this.isEditForm) {
    //   this.showNextStep();
    //   args.direction = STEP_DIRECTIN.forward;
    // }
    if (args.step.index === 2) {
      if (this.planDetailList.length > 0) {
        this.manufacturingService
          .getIngredientForPlanDetail(<ProductionPlanDetailList>{ productionPlanDetails: this.planDetailList }).subscribe((res) => {
            this.planRecipeList = res;
            this.getRecipeArray.clear();
            this.planRecipeList.forEach(x => {
              this.getRecipeArray.push(this.CreateDetailRowValues(x));
            });
          });
      }
    }
    // console.log(args.step);
  }
  checkStepState(step: number) {
    const planCreateForm = this.planCreateForm;
    const stepStates = this.stepStates;
    const mainInfoValid = this.planCreateForm.get('businessPlaceId').valid &&
      planCreateForm.get('productionSessionId').valid &&
      !planCreateForm.hasError('lessThanToday') &&
      planCreateForm.get('date').valid;

    switch (step) {
      case 1:
        return mainInfoValid
          ? stepStates.normal
          : stepStates.error;
      case 2:
        return mainInfoValid
          ? this.getDetailArray.length > 0
            ? stepStates.normal
            : stepStates.error
          : stepStates.normal;
      case 3:
        return mainInfoValid
          ? this.getDetailArray.length > 0
            ? this.getRecipeArray.length > 0
              ? stepStates.normal
              : stepStates.error
            : stepStates.normal
          : stepStates.normal;
      case 4:
        return mainInfoValid
          ? this.getDetailArray.length > 0
            ? this.getRecipeArray.length > 0
              ? this.getWorkersArray.length > 0
                ? stepStates.normal
                : stepStates.error
              : stepStates.normal
            : stepStates.normal
          : stepStates.normal;
      case 5:
        return mainInfoValid
          ? this.getDetailArray.length > 0
            ? this.getRecipeArray.length > 0
              ? this.getWorkersArray.length > 0
                ? this.getMachineArray.length > 0
                  ? stepStates.normal
                  : stepStates.error
                : stepStates.normal
              : stepStates.normal
            : stepStates.normal
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
