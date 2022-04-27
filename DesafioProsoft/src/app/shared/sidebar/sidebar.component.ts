import { Component, AfterViewInit, OnInit } from '@angular/core';
import { ROUTES } from './menu-items';
import { RouteInfo } from './sidebar.metadata';
import { Router, ActivatedRoute } from '@angular/router';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { LoginService } from '@framework-core/auth/LoginService';
import { empty, Observable } from 'rxjs';
import { User } from '@framework-core/auth/user.model';

import { RetornoApi } from '@framework-core/models/RetornoApi';
import { catchError, tap } from 'rxjs/operators';

@Component({
  selector: 'app-sidebar',
  templateUrl: './sidebar.component.html'
})
export class SidebarComponent implements OnInit {
  currentUser$: Observable<User>;
  dados$: Observable<RetornoApi<RouteInfo[]>>;
  showMenu = '';
  showSubMenu = '';
  public sidebarnavItems: RouteInfo[] = [];
  // this is for the open close
  addExpandClass(element: any) {
    if (element === this.showMenu) {
      this.showMenu = '0';
    } else {
      this.showMenu = element;
    }

  }
  addActiveClass(element: any) {
    if (element === this.showSubMenu) {
      this.showSubMenu = '0';
    } else {
      this.showSubMenu = element;
    }
    window.scroll({
      top: 0,
      left: 0,
      behavior: 'smooth'
    });
  }

  constructor(
    private modalService: NgbModal,
    private router: Router,
    private route: ActivatedRoute,
    private loginService: LoginService
    
  ) {

    this.currentUser$ = this.loginService.currentUserValue();
  }

  // End open close
  ngOnInit() {

      
        ROUTES.filter(sidebarnavItem => sidebarnavItem).forEach( (item) => {
          this.sidebarnavItems.push( item );
        })

    // this.viewzohoService.getRotasByUser().subscribe(
    //   // tap(registro => {
    //     registro => {
         
    //     var tempo: RouteInfo = {
    //       path: '',
    //       title: 'Dashboard',
    //       icon: 'mdi mdi-bullseye',
    //       class: 'has-arrow',
    //       extralink: false,
    //       submenu: registro.dados
    //     };
    //     this.sidebarnavItems.push( tempo );
    //     ROUTES.filter(sidebarnavItem => sidebarnavItem).forEach( (item) => {
    //       this.sidebarnavItems.push( item );
    //     })
    //     //this.sidebarnavItems.push( tempo );
    //     //this.sidebarnavItems = [ tempo, ROUTES.filter(sidebarnavItem => sidebarnavItem) ]
    //     //this.sidebarnavItems = ROUTES.filter(sidebarnavItem => sidebarnavItem);

    //   }
    //    ,
    //   catchError(error => {
    //     console.error(error);

    //     return empty();
    //   }
    //   )
    // );

  }

  getAvatar(nome: string) {
    return this.loginService.getAvatarLetter(nome);
  }

  logout() {
    this.loginService.logout();
  }
}
