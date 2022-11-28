import { Component, OnInit } from '@angular/core';
import { Form, NgForm } from '@angular/forms';
import { Router } from '@angular/router';
import { TweetService } from '../shared/tweet.service';
import { UserService } from '../shared/user.service';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css']
})
export class HomeComponent implements OnInit {
  userDetails:any = {};
  searchTerm: string = "";
  searchedTerm: any;
  allTweets: any = [];
  show: boolean = true;
  showForm = false;

  constructor(private router: Router, public service: UserService, private tweetService: TweetService) { }

  ngOnInit(): void {
    if(localStorage.getItem('token') == null)
      this.router.navigateByUrl('user/login');
      this.service.getUserProfile().subscribe({
      next: (res:any) => {
        this.userDetails = res;
      },
      error:(err:any) => {
        console.log(err);
      },
    });
    
    this.getAllTweets();
  }

  searchByUsername(username: string){
    this.show = !this.show;
    this.service.searchByUsername(username).subscribe(
      (res:any) => {
        this.searchedTerm = res;
      },
      (err: any) => {
        console.log(err);
      }
    );
  }

  getAllTweets(){
    this.tweetService.getAllTweets().subscribe({
      next: (res: any) => {
        this.allTweets = res;
      },
      error: (err: any) => {
        console.log(err);
      }
    });
  }

  onLike(id : any, username: string){
    this.tweetService.likeOrDisLikeTweet(id, username).subscribe({
      next: (res:any) => {
        window.location.reload();
        this.router.navigateByUrl("/home");
      },
      error: (err:any) => {
        console.log(err);
      }
    });
    
  }

  onReply(id:any, username:string, message: NgForm){
    console.log(message.value);
    this.tweetService.reply(id, username, message.value).subscribe({
      next: (res: any) => {
        window.location.reload();
      },
      error: (err:any) => {
        console.log(err);
      }
    });
  }

  onClickForm(){
    this.showForm = true;
  }  

  onLogout() {
    localStorage.removeItem('token');
    this.router.navigate(['/user/login']);
  }

}
