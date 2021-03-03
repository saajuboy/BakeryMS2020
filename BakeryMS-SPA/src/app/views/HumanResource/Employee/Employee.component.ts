import { Component, OnInit, ViewChild } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { Employee } from '../../../_models/employee';
import { AlertifyService } from '../../../_services/alertify.service';
import { HumanResourceService } from '../../../_services/humanResource.service';
import { ManufacturingService } from '../../../_services/manufacturing.service';

@Component({
  // tslint:disable-next-line: component-selector
  selector: 'app-Employee',
  templateUrl: './Employee.component.html',
  styleUrls: ['./Employee.component.scss']
})
export class EmployeeComponent implements OnInit {
  @ViewChild('infoModal') public infoModal: ModalDirective;
  @ViewChild('primaryModal') public addModal: ModalDirective;

  employees: Employee[] = [];
  employee: Employee;
  employeeID: number;
  employeeNumber: number;
  search: string = '';
  employeeInfo: Employee = <Employee>{};
  sortOrder = { one: false, two: false, three: false, four: false, five: false, six: false };
  createForm: FormGroup;
  isEditForm: boolean = false;
  get r() { return this.createForm; }

  constructor(private humanresourceService: HumanResourceService,
    private fb: FormBuilder,
    private alertify: AlertifyService,
    private router: Router) { }

  ngOnInit() {
    this.InitiateForm();
    this.getList();
  }

  getList() {
    this.humanresourceService.getEmployees().subscribe(result => {
      this.employees = result;
    }, error => {
      this.alertify.error(error);
    });
  }
  InitiateForm() {
    this.createForm = this.fb.group({
      employeeNumber: [{ value: 0, disabled: true }],
      name: ['', Validators.required],
      contactNumber: [''],
      nIC: ['', [Validators.required, Validators.pattern('^([0-9]{9}[x|X|v|V]|[0-9]{12})$')]],
      address: [''],
      type: ['', Validators.required],
      salary: ['', [Validators.required, Validators.min(1)]],
      role: ['', Validators.required]
    });
  }

  createEmployee() {
    if (this.createForm.valid) {
      this.employee = Object.assign({}, this.createForm.getRawValue());

      this.employee.nic = this.createForm.get('nIC').value;
      if (this.isEditForm === false) {
        this.humanresourceService.CreateEmployee(this.employee).subscribe(() => {
          this.alertify.success('Successfully created employee');
          this.ngOnInit();
        }, res => {
          if (res.status === 400) {
            this.alertify.error(res.message + ': ' + res.code);
          } else {
            this.alertify.error('could\'nt create employee,try again');
          }
        }, () => {
          this.addModal.hide();
          this.createForm.reset();
        });
      } else {
        this.employee.id = this.employeeID;
        this.humanresourceService.updateEmployee(this.employeeID, this.employee).subscribe(() => {
          this.alertify.success('employee updated successfully');
          this.ngOnInit();
        }, res => {
          if (res.status === 400) {
            this.alertify.error(res.message + ': ' + res.code);
          } else {
            this.alertify.error('could\'nt create employee,try again');
          }
        },
          () => {
            this.addModal.hide();
            this.createForm.reset();
          });
      }


      // this.router.navigate(['/master/itemCategory']);
    }
  }

  addEmployee() {
    this.createForm.reset();
    this.humanresourceService.getNextEmployeeNo().subscribe(res => {
      console.log(res);
      this.createForm.patchValue({
        employeeNumber: res
      });
    });
    this.addModal.show();
    this.isEditForm = false;
  }

  delete(id: number) {
    this.alertify.confirm('Are you sure?',
      'Are you sure you want to delete this employee? This action cannot be undone',
      () => {
        this.humanresourceService.deleteEmployee(id).subscribe((next) => {
          this.alertify.success('employee deleted succesfully');
          this.ngOnInit();
        }, () => {
          this.alertify.error('Failed to Delete employee');
          this.alertify.error('Maybe this employee is associated with some entities, check and try again');
        });
      },
      () => { });
  }
  editEmployee(id: number) {
    this.isEditForm = true;
    this.employeeID = id;

    this.humanresourceService.getEmployee(this.employeeID).subscribe(result => {
      this.createForm.patchValue({
        employeeNumber: result.employeeNumber,
        name: result.name,
        contactNumber: result.contactNumber,
        nIC: result.nic,
        address: result.address,
        type: result.type,
        salary: result.salary,
        role: result.role
      });
    }, error => {
      this.alertify.error(error);
    }, () => this.addModal.show());
  }

  ShowEmployeeInfo(id: number) {
    this.humanresourceService.getEmployee(id).subscribe((emp) => {

      this.employeeInfo = emp;
      const type = this.employeeInfo.type;
      const role = this.employeeInfo.role;

      this.employeeInfo.typeName = type === 0 ? 'Permanent' :
        type === 1 ? 'Daily' :
          type === 2 ? 'Contract' : 'Other';
      this.employeeInfo.roleName = role === 0 ? 'Manager' :
        role === 1 ? 'Cashier' :
          role === 2 ? 'Baker' :
            role === 3 ? 'Counter' :
              role === 4 ? 'Waiter' : 'Random';

      this.infoModal.show();
    });
  }

  sort(propertyNumber: number) {

    switch (propertyNumber) {
      case 1:
        this.employees.sort(
          (a, b) => this.sortOrder.one === false ?
            a.employeeNumber - b.employeeNumber : b.employeeNumber - a.employeeNumber
        );
        this.sortOrder.one = !this.sortOrder.one;
        break;
      case 2:
        this.employees.sort(
          (a, b) => this.sortOrder.two === false ?
            a.name.localeCompare(b.name) :
            b.name.localeCompare(a.name)
        );
        this.sortOrder.two = !this.sortOrder.two;
        break;
      case 3:
        this.employees.sort(
          (a, b) => this.sortOrder.three === false ?
            a.contactNumber.localeCompare(b.contactNumber) :
            b.contactNumber.localeCompare(a.contactNumber)
        );
        this.sortOrder.three = !this.sortOrder.three;
        break;
      case 4:
        this.employees.sort(
          (a, b) => this.sortOrder.four === false ?
            a.roleName.localeCompare(b.roleName) :
            b.roleName.localeCompare(a.roleName)
        );
        this.sortOrder.four = !this.sortOrder.four;
        break;
      case 5:
        this.employees.sort(
          (a, b) => this.sortOrder.five === false ?
            a.typeName.localeCompare(b.typeName) :
            b.typeName.localeCompare(a.typeName)
        );
        this.sortOrder.five = !this.sortOrder.five;
        break;
      case 6:
        this.employees.sort(
          (a, b) => this.sortOrder.five === false ?
            a.salary - b.salary :
            b.salary - a.salary
        );
        this.sortOrder.five = !this.sortOrder.five;
        break;
      default:
        this.employees.sort(
          (a, b) => this.sortOrder.one === false ?
            a.employeeNumber - b.employeeNumber : b.employeeNumber - a.employeeNumber
        );
        this.sortOrder.one = !this.sortOrder.one;
        break;
    }
  }

}
