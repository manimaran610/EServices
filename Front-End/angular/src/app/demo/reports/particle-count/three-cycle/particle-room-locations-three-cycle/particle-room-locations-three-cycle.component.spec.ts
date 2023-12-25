/* tslint:disable:no-unused-variable */
import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { By } from '@angular/platform-browser';
import { DebugElement } from '@angular/core';

import { ParticleRoomLocationsThreeCycleComponent } from './particle-room-locations-three-cycle.component';

describe('ParticleRoomLocationsThreeCycleComponent', () => {
  let component: ParticleRoomLocationsThreeCycleComponent;
  let fixture: ComponentFixture<ParticleRoomLocationsThreeCycleComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ParticleRoomLocationsThreeCycleComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ParticleRoomLocationsThreeCycleComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
