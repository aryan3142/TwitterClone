import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { TweetService } from 'src/app/shared/tweet.service';
import { UserService } from 'src/app/shared/user.service';

@Component({
  selector: 'app-post-tweet',
  templateUrl: './post-tweet.component.html',
  styleUrls: ['./post-tweet.component.css']
})
export class PostTweetComponent implements OnInit {

  userDetail:any;

  constructor(public userService: UserService, public tweetService: TweetService,private router: Router) { }

  ngOnInit(): void {
    this.tweetService.formModel.reset();
    this.userService.getUserProfile().subscribe({
      next: (res:any) => {
        this.userDetail = res;
      },
      error: (err:any) => {
        console.log(err);
      },
    });
  }

  onSubmit(){
    this.tweetService.postNewTweet(this.userDetail.emailId).subscribe({
      next: (res:any) => {
        alert("Posted New Tweet")
        this.router.navigateByUrl('/home');
      },
      error: (err:any) =>{
        alert("Please validate tweet message before posting!");
        this.tweetService.formModel.reset();
        console.log(err);
      }
    });
  }

}
