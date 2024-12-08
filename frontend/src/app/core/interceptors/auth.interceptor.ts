import { HttpHandlerFn, HttpRequest } from '@angular/common/http';
import { inject } from '@angular/core';
import { LoginService } from '../../features/login/data/login.service';
import { environment } from '../../../environments/environment';

export function authInterceptor(
    req: HttpRequest<unknown>,
    next: HttpHandlerFn
) {
    // Obtain the token from the LoginService
    const authToken = inject(LoginService).getToken();

    //clone the request and add the Authorization header
    const newReq = req.clone({
        headers: req.headers.append('Authorization', `Bearer ${authToken}`),
    });
    return next(newReq);
}
