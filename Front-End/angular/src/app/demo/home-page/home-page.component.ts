/* eslint-disable @typescript-eslint/no-unused-vars */
import { CoreServices } from '../../../Models/core-services.Model';
// angular import
import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';

// project import
import { SharedModule } from 'src/app/theme/shared/shared.module';

@Component({
  selector: 'app-home-page',
  standalone: true,
  imports: [CommonModule, SharedModule],
  templateUrl: './Home-page.component.html',
  styleUrls: ['./Home-page.component.scss']
})
export default class SamplePageComponent implements OnInit {

  coreServicesList: CoreServices[] = [];

  ngOnInit(): void {
    this.coreServicesList = [
      {
        title: 'Reports',
        services: [
          {
            title: 'ACPH',
            background: 'bg-c-blue',
            icon: 'icon ',
            navigateTo: '/form1'
          },
          {
            title: 'Filter Integrity',
            background: 'bg-c-blue',
            icon: 'icon ',
            navigateTo: '/form1'
          },
          {
            title: 'Particle count',
            subTitle:'(Single Cycle)',
            background: 'bg-c-blue',
            icon: 'icon ',
            navigateTo: '/form1'
          },
          {
            title: 'Particle count',
            subTitle:'(Three Cycle)',
            background: 'bg-c-blue',
            icon: 'icon ',
            navigateTo: '/form1'
          },
          {
            title: 'Temp. Mapping',
            background: 'bg-c-blue',
            icon: 'icon ',
            navigateTo: '/form1'
          },

        ]
      },
      {
        title:'Instrument Details',
        services:[
          {
            title: 'Add Instrument',
            background: 'bg-c-blue',
            icon: 'icon ',
            navigateTo:'/form1'
          }, 
        ]
      }
    ]
  }

}



