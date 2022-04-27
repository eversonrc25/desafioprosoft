import { Injectable, Injector } from '@angular/core';
import {
  HttpInterceptor,
  HttpRequest,
  HttpResponse,
  HttpHandler,
  HttpEvent,
  HttpHeaders
} from '@angular/common/http';


import { LoginService } from './LoginService';
import { Observable } from 'rxjs';
import { tap } from 'rxjs/operators';

@Injectable()
export class AuthInterceptor implements HttpInterceptor {

  constructor(private injector: Injector) { }

  intercept(
    request: HttpRequest<any>,
    next: HttpHandler
  ): Observable<HttpEvent<any>> {
    const loginService = this.injector.get(LoginService);

    
    if (loginService.isLoggedIn()) {          

      const authRequest = request.clone({
        headers: new HttpHeaders({
          'Authorization': `Bearer ${loginService.accessToken}`,
          'Cache-Control': 'no-cache',
          'Pragma': 'no-cache',
          'Expires': 'Sat, 01 Jan 2000 00:00:00 GMT'
        }),
        withCredentials: true
      });
      
      return next.handle(authRequest);
    } else {
      
      return next.handle(request).pipe(
        tap(event => {
          if (event instanceof HttpResponse) {

            // do stuff with response if you want
          }
        }
          , (error: any) => {
 
            let errorMessage: any;
            if ( error.error.mensagem)
               errorMessage = { error: true, status: error.status, message: error.error.mensagem };
            else {
              errorMessage = { error: true, status: error.status, message: error.message };
            }   
  
        //   console.log(error)
        //   let errorMessage: any;
        //   if (error.error instanceof ErrorEvent) {
        //     // Erro ocorreu no lado do client
        //     errorMessage = { error: true, status: error.status, message: error.error.message };
        //   } else {
        //     // Erro ocorreu no lado do servidor
        //     errorMessage = { error: true, status: error.status, message: error.message };
        //   }

        //   if (error.status === 401) {
        //     // redirect to the login route
        //     // or show a modal
        //   }
        //   console.log( 1 )

         }
        ));
    }
  }
}