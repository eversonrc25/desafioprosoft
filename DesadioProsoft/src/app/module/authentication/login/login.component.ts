import { Component } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { LoginService } from '@framework-core/auth/LoginService';
import { ToastrService } from 'ngx-toastr';
import { Observable, of } from 'rxjs';
 
@Component({
  selector: 'app-login',
  templateUrl: './login.component.html' 
})
export class LoginComponent {
 
  loginForm?: FormGroup; 
  navigateTo?: string;
  isLogin = true;
  loading$!: Observable<boolean> | false;
  
  constructor(private fb: FormBuilder,
    private loginService: LoginService,
    private activatedRoute: ActivatedRoute,
    protected toastr: ToastrService,
    private router: Router,
  ) { }

  
  ngOnInit() {
    this.loginForm = this.fb.group({
      email: this.fb.control('', [Validators.required]),
      password: this.fb.control(null, [Validators.required])
    });
    this.navigateTo = 'starter'; //this.activatedRoute.snapshot.params['to'] || btoa('/')
  }


  login() {
    this.loading$ = of(true);
    this.loginService
      .login(this.loginForm?.value.email, this.loginForm?.value.password)
      .subscribe(
        user => {
           this.loading$ = of(false);
          this.toastr.show(`Bem vindo, <strong>${user.nome}</strong>`, "Atenção", {
            closeButton: true, enableHtml: true,
          });
         
        },
        response => {
          
          if (response) {
            this.toastr.error(`<strong>Usuário não autorizado!</strong>`, "Atenção", {
              closeButton: true, enableHtml: true,
            });
           
          } else {
            this.toastr.error(`<strong>${response.error.message}</strong>`, "Atenção", {
              closeButton: true, enableHtml: true,
            });
            
          }
          localStorage.clear();
          this.loading$ = of(false);
        },
        () => {
          if ( this.navigateTo ) 
             this.router.navigate([ this.navigateTo ]);
        }
      );
  }


 
 
}
