import { Component, OnInit } from '@angular/core';

import { environment } from 'src/environments/environment';
import { HttpClient } from '@angular/common/http';
import { TweetService } from '../shared/tweet.service';
import { UserService } from '../shared/user.service';

@Component({
  selector: 'app-tweet',
  templateUrl: './tweet.component.html',
  styleUrls: ['./tweet.component.css']
})
export class TweetComponent implements OnInit {
  
  constructor(private http:HttpClient) { }

  tweets: any = [];
  users: any=[];

  ngOnInit(): void {
    this.refreshList();
  }


  refreshList(){
    this.http.get<any>(environment.API_URL + '/all').subscribe(data=>{
      this.tweets = data;
    });

    this.http.get<any>(environment.API_URL + '/users/all').subscribe(data=>{
      this.users = data;
    });
    
  }

}
