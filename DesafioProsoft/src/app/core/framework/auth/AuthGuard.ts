import { Injectable } from '@angular/core';
import {
    Router,
    CanActivate,
    ActivatedRouteSnapshot,
    RouterStateSnapshot,
} from '@angular/router';

import { Observable, of } from 'rxjs';
import { map } from 'rxjs/operators';
import { LoginService } from './LoginService';
declare var require
const Swal = require('sweetalert2')

@Injectable({ providedIn: 'root' })
export class AuthGuard implements CanActivate {
    constructor(private router: Router, private loginService: LoginService) {}

    canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot) : Observable<boolean> {
        //console.log( "canActivate")
        return this.loginService.currentUserValue().pipe( map( user => {
            //console.log( "canActivate 1 ")
            //console.log( user )
            var retorno =  user != undefined 
            //console.log( "canActivate2 ")
            if (retorno ) {
                //console.log( "canActivate 3")
                return    this.verificarAcesso(route,state,user);
            } else {
                //console.log( "canActivate 4 ")
                this.router.navigate(['/auth/login'], {
                    queryParams: { returnUrl: state.url },
                });
                return   false ;
            }
               
        }));
        

      
    }

    verificarAcesso(route,state,currentUser){

        let retorno = true
        
          if (!currentUser.routes.some(v => {
            
            return route.routeConfig.path === v
        })) {
 
            this.router.navigate(['/starter'], {
                queryParams: { returnUrl: state.url },
            });

            Swal.fire({
                title: "Acesso Negado!",
                 
                showConfirmButton: false,
                timer: 5000                  
                
              })

              retorno = false
   
        }
        
    
        return retorno;
 
}


}
