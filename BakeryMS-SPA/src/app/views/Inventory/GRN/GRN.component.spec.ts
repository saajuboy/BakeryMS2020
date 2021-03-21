/* tslint:disable:no-unused-variable */
import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { By } from '@angular/platform-browser';
import { DebugElement } from '@angular/core';

import { GRNComponent } from './GRN.component';

describe('GRNComponent', () => {
  let component: GRNComponent;
  let fixture: ComponentFixture<GRNComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ GRNComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(GRNComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
