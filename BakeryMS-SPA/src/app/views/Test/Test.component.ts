import { Component, OnInit } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { AlertifyService } from '../../_services/alertify.service';
import { environment } from '../../../environments/environment';

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
    this.http.get(environment.apiUrl + 'weatherforecast').subscribe(response => {
      this.testValues = response;
    }, error => {
      console.log(error);

    });
  }
}
