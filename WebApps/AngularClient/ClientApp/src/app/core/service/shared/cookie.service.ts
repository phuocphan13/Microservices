import { Injectable } from "@angular/core";

@Injectable()
export class CookiesService {
  cookiesArray: string[] = [];

  constructor() {
    this.cookiesArray = document.cookie.split(';');
  }

  getCookie(name: string): any {
    for (let i = 0; i < this.cookiesArray.length; i++) {
      let cookie = this.cookiesArray[i].split('=');
      if (cookie[0].trim() === name) {
        return {
          key: cookie[0].trim(),
          value: cookie[1].trim()
        };
      }
    }

    return null;
  }

  setCookie(name: string, value: string, expireMinutes: number) {
    let date = new Date();
    date.setTime(date.getTime() + (expireMinutes * 60 * 1000));

    let expires = "expires=" + date.toUTCString();

    document.cookie += name + "=" + value + ";" + expires + ";path=/";
  }
}
