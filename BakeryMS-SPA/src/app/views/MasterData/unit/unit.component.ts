import { Component, OnInit, ViewChild } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { Unit } from '../../../_models/item';
import { AlertifyService } from '../../../_services/alertify.service';
import { MasterService } from '../../../_services/master.service';

@Component({
  selector: 'app-unit',
  templateUrl: './unit.component.html',
  styleUrls: ['./unit.component.scss']
})
export class UnitComponent implements OnInit {
  @ViewChild('infoModal') public infoModal: ModalDirective;
  @ViewChild('primaryModal') public addModal: ModalDirective;

  units: Unit[];
  unit: Unit;
  unitID: number;
  search: string = '';
  unitInfo: any = {};
  sortOrder = { one: false };
  createForm: FormGroup;
  isEditForm: boolean = false;
  get r() { return this.createForm; }

  constructor(private masterService: MasterService, private fb: FormBuilder, private alertify: AlertifyService, private router: Router) { }

  ngOnInit() {
    this.InitiateForm();
    this.getUnitList();
  }

  getUnitList() {
    this.masterService.getUnits().subscribe(result => {
      this.units = result;
    }, error => {
      this.alertify.error(error);
    });
  }
  InitiateForm() {
    this.createForm = this.fb.group({
      description: ['', Validators.required]
    });
  }

  createUnit() {
    if (this.createForm.valid) {
      this.unit = Object.assign({}, this.createForm.getRawValue());

      if (this.isEditForm === false) {
        this.masterService.CreateUnit(this.unit).subscribe(() => {
          this.alertify.success('Successfully created Unit');
          this.ngOnInit();
        }, error => {
          if (error.status === 400) {
            this.alertify.error('Unit to create already exists');
          } else {
            this.alertify.error('could\'nt create unit');
          }
        }, () => {
          this.addModal.hide();
        });
      } else {
        this.unit.id = this.unitID;
        this.masterService.updateUnit(this.unitID, this.unit).subscribe(() => {
          this.alertify.success('Unit updated successfully');
          this.ngOnInit();
        }, error => {
          if (error.status === 400) {
            this.alertify.error('Unit to update already exists');
          } else {
            this.alertify.error('could\'nt update Unit');
          }
        },
          () => {
            this.addModal.hide();
          });
      }

      this.createForm.reset();
    }
  }

  addUnit() {
    this.createForm.reset();
    this.addModal.show();
    this.isEditForm = false;
  }

  delete(id: number) {
    this.alertify.confirm('Are you sure?',
      'Are you sure you want to delete this Unit? This action cannot be undone',
      () => {
        this.masterService.deleteUnit(id).subscribe((next) => {
          this.alertify.success('Unit deleted succesfully');
          this.ngOnInit();
        }, () => {
          this.alertify.error('Failed to Delete Unit');
          this.alertify.error('Maybe this Unit is associated with some items, check and try again');
        });
      },
      () => { });
  }
  editUnit(id: number) {
    this.isEditForm = true;
    this.unitID = id;

    this.masterService.getUnit(this.unitID).subscribe(result => {
      this.createForm.patchValue({
        description: result.description
      });
    }, error => {
      this.alertify.error('Failed to edit unit ' + error);
    });
    this.addModal.show();
  }

  ShowUnitInfo(id: number) {
    this.unitInfo = this.units.find(a => a.id === id);
    this.infoModal.show();
  }

  sort(propertyNumber: number) {

    switch (propertyNumber) {
      case 1:
        this.units.sort(
          (a, b) => this.sortOrder.one === false ? a.description.localeCompare(b.description) : b.description.localeCompare(a.description)
        );
        this.sortOrder.one = !this.sortOrder.one;
        break;
      default:
        this.units.sort(
          (a, b) => this.sortOrder.one === false ? a.description.localeCompare(b.description) : b.description.localeCompare(a.description)
        );
        this.sortOrder.one = !this.sortOrder.one;
        break;
    }
  }

}
