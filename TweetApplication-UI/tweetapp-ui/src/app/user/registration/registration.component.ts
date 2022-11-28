import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { UserService } from 'src/app/shared/user.service';

@Component({
  selector: 'app-registration',
  templateUrl: './registration.component.html',
  styleUrls: ['./registration.component.css']
})
export class RegistrationComponent implements OnInit {
  imageUrl: string =''; 

  constructor(public service: UserService, private router: Router) { }

  ngOnInit(): void {
    //this.imageUrl = './assets/UserLogo.jfif';
    
    if (localStorage.getItem('token') != null)
      this.router.navigateByUrl('/home');
    this.service.formModel.reset();
  }

  onSubmit(){
    this.service.register().subscribe({
      next: res =>{
        alert(res.toString());
        this.service.formModel.reset();
      },
      error: err => {
        console.log(err);
      }
    });
  }

}
