import { HttpStatusCode } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { NgForm } from '@angular/forms';
import { Router } from '@angular/router';
import { UserService } from 'src/app/shared/user.service';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit {

  formModel = {
    Username:'',
    Password:''
  }

  constructor(public service: UserService, private router: Router) { }

  ngOnInit(): void {
    if (localStorage.getItem('token') != null)
      this.router.navigateByUrl('/home');
  }

  onSubmit(form: NgForm){
    this.service.login(form.value).subscribe({
      next: (res:any) => {
          localStorage.setItem('token', res.token);
          this.router.navigateByUrl('/home');
      },
      error: err=>{
        if(err.status == 401){
          alert("You are not registered user!");
          form.reset();
        }
      }
    });
  }

}
