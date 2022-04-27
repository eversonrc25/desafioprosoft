import { Component, ElementRef, ViewChild } from '@angular/core'
import { ActivatedRoute, Router } from '@angular/router';
import { FormBuilder, FormControl, Validators } from '@angular/forms';
import { BaseDetailComponent } from '@framework-core/BaseDetailComponent';
import { BaseStore } from '@framework-core/utils/BaseStore';


import { Dev } from '@app-core/models/dev/dev.model';
import { DevService } from '@app-core/service/dev/dev.service';


@Component({
  selector: 'app-dev-crud',
  templateUrl: './dev-crud.component.html'
})
export class DevCrudComponent extends BaseDetailComponent<Dev>  {


  titulo = 'Devs'

 
  @ViewChild('inputFile', {static: false}) inputFile: ElementRef;


  constructor(protected activatedRoute: ActivatedRoute,
    protected router: Router,
    protected store: BaseStore<Dev>,
    protected service: DevService,

    protected formBuilder: FormBuilder) {
    super(activatedRoute, router, store, service, formBuilder);
    this.dataPage.name = 'Dev'
   
  }

  getId(): string {
    return 'id'
  }


  ngOnInit() {
    super.ngOnInit()

  }


  createForm() {

    super.createForm();
 

    this.formDetail.addControl('name', new FormControl({ value: null, disabled: this.readOnly },
      [Validators.required]));
    this.formDetail.addControl('squad', new FormControl({ value: null, disabled: this.readOnly },
      [Validators.required]));
    this.formDetail.addControl('email', new FormControl({ value: null, disabled: this.readOnly }, [Validators.required, Validators.email]));
    this.formDetail.addControl('avatar', new FormControl({ value: null, disabled: this.readOnly }, 
      [ ]));
    this.formDetail.addControl('login', new FormControl({ value: null, disabled: this.readOnly }, 
      [  Validators.maxLength(100),Validators.required]));
   

    this.listValidation = {
      name: { required: 'Nome deve ser informada', maxlength: 'Descrição não pode ser maior que 100 caracteres' },
      squad: { required: 'Squad deve ser informada', maxlength: 'Versão não pode ser maior que 100 caracteres' },
      email: { required: 'Email deve ser informada',email: 'E-mail em formato incorreto',maxlength: 'Descrição não pode ser maior que 100 caracteres'  },
      login: { required: 'login deve ser informada',maxlength: 'Descrição não pode ser maior que 100 caracteres'  },
     
    };

  }

 
  }


