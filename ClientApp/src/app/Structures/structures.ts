import { FormGroup } from "@angular/forms";

export class LoginModel {
    userName!: string; 
    password!: string;
}

export class Token { 
    token!: string; 
    expiration! : Date;
    userName!: string;
}

export class newUser { 
    emailAddress!: string;
    password!: string; 
    firstName!: string;
    lastName!: string;
    userName!: string; 
}

export class hrvRecord { 
    date!: string; 
    hrv!: number;
}

export class hrvOutput extends hrvRecord { 
    id!: number;
}

export class hrvList extends hrvOutput { 
    range: string; 
}

export class passwordChange { 
  oldPassword: string; 
  newPassword: string;
}

export function ComparePassword(controlName: string,  matchingControlName: string) {
    return (formGroup: FormGroup) => {
      const control = formGroup.controls[controlName];
      const matchingControl = formGroup.controls[matchingControlName];
  
      if (matchingControl.errors && !matchingControl.errors.mustMatch) {
        return;
      }
  
      if (control.value !== matchingControl.value) {
        matchingControl.setErrors({ mustMatch: true });
      } else {
        matchingControl.setErrors(null);
      }
    };
}