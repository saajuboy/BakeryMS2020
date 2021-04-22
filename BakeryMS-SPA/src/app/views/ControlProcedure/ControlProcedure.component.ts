import { Component, OnInit, ViewChild } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { ControlProcedure } from '../../_models/configuration';
import { AlertifyService } from '../../_services/alertify.service';
import { MasterService } from '../../_services/master.service';

@Component({
  selector: 'app-ControlProcedure',
  templateUrl: './ControlProcedure.component.html',
  styleUrls: ['./ControlProcedure.component.scss']
})
export class ControlProcedureComponent implements OnInit {
  @ViewChild('infoModal') public infoModal: ModalDirective;
  @ViewChild('primaryModal') public addModal: ModalDirective;

  controlProcedures: ControlProcedure[] = [];
  controlProcedure: ControlProcedure;
  controlProcedureId: number;
  search: string = '';
  controlProcedureInfo: any = {};
  sortOrder = { one: false, two: false };
  createForm: FormGroup;
  isEditForm: boolean = false;
  businessPlaceId: number = 0;
  get r() { return this.createForm; }

  constructor(private masterService: MasterService, private fb: FormBuilder, private alertify: AlertifyService, private router: Router) { }

  ngOnInit() {
    this.InitiateForm();
    const placeId = localStorage.getItem('BusinessPlaceId');
    this.businessPlaceId = +placeId;
    this.getProcedureList(this.businessPlaceId);

  }

  getProcedureList(placeId: number) {
    this.masterService.getControlProcedures(placeId).subscribe(result => {
      this.controlProcedures = result;
      this.controlProcedures.sort((a, b) => b.id - a.id);

    }, error => {
      this.alertify.error(error.error.message);
    });
  }
  InitiateForm() {
    this.createForm = this.fb.group({
      name: ['', Validators.required],
      description: ['', Validators.required],
      businessPlaceId: [0]
    });

    this.createForm.patchValue({
      businessPlaceId: this.businessPlaceId
    });
  }

  createControlProcedure() {
    if (this.createForm.valid) {
      this.controlProcedure = Object.assign({}, this.createForm.getRawValue());

      if (this.isEditForm === false) {
        this.masterService.CreateControlProcedure(this.controlProcedure).subscribe(() => {
          this.alertify.success('Successfully created Control Procedure');
          this.ngOnInit();
        }, error => {
          if (error.error.status === 400) {
            this.alertify.error(error.error.message);
          } else {
            this.alertify.error('could\'nt create Control procedure');
          }
        }, () => {
          this.addModal.hide();
        });
      } else {
        this.controlProcedure.id = this.controlProcedureId;
        this.masterService.updateControlProcedure(this.controlProcedureId, this.controlProcedure).subscribe(() => {
          this.alertify.success('Control Procedure updated successfully');
          this.ngOnInit();
        }, error => {
          if (error.error.status === 400) {
            this.alertify.error(error.error.message);
          } else {
            this.alertify.error('could\'nt update Control Procedure');
          }
        },
          () => {
            this.addModal.hide();
          });
      }

      this.createForm.reset();
      // this.router.navigate(['/master/itemCategory']);
    }
  }

  addControlProcedure() {
    this.createForm.reset();
    this.addModal.show();
    this.createForm.patchValue({
      businessPlaceId: this.businessPlaceId
    });
    this.isEditForm = false;
  }

  delete(id: number) {
    this.alertify.confirm('Are you sure?',
      'Are you sure you want to delete this Control Procedure? This action cannot be undone',
      () => {
        this.masterService.deleteControlProcedure(id).subscribe((next) => {
          this.alertify.success('Control Procedure deleted succesfully');
          this.ngOnInit();
        }, () => {
          this.alertify.error('Failed to Delete Control Procedure');
          this.alertify.error('Maybe this Control Procedure is associated with some Entity, check and try again');
        });
      },
      () => { });
  }
  editControlProcedure(id: number) {
    this.isEditForm = true;
    this.controlProcedureId = id;

    this.masterService.getControlProcedure(this.controlProcedureId).subscribe(result => {
      this.createForm.patchValue({
        name: result.name,
        description: result.description,
        businessPlaceId: this.businessPlaceId
      });
    }, error => {
      this.alertify.error(error);
    });
    this.addModal.show();
  }

  ShowProcedureInfo(id: number) {
    this.controlProcedureInfo = this.controlProcedures.find(a => a.id === id);
    this.infoModal.show();
  }

  sort(propertyNumber: number) {

    switch (propertyNumber) {
      case 1:
        this.controlProcedures.sort(
          (a, b) => this.sortOrder.one === false ? a.name.localeCompare(b.name) : b.name.localeCompare(a.name)
        );
        this.sortOrder.one = !this.sortOrder.one;
        break;
      case 2:
        this.controlProcedures.sort(
          (a, b) => this.sortOrder.two === false ?
            a.businessPlaceName.localeCompare(b.businessPlaceName) :
            b.businessPlaceName.localeCompare(a.businessPlaceName)
        );
        this.sortOrder.two = !this.sortOrder.two;
        break;
      default:
        this.controlProcedures.sort(
          (a, b) => this.sortOrder.one === false ? a.name.localeCompare(b.name) : b.name.localeCompare(a.name)
        );
        this.sortOrder.one = !this.sortOrder.one;
        break;
    }
  }

}

