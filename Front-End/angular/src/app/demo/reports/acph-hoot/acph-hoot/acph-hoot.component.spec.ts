/* tslint:disable:no-unused-variable */
import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { By } from '@angular/platform-browser';
import { DebugElement } from '@angular/core';

import { AcphHootComponent } from './acph-hoot.component';

describe('AcphHootComponent', () => {
  let component: AcphHootComponent;
  let fixture: ComponentFixture<AcphHootComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ AcphHootComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(AcphHootComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
