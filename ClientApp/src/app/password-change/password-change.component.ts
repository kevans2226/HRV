import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { LoginService } from '../Services/login.service';
import { ComparePassword, passwordChange } from '../Structures/structures';

@Component({
  selector: 'app-password-change',
  templateUrl: './password-change.component.html',
  styleUrls: ['./password-change.component.css']
})
export class PasswordChangeComponent implements OnInit {

  public passwordChange: FormGroup; 
  public showForm: boolean = true; 
  public showSuccessMessage: boolean = false; 
  public showErrorMessage: boolean = false; 
  public errorMessage: string; 
  constructor(private formBuilder: FormBuilder, private loginService: LoginService) {
    this.passwordChange = this.formBuilder.group({ 
      oldPassword: ['', [Validators.required]],
      newPassword: ['', [Validators.required]], 
      confirmPassword: ['', [Validators.required]]
    },
    {
      validator: ComparePassword("newPassword", "confirmPassword")
    });


   }

  ngOnInit(): void {
  }

  public changePassword() : void { 
    var pwdChange = { 
      newPassword: this.passwordChange.get("newPassword").value, 
      oldPassword: this.passwordChange.get("oldPassword").value
    } as passwordChange; 

    this.loginService.passwordChange(pwdChange).subscribe(result => { 
      console.log("Accepted"); 
      this.showForm = false; 
      this.showSuccessMessage = true; 
    }, error => { 
      console.log(error); 
      this.showForm = true; 
      this.showErrorMessage = true; 
      this.showSuccessMessage = false; 
      this.errorMessage = error.error; 
    })
  }
}
