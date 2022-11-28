import { Component, OnInit } from '@angular/core';
import { NgForm } from '@angular/forms';
import { Router } from '@angular/router';
import { UserService } from 'src/app/shared/user.service';

@Component({
  selector: 'app-reset-password',
  templateUrl: './reset-password.component.html',
  styleUrls: ['./reset-password.component.css']
})
export class ResetPasswordComponent implements OnInit {

  formModel = {
    OldPassword : '',
    NewPassword : ''
  }

  userDetails:any;

  constructor(public userService: UserService, private router: Router) { }

  ngOnInit(): void {
    this.userService.getUserProfile().subscribe({
      next: (res:any) => {
        this.userDetails = res;
      },
      error: (err:any) => {
        console.log(err);
      },
    });
  }

  onSubmit(form: NgForm){
    this.userService.resetPassword(this.userDetails.emailId, form.value).subscribe({
      next: (res:any) => {
          alert(res);
          this.router.navigateByUrl('/home');
      },
      error: err=>{
        if(err.status == 400){
          alert(err.error);
          form.reset();
        }
      }
    });
  }

}
