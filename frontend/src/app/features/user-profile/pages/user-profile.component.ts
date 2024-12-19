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
  currentPassword: string = '';
  showCurrentPassword: boolean = false;

  constructor(private userService: UserService, private router: Router, private dialogService: DialogService) {}

  ngOnInit(): void {
    this.userService.getUserProfile().subscribe((data: Profile) => {
      this.user = data;
    });
  }

  async onSubmit(form: any): Promise<void> {
    if (form.valid) {
      this.userService.verifyPassword(this.user.email, this.currentPassword).subscribe(
        async (response) => {
          const updatedUser = { ...this.user };
          if (this.newPassword) {
            updatedUser.password = this.newPassword;
          } else {
            updatedUser.password = this.currentPassword;
          }
          this.userService.updateUserProfile(updatedUser).subscribe(async response => {
            await this.dialogService.showInfo('Profile updated successfully');
            this.router.navigate(['/profile']);
          }, async error => {
            const errorMessage = error.error?.message || 'Error updating profile';
            await this.dialogService.showError(errorMessage);
          });
        },
        async (error) => {
          const errorMessage = error.error?.message || 'Current password is incorrect';
          await this.dialogService.showError(errorMessage);
        }
      );
    }
  }
}