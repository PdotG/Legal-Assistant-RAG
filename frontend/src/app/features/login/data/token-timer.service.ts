import { Injectable, Injector } from '@angular/core';
import { LoginService } from './login.service';
import { DialogService } from '../../../shared/ui/dialog/data/dialog.service';

@Injectable({
  providedIn: 'root',
})
export class TokenTimerService {
  private logoutTimer: any;
  private loginService!: LoginService;

  constructor(
    private injector: Injector,
    private dialogService: DialogService
  ) {}

  private getLoginService(): LoginService {
    if (!this.loginService) {
      this.loginService = this.injector.get(LoginService);
    }
    return this.loginService;
  }

  startLogoutTimer(expirationTime: number): void {
    if (this.logoutTimer) {
      clearTimeout(this.logoutTimer);
    }

    this.logoutTimer = setTimeout(async () => {
      this.getLoginService().logout();
      await this.dialogService.showInfo(
        'For security reasons, your session has been closed.'
      );
    }, expirationTime);
  }

  clearLogoutTimer(): void {
    if (this.logoutTimer) {
      clearTimeout(this.logoutTimer);
      this.logoutTimer = null;
    }
  }
}
