import {
  HttpEvent,
  HttpInterceptor,
  HttpHandler,
  HttpRequest
} from "@angular/common/http";
import { HTTP_INTERCEPTORS } from "@angular/common/http";
import {Injectable, NgModule} from '@angular/core';
import {Observable} from 'rxjs';

@Injectable()
export class HttpRequestInterceptor implements HttpInterceptor {
  // THIS MODULE IS FOR SERVICES with HTTP REQUEST
  intercept(
    req: HttpRequest<any>,
    next: HttpHandler
  ): Observable<HttpEvent<any>> {
    const token = localStorage.getItem("bearerToken");

    if (token) {
      const newReq = req.clone({
        headers: req.headers.set("Authorization", "Bearer " + token)
      });
      console.log("intercepted http request");
      return next.handle(newReq);
    } else {
      console.log("not intercepted http request");
      return next.handle(req);
    }
  }
}

@NgModule({
  providers: [
    {
      provide: HTTP_INTERCEPTORS,
      useClass: HttpRequestInterceptor,
      multi: true
    }
  ]
})
export class HttpInterceptorModule {}
