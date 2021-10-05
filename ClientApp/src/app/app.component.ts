import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { LoginService } from './Services/login.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html'
})
export class AppComponent implements OnInit {
  title = 'HRV';
  public userSignedIn: boolean = false;
  public loading: boolean = false; 
  public userId: string; 
  constructor(private login: LoginService, private router: Router) {

  }

  ngOnInit(): void {
    this.loading = true;
    var jwt = localStorage.getItem("HRV_TOKEN");
    if (jwt) {
      this.login.checkUser().subscribe(result => {
        if (!result.tokenValiden) {
          this.clearLocalStorage();
          this.userSignedIn = false;
        }
        else {
          this.userSignedIn = true;
          this.userId = localStorage.getItem("USER_ID");
          var lastRefresh = Date.parse(localStorage.getItem("LAST_REFRESH"));
          var currentDate = new Date().valueOf()
          var dif = currentDate - lastRefresh;
          var bolRefresh = (14400000 < dif); 

          console.log(`${lastRefresh} - ${currentDate} = ${dif}   [${bolRefresh}]`);
          if(bolRefresh) { 
            this.login.refreshToken().subscribe(result => { 
              localStorage.setItem("HRV_TOKEN", result.token); 
              localStorage.setItem("HRV_TOKEN_EXP", result.expiration.toString());
              localStorage.setItem("USER_ID", result.userName);
              localStorage.setItem("LAST_REFRESH", (new Date()).toString()); 
              console.log("Token Refreshed");
            }, error => { 
              console.error(error); 
            });
          }

        }

        this.loading = false;
      }, error => { 
        this.clearLocalStorage(); 
        this.loading = false;
      })
    }
    else {
      console.log("No JWT");
      this.clearLocalStorage();
      this.loading = false;
      this.userSignedIn = false; 
    }

  }

  private clearLocalStorage() {
    localStorage.removeItem("HRV_TOKEN");
    localStorage.removeItem("HRV_TOKEN_EXP");
    localStorage.removeItem("USER_ID");
    localStorage.removeItem("LAST_REFRESH");
  }

  public checkToken(): void {
    var jwt = localStorage.getItem("HRV_TOKEN");
    if (jwt) {
      console.log("Checking Token");
      this.userSignedIn = false;
      this.userId = localStorage.getItem("USER_ID");
      var bolLoggedIn = false;

      this.login.checkUser().subscribe(result => {
        console.log(result);
        if (!result.tokenValiden) {
          this.clearLocalStorage();
          this.userSignedIn = false;
          this.userId = "";
          bolLoggedIn = false;
        }
        else {
          this.userSignedIn = true;
          this.userId = localStorage.getItem("USER_ID");
        }
      }, error => { console.error(error) });

    }
    else {
      this.userSignedIn = false;
      this.userId = "";

    }
  }

  public logOff(): void {
    console.log("logging off...")
    this.clearLocalStorage();
    this.userSignedIn = false;
    this.router.navigate(["/home"]);
    
  }

  public logIn(): void { 
    this.clearLocalStorage();
    this.router.navigate(['/signin']);
  }

  public signUp() : void { 
    this.clearLocalStorage(); 
    this.router.navigate(["/newUser"]);
  }

  public passwordChange() : void { 
    this.router.navigate(['/passwordChange']);
  }
}
