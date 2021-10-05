import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { AppComponent } from '../app.component';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
})
export class HomeComponent {
  constructor(public app: AppComponent, private router: Router) {
    console.log(`Home Componet UserSignedIn ${this.app.userSignedIn}`); 
    if(app.userSignedIn) { 
      this.router.navigate(['/hrv']);
    }
  }
}
