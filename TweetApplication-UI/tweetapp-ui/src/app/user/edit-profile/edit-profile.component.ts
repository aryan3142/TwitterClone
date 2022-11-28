import { Component, OnInit } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { UserService } from 'src/app/shared/user.service';

@Component({
  selector: 'app-edit-profile',
  templateUrl: './edit-profile.component.html',
  styleUrls: ['./edit-profile.component.css']
})
export class EditProfileComponent implements OnInit {

  constructor(private fb: FormBuilder, public service: UserService) { }

  formModel = this.fb.group({
    FirstName: ['', Validators.required],
    LastName: ['', Validators.required],
    Gender: ['', Validators.required],
    DateOfBirth: ['', Validators.required],
    EmailId: ['', [Validators.required, Validators.email]],
  });

  ngOnInit(): void {
  }

  onSubmit(){
    this.service.editProfile(this.formModel).subscribe({
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
