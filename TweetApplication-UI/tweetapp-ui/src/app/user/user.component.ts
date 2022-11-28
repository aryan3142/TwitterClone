import { Component, OnInit } from '@angular/core';

import { environment } from 'src/environments/environment';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { UserService } from '../shared/user.service';
import { ActivatedRoute } from '@angular/router';

@Component({
  selector: 'app-user',
  templateUrl: './user.component.html',
  styleUrls: ['./user.component.css']
})
export class UserComponent implements OnInit {

  constructor(private userService: UserService, public route: ActivatedRoute) { }

  users: any=[];

  ngOnInit(): void {
    this.refreshUsersList();
  }

  refreshUsersList(){
    /*var tokenHeader = new HttpHeaders({Authorization: 'Bearer ' + localStorage.getItem('token')});
    return this.http.get(environment.API_URL + '/tweets/users/all', {headers: tokenHeader}).subscribe(data => {
      this.users = data;
    });*/
    this.userService.getAllUsers().subscribe({
      next: (res:any) =>{
      this.users = res;
    },
      error: (err:any) => {
        console.log(err);
      }
    });
  }

}
