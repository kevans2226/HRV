import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { LoginModel, newUser, passwordChange, Token } from '../Structures/structures';

@Injectable({
  providedIn: 'root'
})
export class LoginService {

  constructor(private http: HttpClient) { }

  public login(login: LoginModel) : Observable<Token> { 
    return this.http.post<Token>("/api/Authenticate/login", login); 
  }

  public createUser(user: newUser) : Observable<any> { 
    return this.http.post<any>('/api/Authenticate/createUser', user);
  }

  public checkUser() : Observable<any> { 
    return this.http.get<any>('/api/Authenticate/check');
  }

  public refreshToken(): Observable<Token> {
    return this.http.get<Token>("/api/Authenticate/refresh");
  }

  public passwordChange(pwdChange: passwordChange): Observable<any> { 
    return this.http.put<any>("/api/Authenticate/passwordChange", pwdChange); 
  }
}
