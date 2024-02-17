/* tslint:disable:no-unused-variable */
import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { By } from '@angular/platform-browser';
import { DebugElement } from '@angular/core';

import { AcphHootRoomGrillsComponent } from './acph-hoot-room-grills.component';

describe('AcphHootRoomGrillsComponent', () => {
  let component: AcphHootRoomGrillsComponent;
  let fixture: ComponentFixture<AcphHootRoomGrillsComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ AcphHootRoomGrillsComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(AcphHootRoomGrillsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
