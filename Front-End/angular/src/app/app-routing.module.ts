import { SearchFormsComponent } from './demo/Search-forms/Search-forms.component';
// Angular Import
import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

// project import
import { AdminComponent } from './theme/layout/admin/admin.component';
import { GuestComponent } from './theme/layout/guest/guest.component';
import { Form1Component } from './demo/ahu-forms/form1/form1.component';
import { AddInstrumentComponent } from './demo/Instruments/add-instrument/add-instrument.component';
import { AcphComponent } from './demo/reports/acph/acph.component';
import { ParticleCountThreeCycleComponent } from './demo/reports/particle-count/three-cycle/particle-count-three-cycle/particle-count-three-cycle.component';
import { ParticleCountSingleCycleComponent } from './demo/reports/particle-count/single-cycle/particle-count-single-cycle/particle-count-single-cycle.component';
import { AddTraineeComponent } from './demo/trainee-details/add-trainee/add-trainee.component';
import { FilterIntegrityComponent } from './demo/reports/filter-integrity/filter-integrity/filter-integrity.component';
import { TempMappingComponent } from './demo/reports/temp-mapping/temp-mapping.component';
import { ParticleCountRecvCycleComponent } from './demo/reports/particle-count/recv-cycle/particle-count-recv-cycle/particle-count-recv-cycle.component';


const routes: Routes = [
  {
    path: '',
    component: AdminComponent,
    children: [
      {
        path: '',
        redirectTo: 'Home',
        pathMatch: 'full'
      },
      {
        path: 'Home',
        redirectTo: 'Home',
        pathMatch: 'full'
      },
      {
        path: 'analytics',
        loadComponent: () => import('./demo/dashboard/dash-analytics/dash-analytics.component')
      },
      {
        path: 'component',
        loadChildren: () => import('./demo/ui-element/ui-basic.module').then((m) => m.UiBasicModule)
      },
      {
        path: 'chart',
        loadComponent: () => import('./demo/chart & map/core-apex/core-apex.component')
      },
      {
        path: 'forms',
        loadComponent: () => import('./demo/forms & tables/form-elements/form-elements.component')
      },
      {
        path: 'tables',
        loadComponent: () => import('./demo/forms & tables/tbl-bootstrap/tbl-bootstrap.component')
      },
      {
        path: 'Home',
        loadComponent: () => import('./demo/home-page/home-page.component')
      },
      {
        path: 'Reports',
        component: SearchFormsComponent,
      },
      {
        path: 'form1',
        component: Form1Component,
      },
      {
        path: 'Instrument',
        children: [
          {
            path: 'Add-Instrument',
            component: AddInstrumentComponent
          }
        ]
      },
      {
        path: 'Employee',
        children: [
          {
            path: 'Add-Employee',
            component: AddTraineeComponent
          }
        ]
      },
      {
        path: 'Reports',
        children: [
          {
            path: 'ACPH',
            component: AcphComponent
          },
          {
            path: 'ACPH/:id',
            component: AcphComponent
          },
          {
            path: 'ParticleCount',
            children: [
              {
                path: 'ThreeCycle',
                component: ParticleCountThreeCycleComponent
              },
              {
                path: 'ThreeCycle/:id',
                component: ParticleCountThreeCycleComponent
              },
              {
                path: 'SingleCycle',
                component: ParticleCountSingleCycleComponent
              },
              {
                path: 'SingleCycle/:id',
                component: ParticleCountSingleCycleComponent
              },
              {
                path: 'RecvCycle',
                component: ParticleCountRecvCycleComponent
              },
              {
                path: 'RecvCycle/:id',
                component: ParticleCountRecvCycleComponent
              }
            ]

          },
          {
            path: 'FilterIntegrity',
            component: FilterIntegrityComponent
          },
          {
            path: 'FilterIntegrity/:id',
            component: FilterIntegrityComponent
          },
          {
            path: 'TempMapping',
            component: TempMappingComponent
          },
          {
            path: 'TempMapping/:id',
            component: TempMappingComponent
          },
        ]
      } 
    ]
  },
  {
    path: '',
    component: GuestComponent,
    children: [
      {
        path: 'auth/signup',
        loadComponent: () => import('./demo/authentication/sign-up/sign-up.component')
      },
      {
        path: 'auth/signin',
        loadComponent: () => import('./demo/authentication/sign-in/sign-in.component')
      }
    ]
  }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
