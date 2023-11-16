/* eslint-disable @typescript-eslint/no-unused-vars */
import { CoreServices } from '../../../Models/core-services.Model';
// angular import
import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';

// project import
import { SharedModule } from 'src/app/theme/shared/shared.module';
import { Router } from '@angular/router';

@Component({
  selector: 'app-home-page',
  standalone: true,
  imports: [CommonModule, SharedModule],
  template:`<div class="col-sm-12">
  <div>
    <h2 class="text-center">Welcome to AHU Portal !</h2>

  </div>
  <hr>
  <div class="row" *ngFor="let coreService of coreServicesList">
    <app-card cardTitle="{{coreService.title}}" [options]="false" >
      <div class="row">
        <div class="col-ld-3 col-md-4 col-sm-12" *ngFor="let item of coreService.services">
          <app-card [hidHeader]="true" cardClass="{{ item.background }} order-card" >
            <a  (click)="navigateToUrl(item.navigateTo)">
              <h3 class="text-white text-center">{{item.title}}<i class="feather {{ item.icon }} m-2"></i></h3>
              <h5 class="text-white text-center">{{item.subTitle}}</h5>
            </a>
          </app-card>
        </div>
      </div>
    </app-card>
  </div>
</div>`
  // templateUrl: './Home-page.component.html',
 // styleUrls: ['./Home-page.component.scss']
})
export default class HomePageComponent implements OnInit {

  coreServicesList: CoreServices[] = [];
  constructor(private router: Router) {

  }
  ngOnInit(): void {
    this.coreServicesList = [
      {
        title: 'Reports',
        services: [
          {
            title: 'ACPH',
            background: 'bg-c-blue',
            icon: 'icon icon-anchor ',
            navigateTo: '/Reports/ACPH'
          },
          {
            title: 'Filter Integrity',
            background: 'bg-c-green',
            icon: 'icon icon-filter',
            navigateTo: '/form1'
          },
          {
            title: 'Particle count',
            subTitle: '(Single Cycle)',
            background: 'bg-c-yellow',
            icon: 'icon icon-codepen',
            navigateTo: '/form1'
          },
          {
            title: 'Particle count',
            subTitle: '(Three Cycle)',
            background: 'bg-c-red',
            icon: 'icon icon-codepen',
            navigateTo: '/form1'
          },
          {
            title: 'Temp. Mapping',
            background: 'bg-c-purple',
            icon: 'icon icon-thermometer',
            navigateTo: '/form1'
          },

        ]
      },
      {
        title: 'Instrument Details',
        services: [
          {
            title: 'Add Instrument',
            background: 'bg-c-blue',
            icon: 'icon icon-framer',
            navigateTo: '/Instrument/Add-Instrument'
          },
        ]
      }
    ]
  }




  navigateToUrl = (url: string | undefined) => this.router.navigateByUrl(url!);

}
// (
//   'blue': $blue,
//   'indigo': $indigo,
//   'purple': $purple,
//   'pink': $pink,
//   'red': $red,
//   'orange': $orange,
//   'yellow': $yellow,
//   'green': $green,
//   'teal': $teal,
//   'cyan': $cyan,
//   'white': $white,
//   'gray': $gray-600,
//   'gray-dark': $gray-800
// ),