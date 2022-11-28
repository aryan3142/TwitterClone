import { Component, OnInit } from '@angular/core';
import { NgForm } from '@angular/forms';
import { Router } from '@angular/router';
import { TweetService } from 'src/app/shared/tweet.service';
import { UserService } from 'src/app/shared/user.service';

@Component({
  selector: 'app-my-tweets',
  templateUrl: './my-tweets.component.html',
  styleUrls: ['./my-tweets.component.css']
})
export class MyTweetsComponent implements OnInit {

  loggedUserDetails: any = {};
  allUserTweets: any = [];
  show: boolean = true;
  showForm = false;

  constructor(private router: Router,public service: UserService, public tweetService: TweetService) { }

  ngOnInit(): void {
    this.service.getUserProfile().subscribe({
      next: (res:any) => {
        this.loggedUserDetails = res;
        this.getAllUserTweets(this.loggedUserDetails.emailId);
      },
      error: (err:any) => {
        console.log(err);
      }
    });    
  }

  getAllUserTweets(email: string){
    this.tweetService.getUserTweets(email).subscribe({
      next: (res: any) => {
        this.allUserTweets = res;
      },
      error: (err: any) => {
        console.log(err);
      }
    });
  }

  onLike(id : any, username: string){
    //this.show = true;
    this.tweetService.likeOrDisLikeTweet(id, username).subscribe({
      next: (res:any) => {
        window.location.reload();
      },
      error: (err:any) => {
        console.log(err);
      }
    });
  }

  onReply(id:any, username:string, message: NgForm){
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

  onDelete(id: string, username: string){
    this.tweetService.deleteTweet(id, username).subscribe({
      next: (res: any) => {
        alert(res);
        window.location.reload();
      },
      error: (err: any) => {
        alert(err.error);
        console.log(err);
      }
    });
  }

  onEdit(id: string, username: string, tweetText: string){
    this.router.navigate(["tweet/edit-tweet", { "id" : id, "username": username, "tweetText": tweetText}]);
  }

}
