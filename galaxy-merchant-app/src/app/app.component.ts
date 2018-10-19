import { environment } from './../environments/environment';
import { CookieService } from 'ngx-cookie-service';
import { Component } from '@angular/core';
import { HttpClient, HttpResponse, HttpHeaders } from '@angular/common/http';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent {
  title = 'Galaxy Merchant';
  input_commands = [];
  input_command = '';
  session = '';
  answers = [];
  constructor(
    private httpClient: HttpClient,
    private cookie: CookieService
  ) {}

  add() {
    const head = {
      headers: new HttpHeaders({
        'Content-Type': 'application/json; application/x-www-form-urlencoded',
        'Access-Control-Allow-Credentials': 'true'
      }),
      observe: 'response' as 'response',
      reportProgress: false,
      withCredentials: true
    };
    if (this.session !== '') {
      head.headers.append('Set-Cookie', this.session);
    }
    this.httpClient.post<any>(
        environment.web_path,
        JSON.stringify({ text: this.input_command }),
        head
      ).subscribe(
        (response: HttpResponse<any>) => {
          if (this.cookie.check('.AspNetCore.Session')) {
            this.session = this.cookie.get('.AspNetCore.Session');
          }

          if (response.body === '') { return; }
          this.answers.push(response.body);
        },
        error => { this.answers.push(error.error.text); }
      );
    this.input_commands.push(this.input_command);
  }
}
