import { Component, OnInit, ViewChild } from '@angular/core';
import { jsPDF } from 'jspdf';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { AvailableItemForList, SalesDetail, SalesDetailForPos, SalesHeader, SalesHeaderForPos } from '../../../_models/availableItems';
import { Customer } from '../../../_models/User';
import { AlertifyService } from '../../../_services/alertify.service';
import { InventoryService } from '../../../_services/inventory.service';
import { MasterService } from '../../../_services/master.service';
import { PosService } from '../../../_services/pos.service';

@Component({
  selector: 'app-SalesCreate',
  templateUrl: './SalesCreate.component.html',
  styleUrls: ['./SalesCreate.component.scss']
})
export class SalesCreateComponent implements OnInit {
  @ViewChild('smallModal') public smallModal: ModalDirective;
  @ViewChild('billingModal') public billingModal: ModalDirective;

  isNotify: boolean = true;
  search: string = '';
  availableItems: AvailableItemForList[] = [];
  sales: SalesHeaderForPos = <SalesHeaderForPos>{ receivedAmount: 0, salesDetails: [] };
  saleToCreate: SalesHeader;
  filter: number = 0;
  businessPlaceId = 0;
  customersName: Customer[] = [];
  isDiscountAllowed: boolean = false;

  constructor(private invService: InventoryService,
    private alertify: AlertifyService,
    private posService: PosService,
    private masterSvc: MasterService) { }

  ngOnInit() {
    this.getAvailableItems(0);
    this.getCustomers();
    const date = new Date();
    this.sales.customerName = 'Cash Payee - ' + date.toDateString() + ' ' + date.toTimeString().substring(0, 5);
  }

  getAvailableItems(filter?: number) {
    // getBusinessPlaceFromConfig
    const placeId = localStorage.getItem('BusinessPlaceId');
    this.businessPlaceId = +placeId;
    if (this.businessPlaceId > 0) {
      this.invService.getAvailableItemsForPOS(this.businessPlaceId, filter == null
        || filter === undefined ? 0 : filter).subscribe((result) => {
          this.availableItems = result;
          // console.log(this.availableItems);
          this.sales.salesDetails.forEach(x => {
            const curr = this.availableItems.find(y => y.id === x.itemId && y.type === x.type);
            curr.availableQuantity -= x.quantity;
            curr.usedQuantity += x.quantity;
          });
        }, (res) => {
          if (res.error.status === 400 || res.error.status === '400') {
            this.alertify.error(res.error.message + ' : ' + res.error.code);
          } else {
            this.alertify.error('some error occured, Try again');
          }
        });
    } else {
      this.alertify.error('Set businessPlace in Config');
    }

  }
  getCustomers() {
    this.masterSvc.getCustomers().subscribe((res) => {
      res.forEach(x => {
        this.customersName.push(<Customer>{ name: x.name, typeName: x.isRetail ? 'Retail' : 'WholeSale' });
      });
    });
  }
  cusChange() {

    this.calculatePriceAndTotal(this.sales);
  }
  discountEligibility() {
    if (this.sales.salesDetails.length !== 0) {
      if (this.customersName.some(a => a.name === this.sales.customerName)) {
        this.isDiscountAllowed = true;
      } else {
        this.isDiscountAllowed = false;
        this.sales.discount = 0;
      }
    } else {
      this.isDiscountAllowed = false;
      this.sales.discount = 0;
    }

  }
  filterChange() {
    this.getAvailableItems(this.filter);
  }
  quantityChange(item: SalesDetailForPos) {
    const curr = this.availableItems.find(y => y.id === item.itemId && y.type === item.type);
    curr.availableQuantity -= (item.quantity - item.previousQuantity);
    curr.usedQuantity += (item.quantity - item.previousQuantity);

    item.previousQuantity = item.quantity;
    item.lineTotal = item.quantity * item.price;

    // this.sales.total = this.sales.salesDetails.reduce((a, b) => a + (b['lineTotal'] || 0), 0);
    this.calculatePriceAndTotal(this.sales);
    // console.log(this.sales.total);
  }
  itemClick(item: AvailableItemForList) {
    // this.alertify.success(item.name);
    if (this.sales.salesDetails.some(a => a.itemId === item.id && a.type === item.type)) {
      const itemUp = this.sales.salesDetails.find(a => a.itemId === item.id && a.type === item.type);
      itemUp.quantity += 1;
      itemUp.previousQuantity = itemUp.quantity;
    } else {
      this.sales.salesDetails.push(
        <SalesDetailForPos>{
          itemId: item.id,
          type: item.type,
          itemName: item.name,
          quantity: 1,
          price: item.sellingPrice,
          lineTotal: item.sellingPrice,
          previousQuantity: 1
        });
    }

    item.availableQuantity -= 1;
    item.usedQuantity += 1;

    this.calculatePriceAndTotal(this.sales);
    // console.log(item);
  }

  createSale() {
    const sales = this.sales;
    this.saleToCreate = <SalesHeader>{
      businessPlaceId: this.businessPlaceId,
      receivedAmount: sales.receivedAmount,
      changeAmount: sales.changeAmount,
      date: new Date(),
      customerName: sales.customerName,
      discount: sales.discount,
      total: sales.total,
      time: new Date().toTimeString().substring(0, 8),

      salesDetails: []
    };

    sales.salesDetails.forEach(x => {
      this.saleToCreate.salesDetails.push(<SalesDetail>{
        itemId: x.itemId,
        price: x.price,
        quantity: x.quantity,
        lineTotal: x.lineTotal,
        type: x.type
      });
    });

    this.posService.createSales(this.saleToCreate).subscribe((res) => {
      // console.log(res);
      this.downloadPdf(res);
      this.billingModal.hide();
      this.clearSale();
    }, (res) => {
      this.billingModal.hide();
      if (res.error.status === 400) {
        this.alertify.error(res.error.message + ' : ' + res.error.code);
      }
    });

    console.log(this.saleToCreate);

  }

  clearSale() {
    this.sales.salesDetails.splice(0, this.sales.salesDetails.length);
    this.getAvailableItems(this.filter);
    this.calculatePriceAndTotal(this.sales);
  }

  calculatePriceAndTotal(sales: SalesHeaderForPos) {
    let total = 0;
    let discount = 0;
    // console.log(this.availableItems);
    this.discountEligibility();
    sales.salesDetails.forEach(x => {
      const availableItem = this.availableItems.find(y => y.id === x.itemId && y.type === x.type);
      const avail = availableItem.availableQuantity + x.quantity;
      const allowedDiscount = availableItem.sellingPrice - (availableItem.costPrice * 1.2);
      // console.log('allowed dis' + allowedDiscount);

      x.lineTotal = (x.quantity > 0 ? (x.quantity > avail ? avail : x.quantity) : 0) * x.price;
      total += x.lineTotal;
      discount += (x.quantity > 0 ? (x.quantity > avail ? avail : x.quantity) : 0) * (allowedDiscount > 0 ? allowedDiscount : 0);
    });
    // console.log(discount);

    if (sales.discount > discount) {
      sales.discount = discount;
    }
    if (sales.discount < 0 || !sales.discount) {
      sales.discount = 0;
    }
    sales.total = total - sales.discount;

    if (sales.receivedAmount > sales.total) {
      sales.changeAmount = sales.receivedAmount - sales.total;
    }

    // this.downloadPdf();
  }

  getMaxQuantity(item: SalesDetailForPos) {
    return this.availableItems.find(x => x.id === item.itemId && x.type === item.type).availableQuantity + item.quantity;
  }

  deleteRow(item: SalesDetailForPos) {
    // const availItem = this.availableItems.find(x => x.id === item.itemId && x.type === item.type);

    // availItem.availableQuantity += item.quantity;
    // availItem.usedQuantity -= item.quantity;
    const index = this.sales.salesDetails.findIndex(a => a.itemId === item.itemId && a.type === item.type);
    this.sales.salesDetails.splice(index, 1);
    this.getAvailableItems(this.filter);
    this.calculatePriceAndTotal(this.sales);
  }

  downloadPdf(sales: SalesHeader) {
    // const starry = [{ name: 'asdf1', qty: 10, prc: 20, ttl: 200 }
    //   , { name: 'asdf2', qty: 10, prc: 20, ttl: 200 },
    // { name: 'asdf3', qty: 10, prc: 20, ttl: 200 }]
    const doc = new jsPDF('portrait', 'pt', 'a4', true);

    let text = 'Upland Bake house';
    let yPos = 40;
    doc.setFontSize(12);
    doc.text(text, this.ofsetCenter(text, doc), yPos, {}, 'center');

    doc.setFontSize(10);
    text = sales.businessPlaceName; // businessPlaceName
    yPos += 15;
    doc.text(text, this.ofsetCenter(text, doc), yPos, {}, {});

    doc.setFontSize(8);
    text = sales.businessPlace.address
      .substring(0, 40); // businessPlaceName
    yPos += 15;
    doc.text(text, this.ofsetCenter(text, doc), yPos, {}, {});

    text = sales.businessPlace.address;
    if (text.length > 40) { // add adress here
      text = text.substring(41, 81);
      yPos += 10;
      doc.text(text, this.ofsetCenter(text, doc), yPos, {}, {});
    }

    doc.setFontSize(10);
    text = 'Tel - 052-2300034';
    yPos += 15;
    doc.text(text, this.ofsetCenter(text, doc), yPos, {}, {});

    doc.setFontSize(8);
    text = 'Invoice No - ' + sales.salesNo;
    yPos += 15;
    doc.text(text, 210, yPos, {}, {});

    text = 'Customer  - ' + sales.customerName.substring(0, 32);
    yPos += 15;
    doc.text(text, 210, yPos, {}, {});

    doc.setFontSize(10);
    text = '------------------------------------------------------'; // 55 characters
    yPos += 10;
    doc.text(text, this.ofsetCenter(text, doc), yPos, {}, {});

    text = 'Cashier     - ' + sales.userName;
    yPos += 10;
    doc.text(text, 210, yPos, {}, {});

    text = 'Date          - ' + sales.date;
    yPos += 15;
    doc.text(text, 210, yPos, {}, {});

    text = 'Time          - ' + sales.time;
    yPos += 15;
    doc.text(text, 210, yPos, {}, {});

    text = '------------------------------------------------------'; // 55 characters
    yPos += 10;
    doc.text(text, this.ofsetCenter(text, doc), yPos, {}, {});

    text = 'Item          Price        Qty           Amt';
    yPos += 10;
    doc.text(text, 210, yPos, {}, {});

    text = '------------------------------------------------------'; // 55 characters
    yPos += 10;
    doc.text(text, this.ofsetCenter(text, doc), yPos, {}, {});

    doc.setFontSize(8);
    let count = 0;
    sales.salesDetails.forEach(x => {
      count += 1;
      text = count + '. ' + (x.itemName ? x.itemName : 'Unknown Item')
        .substring(0, 37);
      yPos += 10;
      doc.text(text, 210, yPos, {}, {});

      yPos += 10;
      text = x.price.toFixed(2);
      doc.text(text, 260, yPos, {}, {});
      text = x.quantity.toFixed(2);
      doc.text(text, 300, yPos, {}, {});
      text = x.lineTotal.toFixed(2);
      doc.text(text, 340, yPos, {}, {});
    });

    doc.setFontSize(10);
    text = '------------------------------------------------------'; // 55 characters
    yPos += 10;
    doc.text(text, this.ofsetCenter(text, doc), yPos, {}, {});

    doc.setFontSize(7);
    text = 'Total Items - ' + count; // 55 characters
    yPos += 7;
    doc.text(text, 210, yPos, {}, {});

    doc.setFontSize(10);
    text = '      Total                  - ' + (sales.total + sales.discount).toFixed(2); // 55 characters
    yPos += 15;
    doc.text(text, 210, yPos, {}, {});

    text = '      Discount            - (' + (sales.discount).toFixed(2) + ')'; // 55 characters
    yPos += 13;
    doc.text(text, 210, yPos, {}, {});

    text = '      Net Total           - ' + (sales.total).toFixed(2); // 55 characters
    yPos += 13;
    doc.text(text, 210, yPos, {}, {});

    text = '      Received Cash - ' + (sales.receivedAmount).toFixed(2); // 55 characters
    yPos += 13;
    doc.text(text, 210, yPos, {}, {});

    text = '      Change             - ' + (sales.changeAmount).toFixed(2); // 55 characters
    yPos += 13;
    doc.text(text, 210, yPos, {}, {});

    doc.setFontSize(10);
    text = '------------------------------------------------------'; // 55 characters
    yPos += 10;
    doc.text(text, this.ofsetCenter(text, doc), yPos, {}, {});

    doc.setFontSize(8);
    text = 'Thank you For Dealing with us !!!!'; // 55 characters
    yPos += 10;
    doc.text(text, this.ofsetCenter(text, doc), yPos, {}, {});

    doc.save('receipt' + sales.salesNo + '.pdf');

  }

  ofsetCenter(text, doc) {
    const xOffset = (doc.internal.pageSize.width / 2) - (doc.getStringUnitWidth(text) * doc.getFontSize() / 2);
    return xOffset;
  }
}
