import { CoreServices } from './../../../Models/core-services.Model';
// angular import
import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';

// project import
import { SharedModule } from 'src/app/theme/shared/shared.module';

@Component({
  selector: 'app-sample-page',
  standalone: true,
  imports: [CommonModule, SharedModule],
  templateUrl: './sample-page.component.html',
  styleUrls: ['./sample-page.component.scss']
})
export default class SamplePageComponent implements OnInit {

  coreServicesList: CoreServices[] = [];

  ngOnInit(): void {
    this.coreServicesList = [
      {
        title: 'Aadhar Services',
        imgSrc:'https://uidai.gov.in/images/langPage/Page-1.svg',
        services: [
          { name: 'Enrolment & Update' },
          { name: 'Mobile & Email Update' },
          { name: 'Smart Card' }
        ],
      },
      {
        title: 'Government Services',
        imgSrc:'https://cdn.educba.com/academy/wp-content/uploads/2015/05/24_P2.jpg',
        services: [
          { name: 'Police Clearance Certificate' },
          { name: 'Domicile Certificate' },
          { name: 'Birth Application' },
          { name: 'Mariage Certificate' },

        ],
      },
      {
        title: 'G2C Services',
        imgSrc:'https://5.imimg.com/data5/GB/QE/MJ/GLADMIN-72586713/hhy-500x500.png',
        services: [
          { name: 'Pan Card' },
          { name: 'Gumasta Licences' },
          { name: 'Driving Licences' },
          { name: 'Udyam Aadhar' },

        ],
      },
      {
        title: 'Election Services',
        imgSrc:'https://voterportal.eci.gov.in/nvsp/image/eci-logo.png',
        services: [
          { name: 'Voter ID Card' },
        ],
      },
      {
        title: 'Gazzatte Services',
        imgSrc:'https://egazette.gov.in/(S(pop3hw5ihdzx2v4gdnfa5p5t))/images/eglogofinal.png',
        services: [
          { name: 'Change of Name' },
          { name: 'Change of Surname' },
          { name: 'Change of Date of Birth' },

        ],
      },
      {
        title: 'RTO Services',
        imgSrc:'https://img.indianauto.com/2020/11/25/ZUwJ9d0r/rto-officies-in-chennai-1192.jpg',
        services: [
          { name: 'Insurance Renewal' },
          { name: 'Duplicate RC & DL' },
          { name: 'Change of Address' },
          { name: 'RC Book Transfer' },


        ],
      },

    ];
  }

}



