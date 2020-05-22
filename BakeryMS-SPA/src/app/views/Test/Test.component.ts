import { Component, OnInit } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { AlertifyService } from '../../_services/alertify.service';

@Component({
  // tslint:disable-next-line: component-selector
  selector: 'app-Test',
  templateUrl: './Test.component.html',
  styleUrls: ['./Test.component.scss']
})
export class TestComponent implements OnInit {

  testValues: any;

  constructor(private http: HttpClient, private alerify: AlertifyService) { }

  ngOnInit() {
    // this.alerify.success('success');
    this.getTestValues();

  }

  getTestValues() {
    this.http.get('https://localhost:5001/weatherforecast').subscribe(response => {
      this.testValues = response;
    }, error => {
      console.log(error);

    });
  }
}
