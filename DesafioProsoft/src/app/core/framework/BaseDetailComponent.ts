import { Directive, OnInit } from '@angular/core';
import { Location } from '@angular/common';
import { FormGroup, FormBuilder, ValidationErrors } from '@angular/forms';
import { Observable, Subject, empty } from 'rxjs';
import { BaseState } from './models/BaseState';
import { BaseStore } from './utils/BaseStore';
import { BaseService } from './BaseService';
import { catchError, retry, tap } from 'rxjs/operators';
import { ToastrService } from 'ngx-toastr';
import * as swalFunctions from './utils/sweet-alerts';
import { RetornoApi } from './models/RetornoApi';
import { HttpErrorResponse } from '@angular/common/http';
import { AppInjector } from './service/app-injector.service';
import { ErrorMessage } from './models/ErrorMessage';
import { ActivatedRoute, Router } from '@angular/router';
import { LoginService } from './auth/LoginService';
import { LogAcaoService } from '@app-core/service/seguranca/logacao.service';
import { Log_Acao } from '@app-core/models/seguranca/logacao.model';

@Directive()
export abstract class BaseDetailComponent<E> implements OnInit {

  swal = swalFunctions;
  protected toastr: ToastrService;
  protected location: Location;
 
  formDetail: FormGroup;
  states: { state: BaseState<E>; stateParent: BaseState<any> } = {
    state: new BaseState<E>(),
    stateParent: new BaseState<any>()
  };
  public dataPage = {
    name: '', nodeName: 'ROOT'
  };
  listValidation: any = {};
  actionScreen: string;
  readOnly: boolean = false;
  retornoApi$: Observable<RetornoApi<E>>;
  dictID: { [id: string]: any; } = {};

  fieldsDate: string[] = [];

  dados$: Observable<RetornoApi<E[]>>;
  error$ = new Subject<ErrorMessage>();

  situacao = [{ situ_tx_situacao: '', tx_situacao: 'Todos' },
  { situ_tx_situacao: 'A', tx_situacao: 'Ativo' },
  { situ_tx_situacao: 'I', tx_situacao: 'Inativo' }];



  constructor(protected activatedRoute: ActivatedRoute,
    protected router: Router,
    protected store: BaseStore<E>,
    protected service: BaseService<E>,
    
    protected formBuilder?: FormBuilder ) {
    const injector = AppInjector.getInjector();
    

    this.toastr = injector.get(ToastrService);
    this.location = injector.get(Location);

    if ( this.formBuilder == null ) {
      this.formBuilder = injector.get(FormBuilder);
    }

  }

  onStates() {
    this.states = {
      ...this.store.InitStore(this.dataPage.nodeName, this.states.state)
    };

    this.states.state = {
      ...this.store.setState(this.dataPage.nodeName, this.states.state, {
        selectedItem: null,
        id: null,
        action: "list"
      })
    };
  }


  public ngOnInit() {

    this.onStates();
    this.getActivatedRoute();

  }


  onTrataData(): any {
    let arrayRetorno: any = {};
    this.fieldsDate.forEach((x) => {
      const data: any = this.formDetail.get(x) === null ? null : this.formDetail.get(x).value;
      if ( (data != null) && ( data !== "" ) ) {
        arrayRetorno[x] = new Date(data['year'], data['month'] - 1, data['day'], 0, 0, 0, 0);
      }
    });

 
    return arrayRetorno;
  }

  abstract getId(): string;

  onPosActivateRoute():void {

  }

  getActivatedRoute() {
    this.activatedRoute.paramMap.subscribe(params => {

      this.actionScreen = this.activatedRoute.snapshot.url[1].path;
      if (this.actionScreen !== 'create') {
        this.dictID["id"] = this.activatedRoute.snapshot.paramMap.get("id");
        this.getById(this.dictID["id"]);
      } else {
        this.retornoApi$ = this.store.setObserver<RetornoApi<E>>({});
      }
      this.readOnly = !((this.actionScreen === 'create') || (this.actionScreen === 'update'))
      this.onPosActivateRoute();
  
      this.createForm();
    });
  }

  getById(id: any) {
    this.retornoApi$ = this.service.getById(id).pipe(
      tap(record => this.setFormDetail(record.dados)),
      retry(0),
      catchError(
        (e) => this.handleError(e)));
  }

  createForm() {
    this.formDetail = this.formBuilder.group({});
  }

  setFormDetail(model: E) {

    var modelForm = { ...model };
    this.fieldsDate.map( x => {

        if ( model[x] ) {
          var dataTransform = new Date(  model[x])
          modelForm[ x ] =  {
            year: dataTransform.getFullYear() ,
            month: dataTransform.getMonth()+1 ,
            day: dataTransform.getDate(),
            hour: dataTransform.getDate(),
            minute: dataTransform.getMinutes(),
            sencond: dataTransform.getSeconds(),
            milliseconds: dataTransform.getMilliseconds()
          };

        }
    });
    if (model) {
      this.formDetail.patchValue(modelForm);
    }
    this.states.state = {
      ...this.store.setState(this.dataPage.nodeName, this.states.state, {
        selectedItem: model,
        id: model[this.getId()]
      })
    };
  }

  onPosInvalid() {
    return true;
  }

  goBack() {
    this.location.back();
  }

  onPosSubmit( ) {

    var log: Log_Acao = {};
    log.loac_tx_funcionalidade =  this.dataPage.name;
    log.loac_txacao = this.actionScreen;
    log.loac_txdados = JSON.stringify(this.getFormSend()) ;

    this.service.saveLog(this.actionScreen, this.dictID.id,log).subscribe(item => null);
    return;
  };

  onMessageRetorno(isSuccess: boolean) {

    let action = (isSuccess ? 'atualizado' : 'atualizar');
    if (this.actionScreen == 'create') {
      action = (isSuccess ? 'criado' : 'criar');
    } else if (this.actionScreen == 'delete') {
      action = (isSuccess ? 'apagado' : 'apagar');
    }
    if (isSuccess) {
      return `${this.dataPage.name} ${action} com sucesso!`;
    } else {
      return `Erro ao ${action} ${this.dataPage.name}, tente novamente!`
    }

  }


  onPreFormSubmit(formDetail: any): any {
    return formDetail;
  }

  getFormSend(): any {

    return {
      ... this.onPreFormSubmit(this.formDetail.getRawValue()),
      ... this.onTrataData()

    } as any;
  }


  onSubmit(exit: boolean) {
    if (this.actionScreen == 'delete')
      return this.onDelete();

    if (this.formDetail.invalid) {
      if (!this.onPosInvalid()) return;
      this.alertMsg();
      return;
    } else {
      this.service.save(this.actionScreen, this.dictID.id, this.getFormSend())
        .subscribe(
          item => {

            if (!item.error) {
             
              this.toastr.success(this.onMessageRetorno(true), "Atenção", {
                closeButton: true, enableHtml: true,
              });
              this.onPosSubmit();
              if (exit)
                this.location.back();
              else {
                if (this.actionScreen == 'create') {
                  let url = this.router.routerState.snapshot.url.replace('create', 'update');
                 
                  this.router.navigate([`${url}/${item.dados[this.getId()]}`], { replaceUrl: true, relativeTo: this.activatedRoute });
                 

                }

              }

           
            } else {
              this.toastr.warning(this.onMessageRetorno(false), "Atenção", {
                closeButton: true, enableHtml: true,
              });
            }


          },
          error => this.toastr.warning(this.onMessageRetorno(false), "Atenção", {
            closeButton: true, enableHtml: true,
          })
        );
    }

  }

  onDelete() {

    var callback = (result: any): void => {
      if (result.value) {

        this.service.delete(this.dictID.id)
          .subscribe(
            item => {
              if (!item.error) {
                this.toastr.success(this.onMessageRetorno(true), "Atenção", {
                  closeButton: true, enableHtml: true,
                });
                this.onPosSubmit();
                this.location.back();
              } else {
                this.toastr.warning(this.onMessageRetorno(false), "Atenção", {
                  closeButton: true, enableHtml: true,
                });
              }


            },
            error => this.toastr.warning(this.onMessageRetorno(false), "Atenção", {
              closeButton: true, enableHtml: true,
            })
          );
      }
    }
    this.swal.Confirma("Alerta", "Confirma deleção ?", 'warning', callback);

  }

  alertMsg(form: FormGroup = this.formDetail) {
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
