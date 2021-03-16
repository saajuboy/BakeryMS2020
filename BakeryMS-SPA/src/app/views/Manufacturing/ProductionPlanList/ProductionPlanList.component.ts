import { Component, OnInit, ViewChild } from '@angular/core';
import { Router } from '@angular/router';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { ProductionPlanHeader } from '../../../_models/productionPlan';
import { AlertifyService } from '../../../_services/alertify.service';
import { ManufacturingService } from '../../../_services/manufacturing.service';

@Component({
  // tslint:disable-next-line: component-selector
  selector: 'app-ProductionPlanList',
  templateUrl: './ProductionPlanList.component.html',
  styleUrls: ['./ProductionPlanList.component.scss']
})
export class ProductionPlanListComponent implements OnInit {
  @ViewChild('infoModal') public infoModal: ModalDirective;

  productionPlans: ProductionPlanHeader[] = [];
  search: string = '';
  productionPlanInfo: ProductionPlanHeader = <ProductionPlanHeader>{};
  sortOrder = { one: false, two: false, three: false, four: false, five: false };

  constructor(private alertify: AlertifyService,
    private router: Router,
    private manufacturingService: ManufacturingService) { }

  ngOnInit() {

    this.manufacturingService.getProductionplans().subscribe(result => {
      this.productionPlans = result;
      // console.log(result);
      console.log(this.productionPlans);
    }, error => {
      this.alertify.error(error);
    });
  }

  add() {
    this.router.navigateByUrl('/manufacturing/productionPlan/create');
  }

  delete(id: number) {
    this.alertify.confirm('Are you sure?',
      'Are you sure you want to delete this Production Plan? This action cannot be undone',
      () => {
        this.manufacturingService.deleteProductionPlan(id).subscribe((next) => {
          this.alertify.success('Production plan deleted succesfully');
          this.productionPlans = this.productionPlans.filter(function (obj) {
            return obj.id !== id;
          });
        }, () => {
          this.alertify.error('Failed to Delete Production plan');
        });
      },
      () => { });

  }
  edit(id: number) {
    this.router.navigate(['/manufacturing/productionPlan/edit', id]);
  }
  ShowInfo(id: number) {

    this.manufacturingService.getProductionPlan(id).subscribe(result => {
      this.productionPlanInfo = Object.assign({}, result);
      this.productionPlanInfo.businessPlaceName = this.productionPlans.find(a => a.id === id).businessPlaceName;
      this.productionPlanInfo.sessionName = this.productionPlans.find(a => a.id === id).sessionName;
      this.infoModal.show();
    });

  }

  sort(propertyNumber: number) {

    switch (propertyNumber) {
      case 1:
        this.productionPlans.sort((a, b) => {
          return this.sortOrder.one === false ?
            <any>new Date(b.date) - <any>new Date(a.date) :
            <any>new Date(a.date) - <any>new Date(b.date);
        });
        this.sortOrder.one = !this.sortOrder.one;
        break;
      case 2:
        this.productionPlans.sort((a, b) => this.sortOrder.two === false ?
          a.sessionName.localeCompare(b.sessionName) :
          b.sessionName.localeCompare(a.sessionName));
        this.sortOrder.two = !this.sortOrder.two;
        break;
      case 3:
        this.productionPlans.sort((a, b) => this.sortOrder.three === false ?
          a.businessPlaceName.localeCompare(b.businessPlaceName) :
          b.businessPlaceName.localeCompare(a.businessPlaceName));
        this.sortOrder.three = !this.sortOrder.three;
        break;
      case 4:
        this.productionPlans.sort((a, b) => this.sortOrder.four === false ?
          +a.isNotEditable - +b.isNotEditable : +b.isNotEditable - +a.isNotEditable);
        this.sortOrder.four = !this.sortOrder.four;
        break;
      case 5:
        this.productionPlans.sort((a, b) => this.sortOrder.five === false ?
          a.userName.localeCompare(b.userName) :
          b.userName.localeCompare(a.userName));
        this.sortOrder.five = !this.sortOrder.five;
        break;
      default:
        case 1:
        this.productionPlans.sort((a, b) => {
          return this.sortOrder.one === false ?
            <any>new Date(b.date) - <any>new Date(a.date) :
            <any>new Date(a.date) - <any>new Date(b.date);
        });
        this.sortOrder.one = !this.sortOrder.one;
        break;
    }
  }


}
