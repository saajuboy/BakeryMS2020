import { Component, OnInit } from '@angular/core';
import { FormArray, FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { BusinessPlace } from '../../../_models/businessPlace';
import { Routine, RoutineList } from '../../../_models/employee';
import { AlertifyService } from '../../../_services/alertify.service';
import { HumanResourceService } from '../../../_services/humanResource.service';
import { MasterService } from '../../../_services/master.service';
import { UtilityService } from '../../../_services/utility.service';

@Component({
  // tslint:disable-next-line: component-selector
  selector: 'app-Routine',
  templateUrl: './Routine.component.html',
  styleUrls: ['./Routine.component.scss']
})
export class RoutineComponent implements OnInit {
  date: any = '';
  routines: Routine[] = [];
  businessPlaces: BusinessPlace[] = [];
  routineForm: FormGroup;
  routinesToUpdate: RoutineList;
  get gettableRowArray(): FormArray {
    return this.routineForm.get('routines') as FormArray;
  }
  constructor(private utiService: UtilityService,
    private fb: FormBuilder,
    private alertify: AlertifyService,
    private hrService: HumanResourceService,
    private masterService: MasterService) { }

  ngOnInit() {
    this.date = this.utiService.currentDate();
    console.log(this.date);
    this.masterService.getBusinessPlaces().subscribe((result) => {
      this.businessPlaces = result;
    });
    this.createForm();
    this.getRoutines(this.date);
  }

  getRoutines(date: Date) {
    this.hrService.getRoutines(date).subscribe((result: Routine[]) => {
      this.routines = result;
      this.createEditForm(result);
    });
  }
  autoAssign() {
    this.hrService.createAutoRoutine(this.date).subscribe(() => {
      this.getRoutines(this.date);
    }, (res) => {
      if (res.error.status === 400) {
        this.alertify.error(res.error.message + ': error code ' + res.error.code);
      } else {
        this.alertify.error('Some error occured,Try again');
      }
    });
  }
  createForm() {
    this.routineForm = this.fb.group({
      routines: this.fb.array([this.initiateRowValues()])
    });
  }
  createEditForm(routines: Routine[]) {
    this.ClearRows();
    this.onDeleteRow(0);
    routines.forEach((routineRow) => {
      this.gettableRowArray.push(this.initiateEditRowValues(routineRow));
      // this.totalValue = this.totalValue + podRow.lineTotal;
    });
  }
  initiateEditRowValues(routineRow: Routine): FormGroup {

    const role = routineRow.roleId;
    const formRow = this.fb.group({
      employeeId: new FormControl(<any>routineRow.employeeId, {
        validators: [Validators.required]
      }),
      businessPlaceId: new FormControl(<any>routineRow.businessPlaceId, {
        validators: [Validators.required]
      }),
      date: new FormControl(routineRow.date, {
        validators: [Validators.required]
      }),
      startTime: new FormControl(routineRow.startTime, {
        validators: [Validators.required]
      }),
      endTime: new FormControl(routineRow.endTime, {
        validators: [Validators.required]
      })
    }, { validators: this.timeUnitValidator }
    );
    return formRow;
  }
  initiateRowValues(): FormGroup {

    const formRow = this.fb.group({
      employeeId: new FormControl(0, {
        validators: [Validators.required]
      }),
      businessPlaceId: new FormControl(0, {
        validators: [Validators.required]
      }),
      roleName: new FormControl('', {
        validators: []
      }),
      date: new FormControl('', {
        validators: [Validators.required]
      }),
      startTime: new FormControl('', {
        validators: [Validators.required]
      }),
      endTime: new FormControl('', {
        validators: [Validators.required]
      }),
      employeeName: new FormControl('', {
        validators: []
      }),
      employeeNo: new FormControl('', {
        validators: []
      })
    }, { validators: this.timeUnitValidator }
    );
    return formRow;
  }
  timeUnitValidator(g: FormGroup) {
    if (g.get('startTime').touched || g.get('endTime').touched) {
      // console.log(g.get('startTime').value);
      const hms1: string = g.get('startTime').value;
      const a1 = hms1.split(':');
      const seconds1 = (+a1[0]) * 60 * 60 + (+a1[1]) * 60 + (+a1[2]);

      // console.log(seconds1);
      // console.log(g.get('endTime').value);
      const hms2: string = g.get('endTime').value;
      const a2 = hms2.split(':');
      const seconds2 = (+a2[0]) * 60 * 60 + (+a2[1]) * 60 + (+a2[2]);

      // console.log(seconds2);
      return seconds1 >= seconds2 ? { startTimeGreater: true } : null;
    }
    return null;
    // console.log(g.get('endTime').value);
    // return g.get('startTimee').value === g.get('endTime').value ? null : { mismatch: true };
  }
  updateRoutine() {
    this.routinesToUpdate = Object.assign({}, this.routineForm.getRawValue());
    console.log(this.routinesToUpdate);

    this.hrService.updateRoutines(this.date, this.routinesToUpdate).subscribe((res) => {
      this.getRoutines(this.date);
      this.alertify.success('Updated successfully');
    }, (res) => {
      if (res.error.status === 400) {
        this.alertify.error(res.error.message + ': error code ' + res.error.code);
      } else {
        this.alertify.error('Some error occured,Try again');
      }
    });
    console.log(this.routinesToUpdate);

  }
  dateChange() {
    // this.alertify.success(this.date);
    this.getRoutines(this.date);
  }
  ClearRows() {
    this.gettableRowArray.clear();
  }

  onDeleteRow(rowIndex: number): void {
    this.gettableRowArray.removeAt(rowIndex);
  }
}
