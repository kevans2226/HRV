import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { HomeComponent } from './home/home.component';
import { HRVComponent } from './hrv/hrv.component';

import { NewUserComponent } from './new-user/new-user.component';
import { PasswordChangeComponent } from './password-change/password-change.component';
import { SigninComponent } from './signin/signin.component';


const routes: Routes = [
  { path: 'home', component: HomeComponent },
  { path: 'newUser', component: NewUserComponent },
  { path: 'signin', component: SigninComponent},
  { path: 'hrv', component: HRVComponent},
  { path: 'hrv/:id', component: HRVComponent},
  { path: 'passwordChange', component: PasswordChangeComponent},
  { path: '', redirectTo: '/home', pathMatch: 'full' },
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
