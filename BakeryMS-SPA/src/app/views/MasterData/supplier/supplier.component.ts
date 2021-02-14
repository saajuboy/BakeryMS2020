import { Component, OnInit, ViewChild } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { Supplier } from '../../../_models/supplier';
import { AlertifyService } from '../../../_services/alertify.service';
import { MasterService } from '../../../_services/master.service';

@Component({
  selector: 'app-supplier',
  templateUrl: './supplier.component.html',
  styleUrls: ['./supplier.component.scss']
})
export class SupplierComponent implements OnInit {
  @ViewChild('infoModal') public infoModal: ModalDirective;
  @ViewChild('primaryModal') public addModal: ModalDirective;

  suppliers: Supplier[];
  supplier: Supplier;
  supplierID: number;
  search: string = '';
  supplierInfo: any = {};
  sortOrder = { one: false, two: false, three: false };
  createForm: FormGroup;
  isEditForm: boolean = false;
  get r() { return this.createForm; }

  constructor(private masterService: MasterService, private fb: FormBuilder, private alertify: AlertifyService, private router: Router) { }


  ngOnInit() {
    this.InitiateForm();
    this.getSupplierList();
  }

  getSupplierList() {
    this.masterService.getSuppliers(false).subscribe(result => {
      this.suppliers = result;
    }, error => {
      this.alertify.error(error);
    });
  }
  InitiateForm() {
    this.createForm = this.fb.group({
      name: ['', [Validators.required, Validators.pattern('^[a-zA-Z0-9_)( ]+')]],
      contactNumber: ['', [Validators.required, Validators.pattern('^[0-9+_ ]{9,14}')]],
      email: ['', [Validators.required, Validators.email]],
      type: ['', Validators.required],
      address: ['', Validators.required]
    });
  }

  createSupplier() {
    if (this.createForm.valid) {
      this.supplier = Object.assign({}, this.createForm.getRawValue());

      if (this.isEditForm === false) {
        this.masterService.CreateSupplier(this.supplier).subscribe(() => {
          this.alertify.success('Successfully created Supplier');
          this.ngOnInit();
        }, error => {
          if (error.status === 400) {
            this.alertify.error('Supplier already exist');
          } else {
            this.alertify.error('could\'nt create Supplier');
          }
        }, () => {
          this.addModal.hide();
        });
      } else {
        this.supplier.id = this.supplierID;
        this.masterService.updateSupplier(this.supplierID, this.supplier).subscribe(() => {
          this.alertify.success('Supplier updated successfully');
          this.ngOnInit();
        }, error => {
          if (error.status === 400) {
            this.alertify.error('Supplier already exist');
          } else {
            this.alertify.error('could\'nt update Supplier');
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

  addSupplier() {
    this.createForm.reset();
    this.addModal.show();
    this.isEditForm = false;
  }

  delete(id: number) {
    this.alertify.confirm('Are you sure?',
      'Are you sure you want to delete this Supplier? This action cannot be undone',
      () => {
        this.masterService.deleteSupplier(id).subscribe((next) => {
          this.alertify.success('Supplier deleted succesfully');
          this.ngOnInit();
        }, () => {
          this.alertify.error('Failed to Delete Supplier');
          this.alertify.error('Maybe this supplier is associated with some other entity, check and try again');
        });
      },
      () => { });
  }
  editSupplier(id: number) {
    this.isEditForm = true;
    this.supplierID = id;

    this.masterService.getSupplier(this.supplierID).subscribe(result => {
      this.createForm.patchValue({
        name: result.name,
        contactNumber: result.contactNumber,
        email: result.email,
        type: result.type,
        address: result.address
      });
    }, error => {
      this.alertify.error('some error occured');
    }, () => {
      this.addModal.show();
    });

  }

  ShowSupplierInfo(id: number) {
    this.supplierInfo = this.suppliers.find(a => a.id === id);
    this.infoModal.show();
  }

  sort(propertyNumber: number) {

    switch (propertyNumber) {
      case 1:
        this.suppliers.sort(
          (a, b) => this.sortOrder.one === false ? a.name.localeCompare(b.name) : b.name.localeCompare(a.name)
        );
        this.sortOrder.one = !this.sortOrder.one;
        break;
      case 2:
        this.suppliers.sort(
          (a, b) => this.sortOrder.two === false ?
            a.contactNumber.localeCompare(b.contactNumber) :
            b.contactNumber.localeCompare(a.contactNumber)
        );
        this.sortOrder.two = !this.sortOrder.two;
        break;

      case 3:
        this.suppliers.sort(
          (a, b) => this.sortOrder.three === false ? a.email.localeCompare(b.email) : b.email.localeCompare(a.email)
        );
        this.sortOrder.three = !this.sortOrder.three;
        break;
      default:
        this.suppliers.sort(
          (a, b) => this.sortOrder.one === false ? a.name.localeCompare(b.name) : b.name.localeCompare(a.name)
        );
        this.sortOrder.one = !this.sortOrder.one;
        break;
    }
  }
}
