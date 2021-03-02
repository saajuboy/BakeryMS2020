import { Component, OnInit, ViewChild } from '@angular/core';
import { Router } from '@angular/router';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { IngredientHeader } from '../../../_models/ingredient';
import { AlertifyService } from '../../../_services/alertify.service';
import { ManufacturingService } from '../../../_services/manufacturing.service';
import { MasterService } from '../../../_services/master.service';

@Component({
  // tslint:disable-next-line: component-selector
  selector: 'app-IngredientList',
  templateUrl: './IngredientList.component.html',
  styleUrls: ['./IngredientList.component.scss']
})
export class IngredientListComponent implements OnInit {
  @ViewChild('infoModal') public infoModal: ModalDirective;

  ingredients: IngredientHeader[] = [];
  search: string = '';
  ingredientInfo: IngredientHeader = <IngredientHeader>{};
  sortOrder = { one: false, two: false, three: false, four: false, five: false };

  constructor(private masterService: MasterService,
    private alertify: AlertifyService,
    private router: Router,
    private manufacturingService: ManufacturingService) { }

  ngOnInit() {

    this.manufacturingService.getIngredients().subscribe(result => {
      this.ingredients = result;
      // console.log(result);
      console.log(this.ingredients);


    }, error => {
      this.alertify.error(error);
    });
  }

  add() {
    this.router.navigateByUrl('/manufacturing/ingredient/create');
  }

  delete(id: number) {
    this.alertify.confirm('Are you sure?',
      'Are you sure you want to delete this Ingredient? This action cannot be undone',
      () => {
        this.manufacturingService.deleteIngredient(id).subscribe((next) => {
          this.alertify.success('Ingredient deleted succesfully');
          this.ingredients = this.ingredients.filter(function (obj) {
            return obj.id !== id;
          });
        }, (res) => {
          const status = res.error.status;
          const code = res.error.code;
          const message = res.error.message;

          if (status === 400) {
            this.alertify.error(message + ': error code - ' + code);
          } else {
            this.alertify.error('failed to delete');
            this.alertify.error('Some error occured :' + res.error);
          }
        });
      },
      () => { });

  }
  edit(id: number) {
    this.router.navigate(['/manufacturing/ingredient/edit', id]);
  }
  ShowInfo(id: number) {

    this.manufacturingService.getIngredient(id).subscribe(result => {
      this.ingredientInfo = result;
      this.infoModal.show();
    }, () => this.alertify.error('some error occured, try again'));

  }

  sort(propertyNumber: number) {

    switch (propertyNumber) {
      case 1:
        this.ingredients.sort((a, b) => this.sortOrder.one === false ?
          a.itemName.localeCompare(b.itemName) : b.itemName.localeCompare(a.itemName));
        this.sortOrder.one = !this.sortOrder.one;
        break;
      case 2:
        this.ingredients.sort((a, b) => this.sortOrder.two === false ?
          a.description.localeCompare(b.description) :
          b.description.localeCompare(a.description));
        this.sortOrder.two = !this.sortOrder.two;
        break;
      case 3:
        this.ingredients.sort((a, b) => this.sortOrder.three === false ?
          a.servingSize - b.servingSize : b.servingSize - a.servingSize);
        this.sortOrder.three = !this.sortOrder.three;
        break;
      default:
        this.ingredients.sort((a, b) => this.sortOrder.one === false ?
          a.itemName.localeCompare(b.itemName) : b.itemName.localeCompare(a.itemName));
        this.sortOrder.one = !this.sortOrder.one;
        break;
    }
  }

}
