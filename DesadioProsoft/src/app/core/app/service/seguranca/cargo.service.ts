import { Injectable } from '@angular/core';
import { BaseService } from '@framework-core/BaseService';
import { Cargo } from '@app-core/models/seguranca/cargo.model';

import { RetornoApi } from '@framework-core/models/RetornoApi';
import { Observable } from 'rxjs';
import { retry, catchError } from 'rxjs/operators';

@Injectable({
  providedIn: 'root'
})

export class CargoService extends BaseService<Cargo> {

  constructor() {
    super( 'cargo');
  }

  getListCombo(model: Cargo, page: number, qtdItens: number): Observable<RetornoApi<Cargo[]>> {
    const url = `${this.baseURL}/${this.routeAPI}/combo?QTD_I=${qtdItens}${this.montaQuery(model)}`;

    return this.http.get<RetornoApi<Cargo[]>>(url, this.httpOptions)
      .pipe(
        retry(0),
        catchError(this.handleError))
  }

}