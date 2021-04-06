import { Component, OnInit, ViewChild } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { Transaction } from '../../../_models/availableItems';
import { BusinessPlace } from '../../../_models/businessPlace';
import { AlertifyService } from '../../../_services/alertify.service';
import { AuthService } from '../../../_services/auth.service';
import { MasterService } from '../../../_services/master.service';
import { PosService } from '../../../_services/pos.service';
import { UtilityService } from '../../../_services/utility.service';

@Component({
  // tslint:disable-next-line: component-selector
  selector: 'app-Transactions',
  templateUrl: './Transactions.component.html',
  styleUrls: ['./Transactions.component.scss']
})
export class TransactionsComponent implements OnInit {
  @ViewChild('primaryModal') public addModal: ModalDirective;
  @ViewChild('infoModal') public infoModal: ModalDirective;

  businessPlaces: BusinessPlace[] = [];
  businessPlace: number = 0;
  transactions: Transaction[] = [];
  transaction: Transaction = <Transaction>{};
  search: string = '';
  transactionInfo: Transaction = <Transaction>{};
  sortOrder = { one: false, two: false, three: false, four: false, five: false, six: false };
  fromDate: string;
  toDate: string;

  createForm: FormGroup;
  get r() { return this.createForm; }
  // pageOfItems: Array<any>;

  constructor(private masterService: MasterService,
    private posService: PosService,
    private alertify: AlertifyService,
    private utiService: UtilityService,
    private authService: AuthService,
    private fb: FormBuilder,
    private router: Router) { }

  ngOnInit() {
    this.InitiateForm();
    this.getList();

  }
  getList() {
    this.fromDate = this.utiService.addDate(new Date(), -1);
    this.toDate = this.utiService.addDate(new Date(), 1);

    this.masterService.getBusinessPlaces().subscribe(result => {
      this.businessPlaces = result;
      this.businessPlaces.sort((a, b) => b.id - a.id);
      this.businessPlace = +localStorage.getItem('BusinessPlaceId');
      this.bpOrdateChange();
    }, error => {
      this.alertify.error(error);
    });
  }
  InitiateForm() {
    this.createForm = this.fb.group({
      description: ['', Validators.required],
      businessPlaceId: [0, Validators.required],
      userId: [0, Validators.required],
      reference: [''],
      debit: [0],
      credit: [0]
    }, { validators: this.debitCreditValidator });
  }
  debitCreditValidator(g: FormGroup) {
    const debit = +g.get('debit').value;
    const credit = +g.get('credit').value;
    if (debit === 0 && credit === 0) {
      return { debCredRequired: true };
    }
    return null;

  }
  bpOrdateChange() {
    this.posService.getTransactions(this.businessPlace, this.fromDate, this.toDate).subscribe((res) => {
      this.transactions = res;
    }, (result) => {
      this.alertify.warning(result.error.message + ' : ' + result.error.code);
    });
  }

  createTransaction() {
    if (this.createForm.valid) {
      this.transaction = Object.assign({}, this.createForm.getRawValue());

      this.posService.createTransaction(this.transaction).subscribe(() => {
        this.alertify.success('Transaction created');
        this.ngOnInit();
      }, res => {
        if (res.status === 400) {
          this.alertify.error(res.message + ': ' + res.code);
        } else {
          this.alertify.error('could\'nt create transaction,try again');
        }
      }, () => {
        this.addModal.hide();
        this.createForm.reset();
      });


      // this.router.navigate(['/master/itemCategory']);
    }
  }

  addTransaction() {
    this.createForm.reset();
    this.createForm.patchValue({
      businessPlaceId: +localStorage.getItem('BusinessPlaceId'),
      userId: this.authService.getuserId(),
      description: '',
      reference: '',
      debit: 0.00,
      credit: 0.00
    });
    this.addModal.show();
  }

  ShowItemInfo(id: number) {
    this.posService.getTransaction(id).subscribe((result) => {
      this.transactionInfo = result;
      // console.log(result);

      this.infoModal.show();
    }, () => {
      this.alertify.error('some Error occured');
    });
  }

  sort(propertyNumber: number) {

    switch (propertyNumber) {
      // case 1:
      //   this.transactions.sort((a, b) => this.sortOrder.one === false ?
      //     a.salesNo - b.salesNo :
      //     b.salesNo - a.salesNo);
      //   this.sortOrder.one = !this.sortOrder.one;
      //   break;

      case 2:
        this.transactions.sort((a, b) => {
          return this.sortOrder.two === false ?
            <any>new Date(a.date) - <any>new Date(b.date) :
            <any>new Date(b.date) - <any>new Date(a.date);
        });
        this.sortOrder.two = !this.sortOrder.two;
        break;

      case 3:
        this.transactions.sort((a, b) => this.sortOrder.three === false
          ? a.description.localeCompare(b.description) : b.description.localeCompare(a.description));
        this.sortOrder.three = !this.sortOrder.three;
        break;
      case 4:
        this.transactions.sort((a, b) => this.sortOrder.four === false ?
          a.debit - b.debit : b.debit - a.debit);
        this.sortOrder.four = !this.sortOrder.four;
        break;
      case 5:
        this.transactions.sort((a, b) => this.sortOrder.five === false ?
          a.credit - b.credit : b.credit - a.credit);
        this.sortOrder.five = !this.sortOrder.five;
        break;
      case 6:
        this.transactions.sort((a, b) => this.sortOrder.six === false ?
          a.userName.localeCompare(b.userName) : b.userName.localeCompare(a.userName));
        this.sortOrder.six = !this.sortOrder.six;
        break;
      default:
        this.transactions.sort((a, b) => {
          return this.sortOrder.two === false ?
            <any>new Date(a.date) - <any>new Date(b.date) :
            <any>new Date(b.date) - <any>new Date(a.date);
        });
        this.sortOrder.two = !this.sortOrder.two;
        break;
    }
  }
  // onChangePage(pageOfItems: Array<any>) {
  //   // update current page of items
  //   this.pageOfItems = pageOfItems;
  // }
}
