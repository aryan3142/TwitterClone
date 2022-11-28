import { Component, OnInit } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { TweetService } from 'src/app/shared/tweet.service';

@Component({
  selector: 'app-edit-tweet',
  templateUrl: './edit-tweet.component.html',
  styleUrls: ['./edit-tweet.component.css']
})
export class EditTweetComponent implements OnInit {

  formModel = this.fb.group({
    Message: ['', Validators.required],
  });

  tweetId : string = "";
  username: string = "";
  tweetText: string = "";

  constructor(private fb: FormBuilder, private tweetService: TweetService,public route: ActivatedRoute, public router: Router) { }

  ngOnInit(): void {
    this.route.params.subscribe( params => {
      this.tweetId = params["id"];
      this.username = params['username'];
      this.tweetText = params['tweetText'];
    });
  }

  onEdit(){
      this.tweetService.editTweet(this.tweetId, this.username, this.formModel.value.Message).subscribe({
        next: (res: any) => {
          alert("Updated successfully!");
          this.router.navigateByUrl("/tweet/my-tweets");
        },
        error: (err: any) => {
          console.log(err);
        }
      });
    
  }

}
