import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { AppComponent } from '../app.component';
import { LoginService } from '../Services/login.service';
import { LoginModel } from '../Structures/structures';

@Component({
  selector: 'app-signin',
  templateUrl: './signin.component.html',
  styleUrls: ['./signin.component.scss']
})
export class SigninComponent implements OnInit {
  signInForm: FormGroup;
  loginFailed: boolean = false; 
  constructor(private fb: FormBuilder, private loginService: LoginService, private router: Router,
              private parent: AppComponent)
   {
    this.signInForm = this.fb.group({
      email: ['', Validators.required],
      password: ['', Validators.required]
    });
  }

  ngOnInit(): void {
  }

  login(): void { 
    var email = this.signInForm.get("email")?.value; 
    var password = this.signInForm.get("password")?.value; 

    var l = { userName: email, password: password } as LoginModel; 
    this.loginService.login(l).subscribe(result => {
      localStorage.setItem("HRV_TOKEN", result.token); 
      localStorage.setItem("HRV_TOKEN_EXP", result.expiration.toString());
      localStorage.setItem("USER_ID", result.userName);
      localStorage.setItem("LAST_REFRESH", (new Date()).toString()); 

      this.parent.checkToken();
      this.parent.userSignedIn = true; 
      this.router.navigate(['hrv']);
      this.loginFailed = false; 
    }, error => { 
      console.log(error); 
      localStorage.removeItem("HRV_TOKEN"); 
      localStorage.removeItem("HRV_TOKEN_EXP");
      localStorage.removeItem("USER_ID");
      localStorage.removeItem("LAST_REFRESH"); 

      this.loginFailed = true;
    });
  }

}
