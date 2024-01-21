/* tslint:disable:no-unused-variable */
import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { By } from '@angular/platform-browser';
import { DebugElement } from '@angular/core';

import { ParticleRoomLocationsRecvCycleComponent } from './particle-room-locations-recv-cycle.component';

describe('ParticleRoomLocationsRecvCycleComponent', () => {
  let component: ParticleRoomLocationsRecvCycleComponent;
  let fixture: ComponentFixture<ParticleRoomLocationsRecvCycleComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ParticleRoomLocationsRecvCycleComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ParticleRoomLocationsRecvCycleComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
