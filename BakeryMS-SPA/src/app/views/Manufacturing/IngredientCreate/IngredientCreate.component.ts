import { Component, OnInit } from '@angular/core';
import { FormArray, FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { IngredientDetail, IngredientHeader } from '../../../_models/ingredient';
import { ItemForDropdown } from '../../../_models/item';
import { AlertifyService } from '../../../_services/alertify.service';
import { ManufacturingService } from '../../../_services/manufacturing.service';
import { MasterService } from '../../../_services/master.service';

@Component({
  // tslint:disable-next-line: component-selector
  selector: 'app-IngredientCreate',
  templateUrl: './IngredientCreate.component.html',
  styleUrls: ['./IngredientCreate.component.scss']
})
export class IngredientCreateComponent implements OnInit {

  ingredient: IngredientHeader;
  ingredientCreateForm: FormGroup;

  productionitems: ItemForDropdown[] = [];
  rawitems: ItemForDropdown[] = [];
  columns: string[];
  totalValue = 0;
  isEditForm: boolean = false;
  ingredientID: number;

  ingredientList: IngredientHeader[] = [];

  get gettableRowArray(): FormArray {
    return this.ingredientCreateForm.get('ingredientDetails') as FormArray;
  }

  constructor(private alertify: AlertifyService,
    private fb: FormBuilder,
    private router: Router,
    private route: ActivatedRoute,
    private manufacturingService: ManufacturingService,
    private masterService: MasterService) {
    this.columns = ['Item Code', 'Item Name', 'Quantity', 'Actions'];
  }

  async ngOnInit() {
    this.masterService.getItems(true, 0).subscribe(result => {
      this.productionitems = result;
    }, error => {
      this.alertify.error(error);
    });

    this.masterService.getItems(true, 2).subscribe(result => {
      this.rawitems = result;
    }, error => {
      this.alertify.error(error);
    });

    this.createIngredientForm();

    this.setInitialValues(this.ingredientCreateForm);

    // for edit Form
    this.route.paramMap.subscribe(params => {
      const ingID = +params.get('id');

      if (ingID) {
        this.getIngredient(ingID);
        this.isEditForm = true;
        this.ingredientID = ingID;
      }
    });

  }

  createIngredientForm() {
    this.ingredientCreateForm = this.fb.group({
      itemId: ['', Validators.required],
      servingSize: [0.01, [Validators.required, Validators.min(0.01)]],
      description: [''],
      method: [''],
      ingredientDetails: this.fb.array([
        this.initiatePodRowValues()
      ])
    });
  }

  // datesValidator(uti: UtilityService) {
  //   return (g: FormGroup) => {
  //     // console.log(Date.parse(g.get('enteredDate').value) > Date.parse(g.get('requiredDate').value));
  //     const today = uti.currentDate();
  //     // console.log(uti.currentDate());
  //     if (Date.parse(g.get('enteredDate').value) > Date.parse(g.get('requiredDate').value)) {
  //       return { greaterThanRequired: true };
  //     }
  //     if (Date.parse(g.get('enteredDate').value) < Date.parse(today)) {
  //       return { lessThanToday: true };
  //     }
  //     if (Date.parse(g.get('requiredDate').value) < Date.parse(today)) {
  //       return { requiredLessThanToday: true };
  //     }
  //     return null;
  //   };
  // }

  setInitialValues(g: FormGroup) {

    g.patchValue({
      description: 'Description of recipe',
      servingSize: 0.01
    });
  }

  getIngredient(id: number) {
    this.manufacturingService.getIngredient(id).subscribe(
      (ingredient: IngredientHeader) => {
        this.createEditPOForm(ingredient);
        // console.log(ingredient);
      },
      (error: any) => {
        console.log(error);
        this.alertify.error('some error occured');
        this.router.navigate(['/manufacturing/ingredient']);
      }
    );
  }
  createEditPOForm(ingredient: IngredientHeader) {
    this.ingredientCreateForm.patchValue({
      itemId: ingredient.itemId,
      servingSize: ingredient.servingSize,
      description: ingredient.description,
      method: ingredient.method,
    });
    this.ClearRows();
    this.onDeleteRow(0);
    // this.totalValue = 0;
    ingredient.ingredientDetails.forEach((podRow) => {
      this.gettableRowArray.push(this.initiateEditPodRowValues(podRow));
      // this.totalValue = this.totalValue + podRow.lineTotal;
    });


  }

  async createPO() {
    // set status,
    if (this.ingredientCreateForm.valid) {

      this.ingredient = Object.assign({}, this.ingredientCreateForm.getRawValue());
      console.log(this.ingredient);

      if (this.isEditForm === false) {
        // if (isForSending === false) {
        this.manufacturingService.createIngredient(this.ingredient).subscribe(() => {
          this.alertify.success('successfully Created');
        }, res => {
          const status = res.error.status;
          const code = res.error.code;
          const message = res.error.message;

          if (status === 400) {
            this.alertify.error(message + ': error code - ' + code);
          } else {
            this.alertify.error('failed to create');
            this.alertify.error('Some error occured :' + res.error);
          }
        }, () => {
          this.backToList();
        });
      } else {
        this.manufacturingService.updateIngredient(this.ingredientID, this.ingredient).subscribe(() => {
          this.alertify.success('successfully Updated');
        }, res => {
          const status = res.error.status;
          const code = res.error.code;
          const message = res.error.message;
          if (status === 400) {
            this.alertify.error(message + ': error code - ' + code);
          } else {
            this.alertify.error('failed to create');
            this.alertify.error('Some error occured :' + res.error);
          }
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
      })
    },
    );
    return formRow;

  }
  initiateEditPodRowValues(pORow: IngredientDetail): FormGroup {

    const formRow = this.fb.group({
      itemId: new FormControl(<any>pORow.itemId, {
        validators: [Validators.required]
      }),
      quantity: new FormControl(pORow.quantity.toFixed(2), {
        validators: [Validators.required, Validators.pattern(/^\d+\.\d{2}$/)]
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
    this.router.navigateByUrl('/manufacturing/ingredient');
  }

  // getPreviousProdOrders() {
  //   this.manufacturingService.getIngredients().subscribe((result) => {
  //     this.ingredientList = result;
  //   }, () => {
  //     this.alertify.error('failed To Process');
  //   }, () => {
  //     this.infoModal.show();
  //   });
  // }

  // retrievePO(id: number) {

  //   this.manufacturingService.getIngredient(id).subscribe(
  //     (ingredient: IngredientHeader) => {
  //       this.createEditPOForm(ingredient);
  //       // console.log(ingredient);
  //       this.ingredientCreateForm.patchValue({
  //         enteredDate: this.utiService.currentDate(),
  //         requiredDate: this.utiService.addDate(new Date(), 1),
  //       });
  //     },
  //     (error: any) => {
  //       console.log(error);
  //       this.alertify.error('some error occured');
  //       this.infoModal.hide();
  //     },
  //     () => this.infoModal.hide()
  //   );
  // }

  // autoGenerateProdOrder(sessionId: number, placeId: number, requiredDate) {
  //   this.manufacturingService.getAutoGeneratedIngredient(sessionId, placeId, requiredDate).subscribe(
  //     (ingredient: IngredientHeader) => {
  //       this.createEditPOForm(ingredient);
  //     }, (res) => {
  //       const status = res.error.status;
  //       const code = res.error.code;
  //       const message = res.error.message;

  //       if (status === 400 && code !== 7) {
  //         this.alertify.error(message + ': error code - ' + code);
  //       } else if (status === 400 && code === 7) {
  //         this.alertify.warning(message + ': error code - ' + code);
  //       } else {
  //         this.alertify.error('some error occured try again');
  //       }
  //     }, () => {

  //     });
  // }
}
