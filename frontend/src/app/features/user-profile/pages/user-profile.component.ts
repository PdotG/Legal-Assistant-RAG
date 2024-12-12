import { CommonModule } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { Router } from '@angular/router';
import { UserService } from '../data/profile.service';
import { Profile } from '../data/profile';
import { DialogService } from '../../../shared/ui/dialog/data/dialog.service';

@Component({
  selector: 'app-user-profile',
  imports: [FormsModule, CommonModule],
  templateUrl: './user-profile.component.html',
  styleUrls: ['./user-profile.component.css']
})
export class UserProfileComponent implements OnInit {
  user: Profile = {
    name: '',
    email: '',
    password: ''
  };
  newPassword: string = '';
  showPassword: boolean = false;

  constructor(private userService: UserService, private router: Router, private dialogService: DialogService) {}

  ngOnInit(): void {
    this.userService.getUserProfile().subscribe((data: Profile) => {
      this.user = data;
    });
  }

  async onSubmit(form: any): Promise<void> {
    if (form.valid) {
      const updatedUser = { ...this.user };
      if (this.newPassword) {
        updatedUser.password = this.newPassword;
      }
      this.userService.updateUserProfile(updatedUser).subscribe(async response => {
        await this.dialogService.showInfo('Profile updated successfully');
        this.router.navigate(['/profile']);
      }, async error => {
        await this.dialogService.showError('Error updating profile ' + error);
      });
    }
  }
}