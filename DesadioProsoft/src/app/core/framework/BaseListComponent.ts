import {  Directive, OnInit } from '@angular/core';
import { FormGroup, FormBuilder, ValidationErrors } from '@angular/forms';
import { Observable, Subject, empty } from 'rxjs';
import { BaseState } from './models/BaseState';
import { BaseStore } from './utils/BaseStore';
import { BaseService } from './BaseService';
import { catchError, retry } from 'rxjs/operators';
import { ToastrService } from 'ngx-toastr';
import * as swalFunctions from './utils/sweet-alerts';
import { RetornoApi } from './models/RetornoApi';
import { HttpErrorResponse } from '@angular/common/http';
import { AppInjector } from './service/app-injector.service';
import { ErrorMessage } from './models/ErrorMessage';

@Directive()
export abstract class BaseListComponent<E> implements OnInit {
  swal = swalFunctions;
  protected toastr: ToastrService;
  //protected formBuilder: FormBuilder;
  private nameComponent: string;

  public dataPage = {
    nodeName: 'ROOT', isCallList: true,
    isClear: true, currentPage: 1, searchPage: 1, pageSize: 10
  };

  listValidation: any = {};
  formFilter: FormGroup;
  fieldsDate: string[] = [];

  dados$: Observable<RetornoApi<E[]>>;
  error$ = new Subject<ErrorMessage>();

  situacao = [{ situ_tx_situacao: '', tx_situacao: 'Todos' },
  { situ_tx_situacao: 'A', tx_situacao: 'Ativo' },
  { situ_tx_situacao: 'I', tx_situacao: 'Inativo' }];


  states: { state: BaseState<E>; stateParent: BaseState<any> } = {
    state: new BaseState<E>(),
    stateParent: new BaseState<any>()
  };


  constructor(   protected store: BaseStore<E>, protected service: BaseService<E>
    , protected formBuilder?: FormBuilder ) {
    const injector = AppInjector.getInjector();

    this.toastr = injector.get(ToastrService);
     
    if ( this.formBuilder == null ) {
      this.formBuilder = injector.get(FormBuilder);
    }

    this.nameComponent = this.constructor.name.toUpperCase();

  }

//   constructor(  
//      protected store?: BaseStore<E>, protected service?: BaseService<E>
//     ) {
//    const injector = AppInjector.getInjector();
//     console.log( "¨¨¨¨¨¨¨¨¨¨¨¨¨¨¨¨¨¨¨¨¨¨¨¨¨¨¨")
//    this.toastr = injector.get(ToastrService);
//    console.log( "¨¨¨¨¨¨¨¨¨¨¨¨¨¨¨¨¨¨¨¨¨¨¨¨¨¨¨")
//    // this.formBuilder = injector.get(FormBuilder);
//    console.log( "¨¨¨¨¨¨¨¨¨¨¨¨¨¨¨¨¨¨¨¨¨¨¨¨¨¨¨")
//    console.log( this.formBuilder)
//    console.log( "¨¨¨¨¨¨¨¨¨¨¨¨¨¨¨¨¨¨¨¨¨¨¨¨¨¨¨")
//    this.nameComponent = this.constructor.name.toUpperCase();

//  }


  onStates() {

    this.states = {
      ...this.store.InitStore(this.dataPage.nodeName, this.states.state, this.dataPage.isClear)
    };

    this.states.state = {
      ...this.store.setState(this.dataPage.nodeName, this.states.state, {
        nameComponent : this.nameComponent,
        filterForm : ( this.states.state.nameComponent != this.nameComponent ?null: this.states.state.filterForm ),
        selectedItem: null,
        id: null,
        pagina:  ( this.states.state.nameComponent != this.nameComponent ? 1: this.states.state.pagina ),
        action: 'list'
      })
    };
    
    this.dataPage.searchPage = this.states.state.pagina;
    this.dataPage.currentPage = this.states.state.pagina;

  }

  public ngOnInit() {

    this.onStates();
    this.createForm();
    this.setFormFiltro();
    if (this.dataPage.isCallList) {

      this.getList();
    }
  }

  onPosInvalid() {
    return true;
  }


  alertMsg(form: FormGroup = this.formFilter) {
    let msg = '<div class="row text-left "  style="font-size:20px;">';
    msg += '<ul>';
    this.getFormValidationErrors(form, this.listValidation).forEach(
      element => {
        msg += "<li >" + element.message + "</li>";
      }
    );
    msg += '<ul>';
    msg += "</div>";
    this.toastr.warning(msg, "Atenção", {
      closeButton: true, enableHtml: true,
    });
  }

  onSubmit() {
    if (this.formFilter.invalid) {
      if (!this.onPosInvalid()) return;
      this.alertMsg();
      return;
    } else {
      this.states.state = { ... this.store.setState(this.dataPage.nodeName, this.states.state, { filterForm: this.formFilter.getRawValue() }) };
      
      this.getList(true);
    }

  }

  onExcel() {
    if (this.formFilter.invalid) {
      if (!this.onPosInvalid()) return;
      this.alertMsg();
      return;
    } else {
        this.getListExcel(true);
    }

  }

  getList(isFilter = false) {

    if (isFilter) {
      this.dataPage.currentPage = 1;
      this.dataPage.searchPage = 1;
    }
    this.list(this.getFormFiltro(), this.dataPage.searchPage, this.dataPage.pageSize);
  }

  getListExcel(isFilter = false) {

    if (isFilter) {
      this.dataPage.currentPage = 1;
      this.dataPage.searchPage = 1;
    }
 
    this.listExcel(this.getFormFiltro(), this.dataPage.searchPage, this.dataPage.pageSize);
  }

  createForm() {
    this.formFilter = this.formBuilder.group({});
  }

  setFormFiltro() {
    const model: E = <E>this.states.state.filterForm;
    if (this.states.state.filterForm) {
      this.formFilter.patchValue(model);
    }
  }


  onPreFormFiltro(filtro: any): any {
    return filtro;
  }

  onTrataData(): any {
    let arrayRetorno: any = {};
    this.fieldsDate.forEach((x) => {
      const data: any = this.formFilter.get(x) === null ? null : this.formFilter.get(x).value;
      if ( (data != null) && ( data !== "" ) ) {
        arrayRetorno[x] = new Date(Date.UTC(data['year'], data['month'] - 1, data['day'], 0, 0, 0, 0));
      }
    });

    return arrayRetorno;
  }

  getFormFiltro(): any {

    return {
      ... this.onPreFormFiltro(this.formFilter.getRawValue()),
      ... this.onTrataData()

    } as any;
  }

  list(model: any, searchPage: number, pageSize: number) {
    
    this.dados$ = this.service.getList(model, searchPage, pageSize).pipe(
      retry(0),
      catchError(
        (e) => this.handleError(e)));
  }

  listExcel(model: any, searchPage: number, pageSize: number) {
    
     this.service.getListExcel(model, searchPage, pageSize);
  }


  getFormValidationErrors(form: FormGroup, listValid: any) {
    const result = [];
    Object.keys(form.controls).forEach(key => {
      const controlErrors: ValidationErrors = form.get(key).errors;
      if (controlErrors) {
        Object.keys(controlErrors).forEach(keyError => {
          result.push({
            control: key,
            error: keyError,
            message: listValid[key][keyError]
          });
        });
      }
    });

    return result;
  }

  setPage(page: number) {

    this.dataPage.searchPage = page;
    this.states.state = { ...this.store.setState(this.dataPage.nodeName, this.states.state, { pagina:  this.dataPage.searchPage }) };

    this.getList();
  }


  handleError(error: HttpErrorResponse) {

    let errorMessage: any;//= '';
    if (error.error instanceof ErrorEvent) {
      // Erro ocorreu no lado do client
      errorMessage = { error: true, status: error.status, message: error.error.message }; //errorMessage = error.error;// = error.error.message;
    } else {
      // Erro ocorreu no lado do servidor
      errorMessage = { error: true, status: error.status, message: error.message };//  error; // `Código do erro: ${error.status}, ` + `menssagem: ${error.message}`;
    }
     
    this.error$.next(errorMessage);
    return empty();
  };


}
