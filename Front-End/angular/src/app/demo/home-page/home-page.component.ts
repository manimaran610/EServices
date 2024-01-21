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
  styles: [`

  *,
  *::before,
  *::after {
    margin: 0;
    padding: 0;
    box-sizing: border-box;
  }
  
  .body {
    --color: rgba(30, 30, 30);
    --bgColor: rgb(215, 215, 215);
    min-height: 100vh;
  
    display: grid;
    align-content: center;
    gap: 2rem;
    padding: 2rem;
  
    font-family: "Oxygen", sans-serif;
    color: var(--color);
    background: var(--bgColor);
  }
  
  h1 {
    text-align: center;
  }
  
  content-body {
    width: min(60rem, 90%);
    margin-inline: auto;
  
    display: flex;
    flex-wrap: wrap;
    gap: 2rem;
  
    list-style: none;
    counter-reset: cardnr;
    justify-content: center;
  }
  
  .li {
    --frontColor: white;
    --width: 15em;
    --inlineP: 0.5rem;
    --borderR: 4rem;
    --arrowW: 2rem;
    --arrowH: 1.5rem;
  
    counter-increment: cardnr;
    width: calc(var(--width) - var(--inlineP) * 2);
    display: grid;
    grid-template:
      "icon" var(--borderR)
      "title"
      "descr" 1fr;
    margin-inline: var(--inlineP);
    margin-bottom: calc(var(--borderR));
    position:relative;
    box-shadow: 0 6px 10px rgba(0,0,0,.08), 0 0 6px rgba(0,0,0,.0);
        transition: .3s transform cubic-bezier(.155,1.105,.295,1.12),.3s box-shadow,.3s -webkit-transform cubic-bezier(.155,1.105,.295,1.12);
  
    cursor: pointer;
  }
  .li .icon,
  .li .title,
  .li .descr {
    background: var(--frontColor);
    padding-inline: 1rem;
    padding-bottom: 1rem;
  }
  .li .icon,
  li .title {
    color: var(--accent-color);
    text-align: center;
    padding-bottom: 0.5rem;
  }
  
  .li .title,
  .li .descr {
    filter: drop-shadow(0.125rem 0.125rem 0.075rem rgba(0, 0, 0, 0.25));
  }
  .li .icon {
    grid-area: icon;
    font-size: 2rem;
    display: grid;
    place-items: center;
    border-radius: var(--borderR) 0 0 0;
    position: relative;
  }
  
  .li .title {
    grid-area: title;
    font-size: 1.1rem;
    font-weight: bold;
  }
  
  .li .descr {
    grid-area: descr;
    font-size: 0.85rem;
    text-align: center;
  }
  .li .descr::before {
    content: "";
    width: var(--arrowW);
    height: var(--arrowH);
    position: absolute;
    right: 1.5rem;
    top: 100%;
    background: inherit;
    clip-path: polygon(0 0, 100% 0, 50% 100%);
  }
  
  .li::after{
  
    content: counter(cardnr, decimal-leading-zero);
    position: absolute;
    z-index: -3;
  
    left: calc(var(--inlineP) * -1);
    right: calc(var(--inlineP) * -1);
    top: var(--borderR);
    bottom: calc(var(--borderR) * -1);
  
    display: flex;
    align-items: flex-end;
    background: var(--accent-color);
    background-image: linear-gradient(
      160deg,
      rgba(255, 255, 255, 0.25),
      transparent 25% 75%,
      rgba(0, 0, 0, 0.25)
    );
    border-radius: 0 0 var(--borderR) 0;
  
    --pad: 1rem;
    padding: var(--pad);
    font-size: calc(var(--borderR) - var(--pad) * 2);
    color: white;
  }
  .li::before {
    content: "";
    position: absolute;
    height: var(--borderR);
    top: calc(100% + var(--borderR) - 2px);
    left: calc(var(--inlineP) * -1);
    right: calc(var(--inlineP) * -1);
    border-radius: 0 var(--borderR) 0 0;
  
    background-image: linear-gradient(var(--accent-color), transparent 60%);
    opacity: 0.5;
    filter: blur(2px);
  }
  
  .credits {
    margin-top: 2rem;
    text-align: right;
  }
  .credits a {
    color: var(--color);
  }
  
  .card{
      border-radius: 4px;
      background: #fff;
      box-shadow: 0 6px 10px rgba(0,0,0,.08), 0 0 6px rgba(0,0,0,.05);
        transition: .3s transform cubic-bezier(.155,1.105,.295,1.12),.3s box-shadow,.3s -webkit-transform cubic-bezier(.155,1.105,.295,1.12);
    padding: 14px 80px 18px 36px;
    cursor: pointer;
  }
  
  .li:hover{
       transform: scale(1.12);
  
  }
  `],

  template: `<div class="col-sm-12">
  <div>
    <h2 class="text-center">Welcome to AHU Portal !</h2>
  </div>
  <hr>
  <div class="row" *ngFor="let coreService of coreServicesList">
  <hr>
    <div class="m-5 ">
      <h4 class="title">{{coreService.title}} :</h4>
      <div class="row content-body">
        <div (click)="navigateToUrl(item.navigateTo)" class="col-ld-3 col-md-4 col-sm-12 li m-5"
          *ngFor="let item of coreService.services" style="--accent-color:#0D6EFD">
          <i class="icon feather {{ item.icon }}"></i>
          <h1 class="title">{{item.title}} <span>{{item.subTitle}}</span></h1>
          <div class="descr">Description of {{item.title}}</div>
        </div>
      </div>
    </div>
  </div>
</div>`
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
            accentColor: '#0D6EFD',
            icon: 'icon icon-anchor ',
            navigateTo: '/Reports/ACPH'
          },
          {
            title: 'Filter Integrity',
            background: 'bg-c-green',
            icon: 'icon icon-filter',
            accentColor: '#6710F5',
            navigateTo: '/Reports/FilterIntegrity'
          },
          {
            title: 'Particle count',
            subTitle: '(Single Cycle)',
            background: 'bg-c-yellow',
            icon: 'icon icon-codepen',
            accentColor: '#6F42C1',
            navigateTo: '/Reports/ParticleCount/SingleCycle'
          },
          {
            title: 'Particle count',
            subTitle: '(Three Cycle)',
            background: 'bg-c-red',
            icon: 'icon icon-codepen',
            accentColor: 'blue',
            navigateTo: '/Reports/ParticleCount/ThreeCycle'
          },
          {
            title: 'Particle count',
            subTitle: '(Recv Cycle)',
            background: 'bg-c-red',
            icon: 'icon icon-codepen',
            accentColor: 'blue',
            navigateTo: '/NotFound'
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
            accentColor: '#0D6EFD',
            navigateTo: '/Instrument/Add-Instrument'
          },
        ]
      },
      {
        title: 'Trainee Details',
        services: [
          {
            title: 'Add Trainee',
            background: 'bg-c-blue',
            icon: 'icon icon-framer',
            accentColor: '#0D6EFD',
            navigateTo: '/Trainee/Add-Trainee'
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