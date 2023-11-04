/* tslint:disable:no-unused-variable */
import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { By } from '@angular/platform-browser';
import { DebugElement } from '@angular/core';

import { AcphComponent } from './acph.component';

describe('AcphComponent', () => {
  let component: AcphComponent;
  let fixture: ComponentFixture<AcphComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ AcphComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(AcphComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
