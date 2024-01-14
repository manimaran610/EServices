/* tslint:disable:no-unused-variable */
import { async, ComponentFixture, TestBed } from '@angular/core/testing';


import { TempMappingComponent } from './temp-mapping.component';

describe('TempMappingComponent', () => {
  let component: TempMappingComponent;
  let fixture: ComponentFixture<TempMappingComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ TempMappingComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(TempMappingComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
