import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { Token } from 'src/model/models';
import { TokenService } from 'src/service/token-service';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss'],
})
export class LoginComponent {
  username = '';
  password = '';

  constructor(private tokenService: TokenService, private router: Router) {
    console.log('LoginComponent created!');
  }
  onSubmit() {
    this.tokenService.login(this.username, this.password).subscribe(
      result => {
        const token = result;
        console.log('Login successful!', result);
        localStorage.setItem('token', token.token);
        this.router.navigate(['/user-list']);
      },
      (error) => {
        console.log('Login failed:', error);
      }
    );
  }
}
