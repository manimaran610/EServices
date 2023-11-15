/* tslint:disable:no-unused-variable */
import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { By } from '@angular/platform-browser';
import { DebugElement } from '@angular/core';

import { AcphRoomGrillsComponent } from './acph-room-grills.component';

describe('AcphRoomGrillsComponent', () => {
  let component: AcphRoomGrillsComponent;
  let fixture: ComponentFixture<AcphRoomGrillsComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ AcphRoomGrillsComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(AcphRoomGrillsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
