import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders, HttpErrorResponse } from '@angular/common/http';
import { Router, NavigationEnd } from '@angular/router';
import { environment } from '../../../../environments/environment';
import { User } from './user.model';
import { filter, map, retry, catchError } from 'rxjs/operators';
import { empty, Observable, throwError } from 'rxjs';
import { Usuario } from '@app-core/models/seguranca/usuario.model';
import { RetornoApi } from '@framework-core/models/RetornoApi';
import { ValidatorFn, AbstractControl } from '@angular/forms';
import { FormGroup } from '@angular/forms';
//import * as console from 'console';

@Injectable({
  providedIn: 'root'
})
export class LoginService {

  BASE_URL = `${environment.urlApi}/auth`;

  user: User;

  accessToken: string = null;
  lastUrl: string;

  httpOptions = {
    headers: new HttpHeaders({ 'Content-Type': 'application/json' })
  };

  constructor(private http: HttpClient, private router: Router) {
    this.router.events
      .pipe(filter(event => event instanceof NavigationEnd))
      .subscribe((e: NavigationEnd) => (this.lastUrl = e.url));
  }

  isLoggedIn(): boolean {

    //let token: any = null;
    if (this.accessToken === null) {
      this.accessToken = localStorage.getItem(environment.LOCSTORE_U);

      //if (token !== null) {
      //loginrefresh( )
      //  this.user = token;
      //}
    }

    return this.accessToken !== null;
  }



currentUserValue(): Observable<User> {

    return new Observable<User>((observer: any) => {
      //console.log( "currentUserValue 2" );
      if (this.user === undefined) {
        this.accessToken = localStorage.getItem(environment.LOCSTORE_U);
        if (this.accessToken !== null) {
          //console.log( "currentUserValue 5" );
          this.loginrefresh().subscribe(retorno => {
            //console.log( retorno );

            this.user = retorno;
            let dateExpiration = new Date(this.user.exp);

            if (dateExpiration.getTime() < Date.now()) {
              this.user = null;
            }
            //console.log(this.user)
            if (this.user == null)
              this.handleLogin();

            //console.log( "currentUserValue 6" );
            // return ( this.user );
            observer.next(this.user);
            return;
          }, (error) => {
            localStorage.clear();
            this.router.navigate(['auth/login']);
            return empty();
          });

        } else {
          //console.log( "currentUserValue 7" );
          
          this.handleLogin();
          observer.next(this.user);
          return;
        }


      } else {
        //console.log( "currentUserValue XXX" );
        observer.next(this.user)
        return;
      }
    });

 
  }

  getCurrentUser(): Observable<User> {

    return this.currentUserValue();


  }

  getRules(): string[] {

    return this.user.roles;
  }

  login(usuario: string, senha: string): Observable<User> {
    
    localStorage.clear();
    return this.http
      .post<User>(`${this.BASE_URL}/login`, {
        usuario: usuario,
        senha: senha,
      })
      .pipe(map(user => {
        localStorage.setItem(environment.LOCSTORE_U, user.accessToken);
        this.user = user
        this.accessToken = user.accessToken;
        return this.user;
      }));
  }

  loginrefresh(): Observable<User> {
    //localStorage.clear();
    return this.http
      .post<User>(`${this.BASE_URL}/login/refresh`, {})
      .pipe(map(user => {
        localStorage.setItem(environment.LOCSTORE_U, user.accessToken);
        this.accessToken = user.accessToken;
        this.user = user
        return this.user;
      }


      ));
  }



  logout() {
    this.user = {} as User;
    localStorage.clear();
    this.accessToken =  null;
    this.router.navigate(['auth/login']);
  }

  esqueciSenha(model: any): Observable<RetornoApi<Usuario[]>> {
    const modelSend = {
      dcemail: model.email,
      'camposAuxiliares.site': environment.SITE_URL,
    };
    return this.http.get<RetornoApi<Usuario[]>>(
      `${this.BASE_URL}/login/esquecisenha/?${this.montaQuery(
        modelSend
      )}`,
      {}
    );
  }

  validatoken(model: any): Observable<RetornoApi<Usuario>> {
    return this.http.get<RetornoApi<Usuario>>(
      `${this.BASE_URL}/login/validatoken/?${this.montaQuery(
        model
      )}`,
      {}
    ).pipe(
      retry(0),
      catchError(this.handleError));
  }

  alterasenha(model: any): Observable<RetornoApi<Usuario>> {
    //const modelSend = { dcemail: model.email, 'camposAuxiliares.site'  : environment.SITE_URL };

    return this.http.post<RetornoApi<Usuario>>(
      `${this.BASE_URL}/login/alterasenha`,
      model
    );
  }

  handleLogin() {
    localStorage.clear();
    this.router.navigate(['auth/login']);
  }

  montaQuery(model): string {
    let _queryString = '&';

    for (const key in model) {
      if (model[key] !== '') {
        if (model[key]) {
          if (typeof model[key].getMonth === 'function') {
            _queryString +=
              key +
              '=' +
              encodeURIComponent(model[key].toISOString()) +
              '&';
          } else {
            _queryString +=
              key + '=' + encodeURIComponent(model[key]) + '&';
          }
        }
      }
    }
    _queryString = _queryString.substring(0, _queryString.length - 1);

    return _queryString;
  }


  sendToken(token) {
    return this.http.post<any>(`${this.BASE_URL}/login/token_validate`, { recaptcha: token })
  }

  patternValidator(): ValidatorFn {
    return (control: AbstractControl): { [key: string]: any } => {
      if (!control.value) {
        return null;
      }
      const regex = new RegExp('^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9]).{8,}$');
      const valid = regex.test(control.value);
      return valid ? null : { invalidPassword: true };
    };
  }

  MatchPassword(password: string, confirmPassword: string) {
    return (formGroup: FormGroup) => {
      const passwordControl = formGroup.controls[password];
      const confirmPasswordControl = formGroup.controls[confirmPassword];

      if (!passwordControl || !confirmPasswordControl) {
        return null;
      }

      if (confirmPasswordControl.errors && !confirmPasswordControl.errors.passwordMismatch) {
        return null;
      }

      if (passwordControl.value !== confirmPasswordControl.value) {
        confirmPasswordControl.setErrors({ passwordMismatch: true });
      } else {
        confirmPasswordControl.setErrors(null);
      }
    }
  }

  handleError(error: HttpErrorResponse) {

    let errorMessage: any;
    if (error.error.mensagem)
      errorMessage = { error: true, status: error.status, message: error.error.mensagem };
    else {
      errorMessage = { error: true, status: error.status, message: error.message };
    }

    //console.log(errorMessage, 2)

    return throwError(errorMessage);
  };


  getAvatarLetter( nome: string) {
    var nomes = nome.trim().split( ' ' );
    if ( nomes.length >= 2) {
       return nomes[0].substring(0,1).toUpperCase() + nomes[1].substring(0,1).toUpperCase();
    } else {
      return nomes[0].substring(0,2).toUpperCase();
    }
  }


}
