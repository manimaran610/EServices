/* tslint:disable:no-unused-variable */
import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { By } from '@angular/platform-browser';
import { DebugElement } from '@angular/core';

import { ParticleCountSingleCycleComponent } from './particle-count-single-cycle.component';

describe('ParticleCountSingleCycleComponent', () => {
  let component: ParticleCountSingleCycleComponent;
  let fixture: ComponentFixture<ParticleCountSingleCycleComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ParticleCountSingleCycleComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ParticleCountSingleCycleComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
