/* tslint:disable:no-unused-variable */
import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { By } from '@angular/platform-browser';
import { DebugElement } from '@angular/core';

import { FilterIntegrityRoomGrillsComponent } from './filter-integrity-room-grills.component';

describe('FilterIntegrityRoomGrillsComponent', () => {
  let component: FilterIntegrityRoomGrillsComponent;
  let fixture: ComponentFixture<FilterIntegrityRoomGrillsComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ FilterIntegrityRoomGrillsComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(FilterIntegrityRoomGrillsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
