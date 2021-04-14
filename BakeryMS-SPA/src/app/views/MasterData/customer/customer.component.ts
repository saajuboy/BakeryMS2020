import { Component, OnInit, ViewChild } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { Customer } from '../../../_models/User';
import { AlertifyService } from '../../../_services/alertify.service';
import { MasterService } from '../../../_services/master.service';

@Component({
  selector: 'app-customer',
  templateUrl: './customer.component.html',
  styleUrls: ['./customer.component.scss']
})
export class CustomerComponent implements OnInit {
  @ViewChild('infoModal') public infoModal: ModalDirective;
  @ViewChild('primaryModal') public addModal: ModalDirective;

  customers: Customer[] = [];
  customer: Customer;
  customerID: number;
  search: string = '';
  customerInfo: Customer = <Customer>{};
  sortOrder = { one: false, two: false, three: false, four: false };
  createForm: FormGroup;
  isEditForm: boolean = false;
  get r() { return this.createForm; }

  constructor(private masterSvc: MasterService,
    private fb: FormBuilder,
    private alertify: AlertifyService) { }

  ngOnInit() {
    this.InitiateForm();
    this.getList();
  }

  getList() {
    this.masterSvc.getCustomers().subscribe(result => {
      this.customers = result;
      this.customers.sort((a, b) => b.id - a.id);
    }, error => {
      this.alertify.error(error);
    });
  }
  InitiateForm() {
    this.createForm = this.fb.group({
      name: ['', Validators.required],
      address: [''],
      contact: [''],
      isRetail: [false, Validators.required]
    });
  }

  createCustomer() {
    if (this.createForm.valid) {
      this.customer = Object.assign({}, this.createForm.getRawValue());
      if (this.isEditForm === false) {
        this.customer.debit = 0;
        this.customer.credit = 0;
        this.customer.status = 1;

        this.masterSvc.CreateCustomer(this.customer).subscribe(() => {
          this.alertify.success('Successfully created customer');
          this.ngOnInit();
        }, res => {
          if (res.status === 400) {
            this.alertify.error(res.message + ': ' + res.code);
          } else {
            this.alertify.error('could\'nt create customer,try again');
          }
        }, () => {
          this.addModal.hide();
          this.createForm.reset();
        });
      } else {
        this.customer.status = 1;
        this.customer.id = this.customerID;
        this.masterSvc.updateCustomer(this.customerID, this.customer).subscribe(() => {
          this.alertify.success('customer updated successfully');
          this.ngOnInit();
        }, res => {
          if (res.status === 400) {
            this.alertify.error(res.message + ': ' + res.code);
          } else {
            this.alertify.error('could\'nt create customer,try again');
          }
        },
          () => {
            this.addModal.hide();
            this.createForm.reset();
          });
      }
    }
  }

  addCustomer() {
    this.createForm.reset();
    this.addModal.show();
    this.isEditForm = false;
  }

  delete(id: number) {
    this.alertify.confirm('Are you sure?',
      'Are you sure you want to delete this customer? This action cannot be undone',
      () => {
        this.masterSvc.deleteCustomer(id).subscribe((next) => {
          this.alertify.success('customer deleted succesfully');
          this.ngOnInit();
        }, () => {
          this.alertify.error('Failed to Delete customer');
          this.alertify.error('Maybe this customer is associated with some entities, check and try again');
        });
      },
      () => { });
  }
  editCustomer(id: number) {
    this.isEditForm = true;
    this.customerID = id;

    this.masterSvc.getCustomer(this.customerID).subscribe(result => {
      this.createForm.patchValue({
        name: result.name,
        contact: result.contact,
        address: result.address,
        isRetail: result.isRetail
      });
    }, error => {
      this.alertify.error(error);
    }, () => this.addModal.show());
  }

  ShowCustomerInfo(id: number) {
    this.masterSvc.getCustomer(id).subscribe((cus) => {

      this.customerInfo = cus;
      this.infoModal.show();
    });
  }

  sort(propertyNumber: number) {

    switch (propertyNumber) {
      case 1:
        this.customers.sort(
          (a, b) => this.sortOrder.one === false ?
            a.name.localeCompare(b.name) :
            b.name.localeCompare(a.name)
        );
        this.sortOrder.one = !this.sortOrder.one;
        break;
      case 2:
        this.customers.sort(
          (a, b) => this.sortOrder.two === false ?
            a.address.localeCompare(b.address) :
            b.address.localeCompare(a.address)
        );
        this.sortOrder.two = !this.sortOrder.two;
        break;
      case 3:
        this.customers.sort(
          (a, b) => this.sortOrder.three === false ?
            a.contact.localeCompare(b.contact) :
            b.contact.localeCompare(a.contact)
        );
        this.sortOrder.three = !this.sortOrder.three;
        break;
      case 4:
        this.customers.sort(
          (a, b) => this.sortOrder.four === false ?
            +a.isRetail - +b.isRetail : +b.isRetail - +a.isRetail
        );
        this.sortOrder.four = !this.sortOrder.four;
        break;
        break;
      default:
        this.customers.sort(
          (a, b) => this.sortOrder.one === false ?
            a.name.localeCompare(b.name) :
            b.name.localeCompare(a.name)
        );
        this.sortOrder.one = !this.sortOrder.one;
        break;
    }
  }

}
