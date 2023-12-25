/* tslint:disable:no-unused-variable */
import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { By } from '@angular/platform-browser';
import { DebugElement } from '@angular/core';

import { ParticleRoomLocationsSingleCycleComponent } from './particle-room-locations-single-cycle.component';

describe('ParticleRoomLocationsSingleCycleComponent', () => {
  let component: ParticleRoomLocationsSingleCycleComponent;
  let fixture: ComponentFixture<ParticleRoomLocationsSingleCycleComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ParticleRoomLocationsSingleCycleComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ParticleRoomLocationsSingleCycleComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
