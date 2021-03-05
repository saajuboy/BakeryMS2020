/* tslint:disable:no-unused-variable */
import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { By } from '@angular/platform-browser';
import { DebugElement } from '@angular/core';

import { RoutineComponent } from './Routine.component';

describe('RoutineComponent', () => {
  let component: RoutineComponent;
  let fixture: ComponentFixture<RoutineComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ RoutineComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(RoutineComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
