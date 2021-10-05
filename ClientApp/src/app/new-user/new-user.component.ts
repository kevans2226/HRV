import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, ValidationErrors, ValidatorFn, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { LoginService } from '../Services/login.service';
import { newUser, ComparePassword } from '../Structures/structures';



@Component({
  selector: 'app-new-user',
  templateUrl: './new-user.component.html',
  styleUrls: ['./new-user.component.scss']
})
export class NewUserComponent implements OnInit {
  
  public formNewUser: FormGroup; 

  constructor(private fb: FormBuilder, private login: LoginService, private route: Router) { 
    this.formNewUser = fb.group({
      email: ['', [Validators.required,Validators.email]],
      userName: ['', Validators.required],
      firstName: ['', Validators.required], 
      lastName: ['', Validators.required], 
      password: ['', Validators.required],
      confirmPassword: ['', Validators.required]
    },
    {
      // Used custom form validator name
      validator: ComparePassword("password", "confirmPassword")
    });
  }

  ngOnInit(): void {
  }

  submitNewUser() : void { 
    var u = { 
      emailAddress: this.formNewUser.get("email")?.value, 
      firstName: this.formNewUser.get("firstName")?.value, 
      lastName: this.formNewUser.get("lastName")?.value, 
      userName: this.formNewUser.get("userName")?.value,
      password: this.formNewUser.get("password")?.value
    } as newUser

    console.log(u); 
    this.login.createUser(u).subscribe(result => { 
      this.route.navigate(['/signin']); 
    })
  }
}



