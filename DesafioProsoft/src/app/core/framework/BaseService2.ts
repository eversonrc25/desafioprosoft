import { environment } from './../../../environments/environment';
import { HttpHeaders, HttpClient, HttpErrorResponse } from '@angular/common/http';
import { Observable, throwError } from 'rxjs';
import { RetornoApi } from './models/RetornoApi';
import { retry, catchError } from 'rxjs/operators';
import { AppInjector } from './service/app-injector.service';


export abstract class BaseService2<T> {

  
  public baseURL = environment.urlApi;
  httpOptions = {
    headers: new HttpHeaders({ 'Content-Type': 'application/json' })
  };

  constructor(protected http: HttpClient, protected routeAPI: string) {
    

  }

  getList(model: T, page: number, qtdItens: number): Observable<RetornoApi<T[]>> {
    const url = `${this.baseURL}/${this.routeAPI}?PAG_C=${page}&QTD_I=${qtdItens}${this.montaQuery(model)}`;

    return this.http.get<RetornoApi<T[]>>(url, this.httpOptions)
      .pipe(
        retry(0),
        catchError(this.handleError))
  }

  getById(id: any): Observable<RetornoApi<T>> {
    const url = `${this.baseURL}/${this.routeAPI}/${id}`;

    return this.http.get<RetornoApi<T>>(url, this.httpOptions)
      .pipe(
        retry(0),
        catchError(this.handleError))
  }

  getByIdChild( routeParent: String, idParent: any,  idChild: any): Observable<RetornoApi<T>> {

    const url = `${this.baseURL}/${routeParent}/${idParent}/${this.routeAPI}/${idChild}`;

    return this.http.get<RetornoApi<T>>(url, this.httpOptions)
      .pipe(
        retry(0),
        catchError(this.handleError))
  }


  getListChild(model: T, idParent: any, routeParent: String, page: number, qtdItens: number): Observable<RetornoApi<T[]>> {

    const url = `${this.baseURL}/${routeParent}/${idParent}/${this.routeAPI}?PAG_C=${page}&QTD_I=${qtdItens}${this.montaQuery(model)}`;

    return this.http.get<RetornoApi<T[]>>(url, this.httpOptions)
      .pipe(
        retry(0),
        catchError(this.handleError))
  }


  private create(record: T): Observable<RetornoApi<T>> {
    return this.http.post<RetornoApi<T>>(`${this.baseURL}/${this.routeAPI}`, record).pipe(
      retry(0),
      catchError(this.handleError));//.pipe(take(1));
  }

  private update(idParent: any, record: T) : Observable<RetornoApi<T>>{
    return this.http.put<RetornoApi<T>>(`${this.baseURL}/${this.routeAPI}/${idParent}`, record).pipe(
      retry(0),
      catchError(this.handleError));//.pipe(take(1));
  }

  save(action: string, idParent: any, record: T): Observable<RetornoApi<T>> {
    if (action == 'update') {
      return this.update(idParent, record);
    }
    return this.create(record);
  }

  saveChild(action: string, routeParent: string, idParent: string, idChild: any, record: T): Observable<RetornoApi<T>> {
    if (action == 'update') {
      return this.updateChild(routeParent, idParent, idChild,  record);
    }
    return this.createChild(routeParent, idParent, record);
  }


  private createChild(routeParent: string, idParent: string, record: T): Observable<RetornoApi<T>> {
    return this.http.post<RetornoApi<T>>(`${this.baseURL}/${routeParent}/${idParent}/${this.routeAPI}`, record).pipe(
      retry(0),
      catchError(this.handleError));//.pipe(take(1));
  }

  private updateChild(routeParent: string, idParent: string, idChild: string,  record: T) : Observable<RetornoApi<T>>{
    return this.http.put<RetornoApi<T>>(`${this.baseURL}/${routeParent}/${idParent}/${this.routeAPI}/${idChild}`, record).pipe(
      retry(0),
      catchError(this.handleError));//.pipe(take(1));
  }


  delete(idParent: any) : Observable<RetornoApi<T>> {
    return this.http.delete<RetornoApi<T>>(`${this.baseURL}/${this.routeAPI}/${idParent}`).pipe(
      retry(0),
      catchError(this.handleError));//.pipe(take(1));
  }

  deleteChild(routeParent: string,  idParent: string, idChild: any) : Observable<RetornoApi<T>> {
    return this.http.delete<RetornoApi<T>>(`${this.baseURL}/${routeParent}/${idParent}/${this.routeAPI}/${idChild}`).pipe(
      retry(0),
      catchError(this.handleError));//.pipe(take(1));
  }

  handleError(error: HttpErrorResponse) {

    let errorMessage: any;
    if (error.error instanceof ErrorEvent) {
      // Erro ocorreu no lado do client
      errorMessage = { error: true, status: error.status, message: error.error.message };
    } else {
      // Erro ocorreu no lado do servidor
      errorMessage = { error: true, status: error.status, message: error.message };
    }
    return throwError(errorMessage);
  };

  montaQuery(model: any, charInit = '&'): string {
    let _queryString = charInit;

    for (const key in model) {
      if (model[key] !== '') {
        if (model[key]) {

          if (typeof model[key].getMonth === 'function') {

            _queryString +=
              key + '=' + encodeURIComponent(model[key].toISOString()) + '&';
          } else {
            _queryString += key + '=' + encodeURIComponent(model[key]) + '&';
          }
        }
      }
    }
    _queryString = _queryString.substring(0, _queryString.length - 1);

    return _queryString;
  }

}
